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

namespace Piranha.Cache
{
	/// <summary>
	/// Interface for creating an application cache provider.
	/// </summary>
	public interface ICache
	{
		/// <summary>
		/// Gets the cached model for the given id.
		/// </summary>
		/// <typeparam name="T">The model type</typeparam>
		/// <param name="id">The unique id</param>
		/// <returns>The model</returns>
		T Get<T>(string id);

		/// <summary>
		/// Sets the cached model for the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <param name="obj">The model</param>
		void Set(string id, object obj);

		/// <summary>
		/// Removes the cached model for the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		void Remove(string id);
	}
}
