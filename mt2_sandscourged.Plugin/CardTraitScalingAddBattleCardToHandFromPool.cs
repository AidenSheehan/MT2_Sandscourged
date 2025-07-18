using System.Collections;
using ShinyShoe;

public sealed class CardTraitScalingAddBattleCardToHandFromPool : CardTraitState
{
    public static CardPool? paramCardPool;

    public override PropDescriptions CreateEditorInspectorDescriptions()
    {
        return [];
    }

    public override IEnumerator OnCardDiscarded(CardManager.DiscardCardParams discardCardParams, ICoreGameManagers coreGameManagers)
    {
        if (!discardCardParams.wasPlayed || discardCardParams.discardCard == null)
        {
            yield break;
        }

        int maxCount = GetAdditionalCards(coreGameManagers.GetCardStatistics());
        CardManager cardManager = coreGameManagers.GetCardManager();
        SaveManager saveManager = coreGameManagers.GetSaveManager();

        int count = maxCount;
        int maxAdd = cardManager.GetMaxHandSize() - cardManager.GetNumCardsInHand();
        if (maxAdd > 0)
        {
            count = UnityEngine.Mathf.Min(count, maxAdd);
        }
        CardManager.AddCardUpgradingInfo? addCardUpgradingInfo = null;
        var cardUpgradeData = GetCardUpgradeDataParam();
        if (cardUpgradeData != null)
        {
            addCardUpgradingInfo = new();
            addCardUpgradingInfo.upgradeDatas.Add(cardUpgradeData);
            addCardUpgradingInfo.tempCardUpgrade = true;
            addCardUpgradingInfo.upgradingCardSource = GetCard();
            addCardUpgradingInfo.ignoreTempUpgrades = false;
            addCardUpgradingInfo.copyModifiersFromCard = GetCard();
        }
        using (GenericPools.GetList(out List<CardData> toProcessCards))
        {
            for (int i = 0; i < paramCardPool?.GetNumCards(); i++)
            {
                CardData cardAtIndex = paramCardPool.GetCardAtIndex(i);
                toProcessCards.Add(cardAtIndex);
            }

            for (int i = 0; i < count; i++)
            {
                int index = RandomManager.Range(0, toProcessCards.Count, RngId.Battle);
                CardData cardData = toProcessCards[index];
                cardManager.AddCard(cardData, CardPile.HandPile, i, count, fromRelic: false, permanent: false, addCardUpgradingInfo);
            }

        }

        yield break;
    }

    private int GetAdditionalCards(CardStatistics cardStatistics)
    {
        CardStatistics.StatValueData statValueData = new()
        {
            cardState = GetCard(),
            trackedValue = GetParamTrackedValue(),
            entryDuration = GetParamEntryDuration(),
            cardTypeTarget = GetParamCardType(),
            paramSubtype = GetParamSubtype(),
            forPreviewText = false
        };
        int statValue = cardStatistics.GetStatValue(statValueData);
        return GetParamInt() * statValue;
    }
}