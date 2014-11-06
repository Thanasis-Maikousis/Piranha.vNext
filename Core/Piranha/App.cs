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
using System.Text.RegularExpressions;

namespace Piranha
{
	/// <summary>
	/// This is the main singleton application instance for Piranha.
	/// </summary>
	public sealed class App
	{
		#region Inner classes
		/// <summary>
		/// Class for configuring the app instance.
		/// </summary>
		public sealed class Config
		{
			/// <summary>
			/// The configured log provider.
			/// </summary>
			public Log.ILog Log;

			/// <summary>
			/// The configured cache provider.
			/// </summary>
			public Cache.ICache Cache;

			/// <summary>
			/// The configured media provider.
			/// </summary>
			public IO.IMedia Media;

			/// <summary>
			/// The configured security provider.
			/// </summary>
			public Security.ISecurity Security;

			/// <summary>
			/// The configured data store.
			/// </summary>
			public Data.IStore Store;

			/// <summary>
			/// The configured slug generation algorithm.
			/// </summary>
			public Func<string, string> GenerateSlug;
		}
		#endregion

		#region Members
		/// <summary>
		/// The singleton application instance.
		/// </summary>
		public readonly static App Instance = new App();

		/// <summary>
		/// Initialization mutex.
		/// </summary>
		private object mutex = new object();

		/// <summary>
		/// The private application config.
		/// </summary>
		private Config config;

		/// <summary>
		/// The private model cache.
		/// </summary>
		private Cache.AppCache modelCache;

		/// <summary>
		/// The private handler collection.
		/// </summary>
		private Web.HandlerCollection handlers;

		/// <summary>
		/// The private extension manager.
		/// </summary>
		private Extend.ExtensionManager extensions;
		#endregion

		#region Properties
		/// <summary>
		/// Gets if the application has been initialized.
		/// </summary>
		public bool IsInitialized { get; private set; }

		/// <summary>
		/// Gets the currently configured cache provider.
		/// </summary>
		public static Cache.ICache Cache {
			get { return Instance.config.Cache; }
		}

		/// <summary>
		/// Gets the currently configured log provider.
		/// </summary>
		public static Log.ILog Logger {
			get { return Instance.config.Log; }
		}

		/// <summary>
		/// Gets the currently configured media provider.
		/// </summary>
		public static IO.IMedia Media {
			get { return Instance.config.Media; }
		}

		/// <summary>
		/// Gets the currently configured security provider.
		/// </summary>
		public static Security.ISecurity Security {
			get { return Instance.config.Security;  }
		}

		/// <summary>
		/// Gets the currently configured data store.
		/// </summary>
		public static Data.IStore Store {
			get { return Instance.config.Store; }
		}

		/// <summary>
		/// Gets the currently configured handlers.
		/// </summary>
		public static Web.HandlerCollection Handlers {
			get { return Instance.handlers; }
		}

		/// <summary>
		/// Gets the extension manager.
		/// </summary>
		public static Extend.ExtensionManager Extensions {
			get { return Instance.extensions; }
		}

		/// <summary>
		/// Gets the currently configured model cache.
		/// </summary>
		internal static Cache.AppCache ModelCache {
			get { return Instance.modelCache; }
		}

		/// <summary>
		/// Gets the configured slug generation algorithm.
		/// </summary>
		internal static Func<string, string> GenerateSlug {
			get { return Instance.config.GenerateSlug; }
		}
		#endregion
		
		/// <summary>
		/// Private constructor.
		/// </summary>
		private App() { }

		/// <summary>
		/// Initializes Piranha CMS.
		/// </summary>
		/// <param name="configure">The optional app configuration</param>
		public static void Init(Action<Config> configure = null) {
			var config = new Config();

			// Run configuration if provided
			if (configure != null)
				configure(config);

			Instance.Initialize(config);
		}

		#region Private methods
		/// <summary>
		/// Initializes the application instance.
		/// </summary>
		private void Initialize(Config config) {
			if (!IsInitialized) {
				lock (mutex) {
					if (!IsInitialized) {
						// Store configuration
						this.config = config;

						// Register logger
						if (config.Log == null)
							config.Log = new Log.FileLog();
						Logger.Log(Log.LogLevel.INFO, "App.Init: Starting application");

						// Configure auto mapper
						Mapper.CreateMap<Models.Page, Web.Models.PageModel>()
							.ForMember(m => m.Type, o => o.MapFrom(p => p.Type.Slug))
							.ForMember(m => m.Route, o => o.MapFrom(p => !String.IsNullOrEmpty(p.Route) ? p.Route : p.Type.Route))
							.ForMember(m => m.View, o => o.MapFrom(p => !String.IsNullOrEmpty(p.View) ? p.View : p.Type.View));
						Mapper.CreateMap<Models.Post, Web.Models.PostModel>()
							.ForMember(m => m.Type, o => o.MapFrom(p => p.Type.Slug))
							.ForMember(m => m.Route, o => o.MapFrom(p => !String.IsNullOrEmpty(p.Route) ? p.Route : p.Type.Route))
							.ForMember(m => m.View, o => o.MapFrom(p => !String.IsNullOrEmpty(p.View) ? p.View : p.Type.View));
						Mapper.CreateMap<Models.PostType, Web.Models.ArchiveModel>()
							.ForMember(m => m.Keywords, o => o.MapFrom(t => t.MetaKeywords))
							.ForMember(m => m.Description, o => o.MapFrom(t => t.MetaDescription))
							.ForMember(m => m.View, o => o.MapFrom(t => t.ArchiveView))
							.ForMember(m => m.Title, o => o.MapFrom(t => t.ArchiveTitle))
							.ForMember(m => m.Year, o => o.Ignore())
							.ForMember(m => m.Month, o => o.Ignore())
							.ForMember(m => m.Page, o => o.Ignore())
							.ForMember(m => m.TotalPages, o => o.Ignore())
							.ForMember(m => m.Posts, o => o.Ignore());

						Mapper.AssertConfigurationIsValid();

						// Register the security provider
						Logger.Log(Log.LogLevel.INFO, "App.Init: Registering security provider");
						if (config.Security == null)
							config.Security = new Security.SimpleSecurity("admin", "password");
						Logger.Log(Log.LogLevel.INFO, "App.Init: Registered " + config.Security.GetType().FullName);

						// Register the cache provider
						Logger.Log(Log.LogLevel.INFO, "App.Init: Registering cache provider");
						if (config.Cache == null)
							config.Cache = new Cache.HttpCache();
						Logger.Log(Log.LogLevel.INFO, "App.Init: Registered " + config.Cache.GetType().FullName);

						// Register the media provider
						Logger.Log(Log.LogLevel.INFO, "App.Init: Registering media provider");
						if (config.Media == null)
							config.Media = new IO.FileMedia();
						Logger.Log(Log.LogLevel.INFO, "App.Init: Registered " + config.Media.GetType().FullName);

						// Register custom slug generation
						if (config.GenerateSlug != null)
							Logger.Log(Log.LogLevel.INFO, "App.Init: Registering custom slug generation");

						// Create the model cache
						Logger.Log(Log.LogLevel.INFO, "App.Init: Creating model cache");
						modelCache = new Piranha.Cache.AppCache(config.Cache);

						// Create the handler collection
						handlers = new Web.HandlerCollection();

						// Create the extension manager
						Logger.Log(Log.LogLevel.INFO, "App.Init: Creating extension manager");
						extensions = new Extend.ExtensionManager();

						// Seed default data
						Logger.Log(Log.LogLevel.INFO, "App.Init: Seeding default data");
						using (var api = new Api()) {
							Data.Seed.Params(api);
						}

						IsInitialized = true;
					}
				}
			}
		}
		#endregion
	}
}
