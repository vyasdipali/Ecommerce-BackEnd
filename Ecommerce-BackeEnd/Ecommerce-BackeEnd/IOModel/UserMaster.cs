using DataBase;

namespace Ecommerce_BackeEnd.IOModel
{
    public class SignUpInputModel
    {

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class LoginInputModelDemo
    {
        public string PhoneNo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class LoginResponseModelDemo : UserMaster
    {
        public string? Token { get; set; }
    }
}
