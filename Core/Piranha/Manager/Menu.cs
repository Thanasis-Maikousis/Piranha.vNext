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

namespace Piranha.Manager
{
	/// <summary>
	/// Static class for defining the manager menu.
	/// </summary>
	public static class Menu
	{
		#region Inner classes
		/// <summary>
		/// An item in the manager menu.
		/// </summary>
		public class MenuItem
		{
			#region Properties
			/// <summary>
			/// Gets/sets the internal id.
			/// </summary>
			public string InternalId { get; set; }

			/// <summary>
			/// Gets/sets the display name.
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Gets/sets the optional css class.
			/// </summary>
			public string Css { get; set; }

			/// <summary>
			/// Gets/sets the manager controller.
			/// </summary>
			public string Controller { get; set; }

			/// <summary>
			/// Gets/sets the default action to invoke.
			/// </summary>
			public string Action { get; set; }

			/// <summary>
			/// Gets/sets the available items.
			/// </summary>
			public IList<MenuItem> Items { get; set; }
			#endregion

			/// <summary>
			/// Default constructor.
			/// </summary>
			public MenuItem() {
				Items = new List<MenuItem>();
			}
		}
		#endregion

		/// <summary>
		/// The basic manager menu.
		/// </summary>
		public static IList<MenuItem> Items = new List<MenuItem>() { 
			new MenuItem() {
				InternalId = "Content", Name = "Content", Css = "ico-content", Items = new List<MenuItem>() {
					new MenuItem() {
						InternalId = "Blocks", Name = "Blocks", Controller = "BlockMgr", Action = "List"
					},
					new MenuItem() {
						InternalId = "Posts", Name = "Posts", Controller = "PostMgr", Action = "List"
					}
				}
			},
			new MenuItem() {
				InternalId = "Settings", Name = "Settings", Css = "ico-settings", Items = new List<MenuItem>() {
					new MenuItem() {
						InternalId = "Aliases", Name = "Aliases", Controller = "AliasMgr", Action = "List"
					},
					new MenuItem() {
						InternalId = "Authors", Name = "Authors", Controller = "AuthorMgr", Action = "List"
					},
					new MenuItem() {
						InternalId = "PostTypes", Name = "Post types", Controller = "PostTypeMgr", Action = "List"
					}
				}
			}
		};
	}
}