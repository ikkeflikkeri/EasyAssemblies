using System;
using System.Collections.Generic;
using System.Linq;
using EasyAssemblies.Services;
using LeagueSharp;
using LeagueSharp.Common;

namespace EasyAssemblies.Champions
{
    class Ezreal : Champion
    {
        protected override void Initialize()
        {
            DrawingService.SetDamageIndicator(DrawDamage);
        }

        protected override void InitializeSpells()
        {
            Q = new Spell(SpellSlot.Q, 1150f);
            W = new Spell(SpellSlot.W, 1000f);
            E = new Spell(SpellSlot.E, 475f);
            R = new Spell(SpellSlot.R);

            Q.SetSkillshot(0.25f, 60f, 2000f, true, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.25f, 80f, 1600f, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(1f, 160f, 2000f, false, SkillshotType.SkillshotLine);
        }

        protected override void InitializeMenu()
        {
            MenuService.Begin();

            MenuService.AddSubMenu("Combo");
            MenuService.AddBool("Combo_q", "Use Q", true);
            MenuService.AddBool("Combo_w", "Use W", true);

            MenuService.AddSubMenu("Harass");
            MenuService.AddBool("Harass_q", "Use Q", true);
            MenuService.AddBool("Harass_w", "Use W", false);

            MenuService.AddSubMenu("Auto");
            MenuService.AddBool("Auto_q", "Use Q", true);
            MenuService.AddBool("Auto_w", "Use W", false);
            MenuService.AddBool("Auto_r", "Use R", true);
            MenuService.AddSlider("Auto_r_min_range", "Min R range", 1000, 0, 1500);
            MenuService.AddSlider("Auto_r_max_range", "Max R range", 3000, 1500, 5000);

            MenuService.AddSubMenu("Drawing");
            MenuService.AddBool("Drawing_q", "Q Range", true);
            MenuService.AddBool("Drawing_w", "W Range", true);
            MenuService.AddBool("Drawing_e", "E Range", true);
            MenuService.AddBool("Drawing_r", "R Range", true);
            MenuService.AddBool("Drawing_r_damage", "R Damage Indicator", true);

            MenuService.End();
        }

        protected override void Draw()
        {
            if (MenuService.BoolLinks["Drawing_q"].Value) DrawingService.RenderSkillshotRange(Q);
            if (MenuService.BoolLinks["Drawing_w"].Value) DrawingService.RenderSkillshotRange(W);
            if (MenuService.BoolLinks["Drawing_e"].Value) DrawingService.RenderSkillshotRange(E);
            if (MenuService.BoolLinks["Drawing_r"].Value) DrawingService.RenderSkillshotRange(R);

            DrawingService.RenderDamageIndicator(MenuService.BoolLinks["Drawing_r_damage"].Value);
        }

        protected override void Combo()
        {
            if (MenuService.BoolLinks["Combo_q"].Value) CastQ();
            if (MenuService.BoolLinks["Combo_w"].Value) CastW();
        }

        protected override void Harass()
        {
            if (MenuService.BoolLinks["Harass_q"].Value) CastQ();
            if (MenuService.BoolLinks["Harass_w"].Value) CastW();
        }

        protected override void Auto()
        {
            if (MenuService.BoolLinks["Auto_q"].Value) CastQ();
            if (MenuService.BoolLinks["Auto_w"].Value) CastW();
        }

        protected override void Update()
        {
            if (MenuService.BoolLinks["Auto_r"].Value) CastR();
        }

        private void CastQ()
        {
            if (!Q.IsReady())
                return;

            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
            if (!target.IsValidTarget(Q.Range) || !target.IsMoving)
                return;

            if (Q.GetPrediction(target).Hitchance < HitChance.VeryHigh)
                Q.Cast(target, IsPacketCastEnabled);
        }

        private void CastW()
        {
            if (!W.IsReady())
                return;

            var target = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Magical);
            if (!target.IsValidTarget(W.Range) || !target.IsMoving)
                return;

            if (W.GetPrediction(target).Hitchance < HitChance.VeryHigh)
                W.Cast(target, IsPacketCastEnabled);
        }

        private void CastR()
        {
            if (!R.IsReady())
                return;

            var minRange = MenuService.SliderLinks["Auto_r_min_range"].Value.Value;
            var maxRange = MenuService.SliderLinks["Auto_r_max_range"].Value.Value;

            var targets = HeroManager.Enemies.Where(enemy => enemy.IsValidTarget(maxRange) && enemy.Distance(Player) >= minRange);

            foreach (var target in targets)
            {
                var predictedHealth = HealthPrediction.GetHealthPrediction(target, (int)(R.Delay + (Player.Distance(target) / R.Speed) * 1000));

                if (DrawDamage(target) < predictedHealth || predictedHealth <= 0)
                    continue;

                R.Cast(target, IsPacketCastEnabled);
            }
        }

        private float DrawDamage(Obj_AI_Hero hero)
        {
            var reduction = Math.Max(1f - ((R.GetCollision(Player.Position.To2D(), new List<SharpDX.Vector2> { hero.Position.To2D() }).Count) / 10f), 0.3f);
            return R.GetDamage(hero) * reduction;
        }
    }
}
