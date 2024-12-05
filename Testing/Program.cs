namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int[]> testList = new List<int[]>();
            testList.Add(new int[] { 1, 2, 3 });

            int[] testArray = new int[] { 1, 2, 3 };
            Console.WriteLine(ListContains(testList, testArray));
        }
        static bool ListContains(List<int[]> list, int[] item)
        {
            bool contains = false;
            for (int i = 0; i < list.Count; i++)
            {
                int[] thisItem = list[i];
                if (thisItem.Length != item.Length)
                {
                    continue;
                }
                for (int j = 0; j < thisItem.Length; j++)
                {
                    if (thisItem[j] == item[j])
                    {
                        contains = true;
                    }
                    else
                    {
                        contains = false;
                        break;
                    }
                }
            }
            return contains;
        }
    }
}
