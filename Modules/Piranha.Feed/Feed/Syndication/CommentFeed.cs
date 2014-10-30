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

namespace Piranha.Feed.Syndication
{
	/// <summary>
	/// Abstract class for creating a comment feed.
	/// </summary>
	public abstract class CommentFeed
	{
		#region Members
		/// <summary>
		/// The protected comment collection
		/// </summary>
		protected readonly IEnumerable<Piranha.Models.Comment> Comments;
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
		/// <param name="comments">The current comments</param>
		public CommentFeed(IEnumerable<Piranha.Models.Comment> comments) : base() {
			Comments = comments;
		}

		/// <summary>
		/// Executes the syndication result on the given context.
		/// </summary>
		/// <param name="context">The current context.</param>
		public virtual void Write(HttpResponse response) {
			var writer = new XmlTextWriter(response.OutputStream, Encoding.UTF8);
			var ui = new Web.Helpers.UIHelper();

			// Write headers
			response.StatusCode = 200;
			response.ContentType = ContentType;
			response.ContentEncoding = Encoding.UTF8;

			var feed = new SyndicationFeed() { 
				Title = new TextSyndicationContent(Config.Site.Title),
				LastUpdatedTime = Comments.First().Created,
				Description = new TextSyndicationContent(Config.Site.Description),
			};
			feed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(Utils.AbsoluteUrl("~/"))));

			var items = new List<SyndicationItem>();
			foreach (var comment in Comments) {
				var item = new SyndicationItem() { 
					Title = SyndicationContent.CreatePlaintextContent(comment.Author),
					PublishDate = comment.Created,
					Summary = SyndicationContent.CreateHtmlContent(comment.Body)
				};
				//item.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(Utils.AbsoluteUrl(ui.Permalink(post).ToHtmlString()))));
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
