namespace AutoFisher.Common.Players
{
    public class AnglerArmorsBenefitsGrantor : ModPlayer
    {
        public override void UpdateEquips()
        {
            if (ConfigContent.NotEnableMod) return;

            var config = ConfigContent.Sever.Common.AnglerArmorsGenerateEffects;

            if (!config.InVanitySlots &&
                !config.InInventoryOrVoidBag &&
                !config.InInventoryOrVoidBagAndFavorited) return;

            Item?[] armors = [null, null, null];
            for (int i = 0; i < 3; i++)
            {
                int id = i + ItemID.AnglerHat;
                if (Player.armor[i].type == id) continue;
                if (Player.armor[i + 10].type == id)
                {
                    if (config.InVanitySlots)
                    {
                        armors[i] = Player.armor[i + 10];
                        continue;
                    }
                }
                int index = Player.FindItemInInventoryOrOpenVoidBag(id, out bool inVoidBag);
                if (index >= 0)
                {
                    Item armor = (inVoidBag ? Player.bank4.item : Player.inventory)[index];
                    if (config.InInventoryOrVoidBag) armors[i] = armor;
                    else if (config.InInventoryOrVoidBagAndFavorited)
                    {
                        if (armor.favorited) armors[i] = armor;
                    }
                }
            }

            foreach (Item? armor in armors)
            {
                if (armor is not null) Player.GrantArmorBenefits(armor);
            }
        }
    }
}
