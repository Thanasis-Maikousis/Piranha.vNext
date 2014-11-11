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
using System.Linq;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Scaling;

namespace Piranha.Server.Handlers
{
	/// <summary>
	/// Handler for media files.
	/// </summary>
	public class MediaHandler : IHandler
	{
		/// <summary>
		/// Tries to handle an incoming request.
		/// </summary>
		/// <param name="api">The current api</param>
		/// <param name="request">The incoming route request</param>
		/// <returns>The result</returns>
		public IResponse Handle(Api api, IRequest request) {
			var slug = request.Segments.Length > 1 ? request.Segments[1] : "";
			int? width = null, height = null;

			if (!String.IsNullOrWhiteSpace(slug)) {
				var index = slug.LastIndexOf('.');

				if (index != -1) {
					var name = slug.Substring(0, index);
					var ending = slug.Substring(index);

					var segments = name.Split(new char[] { '_' });

					if (segments.Length > 2) {
						height = Convert.ToInt32(segments[2]);
					}
					if (segments.Length > 1) {
						width = Convert.ToInt32(segments[1]);
					}
					slug = segments[0] + ending;
				}

				var media = api.Media.GetSingle(slug);

				if (media != null) {
					var response = request.StreamResponse();
					var data = App.Media.Get(media);

					if (data != null) {
						response.ContentType = media.ContentType;

						if (width.HasValue) {
							using (var mem = new MemoryStream(data)) {
								var image = new KalikoImage(mem);
								var scale = height.HasValue ? 
									(ScalingBase)new CropScaling(width.Value, height.Value) : 
									(ScalingBase)new FitScaling(width.Value, Int32.MaxValue);

								image = image.Scale(scale);
								image.SavePng(response.OutputStream);
							}
						} else {
							using (var writer = new BinaryWriter(response.OutputStream)) {
								writer.Write(data);
							}
						}
						return response;
					}
				}
			}
			return null;
		}
	}
}
