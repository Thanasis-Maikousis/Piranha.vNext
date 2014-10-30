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
using Piranha.Manager.Models.Account;

namespace Piranha.Areas.Manager.Controllers
{
	/// <summary>
	/// Manager controller for authentication.
	/// </summary>
	[RouteArea("Manager", AreaPrefix = "manager")]
	public class AccountMgrController : Controller
	{
		/// <summary>
		/// Get the login view.
		/// </summary>
		/// <returns>The view result</returns>
		[HttpGet]
		[Route("login")]
		public ActionResult Login() {
			return View();
		}

		/// <summary>
		/// Signs in the user with the given credentials.
		/// </summary>
		/// <param name="m">The login model</param>
		/// <returns>The redirect result</returns>
		[HttpPost]
		[Route("login")]
		[ValidateAntiForgeryToken]
		public ActionResult LoginUser(LoginModel m) {
			if (ModelState.IsValid) {
				if (App.Security.SignIn(m.Username, m.Password))
					return RedirectToAction("List", "PostMgr");
			}
			return RedirectToAction("Login");
		}

		/// <summary>
		/// Signs out the currently authenticated user.
		/// </summary>
		/// <returns>The redirect result</returns>
		[HttpGet]
		[Route("logout")]
		public ActionResult LogoutUser() {
			App.Security.SignOut();
			return RedirectToAction("Login");
		}
	}
}