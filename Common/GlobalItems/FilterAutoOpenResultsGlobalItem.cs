using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace AutoFisher.Common.GlobalItems;

internal class FilterAutoOpenResultsGlobalItem : GlobalItem
{
    internal static bool Enabled { get; set; }

    public override void OnSpawn(Item item, IEntitySource source)
    {
        if (!Enabled)
            return;

        if (source is not EntitySource_ItemOpen)
            return;

        var player = Main.LocalPlayer;
        var config = ConfigContent.Client.AutoOpenFilter;
        if (!config.EnableAutoOpenFilter)
            return;

        var filtered = config.BlockList.Any(def => def.Type == item.type);
        if (config.TurnBlockListToAllowList)
            filtered = !filtered;

        if (ConfigContent.SellAllCatches || ConfigContent.SellFilteredCatches && filtered || ConfigContent.SellUnfilteredCatches && !filtered)
        {
            Enabled = false;
            AutoFisherUtils.SellItem(player, item, out _);
            Enabled = true;
            item.TurnToAir();
        }
    }
}
