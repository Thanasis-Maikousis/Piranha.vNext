/*
 * Copyright (c) 2014 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * http://github.com/piranhacms/piranha.vnext
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Xml;
using Piranha.Server;

namespace Piranha.Feed.Syndication
{
	/// <summary>
	/// Abstract class for creating a post feed.
	/// </summary>
	public abstract class PostFeed
	{
		#region Members
		/// <summary>
		/// The protected post collection
		/// </summary>
		protected readonly IEnumerable<Piranha.Models.Post> Posts;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the content type of the current feed.
		/// </summary>
		protected abstract string ContentType { get; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="posts">The current posts</param>
		public PostFeed(IEnumerable<Piranha.Models.Post> posts) : base() {
			Posts = posts;
		}

		/// <summary>
		/// Executes the syndication result on the given context.
		/// </summary>
		/// <param name="context">The current context.</param>
		public virtual void Write(IStreamResponse response) {
			var writer = new XmlTextWriter(response.OutputStream, Encoding.UTF8);
			var ui = new Client.Helpers.UIHelper();

			// Write headers
			response.ContentType = ContentType;
			response.ContentEncoding = Encoding.UTF8;

			var feed = new SyndicationFeed() { 
				Title = new TextSyndicationContent(Config.Site.Title),
				LastUpdatedTime = Posts.First().Published.Value,
				Description = new TextSyndicationContent(Config.Site.Description),
			};
			feed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(App.Env.AbsoluteUrl("~/"))));

			var items = new List<SyndicationItem>();
			foreach (var post in Posts) {
				var item = new SyndicationItem() { 
					Title = SyndicationContent.CreatePlaintextContent(post.Title),
					PublishDate = post.Published.Value,
					Summary = SyndicationContent.CreateHtmlContent(post.Body)
				};
				item.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(App.Env.AbsoluteUrl("~/" + post.Type.Slug + "/" + post.Slug))));
				items.Add(item);
			}
			feed.Items = items;

			var formatter = GetFormatter(feed);
			formatter.WriteTo(writer);

			writer.Flush();
			writer.Close();
		}

		#region Abstract methods
		/// <summary>
		/// Gets the current formatter.
		/// </summary>
		/// <param name="feed">The feed</param>
		/// <returns>The formatter</returns>
		protected abstract SyndicationFeedFormatter GetFormatter(SyndicationFeed feed);
		#endregion	
	}
}
