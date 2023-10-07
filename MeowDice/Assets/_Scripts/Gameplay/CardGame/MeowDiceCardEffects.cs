using System;
using System.Collections.Generic;
using Engine.SettingModule;

namespace MeowDice.GamePlay
{
    public abstract class MeowDiceCardEffects
    {
        
        public static readonly Dictionary<int, Action<Player, Cat, int, int, Dictionary<string, object>>> funcDict =
            new Dictionary<int, Action<Player, Cat, int, int, Dictionary<string, object>>>()
            {
                { 1, CardEffect1 }, { 2, CardEffect2}, {5, CardEffect5}, {6, CardEffect6}, {7, CardEffect7}, {8, CardEffect8}, {18, CardEffect18}
            };


        public static void CardEffect1(Player player, Cat cat, int para1, int para2, Dictionary<string, object> context)
        {
            var change = 0;
            if (context.TryGetValue("sanChange", out object value))
            {
                change = (int)value;
            }

            context["sanChange"] = change + para1;
        }
        
        public static void CardEffect2(Player player, Cat cat, int para1, int para2, Dictionary<string, object> context)
        {

            var change = 0;
            if (context.TryGetValue("alterChange", out object value))
            {
                change = (int)value;
            }

            context["alterChange"] = change + para1;
        }
        
        public static void CardEffect5(Player player, Cat cat, int para1, int para2, Dictionary<string, object> context)
        {
            var change = 0;
            if (context.TryGetValue("comfortChange", out object value))
            {
                change = (int)value;
            }

            context["comfortChange"] = change + para1;
        }

        
        public static void CardEffect6(Player player, Cat cat, int para1, int para2, Dictionary<string, object> context)
        {
            int count = 0;
            foreach (var card in player.selectedCards)
            {
                if (card.cardId == (uint)para1)
                {
                    count ++;
                }
            }

            context["sanChangeDec"] = count * para2;
        }
        
        public static void CardEffect7(Player player, Cat cat, int para1, int para2, Dictionary<string, object> context)
        {
            int count = 0;
            foreach (var card in player.selectedCards)
            {
                if (card.cardId == (uint)para1)
                {
                    count ++;
                }
            }

            context["comfortChangeDec"] = count * para2;
        }

        public static void CardEffect8(Player player, Cat cat, int para1, int para2, Dictionary<string, object> context)
        {
            int dice = 0;
            if(context.TryGetValue("dice", out var value))
                dice = (int)value;
            if (para2 == 1)
            {
                cat.angryStateCount += para1 * dice;
            }
            else
            {
                cat.memoryStateCount += para1 * dice;
            }
        }

        public static void CardEffect18(Player player, Cat cat, int para1, int para2,
            Dictionary<string, object> context)
        {
            if (context.TryGetValue("card", out var obj))
            {
                if (obj is MeowDiceCard card)
                {
                    card.canUnselect = false;
                }
            }
        }
    }
}