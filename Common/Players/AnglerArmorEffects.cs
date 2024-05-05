namespace AutoFisher.Common.Players
{
    public class AnglerArmorEffects : ModPlayer
    {
        public override void UpdateEquips()
        {
            if (ConfigContent.NotEnableMod) return;
            if (!ConfigContent.Sever.Common.AnglerArmorsGenerateEffects.InVanitySlots &&
                !ConfigContent.Sever.Common.AnglerArmorsGenerateEffects.InInventoryOrVoidBag &&
                !ConfigContent.Sever.Common.AnglerArmorsGenerateEffects.InInventoryOrVoidBagAndFavorited) return;

            bool[] added = new bool[3] { false, false, false };
            for (int i = 0; i < 3; i++)
            {
                int id = i + ItemID.AnglerHat;
                if (Player.armor[i].type == id) continue;
                if (Player.armor[i + 10].type == id)
                {
                    if (ConfigContent.Sever.Common.AnglerArmorsGenerateEffects.InVanitySlots)
                    {
                        added[i] = true;
                        continue;
                    }
                }
                int index = Player.FindItemInInventoryOrOpenVoidBag(id, out bool inVoidBag);
                if (index >= 0)
                {
                    if (ConfigContent.Sever.Common.AnglerArmorsGenerateEffects.InInventoryOrVoidBag) added[i] = true;
                    else if (ConfigContent.Sever.Common.AnglerArmorsGenerateEffects.InInventoryOrVoidBagAndFavorited)
                    {
                        Item item = (inVoidBag ? Player.bank4.item : Player.inventory)[index];
                        if (item.favorited) added[i] = true;
                    }
                }
            }

            foreach (bool flag in added)
            {
                if (flag) Player.fishingSkill += 5;
            }

            /*Player.FindItem(ItemID.AnglerHat, Player.armor.Skip(10).Take(3).ToArray());*/
        }
    }
}
