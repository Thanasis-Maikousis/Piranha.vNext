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
using Piranha.Manager.Models.PostType;

namespace Piranha.Areas.Manager.Controllers
{
	/// <summary>
	/// Controller for managing post types.
	/// </summary>
	[Authorize]
	[RouteArea("Manager", AreaPrefix="manager")]
	public class PostTypeMgrController : ManagerController
	{
		/// <summary>
		/// Gets the list of the currently available post types.
		/// </summary>
		/// <returns>The view result</returns>
		[Route("posttypes")]
		public ActionResult List() {
			return View(ListModel.Get());
		}

		/// <summary>
		/// Gets the edit view for a new or existing post type.
		/// </summary>
		/// <param name="id">The optional id</param>
		/// <returns>The view result</returns>
		[Route("posttype/edit/{id:Guid?}")]
		public ActionResult Edit(Guid? id = null) {
			if (id.HasValue) {
				ViewBag.Title = Piranha.Manager.Resources.PostType.EditTitle;
				return View(EditModel.GetById(api, id.Value));
			} else {
				ViewBag.Title = Piranha.Manager.Resources.PostType.AddTitle;
				return View(new EditModel());
			}
		}

		/// <summary>
		/// Saves the given post type.
		/// </summary>
		/// <param name="model">The post type</param>
		/// <returns>The view result</returns>
		[HttpPost]
		[Route("posttype/save")]
		[ValidateAntiForgeryToken]
		public ActionResult Save(EditModel model) {
			if (ModelState.IsValid) {
				model.Save(api);
				return RedirectToAction("edit", new { id = model.Id });
			}
			if (model.Id.HasValue)
				ViewBag.Title = Piranha.Manager.Resources.PostType.EditTitle;
			else ViewBag.Title = Piranha.Manager.Resources.PostType.AddTitle;

			return View(model);
		}

		/// <summary>
		/// Deletes the post type with the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <returns>The redirect result</returns>
		[Route("posttype/delete/{id:Guid}")]
		public ActionResult Delete(Guid id) {
			var type = api.PostTypes.GetSingle(where: t => t.Id == id);
			if (type != null) {
				api.PostTypes.Remove(type);
				api.SaveChanges();
			}
			return RedirectToAction("List");
		}
	}
}