using FullStackWebAPI.Controllers.Tests;
using FullStackWebAPI.Tests.Controllers;
using NBench;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullStackWebAPI.Tests.NBench
{
    public class PerformanceTest
    {
        [PerfBenchmark(Description = "You can write your description here.",
NumberOfIterations = 500, RunMode = RunMode.Throughput, RunTimeMilliseconds = 60000, TestMode = TestMode.Measurement)]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        [GcMeasurement(GcMetric.TotalCollections,GcGeneration.AllGc)]
        //[MemoryAssertion(MemoryMetric.TotalBytesAllocated, MustBe.LessThanOrEqualTo, ByteConstants.SixtyFourKb)]
        public void Benchmark_Performance_Memory()
        {
            UserControllerTests userController = new UserControllerTests();
            userController.GetAndPostTest();

            //int i = 0;
            //i++;
        }
    }
}
