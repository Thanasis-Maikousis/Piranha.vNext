#if DEBUG
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
using System.Web.Mvc;
using System.Web.Routing;

namespace Piranha
{
	/// <summary>
	/// Main application class for the manager interface. Only for
	/// debugging purposes.
	/// </summary>
	public class MvcApplication : System.Web.HttpApplication
	{
		/// <summary>
		/// Starts the application.
		/// </summary>
		protected void Application_Start() {
			// Register routes & areas
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			AreaRegistration.RegisterAllAreas();

			// Initialize the application instance
			Piranha.App.Init(c => {
				c.Env = new AspNet.Env();
				c.Security = new AspNet.Security.SimpleSecurity("admin", "password");
				c.Store = new EntityFramework.Store();
			});
		}
	}
}
#endif