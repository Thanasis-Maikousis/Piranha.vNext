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

namespace Piranha.Areas.Manager.Controllers
{
	/// <summary>
	/// Abstract base class for all manager controllers.
	/// </summary>
	public abstract class ManagerController : Controller
	{
		#region Properties
		/// <summary>
		/// The api.
		/// </summary>
		protected readonly Api api = new Api();
		#endregion

		/// <summary>
		/// Disposes the controller
		/// </summary>
		/// <param name="disposing">State of disposal</param>
		protected override void Dispose(bool disposing) {
			// Dispose the api
			api.Dispose();

			// Dispose the base controller
			base.Dispose(disposing);
		}
	}
}