
using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using Xunit.Runners;

namespace Vlerx.SampleService.Tests
{
    public class TestExecutionStatus
    {
        public string Message { get; private set; }
        public string Status { get; private set; }
        public static TestExecutionStatus Skipped(string message)
        => new TestExecutionStatus
        {
            Status = nameof(Skipped),
            Message = message
        };
        static public TestExecutionStatus Passed => new TestExecutionStatus
        {
            Status = nameof(Passed),
        };
        static public TestExecutionStatus Failed(string message)
             => new TestExecutionStatus
             {
                 Status = nameof(Failed),
                 Message = message
             };
    }
    public class TestExecutionViewModel
    {
        public TestExecutionViewModel(string testName, TestExecutionStatus result)
        {
            TestName = testName;
            Result = result;

        }
        public string TestName { get; }
        public TestExecutionStatus Result { get; }

    }
    public class TestRunner
    {
        readonly ManualResetEvent Finished = new ManualResetEvent(false);
        public List<TestExecutionViewModel> Run(string context)
        {
            using (var runner = AssemblyRunner.WithoutAppDomain(context))
            // using (var runner = AssemblyRunner.WithoutAppDomain($"../Vlerx.SampleService.TestServer/bin/Debug/netcoreapp3.1/Vlerx.SampleService.Tests.dll"))
            {
                runner.OnDiscoveryComplete = OnDiscoveryComplete;
                runner.OnExecutionComplete = OnExecutionComplete;
                runner.OnTestPassed = OnTestPassed;
                runner.OnTestFailed = OnTestFailed;
                runner.OnTestSkipped = OnTestSkipped;

                Console.WriteLine("Discovering...");
                runner.Start();

                Finished.WaitOne();  // A ManualResetEvent
                Finished.Dispose();
                Console.WriteLine("Done...");
                Console.WriteLine(JsonConvert.SerializeObject(Result));

                return Result;
            }
        }
        public List<TestExecutionViewModel> Run(string context, string testName)
        {
            using (var runner = AssemblyRunner.WithoutAppDomain(context))
            // using (var runner = AssemblyRunner.WithoutAppDomain($"../Vlerx.SampleService.TestServer/bin/Debug/netcoreapp3.1/Vlerx.SampleService.Tests.dll"))
            {
                runner.TestCaseFilter = test => test.TestMethod.TestClass.Class.Name.Contains(testName);
                runner.OnDiscoveryComplete = OnDiscoveryComplete;
                runner.OnExecutionComplete = OnExecutionComplete;
                runner.OnTestPassed = OnTestPassed;
                runner.OnTestFailed = OnTestFailed;
                runner.OnTestSkipped = OnTestSkipped;

                Console.WriteLine("Discovering...");
                runner.Start();

                Finished.WaitOne();  // A ManualResetEvent
                Finished.Dispose();
                Console.WriteLine("Done...");
                Console.WriteLine(JsonConvert.SerializeObject(Result));

                return Result;
            }
        }

        void OnTestPassed(TestPassedInfo obj)
        => Result.Add(new TestExecutionViewModel(obj.TypeName, TestExecutionStatus.Passed));

        public List<TestExecutionViewModel> Result { get; } = new List<TestExecutionViewModel>();
        void OnTestSkipped(TestSkippedInfo obj)
        => Result.Add(new TestExecutionViewModel(obj.TypeName, TestExecutionStatus.Skipped(obj.SkipReason)));


        void OnTestFailed(TestFailedInfo obj)
        => Result.Add(new TestExecutionViewModel(obj.TypeName, TestExecutionStatus.Failed(obj.Output)));


        void OnExecutionComplete(ExecutionCompleteInfo obj)
        => Finished.Set();
        private static void OnDiscoveryComplete(DiscoveryCompleteInfo obj)
        {
        }
    }
}