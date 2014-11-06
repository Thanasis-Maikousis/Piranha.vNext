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

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Piranha.Manager.Models.Category;

namespace Piranha.Areas.Manager.Controllers
{
	/// <summary>
	/// Controller for managing categories.
	/// </summary>
	[Authorize]
	[RouteArea("Manager", AreaPrefix = "manager")]
    public class CategoryMgrController : ManagerController
    {
		/// <summary>
		/// Gets a list of the currently available categories.
		/// </summary>
		/// <returns>The category list</returns>
		[Route("categories")]
		public ActionResult List() {
			return View();
		}

		/// <summary>
		/// Gets the categories.
		/// </summary>
		/// <returns>The categories</returns>
		[Route("categories/get")]
		public ActionResult Get() {
			return JsonData(true, Mapper.Map<IEnumerable<Piranha.Models.Category>, IEnumerable<ListItem>>(api.Categories.Get()));
		}

		/// <summary>
		/// Gets the category with the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <returns>The category</returns>
		[Route("category/get/{id:Guid}")]
		public ActionResult GetSingle(Guid id) {
			return JsonData(true, EditModel.GetById(api, id));
		}

		/// <summary>
		/// Saves the given category model.
		/// </summary>
		/// <param name="model">The model</param>
		/// <returns>The redirect result</returns>
		[HttpPost]
		[Route("category/save")]
		[ValidateInput(false)]
		public ActionResult Save(EditModel model) {
			if (ModelState.IsValid) {
				model.Save(api);
				return JsonData(true, Mapper.Map<IEnumerable<Piranha.Models.Category>, IEnumerable<ListItem>>(api.Categories.Get()));
			}
			return JsonData(false);
		}

		/// <summary>
		/// Deletes the category with the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <returns>The redirect result</returns>
		[Route("category/delete/{id:Guid}")]
		public ActionResult Delete(Guid id) {
			var category = api.Categories.GetSingle(id);
			if (category != null) {
				api.Categories.Remove(category);
				api.SaveChanges();
				return JsonData(true, Mapper.Map<IEnumerable<Piranha.Models.Category>, IEnumerable<ListItem>>(api.Categories.Get()));
			}
			return JsonData(false);
		}
    }
}