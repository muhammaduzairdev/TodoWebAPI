namespace Todoweb.Models
{
	public class Todo
	{
		public string Id { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
        public string UpdatedAt { get; set; } = string.Empty;
	}

}
