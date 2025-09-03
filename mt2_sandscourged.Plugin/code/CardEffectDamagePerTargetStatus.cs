using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ShinyShoe;

[SearchWindowPath("Damage")]
public sealed class CardEffectDamagePerTargeStatus : CardEffectDamage
{
	public override PropDescriptions CreateEditorInspectorDescriptions()
	{
		PropDescriptions propDescriptions = [];
		string fieldName = CardEffectFieldNames.ParamInt.GetFieldName();
		propDescriptions[fieldName] = new PropDescription("Damage Multiplier", "", null, false);
		string fieldName2 = CardEffectFieldNames.ParamStatusEffects.GetFieldName();
		propDescriptions[fieldName2] = new PropDescription("Status Effect to damage by", "", null, false);
		string fieldName3 = CardEffectFieldNames.ParamBool.GetFieldName();
		propDescriptions[fieldName3] = new PropDescription("Include Buffs", "", null, false);
		string fieldName4 = CardEffectFieldNames.ParamBool.GetFieldName();
		propDescriptions[fieldName4] = new PropDescription("Include Debuffs", "", null, false);
		return propDescriptions;
	}
	public override bool TestEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams, ICoreGameManagers coreGameManagers)
	{
		return cardEffectParams.targets.Count > 0;
	}

	public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams, ICoreGameManagers coreGameManagers, ISystemManagers sysManagers)
	{
		CombatManager combatManager = coreGameManagers.GetCombatManager();
		int multiplier = cardEffectState.GetParamInt();
		string? statusID = null;
		var statusEffects = cardEffectState.GetParamStatusEffectStackData();
		if (statusEffects.Length > 0)
			statusID = statusEffects.First().statusId;
		bool includePositive = cardEffectState.GetParamBool();
		bool includeNegative = cardEffectState.GetParamBool2();
		int soundGateId = SoundManager.INVALID_SOUND_GATE;
		if (cardEffectState.GetTargetMode() == TargetMode.Room)
		{
			soundGateId = combatManager.IgnoreDuplicateSounds(true);
		}
		int num;
		for (int i = 0; i < cardEffectParams.targets.Count; i = num + 1)
		{
			CharacterState target = cardEffectParams.targets[i];
			int damageAmount = CalculateDamage(multiplier, target, statusID, includePositive, includeNegative);
			yield return combatManager.ApplyDamageToTarget(damageAmount, target, new CombatManager.ApplyDamageToTargetParameters
			{
				playedCard = cardEffectParams.playedCard,
				finalEffectInSequence = cardEffectParams.finalEffectInSequence,
				relicState = cardEffectParams.sourceRelic,
				selfTarget = cardEffectParams.selfTarget,
				appliedVfx = cardEffectState.GetAppliedVFX(),
				appliedVfxId = cardEffectParams.appliedVfxId
			});
			num = i;
		}
		combatManager.ReleaseIgnoreDuplicateCuesHandle(soundGateId);
		yield break;
	}

	private int CalculateDamage(int multiplier, CharacterState target, string? statusID, bool includePositive, bool includeNegative)
	{
		if (statusID != null)
			return multiplier * target.GetStatusEffectStacks(statusID);
		else
		{
			int num = 0;
			using (GenericPools.GetList(out List<CharacterState.StatusEffectStack> list))
			{
				target.GetStatusEffects(ref list);
				foreach (var statusEffect in list)
				{
					if (statusEffect.State.GetDisplayCategory() == StatusEffectData.DisplayCategory.Positive && includePositive)
						num += statusEffect.Count;
					if (statusEffect.State.GetDisplayCategory() == StatusEffectData.DisplayCategory.Negative && includeNegative)
						num += statusEffect.Count;
				}
			}
			return multiplier * num;
		}
	}
}
