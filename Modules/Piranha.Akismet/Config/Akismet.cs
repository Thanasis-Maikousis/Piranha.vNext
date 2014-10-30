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

namespace Piranha.Config
{
	/// <summary>
	/// RSS Feed configuration.
	/// </summary>
	public static class Akismet
	{
		/// <summary>
		/// Gets/sets the api key.
		/// </summary>
		public static string ApiKey {
			get { return Utils.GetParam<string>("akismet_apikey", s => s); }
			set { Utils.SetParam("akismet_apikey", value); }
		}

		/// <summary>
		/// Gets/sets the public site url.
		/// </summary>
		public static string SiteUrl {
			get { return Utils.GetParam<string>("akismet_siteurl", s => s); }
			set { Utils.SetParam("akismet_siteurl", value); }
		}
	}
}
