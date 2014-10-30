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

namespace Piranha
{
	/// <summary>
	/// The main data api.
	/// </summary>
	public sealed class Api : IDisposable
	{
		#region Members
		/// <summary>
		/// The private db context.
		/// </summary>
		internal readonly Data.ISession session;

		/// <summary>
		/// If the context is external or created within the API.
		/// </summary>
		private readonly bool isExternal;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the alias repository.
		/// </summary>
		public Repositories.AliasRepository Aliases { get; private set; }

		/// <summary>
		/// Gets the author repository.
		/// </summary>
		public Repositories.AuthorRepository Authors { get; private set; }

		/// <summary>
		/// Gets the block repository.
		/// </summary>
		public Repositories.BlockRepository Blocks { get; private set; }

		/// <summary>
		/// Gets the category repository.
		/// </summary>
		public Repositories.CategoryRepository Categories { get; private set; }

		/// <summary>
		/// Gets the comment repository.
		/// </summary>
		public Repositories.CommentRepository Comments { get; private set; }

		/// <summary>
		/// Gets the page repository.
		/// </summary>
		public Repositories.PageRepository Pages { get; private set; }

		/// <summary>
		/// Gets the page type repository.
		/// </summary>
		public Repositories.PageTypeRepository PageTypes { get; private set; }

		/// <summary>
		/// Gets the param repository.
		/// </summary>
		public Repositories.ParamRepository Params { get; private set; }

		/// <summary>
		/// Gets the post repository.
		/// </summary>
		public Repositories.PostRepository Posts { get; private set; }

		/// <summary>
		/// Gets the post type repository.
		/// </summary>
		public Repositories.PostTypeRepository PostTypes { get; private set; }

		/// <summary>
		/// Gets the media repository.
		/// </summary>
		public Repositories.MediaRepository Media { get; private set; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Api() : this(null) { }

		/// <summary>
		/// Creates a new API on an already open session.
		/// </summary>
		/// <param name="session">The session</param>
		internal Api(Data.ISession session) {
			this.session = session != null ? session : App.Store.OpenSession();
			isExternal = session != null;

			Aliases = new Repositories.AliasRepository(this.session);
			Authors = new Repositories.AuthorRepository(this.session);
			Blocks = new Repositories.BlockRepository(this.session);
			Categories = new Repositories.CategoryRepository(this.session);
			Comments = new Repositories.CommentRepository(this.session);
			Pages = new Repositories.PageRepository(this.session);
			PageTypes = new Repositories.PageTypeRepository(this.session);
			Params = new Repositories.ParamRepository(this.session);
			Posts = new Repositories.PostRepository(this.session);
			PostTypes = new Repositories.PostTypeRepository(this.session);
			Media = new Repositories.MediaRepository(this.session);
		}

		/// <summary>
		/// Saves the changes made to the api.
		/// </summary>
		/// <returns>The number of saved rows</returns>
		public void SaveChanges() {
			session.SaveChanges();
		}

		/// <summary>
		/// Disposes the current api.
		/// </summary>
		public void Dispose() {
			if (!isExternal)
				session.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}