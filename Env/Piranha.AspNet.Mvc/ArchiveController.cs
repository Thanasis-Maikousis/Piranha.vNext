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
		protected ArchiveModel GetModel() {
			return GetModel<ArchiveModel>();
		}

		/// <summary>
		/// Gets the archive model for the current request.
		/// </summary>
		/// <returns>The model</returns>
		protected ArchiveModel GetModel<T>() where T : ArchiveModel {
			return ArchiveModel.GetById<T>(PostTypeId, Page, Year, Month);
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
