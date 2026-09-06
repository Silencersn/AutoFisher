using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFisher.Common.Configs.ClientConfigs;

public class AutoFisher_AutoOpenFilter_ClientConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [DefaultValue(false)]
    public bool EnableAutoOpenFilter;
    [DefaultValue(false)]
    public bool TurnBlockListToAllowList;
    public List<ItemDefinition> BlockList = [];
}
