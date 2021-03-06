﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoDb.Attributes;
using RepoDb.Enumerations;
using RepoDb.Extensions;
using System.Linq;

namespace RepoDb.UnitTests.Enumerations
{
    [TestClass]
    public class OrderTextTest
    {
        private TextAttribute GetOrderTextAttribute(Order order)
        {
            return typeof(Order)
                .GetMembers()
                .First(member => member.Name.ToLower() == order.ToString().ToLower())
                .GetCustomAttribute<TextAttribute>();
        }

        [TestMethod]
        public void TestOrderAscendingText()
        {
            // Prepare
            var operation = Order.Ascending;

            // Act
            var parsed = GetOrderTextAttribute(operation);

            // Assert
            Assert.AreEqual("ASC", parsed.Text);
        }

        [TestMethod]
        public void TestOrderDescendingText()
        {
            // Prepare
            var operation = Order.Descending;

            // Act
            var parsed = GetOrderTextAttribute(operation);

            // Assert
            Assert.AreEqual("DESC", parsed.Text);
        }
    }
}
