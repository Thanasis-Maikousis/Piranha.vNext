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
using System.Web;
using System.Web.Security;

namespace Piranha.Security
{
	/// <summary>
	/// Simple security provider using the simplest forms security
	/// possible.
	/// </summary>
	public sealed class SimpleSecurity : ISecurity
	{
		#region Members
		private static string adminUserId;
		private readonly string adminUsername;
		private readonly string adminPassword;
		private object mutex = new Object();
		#endregion

		/// <summary>
		/// Default constructor. Crates a new forms security object.
		/// </summary>
		/// <param name="username">The admin username</param>
		/// <param name="password">The admin password</param>
		public SimpleSecurity(string username, string password) {
			// Generate user id
			if (String.IsNullOrEmpty(adminUserId)) {
				lock (mutex) {
					if (String.IsNullOrEmpty(adminUserId))
						adminUserId = Guid.NewGuid().ToString();
				}
			}

			// Set username & password
			adminUsername = username;
			adminPassword = password;
		}

		/// <summary>
		/// Authenticates the given user credentials without
		/// signing in the user.
		/// </summary>
		/// <param name="username">The user</param>
		/// <param name="password">The password</param>
		/// <returns>If the credentials was authenticated successfully</returns>
		public bool Authenticate(string username, string password) {
			return username == adminUsername && password == adminPassword;
		}

		/// <summary>
		/// Signs in the user with the given username and password.
		/// </summary>
		/// <param name="username">The username</param>
		/// <param name="password">The password</param>
		/// <returns>If the user was signed in</returns>
		public bool SignIn(string username, string password) {
			if (Authenticate(username, password)) {
				FormsAuthentication.SetAuthCookie(adminUserId, false);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Signs out the currently authenticated user.
		/// </summary>
		public void SignOut() {
			FormsAuthentication.SignOut();
		}

		/// <summary>
		/// Checks if the current user is authenticated.
		/// </summary>
		/// <returns>If the user is authenticated</returns>
		public bool IsAuthenticated() {
			return HttpContext.Current.User.Identity.IsAuthenticated;
		}

		/// <summary>
		/// Checks if the current user has the given role.
		/// </summary>
		/// <param name="role">The role</param>
		/// <returns>If the user has the role</returns>
		public bool IsInRole(string role) {
			if (IsAuthenticated() && HttpContext.Current.User.Identity.Name == adminUserId)
				return true;
			return false;
		}

		/// <summary>
		/// Gets the user id of the currently authenticated user.
		/// </summary>
		/// <returns>The user id</returns>
		public string GetUserId() {
			if (IsAuthenticated())
				return adminUserId;
			return null;
		}
	}
}
