using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStationStatusDomain.Entities;

namespace TestStationStatusInfrastructure.Service
{
    public class ServerDataService
    {
        public IEnumerable<StatusUpdate> GetCurrentStatusUpdate()
        {
            try
            {
                using (var ctx = new TestStationContext())
                {

                    return ctx.CurrentStationStatus.ToArray();
                }
            }
            catch (Exception ex)
            {
                return new StatusUpdate[] { new StatusUpdate { Status = ex.Message } };
            }
        }

        public string SaveStatusUpdate(StatusUpdate value)
        {
            try
            {
                using (var ctx = new TestStationContext())
                {

                    if (value.Status == "Completed" || value.Status == "Cancelled")
                    {
                        CompletedTest completedTest = new CompletedTest
                        { LogFileName = value.LogFileName, TestCaseName = value.TestCaseName, TestStationName = value.TestStationName, Commands = value.Last10Commands, Results = value.Last10Results, DurationSeconds = value.DurationSeconds, StartTime = value.StartTime, TestStatus = value.TestStatus };

                        ctx.CompletedTests.Add(completedTest);
                        ctx.SaveChanges();

                        List<string> items = new List<string>();
                        int startIndex = value.Last10Commands.Count - 10;
                        if (startIndex < 0) { startIndex = 0; }
                        for (int i = startIndex; i < value.Last10Commands.Count; i++)
                        {
                            items.Add(value.Last10Commands[i]);
                        }
                        value.Last10Commands = items;

                        items.Clear();
                        startIndex = value.Last10Results.Count - 10;
                        if (startIndex < 0) { startIndex = 0; }
                        for (int i = startIndex; i < value.Last10Results.Count; i++)
                        {
                            items.Add(value.Last10Results[i]);
                        }
                        value.Last10Results = items;
                    }

                    var existingRecord = ctx.CurrentStationStatus.Where(x => x.TestStationName == value.TestStationName).FirstOrDefault();

                    if (existingRecord == null)
                    {
                        ctx.CurrentStationStatus.Add(value);
                    }
                    else
                    {
                        ctx.Entry(existingRecord).CurrentValues.SetValues(value);
                    }

                    //ctx.CurrentStationStatus.AddIfNotExists(value, x=> x.TestStationName ==value.TestStationName );

                    ctx.SaveChanges();
                    return "";
                }
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
                return test;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
