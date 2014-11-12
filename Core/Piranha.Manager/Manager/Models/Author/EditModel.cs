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

namespace Piranha.Manager.Models.Author
{
	/// <summary>
	/// View model for the author edit view.
	/// </summary>
	public sealed class EditModel
	{
		#region Properties
		/// <summary>
		/// Gets/sets the unique id.
		/// </summary>
		public Guid? Id { get; set; }

		/// <summary>
		/// Gets/sets the display name.
		/// </summary>
		[Required, StringLength(128)]
		public string Name { get; set; }

		/// <summary>
		/// Gets/sets the email address.
		/// </summary>
		[StringLength(128)]
		public string Email { get; set; }

		/// <summary>
		/// Gets/sets the description.
		/// </summary>
		[StringLength(512)]
		public string Description { get; set; }

		/// <summary>
		/// Gets/sets the gravatar url.
		/// </summary>
		public string GravatarUrl { get; set; }
		#endregion

		/// <summary>
		/// Gets the edit model for the author with the given id.
		/// </summary>
		/// <param name="api">The current api</param>
		/// <param name="id">The unique id</param>
		/// <returns>The edit model</returns>
		public static EditModel GetById(Api api, Guid id) {
			var author = api.Authors.GetSingle(where: a => a.Id == id);

			if (author != null) {
				var model = Mapper.Map<Piranha.Models.Author, EditModel>(author);
				var ui = new Web.Helpers.UIHelper();

				model.GravatarUrl = ui.GravatarUrl(model.Email, 80).ToHtmlString();

				return model;
			}
			return null;
		}

		/// <summary>
		/// Saves the current edit model.
		/// </summary>
		public void Save(Api api) {
			var newModel = false;

			// Get or create author
			var author = api.Authors.GetSingle(where: a => a.Id == Id);
			if (author == null) {
				author = new Piranha.Models.Author();
				newModel = true;
			}

			// Map values
			Mapper.Map<EditModel, Piranha.Models.Author>(this, author);

			if (newModel)
				api.Authors.Add(author);
			api.SaveChanges();

			this.Id = author.Id;
		}
	}
}
