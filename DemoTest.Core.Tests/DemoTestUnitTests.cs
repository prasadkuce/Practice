using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DemoTest.Core.Tests
{
    [TestClass]
    public class DemoTestUnitTests
    {
        public List<Item> testItems = new List<Item>() {
                new Item(){ItemId = 1, ItemType = ItemTypeEnum.Operational, Name = "Question1"},
                new Item(){ItemId = 2, ItemType = ItemTypeEnum.Operational, Name = "Question2"},
                new Item(){ItemId = 3, ItemType = ItemTypeEnum.Operational, Name = "Question3"},
                new Item(){ItemId = 4, ItemType = ItemTypeEnum.PreTest, Name = "Question4"},
                new Item(){ItemId = 5, ItemType = ItemTypeEnum.Operational, Name = "Question5"},
                new Item(){ItemId = 6, ItemType = ItemTypeEnum.PreTest, Name = "Question6"},
                new Item(){ItemId = 7, ItemType = ItemTypeEnum.PreTest, Name = "Question7"},
                new Item(){ItemId = 8, ItemType = ItemTypeEnum.Operational, Name = "Question8"},
                new Item(){ItemId = 9, ItemType = ItemTypeEnum.Operational, Name = "Question9"},
                new Item(){ItemId = 10, ItemType = ItemTypeEnum.PreTest, Name = "Question10"},
            };
        [TestMethod]
        public void FailWithNullTestItemsException()
        {
            //Arrange
            var testlet = new Testlet(1, "test1", null);
            //Act
            var ex = Assert.ThrowsException<System.Exception>(() => testlet.Randomize());
            //Assert
            if(ex != null && ex.Message.Equals("Testlet Items are not initialized."))
                Assert.Fail();
        }
        [TestMethod]
        public void TestEmptyTestItemsException()
        {
            //Arrange
            var emptyItems = new List<Item>();
            var testlet = new Testlet(1, "test1", emptyItems);
            //Act
            var ex = Assert.ThrowsException<System.Exception>(() => testlet.Randomize());
            //Assert
            Assert.AreEqual(ex.Message, "Testlet Items are empty. Add some testlet items");
        }
        [TestMethod]
        public void TestRandomizeWithValidTestItems()
        {
            //Arrange
            var testlet = new Testlet(1, "test1", testItems);
            //Act
            var randTestItems = testlet.Randomize();
            var randTestItems2 = testlet.Randomize();
            //Assert
            Assert.IsFalse(System.Linq.Enumerable.SequenceEqual(testItems, randTestItems));
            Assert.IsFalse(System.Linq.Enumerable.SequenceEqual(testItems, randTestItems2));
            Assert.IsFalse(System.Linq.Enumerable.SequenceEqual(randTestItems, randTestItems2));
        }
        [TestMethod]
        public void TestFirstAndSecondTestItemsToBePreTestItems()
        {
            //Arrange
            var testlet = new Testlet(1, "test1", testItems);
            //Act
            var randTestItems = testlet.Randomize();
            //Assert
            Assert.IsTrue(randTestItems.Take(2).Where(t => t.ItemType == ItemTypeEnum.PreTest).Count() == 2);
            Assert.IsTrue(randTestItems.Take(2).Where(t => t.ItemType == ItemTypeEnum.Operational).Count() == 0);
        }
        [TestMethod]
        public void TestLastTestItemsToBeOperationalItems()
        {
            //Arrange
            var testlet = new Testlet(1, "test1", testItems);
            //Act
            var randTestItems = testlet.Randomize();
            //Assert
            Assert.IsTrue(randTestItems.Skip(2).Where(t => t.ItemType == ItemTypeEnum.PreTest).Count() == 2);
            Assert.IsTrue(randTestItems.Skip(2).Where(t => t.ItemType == ItemTypeEnum.Operational).Count() == 6);
        }
        [TestMethod]
        public void TestNotEnoughPreTestItemsException()
        {
            //Arrange
            var testlet = new Testlet(1, "test1", testItems);
            testItems.RemoveAll(t => t.ItemType == ItemTypeEnum.PreTest);
            //Act
            var ex = Assert.ThrowsException<System.Exception>(() => testlet.Randomize());
            //Assert
            Assert.AreEqual(ex.Message, "There are not enough pretest items");
        }
        [TestMethod]
        public void TestNotEnoughOprationalItemsException()
        {
            //Arrange
            var testlet = new Testlet(1, "test1", testItems);
            testItems.RemoveAll(t => t.ItemType == ItemTypeEnum.Operational);
            //Act
            var ex = Assert.ThrowsException<System.Exception>(() => testlet.Randomize());
            //Assert
            Assert.AreEqual(ex.Message, "There are not enough operational items");
        }
        [TestMethod]
        public void FailWithRandomException()
        {
            //Arrange
            var testlet = new Testlet(1, "test1", testItems);
            //Asser
            Assert.ThrowsException<System.Exception>(() => testlet.Randomize());
        }
    }
}