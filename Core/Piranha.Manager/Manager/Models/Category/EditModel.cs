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

namespace Piranha.Manager.Models.Category
{
	/// <summary>
	/// View model for the category edit view.
	/// </summary>
	public sealed class EditModel
	{
		#region Properties
		/// <summary>
		/// Gets/sets the unique id.
		/// </summary>
		public Guid? Id { get; set; }

		/// <summary>
		/// Gets/sets the title.
		/// </summary>
		[Required, StringLength(128)]
		public string Title { get; set; }

		/// <summary>
		/// Gets/sets the unique slug.
		/// </summary>
		[StringLength(128)]
		public string Slug { get; set; }
		#endregion

		/// <summary>
		/// Gets the edit model for the category with the given id.
		/// </summary>
		/// <param name="api">The current api</param>
		/// <param name="id">The unique id</param>
		/// <returns>The edit model</returns>
		public static EditModel GetById(Api api, Guid id) {
			var category = api.Categories.GetSingle(where: a => a.Id == id);

			if (category != null)
				return Mapper.Map<Piranha.Models.Category, EditModel>(category);
			return null;
		}

		/// <summary>
		/// Saves the current edit model.
		/// </summary>
		public void Save(Api api) {
			var newModel = false;

			// Get or create category
			var category = Id.HasValue ? api.Categories.GetSingle(Id.Value) : null;
			if (category == null) {
				category = new Piranha.Models.Category();
				newModel = true;
			}

			// Map values
			Mapper.Map<EditModel, Piranha.Models.Category>(this, category);

			if (newModel)
				api.Categories.Add(category);
			api.SaveChanges();

			this.Id = category.Id;
		}
	}
}
