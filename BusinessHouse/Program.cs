using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessHouse
{
    class Program
    {
        static void Main(string[] args)
        {
            //I called start method with default board and player, you can call BoardMove method with your own all value
            var orderedDict = BusinessHouseGame.Start();
            foreach (var keyValue in orderedDict)
            {
                Console.WriteLine("Player {0} has total worth {1}", keyValue.Key, keyValue.Value);
            }

            Console.WriteLine("Press any key to exit!!");
            Console.ReadLine();
        }
    }
}
