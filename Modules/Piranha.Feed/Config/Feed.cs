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

namespace Piranha.Config
{
	/// <summary>
	/// RSS Feed configuration.
	/// </summary>
	public static class Feed
	{
		/// <summary>
		/// Gets/sets the number of items that should be included in a feed.
		/// </summary>
		public static int PageSize {
			get { return Utils.GetParam<int>("feed_pagesize", s => Convert.ToInt32(s)); }
			set { Utils.SetParam("feed_pagesize", value); }
		}

		/// <summary>
		/// Gets/sets the site feed title format.
		/// </summary>
		public static string SiteFeedTitle {
			get { return Utils.GetParam<string>("feed_sitefeedtitle", s => s); }
			set { Utils.SetParam("feed_sitefeedtitle", value); }
		}

		/// <summary>
		/// Gets/sets the archive feed title format.
		/// </summary>
		public static string ArchiveFeedTitle {
			get { return Utils.GetParam<string>("feed_archivefeedtitle", s => s); }
			set { Utils.SetParam("feed_archivefeedtitle", value); }
		}

		/// <summary>
		/// Gets/sets the comment feed title format.
		/// </summary>
		public static string CommentFeedTitle {
			get { return Utils.GetParam<string>("feed_commentfeedtitle", s => s); }
			set { Utils.SetParam("feed_commentfeedtitle", value); }
		}

		/// <summary>
		/// Gets/sets the post comment feed title format.
		/// </summary>
		public static string PostFeedTitle {
			get { return Utils.GetParam<string>("feed_postfeedtitle", s => s); }
			set { Utils.SetParam("feed_postfeedtitle", value); }
		}
	}
}
