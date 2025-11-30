using AutoFisher.Content.Items;

namespace AutoFisher.Common.Players;

public class FishingStartingPlayer : ModPlayer
{
    public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
    {
        if (ConfigContent.Server.Common.DisableNewItemOfThisMod)
            yield break;

        yield return new Item(ModContent.ItemType<FisherCrate>());
    }
}
