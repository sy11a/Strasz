namespace StarszTDD
{
    public class Item
    {
        public string ItemId;
        public ItemTypeEnum ItemType;
    }

    public enum ItemTypeEnum
    {
        Pretest = 0,
        Operational = 1
    }
}
