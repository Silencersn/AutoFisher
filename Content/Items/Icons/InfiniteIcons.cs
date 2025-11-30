namespace AutoFisher.Content.Items.Icons;

public abstract class ItemIcon : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 0;
    }

    public override LocalizedText Tooltip => LocalizedText.Empty;

    public static string GetIcon<T>() where T : ModItem
    {
        return $"[i:{ModContent.ItemType<T>()}]";
    }
}

public class TabAccessories : ItemIcon
{
    public override LocalizedText DisplayName => Language.GetText("CreativePowers.TabAccessories");
}
public class TabAccessoriesMisc : ItemIcon
{
    public override LocalizedText DisplayName => Language.GetText("CreativePowers.TabAccessoriesMisc");
}
public class TabArmor : ItemIcon
{
    public override LocalizedText DisplayName => Language.GetText("CreativePowers.TabArmor");
}
public class TabBlocks : ItemIcon
{
    public override LocalizedText DisplayName => Language.GetText("CreativePowers.TabBlocks");
}
public class TabConsumables : ItemIcon
{
    public override LocalizedText DisplayName => Language.GetText("CreativePowers.TabConsumables");
}
public class TabFurniture : ItemIcon
{
    public override LocalizedText DisplayName => Language.GetText("CreativePowers.TabFurniture");
}
public class TabMaterials : ItemIcon
{
    public override LocalizedText DisplayName => Language.GetText("CreativePowers.TabMaterials");
}
public class TabMisc : ItemIcon
{
    public override LocalizedText DisplayName => Language.GetText("CreativePowers.TabMisc");
}
public class TabTools : ItemIcon
{
    public override LocalizedText DisplayName => Language.GetText("CreativePowers.TabTools");
}
public class TabVanity : ItemIcon
{
    public override LocalizedText DisplayName => Language.GetText("CreativePowers.TabVanity");
}
public class TabWeapons : ItemIcon
{
    public override LocalizedText DisplayName => Language.GetText("CreativePowers.TabWeapons");
}
