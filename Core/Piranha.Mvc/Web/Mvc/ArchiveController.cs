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
	/// Base controller for a post archive.
	/// </summary>
	public abstract class ArchiveController : Controller
	{
		#region Properties
		/// <summary>
		/// Gets the currenly requested post type.
		/// </summary>
		protected Guid PostTypeId { get; private set; }

		/// <summary>
		/// Gets the currently requested year.
		/// </summary>
		protected int? Year { get; private set; }

		/// <summary>
		/// Gets the currently requested month.
		/// </summary>
		protected int? Month { get; private set; }

		/// <summary>
		/// Gets the currently requested page.
		/// </summary>
		protected int? Page { get; private set; }
		#endregion

		/// <summary>
		/// Gets the archive model for the current request.
		/// </summary>
		/// <returns>The model</returns>
		protected Models.ArchiveModel GetModel() {
			return GetModel<Models.ArchiveModel>();
		}

		/// <summary>
		/// Gets the archive model for the current request.
		/// </summary>
		/// <returns>The model</returns>
		protected Models.ArchiveModel GetModel<T>() where T : Models.ArchiveModel {
			return Models.ArchiveModel.GetById<T>(PostTypeId, Page, Year, Month);
		}

		/// <summary>
		/// Initializes the controller.
		/// </summary>
		/// <param name="filterContext">The current filter context</param>
		protected override void OnActionExecuting(ActionExecutingContext filterContext) {
			PostTypeId = new Guid(Request["id"]);

			try {
				Year = Convert.ToInt32(Request["year"]);
			} catch { }

			if (Year.HasValue) {
				try {
					Month = Convert.ToInt32(Request["month"]);
				} catch { }
			}

			try {
				Page = Convert.ToInt32(Request["page"]);
			} catch { }
	
			base.OnActionExecuting(filterContext);
		}
	}
}
