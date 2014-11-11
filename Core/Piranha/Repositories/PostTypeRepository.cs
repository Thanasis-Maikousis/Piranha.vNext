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

namespace Piranha.Repositories
{
	public sealed class PostTypeRepository : Repository<Models.PostType>
	{
		/// <summary>
		/// Default internal constructor.
		/// </summary>
		/// <param name="session">The current session</param>
		internal PostTypeRepository(Data.ISession session) : base(session) { }

		/// <summary>
		/// Gets the model identified by the given id. 
		/// </summary>
		/// <remarks>
		/// This method uses the configured cache for performance.
		/// </remarks>
		/// <param name="id">The unique id</param>
		/// <returns>The model</returns>
		public override Models.PostType GetSingle(Guid id) {
			var model = App.ModelCache.PostTypes.Get(id);

			if (model == null) {
				model = base.GetSingle(id);

				if (model != null)
					App.ModelCache.PostTypes.Add(model);
			}
			return model;
		}

		/// <summary>
		/// Gets the model identified by the given slug. 
		/// </summary>
		/// <remarks>
		/// This method uses the configured cache for performance.
		/// </remarks>
		/// <param name="slug">The unique slug</param>
		/// <returns>The model</returns>
		public Models.PostType GetSingle(string slug) {
			var model = App.ModelCache.PostTypes.Get(slug);

			if (model == null) {
				model = base.GetSingle(where: p => p.Slug == slug);

				if (model != null)
					App.ModelCache.PostTypes.Add(model);
			}
			return model;
		}

		/// <summary>
		/// Method for getting a post type by id. This method is
		/// cached for performance.
		/// </summary>
		/// <param name="id">The unique slug</param>
		/// <returns>The post type</returns>
		public Models.PostType GetById(Guid id) {
			var model = App.ModelCache.PostTypes.Get(id);

			if (model == null) {
				model = GetSingle(where: t => t.Id == id);

				if (model != null)
					App.ModelCache.PostTypes.Add(model);
			}
			return model;
		}

		/// <summary>
		/// Method for getting a post type by slug. This method is
		/// cached for performance.
		/// </summary>
		/// <param name="slug">The unique slug</param>
		/// <returns>The post type</returns>
		public Models.PostType GetBySlug(string slug) {
			var model = App.ModelCache.PostTypes.Get(slug);

			if (model == null) {
				model = GetSingle(where: t => t.Slug == slug);

				if (model != null)
					App.ModelCache.PostTypes.Add(model);
			}
			return model;
		}

		/// <summary>
		/// Adds a new or updated model to the api.
		/// </summary>
		/// <param name="model">The model</param>
		public override void Add(Models.PostType model) {
			// Ensure slug
			if (String.IsNullOrWhiteSpace(model.Slug))
				model.Slug = Utils.GenerateSlug(model.Name);

			// Add model
			base.Add(model);
		}

		/// <summary>
		/// Orders the post type query by name.
		/// </summary>
		/// <param name="query">The query</param>
		/// <returns>The ordered query</returns>
		protected override IQueryable<Models.PostType> Order(IQueryable<Models.PostType> query) {
			return query.OrderBy(t => t.Name);
		}
	}
}