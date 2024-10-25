using System.Text.Json.Serialization;

namespace Frontend.Models {
	
	/// <summary>
	/// Represents a social media user.
	/// </summary>
	public class User {
		/// <summary>
		/// The unique identifier for the user.
		/// </summary>
		[JsonPropertyName("id")]
		public int Id { get; set; }
		
		/// <summary>
		/// The display name defined by the user.
		/// </summary>
		[JsonPropertyName("username")]
		public string Username { get; set; }

		/// <summary>
		/// The location of the user's profile picture.
		/// </summary>
		[JsonPropertyName("image_url")]
		public string ImageUrl { get; set; }

		/// <summary>
		/// Indicates whether the user has administrative privileges.
		/// </summary>
		[JsonPropertyName("is_admin")]
		public bool IsAdmin { get; set; }
	}
}
