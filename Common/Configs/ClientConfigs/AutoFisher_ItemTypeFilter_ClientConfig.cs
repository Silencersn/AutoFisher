using System.ComponentModel;
using Terraria.GameContent.Creative;

namespace AutoFisher.Common.Configs.ClientConfigs
{
    public class AutoFisher_ItemTypeFilter_ClientConfig : ModConfig, IFilterConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        bool IFilterConfig.Enable => EnableItemTypeFilter;

        [Header("ItemTypeFilter")]
        [DefaultValue(false)]
        public bool EnableItemTypeFilter = false;
        public ModItems ModItems = new();
        public Fishes Fishes = new();
        public Crates Crates = new();
        public Junks Junks = new();
        public AllItems AllItems = new();
    }

    public enum ItemType
    {
        Weapon,
        Armor,
        Vanity,
        BuildingBlock,
        Furniture,
        Accessories,
        MiscAccessories,
        Consumables,
        Tools,
        Materials,
        Misc
    }

    public class ModItems
    {
        public bool Mod = false;
        public bool NotMod = false;
    }

    public class AllItems
    {
        private static readonly List<IItemEntryFilter> _filters =
        [
            new ItemFilters.Weapon(),
            new ItemFilters.Armor(),
            new ItemFilters.Vanity(),
            new ItemFilters.BuildingBlock(),
            new ItemFilters.Furniture(),
            new ItemFilters.Accessories(),
            new ItemFilters.MiscAccessories(),
            new ItemFilters.Consumables(),
            new ItemFilters.Tools(),
            new ItemFilters.Materials()
        ];

        private class AddMiscFallbackFilter : ModSystem
        {
            public override void PostSetupContent()
            {
                _filters.Add(new ItemFilters.MiscFallback(_filters));
            }
        }

        public static bool FitsFilter(Item item, ItemType itemType)
        {
            return _filters[(int)itemType].FitsFilter(item);
        }

        public bool Weapon = false;
        public bool Armor = false;
        public bool Vanity = false;
        public bool BuildingBlock = false;
        public bool Furniture = false;
        public bool Accessories = false;
        public bool MiscAccessories = false;
        public bool Consumables = false;
        public bool Tools = false;
        public bool Materials = false;
        public bool Misc = false;
    }

    public class Fishes
    {
        public bool NotQueseFish = false;
        public bool QueseFish = false;
    }

    public class Crates
    {
        public bool PreHardmodeCrates = false;
        public bool HardmodeCrates = false;
    }

    public class Junks
    {
        public bool OldShoe = false;
        public bool Seaweed = false;
        public bool TinCan = false;
        public bool JojaCola = false;
    }
}
