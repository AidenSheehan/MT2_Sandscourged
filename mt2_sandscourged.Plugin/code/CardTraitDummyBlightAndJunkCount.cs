using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace mt2_sandscourged.Plugin
{
	public sealed class CardTraitDummyBlightAndJunkCount : CardTraitState
	{
		// This literally only exists for the reminder text on Plague of Distortion
		public override PropDescriptions CreateEditorInspectorDescriptions()
		{
			return [];
		}

		public override string GetCurrentEffectText(CardStatistics? cardStatistics, SaveManager? saveManager, RelicManager? relicManager)
		{
			if (!(cardStatistics != null) || !cardStatistics.GetStatValueShouldDisplayOnCardNow(base.StatValueData))
			{
				return string.Empty;
			}
			CardStatistics.StatValueData blightStats = new()
			{
				cardState = GetCard(),
				trackedValue = CardStatistics.TrackedValueType.TypeInDeck,
				cardTypeTarget = CardStatistics.CardTypeTarget.Blight,
				forPreviewText = false
			};
			int numBlights = cardStatistics.GetStatValue(blightStats);
			CardStatistics.StatValueData junkStats = new()
			{
				cardState = GetCard(),
				trackedValue = CardStatistics.TrackedValueType.TypeInDeck,
				cardTypeTarget = CardStatistics.CardTypeTarget.Junk,
				forPreviewText = false
			};
			int numJunks = cardStatistics.GetStatValue(junkStats);
			return string.Format("CardTraitScalingAddStatusEffect_CurrentScaling_CardText".Localize(null), (numBlights + numJunks) * GetParamInt());
		}

	}
}
