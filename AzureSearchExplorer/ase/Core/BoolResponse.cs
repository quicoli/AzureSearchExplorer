using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ase.Core
{
    /// <summary>
    /// A bool response with a complementary text message
    /// </summary>
    public class BoolResponse
    {
        /// <summary>
        /// A bool value
        /// </summary>
        public bool Value { get; set; }
        /// <summary>
        /// A complementary text message
        /// </summary>
        public string Response { get; set; }
    }
}
