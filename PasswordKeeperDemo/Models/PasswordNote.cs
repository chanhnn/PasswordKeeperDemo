using System;
using SQLite;

namespace PasswordKeeperDemo.Models
{
    public class PasswordNote
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime ModifyDateTime { get; set; }

        public string Title { get; set; }

        public string Password { get; set; }

        public string IvKey { get; set; }

        public string Note { get; set; }

        public bool IsFavourite { get; set; }

        /**/
        public int CategoryId { get; set; }

        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }

        public string Icon { get; set; }

        public string Page { get; set; }

        /* Login */
        public string UserName { get; set; }

        public string Website { get; set; }

        /* Password */

        /* Secure Note */

        /* Software license */
        public string LicenseKey { get; set; }

        public string LicenseTo { get; set; }

        /* Social Security Number */
        public string Owner { get; set; }

        public string SocialSecurityNumber { get; set; }
    }
}
