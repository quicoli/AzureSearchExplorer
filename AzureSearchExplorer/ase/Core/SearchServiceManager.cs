using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ase.Core
{
    public class SearchServiceManager : ISearchServiceManager
    {
        private SQLite.Net.SQLiteConnection _database;
        public BoolResponse DeleteValue(SearchService value)
        {
            var result = new BoolResponse();

            var serviceExists = _database.Table<SearchService>().Any(s => s.Id == value.Id);

            //var all = (from entry in _database.Table<SearchService>().AsEnumerable<SearchService>()
            //           where entry.Id == value.Id
            //           select entry).ToList();

            if (serviceExists)
            {
                try
                {
                    _database.Delete(value);
                    result.Value = true;
                    result.Response = "Search service deleted";
                }
                catch (Exception e)
                {
                    result.Value = false;
                    result.Response = e.Message;
                }
                
            }
            else
            {
                result.Value = false;
                result.Response = "Search service not found";
            }
                

            return result;
        }

        public List<SearchService> GetAllItems()
        {
            return _database.Table<SearchService>().AsEnumerable<SearchService>().ToList();
        }

        public SearchService GetItem(string id)
        {
            var result = (from entry in _database.Table<SearchService>().AsEnumerable<SearchService>()
                          where entry.Id == id
                          select entry).FirstOrDefault();
            return result;
        }

        public BoolResponse SaveValue(SearchService value)
        {
            var result = new BoolResponse();

            var aliasDuplicated = _database.Table<SearchService>().Any(s => s.Alias == value.Alias && s.Id != value.Id);

            if (aliasDuplicated)
            {
                result.Value = false;
                result.Response = "The informed Alias already exists";
            }
            else
            {
                var isUpdating = _database.Table<SearchService>().Any(s => s.Id == value.Id);

                try
                {
                    if (isUpdating)
                        _database.Update(value);
                    else
                        _database.Insert(value);
                    result.Value = true;
                    result.Response = "Search service saved";
                }
                catch (Exception e)
                {
                    result.Value = false;
                    result.Response = e.Message;
                }
            }
            return result;
        }

        /// <summary>
        /// Instantiate a new SearchServiceManager using SQLite
        /// </summary>
        /// <param name="databasePath">full path for the database. Example: <code>Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");</code></param>
        /// <param name="platform">an instance of <see cref="ISQLitePlatform"/>, the platform where database runs. Example: <code>new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT()</code> </param>
        public SearchServiceManager(string databasePath, ISQLitePlatform platform)
        {
            var path = databasePath;
            _database = new SQLite.Net.SQLiteConnection(platform, path);
            _database.CreateTable<SearchService>();
        }
    }
}
