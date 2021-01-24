namespace BeautyAtHome.ViewModels
{
    public class AuthCM
    {
        public string AccessToken { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
    }

    public class AuthVM
    {
        public int Uid { get; set; }
        public string DisplayName { get; set; }
        public string Role { get; set; }
        public string AccessToken { get; set; }
        public string ExpiresIn { get; set; }
    }


}
