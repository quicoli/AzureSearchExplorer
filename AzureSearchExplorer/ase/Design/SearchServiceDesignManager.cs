using ase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ase.Design
{
    public class SearchServiceDesignManager : ISearchServiceManager
    {
        public BoolResponse DeleteValue(SearchService value)
        {
            throw new NotImplementedException();
        }

        public List<SearchService> GetAllItems()
        {
            var list = new List<SearchService>()
            {
                new SearchService(){Id="1",ApiKey="123",Name="service1.azuresearch.net",Alias="Service 1"},
                new SearchService(){Id="2",ApiKey="123",Name="service2.azuresearch.net",Alias="Service 2"},
                new SearchService(){Id="3",ApiKey="123",Name="service3.azuresearch.net",Alias="Service 3"},
                new SearchService(){Id="4",ApiKey="123",Name="service4.azuresearch.net",Alias="Service 4"}
            };

            return list;
        }

        public SearchService GetItem(string id)
        {
            throw new NotImplementedException();
        }

        public BoolResponse SaveValue(SearchService value)
        {
            throw new NotImplementedException();
        }
    }
}
