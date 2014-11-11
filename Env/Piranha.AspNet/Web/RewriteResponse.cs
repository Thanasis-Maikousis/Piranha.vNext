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
using System.Text;
using System.Web;
using Piranha.Server;

namespace Piranha.AspNet.Web
{
	/// <summary>
	/// Response for rewriting the application request to a new path.
	/// </summary>
	public class RewriteResponse : IRewriteResponse
	{
		#region Members
		private readonly HttpContext context;
		#endregion

		#region Properties
		/// <summary>
		/// Gets/sets the internal route to rewrite the request to.
		/// </summary>
		public string Route { get; set; }

		/// <summary>
		/// Gets/sets the optional params.
		/// </summary>
		public Param[] Params { get; set; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="context">The current context</param>
		public RewriteResponse(HttpContext context) {
			this.context = context;

			Params = new Param[0];
		}

		/// <summary>
		/// Executes the current response.
		/// </summary>
		public void Execute() {
			var sb = new StringBuilder();

			if (Route[0] != '~')
				sb.Append("~/");
			sb.Append(Route);

			for (var n = 0; n < Params.Length; n++) {
				if (n == 0)
					sb.Append("?");
				else sb.Append("&");

				sb.Append(Params[n].Key);
				sb.Append("=");
				sb.Append(Params[n].Value);
			}
			context.RewritePath(sb.ToString(), true);
		}
	}
}
