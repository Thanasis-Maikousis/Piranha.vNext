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

namespace Piranha.Server.Handlers
{
	/// <summary>
	/// Handler for routing requests for aliases.
	/// </summary>
	public sealed class AliasHandler : IHandler
	{
		/// <summary>
		/// Tries to handle an incoming request.
		/// </summary>
		/// <param name="api">The current api</param>
		/// <param name="request">The incoming route request</param>
		/// <returns>The result</returns>
		public IResponse Handle(Api api, IRequest request) {
			//var context = request.HttpContext;

			// First try to get the alias for the complete url
			var alias = api.Aliases.GetSingle(request.RawUrl);

			if (alias == null) {
				if (request.Params.Length > 0) {
					// Try to get alias for the url without query
					alias = api.Aliases.GetSingle(request.Path);

					if (alias != null) {
						var response = request.RedirectResponse();

						response.IsPermanent = alias.IsPermanent;
						response.Url = alias.NewUrl + request.Query;

						return response;
					}
				}
			} else {
				var response = request.RedirectResponse();

				response.IsPermanent = alias.IsPermanent;
				response.Url = alias.NewUrl;

				return response;
			}
			return null;
		}
	}
}
