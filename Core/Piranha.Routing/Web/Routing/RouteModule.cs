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
using Piranha.Extend;

namespace Piranha.Web.Routing
{
	/// <summary>
	/// The route module is responsible for parsing the incoming request
	/// and routing it to the correct part of Piranha CMS.
	/// </summary>
	public class RouteModule : IModule
	{
		/// <summary>
		/// Initializes the module. This method should be used for
		/// ensuring runtime resources and registering hooks.
		/// </summary>
		public void Init() {
			// Add the default handlers
			App.Logger.Log(Log.LogLevel.INFO, "RouteModule.Init: Registering default handlers");
			App.Handlers.Aliases = new Handlers.AliasHandler();
			App.Handlers.Pages = new Handlers.PageHandler();
			App.Handlers.Posts = new Handlers.PostHandler();
			App.Handlers.Add("media.ashx", new Handlers.MediaHandler());

			// Attach the router
			App.Logger.Log(Log.LogLevel.INFO, "RouteModule.Init: Attaching OnBeginRequest");
			Hooks.App.Request.OnBeginRequest += Router.OnBeginRequest;
		}
	}
}
