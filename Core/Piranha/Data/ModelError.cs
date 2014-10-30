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

namespace Piranha.Data
{
	/// <summary>
	/// Class for representing model validatation errors.
	/// </summary>
	public sealed class ModelError
	{
		#region Inner classes
		/// <summary>
		/// The different types of validation errors.
		/// </summary>
		public enum ErrorType
		{
			Duplicate,
			Required,
			StringLength
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the model member. 
		/// </summary>
		public string Member { get; private set; }

		/// <summary>
		/// Gets the validation error type.
		/// </summary>
		public ErrorType Type { get; private set; }

		/// <summary>
		/// Gets the error message.
		/// </summary>
		public string Message { get; private set; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="member">The model member</param>
		/// <param name="type">The error type</param>
		/// <param name="message">The error message</param>
		internal ModelError(string member, ErrorType type, string message) {
			Member = member;
			Type = type;
			Message = message;
		}
	}
}
