using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFisher.Content.InfoDisplays
{
    public abstract class AFishingInfoDisplay : InfoDisplay
    {
        public override bool Active()
        {
            return BobberManager.IsFishing;
        }
    }
}
