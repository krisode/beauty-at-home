namespace BeautyAtHome.ViewModels
{
    public class AuthCM
    {
        public string IdToken { get; set; }
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
