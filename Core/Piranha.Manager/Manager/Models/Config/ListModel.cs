using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Piranha.Manager.Models.Config
{
	public class ListModel
	{
		public class ConfigItem
		{
			public string SubGroup { get; set; }
			public string Name { get; set; }
			public string Description { get; set; }
			public object Value { get; set; }
		}

		public class ConfigGroup
		{
			public string Name { get; set; }
			public IList<ConfigItem> Items { get; set; }

			public ConfigGroup() {
				Items = new List<ConfigItem>();
			}
		}

		public IList<ConfigGroup> Groups { get; set; }

		public ListModel() {
			Groups = new List<ConfigGroup>();
		}

		public static ListModel Get() {
			var m = new ListModel();

			m.Groups.Add(new ConfigGroup() {
				Name = "General",
				Items = new List<ConfigItem>() {
					new ConfigItem() {
						SubGroup = "Site",
						Name = "Title",
						Value = Piranha.Config.Site.Title
					},
					new ConfigItem() {
						SubGroup = "Site",
						Name = "Desciption",
						Value = Piranha.Config.Site.Description
					},
					new ConfigItem() {
						SubGroup = "Site",
						Name = "Archive page size",
						Value = Piranha.Config.Site.ArchivePageSize
					},
					new ConfigItem() {
						SubGroup = "Cache",
						Name = "Expiration (minutes)",
						Value = Piranha.Config.Cache.Expires
					},
					new ConfigItem() {
						SubGroup = "Cache",
						Name = "Max age (minutes)",
						Value = Piranha.Config.Cache.MaxAge
					}
				}
			});
			m.Groups.Add(new ConfigGroup() { 
				Name = "Blogging",
				Items = new List<ConfigItem>() { 
					new ConfigItem() {
						SubGroup = "Comments",
						Name = "Moderate anonymous",
						Value = Piranha.Config.Comments.ModerateAnonymous
					},
					new ConfigItem() {
						SubGroup = "Comments",
						Name = "Moderate authorized",
						Value = Piranha.Config.Comments.ModerateAuthorized
					}
				}
			});
			return m;
		}
	}
}