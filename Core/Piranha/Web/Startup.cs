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
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(Piranha.Web.Startup), "PreInit")]
[assembly: PostApplicationStartMethod(typeof(Piranha.Web.Startup), "Init")]

namespace Piranha.Web
{
	/// <summary>
	/// Class responsible for starting the web application.
	/// </summary>
	public sealed class Startup
	{
		/// <summary>
		/// Initializes the pre startup events.
		/// </summary>
		public static void PreInit() { 
			Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(Module));
		}

		/// <summary>
		/// Initializes the application.
		/// </summary>
		public static void Init() {
		}
	}
}
