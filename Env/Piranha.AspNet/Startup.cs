/*
 * Copyright (c) 2014 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * http://github.com/piranhacms/piranha.vnext
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(Piranha.AspNet.Startup), "PreInit")]
[assembly: PostApplicationStartMethod(typeof(Piranha.AspNet.Startup), "Init")]

namespace Piranha.AspNet
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
		public static void Init() { }
	}
}
