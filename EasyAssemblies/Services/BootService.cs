using EasyAssemblies.Champions;
using LeagueSharp;
using LeagueSharp.Common;

namespace EasyAssemblies.Services
{
    static class BootService
    {
        public static void Initialize()
        {
            CustomEvents.Game.OnGameLoad += Loading;
        }

        private static void Loading(System.EventArgs args)
        {
            Champion champion = null;

            switch (ObjectManager.Player.ChampionName)
            {
                case "Jinx": champion = new Jinx(); break;
                case "Xerath": champion = new Xerath(); break;
                case "Morgana": champion = new Morgana(); break;
                case "KogMaw": champion = new KogMaw(); break;
            }

            if (champion != null)
                champion.Start();
        }
    }
}
