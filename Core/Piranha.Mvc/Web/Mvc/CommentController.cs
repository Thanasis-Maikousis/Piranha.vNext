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
	/// Base controller for managin comments.
	/// </summary>
	public class CommentController : Controller
	{
		/// <summary>
		/// Adds a comment to a post.
		/// </summary>
		/// <param name="model">The comment</param>
		/// <returns>A redirect result</returns>
		public virtual ActionResult Add(Piranha.Models.Comment model) {
			if (ModelState.IsValid) {
				using (var api = new Api()) {
					model.IP = HttpContext.Request.UserHostAddress;
					model.UserAgent = HttpContext.Request.UserAgent.Substring(0, Math.Min(HttpContext.Request.UserAgent.Length, 128));
					model.SessionID = Session.SessionID;
					model.IsApproved = true;
					if (User.Identity.IsAuthenticated)
						model.UserId = User.Identity.Name;

					if (User.Identity.IsAuthenticated && Config.Comments.ModerateAuthorized)
						model.IsApproved = false;
					else if (!User.Identity.IsAuthenticated && Config.Comments.ModerateAnonymous)
						model.IsApproved = false;

					api.Comments.Add(model);
					api.SaveChanges();
				}
				var post = Models.PostModel.GetById(model.PostId);

				return Redirect("~/" + post.Type + "/" + post.Slug);
			}
			return null;
		}
	}
}
