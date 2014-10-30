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

namespace Piranha.Models
{
	/// <summary>
	/// Pages are used to create content positioned within the site structure.
	/// </summary>
	public sealed class Page : Base.Content<PageType>, Data.IModel, Data.IChanges, Data.IPublishable
	{
		#region Properties
		/// <summary>
		/// Gets/sets the id of the content type.
		/// </summary>
		public override Guid TypeId { get; set; }

		/// <summary>
		/// Gets/sets the unique slug.
		/// </summary>
		public override string Slug { get; set; }

		/// <summary>
		/// Gets/sets the optional parent id.
		/// </summary>
		public Guid? ParentId { get; set; }

		/// <summary>
		/// Gets/sets the sort order of the page.
		/// </summary>
		public int SortOrder { get; set; }

		/// <summary>
		/// Gets/sets if the page should be hidden from navigations.
		/// </summary>
		public bool IsHidden { get; set; }

		/// <summary>
		/// Gets/sets the alternate navigation title.
		/// </summary>
		public string NavigationTitle { get; set; }

		/// <summary>
		/// Gets/sets the main post body.
		/// </summary>
		public string Body { get; set; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Page() {
			SortOrder = 1;
		}

		#region Internal events
		/// <summary>
		/// Called when the model is materialized by the DbContext.
		/// </summary>
		/// <param name="db">The current db context</param>
		public override void OnLoad() {
			if (Hooks.Models.Page.OnLoad != null)
				Hooks.Models.Page.OnLoad(this);
		}

		/// <summary>
		/// Called before the model is saved by the DbContext.
		/// </summary>
		/// <param name="db">The current db context</param>
		public override void OnSave() {
			if (Hooks.Models.Page.OnSave != null)
				Hooks.Models.Page.OnSave(this);

			// Reposition pages.
			/*
			var old = db.Set<Page>()
				.Where(p => p.Id == this.Id)
				.SingleOrDefault();

			if (old == null) {
				Reposition((Db)db, this.Id, this.ParentId, this.SortOrder, true);
			} else if (old.ParentId != this.ParentId || old.SortOrder != this.SortOrder) {
				Reposition((Db)db, this.Id, old.ParentId, old.SortOrder + 1, false);
				Reposition((Db)db, this.Id, this.ParentId, this.SortOrder, true);
			}
			 */

			// Remove from model cache
			App.ModelCache.Pages.Remove(this.Id);
		}

		/// <summary>
		/// Called before the model is deleted by the DbContext.
		/// </summary>
		/// <param name="db">The current db context</param>
		public override void OnDelete() {
			if (Hooks.Models.Page.OnDelete != null)
				Hooks.Models.Page.OnDelete(this);

			// Reposition pages.
			/*
			Reposition((Db)db, this.Id, this.ParentId, this.SortOrder + 1, false);
			*/

			// Remove from model cache
			App.ModelCache.Pages.Remove(this.Id);
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Repositions the site structure.
		/// </summary>
		/// <param name="db">The current db context</param>
		/// <param name="id">The page id</param>
		/// <param name="parentId">The optional parent id</param>
		/// <param name="order">The order</param>
		/// <param name="increase">Whether or not to increase</param>
		//private void Reposition(Db db, Guid id, Guid? parentId, int order, bool increase) {
			/*
			if (parentId.HasValue) {
				db.Database.ExecuteSqlCommand("UPDATE PiranhaPages SET SortOrder = " + (increase ? "SortOrder + 1" : "SortOrder - 1") +
					" WHERE ParentId = {0} AND SortOrder >= {1}", parentId.Value, order);
			} else {
				db.Database.ExecuteSqlCommand("UPDATE PiranhaPages SET SortOrder = " + (increase ? "SortOrder + 1" : "SortOrder - 1") + 
					" WHERE ParentId IS NULL AND SortOrder >= {0}", order);
			}
			 */
		//}
		#endregion
	}
}
