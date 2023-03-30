using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PancakeBot.External_Classes
{
    internal class CardBuilder
    {
        public int[] cardNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30,31, 32,33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47,48, 49, 50, 51, 52, 53,54 , 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65 , 67, 68, 69   };
        public string[] cardSuits = { "pancakes" };

        public int SelectedNumber { get; internal set; }
        public string SelectedCard { get; internal set; }

        public CardBuilder()
        {
            var Random = new Random();
            int indexNumbers = Random.Next(0, this.cardNumbers.Length - 1);
            int indexSuit = Random.Next(0, this.cardSuits.Length - 1);

            this.SelectedNumber = this.cardNumbers[indexNumbers];
            this.SelectedCard = this.cardNumbers[indexNumbers] + " of " + this.cardSuits[indexSuit];
        }
    }
}
