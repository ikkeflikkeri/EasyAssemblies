using System.Linq;
using EasyAssemblies.Services;
using LeagueSharp;
using LeagueSharp.Common;

namespace EasyAssemblies.Champions
{
    class KogMaw : Champion
    {
        private Spell Q { get; set; }
        private Spell W { get; set; }
        private Spell E { get; set; }
        private Spell R { get; set; }

        private int RStacks
        {
            get { return Player.Buffs.Where(buff => buff.DisplayName == "KogMawLivingArtillery").Select(buff => buff.Count).FirstOrDefault(); }
        }

        protected override void Initialize()
        {
            DrawingService.SetDamageIndicator(DrawDamage);
        }

        protected override void InitializeSpells()
        {
            Q = new Spell(SpellSlot.Q, 1000f);
            W = new Spell(SpellSlot.W, 760f);
            E = new Spell(SpellSlot.E, 1200f);
            R = new Spell(SpellSlot.R, 1200f);

            Q.SetSkillshot(0.25f, 70f, 1650f, true, SkillshotType.SkillshotLine);
            E.SetSkillshot(0.25f, 120f, 1400f, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(1.2f, 100f, float.MaxValue, false, SkillshotType.SkillshotCircle);
        }

        protected override void InitializeMenu()
        {
            MenuService.Begin();

            MenuService.AddSubMenu("Combo");
            MenuService.AddBool("Combo_q", "Use Q", true);
            MenuService.AddBool("Combo_w", "Use W", true);
            MenuService.AddBool("Combo_e", "Use E", true);
            MenuService.AddBool("Combo_r", "Use R", true);
            MenuService.AddSlider("Combo_max_r_stacks", "Max R stacks", 5, 0, 10);

            MenuService.AddSubMenu("Harass");
            MenuService.AddBool("Harass_q", "Use Q", true);
            MenuService.AddBool("Harass_w", "Use W", true);
            MenuService.AddBool("Harass_e", "Use E", false);
            MenuService.AddBool("Harass_r", "Use R", true);
            MenuService.AddSlider("Harass_max_r_stacks", "Max R stacks", 2, 0, 10);

            MenuService.AddSubMenu("Auto");
            MenuService.AddBool("Auto_q", "Use Q", false);
            MenuService.AddBool("Auto_w", "Use W", false);
            MenuService.AddBool("Auto_e", "Use E", false);
            MenuService.AddBool("Auto_r", "Use R", true);
            MenuService.AddSlider("Auto_max_r_stacks", "Max R stacks", 1, 0, 10);
            MenuService.AddBool("Auto_r_killsteal", "Use R for killsteal", true);

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
            if (MenuService.BoolLinks["Combo_e"].Value) CastE();
            if (MenuService.BoolLinks["Combo_r"].Value && RStacks < MenuService.SliderLinks["Combo_max_r_stacks"].Value.Value) CastR();
        }

        protected override void Harass()
        {
            if (MenuService.BoolLinks["Harass_q"].Value) CastQ();
            if (MenuService.BoolLinks["Harass_w"].Value) CastW();
            if (MenuService.BoolLinks["Harass_e"].Value) CastE();
            if (MenuService.BoolLinks["Harass_r"].Value && RStacks < MenuService.SliderLinks["Harass_max_r_stacks"].Value.Value) CastR();
        }

        protected override void Auto()
        {
            if (MenuService.BoolLinks["Auto_q"].Value) CastQ();
            if (MenuService.BoolLinks["Auto_w"].Value) CastW();
            if (MenuService.BoolLinks["Auto_e"].Value) CastE();
            if (MenuService.BoolLinks["Auto_r"].Value && RStacks < MenuService.SliderLinks["Auto_max_r_stacks"].Value.Value) CastR();
        }

        protected override void Update()
        {
            if (W.Level > 1) W.Range = 740 + W.Level * 20;
            if (R.Level > 1) R.Range = 900 + R.Level * 300;

            if (MenuService.BoolLinks["Auto_r_killsteal"].Value) CastRKillsteal();
        }

        private void CastQ()
        {
            if (!Q.IsReady())
                return;

            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
            if (!target.IsValidTarget(Q.Range))
                return;

            if (Q.GetPrediction(target).Hitchance >= HitChance.VeryHigh)
                Q.Cast(target, IsPacketCastEnabled);
        }

        private void CastW()
        {
            if (!W.IsReady())
                return;

            var target = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Magical);
            if (!target.IsValidTarget(W.Range))
                return;

            W.Cast(IsPacketCastEnabled);
        }

        private void CastE()
        {
            if (!E.IsReady())
                return;

            var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);
            if (!target.IsValidTarget(E.Range))
                return;

            if (E.GetPrediction(target).Hitchance >= HitChance.VeryHigh)
                E.Cast(target, IsPacketCastEnabled);
        }

        private void CastR()
        {
            if (!R.IsReady())
                return;

            var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);
            if (!target.IsValidTarget(R.Range) || target.GetWaypoints().Count == 1)
                return;

            if (R.GetPrediction(target).Hitchance >= HitChance.VeryHigh)
                R.Cast(target, IsPacketCastEnabled);
        }

        private void CastRKillsteal()
        {
            if (!R.IsReady())
                return;

            HeroManager.Enemies
                .Where(enemy => enemy.IsValidTarget(R.Range))
                .Where(enemy => HealthPrediction.GetHealthPrediction(enemy, (int)R.Delay * 1000) < DrawDamage(enemy))
                .Where(enemy => HealthPrediction.GetHealthPrediction(enemy, (int)R.Delay * 1000) > 0)
                .Where(enemy => R.GetPrediction(enemy).Hitchance >= HitChance.VeryHigh).ToList()
                .ForEach(enemy => R.Cast(enemy, IsPacketCastEnabled));
        }

        private float DrawDamage(Obj_AI_Hero hero)
        {
            return R.GetDamage(hero);
        }
    }
}
