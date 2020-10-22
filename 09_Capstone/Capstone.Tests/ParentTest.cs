using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class ParentTest
    {
        private TransactionScope trans;

        protected string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=excelsior_venues;Integrated Security=True";

        [TestInitialize]
        public void Setup()
        {
            trans = new TransactionScope();
        }

        [TestCleanup]
        public void Reset()
        {
            trans.Dispose();
        }

    }
}
