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
using System.Collections.Generic;
using System.Web;

namespace Piranha.Web
{
	/// <summary>
	/// Class representing an incoming request to Piranha.
	/// </summary>
	public sealed class RouteRequest
	{
		#region Properties
		/// <summary>
		/// Gets the url segments.
		/// </summary>
		public string[] Segments { get; private set; }

		/// <summary>
		/// Gets the query params.
		/// </summary>
		public Tuple<string, string>[] Params { get; private set; }

		/// <summary>
		/// Gets the current http context.
		/// </summary>
		public HttpContext HttpContext { get; private set; }
		#endregion

		/// <summary>
		/// Default internal constructor.
		/// </summary>
		/// <param name="context">The current context</param>
		public RouteRequest(HttpContext context) {
			// Store the http request
			HttpContext = context;

			var param = new List<Tuple<string, string>>();

			// Get the current path
			string path = context.Request.Path.Substring(context.Request.ApplicationPath.Length > 1 ?
				context.Request.ApplicationPath.Length : 0).ToLower();

			Segments = path.Split(new char[] { '/' }).Subset(1);

			foreach (var key in context.Request.QueryString.AllKeys)
				param.Add(new Tuple<string, string>(key, context.Request.Params[key]));
			Params = param.ToArray();
		}
	}
}
