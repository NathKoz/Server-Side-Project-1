using Frontend.Models;

namespace Frontend.Services
{
    public class APIService : IAPIService
    {
        private readonly HttpClient _httpClient;

        public Action OnUserChanged { get; set; }

        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    OnUserChanged?.Invoke();
                }
            }
        }

        public APIService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8000/");
        }

        // User Endpoints

        public async Task<string> CreateUserAsync(User newUser)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "users/",
                new
                {
                    name = newUser.Username,
                    is_admin = newUser.IsAdmin,
                    image_url = newUser.ImageUrl
                }
            );

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<User>> GetUsersAsync(string name = null)
        {
            var response = await _httpClient.GetFromJsonAsync<List<User>>($"users/?name={name}");
            return response;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            var response = await _httpClient.GetFromJsonAsync<User>($"users/{userId}");
            return response;
        }

        public async Task<string> UpdateUserAsync(int userId, User user)
        {
            var response = await _httpClient.PutAsJsonAsync($"users/{userId}", user);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PatchUserNameAsync(int userId, string name)
        {
            var response = await _httpClient.PatchAsync($"users/{userId}?name={name}", null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PatchUserIsAdminAsync(int userId, bool isAdmin)
        {
            var response = await _httpClient.PatchAsync(
                $"users/{userId}/is_admin?is_admin={isAdmin}",
                null
            );
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PatchUserImageUrlAsync(int userId, string imageUrl)
        {
            var response = await _httpClient.PatchAsync(
                $"users/{userId}/image_url?image_url={imageUrl}",
                null
            );
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteUserAsync(int userId)
        {
            var response = await _httpClient.DeleteAsync($"users/{userId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // Post Endpoints

        public async Task<string> CreatePostAsync(Post post)
        {
            var response = await _httpClient.PostAsJsonAsync("posts/", post);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<Post>> GetPostsAsync(string title = null)
        {
            if (title == null)
                return await _httpClient.GetFromJsonAsync<List<Post>>("posts/");
            else
                return await _httpClient.GetFromJsonAsync<List<Post>>($"posts/?title={title}");
        }

        public async Task<List<Post>> GetPostsByUserAsync(int userId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<Post>>($"posts/user/{userId}");
            return response;
        }

        public async Task<Post> GetPostAsync(int postId)
        {
            var response = await _httpClient.GetFromJsonAsync<Post>($"posts/{postId}");
            return response;
        }

        public async Task<string> UpdatePostAsync(int postId, Post post)
        {
            var response = await _httpClient.PutAsJsonAsync($"posts/{postId}", post);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PatchPostTitleAsync(int postId, string title)
        {
            var response = await _httpClient.PatchAsync(
                $"posts/{postId}/title?title={title}",
                null
            );
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PatchPostTextAsync(int postId, string postText)
        {
            var response = await _httpClient.PatchAsync(
                $"posts/{postId}/post_text?post_text={postText}",
                null
            );
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> IncrementPostLikesAsync(int postId)
        {
            var response = await _httpClient.PatchAsync($"posts/{postId}/increment_likes", null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DecrementPostLikesAsync(int postId)
        {
            var response = await _httpClient.PatchAsync($"posts/{postId}/decrement_likes", null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeletePostAsync(int postId)
        {
            var response = await _httpClient.DeleteAsync($"posts/{postId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
