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
using System.Collections.Generic;

namespace Piranha.Cache
{
	/// <summary>
	/// Model cache.
	/// </summary>
	/// <typeparam name="T">The model cache</typeparam>
	internal class ModelCache<T>
	{
		#region Members
		/// <summary>
		/// Cache mutex.
		/// </summary>
		private readonly object mutex = new object();

		/// <summary>
		/// The current cache provider.
		/// </summary>
		private readonly ICache provider;

		/// <summary>
		/// Function for getting the entity id.
		/// </summary>
		private readonly Func<T, Guid> GetId;

		/// <summary>
		/// Function for getting the unique entity key.
		/// </summary>
		private readonly Func<T, string> GetKey;
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="cache">The current cache provider</param>
		/// <param name="getId">Function for get the model id</param>
		/// <param name="getKey">Function for getting the model key</param>
		internal ModelCache(ICache cache, Func<T, Guid> getId, Func<T, string> getKey) {
			provider = cache;
			GetId = getId;
			GetKey = getKey;
		}

		/// <summary>
		/// Adds the given model to the cache.
		/// </summary>
		/// <param name="entity">The model</param>
		public void Add(T model) {
			lock (mutex) {
				var id = GetId(model);
				var key = GetKey(model);
				var keymap = GetKeyMap(this.GetType().FullName);

				keymap[key] = id;
				provider.Set(id.ToString(), model);
				provider.Set(this.GetType().FullName, keymap);
			}
		}

		/// <summary>
		/// Gets the cached model with the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <returns>The model, null if it wasn't found</returns>
		public T Get(Guid id) {
			try {
				return provider.Get<T>(id.ToString());
			} catch { }
			return default(T);
		}

		/// <summary>
		/// Gets the cached model with the given slug.
		/// </summary>
		/// <param name="key">The unique key</param>
		/// <returns>The model, null if it wasn't found</returns>
		public T Get(string key) {
			try {
				var keymap = GetKeyMap(this.GetType().FullName);
				return provider.Get<T>(keymap[key].ToString());
			} catch { }
			return default(T);
		}

		/// <summary>
		/// Removes the model with the given id from the cache.
		/// </summary>
		/// <param name="id">The unique id</param>
		public void Remove(Guid id) {
			lock (mutex) {
				var model = provider.Get<T>(id.ToString());
				if (model != null) {
					var keymap = GetKeyMap(this.GetType().FullName);
					var key = GetKey(model);

					if (keymap.ContainsKey(key)) {
						keymap.Remove(key);
						provider.Set(this.GetType().FullName, keymap);
					}
					provider.Remove(id.ToString());
				}
			}
		}

		#region Private methods
		/// <summary>
		/// Gets or creates the keymap for the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <returns>The keymap</returns>
		private Dictionary<string, Guid> GetKeyMap(string id) {
			var map = provider.Get<Dictionary<string, Guid>>(id);

			// Create a new map if it doesn't exist
			if (map == null) {
				map = new Dictionary<string, Guid>();
				provider.Set(id, map);
			}
			return map;
		}
		#endregion
	}
}
