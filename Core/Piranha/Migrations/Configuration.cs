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
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	/// <summary>
	/// Migrations configuration.
	/// </summary>
	internal sealed class Configuration : DbMigrationsConfiguration<Db>
	{
		#region Members
		private const string ROLE_SYSADMIN = "Piranha SysAdmin";
		private const string ROLE_ADMIN = "Piranha Admin";
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Configuration() {
			AutomaticMigrationsEnabled = false;
			ContextKey = "Piranha.Db";
		}

		/// <summary>
		/// Seed the database with initial data.
		/// </summary>
		/// <param name="context">The current db context</param>
		protected override void Seed(Db db) {
			#region Seed default params
			var param = db.Params.Where(p => p.Name == "site_title").SingleOrDefault();
			if (param == null) {
				db.Params.Add(new Models.Param() {
					Name = "site_title",
					Value = "Piranha CMS"
				});
			}

			param = db.Params.Where(p => p.Name == "site_description").SingleOrDefault();
			if (param == null) {
				db.Params.Add(new Models.Param() {
					Name = "site_description",
					Value = "Welcome to Piranha CMS"
				});
			}

			param = db.Params.Where(p => p.Name == "site_lastmodified").SingleOrDefault();
			if (param == null) {
				db.Params.Add(new Models.Param() {
					Name = "site_lastmodified",
					Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
				});
			}

			param = db.Params.Where(p => p.Name == "archive_pagesize").SingleOrDefault();
			if (param == null) {
				db.Params.Add(new Models.Param() {
					Name = "archive_pagesize",
					Value = "10"
				});
			}

			param = db.Params.Where(p => p.Name == "cache_expires").SingleOrDefault();
			if (param == null) {
				db.Params.Add(new Models.Param() {
					Name = "cache_expires",
					Value = "0"
				});
			}

			param = db.Params.Where(p => p.Name == "cache_maxage").SingleOrDefault();
			if (param == null) {
				db.Params.Add(new Models.Param() {
					Name = "cache_maxage",
					Value = "0"
				});
			}

			param = db.Params.Where(p => p.Name == "comment_moderate_authorized").SingleOrDefault();
			if (param == null) {
				db.Params.Add(new Models.Param() {
					Name = "comment_moderate_authorized",
					Value = false.ToString()
				});
			}

			param = db.Params.Where(p => p.Name == "comment_moderate_anonymous").SingleOrDefault();
			if (param == null) {
				db.Params.Add(new Models.Param() {
					Name = "comment_moderate_anonymous",
					Value = false.ToString()
				});
			}
			#endregion

			#region Seed default roles & permissions
			var permission = db.Permissions.Where(p => p.Name == "admin").SingleOrDefault();
			if (permission == null) {
				permission = new Models.Permission() {
					Name = "admin",
					Description = "Permission to login into the manager interface.",
					Roles = ROLE_ADMIN + "," + ROLE_SYSADMIN
				};
				db.Permissions.Add(permission);
			}

			permission = db.Permissions.Where(p => p.Name == "admin-authors").SingleOrDefault();
			if (permission == null) {
				permission = new Models.Permission() {
					Name = "admin-authors",
					Description = "Permission to add, update and delete authors.",
					Roles = ROLE_ADMIN + "," + ROLE_SYSADMIN
				};
				db.Permissions.Add(permission);
			}

			permission = db.Permissions.Where(p => p.Name == "admin-blocks").SingleOrDefault();
			if (permission == null) {
				permission = new Models.Permission() {
					Name = "admin-blocks",
					Description = "Permission to add, update and delete blocks.",
					Roles = ROLE_ADMIN + "," + ROLE_SYSADMIN
				};
				db.Permissions.Add(permission);
			}

			permission = db.Permissions.Where(p => p.Name == "admin-categories").SingleOrDefault();
			if (permission == null) {
				permission = new Models.Permission() {
					Name = "admin-categories",
					Description = "Permission to add, update and delete categories.",
					Roles = ROLE_ADMIN + "," + ROLE_SYSADMIN
				};
				db.Permissions.Add(permission);
			}

			permission = db.Permissions.Where(p => p.Name == "admin-permissions").SingleOrDefault();
			if (permission == null) {
				permission = new Models.Permission() {
					Name = "admin-permissions",
					Description = "Permission to add, update and delete permissions.",
					Roles = ROLE_SYSADMIN
				};
				db.Permissions.Add(permission);
			}

			permission = db.Permissions.Where(p => p.Name == "admin-posts").SingleOrDefault();
			if (permission == null) {
				permission = new Models.Permission() {
					Name = "admin-posts",
					Description = "Permission to add, update and delete posts.",
					Roles = ROLE_ADMIN + "," + ROLE_SYSADMIN
				};
				db.Permissions.Add(permission);
			}

			permission = db.Permissions.Where(p => p.Name == "admin-posttypes").SingleOrDefault();
			if (permission == null) {
				permission = new Models.Permission() {
					Name = "admin-posttypes",
					Description = "Permission to add, update and delete post types.",
					Roles = ROLE_SYSADMIN
				};
				db.Permissions.Add(permission);
			}
			#endregion
		}
	}
}