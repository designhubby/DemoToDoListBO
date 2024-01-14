using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoToListBE.Configuration
{
    public class ConnectionStrings
    {
        public const string ConnectionStringName = "ConnectionStrings";
        public string SQLLocalDb { get; set; } = String.Empty;
        public string SQLServerDb { get; set; } = String.Empty;


    }
}
