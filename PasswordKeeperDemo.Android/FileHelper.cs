using System;
using System.IO;
using PasswordKeeperDemo.Droid;
using PasswordKeeperDemo.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace PasswordKeeperDemo.Droid
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}