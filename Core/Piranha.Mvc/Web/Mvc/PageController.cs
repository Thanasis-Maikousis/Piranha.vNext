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
using System.Linq;
using System.Web.Mvc;

namespace Piranha.Web.Mvc
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
		/// Gets the post model for the currently requested post.
		/// </summary>
		/// <returns>The model</returns>
		protected Models.PageModel GetModel() {
			return GetModel<Models.PageModel>();
		}

		/// <summary>
		/// Gets the post model for the currently requested post.
		/// </summary>
		/// <returns>The model</returns>
		protected T GetModel<T>() where T : Models.PageModel {
			return Models.PageModel.GetById<T>(PageId);
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
