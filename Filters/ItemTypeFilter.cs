namespace AutoFisher.Filters
{
    public class ItemTypeFilter : ACatchFilter<AutoFisher_ItemTypeFilter_ClientConfig>
    {
        public static readonly bool[] IsFish = ItemID.Sets.Factory.CreateBoolSet(false, 2303, 2299, 2290, 2436, 2317, 2305, 2304, 2313, 2318, 2312, 4401, 2306, 2308, 2437, 2319, 2302, 2315, 2438, 2307, 2310, 2301, 4402, 2298, 2316, 2309, 2321, 2297, 2300, 2311);

        public override bool FitsFilter(Item item, FishingAttempt attempt)
        {
            int type = item.type;

            var modItems = Config.ModItems;
            if (modItems.Mod && item.ModItem is not null) return true;
            if (modItems.NotMod && item.ModItem is null) return true;

            var fishes = Config.Fishes;
            if (fishes.NotQueseFish && IsFish[type]) return true;
            if (fishes.QueseFish && item.questItem) return true;

            var crates = Config.Crates;
            if (crates.PreHardmodeCrates && ItemID.Sets.IsFishingCrate[type] && !ItemID.Sets.IsFishingCrateHardmode[type]) return true;
            if (crates.HardmodeCrates && ItemID.Sets.IsFishingCrateHardmode[type]) return true;

            var junks = Config.Junks;
            if (junks.OldShoe && type is ItemID.OldShoe) return true;
            if (junks.Seaweed && type is ItemID.FishingSeaweed) return true;
            if (junks.TinCan && type is ItemID.TinCan) return true;
            if (junks.JojaCola && type is ItemID.JojaCola) return true;

            var allItems = Config.AllItems;
            if (allItems.Weapon && AllItems.FitsFilter(item, ItemType.Weapon)) return true;
            if (allItems.Armor && AllItems.FitsFilter(item, ItemType.Armor)) return true;
            if (allItems.Vanity && AllItems.FitsFilter(item, ItemType.Vanity)) return true;
            if (allItems.BuildingBlock && AllItems.FitsFilter(item, ItemType.BuildingBlock)) return true;
            if (allItems.Furniture && AllItems.FitsFilter(item, ItemType.Furniture)) return true;
            if (allItems.Accessories && AllItems.FitsFilter(item, ItemType.Accessories)) return true;
            if (allItems.MiscAccessories && AllItems.FitsFilter(item, ItemType.MiscAccessories)) return true;
            if (allItems.Consumables && AllItems.FitsFilter(item, ItemType.Consumables)) return true;
            if (allItems.Tools && AllItems.FitsFilter(item, ItemType.Tools)) return true;
            if (allItems.Materials && AllItems.FitsFilter(item, ItemType.Materials)) return true;
            if (allItems.Misc && AllItems.FitsFilter(item, ItemType.Misc)) return true;

            return false;
        }
    }
}
