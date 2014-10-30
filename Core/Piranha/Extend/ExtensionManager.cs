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
using System.Linq;
using System.Reflection;

namespace Piranha.Extend
{
	/// <summary>
	/// The extension manager is responsible for managing the
	/// imported extensions.
	/// </summary>
	public sealed class ExtensionManager
	{
		#region Properties
		/// <summary>
		/// Gets the available modules.
		/// </summary>
		public IList<IModule> Modules { get; private set; }
		#endregion

		/// <summary>
		/// Default internal constructor.
		/// </summary>
		internal ExtensionManager() {
			Modules = new List<IModule>();

			// Scan all assemblies
			App.Logger.Log(Log.LogLevel.INFO, "ExtensionManager: Scanning assemblies");
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				foreach (var type in assembly.GetTypes()) {
					if (type.IsClass && !type.IsAbstract) {
						if (typeof(IModule).IsAssignableFrom(type)) {
							Modules.Add((IModule)Activator.CreateInstance(type));
						}
					} 
				}
			}

			// Initialize all modules
			App.Logger.Log(Log.LogLevel.INFO, "ExtensionManager: Initializing modules");
			foreach (var module in Modules)
				module.Init();
		}
	}
}
