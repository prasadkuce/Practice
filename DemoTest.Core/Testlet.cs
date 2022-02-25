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
            var retVal = new List<Item>();
            try
            {
                if(Items == null)
                    throw new Exception("Testlet Items are not initialized.");

                int n = 10 / new Random().Next(0, 5); // for simulating runtime exception
                
                if (Items != null && Items.Count > 0)
                {
                    IEnumerable<Item> tempList;
                    List<Item> preTestList = new List<Item>();
                    List<Item> opsTestList = new List<Item>();

                    tempList = Items.Where(l => l.ItemType == ItemTypeEnum.PreTest);
                    if (tempList != null && tempList.Any())
                        preTestList.AddRange(tempList.OrderBy(x => new Random().Next()).Take(2).ToList());
                    
                    if (preTestList != null && preTestList.Count == 2)
                        retVal.AddRange(preTestList);
                    else
                        throw new Exception("There are not enough pretest items");

                    tempList = Items.Except(preTestList);
                    if (tempList != null && tempList.Any())
                        opsTestList.AddRange(tempList.OrderBy(x => new Random().Next()).ToList());
                    if (opsTestList != null && opsTestList.Count == 8)
                        retVal.AddRange(opsTestList);
                    else
                        throw new Exception("There are not enough operational items");
                }
                else
                    throw new Exception("Testlet Items are empty. Add some testlet items");
            }
            catch(Exception ex)
            {
                throw;
            }

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