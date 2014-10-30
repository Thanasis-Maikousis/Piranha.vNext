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
using Piranha.Cache;

namespace PiranhaCMS.Controllers
{
	/// <summary>
	/// Default controller for displaying a page.
	/// </summary>
	public class PageController : Piranha.Web.Mvc.PageController
	{
		/// <summary>
		/// Gets the view for the current page.
		/// </summary>
		/// <returns>The view result</returns>
		public ActionResult Index() {
			var model = GetModel();

			if (!HttpContext.IsCached(model.Id.ToString(), model.GetLastModified()))
				return View(model);
			return null;
		}
	}
}