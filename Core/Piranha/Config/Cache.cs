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

namespace Piranha.Config
{
	/// <summary>
	/// Server caching configuration.
	/// </summary>
	public static class Cache
	{
		/// <summary>
		/// Gets/sets the expiration time in minutes for the public http cache.
		/// </summary>
		public static int Expires {
			get { return Utils.GetParam<int>("cache_expires", s => Convert.ToInt32(s)); }
			set { Utils.SetParam("cache_expires", value); }
		}

		/// <summary>
		/// Gets/sets the max age in minutes for the public http cache.
		/// </summary>
		public static int MaxAge {
			get { return Utils.GetParam<int>("cache_maxage", s => Convert.ToInt32(s)); }
			set { Utils.SetParam("cache_maxage", value); }					
		}
	}
}
