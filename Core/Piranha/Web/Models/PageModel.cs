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

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Piranha.Models;

namespace Piranha.Web.Models
{
	/// <summary>
	/// Application page model.
	/// </summary>
	public class PageModel
	{
		#region Properties
		/// <summary>
		/// Gets/sets the unique id.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets/sets the slug of the page type.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// Gets/sets the optional parent id.
		/// </summary>
		public Guid? ParentId { get; set; }

		/// <summary>
		/// Gets/sets the sort order of the page.
		/// </summary>
		public int SortOrder { get; set; }

		/// <summary>
		/// Gets/sets if the page should be hidden from navigations.
		/// </summary>
		public bool IsHidden { get; set; }

		/// <summary>
		/// Gets/sets the title.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Gets/sets the unique slug.
		/// </summary>
		public string Slug { get; set; }

		/// <summary>
		/// Gets/sets the optional keywords.
		/// </summary>
		public string Keywords { get; set; }

		/// <summary>
		/// Gets/sets the optional description.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets/sets the main page body.
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		/// Gets/sets the optional route that should handle requests.
		/// </summary>
		public string Route { get; set; }

		/// <summary>
		/// Gets/sets the optional view that should render requests.
		/// </summary>
		public string View { get; set; }

		/// <summary>
		/// Gets/sets when the model was initially created.
		/// </summary>
		public DateTime Created { get; set; }

		/// <summary>
		/// Gets/sets when the model was last updated.
		/// </summary>
		public DateTime Updated { get; set; }

		/// <summary>
		/// Gets/sets when the model was published.
		/// </summary>
		public DateTime Published { get; set; }

		/// <summary>
		/// Gets/sets the author responsible for this content.
		/// </summary>
		public Author Author { get; set; }

		/// <summary>
		/// Gets if this is the startpage of the site.
		/// </summary>
		public bool IsStartPage {
			get { return !ParentId.HasValue && SortOrder == 1; }
		}
		#endregion

		/// <summary>
		/// Gets the page model identified by the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <returns>The model</returns>
		public static PageModel GetById(Guid id) {
			return GetById<PageModel>(id);
		}

		/// <summary>
		/// Gets the page model identified by the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <returns>The model</returns>
		public static T GetById<T>(Guid id) where T : PageModel {
			var model = (T)App.ModelCache.Pages.Get(id);

			if (model == null) {
				using (var api = new Api()) {
					var now = DateTime.Now.ToUniversalTime();

					model = Map<T>(api, api.Pages.GetSingle(where: p => p.Id == id && p.Published <= now));

					if (model != null)
						App.ModelCache.Pages.Add(model);
				}
			}
			return model;
		}

		/// <summary>
		/// Gets the page model identified by the given slug.
		/// </summary>
		/// <param name="slug">The unique slug</param>
		/// <returns>The model</returns>
		public static PageModel GetBySlug(string slug) {
			return GetBySlug<PageModel>(slug);
		}

		/// <summary>
		/// Gets the page model identified by the given slug.
		/// </summary>
		/// <param name="slug">The unique slug</param>
		/// <returns>The model</returns>
		public static T GetBySlug<T>(string slug) where T : PageModel {
			var model = (T)App.ModelCache.Pages.Get(slug);

			if (model == null) {
				using (var api = new Api()) {
					var now = DateTime.Now.ToUniversalTime();

					model = Map<T>(api, api.Pages.GetSingle(where: p => p.Slug == slug && p.Published <= now));

					if (model != null)
						App.ModelCache.Pages.Add(model);
				}
			}
			return model;
		}

		/// <summary>
		/// Gets the last modification date for the curremt post model.
		/// </summary>
		public virtual DateTime GetLastModified() {
			if (Updated > Published)
				return Updated;
			return Published;
		}

		#region Private methods
		/// <summary>
		/// Maps the given page to a new page model.
		/// </summary>
		/// <param name="api">The current api</param>
		/// <param name="page">The page</param>
		/// <returns>The page model</returns>
		private static T Map<T>(Api api, Page page) where T : PageModel {
			if (page != null) {
				var model = Activator.CreateInstance<T>();

				Mapper.Map<Page, PageModel>(page, model);

				return model;
			}
			return null;
		}
		#endregion
	}
}
