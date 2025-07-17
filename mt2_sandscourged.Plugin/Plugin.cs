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
using TrainworksReloaded.Base.Extensions;

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
                        // Class stuff
                        "json/class.json",
                        // Champions
                        "json/champions/champion_Pharaoh.json",
                        // Units
                        "json/units/unit_OverseerEternal.json",
                        "json/units/unit_ReveredScarab.json",
                        "json/units/unit_GildedScarab.json",
                        "json/units/unit_JewelledScarab.json",
                        "json/units/unit_ScarabSwarm.json",
                        "json/units/unit_Wretch.json",
                        "json/units/unit_ScourgedHollow.json",
                        "json/units/unit_CartoucheScribe.json",
                        "json/units/unit_SphinxHierarch.json",
                        "json/units/unit_InfestedMummy.json",
                        "json/units/unit_ForgottenHulk.json",
                        "json/units/unit_PriestOfLostThoughts.json",
                        "json/units/unit_EssenceRevoker.json",
                        "json/units/unit_EgoUltimus.json",

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
                        "json/spells/card_Jinx.json",
                        "json/spells/card_Malediction.json",
                        "json/spells/card_PlagueOfScarabs.json",
                        "json/spells/card_SpikeOfTheScourged.json",
                        "json/spells/card_LastWords.json",
                        "json/spells/card_Cripple.json",
                        "json/spells/card_Ruination.json",
                        "json/spells/card_MemorySyphon.json",
                        "json/spells/card_GravenRite.json",
                        "json/spells/card_Relinquish.json",

                        // Equipment
                        "json/equipments/equipment_Needle.json",
                        "json/equipments/equipment_PearlTap.json",
                        "json/equipments/equipment_SpearOfTheDeserted.json",
                        // Rooms
                        // Scourges and Blights
                        "json/scourges/card_Vexation.json"
                    //Debug
                    );
                }
            );

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            Railend.ConfigurePostAction(
  c =>
  {
      var cardPoolManager = c.GetInstance<IRegister<CardPool>>();
      var id = MyPluginInfo.PLUGIN_GUID.GetId(TemplateConstants.CardPool, "TormentPool");
      CardTraitScalingAddBattleCardToHandFromPool.paramCardPool = cardPoolManager.GetValueOrDefault(id);
  }
);

            // Uncomment if you need harmony patches, if you are writing your own custom effects.
            //var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            //harmony.PatchAll();
        }
    }
}
