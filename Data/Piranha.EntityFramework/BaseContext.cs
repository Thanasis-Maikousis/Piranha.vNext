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
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Threading;

namespace Piranha.EntityFramework
{
	/// <summary>
	/// Base class for creating a db context with the built in Piranha CMS
	/// events and features.
	/// </summary>
	public abstract class BaseContext<T> : DbContext where T : BaseContext<T>
	{
		/// <summary>
		/// Default constructor. Creates a new db context instance.
		/// </summary>
		/// <param name="nameOrConnectionString">The name of, or the connection string</param>
		public BaseContext(string nameOrConnectionString, IDatabaseInitializer<T> initializer = null) : base(nameOrConnectionString) {
			// Configure initializer
			if (initializer != null)
				Database.SetInitializer(initializer);

			// Attach OnLoad event
			((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += 
				new ObjectMaterializedEventHandler(OnLoad);
		}

		/// <summary>
		/// Saves the changes made to the data context.
		/// </summary>
		/// <returns>The number of saved changes</returns>
		public override int SaveChanges() {
			DbUtils.OnSave(this);

			return base.SaveChanges();
		}

		/// <summary>
		/// Saves the changes made to the data context.
		/// </summary>
		/// <param name="cancellationToken">The cancellationToken</param>
		/// <returns>The number of saved changes</returns>
		public override System.Threading.Tasks.Task<int> SaveChangesAsync(CancellationToken cancellationToken) {
			DbUtils.OnSave(this);

			return base.SaveChangesAsync(cancellationToken);
		}

		#region Protected methods
		/// <summary>
		/// Processes the loaded entities before returning them.
		/// </summary>
		protected virtual void OnLoad(object sender, ObjectMaterializedEventArgs e) {
			DbUtils.OnLoad(this, e);
		}
		#endregion
	}
}
