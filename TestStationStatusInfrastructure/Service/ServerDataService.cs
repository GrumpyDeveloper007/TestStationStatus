using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStationStatusDomain.Entities;
using TestStationStatusInfrastructure.Database;

namespace TestStationStatusInfrastructure.Service
{
    /// <summary>
    /// A database layer that uses entity framework
    /// </summary>
    public class ServerDataService
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private TestStationContextFactory _testStationContextFactory;

        public ServerDataService(TestStationContextFactory testStationContextFactory)
        {
            _testStationContextFactory = testStationContextFactory;
        }

        public IEnumerable<StatusUpdate> GetCurrentStatusUpdate()
        {
            try
            {
                using (var ctx = _testStationContextFactory.OpenSession())
                {

                    return ctx.CurrentStationStatus.ToArray();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                return new StatusUpdate[] { new StatusUpdate { Status = ex.Message } };
            }
        }

        public IEnumerable<ReverseDNSFail> GetReverseDNSFails()
        {
            try
            {
                using (var ctx = _testStationContextFactory.OpenSession())
                {
                    return ctx.ReverseDNSFails.ToArray();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                return new ReverseDNSFail[0];
            }
        }

        public IEnumerable<string> GetReverseDNSFailsAsList()
        {
            try
            {
                using (var ctx = _testStationContextFactory.OpenSession())
                {
                    return ctx.ReverseDNSFails.Select((x) => (x.PCName)).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                return new string[0];
            }
        }


        public double GetDurationOfTestCase(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            { return 0; }

            try
            {
                using (var ctx = _testStationContextFactory.OpenSession())
                {
                    // try to match PC name first
                    var existingRecord = ctx.TestDuration.Where(x => x.TestCaseName == fileName && x.TestStationName == Environment.MachineName).FirstOrDefault();

                    // if no record exists for the test station, try other test stations
                    if (existingRecord == null)
                        existingRecord = ctx.TestDuration.Where(x => x.TestCaseName == fileName).FirstOrDefault();

                    if (existingRecord == null)
                    {
                        _logger.Log(LogLevel.Debug, "Duration not found for test case : " + fileName);
                        return 0;
                    }
                    else
                    {
                        //_logger.Log(LogLevel.Debug, "Duration found for test case : " + fileName + " duration : " + existingRecord.DurationSeconds);
                        return existingRecord.DurationSeconds;
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                return 0;
            }
        }


        public string SaveDuration(TestDuration value)
        {
            try
            {
                using (var ctx = _testStationContextFactory.OpenSession())
                {
                    var existingRecord = ctx.TestDuration.Where(x => x.TestStationName == value.TestStationName && x.TestCaseName == value.TestCaseName).FirstOrDefault();

                    if (existingRecord == null)
                    {
                        ctx.TestDuration.Add(value);
                    }
                    else
                    {
                        existingRecord.DurationSeconds = value.DurationSeconds;
                        //ctx.Entry(existingRecord).CurrentValues.SetValues(value);
                    }

                    ctx.SaveChanges();
                    return "";
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                return ex.Message;
            }
        }

        public string SaveCompletedTest(CompletedTest completedTest)
        {
            try
            {
                using (var ctx = _testStationContextFactory.OpenSession())
                {
                    ctx.CompletedTests.Add(completedTest);
                    ctx.SaveChanges();
                    return "";
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                return ex.Message;
            }
        }

        public string SaveStatusUpdate(StatusUpdate value)
        {
            try
            {
                using (var ctx = _testStationContextFactory.OpenSession())
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
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                return ex.Message;
            }
        }
    }
}