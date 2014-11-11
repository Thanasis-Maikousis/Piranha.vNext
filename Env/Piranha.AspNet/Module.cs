/*
 * Copyright (c) 2014 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * http://github.com/piranhacms/piranha.vnext
 * 
 */

using System;
using System.Web;

namespace Piranha.AspNet
{
	/// <summary>
	/// The main Http module for the ASP.Net runtime.
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
					Hooks.App.Request.OnBeginRequest(new Web.Request(((HttpApplication)sender).Context));
			};

			// Register end request
			context.EndRequest += (sender, e) => { 
				if (Hooks.App.Request.OnEndRequest != null)
					Hooks.App.Request.OnEndRequest(new Web.Request(((HttpApplication)sender).Context));
			};

			// Register error event
			context.Error += (sender, e) => {
				var exception = ((HttpApplication)sender).Context.Server.GetLastError();

				App.Logger.Log(Log.LogLevel.ERROR, "HttpApplication.Error: Unhandled exception.", exception);

				if (Hooks.App.Request.OnError != null)
					Hooks.App.Request.OnError(new Web.Request(((HttpApplication)sender).Context), exception);
			};
		}
	}
}
