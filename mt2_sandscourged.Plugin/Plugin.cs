using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using I2.Loc;
using Microsoft.Extensions.Configuration;
using ShinyShoe.Logging;
using SimpleInjector;
using TrainworksReloaded.Base;
using TrainworksReloaded.Base.Card;
using TrainworksReloaded.Base.CardUpgrade;
using TrainworksReloaded.Base.Character;
using TrainworksReloaded.Base.Class;
using TrainworksReloaded.Base.Effect;
using TrainworksReloaded.Base.Localization;
using TrainworksReloaded.Base.Prefab;
using TrainworksReloaded.Base.Trait;
using TrainworksReloaded.Base.Trigger;
using TrainworksReloaded.Core;
using TrainworksReloaded.Core.Impl;
using TrainworksReloaded.Core.Interfaces;
using TrainworksReloaded.Core.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace mt2_sandscourged.Plugin
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger = new(MyPluginInfo.PLUGIN_GUID);
        public void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;

            var builder = Railhead.GetBuilder();
            builder.Configure(
                MyPluginInfo.PLUGIN_GUID,
                c =>
                {
                    // Be sure to include all of your json files if you add more.
                    // Be sure to update the project configuration if you include more folders
                    //   the project only copies json files in the json folder and not in subdirectories.
                    c.AddMergedJsonFile(
                        "json/plugin.json",
                        "json/global.json",
                        // Champions

                        // Units
                        "json/units/unit_OverseerEternal.json",
                        "json/units/unit_Scarab.json",
                        "json/units/unit_GildedScarab.json",
                        "json/units/unit_JewelledScarab.json",
                        "json/units/unit_ScarabSwarm.json",
                        "json/units/unit_Wretch.json",
                        "json/units/unit_CartoucheScribe.json",

                        // Spells
                        "json/spells/card_Pitfall.json",
                        "json/spells/card_OblivionSands.json",
                        "json/spells/card_Sandstorm.json",
                        "json/spells/card_Reverence.json",
                        "json/spells/card_InMemoriam.json",
                        "json/spells/card_TormentA.json",
                        "json/spells/card_TormentB.json",
                        "json/spells/card_TormentC.json",
                        "json/spells/card_TormentD.json",
                        "json/spells/card_TormentE.json",
                        "json/spells/card_Dessicate.json",
                        "json/spells/card_Efface.json",
                        "json/spells/card_Hex.json",
                        // Scourges and Blights
                        "json/scourges/card_Vexation.json",
                        //Debug
                        "json/spells/card_TestTorment.json"
                    );
                }
            );

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            
            // Uncomment if you need harmony patches, if you are writing your own custom effects.
            //var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            //harmony.PatchAll();
        }
    }
}
