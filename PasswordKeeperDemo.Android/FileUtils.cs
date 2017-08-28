using System;
using System.IO;
using PasswordKeeperDemo.Droid;
using PasswordKeeperDemo.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileUtils))]
namespace PasswordKeeperDemo.Droid
{
    public class FileUtils : IFileUtils
    {
        public byte[] GetFileContent(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    return System.IO.File.ReadAllBytes(fileName);
                }

                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveFileContent(string fileName, byte[] fileContent)
        {
            try
            {
                System.IO.File.WriteAllBytes(fileName, fileContent);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}