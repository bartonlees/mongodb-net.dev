﻿using MongoDB.Driver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FluentAssertions;

namespace MongoDB.MSTest
{


    /// <summary>
    ///This is a test class for IDBRequestTest and is intended
    ///to contain all IDBRequestTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IDBRequestTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        internal virtual IDBRequest CreateIDBRequest()
        {
            // TODO: Instantiate an appropriate concrete class.
            IDBRequest target = null;
            return target;
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [TestMethod()]
        public void WriteTest()
        {
            IDBRequest target = CreateIDBRequest(); // TODO: Initialize to an appropriate value
            WireProtocolWriter writer = null; // TODO: Initialize to an appropriate value
            target.Write(writer);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Partial
        ///</summary>
        [TestMethod()]
        public void PartialTest()
        {
            IDBRequest target = CreateIDBRequest(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Partial;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
