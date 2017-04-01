using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ase.Core
{
    public class SearchService
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public string ApiKey { get; set; }
        public string Alias { get; set; }

        public SearchService()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
