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

namespace Piranha.Web.Models
{
	/// <summary>
	/// Class for defining the current content being viewed.
	/// </summary>
	public sealed class Content
	{
		/// <summary>
		/// Gets/sets the unique id.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets/sets the content title.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Gets/sets the meta keyworkds.
		/// </summary>
		public string Keywords { get; set; }

		/// <summary>
		/// Gets/sets the meta description.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets the virtual path to the document
		/// </summary>
		public string VirtualPath { get; set; }

		/// <summary>
		/// Gets/sets the current content type.
		/// </summary>
		public ContentType Type { get; set; }
	}
}
