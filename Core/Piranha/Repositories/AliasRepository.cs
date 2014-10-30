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
	public sealed class AliasRepository : Repository<Models.Alias>
	{
		/// <summary>
		/// Default internal constructor.
		/// </summary>
		/// <param name="session">The current session</param>
		internal AliasRepository(Data.ISession session) : base(session) { }

		/// <summary>
		/// Adds a new or updated model to the api.
		/// </summary>
		/// <param name="model">The model</param>
		public override void Add(Models.Alias model) {
			// Ensure URL's
			model.OldUrl = EnsureUrlSyntax(model.OldUrl);
			model.NewUrl = EnsureUrlSyntax(model.NewUrl);

			// Add model
			base.Add(model);
		}

		/// <summary>
		/// Gets the model identified by the given id. 
		/// </summary>
		/// <remarks>
		/// This method uses the configured cache for performance.
		/// </remarks>
		/// <param name="id">The unique id</param>
		/// <returns>The model</returns>
		public override Models.Alias GetSingle(Guid id) {
			var model = App.ModelCache.Aliases.Get(id);

			if (model == null) {
				model = base.GetSingle(id);

				if (model != null)
					App.ModelCache.Aliases.Add(model);
			}
			return model;
		}

		/// <summary>
		/// Gets the model identified by the given slug. 
		/// </summary>
		/// <remarks>
		/// This method uses the configured cache for performance.
		/// </remarks>
		/// <param name="url">The unique old url</param>
		/// <returns>The model</returns>
		public Models.Alias GetSingle(string url) {
			var model = App.ModelCache.Aliases.Get(url);

			if (model == null) {
				model = base.GetSingle(where: a => a.OldUrl == url);

				if (model != null)
					App.ModelCache.Aliases.Add(model);
			}
			return model;
		}

		/// <summary>
		/// Orders the category query by name.
		/// </summary>
		/// <param name="query">The query</param>
		/// <returns>The ordered query</returns>
		protected override IQueryable<Models.Alias> Order(IQueryable<Models.Alias> query) {
			return query.OrderBy(a => a.OldUrl);
		}

		#region Private methods
		/// <summary>
		/// Ensures the syntax of the provided url.
		/// </summary>
		/// <param name="url">The given url</param>
		/// <returns>The new url</returns>
		private string EnsureUrlSyntax(string url) {
			if (!String.IsNullOrWhiteSpace(url)) {
				if (url.StartsWith("~"))
					url = url.Substring(1);
				if (!url.StartsWith("/"))
					url = "/" + url;
				return url;
			}
			return null;
		}
		#endregion
	}
}