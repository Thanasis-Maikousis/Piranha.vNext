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
using System.Linq.Expressions;

namespace Piranha.Repositories
{
	public sealed class PostRepository : Repository<Models.Post>
	{
		/// <summary>
		/// Default internal constructor.
		/// </summary>
		/// <param name="session">The current session</param>
		internal PostRepository(Data.ISession session) : base(session) { }

		/// <summary>
		/// Gets the model identified by the given id. 
		/// </summary>
		/// <remarks>
		/// This method uses the configured cache for performance.
		/// </remarks>
		/// <param name="id">The unique id</param>
		/// <returns>The model</returns>
		public override Models.Post GetSingle(Guid id) {
			var model = App.ModelCache.Posts.Get(id);

			if (model == null) {
				model = base.GetSingle(id);

				if (model != null)
					App.ModelCache.Posts.Add(model);
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
		/// <param name="type">The unique post type slug</param>
		/// <returns>The model</returns>
		public Models.Post GetSingle(string slug, string type) {
			var model = App.ModelCache.Posts.Get(type + "_" + slug);

			if (model == null) {
				model = base.GetSingle(where: p => p.Slug == slug && p.Type.Slug == type);

				if (model != null)
					App.ModelCache.Posts.Add(model);
			}
			return model;
		}

		/// <summary>
		/// Adds a new or updated model to the api.
		/// </summary>
		/// <param name="model">The model</param>
		public override void Add(Models.Post model) {
			// Ensure slug
			if (String.IsNullOrWhiteSpace(model.Slug))
				model.Slug = Utils.GenerateSlug(model.Title);

			// Add model
			base.Add(model);
		}

		/// <summary>
		/// Orders the post query by published date.
		/// </summary>
		/// <param name="query">The query</param>
		/// <returns>The ordered query</returns>
		protected override IQueryable<Models.Post> Order(IQueryable<Models.Post> query) {
			return query.OrderByDescending(p => p.Published);
		}

		/// <summary>
		/// Maps the source model to the destination.
		/// </summary>
		/// <param name="model">The source model</param>
		protected override Models.Post FromDb(Models.Post model) {
			model.Categories = model.Categories.OrderBy(c => c.Title).ToList();

			return base.FromDb(model);
		}
	}
}