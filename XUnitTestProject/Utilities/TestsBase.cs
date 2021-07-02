using System;

namespace Tests.Utilities
{
    public abstract class TestsBase : IDisposable
    {
        // Will be called before every test method.
        protected TestsBase()
        {
            TestsHelper.FillInInMemoryNorthwindContextWithTestData();
        }

        // Will be called after every test method.
        public void Dispose()
        {
            TestsHelper.DropTestDatabase();
        }
    }
}