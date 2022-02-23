using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DemoTest.Core.Tests
{
    [TestClass]
    public class UnitTest1
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
        public void TestRandomize()
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
        public void TestFirstAndSecondArePreTestItems()
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
        public void TestRemainingPreTestItems()
        {
            //Arrange
            var testlet = new Testlet(1, "test1", testItems);
            //Act
            var randTestItems = testlet.Randomize();
            //Assert
            Assert.IsTrue(randTestItems.Skip(2).Where(t => t.ItemType == ItemTypeEnum.PreTest).Count() == 2);
            Assert.IsTrue(randTestItems.Skip(2).Where(t => t.ItemType == ItemTypeEnum.Operational).Count() == 6);
        }
    }
}