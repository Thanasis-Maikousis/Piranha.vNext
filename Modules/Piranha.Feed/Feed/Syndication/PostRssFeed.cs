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
using System.ServiceModel.Syndication;

namespace Piranha.Feed.Syndication
{
	/// <summary>
	/// Class for creating a RSS post feed.
	/// </summary>
	public sealed class PostRssFeed : PostFeed
	{
		#region Properties
		/// <summary>
		/// Gets the content type of the current feed.
		/// </summary>
		protected override string ContentType {
			get { return "application/rss+xml"; }
		}
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="posts">The current posts</param>
		public PostRssFeed(IEnumerable<Piranha.Models.Post> posts) : base(posts) { }

		/// <summary>
		/// Gets the current formatter.
		/// </summary>
		/// <returns>The formatter</returns>
		protected override SyndicationFeedFormatter GetFormatter(SyndicationFeed feed) {
			return new Rss20FeedFormatter(feed);
		}
	}
}