using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ase.Core
{

    public interface ISearchServiceManager
    {
        BoolResponse SaveValue(SearchService value);
        BoolResponse DeleteValue(SearchService value);
        List<SearchService> GetAllItems();
        SearchService GetItem(string id);
    }
}
