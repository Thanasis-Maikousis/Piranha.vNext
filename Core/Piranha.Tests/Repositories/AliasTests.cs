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
	/// Tests for the alias repository.
	/// </summary>
	public abstract class AliasTests
	{
		/// <summary>
		/// Test the alias repository.
		/// </summary>
		protected void Run() {
			var id = Guid.Empty;

			using (var api = new Api()) {
				// Add new model
				var model = new Models.Alias() {
					OldUrl = "oldstuff.aspx?id=thisisalongunreadableandyglyurl",
					NewUrl = "~/blog/my-new-permalink",
					IsPermanent = true
				};
				api.Aliases.Add(model);
				api.SaveChanges();
				id = model.Id;
			}

			using (var api = new Api()) {
				// Get model
				var model = api.Aliases.GetSingle(where: a => a.Id == id);
				Assert.IsNotNull(model);
				Assert.AreEqual("/oldstuff.aspx?id=thisisalongunreadableandyglyurl", model.OldUrl);
				Assert.AreEqual("/blog/my-new-permalink", model.NewUrl);
				Assert.AreEqual(true, model.IsPermanent);

				// Update model
				model.NewUrl = "/blog/welcome";
				api.SaveChanges();
			}

			using (var api = new Api()) {
				// Verify update
				var model = api.Aliases.GetSingle(where: a => a.Id == id);
				Assert.IsNotNull(model);
				Assert.AreEqual("/oldstuff.aspx?id=thisisalongunreadableandyglyurl", model.OldUrl);
				Assert.AreEqual("/blog/welcome", model.NewUrl);
				Assert.AreEqual(true, model.IsPermanent);

				// Remove model
				api.Aliases.Remove(model);
				api.SaveChanges();
			}

			using (var api = new Api()) {
				// Verify remove
				var model = api.Aliases.GetSingle(where: a => a.Id == id);
				Assert.IsNull(model);
			}
		}
	}
}
