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
using System.Collections.Generic;
using System.Web;
using Piranha.Server;

namespace Piranha.AspNet.Web
{
	/// <summary>
	/// The request object for the ASP.NET runtime.
	/// </summary>
	public class Request : IRequest
	{
		#region Members
		private readonly HttpContext context;
		#endregion

		#region Properties
		public string RawUrl { get; private set; }
		public string Path { get; private set; }
		public string Query { get; private set; }
		public string[] Segments { get; private set; }
		public Param[] Params { get; private set; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="context">The current context</param>
		public Request(HttpContext context) {
			// Get the context
			this.context = context;

			// Get the raw url
			RawUrl = context.Request.RawUrl;

			// Get the path
			Path = context.Request.Url.AbsolutePath;

			// Get the query
			Query = context.Request.Url.Query;

			// Get the segments
			string path = context.Request.Path.Substring(context.Request.ApplicationPath.Length > 1 ?
				context.Request.ApplicationPath.Length : 0).ToLower();
			Segments = path.Split(new char[] { '/' }).Subset(1);

			if (Segments.Length > 0) {
				if (String.IsNullOrWhiteSpace(Segments[Segments.Length - 1]))
					Segments = Segments.Subset(0, Segments.Length - 1);
			}

			// Get the optional params
			var param = new List<Param>();
			foreach (var key in context.Request.QueryString.AllKeys) {
				param.Add(new Param() { 
					Key = key, 
					Value = context.Request.Params[key] 
				});
			}
			Params = param.ToArray();
		}

		/// <summary>
		/// Creates a new redirect response for the current request.
		/// </summary>
		/// <returns>The response</returns>
		public IRedirectResponse RedirectResponse() {
			return new RedirectResponse(context);
		}

		/// <summary>
		/// Creates a new rewrite response for the current request.
		/// </summary>
		/// <returns>The response</returns>
		public IRewriteResponse RewriteResponse() {
			return new RewriteResponse(context);
		}

		/// <summary>
		/// Creates a new stream response for the current request.
		/// </summary>
		/// <returns>The response</returns>
		public IStreamResponse StreamResponse() {
			return new StreamResponse(context);
		}
	}
}
