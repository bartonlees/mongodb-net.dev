﻿using MongoDB.Driver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MongoDB.MSTest
{
    
    
    /// <summary>
    ///This is a test class for IServerExtensionsTest and is intended
    ///to contain all IServerExtensionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IServerExtensionsTest
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


        /// <summary>
        ///A test for GetDatabase
        ///</summary>
        [TestMethod()]
        public void GetDatabaseTest()
        {
            IServer server = null; // TODO: Initialize to an appropriate value
            string databaseUri = string.Empty; // TODO: Initialize to an appropriate value
            IDatabase expected = null; // TODO: Initialize to an appropriate value
            IDatabase actual;
            actual = IServerExtensions.GetDatabase(server, databaseUri);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}