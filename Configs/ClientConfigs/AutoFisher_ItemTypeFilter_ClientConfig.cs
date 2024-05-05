using System.ComponentModel;
using Terraria.GameContent.Creative;

namespace AutoFisher.Configs.ClientConfigs
{
    public class AutoFisher_ItemTypeFilter_ClientConfig : ModConfig, IFilterConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        bool IFilterConfig.Enable => EnableItemTypeFilter;

        [Header("ItemTypeFilter")]
        [DefaultValue(false)]
        public bool EnableItemTypeFilter;
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
        [DefaultValue(false)]
        public bool Mod;
        [DefaultValue(false)]
        public bool NotMod;
    }

    public class AllItems
    {
        private static readonly List<IItemEntryFilter> _filters = new()
        {
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
        };

        public static bool FitsFilter(Item item, ItemType itemType)
        {
            if (itemType != ItemType.Misc) return _filters[(int)itemType].FitsFilter(item);
            else
            {
                foreach (var filter in _filters)
                {
                    if (filter.FitsFilter(item)) return false;
                }
                return true;
            }
        }

        [DefaultValue(false)]
        public bool Weapon;
        [DefaultValue(false)]
        public bool Armor;
        [DefaultValue(false)]
        public bool Vanity;
        [DefaultValue(false)]
        public bool BuildingBlock;
        [DefaultValue(false)]
        public bool Furniture;
        [DefaultValue(false)]
        public bool Accessories;
        [DefaultValue(false)]
        public bool MiscAccessories;
        [DefaultValue(false)]
        public bool Consumables;
        [DefaultValue(false)]
        public bool Tools;
        [DefaultValue(false)]
        public bool Materials;
        [DefaultValue(false)]
        public bool Misc;
    }

    public class Fishes
    {
        [DefaultValue(false)]
        public bool NotQueseFish;
        [DefaultValue(false)]
        public bool QueseFish;
    }

    /*public class UsableItems
    {
        [DefaultValue(false)]
        public bool AllUsableItems;
        [DefaultValue(false)]
        public bool Accessory;
        [DefaultValue(false)]
        public bool Weapon;
        [DefaultValue(false)]
        public bool Sword;
        [DefaultValue(false)]
        public bool Pickaxe;
        [DefaultValue(false)]
        public bool Hammer;
        [DefaultValue(false)]
        public bool Chainsaw;
        [DefaultValue(false)]
        public bool Spear;
        [DefaultValue(false)]
        public bool PetSummon;
        [DefaultValue(false)]
        public bool RecoveryPotion;
        [DefaultValue(false)]
        public bool MountSummon;
        [DefaultValue(false)]
        public bool CraftingStation;
        [DefaultValue(false)]
        public bool GrabBag;
        [DefaultValue(false)]
        public bool Consumable;
        [DefaultValue(false)]
        public bool Tool;
        [DefaultValue(false)]
        public bool Painting;
    }*/

    public class Crates
    {
        [DefaultValue(false)]
        public bool PreHardmodeCrates;
        [DefaultValue(false)]
        public bool HardmodeCrates;
    }

    public class Junks
    {
        [DefaultValue(false)]
        public bool OldShoe;
        [DefaultValue(false)]
        public bool Seaweed;
        [DefaultValue(false)]
        public bool TinCan;
        [DefaultValue(false)]
        public bool JojaCola;
    }
}
