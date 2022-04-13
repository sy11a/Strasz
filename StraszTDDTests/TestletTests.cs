using StraszTDD;
using StraszTDD.Services;
using StraszTDDTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace StraszTDDTests
{
    public class TestletTests
    {
        [Fact]
        public void ShouldReturnSortedListOfItems()
        {
            var testlet = TestletSetUp(GetTestletItmes(6,4), new ShufflerService<Item>(new Random()));

            var result = testlet.Randomize();

            Assert.NotNull(result);

            Assert.NotEmpty(result);

            Assert.Equal(ITEM_LIST_LENGTH, result.Count());

            result.Take(Config.TESTLET_PRIORITY_AMOUNT)
                .Select(x => x.ItemType)
                .ToList()
                .ForEach(x => Assert.True(x == ItemTypeEnum.Pretest));

            Assert.Equal(Config.TESTLET_OPERATIONAL_AMOUNT, result.Where(x => x.ItemType == ItemTypeEnum.Operational).Count());
            Assert.Equal(Config.TESTLET_PRETEST_AMOUNT, result.Where(x => x.ItemType == ItemTypeEnum.Pretest).Count());

            Assert.True(result.Distinct().Count() == result.Count());
        }

        [Fact]
        public void ShouldShuffleAsExpected()
        {
            List<Item> testRandomItems = new List<Item>
            {
                new Item(){ItemId = "0", ItemType = ItemTypeEnum.Pretest},
                new Item(){ItemId = "1", ItemType = ItemTypeEnum.Pretest},
                new Item(){ItemId = "2", ItemType = ItemTypeEnum.Pretest},
                new Item(){ItemId = "3", ItemType = ItemTypeEnum.Pretest},
                new Item(){ItemId = "4", ItemType = ItemTypeEnum.Operational},
                new Item(){ItemId = "5", ItemType = ItemTypeEnum.Operational},
                new Item(){ItemId = "6", ItemType = ItemTypeEnum.Operational},
                new Item(){ItemId = "7", ItemType = ItemTypeEnum.Operational},
                new Item(){ItemId = "8", ItemType = ItemTypeEnum.Operational},
                new Item(){ItemId = "9", ItemType = ItemTypeEnum.Operational},
            };

            var shufflerService = new ShufflerService<Item>(new Random(9999));
            var testlet = new Testlet("testRandom", testRandomItems, shufflerService);

            var testletResult = testlet.Randomize();

            List<string> resultIdOrder = testletResult.Select(x => x.ItemId).ToList();
            string result = String.Join("", resultIdOrder);

            Assert.Equal("1290835764", result);
        }

        [Fact]
        public void ShouldRandomizeDifferentlyWithDifferentSeed()
        {
            for(int i = 0; i < 1000; i++)
            {
                var testlet = TestletSetUp(GetTestletItmes(6, 4), new ShufflerService<Item>(new Random()));

                var testletResult = testlet.Randomize();
                var testletResult2 = testlet.Randomize();

                Assert.False(Enumerable.SequenceEqual<Item>(testletResult, testletResult2));
            }

        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(20, 2)]
        [InlineData(2, 20)]
        [InlineData(20, 20)]
        public void ShouldThrowExceptrionIfDataSizeNotCorrect(int operationalItems, int pretestItems)
        {
            var shufflerService = new ShufflerService<Item>(new Random());
            Assert.Throws<TestletDataException>(() 
                => new Testlet("exceptionTest", GetTestletItmes(operationalItems, pretestItems), shufflerService));
        }

        private const int ITEM_LIST_LENGTH = 10;
        private const int RANDOM_STRING_LENGTH = 7;

        private Testlet TestletSetUp(List<Item> testletItems, ShufflerService<Item> shufflerService)
        {
            return new Testlet(RandomizeHelper.GetRandomString(RANDOM_STRING_LENGTH), testletItems, shufflerService);
        }

        private List<Item> GetTestletItmes(int operationalItems, int pretestItems)
        {
            var itemsTypeCount = new Dictionary<ItemTypeEnum, int>();
            itemsTypeCount.Add(ItemTypeEnum.Operational, operationalItems);
            itemsTypeCount.Add(ItemTypeEnum.Pretest, pretestItems);

            List<Item> testItems = new List<Item>();

            foreach(var item in itemsTypeCount)
            {
                for(int i =0; i<item.Value; i++)
                {
                    testItems.Add(new Item() {ItemId = RandomizeHelper.GetRandomString(ITEM_LIST_LENGTH), ItemType = item.Key });
                }
            }

            return testItems;
        }

    }
}
