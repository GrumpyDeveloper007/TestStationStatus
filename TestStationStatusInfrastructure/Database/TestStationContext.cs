using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using TestStationStatusDomain.Entities;
using System.Data.Entity.Validation;
using NLog;

namespace TestStationStatusInfrastructure.Database
{
    public class TestStationContext : DbContext
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public DbSet<StatusUpdate> CurrentStationStatus { get; set; }
        public DbSet<CompletedTest> CompletedTests { get; set; }
        public DbSet<TestDuration> TestDuration { get; set; }
        public DbSet<ReverseDNSFail> ReverseDNSFails { get; set; }

        public TestStationContext(): base ("name=TestDB")
            {}
         

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                string test = "";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        test += validationError.PropertyName + "," + validationError.ErrorMessage + "\r\n";
                    }
                }
                _logger.Log(LogLevel.Error, test);
                throw;
            }
        }
    }
}
