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
using System.Web.Mvc;

namespace PiranhaCMS.Controllers
{
	/// <summary>
	/// Controller for handling application errors.
	/// </summary>
	public class ErrorController : Controller
	{
		/// <summary>
		/// Gets the default error view.
		/// </summary>
		/// <returns>The view result</returns>
		public ActionResult Index() {
			return View();
		}

		/// <summary>
		/// Gets the error view for a 404.
		/// </summary>
		/// <returns>The view result</returns>
		public ActionResult NotFound() {
			return View();
		}
	}
}