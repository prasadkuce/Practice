namespace DemoTest.Core
{
    public class Testlet
    {
        public int TestletId { get; set; }
        public string Name { get; set; }
        private List<Item> Items;
        public Testlet(int testletId, string name, List<Item> items)
        {
            TestletId = testletId;
            Name = name;
            Items = items;
        }
        public List<Item> Randomize()
        {
            var preTestList = Items.Where(l => l.ItemType == ItemTypeEnum.PreTest).OrderBy(x => new Random().Next()).Take(2).ToList();
            var opsTestList = Items.Except(preTestList).OrderBy(x => new Random().Next()).ToList();
            
            var retVal = new List<Item>();
            retVal.AddRange(preTestList);
            retVal.AddRange(opsTestList);

            return retVal;
        }
    }
    public class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public ItemTypeEnum ItemType { get; set; }
    }
    public enum ItemTypeEnum
    {
        PreTest,
        Operational
    }
}