using System;
using System.Diagnostics;

namespace AutoFisher;

public static class AutoFisherUtils
{
    public static void TryGiveItemToPlayerElseDropItem(Entity source, Player player, Item item, bool newAndShiny)
    {
        item.newAndShiny = newAndShiny;
        Item overflow = player.GetItem(player.whoAmI, item, default);
        if (overflow.stack > 0)
        {
            player.QuickSpawnItem(new EntitySource_OverfullInventory(player, nameof(AutoFisher)), overflow.type, overflow.stack);
        }
        else
        {
            item.position = source.Center;
            item.active = true;
            PopupText.NewText(PopupTextContext.RegularItemPickup, item, 0);
        }
    }
    public static string GetTimeString(string end = "")
    {
        float hours = Utils.GetDayTimeAs24FloatStartingFromMidnight();
        float minutes = (hours - (int)hours) * 60;
        float seconds = (minutes - (int)minutes) * 60;

        static string format(float value)
        {
            return ((int)value).ToString().PadLeft(2, '0');
        }

        return $"[{format(hours)}:{format(minutes)}:{format(seconds)}]{end}";
    }
    public static string GetItemIconString(int type, int stack = 1)
    {
        return $"[i/s{stack}:{type}]";
    }

    public static void SplitCoins(long count, Span<int> splitCoins)
    {
        Debug.Assert(splitCoins.Length >= 4);

        long sum = 0;
        long coinValue = Item.platinum;
        for (int i = 3; i >= 0; i--)
        {
            splitCoins[i] = (int)((count - sum) / coinValue);
            sum += splitCoins[i] * coinValue;
            coinValue /= 100;
        }
    }
    public static void SellItem(Player player, Item item, out long value)
    {
        if (item.value <= 0)
        {
            value = 0;
            return;
        }

        Span<int> coins = stackalloc int[4];
        value = (long)item.value * item.stack / 5;
        if (value is 0)
            value = 1;
        SplitCoins(value, coins);

        var source = player.GetSource_OpenItem(item.type);
        for (int i = 0; i < 4; i++)
        {
            if (coins[i] > 0)
                player.QuickSpawnItem(source, ItemID.CopperCoin + i, coins[i]);
        }
    }
    public static Item? FindBait(Player player, bool includeInventory, bool includeOpenVoidBag, bool includePiggyBank, bool includeSafe, bool includeDefendersForge)
    {
        if (includeInventory)
        {
            foreach (var item in player.inventory)
            {
                if (item.bait > 0)
                    return item;
            }
        }

        if (includeOpenVoidBag && player.useVoidBag())
        {
            foreach (var item in player.bank4.item)
            {
                if (item.bait > 0)
                    return item;
            }
        }

        if (includePiggyBank)
        {
            foreach (var item in player.bank.item)
            {
                if (item.bait > 0)
                    return item;
            }
        }

        if (includeSafe)
        {
            foreach (var item in player.bank2.item)
            {
                if (item.bait > 0)
                    return item;
            }
        }

        if (includeDefendersForge)
        {
            foreach (var item in player.bank3.item)
            {
                if (item.bait > 0)
                    return item;
            }
        }

        return null;
    }
    public static int CountBait(Player player, bool includeInventory, bool includeOpenVoidBag, bool includePiggyBank, bool includeSafe, bool includeDefendersForge)
    {
        int count = 0;

        if (includeInventory)
        {
            foreach (var item in player.inventory)
            {
                if (item.bait > 0)
                    count += item.stack;
            }
        }

        if (includeOpenVoidBag && player.useVoidBag())
        {
            foreach (var item in player.bank4.item)
            {
                if (item.bait > 0)
                    count += item.stack;
            }
        }

        if (includePiggyBank)
        {
            foreach (var item in player.bank.item)
            {
                if (item.bait > 0)
                    count += item.stack;
            }
        }

        if (includeSafe)
        {
            foreach (var item in player.bank2.item)
            {
                if (item.bait > 0)
                    count += item.stack;
            }
        }

        if (includeDefendersForge)
        {
            foreach (var item in player.bank3.item)
            {
                if (item.bait > 0)
                    count += item.stack;
            }
        }

        return count;
    }
    public static void GetDayTimeAs24HoursMinutesSeconds(out int hours, out int minutes, out int seconds)
    {
        const int DayStartingTime = 16200; // 4:30 AM
        const int NightStartingTime = 70200; // 7:30 PM
        const int SecondsPerHour = 3600;
        const int SecondsPerMinute = 60;

        int time = Main.dayTime ? DayStartingTime : NightStartingTime;
        time += (int)Main.time;
        hours = time / SecondsPerHour;
        minutes = (time % SecondsPerHour) / SecondsPerMinute;
        seconds = time % SecondsPerMinute;
    }
}
