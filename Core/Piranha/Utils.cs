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
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Piranha
{
	/// <summary>
	/// Internal utility methods
	/// </summary>
	public static class Utils
	{
		/// <summary>
		/// Gets a subset of the given array as a new array.
		/// </summary>
		/// <typeparam name="T">The array type</typeparam>
		/// <param name="arr">The array</param>
		/// <param name="startpos">The startpos</param>
		/// <param name="length">The length</param>
		/// <returns>The new array</returns>
		public static T[] Subset<T>(this T[] arr, int startpos = 0, int length = 0) {
			List<T> tmp = new List<T>();

			length = length > 0 ? length : arr.Length - startpos;

			for (var i = 0; i < arr.Length; i++) {
				if (i >= startpos && i < (startpos + length))
					tmp.Add(arr[i]);
			}
			return tmp.ToArray();
		}

		/// <summary>
		/// Removes all html tags from the given string.
		/// </summary>
		/// <param name="str">The text</param>
		/// <returns>The processed text</returns>
		public static string StripHtml(this string str) {
			return Regex.Replace(str, "<.*?>", "");
		}

		/// <summary>
		/// Generates nofollow html links for all urls in the given string.
		/// </summary>
		/// <param name="str">The text</param>
		/// <returns>The processed text</returns>
		public static string GenerateLinks(this string str) {
			var regx = new Regex(@"http://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&amp;\*\(\)_\-\=\+\\\/\?\.\:\;\'‌​\,]*)?");
			var matches = regx.Matches(str);

			foreach (Match match in matches) { 
				str = str.Replace(match.Value, String.Format("<a href=\"{0}\" rel=\"nofollow\">{0}</a>", match.Value));
			}
			return str;
		}

		/// <summary>
		/// Formats a media slug width the given width & height.
		/// </summary>
		/// <param name="slug">The media slug</param>
		/// <param name="width">The optional width</param>
		/// <param name="height">The optional height</param>
		/// <returns>The formatted slug</returns>
		public static string FormatMediaSlug(string slug, int? width, int? height) {
			if (width.HasValue) {
				var index = slug.LastIndexOf('.');

				if (index != -1) {
					var name = slug.Substring(0, index);
					var ending = slug.Substring(index);

					return name + "_" + width.ToString() + (height.HasValue ? "_" + height.ToString() : "") + ending;
				}				
			}
			return slug;
		}

		/// <summary>
		/// Generates a slug from the given string.
		/// </summary>
		/// <param name="str">The string</param>
		/// <returns>The slug</returns>
		public static string GenerateSlug(string str) {
			// Use custom slug generation, if registered
			if (App.GenerateSlug != null) {
				return App.GenerateSlug(str);
			} else {
				// Default slug generation
				var slug = Regex.Replace(str.ToLower()
					.Replace(" ", "-")
					.Replace("å", "a")
					.Replace("ä", "a")
					.Replace("á", "a")
					.Replace("à", "a")
					.Replace("ö", "o")
					.Replace("ó", "o")
					.Replace("ò", "o")
					.Replace("é", "e")
					.Replace("è", "e")
					.Replace("í", "i")
					.Replace("ì", "i"), @"[^a-z0-9-/]", "").Replace("--", "-");

				if (slug.EndsWith("-"))
					slug = slug.Substring(0, slug.LastIndexOf("-"));
				if (slug.StartsWith("-"))
					slug = slug.Substring(Math.Min(slug.IndexOf("-") + 1, slug.Length));
				return slug;
			}
		}

		/// <summary>
		/// Generates a slug for the given filename.
		/// </summary>
		/// <param name="str">The filename</param>
		/// <returns>The slug</returns>
		public static string GenerateFileSlug(string filename) {
			var index = filename.LastIndexOf('.');

			if (index != -1) {
				var name = filename.Substring(0, index);
				var ending = filename.Substring(index);

				return GenerateSlug(name) + ending;
			}
			return GenerateSlug(filename);
		}

		/// <summary>
		/// Gets the param value with the given key.
		/// </summary>
		/// <typeparam name="T">The value type</typeparam>
		/// <param name="key">The param name</param>
		/// <param name="func">The conversion function</param>
		/// <returns>The param value</returns>
		public static T GetParam<T>(string key, Func<string, T> func) {
			using (var api = new Api()) {
				var param = api.Params.GetSingle(key);

				if (param != null)
					return func(param.Value);
				return default(T);
			}
		}

		/// <summary>
		/// Sets the param value with the given key.
		/// </summary>
		/// <param name="key">The param name</param>
		/// <param name="value">The value</param>
		public static void SetParam(string key, object value) {
			using (var api = new Api()) { 
				var param = api.Params.GetSingle(key);

				param.Value = value.ToString();

				api.SaveChanges();
			}
		}

		/// <summary>
		/// Gets the current file version.
		/// </summary>
		/// <returns>The file version</returns>
		public static string GetFileVersion() {
			return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
		}

		/// <summary>
		/// Gets the current assembly version.
		/// </summary>
		/// <returns>The assembly version</returns>
		public static string GetAssemblyVersion() {
			return Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}
	}
}
