using AutoFisher.Common.Systems;
using Microsoft.Xna.Framework.Input;

namespace AutoFisher
{
    public partial class AutoFisher
    {
        // TODO: 自动合成
        // TODO: 自动放入箱子
        // TODO: 自动提炼鱼饵

        // TODO: fishing anying 兼容
        // TODO: 输出钓怪物信息            OK
        // TODO: 仅钓起怪物（不钓物品）
        // TODO: 统计卖出的钱币、怪物
        // TODO: 多人模式测试
        // TODO: 幸运仅受正面影响           OK
        // TODO: 卖出价格根据NPC快乐
        // TODO: 多人模式下自动击杀无音效 bug
        // TODO: 声纳药水 先 鱼线断裂 后 被过滤 bug

        public class DebugSystem : ModSystem
        {
            private static bool unlock = false;
            private static int cd = 0;
            private static int count = 0;

            public static bool JustPressed(Keys key)
            {
                if (Environment.UserName is not "silen") return false;
                return Main.keyState.IsKeyDown(key) && !Main.oldKeyState.IsKeyDown(key);
            }

            public override void PostUpdateWorld()
            {
                if (!unlock)
                {
                    if (JustPressed(Keys.D1))
                    {
                        cd = 30;
                        count++;
                        if (count >= 5)
                        {
                            unlock = true;
                            Main.NewText("Debug Mode is Enabled");
                        }
                    }
                    else if (cd > 0)
                    {
                        cd--;
                    }
                    else
                    {
                        count = 0;
                    }
                    return;
                }
                if (JustPressed(Keys.D0))
                {
                    Main.NewText("Debug Test");
                }
                if (JustPressed(Keys.D1))
                {
                    Main.NewText($"item: ({ILCodeLoader.counterItem}/{ILCodeLoader.counterItemAll})");
                    Main.NewText($"npc: ({ILCodeLoader.counterNPC}/{ILCodeLoader.counterNPCAll})");
                }
                if (JustPressed(Keys.D2))
                {
                    /*var item = new Item(ItemID.CopperAxe, 2);
                    Main.LocalPlayer.SellItem(item, 2);
                    Main.NewText(item.stack);*/
                }

            }
        }
    }
}
