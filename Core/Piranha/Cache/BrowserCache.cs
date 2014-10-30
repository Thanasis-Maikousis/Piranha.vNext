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
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Piranha.Cache
{
	/// <summary>
	/// Static helper class for handling web browser cache.
	/// </summary>
	public static class BrowserCache
	{
		/// <summary>
		/// Checks if the client has the correct version cached for the
		/// given key and last modification date. The method alos outputs
		/// the neccessary HTTP headers for the caching depending on the state.
		/// <param name="context">The http context</param>
		/// <param name="key">The entity key</param>
		/// <param name="modified">The modification date</param>
		/// <returns>If the correct version is cached</returns>
		public static bool IsCached(this HttpContextBase context, string key, DateTime modified) {
#if !DEBUG
			if (Config.Cache.Expires > 0) {
				var etag = GenerateEtag(key, modified);

				if (HasCache(context.Request, etag, modified)) {
					WriteCachedHeaders(context.Response);
					return true;
				} else {
					WriteHeaders(context.Response, etag, modified);
					return false;
				}
			}
#endif
			WriteNoCacheHeaders(context.Response);
			return false;
		}

		#region Private methods
		/// <summary>
		/// Generates an entity tag for the given key and last modification date.
		/// </summary>
		/// <param name="key">The entity key</param>
		/// <param name="modified">The modification date</param>
		/// <returns></returns>
		private static string GenerateEtag(string key, DateTime modified) {
			UTF8Encoding encoder = new UTF8Encoding();
			MD5CryptoServiceProvider crypto = new MD5CryptoServiceProvider();

			string str = key + modified.ToLongTimeString();
			byte[] bts = crypto.ComputeHash(encoder.GetBytes(str));
			return "\"" + Convert.ToBase64String(bts, 0, bts.Length) + "\"";
		}

		/// <summary>
		/// Checks if the correct version of the entity is chached in the web browser 
		/// according to the given entity key and last modification date.
		/// </summary>
		/// <param name="request">The http request</param>
		/// <param name="etag">The entity tag</param>
		/// <param name="modified">The modification date</param>
		/// <returns>If the correct version is cached</returns>
		private static bool HasCache(this HttpRequestBase request, string etag, DateTime modified) {
			if (Config.Cache.Expires > 0) {
				// Check If-None-Match
				string requestTag = request.Headers["If-None-Match"];
				if (!String.IsNullOrEmpty(requestTag) && requestTag == etag)
					return true;

				// Check If-Modified-Since
				string requestMod = request.Headers["If-Modified-Since"];
				if (!String.IsNullOrEmpty(requestMod))
					try {
						DateTime since;
						if (DateTime.TryParse(requestMod, out since))
							return since >= modified;
					} catch { }
				return false;
			}
			return false;
		}

		/// <summary>
		/// Outputs the correct cache headers for the given key and modification date.
		/// </summary>
		/// <param name="response">The http response</param>
		/// <param name="etag">The entity tag</param>
		/// <param name="modified">The modification date</param>
		private static void WriteHeaders(this HttpResponseBase response, string etag, DateTime modified) {
			if (Config.Cache.Expires > 0) {
				response.Cache.SetETag(etag);
				response.Cache.SetLastModified(modified <= DateTime.Now ? modified : DateTime.Now);
				response.Cache.SetCacheability(System.Web.HttpCacheability.ServerAndPrivate);
				response.Cache.SetExpires(DateTime.Now.AddMinutes(Config.Cache.Expires));
				response.Cache.SetMaxAge(new TimeSpan(0, Config.Cache.MaxAge, 0));
			}
		}

		/// <summary>
		/// Outputs the correct headers for a cached request.
		/// </summary>
		/// <param name="response">The http response</param>
		private static void WriteCachedHeaders(this HttpResponseBase response) {
			response.StatusCode = 304;
			response.SuppressContent = true;
		}

		/// <summary>
		/// Outputs the correct headers for no cache.
		/// </summary>
		/// <param name="response">The http response</param>
		private static void WriteNoCacheHeaders(this HttpResponseBase response) {
			response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
		}
		#endregion
	}
}
