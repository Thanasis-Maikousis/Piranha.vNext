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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Piranha.Tests.Repositories
{
	/// <summary>
	/// Tests for the post repository.
	/// </summary>
	public abstract class PostTests
	{
		/// <summary>
		/// Test the post repository.
		/// </summary>
		protected void Run() {
			Models.PostType type = null;
			Models.Author author = null;
			Models.Post post = null;

			using (var api = new Api()) {
				// Add new post type
				type = new Models.PostType() {
					Name = "Test post",
					Route = "post"
				};
				api.PostTypes.Add(type);
				api.SaveChanges();

				// Add new author
				author = new Models.Author() {
					Name = "Jane Doe",
					Email = "jane@doe.com"
				};
				api.Authors.Add(author);
				api.SaveChanges();

				// Add new post
				post = new Models.Post() {
					TypeId = type.Id,
					AuthorId = author.Id,
					Title = "My first post",
					Excerpt = "Read my first post.",
					Body = "<p>Lorem ipsum</p>",
					Published = DateTime.Now
				};
				api.Posts.Add(post);
				api.SaveChanges();
			}
			
			using (var api = new Api()) {
				// Get model
				var model = api.Posts.GetSingle(where: p => p.Slug == "my-first-post");

				Assert.IsNotNull(model);
				Assert.AreEqual("Read my first post.", model.Excerpt);
				Assert.AreEqual("<p>Lorem ipsum</p>", model.Body);

				// Update model
				model.Excerpt = "Updated post";
				api.SaveChanges();
			}
			
			using (var api = new Api()) {
				// Verify update
				var model = api.Posts.GetSingle(where: p => p.Slug == "my-first-post");

				Assert.IsNotNull(model);
				Assert.AreEqual("Updated post", model.Excerpt);
				Assert.AreEqual("<p>Lorem ipsum</p>", model.Body);

				// Remove
				api.Posts.Remove(model);
				api.PostTypes.Remove(type.Id);
				api.Authors.Remove(author.Id);
				api.SaveChanges();
			}

			using (var api = new Api()) {
				// Verify remove
				post = api.Posts.GetSingle(where: p => p.Slug == "my-first-post");
				type = api.PostTypes.GetSingle(where: t => t.Id == type.Id);
				author = api.Authors.GetSingle(where: a => a.Id == author.Id);

				Assert.IsNull(post);
				Assert.IsNull(type);
				Assert.IsNull(author);
			}
		}
	}
}
