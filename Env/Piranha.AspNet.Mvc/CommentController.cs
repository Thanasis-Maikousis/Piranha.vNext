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
				var post = PostModel.GetById(model.PostId);

				return Redirect("~/" + post.Type + "/" + post.Slug);
			}
			return null;
		}
	}
}
