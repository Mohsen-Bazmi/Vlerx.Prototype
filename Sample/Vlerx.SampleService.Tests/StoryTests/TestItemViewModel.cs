using System.Collections.Generic;
using System.Linq;

namespace Vlerx.SampleService.Tests.StoryTests
{
    public class TestItemViewModel
    {
        public TestItemViewModel(string testName, string sutTestName, IStorySpecification testClass)
        {
            TestName = testName;
            SutName = sutTestName;
            TestClass = testClass;
        }
        public string TestName { get; }
        public string SutName { get; }
        public IStorySpecification TestClass { get; }
    }
}