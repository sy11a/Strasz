using System;

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
            int n = items.Length;

            while(n > 0)
            {
                int k = _rnd.Next(n);
                n--;

                SwapElements(items, n, k);
            }
        }

        public void SwapElements(T[] items, int n, int k)
        {
            T item = items[n];
            items[n] = items[k];
            items[k] = item;
        }
    }
}
