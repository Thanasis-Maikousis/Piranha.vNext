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
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Piranha.Data;

namespace Piranha.EntityFramework
{
	/// <summary>
	/// Interface defining the different methods that should be provided
	/// by an open session to a document store.
	/// </summary>
	public class Session : ISession
	{
		#region Members
		/// <summary>
		/// The document session.
		/// </summary>
		private readonly Db db;
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="session">The session</param>
		public Session() {
			db = new Db();
		}

		/// <summary>
		/// Gets the document identified by the given id.
		/// </summary>
		/// <typeparam name="T">The document type</typeparam>
		/// <param name="id">The unique id</param>
		/// <returns>The document</returns>
		public T GetSingle<T>(Guid id) where T : class, Data.IModel {
			return Query<T>().Where(m => m.Id == id).SingleOrDefault();
		}

		/// <summary>
		/// Gets the documents matching the given expression.
		/// </summary>
		/// <typeparam name="T">The document type</typeparam>
		/// <param name="where">The optional where expression</param>
		/// <returns>The matching documents</returns>
		public IEnumerable<T> Get<T>(Expression<Func<T, bool>> where = null, int? limit = null, Func<IQueryable<T>, IQueryable<T>> order = null) 
			where T : class, Data.IModel 
		{
			IQueryable<T> query = Query<T>();

			if (where != null)
				query = query.Where(where);
			if (order != null)
				query = order(query);
			if (limit.HasValue)
				query = query.Take(limit.Value);
			return query.ToList();
		}

		/// <summary>
		/// Adds the given document to the session.
		/// </summary>
		/// <typeparam name="T">The document type</typeparam>
		/// <param name="document">The document</param>
		public void Add<T>(T document) where T : class, Data.IModel {
			db.Set<T>().Add(document);
		}

		/// <summary>
		/// Removes the given document from the session.
		/// </summary>
		/// <typeparam name="T">The document type</typeparam>
		/// <param name="document">The document</param>
		public void Remove<T>(T document) where T : class, Data.IModel {
			db.Set<T>().Remove(document);
		}

		/// <summary>
		/// Saves the changes made to the current session.
		/// </summary>
		public void SaveChanges() {
			db.SaveChanges();
		}

		/// <summary>
		/// Disposes the session.
		/// </summary>
		public void Dispose() {
			db.Dispose();
			GC.SuppressFinalize(this);
		}

		#region Private methods
		/// <summary>
		/// Gets the base query.
		/// </summary>
		/// <typeparam name="T">The model type</typeparam>
		/// <returns>The query</returns>
		private IQueryable<T> Query<T>() where T : class, Data.IModel {
			if (typeof(Models.Post) == typeof(T)) {
				return (IQueryable<T>)db.Posts
					.Include(p => p.Author)
					.Include(p => p.Attachments)
					.Include(p => p.Categories)
					.Include(p => p.Type);
			} else if (typeof(Models.Page) == typeof(T)) {
				return (IQueryable<T>)db.Pages
					.Include(p => p.Author)
					.Include(p => p.Type);
			}
			return db.Set<T>();
		}
		#endregion
	}
}
