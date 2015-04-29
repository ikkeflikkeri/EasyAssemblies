using System.Drawing;
using LeagueSharp;
using LeagueSharp.Common;

namespace EasyAssemblies.Services
{
    class DrawingService
    {
        private readonly static Color GreenColor = Color.FromArgb(100, 0, 255, 0);
        private readonly static Color RedColor = Color.FromArgb(100, 255, 0, 0);

        public static void RenderSkillshotRange(Spell spell, bool minimap = false)
        {
            if(minimap)
                Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, (spell.IsReady() ? GreenColor : RedColor), 2, 30, true);
            else
                Render.Circle.DrawCircle(ObjectManager.Player.Position, spell.Range, (spell.IsReady() ? GreenColor : RedColor));
        }

        public static void RenderDamageIndicator(bool enabled)
        {
            Utility.HpBarDamageIndicator.Enabled = enabled;
        }

        public static void SetDamageIndicator(Utility.HpBarDamageIndicator.DamageToUnitDelegate function)
        {
            Utility.HpBarDamageIndicator.DamageToUnit = function;
        }
    }
}
