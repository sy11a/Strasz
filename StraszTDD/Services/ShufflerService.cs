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
            for (int i = items.Length; i > 0; i--)
                Swap(ref items[i - 1], ref items[_rnd.Next(i)]);
        }

        public void Swap(ref T a, ref T b)
        {
            T tempswap = a;
            a = b;
            b = tempswap;
        }
    }
}
