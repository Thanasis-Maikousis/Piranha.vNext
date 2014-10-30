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
using System.Web;

namespace Piranha.Web
{
	/// <summary>
	/// Web extensions.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Gets/sets the current item being processed.
		/// </summary>
		public static Models.Content GetCurrent(this HttpContext context) {
			return (Web.Models.Content)context.Items["PiranhaCurrent"];
		}

		/// <summary>
		/// Gets/sets the current item being processed.
		/// </summary>
		public static void SetCurrent(this HttpContext context, Models.Content current) {
			context.Items["PiranhaCurrent"] = current;
		}
	}
}
