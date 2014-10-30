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

	/// <summary>
	/// Initial database creation.
	/// </summary>
	public partial class InitialCreate : DbMigration
	{
		public override void Up() {
			CreateTable(
				"dbo.PiranhaAliases",
				c => new {
					Id = c.Guid(nullable: false),
					OldUrl = c.String(nullable: false, maxLength: 255),
					NewUrl = c.String(nullable: false, maxLength: 255),
					IsPermanent = c.Boolean(nullable: false),
					Created = c.DateTime(nullable: false),
					Updated = c.DateTime(nullable: false),
				})
				.PrimaryKey(t => t.Id)
				.Index(t => t.OldUrl, unique: true);

			CreateTable(
				"dbo.PiranhaAuthors",
				c => new {
					Id = c.Guid(nullable: false),
					Name = c.String(nullable: false, maxLength: 128),
					Email = c.String(maxLength: 128),
					Description = c.String(maxLength: 512),
					Created = c.DateTime(nullable: false),
					Updated = c.DateTime(nullable: false),
				})
				.PrimaryKey(t => t.Id);

			CreateTable(
				"dbo.PiranhaBlocks",
				c => new {
					Id = c.Guid(nullable: false),
					Name = c.String(nullable: false, maxLength: 128),
					Slug = c.String(nullable: false, maxLength: 128),
					Description = c.String(maxLength: 255),
					Body = c.String(),
					Created = c.DateTime(nullable: false),
					Updated = c.DateTime(nullable: false),
				})
				.PrimaryKey(t => t.Id);

			CreateTable(
				"dbo.PiranhaCategories",
				c => new {
					Id = c.Guid(nullable: false),
					Title = c.String(nullable: false, maxLength: 128),
					Slug = c.String(nullable: false, maxLength: 128),
					Created = c.DateTime(nullable: false),
					Updated = c.DateTime(nullable: false),
				})
				.PrimaryKey(t => t.Id)
				.Index(t => t.Slug, unique: true);

			CreateTable(
				"dbo.PiranhaComments",
				c => new {
					Id = c.Guid(nullable: false),
					PostId = c.Guid(nullable: false),
					UserId = c.String(maxLength: 128),
					Author = c.String(maxLength: 128),
					Email = c.String(maxLength: 128),
					WebSite = c.String(maxLength: 128),
					Body = c.String(),
					IP = c.String(maxLength: 16),
					UserAgent = c.String(maxLength: 128),
					SessionID = c.String(maxLength: 64),
					IsApproved = c.Boolean(nullable: false),
					IsSpam = c.Boolean(nullable: false),
					Created = c.DateTime(nullable: false),
					Updated = c.DateTime(nullable: false),
				})
				.PrimaryKey(t => t.Id)
				.ForeignKey("dbo.PiranhaPosts", t => t.PostId, cascadeDelete: true)
				.Index(t => t.PostId);

			CreateTable(
				"dbo.PiranhaPosts",
				c => new {
					Id = c.Guid(nullable: false),
					TypeId = c.Guid(nullable: false),
					Slug = c.String(nullable: false, maxLength: 128),
					Excerpt = c.String(maxLength: 512),
					Body = c.String(),
					AuthorId = c.Guid(nullable: false),
					Title = c.String(nullable: false, maxLength: 128),
					Keywords = c.String(maxLength: 128),
					Description = c.String(maxLength: 255),
					Route = c.String(maxLength: 255),
					View = c.String(maxLength: 255),
					Created = c.DateTime(nullable: false),
					Updated = c.DateTime(nullable: false),
					Published = c.DateTime(),
				})
				.PrimaryKey(t => t.Id)
				.ForeignKey("dbo.PiranhaAuthors", t => t.AuthorId, cascadeDelete: true)
				.ForeignKey("dbo.PiranhaPostTypes", t => t.TypeId, cascadeDelete: true)
				.Index(t => new { t.TypeId, t.Slug }, unique: true, name: "IX_Slug")
				.Index(t => t.AuthorId);

			CreateTable(
				"dbo.PiranhaMedia",
				c => new {
					Id = c.Guid(nullable: false),
					Name = c.String(nullable: false, maxLength: 128),
					Slug = c.String(nullable: false, maxLength: 128),
					ContentType = c.String(),
					ContentLength = c.Long(nullable: false),
					IsImage = c.Boolean(nullable: false),
					Width = c.Int(),
					Height = c.Int(),
					Created = c.DateTime(nullable: false),
					Updated = c.DateTime(nullable: false),
				})
				.PrimaryKey(t => t.Id);

			CreateTable(
				"dbo.PiranhaPostTypes",
				c => new {
					Id = c.Guid(nullable: false),
					IncludeInRss = c.Boolean(nullable: false),
					EnableArchive = c.Boolean(nullable: false),
					MetaKeywords = c.String(maxLength: 128),
					MetaDescription = c.String(maxLength: 255),
					ArchiveTitle = c.String(maxLength: 128),
					ArchiveRoute = c.String(maxLength: 255),
					CommentRoute = c.String(maxLength: 255),
					Slug = c.String(nullable: false, maxLength: 128),
					Name = c.String(nullable: false, maxLength: 128),
					Description = c.String(maxLength: 255),
					Route = c.String(maxLength: 255),
					View = c.String(maxLength: 255),
					Created = c.DateTime(nullable: false),
					Updated = c.DateTime(nullable: false),
				})
				.PrimaryKey(t => t.Id)
				.Index(t => t.Slug, unique: true);

			CreateTable(
				"dbo.PiranhaPages",
				c => new {
					Id = c.Guid(nullable: false),
					TypeId = c.Guid(nullable: false),
					Slug = c.String(nullable: false, maxLength: 128),
					ParentId = c.Guid(),
					SortOrder = c.Int(nullable: false),
					IsHidden = c.Boolean(nullable: false),
					NavigationTitle = c.String(maxLength: 128),
					Body = c.String(),
					AuthorId = c.Guid(nullable: false),
					Title = c.String(nullable: false, maxLength: 128),
					Keywords = c.String(maxLength: 128),
					Description = c.String(maxLength: 255),
					Route = c.String(maxLength: 255),
					View = c.String(maxLength: 255),
					Created = c.DateTime(nullable: false),
					Updated = c.DateTime(nullable: false),
					Published = c.DateTime(),
				})
				.PrimaryKey(t => t.Id)
				.ForeignKey("dbo.PiranhaAuthors", t => t.AuthorId, cascadeDelete: true)
				.ForeignKey("dbo.PiranhaPageTypes", t => t.TypeId, cascadeDelete: true)
				.Index(t => t.TypeId)
				.Index(t => t.Slug, unique: true)
				.Index(t => t.AuthorId);

			CreateTable(
				"dbo.PiranhaPageTypes",
				c => new {
					Id = c.Guid(nullable: false),
					Slug = c.String(nullable: false, maxLength: 128),
					Name = c.String(nullable: false, maxLength: 128),
					Description = c.String(maxLength: 255),
					Route = c.String(maxLength: 255),
					View = c.String(maxLength: 255),
					Created = c.DateTime(nullable: false),
					Updated = c.DateTime(nullable: false),
				})
				.PrimaryKey(t => t.Id)
				.Index(t => t.Slug, unique: true);

			CreateTable(
				"dbo.PiranhaParams",
				c => new {
					Id = c.Guid(nullable: false),
					Name = c.String(nullable: false, maxLength: 128),
					Value = c.String(),
					Created = c.DateTime(nullable: false),
					Updated = c.DateTime(nullable: false),
				})
				.PrimaryKey(t => t.Id)
				.Index(t => t.Name, unique: true);

			CreateTable(
				"dbo.PiranhaPermissions",
				c => new {
					Id = c.Guid(nullable: false),
					Name = c.String(nullable: false, maxLength: 128),
					Description = c.String(maxLength: 255),
					Roles = c.String(maxLength: 128),
					Created = c.DateTime(nullable: false),
					Updated = c.DateTime(nullable: false),
				})
				.PrimaryKey(t => t.Id)
				.Index(t => t.Name, unique: true);

			CreateTable(
				"dbo.PiranhaPostMedia",
				c => new {
					Post_Id = c.Guid(nullable: false),
					Media_Id = c.Guid(nullable: false),
				})
				.PrimaryKey(t => new { t.Post_Id, t.Media_Id })
				.ForeignKey("dbo.PiranhaPosts", t => t.Post_Id, cascadeDelete: true)
				.ForeignKey("dbo.PiranhaMedia", t => t.Media_Id, cascadeDelete: true)
				.Index(t => t.Post_Id)
				.Index(t => t.Media_Id);

			CreateTable(
				"dbo.PiranhaPostCategories",
				c => new {
					Post_Id = c.Guid(nullable: false),
					Category_Id = c.Guid(nullable: false),
				})
				.PrimaryKey(t => new { t.Post_Id, t.Category_Id })
				.ForeignKey("dbo.PiranhaPosts", t => t.Post_Id, cascadeDelete: true)
				.ForeignKey("dbo.PiranhaCategories", t => t.Category_Id, cascadeDelete: true)
				.Index(t => t.Post_Id)
				.Index(t => t.Category_Id);

		}

		public override void Down() {
			DropForeignKey("dbo.PiranhaPages", "TypeId", "dbo.PiranhaPageTypes");
			DropForeignKey("dbo.PiranhaPages", "AuthorId", "dbo.PiranhaAuthors");
			DropForeignKey("dbo.PiranhaPosts", "TypeId", "dbo.PiranhaPostTypes");
			DropForeignKey("dbo.PiranhaComments", "PostId", "dbo.PiranhaPosts");
			DropForeignKey("dbo.PiranhaPostCategories", "Category_Id", "dbo.PiranhaCategories");
			DropForeignKey("dbo.PiranhaPostCategories", "Post_Id", "dbo.PiranhaPosts");
			DropForeignKey("dbo.PiranhaPosts", "AuthorId", "dbo.PiranhaAuthors");
			DropForeignKey("dbo.PiranhaPostMedia", "Media_Id", "dbo.PiranhaMedia");
			DropForeignKey("dbo.PiranhaPostMedia", "Post_Id", "dbo.PiranhaPosts");
			DropIndex("dbo.PiranhaPostCategories", new[] { "Category_Id" });
			DropIndex("dbo.PiranhaPostCategories", new[] { "Post_Id" });
			DropIndex("dbo.PiranhaPostMedia", new[] { "Media_Id" });
			DropIndex("dbo.PiranhaPostMedia", new[] { "Post_Id" });
			DropIndex("dbo.PiranhaPermissions", new[] { "Name" });
			DropIndex("dbo.PiranhaParams", new[] { "Name" });
			DropIndex("dbo.PiranhaPageTypes", new[] { "Slug" });
			DropIndex("dbo.PiranhaPages", new[] { "AuthorId" });
			DropIndex("dbo.PiranhaPages", new[] { "Slug" });
			DropIndex("dbo.PiranhaPages", new[] { "TypeId" });
			DropIndex("dbo.PiranhaPostTypes", new[] { "Slug" });
			DropIndex("dbo.PiranhaPosts", new[] { "AuthorId" });
			DropIndex("dbo.PiranhaPosts", "IX_Slug");
			DropIndex("dbo.PiranhaComments", new[] { "PostId" });
			DropIndex("dbo.PiranhaCategories", new[] { "Slug" });
			DropIndex("dbo.PiranhaAliases", new[] { "OldUrl" });
			DropTable("dbo.PiranhaPostCategories");
			DropTable("dbo.PiranhaPostMedia");
			DropTable("dbo.PiranhaPermissions");
			DropTable("dbo.PiranhaParams");
			DropTable("dbo.PiranhaPageTypes");
			DropTable("dbo.PiranhaPages");
			DropTable("dbo.PiranhaPostTypes");
			DropTable("dbo.PiranhaMedia");
			DropTable("dbo.PiranhaPosts");
			DropTable("dbo.PiranhaComments");
			DropTable("dbo.PiranhaCategories");
			DropTable("dbo.PiranhaBlocks");
			DropTable("dbo.PiranhaAuthors");
			DropTable("dbo.PiranhaAliases");
		}
	}
}
