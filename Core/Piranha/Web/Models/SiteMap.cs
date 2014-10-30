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

namespace Piranha.Web.Models
{
	/// <summary>
	/// The sitemap is used to represent the current site structure
	/// of published pages.
	/// </summary>
	public class SiteMap
	{
		#region Inner classes
		/// <summary>
		/// An item in the sitemap.
		/// </summary>
		public class SiteMapItem
		{
			#region Properties
			/// <summary>
			/// Gets/sets the unique id.
			/// </summary>
			public Guid Id { get; set; }

			/// <summary>
			/// Gets/sets the internal parent id.
			/// </summary>
			internal Guid? ParentId { get; set; }

			/// <summary>
			/// Gets/sets the internal sort order.
			/// </summary>
			internal int SortOrder { get; set; }

			/// <summary>
			/// Gets/sets if the page should be hidden from navigations.
			/// </summary>
			public bool IsHidden { get; set; }

			/// <summary>
			/// Gets/sets the title.
			/// </summary>
			public string Title { get; set; }

			/// <summary>
			/// Gets/sets the alternate navigation title.
			/// </summary>
			public string NavigationTitle { get; set; }

			/// <summary>
			/// Gets/sets the unique slug.
			/// </summary>
			public string Slug { get; set; }

			/// <summary>
			/// Gets/sets the url.
			/// </summary>
			public string Url { get; set; }

			/// <summary>
			/// Gets/sets the items level of depth in the sitemap.
			/// </summary>
			public int Level { get; set; }

			/// <summary>
			/// Gets/sets the child items.
			/// </summary>
			public IEnumerable<SiteMapItem> Items { get; set; }
			#endregion

			/// <summary>
			/// Default constructor.
			/// </summary>
			public SiteMapItem() {
				Items = new List<SiteMapItem>();
			}

			/// <summary>
			/// Checks if the item and it's children contains the given id.
			/// </summary>
			/// <param name="id">The unique id</param>
			/// <returns>If the id is contained within the item</returns>
			internal bool Contains(Guid id) {
				if (Id == id)
					return true;
				foreach (var item in Items) {
					if (item.Contains(id))
						return true;
				}
				return false;
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets/sets the collection of sitemap items.
		/// </summary>
		public IEnumerable<SiteMapItem> Items { get; set; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SiteMap() {
			Items = new List<SiteMapItem>();
		}

		/// <summary>
		/// Gets the current sitemap structure.
		/// </summary>
		/// <returns>The sitemap</returns>
		public static SiteMap Get() {
			var sitemap = App.ModelCache.GetSiteMap();

			if (sitemap == null) {
				sitemap = new SiteMap();

				using (var api = new Api()) {
					var now = DateTime.Now.ToUniversalTime();

					var items = api.Pages.Get(where: p => p.Published <= now).Select(p => new SiteMapItem() {
						Id = p.Id,
						ParentId = p.ParentId,
						SortOrder = p.SortOrder,
						Title = p.Title,
						NavigationTitle = p.NavigationTitle,
						IsHidden = p.IsHidden,
						Slug = p.Slug
					}).OrderBy(p => p.ParentId).ThenBy(p => p.SortOrder).ToList();

					sitemap.Items = Sort(items, null);
				}
				App.ModelCache.SetSiteMap(sitemap);
			}
			return sitemap;
		}

		/// <summary>
		/// Gets a partial sitemap structure with the item with the
		/// given slug as root node.
		/// </summary>
		/// <param name="slug">The slug</param>
		/// <returns>The partial sitemap</returns>
		public static SiteMapItem GetPartial(string slug) {
			return FindItem(Get().Items, slug);
		}

		/// <summary>
		/// Gets the level in the hierarchy with the specified parent.
		/// </summary>
		/// <param name="id">The parent id</param>
		/// <param name="level">The requested level</param>
		/// <returns>The level</returns>
		public IEnumerable<SiteMapItem> GetLevel(Guid? id, int level) {
			return GetLevel(Items, id, level);
		}

		#region Private methods
		/// <summary>
		/// Sorts the pages into a hierarchical structure.
		/// </summary>
		/// <param name="pages">The page collection</param>
		/// <param name="parentId">The current parent id</param>
		/// <param name="level">The current level in the sitemap</param>
		/// <returns>The sorted structure</returns>
		private static IEnumerable<SiteMapItem> Sort(IEnumerable<SiteMapItem> pages, Guid? parentId, int level = 1) {
			var ret = new List<SiteMapItem>();

			foreach (var page in pages.Where(p => p.ParentId == parentId)) {
				page.Level = level;
				page.Url = Utils.Url("~/" + page.Slug);
				page.Items = Sort(pages.Where(p => p.ParentId != parentId), page.Id, level + 1);
				ret.Add(page);
			}
			return ret;
		}

		/// <summary>
		/// Finds the sitemap item with the given slug recursively in the given item list.
		/// </summary>
		/// <param name="items">The items</param>
		/// <param name="slug">The slug to search for</param>
		/// <returns>The item, null if not found</returns>
		private static SiteMapItem FindItem(IEnumerable<SiteMapItem> items, string slug) {
			foreach (var item in items) {
				if (item.Slug == slug)
					return item;

				var node = FindItem(item.Items, slug);
				if (node != null)
					return node;
			}
			return null;
		}

		/// <summary>
		/// Gets the level in the hierarchy with the specified parent.
		/// </summary>
		/// <param name="items">The items to search</param>
		/// <param name="id">The parent id</param>
		/// <param name="level">The requested level</param>
		/// <returns>The level</returns>
		private IEnumerable<SiteMapItem> GetLevel(IEnumerable<SiteMapItem> items, Guid? id, int level) {
			if (items == null || items.Count() == 0 || items.First().Level == level)
				return items;
			if (id.HasValue) {
				foreach (var item in items) {
					if (item.Contains(id.Value))
						return GetLevel(item.Items, id, level);
				}
			}
			return null;
		}
		#endregion
	}
}
