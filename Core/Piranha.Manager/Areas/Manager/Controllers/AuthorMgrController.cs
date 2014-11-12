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
using Piranha.Manager.Models.Author;

namespace Piranha.Areas.Manager.Controllers
{
	/// <summary>
	/// Controller for managing blocks.
	/// </summary>
	[Authorize]
	[RouteArea("Manager", AreaPrefix = "manager")]
	public class AuthorMgrController : ManagerController
	{
		/// <summary>
		/// Gets a list of the currently available authors
		/// </summary>
		/// <returns>The author list</returns>
		[Route("authors")]
		public ActionResult List() {
			return View();
		}

		/// <summary>
		/// Gets the authors.
		/// </summary>
		/// <returns>The authors</returns>
		[Route("authors/get")]
		public ActionResult Get() {
			var list = Mapper.Map<IEnumerable<Piranha.Models.Author>, IEnumerable<ListItem>>(api.Authors.Get());
			var ui = new Web.Helpers.UIHelper();

			foreach (var item in list) {
				item.GravatarUrl = ui.GravatarUrl(item.Email, 40).ToHtmlString();
			}
			return JsonData(true, list);
		}

		/// <summary>
		/// Gets the author with the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <returns>The author</returns>
		[Route("author/get/{id:Guid}")]
		public ActionResult GetSingle(Guid id) {
			return JsonData(true, EditModel.GetById(api, id));
		}

		/// <summary>
		/// Saves the given post model.
		/// </summary>
		/// <param name="model">The model</param>
		/// <returns>The redirect result</returns>
		[HttpPost]
		[Route("author/save")]
		[ValidateInput(false)]
		public ActionResult Save(EditModel model) {
			if (ModelState.IsValid) {
				model.Save(api);
				return JsonData(true, Mapper.Map<IEnumerable<Piranha.Models.Author>, IEnumerable<ListItem>>(api.Authors.Get()));
			}
			return JsonData(false);
		}

		/// <summary>
		/// Deletes the block with the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <returns>The redirect result</returns>
		[Route("author/delete/{id:Guid}")]
		public ActionResult Delete(Guid id) {
			var author = api.Authors.GetSingle(where: a => a.Id == id);
			if (author != null) {
				api.Authors.Remove(author);
				api.SaveChanges();
				return JsonData(true, Mapper.Map<IEnumerable<Piranha.Models.Author>, IEnumerable<ListItem>>(api.Authors.Get()));
			}
			return JsonData(false);
		}
	}
}