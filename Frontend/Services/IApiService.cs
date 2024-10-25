using Frontend.Models;

namespace Frontend.Services {
	public interface IAPIService {
		public User CurrentUser { get; set; }
		public Action OnUserChanged { get; set; }

        // User Endpoints
        Task<string> CreateUserAsync(User user);
		Task<List<User>> GetUsersAsync(string name = null);
		Task<User> GetUserAsync(int userId);
		Task<string> UpdateUserAsync(int userId, User user);
		Task<string> PatchUserNameAsync(int userId, string name);
		Task<string> PatchUserIsAdminAsync(int userId, bool isAdmin);
		Task<string> PatchUserImageUrlAsync(int userId, string imageUrl);
		Task<string> DeleteUserAsync(int userId);

		// Post Endpoints
		Task<string> CreatePostAsync(Post post);
		Task<List<Post>> GetPostsAsync(string title = null);
		Task<List<Post>> GetPostsByUserAsync(int userId);
		Task<Post> GetPostAsync(int postId);
		Task<string> UpdatePostAsync(int postId, Post post);
		Task<string> PatchPostTitleAsync(int postId, string title);
		Task<string> PatchPostTextAsync(int postId, string postText);
		Task<string> IncrementPostLikesAsync(int postId);
		Task<string> DecrementPostLikesAsync(int postId);
		Task<string> DeletePostAsync(int postId);
	}
}
