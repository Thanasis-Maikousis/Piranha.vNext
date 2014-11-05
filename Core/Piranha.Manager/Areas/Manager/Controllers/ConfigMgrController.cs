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
using System.Web.Mvc;
using Piranha.Manager.Models.Config;

namespace Piranha.Areas.Manager.Controllers
{
	/// <summary>
	/// Controller for managing application & module config.
	/// </summary>
    public class ConfigMgrController : ManagerController
    {
		/// <summary>
		/// Gets the main view for the config.
		/// </summary>
		/// <returns>The list view</returns>
        public ActionResult List() {
            return View();
        }

		/// <summary>
		/// Gets the list model data.
		/// </summary>
		/// <returns>The model data</returns>
		public ActionResult Get() {
			return JsonData(true, ListModel.Get());
		}
    }
}