using System;
using System.Collections.Generic;
using Engine.SettingModule;

namespace MeowDice.GamePlay
{
    public abstract class MeowDiceCardEffects
    {
        
        public static readonly Dictionary<uint, Func<Player, Cat, (int, int)>> funcDict =
            new Dictionary<uint, Func<Player, Cat, (int, int)>>()
            {
                { 1, CardEffect1 },
                { 2, CardEffect2},
            };

        public static (int, int) CardEffect1(Player player, Cat cat)
        {
            int alterChange = 0;
            int sanChange = 0;
            var table = TableModule.Get("CardEffect");
            sanChange = (int)table.GetData(0, "Para1");

            return (alterChange, sanChange);
        }

        public static (int, int) CardEffect2(Player player, Cat cat)
        {
            int alterChange = 0;
            int sanChange = 0;
            var table = TableModule.Get("CardEffect");
            alterChange = (int)table.GetData(0, "Para1");

            return (alterChange, sanChange);
        }
        
    }
}