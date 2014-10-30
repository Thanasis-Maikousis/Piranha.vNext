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

namespace Piranha.Security
{
	/// <summary>
	/// Interface for the security manager.
	/// </summary>
	public interface ISecurity
	{
		/// <summary>
		/// Authenticates the given user credentials without
		/// signing in the user.
		/// </summary>
		/// <param name="username">The user</param>
		/// <param name="password">The password</param>
		/// <returns>If the credentials was authenticated successfully</returns>
		bool Authenticate(string username, string password);

		/// <summary>
		/// Signs in the user with the given username and password.
		/// </summary>
		/// <param name="username">The username</param>
		/// <param name="password">The password</param>
		/// <returns>If the user was signed in</returns>
		bool SignIn(string username, string password);

		/// <summary>
		/// Signs out the currently authenticated user.
		/// </summary>
		void SignOut();

		/// <summary>
		/// Checks if the current user is authenticated.
		/// </summary>
		/// <returns>If the user is authenticated</returns>
		bool IsAuthenticated();

		/// <summary>
		/// Checks if the current user has the given role.
		/// </summary>
		/// <param name="role">The role</param>
		/// <returns>If the user has the role</returns>
		bool IsInRole(string role);

		/// <summary>
		/// Gets the user id of the currently authenticated user.
		/// </summary>
		/// <returns>The user id</returns>
		string GetUserId();
	}
}
