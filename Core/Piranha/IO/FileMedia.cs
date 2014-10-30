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
	/// Media provider for storing media to file.
	/// </summary>
	public class FileMedia : IMedia
	{
		#region Members
		private const string mediaPath = @"App_Data\Uploads\Media";
		private const string cachePath = @"App_Data\Cache\Media";
		private readonly string mediaMapped;
		private readonly string cacheMapped;
		private readonly bool disabled;
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public FileMedia() {
			if (AppDomain.CurrentDomain != null && !String.IsNullOrEmpty(AppDomain.CurrentDomain.BaseDirectory)) {
				mediaMapped = AppDomain.CurrentDomain.BaseDirectory + mediaPath;
				cacheMapped = AppDomain.CurrentDomain.BaseDirectory + cachePath;

				// Ensure directories
				if (!Directory.Exists(mediaMapped))
					Directory.CreateDirectory(mediaMapped);
				if (!Directory.Exists(cacheMapped))
					Directory.CreateDirectory(cacheMapped);
			} else {
				disabled = true;
			}
		}

		/// <summary>
		/// Gets the binary data for the given media object.
		/// </summary>
		/// <param name="media">The media object</param>
		/// <returns>The binary data</returns>
		public byte[] Get(Models.Media media) {
			if (!disabled) {
				if (File.Exists(mediaMapped + "/" + media.Id.ToString())) {
					return File.ReadAllBytes(mediaMapped + "/" + media.Id.ToString());
				}
			}
			return null;
		}

		/// <summary>
		/// Saves the given binary data for the given media object.
		/// </summary>
		/// <param name="media">The media object</param>
		/// <param name="bytes">The binary data</param>
		public void Put(Models.Media media, byte[] bytes) {
			if (!disabled) {
				using (var writer = new FileStream(mediaMapped + "/" + media.Id.ToString(), FileMode.Create)) {
					writer.Write(bytes, 0, bytes.Length);
				}
			}
		}

		/// <summary>
		/// Saves the binary data available in the stream in the
		/// given media object.
		/// </summary>
		/// <param name="media">The media object</param>
		/// <param name="stream">The stream</param>
		public async void Put(Models.Media media, Stream stream) {
			if (!disabled) {
				using (var writer = new FileStream(mediaMapped + "/" + media.Id.ToString(), FileMode.Create)) {
					await stream.CopyToAsync(writer);
				}
			}
		}

		/// <summary>
		/// Deletes the binary data for the given media object.
		/// </summary>
		/// <param name="media">The media object</param>
		public void Delete(Models.Media media) {
			File.Delete(mediaMapped + "/" + media.Id.ToString());
		}
	}
}
