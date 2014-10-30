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
using System.Text;

namespace Piranha.Web
{
	/// <summary>
	/// Class for handling the result of a routing request.
	/// </summary>
	public class RouteResult
	{
		#region Properties
		/// <summary>
		/// Gets if the current result was handled.
		/// </summary>
		public bool Handled { get; private set; }

		/// <summary>
		/// Gets if the request should be rewritten to the provided url.
		/// </summary>
		public bool Rewrite { get; private set; }

		/// <summary>
		/// Gets the route to rewrite to.
		/// </summary>
		public string Route { get; private set; }

		/// <summary>
		/// Gets the query params.
		/// </summary>
		public Tuple<string, string>[] Params { get; private set; }
		#endregion

		/// <summary>
		/// Default internal constructor.
		/// </summary>
		/// <param name="handled">If it was a success</param>
		/// <param name="rewrite">If the response should be rewritten</param>
		/// <param name="route">The current route</param>
		/// <param name="query">Optional query params</param>
		public RouteResult(bool handled, bool rewrite = false, string route = null, params Tuple<string, string>[] query) {
			Handled = handled;
			Rewrite = rewrite;
			Route = route;
			Params = query;
		}

		/// <summary>
		/// Renders a rewrite path from the current result.
		/// </summary>
		/// <returns>The string</returns>
		public override string ToString() {
			var sb = new StringBuilder();

			if (Route[0] != '~')
				sb.Append("~/");
			sb.Append(Route);

			for (var n = 0; n < Params.Length; n++) {
				if (n == 0)
					sb.Append("?");
				else sb.Append("&");

				sb.Append(Params[n].Item1);
				sb.Append("=");
				sb.Append(Params[n].Item2);
			}
			return sb.ToString();
		}
	}
}
