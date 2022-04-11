using StraszTDD;
using StraszTDD.Services;
using StraszTDDTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;


namespace StraszTDDTests
{
    public class TestletTests
    {
        [Fact]
        public void ShouldReturnSortedListOfItems()
        {
            var testlet = TestletSetUp(6, 4);

            var result = testlet.Randomize();

            int firstOrderPretestAmount = (int)(typeof(Testlet)
                .GetField("_firstOrderPretestAmount", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy).GetValue(testlet));

            int pretestAmount = (int)(typeof(Testlet)
                .GetField("_pretestAmount", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy).GetValue(testlet));

            int operationalAmount = (int)(typeof(Testlet)
                .GetField("_operationalAmount", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy).GetValue(testlet));

            Assert.NotNull(result);

            Assert.NotEmpty(result);

            Assert.Equal(_itemListLength, result.Count());

            result.Take(firstOrderPretestAmount)
                .Select(x => x.ItemType)
                .ToList()
                .ForEach(x => Assert.True(x == ItemTypeEnum.Pretest));

            Assert.Equal(operationalAmount, result.Where(x => x.ItemType == ItemTypeEnum.Operational).Count());
            Assert.Equal(pretestAmount, result.Where(x => x.ItemType == ItemTypeEnum.Pretest).Count());

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
                var testlet = TestletSetUp(6, 4);

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
            var testlet = TestletSetUp(operationalItems, pretestItems);
            Assert.Throws<TestletDataException>(() => testlet.Randomize());
        }

        private const int _itemListLength = 10;
        private const int _randomStringLength = 7;

        private Testlet TestletSetUp(int operationalItems, int pretestItems)
        {
            var testletItemsSetUp = new Dictionary<ItemTypeEnum, int>();
            testletItemsSetUp.Add(ItemTypeEnum.Operational, operationalItems);
            testletItemsSetUp.Add(ItemTypeEnum.Pretest, pretestItems);
            var shufflerService = new ShufflerService<Item>(new Random());

            return new Testlet(RandomizeHelper.GetRandomString(_randomStringLength), GetTestletItmes(testletItemsSetUp), shufflerService);
        }

        private List<Item> GetTestletItmes(IDictionary<ItemTypeEnum,int> itemsTypeCount)
        {
            List<Item> testItems = new List<Item>();

            foreach(var item in itemsTypeCount)
            {
                for(int i =0; i<item.Value; i++)
                {
                    testItems.Add(new Item() {ItemId = RandomizeHelper.GetRandomString(_randomStringLength), ItemType = item.Key });
                }
            }

            return testItems;
        }

    }
}
