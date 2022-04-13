using StraszTDD.Services;
using System.Collections.Generic;
using System.Linq;

namespace StraszTDD
{
    public class Testlet
    {
        public string TestletId;

        private IEnumerable<Item> _items;

        private ShufflerService<Item> _shufflerService;

        private bool CountValidation(IEnumerable<Item> items, ItemTypeEnum type, int count) 
            => items.Where(item => item.ItemType == type).Count() == count;

        public Testlet(string testletId, IEnumerable<Item> items, ShufflerService<Item> shufflerService)
        {
            TestletId = testletId;

            _shufflerService = shufflerService;

            _items = items.Validate(list => CountValidation(list, ItemTypeEnum.Operational, Config.TESTLET_OPERATIONAL_AMOUNT))
                          .Validate(list => CountValidation(list, ItemTypeEnum.Pretest, Config.TESTLET_PRETEST_AMOUNT));
        }

        public IEnumerable<Item> Randomize()
        {
            Item[] items = _items.ToArray();

           _shufflerService.RandomShuffle(items);

           _shufflerService.Prioritize(
               items,  Config.TESTLET_PRIORITY_AMOUNT, 
               (item) => item.ItemType == ItemTypeEnum.Pretest);

            return items;
        }
    }
}
