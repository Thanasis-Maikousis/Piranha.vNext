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
using System.Web;
using Piranha;

/// <summary>
/// Model extensions.
/// </summary>
public static class Extensions
{
	/// <summary>
	/// Generates html content with nofollow links from the comment.
	/// </summary>
	/// <param name="comment">The comment</param>
	/// <returns>The html content</returns>
	public static IHtmlString Html(this Piranha.Models.Comment comment) {
		return new HtmlString(comment.Body.Replace("\n", "<br>").GenerateLinks());
	}
}
