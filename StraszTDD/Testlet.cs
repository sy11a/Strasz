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

        private const int _priorityItemsAmount = 2;
        private const int _pretestAmount = 4;
        private const int _operationalAmount = 6;

        private IEnumerable<Item> _validItems 
        {
            get => _items
                .Validate(list => CountValidation(ItemTypeEnum.Operational, _operationalAmount))
                .Validate(list => CountValidation(ItemTypeEnum.Pretest, _pretestAmount));
        }

        private bool CountValidation(ItemTypeEnum type, int count) 
            =>_items.Where(item => item.ItemType == type).Count() == count;

        public Testlet(string testletId, List<Item> items, ShufflerService<Item> shufflerService)
        {
            TestletId = testletId;
            _shufflerService = shufflerService;
            _items = items;
        }

        public IEnumerable<Item> Randomize()
        {
            Item[] randomizedItems = _validItems.ToArray();

           _shufflerService.RandomShuffle(randomizedItems);

            var indexSwapPairs = randomizedItems
                .Where(item => item.ItemType == ItemTypeEnum.Pretest)
                .Take(_priorityItemsAmount)
                .Select((item, index) => (index, Array.IndexOf(randomizedItems, item)));

            foreach(var (currentIndex, targetIndex) in indexSwapPairs)
                _shufflerService.Swap(ref randomizedItems[currentIndex], ref randomizedItems[targetIndex]);

            return randomizedItems;

        }
    }
}
