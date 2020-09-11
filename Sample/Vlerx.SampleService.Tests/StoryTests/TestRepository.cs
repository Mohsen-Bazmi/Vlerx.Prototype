using System.Collections.Generic;
using System.Linq;

namespace Vlerx.SampleService.Tests.StoryTests
{
    public static class TestRepository
    {
        const int MAX_PAGE_SIZE = 500;
        static readonly IDictionary<string, (string systemUnderTestName, IStorySpecification testClass)> all
        = new Dictionary<string, (string systemUnderTestName, IStorySpecification testClass)>();
        // public static IEnumerable<TestItemViewModel> GetAll(int pageNumber, int pageSize)
        // public static IEnumerable<TestItemViewModel> GetAll(int pageNumber, int pageSize)
        // {
        //     if (MAX_PAGE_SIZE < pageSize)
        //     {
        //         pageSize = MAX_PAGE_SIZE;
        //     }
        //     if (all.Count < pageNumber * pageSize)
        //     {
        //         return new TestItemViewModel[0];
        //     }
        //     var result = new KeyValuePair<string, (string systemUnderTestName, IStorySpecification testClass)>[all.Count];
        //     all.CopyTo(result, pageNumber * pageSize);
        //     return result.Select();
        // }
        public static void Register(string testName, string systemUnderTestName, IStorySpecification testClass)
        {
        }
    }
}