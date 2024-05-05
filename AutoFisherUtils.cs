namespace AutoFisher
{
    public static class AutoFisherUtils
    {
        /// <summary>
        /// Try to find item in player's Inventory, OpenVoidBag, PiggyBank, Safe and Defender's Forge.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <param name="includeInventory"></param>
        /// <param name="includeOpenVoidBag"></param>
        /// <param name="includePiggyBank"></param>
        /// <param name="inlcudeSafe"></param>
        /// <param name="includeDefendersForge"></param>
        /// <returns>If find item, return item and index. If not, return new Item(ItemID.None) </returns>
        public static Item FindItem(this Player player, Func<Item, bool> match, bool includeInventory, bool includeOpenVoidBag, bool includePiggyBank, bool inlcudeSafe, bool includeDefendersForge)
        {
            int index = -1;
            if (includeInventory)
            {
                index = FindItemInCollection(match, player.inventory);
                if (index > -1) return player.inventory[index];
            }
            if (includeOpenVoidBag && player.useVoidBag())
            {
                index = FindItemInCollection(match, player.bank4.item);
                if (index > -1) return player.bank4.item[index];
            }
            if (includePiggyBank)
            {
                index = FindItemInCollection(match, player.bank.item);
                if (index > -1) return player.bank.item[index];
            }
            if (inlcudeSafe)
            {
                index = FindItemInCollection(match, player.bank2.item);
                if (index > -1) return player.bank2.item[index];
            }
            if (includeDefendersForge)
            {
                index = FindItemInCollection(match, player.bank3.item);
                if (index > -1) return player.bank3.item[index];
            }
            return null;
        }
        public static int FindItemInCollection(Func<Item, bool> match, Item[] collection)
        {
            for (int i = 0; i < collection.Length; i++)
            {
                if (match(collection[i]))
                {
                    return i;
                }
            }
            return -1;
        }
        public static int CountItem(this Player player, Func<Item, bool> match, bool includeInventory, bool includeOpenVoidBag, bool includePiggyBank, bool inlcudeSafe, bool includeDefendersForge)
        {
            int count = 0;
            if (includeInventory)
            {
                count += CountItemInCollection(match, player.inventory);
            }
            if (includeOpenVoidBag && player.useVoidBag())
            {
                count += CountItemInCollection(match, player.bank4.item);
            }
            if (includePiggyBank)
            {
                count += CountItemInCollection(match, player.bank.item);
            }
            if (inlcudeSafe)
            {
                count += CountItemInCollection(match, player.bank2.item);
            }
            if (includeDefendersForge)
            {
                count += CountItemInCollection(match, player.bank3.item);
            }
            return count;
        }
        public static int CountItemInCollection(Func<Item, bool> match, Item[] collection)
        {
            int count = 0;
            for (int i = 0; i < collection.Length; i++)
            {
                if (match(collection[i]))
                {
                    count += collection[i].stack;
                }
            }
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="item"></param>
        /// <returns>return true if no excess</returns>
        public static bool TryGiveItemToPlayerElseDropItem(Entity source, Player player, Item item, bool newAndShiny)
        {
            item.newAndShiny = newAndShiny;
            Item excess = player.GetItem(player.whoAmI, item, default);
            if (excess.stack > 0)
            {
                player.QuickSpawnItem(new EntitySource_GetCatchesOrCoins(player), item.type, excess.stack);
                return false;
            }
            else
            {
                item.position.X = source.Center.X - item.width / 2;
                item.position.Y = source.Center.Y - item.height / 2;
                item.active = true;
                PopupText.NewText(PopupTextContext.RegularItemPickup, item, 0);
                return true;
            }
        }
    }
}
