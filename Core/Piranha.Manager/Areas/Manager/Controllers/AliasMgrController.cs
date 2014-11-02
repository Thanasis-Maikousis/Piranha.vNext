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
using Piranha.Manager.Models.Alias;

namespace Piranha.Areas.Manager.Controllers
{
	/// <summary>
	/// Controller for managing aliases.
	/// </summary>
	[Authorize]
	[RouteArea("Manager", AreaPrefix = "manager")]
    public class AliasMgrController : ManagerController
    {
		/// <summary>
		/// Gets a list of the currently available aliases.
		/// </summary>
		/// <returns>The alias list</returns>
		[Route("aliases")]
		public ActionResult List() {
			return View();
		}

		/// <summary>
		/// Gets the aliases.
		/// </summary>
		/// <returns>The aliases</returns>
		[Route("aliases/get")]
		public ActionResult Get() {
			return JsonData(true, Mapper.Map<IEnumerable<Piranha.Models.Alias>, IEnumerable<ListItem>>(api.Aliases.Get()));
		}

		/// <summary>
		/// Gets the alias with the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <returns>The alias</returns>
		[Route("alias/get/{id:Guid}")]
		public ActionResult GetSingle(Guid id) {
			return JsonData(true, EditModel.GetById(api, id));
		}

		/// <summary>
		/// Saves the given post model.
		/// </summary>
		/// <param name="model">The model</param>
		/// <returns>The redirect result</returns>
		[HttpPost]
		[Route("alias/save")]
		[ValidateInput(false)]
		public ActionResult Save(EditModel model) {
			if (ModelState.IsValid) {
				model.Save(api);
				return JsonData(true, Mapper.Map<IEnumerable<Piranha.Models.Alias>, IEnumerable<ListItem>>(api.Aliases.Get()));
			}
			return JsonData(false);
		}

		/// <summary>
		/// Deletes the alias with the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <returns>The redirect result</returns>
		[Route("alias/delete/{id:Guid}")]
		public ActionResult Delete(Guid id) {
			var alias = api.Aliases.GetSingle(id);
			if (alias != null) {
				api.Aliases.Remove(alias);
				api.SaveChanges();
				return JsonData(true, Mapper.Map<IEnumerable<Piranha.Models.Alias>, IEnumerable<ListItem>>(api.Aliases.Get()));
			}
			return JsonData(false);
		}
    }
}