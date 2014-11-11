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
using Piranha.Server;

namespace Piranha.AspNet.Web
{
	/// <summary>
	/// Response for redirecting the request to a new path.
	/// </summary>
	public class RedirectResponse : IRedirectResponse
	{
		#region Members
		private readonly HttpContext context;
		#endregion

		#region Properties
		/// <summary>
		/// Gets/sets the redirect url.
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Gets/sets if this redirect is permanent.
		/// </summary>
		public bool IsPermanent { get; set; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="context">The current context</param>
		public RedirectResponse(HttpContext context) {
			this.context = context;
		}

		/// <summary>
		/// Executes the current response.
		/// </summary>
		public void Execute() {
			if (IsPermanent)
				context.Response.RedirectPermanent(Url, true);
			else context.Response.Redirect(Url, true);
		}
	}
}
