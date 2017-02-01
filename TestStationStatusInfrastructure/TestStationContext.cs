using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using TestStationStatusDomain.Entities;

namespace TestStationStatusInfrastructure
{
    public class TestStationContext : DbContext
    {
        public DbSet<StatusUpdate> CurrentStationStatus { get; set; }
        public DbSet<CompletedTest> CompletedTests { get; set; }

    }
}
