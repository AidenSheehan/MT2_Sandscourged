using System;
using System.Runtime.CompilerServices;

namespace mt2_sandscourged.Plugin
{
    public class CardTraitReverenceUpgrade : CardTraitState
    {
        public int NumDiscardableCards { get; private set; }
        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            throw new NotImplementedException();
        }

        public override System.Collections.IEnumerator OnPreCardPlayed(CardState thisCard, ICoreGameManagers coreGameManagers)
        {
            var cardManager = coreGameManagers.GetCardManager();
            var cardsInHand = cardManager.GetHand();
            NumDiscardableCards = cardsInHand.Count(c => c.CanBeDiscarded() && c != thisCard);
            yield break;
        }


        public override void OnApplyingCardUpgradeToUnit(CardState thisCard, CharacterState targetUnit, CardUpgradeState upgradeState, ICoreGameManagers coreGameManagers)
        {
            base.OnApplyingCardUpgradeToUnit(thisCard, targetUnit, upgradeState, coreGameManagers);

            upgradeState.SetAttackDamage(GetParamInt() * NumDiscardableCards);
            upgradeState.SetAdditionalHP(GetParamInt2() * NumDiscardableCards);
        }
    }
}
