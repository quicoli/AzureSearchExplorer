
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ase.Core;
using System.IO;
using System.Linq;

namespace ase.UnitTests
{
    [TestClass]
    public class SearchServiceManagerTest
    {
        private string databasepath = Path.Combine(Windows.Storage.ApplicationData.Current.TemporaryFolder.Path, "db.sqlite");

        private ISearchServiceManager InitializeDatabase()
        {
            ISearchServiceManager manager = new SearchServiceManager(databasepath, new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT());
            return manager;
        }

        private void CleanDatabase(ISearchServiceManager manager)
        {
            foreach (var item in manager.GetAllItems())
            {
                manager.DeleteValue(item);
            }
        }

        private SearchService CreateNewSearchService()
        {
            var svc = new SearchService()
            {
                Alias = "service1",
                ApiKey = "123",
                Name = "test service"
            };

            return svc;
        }


        [TestMethod]
        public void CanCreateDatabaseAndTable()
        {
            ISearchServiceManager manager = new SearchServiceManager(databasepath, new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT());
        }

        [TestMethod]
        public void CanInsertAndFindSearchServiceById()
        {
            var manager = InitializeDatabase();

            var svc = CreateNewSearchService();
            var id = svc.Id;
            manager.SaveValue(svc);

            var loadedSvc = manager.GetItem(id);

            Assert.IsNotNull(loadedSvc, "loadedSvc is null, should not");
            Assert.IsTrue(loadedSvc.Id == svc.Id, "loadedSvc differs, Id");
            Assert.IsTrue(loadedSvc.Name == svc.Name, "loadedSvc differs, Name");
            Assert.IsTrue(loadedSvc.ApiKey == svc.ApiKey, "loadedSvc differs, ApiKey");
            Assert.IsTrue(loadedSvc.Alias == svc.Alias, "loadedSvc differs, Alias");

            CleanDatabase(manager);
        }

        [TestMethod]
        public void CanUpdateService()
        {
            var manager = InitializeDatabase();

            var svc = CreateNewSearchService();
            var id = svc.Id;
            manager.SaveValue(svc);

            var loadedSvc = manager.GetItem(id);

            loadedSvc.Alias = "service updated";
            manager.SaveValue(loadedSvc);

            var updatedSvc = manager.GetItem(id);
            Assert.IsNotNull(loadedSvc, "updatedSvc is null, should not");
            Assert.IsTrue(updatedSvc.Alias == "service updated", "updatedSvc alias differs");
            Assert.IsTrue(updatedSvc.Alias != svc.Alias, "updatedSvc.Alias should differs from svc.Alias");

            CleanDatabase(manager);
        }

        [TestMethod]
        public void CanDeleteService()
        {
            var manager = InitializeDatabase();

            var svc = CreateNewSearchService();
            var id = svc.Id;
            manager.SaveValue(svc);

            manager.DeleteValue(svc);

            Assert.IsTrue(manager.GetAllItems().Count == 0,"GetAllItems.Count should return 0 after delete");

            var loadedSvc = manager.GetItem(id);
            Assert.IsNull(loadedSvc, "GetItem should return null after delete");

            CleanDatabase(manager);
        }

        [TestMethod]
        public void CandGetAllItems()
        {
            var manager = InitializeDatabase();

            var svc1 = CreateNewSearchService();
            var svc2 = CreateNewSearchService();
            var id1 = svc1.Id;
            var id2 = svc2.Id;

            manager.SaveValue(svc1);
            manager.SaveValue(svc2);

            var list = manager.GetAllItems();

            Assert.IsTrue(list.Count == 2);
            Assert.IsTrue(list.Any(x => x.Id == id1));
            Assert.IsTrue(list.Any(x => x.Id == id2));

            CleanDatabase(manager);
        }
    }
}
