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

namespace Piranha.Web.Handlers
{
	public sealed class AliasHandler : IHandler
	{
		/// <summary>
		/// Tries to handle an incoming request.
		/// </summary>
		/// <param name="api">The current api</param>
		/// <param name="request">The incoming route request</param>
		/// <returns>The result</returns>
		public RouteResult Handle(Api api, RouteRequest request) {
			var context = request.HttpContext;

			// First try to get the alias for the complete url
			var alias = api.Aliases.GetSingle(context.Request.RawUrl);

			if (alias == null) {
				if (!String.IsNullOrWhiteSpace(context.Request.Url.Query)) {
					// Try to get alias for the url without query
					alias = api.Aliases.GetSingle(context.Request.Url.AbsolutePath);

					if (alias != null) {
						if (alias.IsPermanent)
							context.Response.RedirectPermanent(alias.NewUrl + context.Request.Url.Query);
						else context.Response.Redirect(alias.NewUrl + context.Request.Url.Query);

						return new RouteResult(true);
					}
				}
			} else {
				if (alias.IsPermanent)
					context.Response.RedirectPermanent(alias.NewUrl);
				else context.Response.Redirect(alias.NewUrl);

				return new RouteResult(true);
			}
			return new RouteResult(false);
		}
	}
}
