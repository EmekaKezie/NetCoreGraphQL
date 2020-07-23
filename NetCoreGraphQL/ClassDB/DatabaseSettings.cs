﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreGraphQL.ClassDB
{
    public class DatabaseSettings
    {
        public String Server { get; set; }
        public String ProviderName { get; set; }
        public String DatabaseType { get; set; }
        public String DatabaseName { get; set; }
        public String Password { get; set; }
        public String UserId { get; set; }
        public Int32 Port { get; set; }
        public Int32 MaxPoolSize { get; set; }
        public Int32 ConnectionTimeout { get; set; }
        public Int32 CommandTimeout { get; set; }
    }
}
