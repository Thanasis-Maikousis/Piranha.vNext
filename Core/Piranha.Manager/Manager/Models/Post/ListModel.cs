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

namespace Piranha.Manager.Models.Post
{
	/// <summary>
	/// View model for the post list.
	/// </summary>
	public class ListModel
	{
		#region Inner classes
		/// <summary>
		/// An item in the post list.
		/// </summary>
		public class PostListItem
		{
			/// <summary>
			/// Gets/sets the unique id.
			/// </summary>
			public Guid Id { get; set; }

			/// <summary>
			/// Gets/sets the title.
			/// </summary>
			public string Title { get; set; }

			/// <summary>
			/// Gets/sets the name of the post type.
			/// </summary>
			public string Type { get; set; }

			/// <summary>
			/// Gets/sets the date the post was initially created.
			/// </summary>
			public DateTime Created { get; set; }

			/// <summary>
			/// Gets/sets the date the post was last updated.
			/// </summary>
			public DateTime Updated { get; set; }

			/// <summary>
			/// Gets/sets the published date.
			/// </summary>
			public DateTime? Published { get; set; }
		}

		/// <summary>
		/// An item in the post type list.
		/// </summary>
		public class PostTypeListItem
		{
			/// <summary>
			/// Gets/sets the display name.
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Gets/sets the unique slug.
			/// </summary>
			public string Slug { get; set; }

			/// <summary>
			/// Gets/sets if this is the currently selected post type.
			/// </summary>
			public bool IsActive { get; set; }
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets/sets the available items.
		/// </summary>
		public IList<PostListItem> Items { get; set; }

		/// <summary>
		/// Gets/sets the available post types.
		/// </summary>
		public IList<PostTypeListItem> PostTypes { get; set; }

		/// <summary>
		/// Gets/sets if no specific post type has been selected.
		/// </summary>
		public bool IsDefaultSelection { get; set; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ListModel() {
			Items = new List<PostListItem>();
			PostTypes = new List<PostTypeListItem>();
			IsDefaultSelection = true;
		}

		/// <summary>
		/// Gets the post list model.
		/// </summary>
		/// <param name="slug">Optional post type slug</param>
		/// <returns>The model</returns>
		public static ListModel Get(string slug = null) {
			using (var api = new Api()) {
				var m = new ListModel();

				if (!String.IsNullOrWhiteSpace(slug)) {
					m.Items = api.Posts.Get(where: p => p.Type.Slug == slug).Select(p => new PostListItem() {
						Id = p.Id,
						Title = p.Title,
						Type = p.Type.Name,
						Created = p.Created,
						Updated = p.Updated,
						Published = p.Published
					}).ToList();
					m.IsDefaultSelection = false;
				} else {
					m.Items = api.Posts.Get().Select(p => new PostListItem() {
						Id = p.Id,
						Title = p.Title,
						Type = p.Type.Name,
						Created = p.Created,
						Updated = p.Updated,
						Published = p.Published
					}).ToList();
				}
				m.PostTypes = api.PostTypes.Get().Select(t => new PostTypeListItem() {
					Name = t.Name,
					Slug = t.Slug,
					IsActive = t.Slug == slug
				}).ToList();
				return m;
			}
		}
	}
}