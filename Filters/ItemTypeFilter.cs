namespace AutoFisher.Filters;

public class ItemTypeFilter : ACatchFilter<AutoFisher_ItemTypeFilter_ClientConfig>
{
    // https://terraria.wiki.gg/wiki/Fishing#Fish
    public static readonly bool[] IsVanillaFish = ItemID.Sets.Factory.CreateBoolSet(false,
        ItemID.ArmoredCavefish, ItemID.AtlanticCod, ItemID.Bass, ItemID.BlueJellyfish, ItemID.ChaosFish,
        ItemID.CrimsonTigerfish, ItemID.Damselfish, ItemID.DoubleCod, ItemID.Ebonkoi, ItemID.FlarefinKoi,
        ItemID.Flounder, ItemID.FrostMinnow, ItemID.GoldenCarp, ItemID.GreenJellyfish, ItemID.Hemopiranha,
        ItemID.Honeyfin, ItemID.NeonTetra, ItemID.Obsidifish, ItemID.PinkJellyfish, ItemID.PrincessFish,
        ItemID.Prismite, ItemID.RedSnapper, ItemID.RockLobster, ItemID.Salmon, ItemID.Shrimp,
        ItemID.SpecularFish, ItemID.Stinkfish, ItemID.Trout, ItemID.Tuna, ItemID.VariegatedLardfish);

    public override bool FitsFilter(Item item, FishingAttempt attempt)
    {
        int type = item.type;

        var modItems = Config.ModItems;
        if (modItems.Mod && item.ModItem is not null) return true;
        if (modItems.NotMod && item.ModItem is null) return true;

        var fishes = Config.Fishes;
        bool isCommonFish = IsVanillaFish[type];
        bool isQuestFish = Main.anglerQuestItemNetIDs.Contains(item.type);
        if (fishes.NotFish && !isCommonFish && !isQuestFish) return true;
        if (fishes.NotQueseFish && isCommonFish) return true;
        if (fishes.QueseFish && isQuestFish) return true;

        var crates = Config.Crates;
        if (crates.PreHardmodeCrates && ItemID.Sets.IsFishingCrate[type] && !ItemID.Sets.IsFishingCrateHardmode[type]) return true;
        if (crates.HardmodeCrates && ItemID.Sets.IsFishingCrateHardmode[type]) return true;
        if (crates.NonCrates && !ItemID.Sets.IsFishingCrate[type]) return true;

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
