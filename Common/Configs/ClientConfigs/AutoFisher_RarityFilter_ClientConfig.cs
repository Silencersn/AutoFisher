using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using ReLogic.Content;
using System.ComponentModel;
using Terraria.GameContent;
using Terraria.ModLoader.Config.UI;
using Terraria.UI.Chat;
using Terraria.UI;

namespace AutoFisher.Common.Configs.ClientConfigs
{
    public class AutoFisher_RarityFilter_ClientConfig : ModConfig, IFilterConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        bool IFilterConfig.Enable => EnableRarityFilter;

        public PromptEnableAllFilters PromptEnableAllFilters = new();

        [Header("RarityFilter")]
        [DefaultValue(false)]
        public bool EnableRarityFilter;
        [CustomModConfigItem(typeof(GrayColorBooleanElement))]
        [DefaultValue(false)]
        public bool Gray;
        [CustomModConfigItem(typeof(WhiteColorBooleanElement))]
        [DefaultValue(false)]
        public bool White;
        [CustomModConfigItem(typeof(BlueColorBooleanElement))]
        [DefaultValue(false)]
        public bool Blue;
        [CustomModConfigItem(typeof(GreenColorBooleanElement))]
        [DefaultValue(false)]
        public bool Green;
        [CustomModConfigItem(typeof(OrangeColorBooleanElement))]
        [DefaultValue(false)]
        public bool Orange;
        [CustomModConfigItem(typeof(LightRedColorBooleanElement))]
        [DefaultValue(false)]
        public bool LightRed;
        [CustomModConfigItem(typeof(PinkColorBooleanElement))]
        [DefaultValue(false)]
        public bool Pink;
        [CustomModConfigItem(typeof(LightPurpleColorBooleanElement))]
        [DefaultValue(false)]
        public bool LightPurple;
        [CustomModConfigItem(typeof(LimeColorBooleanElement))]
        [DefaultValue(false)]
        public bool Lime;
        [CustomModConfigItem(typeof(YellowColorBooleanElement))]
        [DefaultValue(false)]
        public bool Yellow;
        [CustomModConfigItem(typeof(CyanColorBooleanElement))]
        [DefaultValue(false)]
        public bool Cyan;
        [CustomModConfigItem(typeof(RedColorBooleanElement))]
        [DefaultValue(false)]
        public bool Red;
        [CustomModConfigItem(typeof(PurpleColorBooleanElement))]
        [DefaultValue(false)]
        public bool Purple;
        [CustomModConfigItem(typeof(QuestColorBooleanElement))]
        [DefaultValue(false)]
        public bool Quest;
        [CustomModConfigItem(typeof(ExpertColorBooleanElement))]
        [DefaultValue(false)]
        public bool Expert;
        [CustomModConfigItem(typeof(MasterColorBooleanElement))]
        [DefaultValue(false)]
        public bool Master;
    }

    internal class ColorableLabelBooleanElement : ConfigElement<bool>
    {
        private Asset<Texture2D> _toggleTexture = null!;

        protected virtual Color LabelColor { get; } = Color.White;

        public override void OnBind()
        {
            base.OnBind();
            _toggleTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Toggle");
            OnLeftClick += (ev, v) => Value = !Value;
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

                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, label, position, MouseTextColor(LabelColor), 0f, Vector2.Zero, baseScale, settingsWidth, 2f);
            }

            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, Value ? Lang.menu[126].Value : Lang.menu[124].Value, new Vector2(dimensions.X + dimensions.Width - 60f, dimensions.Y + 8f), Color.White, 0f, Vector2.Zero, new Vector2(0.8f), -1f, 2f);
            Rectangle sourceRectangle = new(Value ? ((_toggleTexture.Width() - 2) / 2 + 2) : 0, 0, (_toggleTexture.Width() - 2) / 2, _toggleTexture.Height());
            Vector2 drawPosition = new(dimensions.X + dimensions.Width - sourceRectangle.Width - 10f, dimensions.Y + 8f);
            spriteBatch.Draw(_toggleTexture.Value, drawPosition, new Rectangle?(sourceRectangle), Color.White, 0f, Vector2.Zero, Vector2.One, 0, 0f);
        }
    }

    internal class GrayColorBooleanElement : ColorableLabelBooleanElement
    {
        protected override Color LabelColor => new(0x82, 0x82, 0x82);
    }

    internal class WhiteColorBooleanElement : ColorableLabelBooleanElement
    {
        protected override Color LabelColor => new(0xFF, 0xFF, 0xFF);
    }

    internal class BlueColorBooleanElement : ColorableLabelBooleanElement
    {
        protected override Color LabelColor => new(0x96, 0x96, 0xFF);
    }

    internal class GreenColorBooleanElement : ColorableLabelBooleanElement
    {
        protected override Color LabelColor => new(0x96, 0xFF, 0x96);
    }

    internal class OrangeColorBooleanElement : ColorableLabelBooleanElement
    {
        protected override Color LabelColor => new(0xFF, 0xC8, 0x96);
    }

    internal class LightRedColorBooleanElement : ColorableLabelBooleanElement
    {
        protected override Color LabelColor => new(0xFF, 0x96, 0x96);
    }

    internal class PinkColorBooleanElement : ColorableLabelBooleanElement
    {
        protected override Color LabelColor => new(0xFF, 0x96, 0xFF);
    }

    internal class LightPurpleColorBooleanElement : ColorableLabelBooleanElement
    {
        protected override Color LabelColor => new(0xD2, 0xA0, 0xFF);
    }

    internal class LimeColorBooleanElement : ColorableLabelBooleanElement
    {
        protected override Color LabelColor => new(0x96, 0xFF, 0x0A);
    }

    internal class YellowColorBooleanElement : ColorableLabelBooleanElement
    {
        protected override Color LabelColor => new(0xFF, 0xFF, 0x0A);
    }

    internal class CyanColorBooleanElement : ColorableLabelBooleanElement
    {
        protected override Color LabelColor => new(0x05, 0xC8, 0xFF);
    }

    internal class RedColorBooleanElement : ColorableLabelBooleanElement
    {
        protected override Color LabelColor => new(0xFF, 0x28, 0x64);
    }

    internal class PurpleColorBooleanElement : ColorableLabelBooleanElement
    {
        protected override Color LabelColor => new(0xB4, 0x28, 0xFF);
    }

    internal class QuestColorBooleanElement : ColorableLabelBooleanElement
    {
        protected override Color LabelColor => new(0xFF, 0xAF, 0x00);
    }

    internal class ExpertColorBooleanElement : ColorableLabelBooleanElement
    {
        protected override Color LabelColor => Main.DiscoColor;
    }

    internal class MasterColorBooleanElement : ColorableLabelBooleanElement
    {
        protected override Color LabelColor => new(255, (byte)(Main.masterColor * 200f), 0);
    }
}
