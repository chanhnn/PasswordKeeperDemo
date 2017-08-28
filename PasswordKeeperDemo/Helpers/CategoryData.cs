using System.Collections.Generic;

namespace PasswordKeeperDemo.Helpers
{
    public static class CategoryData
    {
        public static IEnumerable<Cat> CreateData()
        {
            return new List<Cat>()
            {
                new Cat() { CategoryId = 1, CategoryCode = "Login", CategoryName = "Login", Icon = "catLogin.png", Page = "LoginTemplate" },
                new Cat() { CategoryId = 2, CategoryCode = "Password", CategoryName = "Password", Icon = "catPassword.png", Page = "PasswordTemplate" },
                new Cat() { CategoryId = 3, CategoryCode = "SecureNote", CategoryName = "Secure Note", Icon = "catSecure.png", Page = "SecureTemplate" },
                //new Cat() { CategoryId = 4, CategoryCode = "SoftwareLicense", CategoryName = "Software License", Icon = "catLicense.png", Page = "LicenseTemplate" },
                //new Cat() { CategoryId = 5, CategoryCode = "SocialSecurityNumber", CategoryName = "Social Security Number", Icon = "catSocial.png", Page = "SocialTemplate" }
            };
        }
    }

    public class Cat
    {
        public int CategoryId { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string Icon { get; set; }
        public string Page { get; set; }
    }
}
