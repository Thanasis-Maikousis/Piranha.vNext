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
	/// Base controller for a single post.
	/// </summary>
	public abstract class PostController : Controller
	{
		#region Properties
		/// <summary>
		/// Gets the currently requested post id.
		/// </summary>
		private Guid PostId { get; set; }
		#endregion

		/// <summary>
		/// Gets the model for the currently requested post.
		/// </summary>
		/// <returns>The model</returns>
		protected PostModel GetModel() {
			return GetModel<PostModel>();
		}

		/// <summary>
		/// Gets the model for the currently requested post.
		/// </summary>
		/// <returns>The model</returns>
		protected T GetModel<T>() where T : PostModel {
			return PostModel.GetById<T>(PostId);
		}

		/// <summary>
		/// Initializes the controller.
		/// </summary>
		/// <param name="filterContext">The current filter context</param>
		protected override void OnActionExecuting(ActionExecutingContext filterContext) {
			PostId = new Guid(Request["id"]);

			base.OnActionExecuting(filterContext);
		}
	}
}
