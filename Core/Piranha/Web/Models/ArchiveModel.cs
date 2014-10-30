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

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Piranha.Web.Models
{
	/// <summary>
	/// Application post archive model.
	/// </summary>
	public class ArchiveModel
	{
		#region Properties
		/// <summary>
		/// Gets/ses the unique id.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets/sets the title.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Gets/sets the unique slug.
		/// </summary>
		public string Slug { get; set; }

		/// <summary>
		/// Gets/sets the meta keywords.
		/// </summary>
		public string Keywords { get; set; }

		/// <summary>
		/// Gets/sets the meta description.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets/sets the optionally requested year.
		/// </summary>
		public int? Year { get; set; }

		/// <summary>
		/// Gets/sets the optionally requested month.
		/// </summary>
		public int? Month { get; set; }

		/// <summary>
		/// Gets/sets the current page.
		/// </summary>
		public int Page { get; set; }

		/// <summary>
		/// Gets/sets the total number of pages available.
		/// </summary>
		public int TotalPages { get; set; }

		/// <summary>
		/// Gets/sets the available posts.
		/// </summary>
		public IList<PostModel> Posts { get; set; }
		#endregion

		#region Calculated properties
		/// <summary>
		/// Gets if the archive is for a specific year.
		/// </summary>
		public bool HasYear {
			get { return Year.HasValue;  }
		}

		/// <summary>
		/// Gets if the archive is for a specific year & month.
		/// </summary>
		public bool HasMonth {
			get { return HasYear && Month.HasValue; }
		}

		/// <summary>
		/// Gets if there's a previous page for the current archive.
		/// </summary>
		public bool HasPrev {
			get { return Page > 1; }
		}

		/// <summary>
		/// Gets if there's a next page for the current archive.
		/// </summary>
		public bool HasNext {
			get { return Page < TotalPages; }
		}

		/// <summary>
		/// Gets the href for the previous page in the current archive.
		/// </summary>
		public string LinkPrev {
			get {
				if (HasPrev) {
					return Utils.Url("~/" + Slug +
						(HasYear ? "/" + Year.ToString() : "") +
						(HasMonth ? "/" + Month.ToString() : "") +
						(Page > 2 ? "/page/" + (Page - 1) : ""));
				}
				return "";
			}
		}

		/// <summary>
		/// Gets the href for the next page in the current archive.
		/// </summary>
		public string LinkNext {
			get {
				if (HasNext) {
					return Utils.Url("~/" + Slug +
						(HasYear ? "/" + Year.ToString() : "") +
						(HasMonth ? "/" + Month.ToString() : "") +
						"/page/" + (Page + 1));
				}
				return "";
			}
		}
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ArchiveModel() {
			Posts = new List<PostModel>();
		}

		/// <summary>
		/// Gets the archive model for the post type identitified
		/// by the given slug.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <param name="page">The optional page</param>
		/// <param name="year">The optional year</param>
		/// <param name="month">The optional month</param>
		/// <returns>The archive model</returns>
		public static ArchiveModel GetById(Guid id, int? page = 1, int? year = null, int? month = null) {
			return GetById<ArchiveModel>(id, page, year, month);
		}

		/// <summary>
		/// Gets the archive model for the post type identitified
		/// by the given slug.
		/// </summary>
		/// <param name="id">The unique id</param>
		/// <param name="page">The optional page</param>
		/// <param name="year">The optional year</param>
		/// <param name="month">The optional month</param>
		/// <returns>The archive model</returns>
		public static T GetById<T>(Guid id, int? page = 1, int? year = null, int? month = null) where T : ArchiveModel {
			using (var api = new Api()) {
				return Map<T>(api, api.PostTypes.GetSingle(where: t => t.Id == id), page, year, month);
			}
		}

		/// <summary>
		/// Gets the archive model for the post type identitified
		/// by the given slug.
		/// </summary>
		/// <param name="slug">The unique slug</param>
		/// <param name="page">The optional page</param>
		/// <param name="year">The optional year</param>
		/// <param name="month">The optional month</param>
		/// <returns>The archive model</returns>
		public static ArchiveModel GetBySlug(string slug, int? page = 1, int? year = null, int? month = null) {
			return GetBySlug<ArchiveModel>(slug, page, year, month);
		}

		/// <summary>
		/// Gets the archive model for the post type identitified
		/// by the given slug.
		/// </summary>
		/// <param name="slug">The unique slug</param>
		/// <param name="page">The optional page</param>
		/// <param name="year">The optional year</param>
		/// <param name="month">The optional month</param>
		/// <returns>The archive model</returns>
		public static T GetBySlug<T>(string slug, int? page = 1, int? year = null, int? month = null) where T : ArchiveModel {
			using (var api = new Api()) {
				return Map<T>(api, api.PostTypes.GetSingle(where: t => t.Slug == slug), page, year, month);
			}
		}

		#region Private methods
		/// <summary>
		/// Maps the given post type to an archive model.
		/// </summary>
		/// <param name="api">The current api</param>
		/// <param name="type">The post type</param>
		/// <param name="page">The requested page</param>
		/// <param name="year">Optional year</param>
		/// <param name="month">Optional month</param>
		/// <returns>The archive model</returns>
		private static T Map<T>(Api api, Piranha.Models.PostType type, int? page, int? year, int? month) where T : ArchiveModel {
			if (type != null) {
				var model = Activator.CreateInstance<T>();
				
				Mapper.Map<Piranha.Models.PostType, ArchiveModel>(type, model);

				model.Year = year;
				model.Month = month;
				model.Page = page.HasValue ? page.Value : 1;

				// Build query
				Expression<Func<Piranha.Models.Post, bool>> query = null;
				var now = DateTime.Now.ToUniversalTime();

				if (year.HasValue) {
					DateTime from;
					DateTime to;

					if (month.HasValue) {
						from = new DateTime(year.Value, month.Value, 1);
						to = from.AddMonths(1);
					} else {
						from = new DateTime(year.Value, 1, 1);
						to = from.AddYears(1);
					}
					query = p => p.TypeId == type.Id && p.Published <= now && p.Published >= from && p.Published < to;
				} else {
					query = p => p.TypeId == type.Id && p.Published <= now;
				}

				// Get posts
				var posts = api.Posts.Get(where: query).Select(p => p.Id).ToList();
				var count = posts.Count();

				// Get pages
				model.TotalPages = Math.Max(Convert.ToInt32(Math.Ceiling((double)count / Config.Site.ArchivePageSize)), 1);
				model.Page = Math.Min(model.Page, model.TotalPages);

				for (var n = (model.Page - 1) * Config.Site.ArchivePageSize; n < Math.Min(model.Page * Config.Site.ArchivePageSize, count); n++)
					model.Posts.Add(PostModel.GetById(posts[n]));

				return model;
			}
			return null;
		}
		#endregion
	}
}
