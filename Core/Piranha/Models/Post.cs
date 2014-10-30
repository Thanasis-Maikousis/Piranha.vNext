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
	/// Posts are used to create content not positioned in the
	/// site structure.
	/// </summary>
	public sealed class Post : Base.Content<PostType>, Data.IModel, Data.IChanges, Data.IPublishable
	{
		#region Properties
		/// <summary>
		/// Gets/sets the id of the content type.
		/// </summary>
		public override Guid TypeId { get; set; }

		/// <summary>
		/// Gets/sets the unique slug.
		/// </summary>
		public override string Slug { get; set; }

		/// <summary>
		/// Gets/sets the optional excerpt.
		/// </summary>
		public string Excerpt { get; set; }

		/// <summary>
		/// Gets/sets the main post body.
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		/// Gets/sets the number of available comments.
		/// </summary>
		public int CommentCount { get; set; }
		#endregion

		#region Navigation properties
		/// <summary>
		/// Gets/sets the currently selected media attachments.
		/// </summary>
		public IList<Media> Attachments { get; set; }

		/// <summary>
		/// Gets/sets the currently selected categories.
		/// </summary>
		public IList<Category> Categories { get; set; }

		/// <summary>
		/// Gets/sets the currently available comments.
		/// </summary>
		public IList<Comment> Comments { get; set; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Post() {
			Attachments = new List<Media>();
			Categories = new List<Category>();
			Comments = new List<Comment>();
		}

		#region Events
		/// <summary>
		/// Called when the model is materialized by the DbContext.
		/// </summary>
		/// <param name="db">The current db context</param>
		public override void OnLoad() {
			if (Hooks.Models.Post.OnLoad != null)
				Hooks.Models.Post.OnLoad(this);
		}

		/// <summary>
		/// Called before the model is saved by the DbContext.
		/// </summary>
		/// <param name="db">The current db context</param>
		public override void OnSave() {
			if (Hooks.Models.Post.OnSave != null)
				Hooks.Models.Post.OnSave(this);

			// Remove from model cache
			App.ModelCache.Posts.Remove(this.Id);
		}

		/// <summary>
		/// Called before the model is deleted by the DbContext.
		/// </summary>
		/// <param name="db">The current db context</param>
		public override void OnDelete() {
			if (Hooks.Models.Post.OnDelete != null)
				Hooks.Models.Post.OnDelete(this);

			// Remove from model cache
			App.ModelCache.Posts.Remove(this.Id);
		}
		#endregion
	}
}
