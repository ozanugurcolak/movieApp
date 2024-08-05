using System.Collections.Generic;

namespace movieApp.web.Entity
{
	public class User
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string ImageUrl { get; set; }
		public Person Person { get; set; }
	}
}
		

	
