using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Filter2RLC.Models
{
    public class FilterDBContext: DbContext
    {
        public DbSet<FilterParams> SetOfFilters { get; set; }
    }
}