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
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<Db>
	{
		public Configuration() {
			AutomaticMigrationsEnabled = false;
			ContextKey = "Piranha.EntityFramework.Db";
		}
	}
}
