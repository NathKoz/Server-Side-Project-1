using System.Text.Json.Serialization;

namespace Frontend.Models {
	/// <summary>
	/// Represents a social media post.
	/// </summary>
	public class Post {
		/// <summary>
		/// The unique identifier for the post.
		/// </summary>
		[JsonPropertyName("id")]
		public int Id { get; set; }
		
		/// <summary>
		/// The ID of the user who created the post.
		/// </summary>
		[JsonPropertyName("user_id")]
		public int UserId { get; set; }

		/// <summary>
		/// Appears at the top of the post as a heading.
		/// </summary>
		[JsonPropertyName("title")]
		public string Title { get; set; }

		/// <summary>
		/// The main content of the post.
		/// </summary>
		[JsonPropertyName("post_text")]
		public string PostText { get; set; }

		/// <summary>
		/// Counter variable for the interactive like button.
		/// </summary>
		[JsonPropertyName("likes")]
		public int Likes { get; set; }
	}
}
