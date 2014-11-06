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

namespace Piranha.Models
{
	/// <summary>
	/// Post types are used to define different kinds of posts.
	/// </summary>
	public sealed class PostType : Base.ContentType, Data.IModel, Data.IChanges
	{
		#region Properties
		/// <summary>
		/// Gets/sets if posts of this type should be included
		/// in the site RSS or not.
		/// </summary>
		public bool IncludeInRss { get; set; }

		/// <summary>
		/// Gets/sets if archives should be enabled for this post type.
		/// </summary>
		public bool EnableArchive { get; set; }

		/// <summary>
		/// Gets/sets the meta keywords used when rendering the
		/// archive page for the post type.
		/// </summary>
		public string MetaKeywords { get; set; }

		/// <summary>
		/// Gets/sets the meta description used when rendering the
		/// archive page for the post type.
		/// </summary>
		public string MetaDescription { get; set; }

		/// <summary>
		/// Gets/sets the optional archive title.
		/// </summary>
		public string ArchiveTitle { get; set; }

		/// <summary>
		/// Gets/sets the optional route for the archive page
		/// for posts of this type.
		/// </summary>
		public string ArchiveRoute { get; set; }

		/// <summary>
		/// Gets/sets the optional view for the archive page
		/// for posts of this type.
		/// </summary>
		public string ArchiveView { get; set; }

		/// <summary>
		/// Gets/sets the optional route for commenting posts
		/// of this type.
		/// </summary>
		public string CommentRoute { get; set; }
		#endregion

		#region Internal events
		/// <summary>
		/// Called before the model is saved by the DbContext.
		/// </summary>
		/// <param name="db">The current db context</param>
		public override void OnSave() {
			// Remove from model cache
			App.ModelCache.PostTypes.Remove(this.Id);
		}

		/// <summary>
		/// Called before the model is deleted by the DbContext.
		/// </summary>
		/// <param name="db">The current db context</param>
		public override void OnDelete() {
			// Remove from model cache
			App.ModelCache.PostTypes.Remove(this.Id);
		}
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public PostType() {
			EnableArchive = true;
		}
	}
}