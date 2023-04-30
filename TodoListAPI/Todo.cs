namespace TodoListAPI
{
	public class Todo
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string CreatedAt { get; set; } = DateTime.Now.ToString();
		public string UpdatedAt { get; set; } = string.Empty;
	}
}
