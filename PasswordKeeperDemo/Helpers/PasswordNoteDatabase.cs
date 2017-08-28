using System.Collections.Generic;
using System.Threading.Tasks;
using PasswordKeeperDemo.Models;

namespace PasswordKeeperDemo.Helpers
{
    public class PasswordNoteDatabase : AbstractDao<PasswordNote, int>
    {
        public PasswordNoteDatabase()
        {
            Database.CreateTableAsync<PasswordNote>().Wait();
        }

        public Task<List<PasswordNote>> GetFavouriteItemsAsync()
        {
            return Database.QueryAsync<PasswordNote>("SELECT * FROM [PasswordNote] WHERE [IsFavourite] = 1");
        }
    }
}
