using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFisher.Content.InfoDisplays
{
    public class CatchesRecorderInfoDisplay : InfoDisplay
    {
        public override bool Active()
        {
            return BobberManager.IsFishing;
        }

        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            ItemDefinition item = CatchesRecorder.GetLocalPlayerLastCatchedItem(out int count);

            if (count <= 0) return string.Empty;

            return $"{new Item(item.Type).Name}: {count}";
        }
    }
}
