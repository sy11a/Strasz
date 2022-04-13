using StraszTDD.Services;
using System.Collections.Generic;
using System.Linq;

namespace StraszTDD
{
    public class Testlet
    {
        public string TestletId;

        private List<Item> _items;

        private ShufflerService<Item> _shufflerService;

        private IEnumerable<Item> _validItems 
        {
            get => _items
                .Validate(list => CountValidation(ItemTypeEnum.Operational, Config.TESTLET_OPERATIONAL_AMOUNT))
                .Validate(list => CountValidation(ItemTypeEnum.Pretest, Config.TESTLET_PRETEST_AMOUNT));
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
            Item[] items = _validItems.ToArray();

           _shufflerService.RandomShuffle(items);

           _shufflerService.Prioritize(
               items,  Config.TESTLET_PRIORITY_AMOUNT, 
               (item) => item.ItemType == ItemTypeEnum.Pretest);

            return items;
        }
    }
}
