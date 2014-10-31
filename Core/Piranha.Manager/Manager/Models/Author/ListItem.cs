using System;

namespace Piranha.Manager.Models.Author
{
	public class ListItem
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Created { get; set; }
		public string Updated { get; set; }
	}
}