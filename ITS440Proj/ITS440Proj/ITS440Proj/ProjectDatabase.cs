using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SQLite;
using System.Threading.Tasks;

namespace ITS440Proj
{
    public class ProjectDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public ProjectDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(MVVM.ObservableItem).Name))
                {
                    await Database.CreateTableAsync<MVVM.ObservableItem>(CreateFlags.None).ConfigureAwait(false); // creates database
                }
                initialized = true;
            }
        }

        public Task<List<MVVM.ObservableItem>> GetItemsAsync()
        {
            return Database.Table<MVVM.ObservableItem>().ToListAsync();
        }

        public Task<int> UpdateItemAsync(MVVM.ObservableItem item)
        {
            return Database.UpdateAsync(item);
        }

        public Task<int> InsertItemAsync(MVVM.ObservableItem item)
        {
            return Database.InsertAsync(item);
        }

        public Task<int> DeleteItemAsync(object PrimaryKey)
        {
            return Database.DeleteAsync<MVVM.ObservableItem>(PrimaryKey);
        }
    }
}
