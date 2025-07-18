using System;
using System.Runtime.CompilerServices;

namespace mt2_sandscourged.Plugin
{
    public class CardTraitDiscardHandCount : CardTraitState
    {
        public int NumDiscardableCards { get; private set; }
        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return new PropDescriptions
            {
                [CardTraitFieldNames.ParamInt.GetFieldName()] = new PropDescription("Attack Damage per discarded card"),
                [CardTraitFieldNames.ParamInt2.GetFieldName()] = new PropDescription("Health per discarded card")
            };
        }

        public override System.Collections.IEnumerator OnPreCardPlayed(CardState thisCard, ICoreGameManagers coreGameManagers)
        {
            var cardManager = coreGameManagers.GetCardManager();
            var cardsInHand = cardManager.GetHand();
            NumDiscardableCards = cardsInHand.Count(c => c.CanBeDiscarded() && c != thisCard);
            yield break;
        }


        // For Reverence upgrade
        public override void OnApplyingCardUpgradeToUnit(CardState thisCard, CharacterState targetUnit, CardUpgradeState upgradeState, ICoreGameManagers coreGameManagers)
        {
            base.OnApplyingCardUpgradeToUnit(thisCard, targetUnit, upgradeState, coreGameManagers);

            upgradeState.SetAttackDamage(GetParamInt() * NumDiscardableCards);
            upgradeState.SetAdditionalHP(GetParamInt2() * NumDiscardableCards);
        }

        // For Priest Of Lost Thoughts
        public override int OnStatusEffectApplied(CharacterState affectedCharacter, CardState thisCard, ICoreGameManagers coreGameManagers, string statusId, int sourceStacks = 0)
        {
            // This is not a great way to do this, but it works for now.
            return sourceStacks * (NumDiscardableCards/2) - sourceStacks;
        }
    }
}
