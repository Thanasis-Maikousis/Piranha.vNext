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

namespace Piranha.Server
{
	/// <summary>
	/// Interface for defining a request to Piranha CMS.
	/// </summary>
	public interface IRequest
	{
		#region Properties
		/// <summary>
		/// Gets the full raw url requested.
		/// </summary>
		string RawUrl { get; }

		/// <summary>
		/// Gets the absolute path of the request url.
		/// </summary>
		string Path { get; }

		/// <summary>
		/// Gets the optional query of the requested url.
		/// </summary>
		string Query { get; }

		/// <summary>
		/// Gets the segments of the absolute path.
		/// </summary>
		string[] Segments { get; }

		/// <summary>
		/// Gets the params of the optional query.
		/// </summary>
		Param[] Params { get; }
		#endregion

		/// <summary>
		/// Creates a new redirect response for the current request.
		/// </summary>
		/// <returns>The response</returns>
		IRedirectResponse RedirectResponse();

		/// <summary>
		/// Creates a new rewrite response for the current request.
		/// </summary>
		/// <returns>The response</returns>
		IRewriteResponse RewriteResponse();

		/// <summary>
		/// Creates a new stream response for the current request.
		/// </summary>
		/// <returns>The response</returns>
		IStreamResponse StreamResponse();
	}
}
