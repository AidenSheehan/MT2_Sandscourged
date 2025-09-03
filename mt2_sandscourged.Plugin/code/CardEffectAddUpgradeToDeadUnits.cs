using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mono.Cecil.Cil;

namespace mt2_sandscourged.Plugin
{
	[SearchWindowPath("Card Upgrade")]
	public sealed class CardEffectAddUpgradeToDeadUnits : CardEffectBase
	{
		public static List<CardState>? consumedCards;

		public override bool CanPlayAfterBossDead
		{
			get
			{
				return true;
			}
		}
		public override bool CanApplyInPreviewMode
		{
			get
			{
				return false;
			}
		}

		public override PropDescriptions CreateEditorInspectorDescriptions()
		{
			PropDescriptions propDescriptions = [];
			string fieldName = CardEffectFieldNames.ParamCardUpgradeData.GetFieldName();
			propDescriptions[fieldName] = new PropDescription("Card Upgrade", "", null, false);
			string fieldName2 = CardEffectFieldNames.ParamInt.GetFieldName();
			propDescriptions[fieldName2] = new PropDescription("Upgrade Lifetime", CardUpgradeHelper.UpgradeLifetimeEditorTooltip, typeof(UnitUpgradeLifetime), false);
			string fieldName3 = CardEffectFieldNames.UseStatusEffectStackMultiplier.GetFieldName();
			propDescriptions[fieldName3] = new PropDescription("Use Status Effect Stack Multiplier", "", typeof(bool), false);
			string fieldName4 = CardEffectFieldNames.ParamStatusEffects.GetFieldName();
			propDescriptions[fieldName4] = new PropDescription("Status Effects to multiply by", "", null, false);
			return propDescriptions;
		}


		public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams, ICoreGameManagers coreGameManagers, ISystemManagers sysManagers)
		{
			// This implementation will only work with Celebrate, and ONLY if the AddUpgradeToDeadUnitsPatch is applied.
			if (consumedCards.IsNullOrEmpty())
			{
				Console.Write("No consumed cards found, skipping upgrade application.");
				yield break;
			}
			else
			{
				var deadUnits = consumedCards.Where(c => c.IsSpawnerCard());
				Console.Write(deadUnits.Count() + " dead units found for upgrade application.");
				bool flag = false;
				bool isTemporary = cardEffectState.GetParamInt() != (int)UnitUpgradeLifetime.Permanent;
				int mult = 1;
				if (cardEffectState.GetUseStatusEffectStackMultiplier())
				{
					if (cardEffectParams.selfTarget == null)
					{
						yield break;
					}

					// Apply status effect stack multiplier to the upgrade data
					string? statusId = cardEffectState.GetParamStatusEffectStackData().FirstOrDefault()?.statusId;
					if (statusId == null)
					{
						yield break;
					}
					mult = cardEffectParams.selfTarget.GetStatusEffectStacks(statusId);
				}

				foreach (CardState cardState in deadUnits)
				{
					for (int i = 0; i < mult; i++)
					{
						Console.Write(cardState.GetDebugName() + " is attempting upgrade.");
						if (CardUpgradeHelper.TryApplyUpgradeToCard(cardState, cardEffectState.GetParamCardUpgradeData(), isTemporary, cardEffectParams.isFromHiddenTrigger, coreGameManagers, out CardUpgradeState cardUpgradeState))
						{
							Console.Write(cardState.GetDebugName() + $" {i}: Success!");
							cardState.UpdateCardBodyText(null);
							flag = true;
						}
					}
				}

				if (flag)
				{
					base.ShowPyreNotification(cardEffectState, coreGameManagers.GetSaveManager(), sysManagers.GetPopupNotificationManager(), null);
				}
			}
			consumedCards = null; // Clear the consumed cards after processing
			yield break;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x000161A2 File Offset: 0x000143A2
		public override void GetTooltipsStatusList(CardEffectState cardEffectState, ref List<string> outStatusIdList)
		{
			CardEffectAddTempCardUpgradeToCardsInHand.GetTooltipsStatusList(cardEffectState.GetSourceCardEffectData(), ref outStatusIdList);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x000161B0 File Offset: 0x000143B0
		public static void GetTooltipsStatusList(CardEffectData cardEffectData, ref List<string> outStatusIdList)
		{
			foreach (StatusEffectStackData statusEffectStackData in cardEffectData.GetParamCardUpgradeData().GetStatusEffectUpgrades())
			{
				outStatusIdList.Add(statusEffectStackData.statusId);
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00016210 File Offset: 0x00014410
		public override void CreateAdditionalTooltips(CardEffectState cardEffectState, TooltipContainer tooltipContainer, SaveManager saveManager)
		{
			CardUpgradeState cardUpgradeState = new CardUpgradeState();
			cardUpgradeState.Setup(cardEffectState.GetParamCardUpgradeData(), false, false);
			tooltipContainer.AddTooltipsCardUpgrade(CardState.None, cardUpgradeState, saveManager);
			tooltipContainer.AddTooltipsUpgradedCardTraits(cardUpgradeState);
		}
	}
}