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

namespace Piranha.Config
{
	/// <summary>
	/// Main site configuration.
	/// </summary>
	public static class Site
	{
		/// <summary>
		/// Gets/sets the site title.
		/// </summary>
		public static string Title {
			get { return Utils.GetParam<string>("site_title", s => s); }
			set { Utils.SetParam("site_title", value); }
		}

		/// <summary>
		/// Gets/sets the site description.
		/// </summary>
		public static string Description {
			get { return Utils.GetParam<string>("site_description", s => s); }
			set { Utils.SetParam("site_description", value); }
		}

		/// <summary>
		/// Gets/sets the global last modification date.
		/// </summary>
		public static DateTime LastModified {
			get { return Utils.GetParam<DateTime>("site_lastmodified", s => DateTime.Parse(s)); }
			set { Utils.SetParam("site_lastmodified", value.ToString("yyyy-MM-dd HH:mm:ss")); }
		}

		/// <summary>
		/// Gets/sets the number of posts that should be displayed on
		/// an archive page.
		/// </summary>
		public static int ArchivePageSize {
			get { return Utils.GetParam<int>("archive_pagesize", s => Convert.ToInt32(s)); }
			set { Utils.SetParam("archive_pagesize", value); }
		}
	}
}
