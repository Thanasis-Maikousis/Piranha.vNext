/*
 * Piranha CMS
 * Copyright (c) 2014, Håkan Edling, All rights reserved.
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Piranha.Web.Helpers
{
	/// <summary>
	/// Helper class for UI methods.
	/// </summary>
	public class UIHelper
	{
		#region Members
		private const string META_TAG = "<meta name=\"{0}\" content=\"{1}\">\n";
		#endregion

		/// <summary>
		/// Gets the block with the given slug.
		/// </summary>
		/// <param name="slug">The unique slug</param>
		/// <returns>The block content</returns>
		public IHtmlString Block(string slug) {
			using (var api = new Api()) {
				var block = api.Blocks.GetSingle(slug);

				if (block != null)
					return new HtmlString(block.Body);
				return new HtmlString("");
			}
		}

		/// <summary>
		/// Renders the head meta data from the currently requested content.
		/// </summary>
		/// <returns>The meta data</returns>
		public IHtmlString Head() {
			var sb = new StringBuilder();
			var current = HttpContext.Current.GetCurrent();

			if (current != null) {
				// Generator
				sb.Append(String.Format(META_TAG, "generator", "Piranha CMS " + Utils.GetFileVersion()));

				// Meta tags
				if (!String.IsNullOrWhiteSpace(current.Keywords))
					sb.Append(String.Format(META_TAG, "keywords", current.Keywords));
				if (!String.IsNullOrWhiteSpace(current.Description))
					sb.Append(String.Format(META_TAG, "description", current.Description));

				// Open graph
				if (!String.IsNullOrWhiteSpace(current.Title))
					sb.Append(String.Format(META_TAG, "og:title", current.Title));
				if (!String.IsNullOrWhiteSpace(current.Description))
					sb.Append(String.Format(META_TAG, "og:description", current.Description));
				if (current.Type == Models.ContentType.Start)
					sb.Append(String.Format(META_TAG, "og:type", "website"));
				else sb.Append(String.Format(META_TAG, "og:type", "article"));
				if (!String.IsNullOrWhiteSpace(current.VirtualPath))
					sb.Append(String.Format(META_TAG, "og:url", Utils.AbsoluteUrl(current.VirtualPath)));

				if (Hooks.UI.Head.Render != null)
					Hooks.UI.Head.Render(sb);
			}
			return new HtmlString(sb.ToString());
		}

		/// <summary>
		/// Renders the permalink for the given post.
		/// </summary>
		/// <param name="post">The post</param>
		/// <returns>The generated permalink</returns>
		public IHtmlString Permalink(Models.PostModel post) {
			return new HtmlString(Utils.Url("~/" + post.Type + "/" + post.Slug));
		}

		/// <summary>
		/// Renders the permalink for the given post.
		/// </summary>
		/// <param name="post">The post</param>
		/// <returns>The generated permalink</returns>
		public IHtmlString Permalink(Piranha.Models.Post post) {
			if (post.Type != null)
				return new HtmlString(Utils.Url("~/" + post.Type.Slug + "/" + post.Slug));
			return Permalink(Models.PostModel.GetById(post.Id));
		}

		/// <summary>
		/// Generates a gravatar url for the given email and size.
		/// </summary>
		/// <param name="email">The email address</param>
		/// <param name="size">The size in pixels</param>
		/// <returns>The gravatar url</returns>
		public IHtmlString GravatarUrl(string email, int size) {
			var md5 = new MD5CryptoServiceProvider();

			var encoder = new UTF8Encoding();
			var hash = new MD5CryptoServiceProvider();
			var bytes = hash.ComputeHash(encoder.GetBytes(email));

			var sb = new StringBuilder(bytes.Length * 2);
			for (int n = 0; n < bytes.Length; n++) {
				sb.Append(bytes[n].ToString("X2"));
			}

			return new HtmlString("http://www.gravatar.com/avatar/" + sb.ToString().ToLower() +
				(size > 0 ? "?s=" + size : ""));
		}

		/// <summary>
		/// Renders the url to the given media file.
		/// </summary>
		/// <param name="media">The media file</param>
		/// <param name="width">Optional width</param>
		/// <param name="height">Optional height</param>
		/// <returns>The url</returns>
		public IHtmlString Media(Piranha.Models.Media media, int? width, int? height) {
			return new HtmlString(Utils.Url("~/media.ashx/" + media.Slug));
		}

		/// <summary>
		/// Renders the thumbnail url to the given media file.
		/// </summary>
		/// <param name="media">The media file</param>
		/// <returns>The url</returns>
		public IHtmlString Thumbnail(Piranha.Models.Media media, int? size) {
			return new HtmlString(Utils.Url("~/media.ashx/" + Utils.FormatMediaSlug(media.Slug, size, size)));
		}

		/// <summary>
		/// Return the site structure as an ul/li list with the current page selected.
		/// </summary>
		/// <param name="start">The start level of the menu</param>
		/// <param name="stop">The stop level of the menu</param>
		/// <param name="levels">The number of levels. Use this if you don't know the start level</param>
		/// <param name="root">Optional rootnode for the menu to start from</param>
		/// <param name="css">Optional css class for the outermost container</param>
		/// <returns>A rendered menu</returns>
		public IHtmlString Menu(int start = 1, int stop = Int32.MaxValue, int levels = 0, string root = "", string css = "menu") {
			StringBuilder str = new StringBuilder();
			IEnumerable<Models.SiteMap.SiteMapItem> sm = null;
			var content = HttpContext.Current.GetCurrent();
			var current = content != null && (content.Type == Models.ContentType.Page || content.Type == Models.ContentType.Start) ? (Guid?)content.Id : null;

			if (current.HasValue || start == 1) {
				if (root != "") {
					var item = Models.SiteMap.GetPartial(root);
					if (item != null)
						sm = item.Items;
				} else {
					sm = Models.SiteMap.Get().GetLevel(current, start);
				}
				if (sm != null) {
					if (stop == Int32.MaxValue && levels > 0 && sm.Count() > 0)
						stop = sm.First().Level + Math.Max(0, levels - 1);
					RenderUL(current, sm, str, stop, css);
				}
			}
			return new HtmlString(str.ToString());
		}

		/// <summary>
		/// Renders an UL list for the given sitemap elements
		/// </summary>
		/// <param name="curr">The current page</param>
		/// <param name="items">The sitemap items</param>
		/// <param name="sb">The string builder</param>
		/// <param name="stop">The desired stop level</param>
		private void RenderUL(Guid? curr, IEnumerable<Models.SiteMap.SiteMapItem> items, StringBuilder sb, int stop, string css = "") {
			if (items != null && items.Count() > 0 && items.First().Level <= stop) {
				// Render level start
				if (Hooks.UI.Menu.RenderLevelStart != null) {
					Hooks.UI.Menu.RenderLevelStart(sb, css);
				} else {
					sb.AppendLine("<ul class=\"" + css + "\">");
				}
				// Render items
				foreach (var item in items)
					if (!item.IsHidden) RenderLI(curr, item, sb, stop);
				// Render level end
				if (Hooks.UI.Menu.RenderLevelEnd != null) {
					Hooks.UI.Menu.RenderLevelEnd(sb, css);
				} else {
					sb.AppendLine("</ul>");
				}
			}
		}

		/// <summary>
		/// Renders an LI element for the given sitemap node.
		/// </summary>
		/// <param name="curr">The current page</param>
		/// <param name="item">The sitemap element</param>
		/// <param name="str">The string builder</param>
		/// <param name="stop">The desired stop level</param>
		private void RenderLI(Guid? curr, Models.SiteMap.SiteMapItem item, StringBuilder sb, int stop) {
			//if (page.GroupId == Guid.Empty || HttpContext.Current.User.IsMember(page.GroupId)) {
			var active = curr.HasValue && curr.Value == item.Id;
			var childactive = curr.HasValue && item.Contains(curr.Value);

			// Render item start
			if (Hooks.UI.Menu.RenderItemStart != null) {
				Hooks.UI.Menu.RenderItemStart(sb, item, active, childactive);
			} else {
				var hasChild = item.Items.Count() > 0 ? " has-child" : "";
				sb.AppendLine("<li" + (active ? " class=\"active" + hasChild + "\"" :
					(childactive ? " class=\"active-child" + hasChild + "\"" :
					(item.Items.Count() > 0 ? " class=\"has-child\"" : ""))) + ">");
			}
			// Render item link
			//if (WebPages.Hooks.Menu.RenderItemLink != null) {
			//	WebPages.Hooks.Menu.RenderItemLink(this, str, page) ;
			//} else {
			sb.AppendLine(String.Format("<a href=\"{0}\">{1}</a>", Utils.Url("~/" + item.Slug),
				!String.IsNullOrEmpty(item.NavigationTitle) ? item.NavigationTitle : item.Title));
			//}
			// Render subpages
			if (item.Items.Count() > 0)
				RenderUL(curr, item.Items, sb, stop);
			// Render item end
			if (Hooks.UI.Menu.RenderItemEnd != null) {
				Hooks.UI.Menu.RenderItemEnd(sb, item, active, childactive);
			} else {
				sb.AppendLine("</li>");
			}
			//}
		}
	}
}
