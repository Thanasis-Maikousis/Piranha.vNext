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
using System.IO;
using System.Text;
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
		/// Returns a Json object with the given status code and rendered view.
		/// </summary>
		/// <param name="success">The status code</param>
		/// <param name="result">The view</param>
		/// <returns>The result</returns>
		protected ActionResult JsonView(bool success, ViewResult result) {
			return Json(new {
				success = success,
				body = result != null ? ViewToString(result) : null
			});
		}

		/// <summary>
		/// Returns a Json object with the given status code and data.
		/// </summary>
		/// <param name="success">The status code</param>
		/// <param name="data">The data</param>
		/// <returns>The result</returns>
		protected ActionResult JsonView(bool success, object data = null) {
			return Json(new {
				success = success,
				body = data
			});
		}

		protected ActionResult JsonData(bool success, object data = null) {
			return Json(new {
				success = success,
				data = data
			}, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Renders the given view result to a string
		/// </summary>
		/// <param name="viewResult">The view result</param>
		/// <returns>A html string</returns>
		protected string ViewToString(ViewResult viewResult) {
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb) ;

			//Finding rendered view
			var view = ViewEngines.Engines.FindView(ControllerContext, viewResult.ViewName, viewResult.MasterName).View ;

			//Creating view context
			var viewContext = new ViewContext(ControllerContext, view, ViewData, TempData, sw) ;
			view.Render(viewContext, sw) ;
			return sb.ToString() ;	
		}

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