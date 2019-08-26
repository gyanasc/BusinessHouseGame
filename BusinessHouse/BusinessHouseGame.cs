using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessHouse
{
    class BusinessHouseGame
    {
        /// <summary>
        /// It will call BoardMove function with default array and player
        /// </summary>
        public static IEnumerable<KeyValuePair<int, int>> Start()
        {
            char[] cellsArr = { 'E', 'E', 'H', 'T', 'E', 'E', 'J', 'E', 'E', 'H', 'E', 'E', 'T', 'E', 'E', 'E', 'E', 'J', 'E', 'H', 'E', 'E', 'T', 'E', 'E', 'E', 'J', 'E', 'E', 'E', 'E', 'H', 'T', 'E', 'J', 'E', 'E', 'E' };
            int noOfplayers = 3, diceMin = 2, diceMax = 12, entryFeePerPlayer = 1000, hotWorth = 200, hotRent = 50, jailFine = 150, traisure = 200, noOfChances = 10;
            bool collectAllCell = true;
            return BoardMove(cellsArr, noOfplayers);
        }

        /// <summary>
        /// Business house is a board game which require minimum two players. Player uses a random
        /// number between 2-12 and move on the board accordingly.
        /// </summary>
        /// <param name="cellsArr">send board details or cells array with celltype(E → EMPTY, J → JAIL, T → TREASURE, H → HOTEL)</param>
        /// <param name="noOfplayers">is the no of players want to play this game</param>
        /// <param name="diceMin">is the lowest output of one dice</param>
        /// <param name="diceMax">is the highest output of one dice</param>
        /// <param name="entryFeePerPlayer">is the minimum entry fee per player</param>
        /// <param name="hotWorth">is the hotel worth to buy</param>
        /// <param name="hotRent">is the hotel rent to pay</param>
        /// <param name="jailFine">is the fine for jail</param>
        /// <param name="traisure">is the value of traisure</param>
        /// <param name="noOfChances">is the no of chances player will get </param>
        /// <param name="collectAllCell">if true it will collect and follow all cell rule on the way from player position to dice output position, 
        /// else it will collect only player expected (current position + dice output) position</param>
        public static IEnumerable<KeyValuePair<int, int>> BoardMove(char[] cellsArr, int noOfplayers, int diceMin = 2, int diceMax = 12, int entryFeePerPlayer = 1000, int hotWorth = 200, int hotRent = 50, int jailFine = 150, int traisure = 200, int noOfChances = 10, bool collectAllCell = true)
        {
            if (noOfplayers < 2)
                return new List<KeyValuePair<int, int>>();

            var playerAmtDict = new Dictionary<int, int>();
            var playerPosDict = new Dictionary<int, int>();
            var hotelPlayerDict = new Dictionary<int, int>();
            var playerHotelCntDict = new Dictionary<int, int>();

            for (int chance = 1; chance <= noOfChances; chance++)
            {
                for (int player = 1; player <= noOfplayers; player++)
                {
                    Random random = new Random();
                    int diceOutput = random.Next(diceMin, diceMax);
                    
                    int playerPos;
                    playerPosDict.TryGetValue(player, out playerPos);

                    if (playerPos >= cellsArr.Length - 1)
                        continue;

                    int curPosition = (playerPos + diceOutput) > cellsArr.Length ? cellsArr.Length : (playerPos + diceOutput);
                    
                    int playerAmt;
                    playerAmtDict.TryGetValue(player, out playerAmt);
                    playerAmt = playerAmt == 0 ? entryFeePerPlayer : playerAmt;

                    if (collectAllCell)
                    {
                        for (int i = playerPos; i < curPosition; i++)
                        {
                            if (cellsArr[i] == 'H')
                            {
                                int hotelOwner;
                                hotelPlayerDict.TryGetValue(i, out hotelOwner);

                                if (hotelOwner == 0)
                                {
                                    if (playerAmt >= hotWorth)
                                    {
                                        playerAmt = playerAmt - hotWorth;
                                        hotelPlayerDict[i] = player;
                                        int count;
                                        playerHotelCntDict.TryGetValue(player, out count);
                                        playerHotelCntDict[player] = ++count;
                                    }
                                }
                                else
                                {
                                    playerAmt = playerAmt - hotRent;

                                    //if(playerAmt>=hotRent)
                                    //{
                                    //    playerAmt = playerAmt - hotRent;
                                    //}
                                }
                            }
                            else if (cellsArr[i] == 'J')
                            {
                                playerAmt = playerAmt - jailFine;
                            }
                            else if (cellsArr[i] == 'T')
                            {
                                playerAmt = playerAmt + traisure;
                            }
                        }

                        //playerAmtDict[player] = playerAmt;
                    }
                    else
                    {
                        curPosition = playerPos == 0 ? curPosition - 1 : curPosition;

                        if (cellsArr[curPosition] == 'H')
                        {
                            int hotelOwner;
                            hotelPlayerDict.TryGetValue(curPosition, out hotelOwner);

                            if (hotelOwner == 0)
                            {
                                if (playerAmt >= hotWorth)
                                {
                                    playerAmt = playerAmt - hotWorth;
                                    hotelPlayerDict[curPosition] = player;
                                    int count;
                                    playerHotelCntDict.TryGetValue(player, out count);
                                    playerHotelCntDict[player] = ++count;
                                }
                            }
                            else
                            {
                                playerAmt = playerAmt - hotRent;

                                //if(playerAmt>=hotRent)
                                //{
                                //    playerAmt = playerAmt - hotRent;
                                //}
                            }
                        }
                        else if (cellsArr[curPosition] == 'J')
                        {
                            playerAmt = playerAmt - jailFine;
                        }
                        else if (cellsArr[curPosition] == 'T')
                        {
                            playerAmt = playerAmt + traisure;
                        }
                    }

                    playerPosDict[player] = curPosition;
                    playerAmtDict[player] = playerAmt;
                }
            }

            for (int player = 1; player <= noOfplayers; player++)
            {
                int count;
                playerHotelCntDict.TryGetValue(player, out count);

                playerAmtDict[player] = playerAmtDict[player] + (count * hotWorth);
            }

            var orderedDict = playerAmtDict.OrderByDescending(o => o.Value);

            return orderedDict;
        }
    }
}
