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
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Piranha.Manager.Models.PostType
{
	/// <summary>
	/// View model for the post type edit view.
	/// </summary>
	public class EditModel
	{
		#region Properties
		/// <summary>
		/// Gets/sets the unique id.
		/// </summary>
		public Guid? Id { get; set; }

		/// <summary>
		/// Gets/sets the unique slug.
		/// </summary>
		[StringLength(128)]
		public string Slug { get; set; }

		/// <summary>
		/// Gets/sets the display name.
		/// </summary>
		[Required, StringLength(128)]
		public string Name { get; set; }

		/// <summary>
		/// Gets/sets the optional description.
		/// </summary>
		[StringLength(255)]
		public string Description { get; set; }

		/// <summary>
		/// Gets/sets the optional route that should handle
		/// posts of this type.
		/// </summary>
		[StringLength(255)]
		public string Route { get; set; }

		/// <summary>
		/// Gets/sets the optional view that should render
		/// posts of this type.
		/// </summary>
		[StringLength(255)]
		public string View { get; set; }

		/// <summary>
		/// Gets/sets if archives should be enabled for this post type.
		/// </summary>
		public bool EnableArchive { get; set; }

		/// <summary>
		/// Gets/sets the meta keywords used when rendering the
		/// archive page for the post type.
		/// </summary>
		[StringLength(128)]
		public string MetaKeywords { get; set; }

		/// <summary>
		/// Gets/sets the meta description used when rendering the
		/// archive page for the post type.
		/// </summary>
		[StringLength(255)]
		public string MetaDescription { get; set; }

		/// <summary>
		/// Gets/sets the optional archive title.
		/// </summary>
		[StringLength(128)]
		public string ArchiveTitle { get; set; }

		/// <summary>
		/// Gets/sets the optional route for the archive page
		/// for posts of this type.
		/// </summary>
		[StringLength(255)]
		public string ArchiveRoute { get; set; }

		/// <summary>
		/// Gets/sets the optional view for the archive page
		/// for posts of this type.
		/// </summary>
		[StringLength(255)]
		public string ArchiveView { get; set; }

		/// <summary>
		/// Gets/sets the optional route for commenting posts
		/// of this type.
		/// </summary>
		[StringLength(255)]
		public string CommentRoute { get; set; }
		#endregion

		/// <summary>
		/// Gets the post type model for the given id.
		/// </summary>
		/// <param name="api">The current api</param>
		/// <param name="id">The unique id</param>
		/// <returns>The model</returns>
		public static EditModel GetById(Api api, Guid id) {
			var type = api.PostTypes.GetSingle(where: t => t.Id == id);

			if (type != null)
				return Mapper.Map<Piranha.Models.PostType, EditModel>(type);
			return new EditModel();
		}

		/// <summary>
		/// Saves the current post type.
		/// </summary>
		/// <param name="api">The current api</param>
		public void Save(Api api) {
			var newModel = false;

			var type = api.PostTypes.GetSingle(where: t => t.Id == Id);
			if (type == null) {
				type = new Piranha.Models.PostType();
				newModel = true;
			}

			Mapper.Map<EditModel, Piranha.Models.PostType>(this, type);

			if (newModel)
				api.PostTypes.Add(type);
			api.SaveChanges();

			this.Id = type.Id;
		}
	}
}