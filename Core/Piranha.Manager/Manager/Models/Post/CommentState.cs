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

namespace Piranha.Manager.Models.Post
{
	/// <summary>
	/// Models for changing a comments state.
	/// </summary>
	public class CommentState
	{
		#region Properties
		/// <summary>
		/// Gets/sets the post id.
		/// </summary>
		public Guid PostId { get; set; }

		/// <summary>
		/// Gets/sets the comment id.
		/// </summary>
		public Guid CommentId { get; set; }

		/// <summary>
		/// Gets/sets the status.
		/// </summary>
		public bool Status { get; set; }
		#endregion
	}
}