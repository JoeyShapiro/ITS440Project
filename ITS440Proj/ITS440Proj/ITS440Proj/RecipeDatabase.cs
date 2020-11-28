using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS440Proj
{
    public class RecipeDatabase // database for recipes cause best way to add 2 tables
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public RecipeDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Recipe).Name))
                {
                    await Database.CreateTableAsync<Recipe>(CreateFlags.None).ConfigureAwait(false); // creates database
                }
                initialized = true;
            }
        }

        public Task<List<Recipe>> GetItemsAsync()
        {
            return Database.Table<Recipe>().ToListAsync();
        }

        public Task<int> UpdateItemAsync(Recipe item)
        {
            return Database.UpdateAsync(item);
        }

        public Task<int> InsertItemAsync(Recipe item)
        {
            return Database.InsertAsync(item);
        }

        public Task<int> DeleteItemAsync(object PrimaryKey)
        {
            return Database.DeleteAsync<Recipe>(PrimaryKey);
        }
    }
}
