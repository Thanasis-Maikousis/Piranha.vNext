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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Piranha.Repositories
{
	/// <summary>
	/// Base repository.
	/// </summary>
	/// <typeparam name="T">The repository type</typeparam>
	public abstract class Repository<T> where T : class, Data.IModel
	{
		#region Members
		/// <summary>
		/// The protected storage session. 
		/// </summary>
		protected readonly Data.ISession session;
		#endregion
	
		/// <summary>
		/// Default internal constructor.
		/// </summary>
		/// <param name="session">The current session</param>
		public Repository(Data.ISession session) {
			this.session = session;
		}

		/// <summary>
		/// Gets the model identified by the given id.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <returns>The model</returns>
		public virtual T GetSingle(Guid id) { 
			return FromDb(session.GetSingle<T>(id));
		}

		/// <summary>
		/// Gets the model matching the given where expression.
		/// </summary>
		/// <param name="where">The where expression</param>
		/// <returns>The model</returns>
		public virtual T GetSingle(Expression<Func<T, bool>> where = null) {
			var model = session.Get<T>(where).SingleOrDefault();

			if (model != null)
				return FromDb(model);
			return null;
		}

		/// <summary>
		/// Gets the models matching the given where expression.
		/// </summary>
		/// <param name="where">The optional where expression</param>
		/// <returns>The matching models</returns>
		public virtual IEnumerable<T> Get(Expression<Func<T, bool>> where = null, int? limit = null, Func<IQueryable<T>, IQueryable<T>> order = null) {
			var models = session.Get<T>(where, limit, order);

			foreach (var model in models)
				FromDb(model);
			return models;
		}

		/// <summary>
		/// Adds a new model to the current session.
		/// </summary>
		/// <param name="model">The model</param>
		public virtual void Add(T model) {
			// Ensure id
			if (model.Id == Guid.Empty)
				model.Id = Guid.NewGuid();

			var errors = Validate(model);
			if (errors.Count() > 0)
				throw new Data.ModelException("Model validation failed", errors);

			session.Add<T>(model);
		}

		/// <summary>
		/// Removes the given model from the current session.
		/// </summary>
		/// <param name="model">The model</param>
		public virtual void Remove(T model) { 
			session.Remove<T>(model);
		}

		/// <summary>
		/// Removes the model with the given id from the current session.
		/// </summary>
		/// <param name="id">The unique id</param>
		public virtual void Remove(Guid id) {
			var doc = session.GetSingle<T>(id);

			if (doc != null)
				Remove(doc);
		}

		/// <summary>
		/// Applies the default sort order for the query.
		/// </summary>
		/// <param name="query">The query</param>
		/// <returns>The ordered query</returns>
		protected virtual IQueryable<T> Order(IQueryable<T> query) {
			return query;
		}

		/// <summary>
		/// Maps the source model to the destination.
		/// </summary>
		/// <param name="model">The source model</param>
		protected virtual T FromDb(T model) {
			return model;
		}

		/// <summary>
		/// Maps the source model to the destination.
		/// </summary>
		/// <param name="model">The source model</param>
		protected virtual T ToDb(T model) {
			return model;
		}

		/// <summary>
		/// Validates the model before storing it in the datastore.
		/// </summary>
		/// <param name="model">The current model</param>
		/// <returns>Any possible validation errors</returns>
		protected virtual IEnumerable<Data.ModelError> Validate(T model) {
			return new Data.ModelError[0];
		}
	}
}