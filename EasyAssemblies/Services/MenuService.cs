using System.Collections.Generic;
using System.Drawing;
using LeagueSharp;
using LeagueSharp.Common;

namespace EasyAssemblies.Services
{
    static class MenuService
    {
        private static MenuWrapper _menu;
        private static MenuWrapper.SubMenu _currentSubMenu;

        public static Dictionary<string, MenuWrapper.BoolLink> BoolLinks { get; private set; }
        public static Dictionary<string, MenuWrapper.CircleLink> CircleLinks { get; private set; }
        public static Dictionary<string, MenuWrapper.KeyBindLink> KeyLinks { get; private set; }
        public static Dictionary<string, MenuWrapper.SliderLink> SliderLinks { get; private set; }
        public static Dictionary<string, MenuWrapper.StringListLink> StringListLinks { get; private set; }

        public static void Begin()
        {
            BoolLinks = new Dictionary<string, MenuWrapper.BoolLink>();
            CircleLinks = new Dictionary<string, MenuWrapper.CircleLink>();
            KeyLinks = new Dictionary<string, MenuWrapper.KeyBindLink>();
            SliderLinks = new Dictionary<string, MenuWrapper.SliderLink>();
            StringListLinks = new Dictionary<string, MenuWrapper.StringListLink>();

            _menu = new MenuWrapper("Easy" + ObjectManager.Player.ChampionName);
        }

        public static void AddSubMenu(string title)
        {
            _currentSubMenu = _menu.MainMenu.AddSubMenu(title);
        }

        public static void AddBool(string key, string title, bool value)
        {
            BoolLinks.Add(key, _currentSubMenu.AddLinkedBool(title, value));
        }

        public static void AddCircle(string key, string title, bool enabled, Color color, float radius = 100f)
        {
            CircleLinks.Add(key, _currentSubMenu.AddLinkedCircle(title, enabled, color, radius));
        }

        public static void AddKeyBind(string key, string title, uint keyCode, KeyBindType type)
        {
            KeyLinks.Add(key, _currentSubMenu.AddLinkedKeyBind(title, keyCode, type));
        }

        public static void AddSlider(string key, string title, int value, int minValue, int maxValue)
        {
            SliderLinks.Add(key, _currentSubMenu.AddLinkedSlider(title, value, minValue, maxValue));
        }

        public static void AddStringList(string key, string title, string[] list, int defaultIndex)
        {
            StringListLinks.Add(key, _currentSubMenu.AddLinkedStringList(title, list, defaultIndex));
        }

        public static void End()
        {
            BoolLinks.Add("packets", _menu.MainMenu.AddLinkedBool("Use packets", false));
        }

        public static Orbwalking.Orbwalker Orbwalker
        {
            get { return _menu.Orbwalker; }
        }
    }
}
