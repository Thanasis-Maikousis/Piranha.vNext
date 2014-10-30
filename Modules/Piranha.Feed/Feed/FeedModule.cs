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
using System.Linq;
using Piranha.Extend;
using Piranha.Web.Models;
using System.Web;
using Piranha.Web;

namespace Piranha.Feed
{
	/// <summary>
	/// Main entry point for the feed module.
	/// </summary>
	public class FeedModule : IModule
	{
		#region Members
		private const string LINK_TAG = "<link rel=\"{0}\" type=\"{1}\" title=\"{2}\" href=\"{3}\">\n";
		#endregion

		/// <summary>
		/// Initializes the module. This method should be used for
		/// ensuring runtime resources and registering hooks.
		/// </summary>
		public void Init() {
			using (var api = new Api()) {
				// Ensure configuration params
				var param = api.Params.GetSingle(where: p => p.Name == "feed_pagesize");
				if (param == null) {
					param = new Models.Param() { 
						Name = "feed_pagesize",
						Value = "10"
					};
					api.Params.Add(param);
				}
				param = api.Params.GetSingle(where: p => p.Name == "feed_sitefeedtitle");
				if (param == null) {
					param = new Models.Param() { 
						Name = "feed_sitefeedtitle",
						Value = "{SiteTitle} > Feed"
					};
					api.Params.Add(param);
				}
				param = api.Params.GetSingle(where: p => p.Name == "feed_archivefeedtitle");
				if (param == null) {
					param = new Models.Param() { 
						Name = "feed_archivefeedtitle",
						Value = "{SiteTitle} > {PostType} Archive Feed"
					};
					api.Params.Add(param);
				}
				param = api.Params.GetSingle(where: p => p.Name == "feed_commentfeedtitle");
				if (param == null) {
					param = new Models.Param() { 
						Name = "feed_commentfeedtitle",
						Value = "{SiteTitle} > Comments Feed"
					};
					api.Params.Add(param);
				}
				param = api.Params.GetSingle(where: p => p.Name == "feed_postfeedtitle");
				if (param == null) {
					param = new Models.Param() { 
						Name = "feed_postfeedtitle",
						Value = "{SiteTitle} > {PostTitle} Comments Feed"
					};
					api.Params.Add(param);
				}

				// Save changes
				api.SaveChanges();
			}

			// Add the feed handler
			App.Handlers.Add("feed", new FeedHandler());

			// Add UI rendering
			Hooks.UI.Head.Render += (sb) => {
				// Get current
				var current = HttpContext.Current.GetCurrent();

				// Render base feeds
				var sTitle = HttpUtility.HtmlEncode(Config.Feed.SiteFeedTitle
					.Replace("{SiteTitle}", Config.Site.Title));

				var cTitle = HttpUtility.HtmlEncode(Config.Feed.CommentFeedTitle
					.Replace("{SiteTitle}", Config.Site.Title));

				sb.Append(String.Format(LINK_TAG, "alternate", "application/rss+xml", sTitle,
					Utils.AbsoluteUrl("~/feed")));
				sb.Append(String.Format(LINK_TAG, "alternate", "application/rss+xml", cTitle,
					Utils.AbsoluteUrl("~/feed/comments")));

				if (current.Type == ContentType.Archive) {
					using (var api = new Api()) {
						var type = api.PostTypes.GetById(current.Id);

						var title = HttpUtility.HtmlEncode(Config.Feed.ArchiveFeedTitle
							.Replace("{SiteTitle}", Config.Site.Title)
							.Replace("{PostType}", type.ArchiveTitle));

						sb.Append(String.Format(LINK_TAG, "alternate", "application/rss+xml", title,
							Utils.AbsoluteUrl("~/feed/blog")));
					}
				} else if (current.Type == ContentType.Post) {
					var post = Web.Models.PostModel.GetById(current.Id);

					var title = HttpUtility.HtmlEncode(Config.Feed.PostFeedTitle
						.Replace("{SiteTitle}", Config.Site.Title)
						.Replace("{PostTitle}", post.Title));

					sb.Append(String.Format(LINK_TAG, "alternate", "application/rss+xml", title,
						Utils.AbsoluteUrl("~/feed/" + post.Type + "/" + post.Slug)));
				}
			};
		}
	}
}