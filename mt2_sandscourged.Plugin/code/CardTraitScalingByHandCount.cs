using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace mt2_sandscourged.Plugin
{
    public sealed class CardTraitScalingByHandCount : CardTraitState
    {
        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return new PropDescriptions
            {
                [CardTraitFieldNames.ParamInt.GetFieldName()] = new PropDescription("Multiplier per card in hand"),
                [CardTraitFieldNames.ParamInt2.GetFieldName()] = new PropDescription("Divider per card in hand"),
            };
        }

        // For Reverence upgrade
        public override void OnApplyingCardUpgradeToUnit(CardState thisCard, CharacterState targetUnit, CardUpgradeState upgradeState, ICoreGameManagers coreGameManagers)
        {
            base.OnApplyingCardUpgradeToUnit(thisCard, targetUnit, upgradeState, coreGameManagers);
            var cardManager = coreGameManagers.GetCardManager();
            int numCardsInHand = cardManager.GetHand().Count(c => c != thisCard);

            upgradeState.SetAttackDamage(GetParamInt() * numCardsInHand);
            upgradeState.SetAdditionalHP(GetParamInt2() * numCardsInHand);
        }

        // For Vizier: Curator
        public override int OnApplyingDamage(ApplyingDamageParameters damageParams, ICoreGameManagers coreGameManagers)
        {
            var cardManager = coreGameManagers.GetCardManager();
            int numCardsInHand = cardManager.GetHand().Count();
            return numCardsInHand * GetParamInt() / GetParamInt2();
        }

        // For Priest Of Lost Thoughts
        public override int OnStatusEffectApplied(CharacterState affectedCharacter, CardState thisCard, ICoreGameManagers coreGameManagers, string statusId, int sourceStacks = 0)
        {
            var cardManager = coreGameManagers.GetCardManager();
            int numCardsInHand = cardManager.GetHand().Count(c => c != thisCard);
            // This is not a great way to do this, but it works for now.
            return sourceStacks * (numCardsInHand * GetParamInt() / GetParamInt2()) - sourceStacks;
        }

    }
}
