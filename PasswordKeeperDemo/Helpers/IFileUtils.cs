namespace PasswordKeeperDemo.Helpers
{
    public interface IFileUtils
    {
        byte[] GetFileContent(string filename);

        void SaveFileContent(string fileName, byte[] fileContent);
    }
}
