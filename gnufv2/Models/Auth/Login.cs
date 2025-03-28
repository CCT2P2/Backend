namespace Gnuf.Models.Auth {
    public class LoginRequest {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse {
        public string UserId { get; set; }
    }
}
