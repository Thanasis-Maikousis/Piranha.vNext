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
	/// An imported extension.
	/// </summary>
	public sealed class Import
	{
		/// <summary>
		/// Gets the display name.
		/// </summary>
		public string Name { get; internal set; }

		/// <summary>
		/// Gets the CLR value type.
		/// </summary>
		public Type ValueType { get; internal set; }

		/// <summary>
		/// Gets the extension type.
		/// </summary>
		public ExtensionType Type { get; internal set; }

		/// <summary>
		/// Internal constructor.
		/// </summary>
		internal Import() { }
	}
}
