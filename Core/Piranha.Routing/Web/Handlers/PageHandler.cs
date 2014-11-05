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

namespace Piranha.Web.Handlers
{
	/// <summary>
	/// Handler for routing requests for pages.
	/// </summary>
	public class PageHandler : IHandler
	{
		/// <summary>
		/// Tries to handle an incoming request.
		/// </summary>
		/// <param name="api">The current api</param>
		/// <param name="request">The incoming route request</param>
		/// <returns>The result</returns>
		public RouteResult Handle(Api api, RouteRequest request) {
			var slug = request.Segments.Length > 0 ? request.Segments[0] : "";
			var now = DateTime.Now.ToUniversalTime();
			var route = "";

			slug = slug != "" ? slug : 
				api.Pages.GetSingle(where: p => !p.ParentId.HasValue && p.SortOrder == 1 && p.Published <= now).Slug;

			// Get startpage or by slug.
			var page = Models.PageModel.GetBySlug(slug);

			if (page != null) {
				route = !String.IsNullOrWhiteSpace(page.Route) ? page.Route : "page";

				// Append extra url segments
				for (var n = 1; n < request.Segments.Length; n++) {
					route += "/" + request.Segments[n];
				}

				// Set current
				request.HttpContext.SetCurrent(new Models.Content() {
					Id = page.Id,
					Title = page.Title,
					Keywords = page.Keywords,
					Description = page.Description,
					VirtualPath = "~/" + page.Slug,
					Type = !page.ParentId.HasValue && page.SortOrder == 1 ? Models.ContentType.Start : Models.ContentType.Page
				});

				return new RouteResult(true, true, route, request.Params.Concat(new Tuple<string, string>[] { 
					new Tuple<string, string>("id", page.Id.ToString())
				}).ToArray());
			}
			return new RouteResult(false);
		}
	}
}
