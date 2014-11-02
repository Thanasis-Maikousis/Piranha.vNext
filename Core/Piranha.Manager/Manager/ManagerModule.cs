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
using Piranha.Extend;

namespace Piranha.Manager
{
	/// <summary>
	/// The main entry point for the manager module.
	/// </summary>
	public class ManagerModule : IModule
	{
		/// <summary>
		/// Initializes the module. This method should be used for
		/// ensuring runtime resources and registering hooks.
		/// </summary>
		public void Init() {
			// Author
			Mapper.CreateMap<Piranha.Models.Author, Models.Author.ListItem>()
				.ForMember(a => a.Created, o => o.MapFrom(m => m.Created.ToString("yyyy-MM-dd")))
				.ForMember(a => a.Updated, o => o.MapFrom(m => m.Updated.ToString("yyyy-MM-dd")));

			// Block
			Mapper.CreateMap<Piranha.Models.Block, Models.Block.ListItem>()
				.ForMember(b => b.Created, o => o.MapFrom(m => m.Created.ToString("yyyy-MM-dd")))
				.ForMember(b => b.Updated, o => o.MapFrom(m => m.Updated.ToString("yyyy-MM-dd")));
			Mapper.CreateMap<Piranha.Models.Block, Models.Block.EditModel>();
			Mapper.CreateMap<Models.Block.EditModel, Piranha.Models.Block>()
				.ForMember(b => b.Id, o => o.Ignore())
				.ForMember(b => b.Created, o => o.Ignore())
				.ForMember(b => b.Updated, o => o.Ignore());

			// Post
			Mapper.CreateMap<Piranha.Models.Post, Models.Post.EditModel>()
				.ForMember(p => p.Authors, o => o.Ignore())
				.ForMember(p => p.Categories, o => o.Ignore())
				.ForMember(p => p.Comments, o => o.Ignore())
				.ForMember(p => p.SelectedCategories, o => o.Ignore())
				.ForMember(p => p.Action, o => o.Ignore());
			Mapper.CreateMap<Models.Post.EditModel, Piranha.Models.Post>()
				.ForMember(p => p.Id, o => o.Ignore())
				.ForMember(p => p.Type, o => o.Ignore())
				.ForMember(p => p.Author, o => o.Ignore())
				.ForMember(p => p.Attachments, o => o.Ignore())
				.ForMember(p => p.Comments, o => o.Ignore())
				.ForMember(p => p.CommentCount, o => o.Ignore())
				.ForMember(p => p.Categories, o => o.Ignore())
				.ForMember(p => p.Created, o => o.Ignore())
				.ForMember(p => p.Updated, o => o.Ignore())
				.ForMember(p => p.Published, o => o.Ignore());

			// Post type
			Mapper.CreateMap<Piranha.Models.PostType, Models.PostType.EditModel>();
			Mapper.CreateMap<Models.PostType.EditModel, Piranha.Models.PostType>()
				.ForMember(t => t.Id, o => o.Ignore())
				.ForMember(t => t.IncludeInRss, o => o.Ignore())
				.ForMember(t => t.Created, o => o.Ignore())
				.ForMember(t => t.Updated, o => o.Ignore());

			// User
			Mapper.CreateMap<Piranha.Models.Author, Models.Author.EditModel>();
			Mapper.CreateMap<Models.Author.EditModel, Piranha.Models.Author>()
				.ForMember(a => a.Id, o => o.Ignore())
				.ForMember(a => a.Created, o => o.Ignore())
				.ForMember(a => a.Updated, o => o.Ignore());

			Mapper.AssertConfigurationIsValid();
		}
	}
}