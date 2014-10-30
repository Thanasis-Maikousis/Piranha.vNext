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
	/// Tests for the author repository.
	/// </summary>
	public abstract class AuthorTests
	{
		/// <summary>
		/// Test the author repository.
		/// </summary>
		protected void Run() {
			using (var api = new Api()) {
				// Add new model
				var model = new Models.Author() {
					Name = "John Doe",
					Email = "john@doe.com"
				};
				api.Authors.Add(model);
				api.SaveChanges();
			}

			using (var api = new Api()) {
				// Get model
				var model = api.Authors.GetSingle(where: a => a.Name == "John Doe");

				Assert.IsNotNull(model);
				Assert.AreEqual("john@doe.com", model.Email);

				// Update model
				model.Name = "Sir John Doe";
				api.SaveChanges();
			}

			using (var api = new Api()) {
				// Verify update
				var model = api.Authors.GetSingle(where: a => a.Email == "john@doe.com");

				Assert.IsNotNull(model);
				Assert.AreEqual("Sir John Doe", model.Name);

				// Remove model
				api.Authors.Remove(model);
				api.SaveChanges();
			}

			using (var api = new Api()) {
				// Verify remove
				var model = api.Authors.GetSingle(where: a => a.Email == "john@doe.com");
				Assert.IsNull(model);
			}
		}
	}
}
