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

namespace Piranha.Cache
{
	/// <summary>
	/// The main entity cache object.
	/// </summary>
	internal class AppCache
	{
		#region Members
		/// <summary>
		/// The private sitemap cache id.
		/// </summary>
		private const string CACHE_SITEMAP = "_piranha_sitemap_";

		/// <summary>
		/// The private cache provider.
		/// </summary>
		private readonly ICache cache;

		/// <summary>
		/// The alias cache.
		/// </summary>
		public readonly ModelCache<Models.Alias> Aliases;

		/// <summary>
		/// The block cache.
		/// </summary>
		public readonly ModelCache<Models.Block> Blocks;

		/// <summary>
		/// The media cache.
		/// </summary>
		public readonly ModelCache<Models.Media> Media;

		/// <summary>
		/// The page cache.
		/// </summary>
		public readonly ModelCache<Web.Models.PageModel> Pages;

		/// <summary>
		/// The param cache.
		/// </summary>
		public readonly ModelCache<Models.Param> Params;

		/// <summary>
		/// The post cache.
		/// </summary>
		public readonly ModelCache<Models.Post> Posts;

		/// <summary>
		/// The post type cache.
		/// </summary>
		public readonly ModelCache<Models.PostType> PostTypes;
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="cache">The current cache provider</param>
		public AppCache(ICache cache) {
			this.cache = cache;

			Aliases = new ModelCache<Models.Alias>(cache, a => a.Id, a => a.OldUrl);
			Blocks = new ModelCache<Models.Block>(cache, b => b.Id, b => b.Slug);
			Media = new ModelCache<Models.Media>(cache, m => m.Id, m => m.Slug);
			Pages = new ModelCache<Web.Models.PageModel>(cache, p => p.Id, p => p.Slug);
			Params = new ModelCache<Models.Param>(cache, p => p.Id, p => p.Name);
			Posts = new ModelCache<Models.Post>(cache, p => p.Id, p => p.Type.Slug + "_" + p.Slug);
			PostTypes = new ModelCache<Models.PostType>(cache, p => p.Id, p => p.Slug);
		}

		/// <summary>
		/// Gets the current sitemap from the cache.
		/// </summary>
		/// <returns>The sitemap</returns>
		public Web.Models.SiteMap GetSiteMap() {
			return cache.Get<Web.Models.SiteMap>(CACHE_SITEMAP);
		}

		/// <summary>
		/// Sets the current sitemap.
		/// </summary>
		/// <param name="sitemap">The sitemap</param>
		public void SetSiteMap(Web.Models.SiteMap sitemap) {
			cache.Set(CACHE_SITEMAP, sitemap);
		}

		/// <summary>
		/// Removes the current sitemap from the cache.
		/// </summary>
		public void RemoveSiteMap() {
			cache.Remove(CACHE_SITEMAP);
		}
	}
}
