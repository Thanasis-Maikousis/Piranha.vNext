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

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Piranha.Models;

namespace Piranha.Web.Models
{
	/// <summary>
	/// Application post model.
	/// </summary>
	public class PostModel
	{
		#region Properties
		/// <summary>
		/// Gets/sets the unique id.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets/sets the slug of the post type.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// Gets/sets the title.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Gets/sets the unique slug.
		/// </summary>
		public string Slug { get; set; }

		/// <summary>
		/// Gets/sets the optional keywords.
		/// </summary>
		public string Keywords { get; set; }

		/// <summary>
		/// Gets/sets the optional description.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets/sets the optional excerpt.
		/// </summary>
		public string Excerpt { get; set; }

		/// <summary>
		/// Gets/sets the main post body.
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		/// Gets/sets the optional route that should handle requests.
		/// </summary>
		public string Route { get; set; }

		/// <summary>
		/// Gets/sets the optional view that should render requests.
		/// </summary>
		public string View { get; set; }

		/// <summary>
		/// Gets/sets when the model was initially created.
		/// </summary>
		public DateTime Created { get; set; }

		/// <summary>
		/// Gets/sets when the model was last updated.
		/// </summary>
		public DateTime Updated { get; set; }

		/// <summary>
		/// Gets/sets when the model was published.
		/// </summary>
		public DateTime Published { get; set; }

		/// <summary>
		/// Gets/sets the number of available comments.
		/// </summary>
		public int CommentCount { get; set; }

		/// <summary>
		/// Gets/sets the author responsible for this content.
		/// </summary>
		public Author Author { get; set; }

		/// <summary>
		/// Gets/sets the attached media.
		/// </summary>
		public IList<Media> Attachments { get; set; }

		/// <summary>
		/// Gets/sets the available categories.
		/// </summary>
		public IList<Category> Categories { get; set; }

		/// <summary>
		/// Gets/sets the available comments.
		/// </summary>
		public IList<Comment> Comments { get; set; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public PostModel() {
			Attachments = new List<Media>();
			Categories = new List<Category>();
			Comments = new List<Comment>();
		}

		/// <summary>
		/// Gets the post model identified by the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <returns>The model</returns>
		public static PostModel GetById(Guid id) {
			return GetById<PostModel>(id);
		}

		/// <summary>
		/// Gets the post model identified by the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <returns>The model</returns>
		public static T GetById<T>(Guid id) where T : PostModel {
			using (var api = new Api()) {
				var post = api.Posts.GetSingle(id);

				if (post != null && post.Published <= DateTime.Now) {
					post.CommentCount = api.Comments.Get(where: c => c.PostId == post.Id).Count();
					return Map<T>(post);
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the post model identified by the given slug.
		/// </summary>
		/// <param name="slug">The unique slug</param>
		/// <param name="posttype">The post type slug</param>
		/// <returns>The model</returns>
		public static PostModel GetBySlug(string slug, string posttype) {
			return GetBySlug<PostModel>(slug, posttype);
		}

		/// <summary>
		/// Gets the post model identified by the given slug.
		/// </summary>
		/// <param name="slug">The unique slug</param>
		/// <param name="posttype">The post type slug</param>
		/// <returns>The model</returns>
		public static T GetBySlug<T>(string slug, string posttype) where T : PostModel {
			using (var api = new Api()) {
				var post = api.Posts.GetSingle(slug, posttype);

				if (post != null && post.Published <= DateTime.Now) {
					post.CommentCount = api.Comments.Get(where: c => c.PostId == post.Id).Count();
					return Map<T>(post);
				}
			}
			return null;
		}

		/// <summary>
		/// Loads all available comments for the current post.
		/// </summary>
		public virtual PostModel WithComments() {
			// Get all comments
			using (var api = new Api()) {
				Comments = api.Comments.Get(where: c => c.PostId == Id).ToList();
			}
			return this;
		}

		/// <summary>
		/// Gets the last modification date for the curremt post model.
		/// </summary>
		public virtual DateTime GetLastModified() {
			var modified = Comments.Count > 0 ? Comments.OrderBy(c => c.Created).First().Created : DateTime.MinValue;

			if (Published > modified)
				modified = Published;
			if (Updated > modified)
				modified = Updated;
			return modified;
		}

		#region Private methods
		/// <summary>
		/// Maps the given post to a new post model.
		/// </summary>
		/// <param name="api">The current api</param>
		/// <param name="post">The post</param>
		/// <returns>The post model</returns>
		private static T Map<T>(Post post) where T : PostModel {
			if (post != null) {
				var model = Activator.CreateInstance<T>();

				Mapper.Map<Post, PostModel>(post, model);

				return model;
			}
			return null;
		}
		#endregion
	}
}
