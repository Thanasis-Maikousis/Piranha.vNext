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
using Piranha.Manager.Models.Block;

namespace Piranha.Areas.Manager.Controllers
{
	/// <summary>
	/// Controller for managing blocks.
	/// </summary>
	[Authorize]
	[RouteArea("Manager", AreaPrefix="manager")]
    public class BlockMgrController : ManagerController
    {
		/// <summary>
		/// Gets a list of the currently available blocks
		/// </summary>
		/// <returns>The block list</returns>
		[Route("blocks")]
		public ActionResult List() {
			return View(api.Blocks.Get());
		}

		/// <summary>
		/// Gets the edit view for a new block.
		/// </summary>
		/// <returns>The view result</returns>
		[Route("block/add")]
		public ActionResult Add() {
			ViewBag.Title = Piranha.Manager.Resources.Block.AddTitle;
			return View("Edit", new EditModel());
		}

		/// <summary>
		/// Gets the edit view for an existing block.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <returns>The view result</returns>
		[Route("block/edit/{id:Guid}")]
		public ActionResult Edit(Guid id) {
			ViewBag.Title = Piranha.Manager.Resources.Block.EditTitle;
			return View(EditModel.GetById(api, id));
		}

		/// <summary>
		/// Saves the given post model.
		/// </summary>
		/// <param name="model">The model</param>
		/// <returns>The redirect result</returns>
		[HttpPost]
		[Route("block/save")]
		[ValidateInput(false)]
		[ValidateAntiForgeryToken]
		public ActionResult Save(EditModel model) {
			if (ModelState.IsValid) {
				model.Save(api);
				return RedirectToAction("Edit", new { id = model.Id });
			}
			if (model.Id.HasValue)
				ViewBag.Title = Piranha.Manager.Resources.Block.EditTitle;
			else ViewBag.Title = Piranha.Manager.Resources.Block.AddTitle;

			return View(model);
		}

		/// <summary>
		/// Deletes the block with the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <returns>The redirect result</returns>
		[Route("block/delete/{id:Guid}")]
		public ActionResult Delete(Guid id) {
			var block = api.Blocks.GetSingle(where: t => t.Id == id);
			if (block != null) {
				api.Blocks.Remove(block);
				api.SaveChanges();
			}
			return RedirectToAction("List");
		}
	}
}