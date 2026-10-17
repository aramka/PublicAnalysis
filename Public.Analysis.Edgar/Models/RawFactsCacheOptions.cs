using Public.Frameworks.Cache;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Edgar.Models
{
    public class RawFactsCacheOptions : CacheOptions
    {
        public int CacheMissWaitToUpdateWithFactoryMilli { get; set; }

    }
}
