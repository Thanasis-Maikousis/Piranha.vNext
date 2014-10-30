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
	/// Tests for the page type repository.
	/// </summary>
	public abstract class PageTypeTests
	{
		/// <summary>
		/// Test the page type repository.
		/// </summary>
		protected void Run() {
			using (var api = new Api()) {
				// Add new model
				var model = new Models.PageType() {
					Name = "Standard page",
					Route = "page"
				};
				api.PageTypes.Add(model);
				api.SaveChanges();
			}

			using (var api = new Api()) {
				// Get model
				var model = api.PageTypes.GetSingle(where: t => t.Slug == "standard-page");

				Assert.IsNotNull(model);
				Assert.AreEqual("Standard page", model.Name);
				Assert.AreEqual("page", model.Route);

				// Update model
				model.Name = "Updated page";
				api.SaveChanges();
			}

			using (var api = new Api()) {
				// Verify update
				var model = api.PageTypes.GetSingle(where: t => t.Slug == "standard-page");

				Assert.IsNotNull(model);
				Assert.AreEqual("Updated page", model.Name);
				Assert.AreEqual("page", model.Route);

				// Remove model
				api.PageTypes.Remove(model);
				api.SaveChanges();
			}

			using (var api = new Api()) {
				// Verify remove
				var model = api.PageTypes.GetSingle(where: t => t.Slug == "standard-page");
				Assert.IsNull(model);
			}
		}
	}
}
