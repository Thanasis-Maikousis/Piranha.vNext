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

namespace Piranha.Hooks
{
	/// <summary>
	/// The main application hooks available.
	/// </summary>
	public static class App
	{
		/// <summary>
		/// The different delegates used by the app hooks.
		/// </summary>
		public static class Delegates
		{
			/// <summary>
			/// Delegate for request hooks.
			/// </summary>
			/// <param name="context">The current http context</param>
			public delegate void RequestDelegate(Server.IRequest request);

			/// <summary>
			/// Delegate for request errors.
			/// </summary>
			/// <param name="context">The current http context</param>
			public delegate void ErrorDelegate(Server.IRequest context, Exception e);
		}

		/// <summary>
		/// The hooks available for the http request.
		/// </summary>
		public static class Request
		{
			/// <summary>
			/// Called when the request begins before the Piranha module.
			/// </summary>
			public static Delegates.RequestDelegate OnBeginRequest;

			/// <summary>
			/// Called when the request ends after the Piranha module.
			/// </summary>
			public static Delegates.RequestDelegate OnEndRequest;

			/// <summary>
			/// Called when an unhandled exception occurs in an application request.
			/// </summary>
			public static Delegates.ErrorDelegate OnError;
		}
	}
}
