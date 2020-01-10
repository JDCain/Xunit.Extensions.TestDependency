using System;
using System.Collections.Generic;
using System.Linq;

namespace Xunit.Extensions.TestDependency
{
    /// <summary>
    /// Attribute used to indicate the name(s) of proceeding tests which are depended on. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestDependencyAttribute : Attribute
    {
        /// <summary>
        /// </summary>
        /// <param name="tests">Test Method names this method depend on having run first</param>
        public TestDependencyAttribute(params string[] tests) => Tests = tests.ToList();

        /// <summary>
        /// Test Method names this method depend on having run first
        /// </summary>
        public IReadOnlyList<string> Tests { get; }
    }
}