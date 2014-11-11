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
using System.Web;
using Piranha;

/// <summary>
/// Model extensions for AspNet.
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
