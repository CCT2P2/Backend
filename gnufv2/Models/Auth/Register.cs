namespace Gnuf.Models.Auth {
    public class RegisterRequest {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterResponse {
        public string UserId { get; set; }
    }
}
