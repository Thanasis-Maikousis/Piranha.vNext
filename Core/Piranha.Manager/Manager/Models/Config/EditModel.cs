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

namespace Piranha.Manager.Models.Config
{
	/// <summary>
	/// View model for the config edit view.
	/// </summary>
	public class EditModel
	{
		#region Inner classes
		/// <summary>
		/// The config params available for caching.
		/// </summary>
		public class CacheModel
		{
			public int Expires { get; set; }
			public int MaxAge { get; set; }
		}

		/// <summary>
		/// The config params available for comments.
		/// </summary>
		public class CommentModel
		{
			public bool ModerateAnonymous { get; set; }
			public bool ModerateAuthorized { get; set; }
		}

		/// <summary>
		/// The config params available for the site.
		/// </summary>
		public class SiteModel
		{
			public string Title { get; set; }
			public string Description { get; set; }
			public int ArchivePageSize { get; set; }
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets/sets the cache configuration.
		/// </summary>
		public CacheModel Cache { get; set; }

		/// <summary>
		/// Gets/sets the comment configuration.
		/// </summary>
		public CommentModel Comments { get; set; }

		/// <summary>
		/// Gets/sets the site configuration.
		/// </summary>
		public SiteModel Site { get; set; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public EditModel() {
			Cache = new CacheModel();
			Comments = new CommentModel();
			Site = new SiteModel();
		}

		/// <summary>
		/// Gets the edit model with the current configuration values.
		/// </summary>
		/// <returns>The model</returns>
		public static EditModel Get() {
			var m = new EditModel();

			m.Cache.Expires = Piranha.Config.Cache.Expires;
			m.Cache.MaxAge = Piranha.Config.Cache.MaxAge;

			m.Comments.ModerateAnonymous = Piranha.Config.Comments.ModerateAnonymous;
			m.Comments.ModerateAuthorized = Piranha.Config.Comments.ModerateAuthorized;

			m.Site.Title = Piranha.Config.Site.Title;
			m.Site.Description = Piranha.Config.Site.Description;
			m.Site.ArchivePageSize = Piranha.Config.Site.ArchivePageSize;

			return m;
		}
	}
}