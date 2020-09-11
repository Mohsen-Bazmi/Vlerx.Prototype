using System.Collections.Generic;
using System.Linq;

namespace Vlerx.SampleService.Tests.StoryTests
{
    public class TestMetadataItemViewModel
    {
        public TestMetadataItemViewModel(string testName,string context, string metadata)
        {

            TestName = testName;
            Metadata = metadata;
            Context = context;
        }
        public string Context { get; }
        public string TestName { get; }
        public string Metadata { get; }
    }
}