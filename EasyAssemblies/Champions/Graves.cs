using System;
using System.Linq;
using EasyAssemblies.Services;
using LeagueSharp;
using LeagueSharp.Common;

namespace EasyAssemblies.Champions
{
    class Graves : Champion
    {
        private Spell Q { get; set; }
        private Spell W { get; set; }
        private Spell E { get; set; }
        private Spell R { get; set; }

        protected override void Initialize()
        {
            DrawingService.SetDamageIndicator(DrawDamage);
        }

        protected override void InitializeSpells()
        {
            Q = new Spell(SpellSlot.Q, 915f);
            W = new Spell(SpellSlot.W, 950f);
            E = new Spell(SpellSlot.E, 425f);
            R = new Spell(SpellSlot.R, 1900f);

            Q.SetSkillshot(0.25f, 15f * 2 * (float)Math.PI / 180, 2000f, false, SkillshotType.SkillshotCone);
            W.SetSkillshot(0.25f, 250f, 1650f, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(0.25f, 100f, 2100f, true, SkillshotType.SkillshotLine);
        }

        protected override void InitializeMenu()
        {
            MenuService.Begin();

            MenuService.AddSubMenu("Combo");
            MenuService.AddBool("Combo_q", "Use Q", true);
            MenuService.AddBool("Combo_w", "Use W", true);

            MenuService.AddSubMenu("Harass");
            MenuService.AddBool("Harass_q", "Use Q", true);
            MenuService.AddBool("Harass_w", "Use W", true);

            MenuService.AddSubMenu("Auto");
            MenuService.AddBool("Auto_q", "Use Q", false);
            MenuService.AddBool("Auto_w", "Use W", false);
            MenuService.AddBool("Auto_r", "Use R", false);

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

            foreach (var target in HeroManager.Enemies.Where(enemy => enemy.IsValidTarget(R.Range) && enemy.IsMoving))
            {
                var predictedHealth = HealthPrediction.GetHealthPrediction(target, (int) (R.Delay + (Player.Distance(target)/R.Speed)*1000));
                if (predictedHealth < 0 || predictedHealth > R.GetDamage(target))
                    continue;

                var prediction = R.GetPrediction(target);
                if (prediction.Hitchance < HitChance.High)
                    continue;

                var collision = prediction.CollisionObjects.Any(x => x.IsEnemy && x.Distance(Player) < target.Distance(Player) - 750f);
                if (!collision)
                    R.Cast(target, IsPacketCastEnabled);
            }
        }

        private float DrawDamage(Obj_AI_Hero hero)
        {
            return R.GetDamage(hero);
        }

        /*private static float DistanceFromLine(Vector2 p, Vector2 v, Vector2 w)
        {
            var l2 = (w - v).LengthSquared();
            if (Math.Abs(l2) < 0.0) return Vector2.Distance(p, v);

            var t = Vector2.Dot(p - v, w - v) / l2;
            if (t < 0.0) return Vector2.Distance(p, v);
            if (t > 1.0) return Vector2.Distance(p, w);

            var projection = v + t * (w - v);
            return Vector2.Distance(p, projection);
        }*/
    }
}
