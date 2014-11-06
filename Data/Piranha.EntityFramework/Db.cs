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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Threading;

namespace Piranha.EntityFramework
{
	/// <summary>
	/// The internal db context.
	/// </summary>
	internal class Db : BaseContext<Db>
	{
		#region Db sets
		/// <summary>
		/// Gets/sets the alias set.
		/// </summary>
		public DbSet<Models.Alias> Aliases { get; set; }

		/// <summary>
		/// Gets/sets the author set.
		/// </summary>
		public DbSet<Models.Author> Authors { get; set; }

		/// <summary>
		/// Gets/sets the category set.
		/// </summary>
		public DbSet<Models.Category> Categories { get; set; }

		/// <summary>
		/// Gets/sets the comment set.
		/// </summary>
		public DbSet<Models.Comment> Comments { get; set; }

		/// <summary>
		/// Gets/sets the block set.
		/// </summary>
		public DbSet<Models.Block> Blocks { get; set; }

		/// <summary>
		/// Gets/sets the media set.
		/// </summary>
		public DbSet<Models.Media> Media { get; set; }

		/// <summary>
		/// Gets/sets the page set.
		/// </summary>
		public DbSet<Models.Page> Pages { get; set; }

		/// <summary>
		/// Gets/sets the page type set.
		/// </summary>
		public DbSet<Models.PageType> PageTypes { get; set; }

		/// <summary>
		/// Gets/sets the param set.
		/// </summary>
		public DbSet<Models.Param> Params { get; set; }

		/// <summary>
		/// Gets/sets the post set.
		/// </summary>
		public DbSet<Models.Post> Posts { get; set; }

		/// <summary>
		/// Gets/sets the post type set.
		/// </summary>
		public DbSet<Models.PostType> PostTypes { get; set; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Db() : base("piranha", new MigrateDatabaseToLatestVersion<Db, Migrations.Configuration>()) {}

		/// <summary>
		/// Configures the data model.
		/// </summary>
		/// <param name="modelBuilder">The current model builder</param>
		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			// Alias
			modelBuilder.Entity<Models.Alias>().ToTable("PiranhaAliases");
			modelBuilder.Entity<Models.Alias>().Property(a => a.NewUrl).HasMaxLength(255).IsRequired();
			modelBuilder.Entity<Models.Alias>().Property(a => a.OldUrl).HasMaxLength(255).IsRequired()
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute() {
					IsUnique = true
				}));

			// Author
			modelBuilder.Entity<Models.Author>().ToTable("PiranhaAuthors");
			modelBuilder.Entity<Models.Author>().Property(a => a.Name).HasMaxLength(128).IsRequired();
			modelBuilder.Entity<Models.Author>().Property(a => a.Email).HasMaxLength(128);
			modelBuilder.Entity<Models.Author>().Property(a => a.Description).HasMaxLength(512);

			// Block
			modelBuilder.Entity<Models.Block>().ToTable("PiranhaBlocks");
			modelBuilder.Entity<Models.Block>().Property(b => b.Name).HasMaxLength(128).IsRequired();
			modelBuilder.Entity<Models.Block>().Property(b => b.Description).HasMaxLength(255);
			modelBuilder.Entity<Models.Block>().Property(b => b.Slug).HasMaxLength(128).IsRequired()
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute() {
					IsUnique = true
				}));
	
			// Category
			modelBuilder.Entity<Models.Category>().ToTable("PiranhaCategories");
			modelBuilder.Entity<Models.Category>().Property(c => c.Title).HasMaxLength(128).IsRequired();
			modelBuilder.Entity<Models.Category>().Property(c => c.Slug).HasMaxLength(128).IsRequired()
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute() {
					IsUnique = true
				}));

			// Comment
			modelBuilder.Entity<Models.Comment>().ToTable("PiranhaComments");
			modelBuilder.Entity<Models.Comment>().Property(c => c.UserId).HasMaxLength(128);
			modelBuilder.Entity<Models.Comment>().Property(c => c.Author).HasMaxLength(128);
			modelBuilder.Entity<Models.Comment>().Property(c => c.Email).HasMaxLength(128);
			modelBuilder.Entity<Models.Comment>().Property(c => c.IP).HasMaxLength(16);
			modelBuilder.Entity<Models.Comment>().Property(c => c.UserAgent).HasMaxLength(128);
			modelBuilder.Entity<Models.Comment>().Property(c => c.WebSite).HasMaxLength(128);
			modelBuilder.Entity<Models.Comment>().Property(c => c.SessionID).HasMaxLength(64);

			// Media
			modelBuilder.Entity<Models.Media>().ToTable("PiranhaMedia");
			modelBuilder.Entity<Models.Media>().Property(m => m.Name).HasMaxLength(128).IsRequired();
			modelBuilder.Entity<Models.Media>().Property(m => m.Slug).HasMaxLength(128).IsRequired()
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute() {
					IsUnique = true
				}));
				
			// Page
			modelBuilder.Entity<Models.Page>().ToTable("PiranhaPages");
			modelBuilder.Entity<Models.Page>().Property(p => p.Title).HasMaxLength(128).IsRequired();
			modelBuilder.Entity<Models.Page>().Property(p => p.NavigationTitle).HasMaxLength(128);
			modelBuilder.Entity<Models.Page>().Property(p => p.Keywords).HasMaxLength(128);
			modelBuilder.Entity<Models.Page>().Property(p => p.Description).HasMaxLength(255);
			modelBuilder.Entity<Models.Page>().Property(p => p.Route).HasMaxLength(255);
			modelBuilder.Entity<Models.Page>().Property(p => p.View).HasMaxLength(255);
			modelBuilder.Entity<Models.Page>().Property(p => p.Slug).HasMaxLength(128).IsRequired()
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute() {
					IsUnique = true
				}));

			// PageType
			modelBuilder.Entity<Models.PageType>().ToTable("PiranhaPageTypes");
			modelBuilder.Entity<Models.PageType>().Property(t => t.Name).HasMaxLength(128).IsRequired();
			modelBuilder.Entity<Models.PageType>().Property(t => t.Description).HasMaxLength(255);
			modelBuilder.Entity<Models.PageType>().Property(t => t.Route).HasMaxLength(255);
			modelBuilder.Entity<Models.PageType>().Property(t => t.View).HasMaxLength(255);
			modelBuilder.Entity<Models.PageType>().Property(t => t.Slug).HasMaxLength(128).IsRequired()
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute() {
					IsUnique = true
				}));

			// Param
			modelBuilder.Entity<Models.Param>().ToTable("PiranhaParams");
			modelBuilder.Entity<Models.Param>().Property(p => p.Name).HasMaxLength(128).IsRequired()
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute() {
					IsUnique = true
				}));

			// Post
			modelBuilder.Entity<Models.Post>().ToTable("PiranhaPosts");
			modelBuilder.Entity<Models.Post>().Property(p => p.Title).HasMaxLength(128).IsRequired();
			modelBuilder.Entity<Models.Post>().Property(p => p.Keywords).HasMaxLength(128);
			modelBuilder.Entity<Models.Post>().Property(p => p.Description).HasMaxLength(255);
			modelBuilder.Entity<Models.Post>().Property(p => p.Route).HasMaxLength(255);
			modelBuilder.Entity<Models.Post>().Property(p => p.View).HasMaxLength(255);
			modelBuilder.Entity<Models.Post>().Property(p => p.Excerpt).HasMaxLength(512);
			modelBuilder.Entity<Models.Post>().Property(p => p.TypeId)
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Slug") {
					Order = 1,
					IsUnique = true
				}));
			modelBuilder.Entity<Models.Post>().Property(p => p.Slug).HasMaxLength(128).IsRequired()
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Slug") {
					Order = 2,
					IsUnique = true
				}));
			modelBuilder.Entity<Models.Post>().Ignore(p => p.CommentCount);
			modelBuilder.Entity<Models.Post>().HasMany(p => p.Attachments).WithMany().Map(m => m.ToTable("PiranhaPostMedia"));
			modelBuilder.Entity<Models.Post>().HasMany(p => p.Categories).WithMany().Map(m => m.ToTable("PiranhaPostCategories"));

			// PostType
			modelBuilder.Entity<Models.PostType>().ToTable("PiranhaPostTypes");
			modelBuilder.Entity<Models.PostType>().Property(t => t.Name).HasMaxLength(128).IsRequired();
			modelBuilder.Entity<Models.PostType>().Property(t => t.Description).HasMaxLength(255);
			modelBuilder.Entity<Models.PostType>().Property(t => t.Route).HasMaxLength(255);
			modelBuilder.Entity<Models.PostType>().Property(t => t.View).HasMaxLength(255);
			modelBuilder.Entity<Models.PostType>().Property(p => p.ArchiveTitle).HasMaxLength(128);
			modelBuilder.Entity<Models.PostType>().Property(p => p.MetaKeywords).HasMaxLength(128);
			modelBuilder.Entity<Models.PostType>().Property(p => p.MetaDescription).HasMaxLength(255);
			modelBuilder.Entity<Models.PostType>().Property(p => p.ArchiveRoute).HasMaxLength(255);
			modelBuilder.Entity<Models.PostType>().Property(p => p.ArchiveView).HasMaxLength(255);
			modelBuilder.Entity<Models.PostType>().Property(p => p.CommentRoute).HasMaxLength(255);
			modelBuilder.Entity<Models.PostType>().Property(t => t.Slug).HasMaxLength(128).IsRequired()
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute() {
					IsUnique = true
				}));

			base.OnModelCreating(modelBuilder);
		}
	}
}