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
using Piranha.Client.Models;

namespace Piranha.AspNet
{
	/// <summary>
	/// Runtime environment for ASP.Net
	/// </summary>
	public class Env : IEnv
	{
		/// <summary>
		/// Gets/sets the current item being processed.
		/// </summary>
		public Content GetCurrent() {
			return (Content)HttpContext.Current.Items["PiranhaCurrent"];
		}

		/// <summary>
		/// Gets/sets the current item being processed.
		/// </summary>
		public void SetCurrent(Content current) {
			HttpContext.Current.Items["PiranhaCurrent"] = current;
		}

		/// <summary>
		/// Generates an absolute url from the given virtual path.
		/// </summary>
		/// <param name="virtualpath">The virtual path</param>
		/// <returns>The absolute url</returns>
		public string AbsoluteUrl(string virtualpath) {
			var request = HttpContext.Current.Request;

			// First, convert virtual paths to site url's
			if (virtualpath.StartsWith("~/"))
				virtualpath = Url(virtualpath);

			// Now add server, scheme and port
			return request.Url.Scheme + "://" + request.Url.DnsSafeHost +
				(!request.Url.IsDefaultPort ? ":" + request.Url.Port.ToString() : "") + virtualpath;
		}

		/// <summary>
		/// Generates an url from the given virtual path.
		/// </summary>
		/// <param name="virtualpath">The virtual path</param>
		/// <returns>The url</returns>
		public string Url(string virtualpath) {
			var request = HttpContext.Current.Request;
			return virtualpath.Replace("~/", request.ApplicationPath + (request.ApplicationPath != "/" ? "/" : ""));
		}
	}
}
