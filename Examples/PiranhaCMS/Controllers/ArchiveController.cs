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
	/// Default controller for displaying a post archive.
	/// </summary>
	public class ArchiveController : Piranha.AspNet.Mvc.ArchiveController
	{
		/// <summary>
		/// Gets the view for the currently requested archive.
		/// </summary>
		/// <returns>The view result</returns>
		public ActionResult Index() {
			var model = GetModel<Models.ArchiveModel>();

			return View(model.View, model);
		}
	}
}