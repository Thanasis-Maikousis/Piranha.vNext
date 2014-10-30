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

namespace Piranha.Data
{
	/// <summary>
	/// Model validation exception.
	/// </summary>
	public sealed class ModelException : Exception
	{
		#region Members
		/// <summary>
		/// Gets the array of model errors.
		/// </summary>
		public readonly ModelError[] Errors;
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="message">The error message</param>
		/// <param name="errors">The model errors</param>
		public ModelException(string message, IEnumerable<ModelError> errors) {
			Errors = errors.ToArray();
		}
	}
}
