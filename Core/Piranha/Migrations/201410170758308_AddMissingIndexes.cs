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

namespace Piranha.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class AddMissingIndexes : DbMigration
	{
		public override void Up() {
			CreateIndex("dbo.PiranhaBlocks", "Slug", unique: true);
			CreateIndex("dbo.PiranhaMedia", "Slug", unique: true);
		}

		public override void Down() {
			DropIndex("dbo.PiranhaMedia", new[] { "Slug" });
			DropIndex("dbo.PiranhaBlocks", new[] { "Slug" });
		}
	}
}
