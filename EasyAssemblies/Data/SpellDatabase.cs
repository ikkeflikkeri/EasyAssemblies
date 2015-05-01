using System.Collections.Generic;
using LeagueSharp;

namespace EasyAssemblies.Data
{
    public class SpellDatabaseItem
    {
        public string ChampionName { get; set; }
        public SpellSlot Slot { get; set; }
    }

    public static class SpellDatabase
    {
        public static List<SpellDatabaseItem> DangerousSpells = new List<SpellDatabaseItem>();

        static SpellDatabase()
        {
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Aatrox", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Aatrox", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Ahri", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Alistar", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Alistar", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Amumu", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Amumu", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Anivia", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Anivia", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Annie", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Annie", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Annie", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Ashe", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Ashe", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Azir", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Azir", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Blitzcrank", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Blitzcrank", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Blitzcrank", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Brand", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Braum", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Braum", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Caitlyn", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Cassiopeia", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Cassiopeia", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Chogath", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Chogath", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Darius", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Diana", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "DrMundo", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Draven", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Elise", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Evelynn", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Fizz", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Fizz", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "FiddleSticks", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "FiddleSticks", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Galio", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Galio", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Garen", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Gnar", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Gnar", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Gnar", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Gragas", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Gragas", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Gragas", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Heimerdinger", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Hecarim", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Hecarim", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Janna", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Janna", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Jax", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "JarvanIV", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Jayce", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Jinx", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Jinx", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Karma", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Karma", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Kassadin", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Khazix", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Kayle", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "KogMaw", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Leblanc", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "LeeSin", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Leona", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Leona", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Leona", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Lissandra", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Lissandra", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Lulu", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Lux", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Lux", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Lux", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Malphite", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Malphite", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Malzahar", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Malzahar", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Maokai", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Maokai", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Morgana", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Mordekaiser", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Wukong", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Nami", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Nami", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Nasus", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Karthus", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Nautilus", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Nautilus", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Nautilus", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Nidalee", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Olaf", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Orianna", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Orianna", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Quinn", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Rammus", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Rengar", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Renekton", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Riven", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Rumble", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Rumble", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Ryze", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Sejuani", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Sejuani", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Singed", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Singed", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Nocturne", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Shen", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Soraka", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Shyvana", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Skarner", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Skarner", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Pantheon", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Poppy", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Nunu", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Nunu", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Sona", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Swain", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Syndra", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Talon", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Taric", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Thresh", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Thresh", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Thresh", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Tristana", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Trundle", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Trundle", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Tryndamere", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Twitch", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Urgot", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Varus", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Varus", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Veigar", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Velkoz", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Velkoz", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Vi", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Vi", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Viktor", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Viktor", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Vayne", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Warwick", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Xerath", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Xerath", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Xerath", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "XinZhao", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "XinZhao", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "XinZhao", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Yasuo", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Yasuo", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Yoric", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Zac", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Zac", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Zed", Slot = SpellSlot.R });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Ziggs", Slot = SpellSlot.W });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Ziggs", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Zyra", Slot = SpellSlot.Q });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Zyra", Slot = SpellSlot.E });
            DangerousSpells.Add(new SpellDatabaseItem { ChampionName = "Zyra", Slot = SpellSlot.R });
        }
    }
}
