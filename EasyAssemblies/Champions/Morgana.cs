using EasyAssemblies.Services;
using LeagueSharp;
using LeagueSharp.Common;

namespace EasyAssemblies.Champions
{
    class Morgana : Champion
    {
        private Spell Q { get; set; }
        private Spell W { get; set; }
        private Spell E { get; set; }
        private Spell R { get; set; }

        protected override void InitializeSpells()
        {
            Q = new Spell(SpellSlot.Q, 1175f);
            W = new Spell(SpellSlot.W, 900f);
            E = new Spell(SpellSlot.E, 750f);
            R = new Spell(SpellSlot.R, 600f);

            Q.SetSkillshot(0.25f, 72f, 1200f, true, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.25f, 175f, 1200f, false, SkillshotType.SkillshotCircle);
        }

        protected override void InitializeMenu()
        {
            MenuService.Begin();

            MenuService.AddSubMenu("Combo");
            MenuService.AddBool("Combo_q", "Use Q", true);

            MenuService.AddSubMenu("Harass");
            MenuService.AddBool("Harass_q", "Use Q", true);

            MenuService.AddSubMenu("Auto");
            MenuService.AddBool("Auto_q", "Use Q", false);
            MenuService.AddBool("Auto_e", "Use E", true);
            HeroManager.Enemies.ForEach(hero => MenuService.AddBool("Auto_e_" + hero.ChampionName, "Use E on " + hero.ChampionName, true));

            MenuService.AddSubMenu("Drawing");
            MenuService.AddBool("Drawing_q", "Q Range", true);
            MenuService.AddBool("Drawing_w", "W Range", true);
            MenuService.AddBool("Drawing_e", "E Range", true);
            MenuService.AddBool("Drawing_r", "R Range", true);

            MenuService.AddSubMenu("Misc");

            MenuService.End();
        }

        protected override void Draw()
        {
            if (MenuService.BoolLinks["Drawing_q"].Value) DrawingService.RenderSkillshotRange(Q);
            if (MenuService.BoolLinks["Drawing_w"].Value) DrawingService.RenderSkillshotRange(W);
            if (MenuService.BoolLinks["Drawing_e"].Value) DrawingService.RenderSkillshotRange(E);
            if (MenuService.BoolLinks["Drawing_r"].Value) DrawingService.RenderSkillshotRange(R);
        }

        protected override void Combo()
        {
            if (MenuService.BoolLinks["Combo_q"].Value) CastQ();
        }

        protected override void Harass()
        {
            if (MenuService.BoolLinks["Harass_q"].Value) CastQ();
        }

        protected override void Auto()
        {
            if (MenuService.BoolLinks["Auto_q"].Value) CastQ();
        }

        private void CastQ()
        {
            if (!Q.IsReady())
                return;

            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
            if (!target.IsValidTarget(Q.Range) || target.GetWaypoints().Count == 1)
                return;

            if (Q.GetPrediction(target).Hitchance >= HitChance.VeryHigh)
                Q.Cast(target, IsPacketCastEnabled);
        }

        private void CastW()
        {
            
        }

        private void CastE()
        {
            
        }

        private void CastR()
        {
            
        }
    }
}
