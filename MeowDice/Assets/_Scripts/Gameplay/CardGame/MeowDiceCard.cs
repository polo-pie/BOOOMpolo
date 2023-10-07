using System.Collections;
using System.Collections.Generic;
using Engine.SettingModule;
using Unity.Mathematics;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

namespace MeowDice.GamePlay
{
    public enum CardType
    {
        
    }
    
    /// <summary>
    /// 卡牌数据
    /// </summary>
    public struct MeowDiceCardData
    {
        public string Name;
        public List<int> CardEffects;
        public int DiceCost;
        public string Description;
    }

    public class MeowDiceCard
    {
        public MeowDiceCardData cardData;
        public readonly uint cardUid;
        public readonly uint cardId;
        public bool canUnselect;

        public MeowDiceCard(uint cardId, uint cardUid)
        {
            this.cardId = cardId;
            this.cardUid = cardUid;

            var table = TableModule.Get("Card");
            cardData = new MeowDiceCardData()
            {
                Name=table.GetData(cardId, "Name").ToString(),
                CardEffects=table.GetData(cardId, "Effect") as List<int>,
                DiceCost=(int)table.GetData(cardId, "DiceCost"),
                Description = table.GetData(cardId, "CardDes").ToString()
            };
            canUnselect = true;
        }

        public void DoEffect(int stage, out int alterChange, out int sanChange, out int comfortChange)
        {
            var table = TableModule.Get("CardEffect");
            sanChange = 0;
            alterChange = 0;
            comfortChange = 0;
            var context = new Dictionary<string, object>()
            {
                { "card", this }, { "dice", MeowDiceCardGame.Instance.CardManager.RandomSelectCardIndex + 1 }, {"sanChangeDec", 0}, {"comfortChangeDec", 0},
                {"sanChange", 0}, {"alterChange", 0}, {"comfortChange", 0}
            };
            foreach (uint effetId in cardData.CardEffects)
            {
                var typeId = (int)table.GetData(effetId, "TypeID");
                var para1 = (int)table.GetData(effetId, "Para1");
                var para2 = (int)table.GetData(effetId, "Para2");
                var effectTime = (int)table.GetData(effetId, "EffectTime");
                if (effectTime == stage)
                {
                    if (MeowDiceCardEffects.funcDict.TryGetValue(typeId, out var func))
                    {
                        func.Invoke(MeowDiceCardGame.Instance.Player, MeowDiceCardGame.Instance.Cat, para1, para2, context);
                    }
                }
            }

            var sanChangeFlag = (int)context["sanChange"] < 0 ? -1 : 1;
            sanChange = sanChangeFlag * math.max(0, math.abs((int)context["sanChange"]) - (int)context["sanChangeDec"]);
            alterChange = (int)context["alterChange"];
            var comfortChangeFlag = (int)context["comfortChange"] < 0 ? -1 : 1;
            comfortChange = comfortChangeFlag *
                            math.max(0, math.abs((int)context["comfortChange"]) - (int)context["comfortChangeDec"]);

        }
        
    }
}

