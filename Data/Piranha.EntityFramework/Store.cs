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
using Piranha.Data;

namespace Piranha.EntityFramework
{
	/// <summary>
	/// Store implementation for Entity Framework.
	/// </summary>
	public class Store : IStore
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Store() { }

		/// <summary>
		/// Opens a new session on the current store.
		/// </summary>
		/// <returns>The new session</returns>
		public ISession OpenSession() {
			return new Session();
		}
	}
}
