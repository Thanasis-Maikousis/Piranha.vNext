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
	/// Tests for the block repository.
	/// </summary>
	[TestClass]
	public class BlockTests : Piranha.Tests.Repositories.BlockTests
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public BlockTests() {
			App.Init(c => {
				c.Store = new EntityFramework.Store();
			});
		}

		/// <summary>
		/// Test the category repository.
		/// </summary>
		[TestMethod]
		[TestCategory("Entity Framework")]
		public void Blocks() {
			base.Run();
		}
	}
}
