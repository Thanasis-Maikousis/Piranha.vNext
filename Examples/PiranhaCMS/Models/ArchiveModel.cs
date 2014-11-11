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
using System.Globalization;

namespace PiranhaCMS.Models
{
	/// <summary>
	/// Custom archive model with some formatting methods.
	/// </summary>
	public class ArchiveModel : Piranha.Client.Models.ArchiveModel
	{
		/// <summary>
		/// Formats the currently selected period to a nice string.
		/// </summary>
		/// <returns>The formatted period</returns>
		public string FormatPeriod() {
			if (HasMonth) {
				return "/ " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month.Value) + " " + Year.ToString();
			} else if (HasYear) {
				return "/ The year of " + Year.ToString();
			}
			return "";
		}
	}
}