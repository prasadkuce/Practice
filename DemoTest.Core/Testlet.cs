namespace DemoTest.Core
{
    public static class ListExtensions
    {
        private static Random rnd = new Random();
        //List that maintains the random number generated to check the for uniques.
        private static List<int> randomList = new List<int>();
        //Create an Extension Method for the List<T>
        public static IList<T> Randomize<T>(this IList<T> list)
        {
            int n = list.Count;
            // Make N swaps in the list for sshuffling through. We can reduce this to lesser number also. 
            while (n > 0) 
            {
                n--;
                // generate all numbers in the list count by random. We are maintaining a list to make sure random return all uniques.
                int k = rnd.Next(n + 1);
                if (!randomList.Contains(k)) 
                    randomList.Add(k);
                else
                    k = rnd.Next(n + 1);
                //Start Swapping
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }
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

                //int n = 10 / new Random().Next(0, 5); // for simulating runtime exception
                
                if (Items != null && Items.Count > 0)
                {
                    IEnumerable<Item> tempList;
                    List<Item> preTestList = new List<Item>();
                    List<Item> opsTestList = new List<Item>();

                    tempList = Items.Where(l => l.ItemType == ItemTypeEnum.PreTest);
                    if (tempList != null && tempList.Any())
                    {
                        // Shuffle the list itmes based on random swaps
                        tempList = tempList.ToList().Randomize();
                        // Take 2 Pretest Iterms for the randomized List
                        preTestList.AddRange(tempList.Take(2)); 
                    }
                    
                    if (preTestList != null && preTestList.Count == 2)
                        retVal.AddRange(preTestList);
                    else
                        throw new Exception("There are not enough pretest items");

                    // Pick all remaining list items
                    tempList = Items.Except(preTestList);
                    if (tempList != null && tempList.Any()) 
                    {
                        // Shuffle the list itmes based on random swaps
                        tempList = tempList.ToList().Randomize(); 
                        opsTestList.AddRange(tempList);
                    }
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