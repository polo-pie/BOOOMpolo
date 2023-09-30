using System.Collections;
using System.Collections.Generic;
using Engine.SettingModule;
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
        public List<int> CardEffets;
        public int DiceCost;
    }

    public class MeowDiceCard
    {
        public MeowDiceCardData cardData;
        public readonly uint cardUid;
        public readonly uint cardId;

        public MeowDiceCard(uint cardId, uint cardUid)
        {
            this.cardId = cardId;
            this.cardUid = cardUid;

            var table = TableModule.Get("Card");
            cardData = new MeowDiceCardData()
            {
                Name=table.GetData(cardId, "Name").ToString(),
                CardEffets=table.GetData(cardId, "Effect") as List<int>,
                DiceCost=(int)table.GetData(cardId, "DiceCost")
            };
        }

        public void DoEffect(out int alterChange, out int sanChange)
        {
            var table = TableModule.Get("CardEffect");
            sanChange = 0;
            alterChange = 0;
            foreach (uint effetId in cardData.CardEffets)
            {
                var typeId = (int)table.GetData(effetId, "TypeID");
                var para1 = (int)table.GetData(effetId, "Para1");
                var para2 = (int)table.GetData(effetId, "Para2");
                if (typeId == 1)
                {
                    sanChange += para1;
                }
                else if (typeId == 2)
                {
                    alterChange += para1;
                }
            }
        }
    }
}

