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

namespace Piranha.Log
{
	/// <summary>
	/// Log provider for writing application logs to file.
	/// </summary>
	public class FileLog : ILog
	{
		#region Members
		private const string path = @"App_Data\Logs";
		private const string msg = "{0} [{1}] {2}";
		private readonly object mutex = new object();
		private readonly string filePath;
		private readonly bool disabled;
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public FileLog() {
			if (AppDomain.CurrentDomain != null && !String.IsNullOrWhiteSpace(AppDomain.CurrentDomain.BaseDirectory)) {
				var mapped = AppDomain.CurrentDomain.BaseDirectory + path;

				// Ensure log directory
				if (!Directory.Exists(mapped))
					Directory.CreateDirectory(mapped);

				// Store mapped file path
				filePath = mapped + @"\Log.txt";
			} else {
				disabled = true;
			}
		}

		/// <summary>
		/// Writes the given message to the log.
		/// </summary>
		/// <param name="level">The level</param>
		/// <param name="message">The log message</param>
		/// <param name="exception">The optional exception</param>
		public void Log(LogLevel level, string message, Exception exception = null) {
			if (!disabled) {
				if (level == LogLevel.INFO || level == LogLevel.WARNING) {
#if DEBUG
					// Only log info & warning message in Debug.
					Write(level, message, exception);
#endif
				} else {
					// Always log errors.
					Write(level, message, exception);
				}
			}
		}

		#region Private methods
		/// <summary>
		/// Writes the given message to the log.
		/// </summary>
		/// <param name="level">The level</param>
		/// <param name="message">The log message</param>
		/// <param name="exception">The optional exception</param>
		private void Write(LogLevel level, string message, Exception exception = null) {
			lock (mutex) {
				using (var writer = new StreamWriter(filePath, true)) {
					writer.WriteLine(String.Format(msg, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
						level.ToString(), message));
					if (exception != null)
						writer.WriteLine(exception.Message);
					writer.Flush();
					writer.Close();
				}
			}
		}
		#endregion
	}
}
