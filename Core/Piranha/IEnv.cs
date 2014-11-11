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

namespace Piranha
{
	/// <summary>
	/// Interface defining a runtime environment.
	/// </summary>
	public interface IEnv
	{
		/// <summary>
		/// Gets/sets the current item being processed.
		/// </summary>
		Client.Models.Content GetCurrent();

		/// <summary>
		/// Gets/sets the current item being processed.
		/// </summary>
		void SetCurrent(Client.Models.Content current);

		/// <summary>
		/// Generates an absolute url from the given virtual path.
		/// </summary>
		/// <param name="virtualpath">The virtual path</param>
		/// <returns>The absolute url</returns>
		string AbsoluteUrl(string virtualpath);

		/// <summary>
		/// Generates an url from the given virtual path.
		/// </summary>
		/// <param name="virtualpath">The virtual path</param>
		/// <returns>The url</returns>
		string Url(string virtualpath);
	}
}
