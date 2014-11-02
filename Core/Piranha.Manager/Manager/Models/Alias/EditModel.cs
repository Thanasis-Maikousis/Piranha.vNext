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
using System.Web.Mvc;

namespace Piranha.Manager.Models.Alias
{
	/// <summary>
	/// View model for the alias edit view.
	/// </summary>
	public sealed class EditModel
	{
		#region Properties
		/// <summary>
		/// Gets/sets the unique id.
		/// </summary>
		public Guid? Id { get; set; }

		/// <summary>
		/// Gets/sets the old URL.
		/// </summary>
		[Required, StringLength(255)]
		public string OldUrl { get; set; }

		/// <summary>
		/// Gets/sets the new URL.
		/// </summary>
		[Required, StringLength(255)]
		public string NewUrl { get; set; }

		/// <summary>
		/// Gets/sets if this is a permanent redirect or not.
		/// </summary>
		public bool IsPermanent { get; set; }
		#endregion

		/// <summary>
		/// Gets the edit model for the alias with the given id.
		/// </summary>
		/// <param name="api">The current api</param>
		/// <param name="id">The unique id</param>
		/// <returns>The edit model</returns>
		public static EditModel GetById(Api api, Guid id) {
			var alias = api.Aliases.GetSingle(where: a => a.Id == id);

			if (alias != null)
				return Mapper.Map<Piranha.Models.Alias, EditModel>(alias);
			return null;
		}

		/// <summary>
		/// Saves the current edit model.
		/// </summary>
		public void Save(Api api) {
			var newModel = false;

			// Get or create alias
			var alias = api.Aliases.GetSingle(where: a => a.Id == Id);
			if (alias == null) {
				alias = new Piranha.Models.Alias();
				newModel = true;
			}

			// Map values
			Mapper.Map<EditModel, Piranha.Models.Alias>(this, alias);

			if (newModel)
				api.Aliases.Add(alias);
			api.SaveChanges();

			this.Id = alias.Id;
		}
	}
}
