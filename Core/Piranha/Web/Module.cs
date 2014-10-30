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
using System.Web;

namespace Piranha.Web
{
	/// <summary>
	/// The main Http module for Piranha.
	/// </summary>
	public sealed class Module : IHttpModule
	{
		#region Members
		private const string POST_ROUTE = "~/{0}?type={1}&slug={2}";
		private const string ARCHIVE_ROUTE = "~/{0}?type={1}&year={2}&month={3}";
		#endregion

		/// <summary>
		/// Disposes the http module.
		/// </summary>
		public void Dispose() {
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Initializes the goldfish http module.
		/// </summary>
		/// <param name="context">The application context</param>
		public void Init(HttpApplication context) {
			// Register begin request 
			context.BeginRequest += (sender, e) => {
				if (Hooks.App.Request.OnBeginRequest != null)
					Hooks.App.Request.OnBeginRequest(((HttpApplication)sender).Context);

				// Call the main entry point for Piranha
				BeginRequest(((HttpApplication)sender).Context);
			};

			// Register end request
			context.EndRequest += (sender, e) => { 
				if (Hooks.App.Request.OnEndRequest != null)
					Hooks.App.Request.OnEndRequest(((HttpApplication)sender).Context);
			};

			// Register error event
			context.Error += (sender, e) => {
				var exception = ((HttpApplication)sender).Context.Server.GetLastError();

				App.Logger.Log(Log.LogLevel.ERROR, "HttpApplication.Error: Unhandled exception.", exception);

				if (Hooks.App.Request.OnError != null)
					Hooks.App.Request.OnError(((HttpApplication)sender).Context, exception);
			};
		}

		#region Private methods
		/// <summary>
		/// Handles a request to Piranha.
		/// </summary>
		/// <param name="context">The current http context</param>
		private void BeginRequest(HttpContext context) {
			var result = new RouteResult(false);


			if (!context.Request.RawUrl.StartsWith("/__browserLink/")) {
				using (var api = new Api()) {
					var request = new RouteRequest(context);

					if (request.Segments.Length == 0) {
						// Handle startpage
						result = App.Handlers.Pages.Handle(api, request);
					} else {
						// Handle alias redirects
						result = App.Handlers.Aliases.Handle(api, request);

						// Handle request by keyword
						if (!result.Handled) {
							var handler = App.Handlers[request.Segments[0]];
							if (handler != null)
								result = handler.Handle(api, request);
						}

						// Handle posts
						if (!result.Handled)
							result = App.Handlers.Posts.Handle(api, request);

						// Handle pages
						if (!result.Handled)
							result = App.Handlers.Pages.Handle(api, request);
					}
				}

				// Check if we should rewrite the request
				if (result.Handled) {
					if (result.Rewrite)
						context.RewritePath(result.ToString(), true);
					else context.Response.End();
				}
			}
		}
		#endregion
	}
}
