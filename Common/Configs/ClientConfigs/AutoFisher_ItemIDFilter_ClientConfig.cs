using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using Terraria.GameContent;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using Terraria.UI.Chat;

namespace AutoFisher.Common.Configs.ClientConfigs;

public class AutoFisher_ItemIDFilter_ClientConfig : ModConfig, IFilterConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;
    bool IFilterConfig.Enable => EnableItemIDFilter;

    public PromptEnableAllFilters PromptEnableAllFilters = new();

    [Header("ItemIDFilter")]
    [DefaultValue(false)]
    public bool EnableItemIDFilter;
    [DefaultValue(false)]
    public bool TurnBlockListToAllowList;
    public List<ItemDefinition> BlockList = [];
    [Header("CalculateCatches")]
    [DefaultValue(2000)]
    [Range(0, 5000)]
    [Increment(500)]
    [Slider]
    [DrawTicks]
    public int Attempts;
    [DefaultValue(true)]
    public bool CalculateImmediately;
    public List<CatchItem> CatchesInTheLakeWhereCurrentOrLastFishing = [];
}

public class CatchItem
{
    private string _probability;

    public ItemDefinition Catch;
    [CustomModConfigItem(typeof(ButtonElement))]
    public ItemDefinition ClickToAddCatchToBlockList;
    public string Probability
    {
        get
        {
            return _probability ??= UnknownText?.Value!;
        }
        set
        {
            if (_probability is null || _probability == UnknownText.Value)
            {
                _probability = value;
            }
        }
    }

    public CatchItem()
    {
        Catch = new ItemDefinition(ItemID.None);
        ClickToAddCatchToBlockList = Catch;
        _probability = null!;
    }

    public CatchItem(int type, int count, int totalCount)
    {
        Catch = new ItemDefinition(type);
        ClickToAddCatchToBlockList = Catch;
        _probability = double.Clamp((double)count / totalCount, 0, 1).ToString("0.##%");
    }
}


internal class ButtonElement : ConfigElement<ItemDefinition>
{
    public override void OnBind()
    {
        base.OnBind();
        OnLeftClick += (ev, v) =>
        {
            ConfigContent.Client.ItemIDFilter.Modify(config =>
            {
                if (Value is null)
                    return;
                var str = Value.ToString();
                if (!config.BlockList.Any(item => item.ToString() == str))
                    config.BlockList.Add(Value);
            });
        };
    }

    private static Color MouseTextColor(Color color)
    {
        float scale = Main.mouseTextColor / 255f;
        return color * scale;
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        var drawLabel = DrawLabel;
        DrawLabel = false;
        base.DrawSelf(spriteBatch);
        DrawLabel = drawLabel;

        CalculatedStyle dimensions = GetDimensions();
        if (DrawLabel)
        {
            float settingsWidth = dimensions.Width + 1f;
            Vector2 baseScale = new(0.8f);
            Vector2 position = new(dimensions.X + 8f, dimensions.Y + 8f);

            string label = TextDisplayFunction();
            if (ReloadRequired && ValueChanged)
            {
                label += " - [c/FF0000:" + Language.GetTextValue("tModLoader.ModReloadRequired") + "]";
            }

            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, label, position, MouseTextColor(Color.Cyan), 0f, Vector2.Zero, baseScale, settingsWidth, 2f);
        }
    }
}
