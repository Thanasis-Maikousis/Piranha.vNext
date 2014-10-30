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
using Piranha.Manager.Models.Post;

namespace Piranha.Areas.Manager.Controllers
{
	/// <summary>
	/// Controller for managing posts.
	/// </summary>
	[Authorize]
	[RouteArea("Manager", AreaPrefix="manager")]
    public class PostMgrController : ManagerController
    {
		/// <summary>
		/// Gets a list of the currently available posts
		/// </summary>
		/// <returns></returns>
		[Route("posts/{slug?}")]
		public ActionResult List(string slug = null) {
			return View(ListModel.Get(slug));
		}

		[Route("post/add/{type}")]
		public ActionResult Add(string type) {
			ViewBag.Title = Piranha.Manager.Resources.Post.AddTitle;
			return View("Edit", new EditModel(api, type));
		}

		/// <summary>
		/// Gets the edit view for a new or existing post.
		/// </summary>
		/// <param name="id">The optional id</param>
		/// <returns>The view result</returns>
		[Route("post/edit/{id:Guid}")]
		public ActionResult Edit(Guid id) {
			ViewBag.Title = Piranha.Manager.Resources.Post.EditTitle;
			return View(EditModel.GetById(api, id));
		}

		/// <summary>
		/// Saves the given post model.
		/// </summary>
		/// <param name="model">The model</param>
		/// <returns>The redirect result</returns>
		[HttpPost]
		[Route("post/save")]
		[ValidateInput(false)]
		[ValidateAntiForgeryToken]
		public ActionResult Save(EditModel model) {
			if (ModelState.IsValid) {
				model.Save(api);
				return RedirectToAction("Edit", new { id = model.Id });
			}
			if (model.Id.HasValue)
				ViewBag.Title = Piranha.Manager.Resources.Post.EditTitle;
			else ViewBag.Title = Piranha.Manager.Resources.Post.AddTitle;

			return View("Edit", model);
		}

		/// <summary>
		/// Deletes the post with the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <returns>The redirect result</returns>
		[Route("post/delete/{id:Guid}")]
		public ActionResult Delete(Guid id) {
			var post = api.Posts.GetSingle(where: t => t.Id == id);
			if (post != null) {
				api.Posts.Remove(post);
				api.SaveChanges();
			}
			return RedirectToAction("List");
		}

		/// <summary>
		/// Changes the approval status for the specifed comment.
		/// </summary>
		/// <param name="state">The status</param>
		/// <returns>The partial comment list.</returns>
		[HttpPost]
		[Route("post/comment/approve")]
		public ActionResult CommentApprove(CommentState state) {
			var comment = api.Comments.GetSingle(where: c => c.Id == state.CommentId && c.PostId == state.PostId);

			if (comment != null) {
				comment.IsApproved = state.Status;

				api.Comments.Add(comment);
				api.SaveChanges();
			}
			return View("Partial/CommentList", api.Comments.Get(where: c => c.PostId == state.PostId)
				.Select(c => new Piranha.Manager.Models.Post.EditModel.CommentListItem() {
					Author = c.Author,
					Body = c.Body,
					Created = c.Created,
					Email = c.Email,
					Id = c.Id,
					IsApproved = c.IsApproved,
					IsSpam = c.IsSpam,
					WebSite = c.WebSite
				}));
		}

		/// <summary>
		/// Changes the spam status for the specifed comment.
		/// </summary>
		/// <param name="state">The status</param>
		/// <returns>The partial comment list.</returns>
		[HttpPost]
		[Route("post/comment/spam")]
		public ActionResult CommentSpam(CommentState state) {
			var comment = api.Comments.GetSingle(where: c => c.Id == state.CommentId && c.PostId == state.PostId);

			if (comment != null) {
				comment.IsSpam = state.Status;

				api.Comments.Add(comment);
				api.SaveChanges();
			}
			return View("Partial/CommentList", api.Comments.Get(where: c => c.PostId == state.PostId)
				.Select(c => new Piranha.Manager.Models.Post.EditModel.CommentListItem() {
					Author = c.Author,
					Body = c.Body,
					Created = c.Created,
					Email = c.Email,
					Id = c.Id,
					IsApproved = c.IsApproved,
					IsSpam = c.IsSpam,
					WebSite = c.WebSite
				}));
		}
	}
}