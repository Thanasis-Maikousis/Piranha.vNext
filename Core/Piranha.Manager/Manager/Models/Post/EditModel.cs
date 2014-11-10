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
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Piranha.Manager.Models.Post
{
	/// <summary>
	/// View model for the post edit view.
	/// </summary>
	public class EditModel
	{
		#region Inner classes
		/// <summary>
		/// View model for displaying comments in the post edit view.
		/// </summary>
		public class CommentListItem
		{
			public Guid Id { get; set; }
			public string Author { get; set; }
			public string Email { get; set; }
			public string WebSite { get; set; }
			public string Body { get; set; }
			public bool IsApproved { get; set; }
			public bool IsSpam { get; set; }
			public DateTime Created { get; set; }
		}

		public enum SubmitAction
		{
			Save,
			Publish,
			Unpublish
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets/sets the unique id.
		/// </summary>
		public Guid? Id { get; set; }

		/// <summary>
		/// Gets/sets the id of the post type.
		/// </summary>
		public Guid TypeId { get; set; }

		/// <summary>
		/// Gets/sets the name of the post type.
		/// </summary>
		public string TypeName { get; set; }

		/// <summary>
		/// Gets/sets the author responsible for this content.
		/// </summary>
		public Guid AuthorId { get; set; }

		/// <summary>
		/// Gets/sets the title.
		/// </summary>
		[Required, StringLength(128)]
		public string Title { get; set; }

		/// <summary>
		/// Gets/sets the unique slug.
		/// </summary>
		[StringLength(128)]
		public string Slug { get; set; }

		/// <summary>
		/// Gets/sets the optional keywords.
		/// </summary>
		[StringLength(128)]
		public string Keywords { get; set; }

		/// <summary>
		/// Gets/sets the optional description.
		/// </summary>
		[StringLength(255)]
		public string Description { get; set; }

		/// <summary>
		/// Gets/sets the optional excerpt.
		/// </summary>
		[StringLength(512)]
		public string Excerpt { get; set; }

		/// <summary>
		/// Gets/sets the main post body.
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		/// Gets/sets the optional route that should handle requests.
		/// </summary>
		[StringLength(255)]
		public string Route { get; set; }

		/// <summary>
		/// Gets/sets the optional view that should render requests.
		/// </summary>
		[StringLength(255)]
		public string View { get; set; }

		/// <summary>
		/// Gets/sets the publish date.
		/// </summary>
		public DateTime? Published { get; set; }

		/// <summary>
		/// Gets/sets the currently selected categories.
		/// </summary>
		public string SelectedCategories { get; set; }

		/// <summary>
		/// Gets/sets the available authors.
		/// </summary>
		public SelectList Authors { get; set; }

		/// <summary>
		/// Gets/sets the available categories.
		/// </summary>
		public SelectList Categories { get; set; }

		/// <summary>
		/// Gets/sets the available comments.
		/// </summary>
		public IList<CommentListItem> Comments { get; set; }

		/// <summary>
		/// Gets/sets the selected submit action.
		/// </summary>
		public SubmitAction Action { get; set; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public EditModel() {
			using (var api = new Api()) {
				Init(api);
			}
		}

		/// <summary>
		/// Creates a new edit model using the given api.
		/// </summary>
		/// <param name="api">The current api.</param>
		/// <param name="posttype">The selected post type slug</param>
		public EditModel(Api api, string posttype) {
			Init(api, posttype);
		}

		/// <summary>
		/// Gets the edit model for the post with the given id.
		/// </summary>
		/// <param name="api">The current api</param>
		/// <param name="id">The unique id</param>
		/// <returns>The model</returns>
		public static EditModel GetById(Api api, Guid id) {
			var post = api.Posts.GetSingle(where: p => p.Id == id);

			if (post != null) {
				var m = Mapper.Map<Piranha.Models.Post, EditModel>(post);
				m.TypeName = post.Type.Name;
				foreach (var cat in post.Categories) {
					if (m.SelectedCategories != "")
						m.SelectedCategories += ",";
					m.SelectedCategories += cat.Title;
				}
				m.Comments = api.Comments.Get(where: c => c.PostId == post.Id).Select(c => new CommentListItem() {
					Id = c.Id,
					Author = c.Author,
					Email = c.Email,
					WebSite = c.WebSite,
					Body = c.Body,
					IsApproved = c.IsApproved,
					IsSpam = c.IsSpam,
					Created = c.Created
				}).ToList();
				return m;
			}
			return null;
		}

		/// <summary>
		/// Saves the current post edit model.
		/// </summary>
		/// <param name="api">The current api</param>
		public void Save(Api api) {
			var newModel = false;

			// Get or create post
			var post = api.Posts.GetSingle(where: p => p.Id == this.Id);
			if (post == null) {
				post = new Piranha.Models.Post();
				newModel = true;
			}

			// Map values
			Mapper.Map<EditModel, Piranha.Models.Post>(this, post);

			// Check action
			if (Action == SubmitAction.Publish)
				post.Published = DateTime.Now;
			else if (Action == SubmitAction.Unpublish)
				post.Published = null;

			// Remove old categories
			post.Categories.Clear();

			// Map current categories
			if (!String.IsNullOrWhiteSpace(SelectedCategories)) {
				foreach (var str in SelectedCategories.Split(new char[] {','})) {
					var categoryName = str.Trim();

					var cat = api.Categories.GetSingle(where: c => c.Title == categoryName);
					if (cat == null) {
						cat = new Piranha.Models.Category() {
							Title = categoryName
						};
						api.Categories.Add(cat);
					}
					post.Categories.Add(cat);
				}
			}
			if (newModel)
				api.Posts.Add(post);
			api.SaveChanges();

			this.Id = post.Id;
		}

		#region Private methods
		/// <summary>
		/// Initializes the model.
		/// </summary>
		/// <param name="api">The current api</param>
		/// <param name="posttype">The optional post type</param>
		private void Init(Api api, string posttype = "") {
			// Get the post type
			if (!String.IsNullOrWhiteSpace(posttype)) {
				var type = api.PostTypes.GetSingle(where: t => t.Slug == posttype);

				TypeId = type.Id;
				TypeName = type.Name;
			}

			// Get available authors
			var authors = api.Authors.Get();
			Authors = new SelectList(authors, "Id", "Name");

			// Get available categories
			var categories = api.Categories.Get();
			Categories = new SelectList(categories, "Id", "Title");

			// Get default author
			var userid = App.Security.GetUserId();
			if (!String.IsNullOrEmpty(userid)) {
				// TODO
			}

			// Setup comment list
			Comments = new List<CommentListItem>();
			SelectedCategories = "";
		}
		#endregion
	}
}