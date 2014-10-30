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

namespace Piranha.Manager.Models.Block
{
	/// <summary>
	/// View model for the block edit view.
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
		/// Gets/sets the unique slug.
		/// </summary>
		[StringLength(128)]
		public string Slug { get; set; }

		/// <summary>
		/// Gets/sets the optional description.
		/// </summary>
		[StringLength(255)]
		public string Description { get; set; }

		/// <summary>
		/// Gets/sets the main body.
		/// </summary>
		public string Body { get; set; }
		#endregion

		/// <summary>
		/// Gets the edit model for the block with the given id.
		/// </summary>
		/// <param name="api">The current api</param>
		/// <param name="id">The unique id</param>
		/// <returns>The edit model</returns>
		public static EditModel GetById(Api api, Guid id) {
			var block = api.Blocks.GetSingle(where: b => b.Id == id);

			if (block != null)
				return Mapper.Map<Piranha.Models.Block, EditModel>(block);
			return new EditModel();
		}

		/// <summary>
		/// Saves the current edit model.
		/// </summary>
		public void Save(Api api) {
			bool newModel = false;

			// Get or create block
			var block = api.Blocks.GetSingle(where: b => b.Id == Id);
			if (block == null) {
				block = new Piranha.Models.Block();
				newModel = true;
			}

			// Map values
			Mapper.Map<EditModel, Piranha.Models.Block>(this, block);

			if (newModel)
				api.Blocks.Add(block);
			api.SaveChanges();

			this.Id = block.Id;
		}
	}
}
