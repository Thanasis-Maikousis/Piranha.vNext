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

namespace Piranha.Server.Routing
{
	/// <summary>
	/// Main router that handles the incoming requests.
	/// </summary>
	public static class Router
	{
		/// <summary>
		/// Executed on the beginning of each request.
		/// </summary>
		/// <param name="request">The current request</param>
		public static void OnBeginRequest(IRequest request) {
			IResponse response = null;

			if (!request.RawUrl.StartsWith("/__browserLink/")) {
				using (var api = new Api()) {
					if (request.Segments.Length == 0) {
						// Handle startpage
						response = App.Handlers.Pages.Handle(api, request);
					} else {
						// Handle alias redirects
						response = App.Handlers.Aliases.Handle(api, request);

						// Handle request by keyword
						if (response == null) {
							var handler = App.Handlers[request.Segments[0]];
							if (handler != null)
								response = handler.Handle(api, request);
						}

						// Handle posts
						if (response == null)
							response = App.Handlers.Posts.Handle(api, request);

						// Handle pages
						if (response == null)
							response = App.Handlers.Pages.Handle(api, request);
					}
				}

				// Execute the response
				if (response != null)
					response.Execute();
			}
		}
	}
}
