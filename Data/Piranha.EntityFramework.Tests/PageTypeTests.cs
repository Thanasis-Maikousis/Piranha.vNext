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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Piranha.EntityFramework.Tests
{
	/// <summary>
	/// Tests for the page type repository.
	/// </summary>
	[TestClass]
	public class PageTypeTests : Piranha.Tests.Repositories.PageTypeTests
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public PageTypeTests() {
			App.Init(c => {
				c.Store = new EntityFramework.Store();
			});
		}

		/// <summary>
		/// Test the page type repository.
		/// </summary>
		[TestMethod]
		[TestCategory("Entity Framework")]
		public void PageTypes() {
			base.Run();
		}
	}
}
