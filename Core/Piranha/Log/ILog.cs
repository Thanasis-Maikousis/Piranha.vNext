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

namespace Piranha.Log
{
	/// <summary>
	/// Interface for creating a log provider.
	/// </summary>
	public interface ILog
	{
		/// <summary>
		/// Writes the given message to the log.
		/// </summary>
		/// <param name="level">The level</param>
		/// <param name="message">The log message</param>
		/// <param name="exception">The optional exception</param>
		void Log(LogLevel level, string message, Exception exception = null);
	}
}
