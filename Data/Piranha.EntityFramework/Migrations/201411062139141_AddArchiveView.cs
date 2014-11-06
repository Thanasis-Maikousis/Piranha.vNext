/*
 * Copyright (c) 2014 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * http://github.com/piranhacms/piranha.vnext
 * 
 */

namespace Piranha.EntityFramework.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	/// <summary>
	/// Adds archive view to post types.
	/// </summary>
	public partial class AddArchiveView : DbMigration
	{
		public override void Up() {
			AddColumn("dbo.PiranhaPostTypes", "ArchiveView", c => c.String(maxLength: 255));
		}

		public override void Down() {
			DropColumn("dbo.PiranhaPostTypes", "ArchiveView");
		}
	}
}
