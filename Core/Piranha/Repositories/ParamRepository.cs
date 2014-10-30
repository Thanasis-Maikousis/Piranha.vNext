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
	public sealed class ParamRepository : Repository<Models.Param>
	{
		/// <summary>
		/// Default internal constructor.
		/// </summary>
		/// <param name="session">The current session</param>
		internal ParamRepository(Data.ISession session) : base(session) { }

		/// <summary>
		/// Gets the model identified by the given id. 
		/// </summary>
		/// <remarks>
		/// This method uses the configured cache for performance.
		/// </remarks>
		/// <param name="id">The unique id</param>
		/// <returns>The model</returns>
		public override Models.Param GetSingle(Guid id) {
			var model = App.ModelCache.Params.Get(id);

			if (model == null) {
				model = base.GetSingle(id);

				if (model != null)
					App.ModelCache.Params.Add(model);
			}
			return model;
		}

		/// <summary>
		/// Gets the model identified by the given name. 
		/// </summary>
		/// <remarks>
		/// This method uses the configured cache for performance.
		/// </remarks>
		/// <param name="name">The unique name</param>
		/// <returns>The model</returns>
		public Models.Param GetSingle(string name) {
			var model = App.ModelCache.Params.Get(name);

			if (model == null) {
				model = base.GetSingle(where: p => p.Name == name);

				if (model != null)
					App.ModelCache.Params.Add(model);
			}
			return model;
		}

		/// <summary>
		/// Orders the category query by name.
		/// </summary>
		/// <param name="query">The query</param>
		/// <returns>The ordered query</returns>
		protected override IQueryable<Models.Param> Order(IQueryable<Models.Param> query) {
			return query.OrderBy(b => b.Name);
		}
	}
}