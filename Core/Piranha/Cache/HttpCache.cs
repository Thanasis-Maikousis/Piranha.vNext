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
using System.Web;

namespace Piranha.Cache
{
	/// <summary>
	/// The default cache uses the HttpRuntime Cache object to
	/// store its values in. 
	/// </summary>
	public class HttpCache : ICache
	{
		/// <summary>
		/// Gets the cached model for the given id.
		/// </summary>
		/// <typeparam name="T">The model type</typeparam>
		/// <param name="id">The unique id</param>
		/// <returns>The model</returns>
		public T Get<T>(string id) {
			// Make sure we have a http context
			if (HttpRuntime.Cache != null) {
				return (T)HttpRuntime.Cache[id];
			}
			return default(T);
		}

		/// <summary>
		/// Sets the cached model for the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <param name="obj">The model</param>
		public void Set(string id, object obj) {
			// Make sure we have a http context
			if (HttpRuntime.Cache != null)
				HttpRuntime.Cache[id] = obj;
		}

		/// <summary>
		/// Removes the cached model for the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		public void Remove(string id) {
			// Make sure we have a http context
			if (HttpRuntime.Cache != null) {
				HttpRuntime.Cache.Remove(id);
			}
		}
	}
}