using StraszTDD.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StraszTDD
{
    public class Testlet
    {
        public string TestletId;

        private List<Item> _items;

        private ShufflerService<Item> _shufflerService;

        private const int _firstOrderPretestAmount = 2;
        private const int _pretestAmount = 4;
        private const int _operationalAmount = 6;

        public Testlet(string testletId, List<Item> items, ShufflerService<Item> shufflerService)
        {
            TestletId = testletId;
            _shufflerService = shufflerService;
            _items = items;
        }

        public IEnumerable<Item> Randomize()
        {
            Item[] sortedItems = _items
                .Validate(list => list.Where(item => item.ItemType == ItemTypeEnum.Operational).Count() == _operationalAmount)
                .Validate(list => list.Where(item => item.ItemType == ItemTypeEnum.Pretest).Count() == _pretestAmount)
                .ToArray();

           _shufflerService.RandomShuffle(sortedItems);

            var pretestItems = sortedItems
                .Where(item => item.ItemType == ItemTypeEnum.Pretest)
                .Take(_firstOrderPretestAmount);

            int swapIndex = 0;

            while(swapIndex < _firstOrderPretestAmount)
            {
                var priorityElement = pretestItems.ElementAt(swapIndex);

                var currentIndex = Array.IndexOf(sortedItems, priorityElement);

                _shufflerService.SwapElements(sortedItems, currentIndex, swapIndex);

                swapIndex++;
            }

            return sortedItems;

        }

    }
}
