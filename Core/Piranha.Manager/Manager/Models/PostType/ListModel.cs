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

namespace Piranha.Manager.Models.PostType
{
	/// <summary>
	/// View model for the post type list.
	/// </summary>
	public class ListModel
	{
		#region Inner classes
		/// <summary>
		/// An item in the post type list.
		/// </summary>
		public class PostTypeListItem
		{
			/// <summary>
			/// Gets/sets the unique id.
			/// </summary>
			public Guid Id { get; set; }

			/// <summary>
			/// Gets/sets the name.
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Gets/sets the date the post type was initially created.
			/// </summary>
			public DateTime Created { get; set; }

			/// <summary>
			/// Gets/sets the date the post type was initially updated.
			/// </summary>
			public DateTime Updated { get; set; }
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets/sets the available items.
		/// </summary>
		public IEnumerable<PostTypeListItem> Items { get; set; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ListModel() {
			Items = new List<PostTypeListItem>();
		}

		/// <summary>
		/// Gets the post type list model.
		/// </summary>
		/// <returns>The model</returns>
		public static ListModel Get() {
			using (var api = new Api()) {
				var m = new ListModel();

				m.Items = api.PostTypes.Get().Select(t => new PostTypeListItem() {
					Id = t.Id,
					Name = t.Name,
					Created = t.Created,
					Updated = t.Updated
				});
				return m;
			}
		}
	}
}