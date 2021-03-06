// <copyright file="MyTestTest.cs" company="Microsoft">Copyright © Microsoft 2018</copyright>
using System;
using Games;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Games.Tests
{
    /// <summary>This class contains parameterized unit tests for MyTest</summary>
    [PexClass(typeof(MyTest))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class MyTestTest
    {
        /// <summary>Test stub for Test()</summary>
        [PexMethod]
        public void TestTest()
        {
            MyTest.Test();
            // TODO: add assertions to method MyTestTest.TestTest()
        }
    }
}
