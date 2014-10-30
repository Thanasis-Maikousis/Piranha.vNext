/*
 * Copyright (c) 2014 Håkan Edling
 *
 * See the file LICENSE for copying permission.
 * 
 * http://github.com/tidyui/piranha
 * 
 */

using System;

namespace Piranha.Extend
{
	/// <summary>
	/// Base class for creating extensions.
	/// </summary>
	public abstract class Extension : IExtension
	{
		/// <summary>
		/// Transforms the extension value for the application model.
		/// </summary>
		/// <returns>The extension value</returns>
		public virtual object GetValue() {
			return this;
		}
	}
}
