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
using System.Linq;
using System.Web.Mvc;
using Piranha.Client.Models;

namespace Piranha.AspNet.Mvc
{
	/// <summary>
	/// Base controller for a single page.
	/// </summary>
	public abstract class PageController : Controller
	{
		#region Properties
		/// <summary>
		/// Gets the currently requested page id.
		/// </summary>
		private Guid PageId { get; set; }
		#endregion

		/// <summary>
		/// Gets the model for the currently requested page.
		/// </summary>
		/// <returns>The model</returns>
		protected PageModel GetModel() {
			return GetModel<PageModel>();
		}

		/// <summary>
		/// Gets the model for the currently requested page.
		/// </summary>
		/// <returns>The model</returns>
		protected T GetModel<T>() where T : PageModel {
			return PageModel.GetById<T>(PageId);
		}

		/// <summary>
		/// Initializes the controller.
		/// </summary>
		/// <param name="filterContext">The current filter context</param>
		protected override void OnActionExecuting(ActionExecutingContext filterContext) {
			PageId = new Guid(Request["id"]);

			base.OnActionExecuting(filterContext);
		}
	}
}
