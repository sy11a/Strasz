using System;
using System.Linq;

namespace StraszTDD.Services
{
    public class ShufflerService<T>
    {
        Random _rnd;

        public ShufflerService(Random random)
        {
            _rnd = random;
        }

        public void RandomShuffle(T[] items)
        {
            for (int i = items.Length; i > 0; i--)
                Swap(ref items[i - 1], ref items[_rnd.Next(i)]);
        }

        public void Prioritize(T[] items, int priorityAmount, Func<T,bool> priorityRule)
        {
            var indexSwapPairs = items
               .Where(item => priorityRule(item))
               .Take(priorityAmount)
               .Select((item, index) => (Array.IndexOf(items, item), index));

            foreach (var (currentIndex, targetIndex) in indexSwapPairs)
                Swap(ref items[currentIndex], ref items[targetIndex]);
        }

        private void Swap(ref T a, ref T b)
        {
            T tempswap = a;
            a = b;
            b = tempswap;
        }
    }
}
