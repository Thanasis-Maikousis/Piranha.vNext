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
using System.IO;

namespace Piranha.IO
{
	/// <summary>
	/// Interface for creating an media provider.
	/// </summary>
	public interface IMedia
	{
		/// <summary>
		/// Gets the binary data for the given media object.
		/// </summary>
		/// <param name="media">The media object</param>
		/// <returns>The binary data</returns>
		byte[] Get(Models.Media media);

		/// <summary>
		/// Saves the given binary data for the given media object.
		/// </summary>
		/// <param name="media">The media object</param>
		/// <param name="bytes">The binary data</param>
		void Put(Models.Media media, byte[] bytes);

		/// <summary>
		/// Saves the binary data available in the stream in the
		/// given media object.
		/// </summary>
		/// <param name="media">The media object</param>
		/// <param name="stream">The stream</param>
		void Put(Models.Media media, Stream stream);

		/// <summary>
		/// Deletes the binary data for the given media object.
		/// </summary>
		/// <param name="media">The media object</param>
		void Delete(Models.Media media);
	}
}
