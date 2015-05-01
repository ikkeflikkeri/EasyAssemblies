using System.Linq;
using EasyAssemblies.Services;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace EasyAssemblies.Champions
{
    class Jinx : Champion
    {
        private Spell Q { get; set; }
        private Spell W { get; set; }
        private Spell E { get; set; }
        private Spell R { get; set; }

        private bool IsUsingRockets
        {
            get { return Player.AttackRange > 525; }
        }

        protected override void Initialize()
        {
            DrawingService.SetDamageIndicator(DrawDamage);
        }

        protected override void InitializeSpells()
        {
            Q = new Spell(SpellSlot.Q, 725f);
            W = new Spell(SpellSlot.W, 1500f);
            E = new Spell(SpellSlot.E, 900f);
            R = new Spell(SpellSlot.R);

            W.SetSkillshot(0.6f, 60f, 3300f, true, SkillshotType.SkillshotLine);
            E.SetSkillshot(1.0f, 40f, 1750f, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(0.6f, 140f, 1700f, false, SkillshotType.SkillshotLine);
        }

        protected override void InitializeMenu()
        {
            MenuService.Begin();

            MenuService.AddSubMenu("Combo");
            MenuService.AddBool("Combo_q", "Use Q", true);
            MenuService.AddBool("Combo_w", "Use W", true);
            MenuService.AddBool("Combo_e", "Use E", true);
            MenuService.AddBool("Combo_r", "Use R for kill", true);

            MenuService.AddSubMenu("Harass");
            MenuService.AddBool("Harass_q", "Use Q", true);
            MenuService.AddBool("Harass_w", "Use W", false);
            MenuService.AddBool("Harass_e", "Use E", false);

            MenuService.AddSubMenu("Auto");
            MenuService.AddBool("Auto_e_slow", "Use E on slowed enemies", true);
            MenuService.AddBool("Auto_e_stun", "Use E on stunned enemies", true);
            MenuService.AddBool("Auto_e_gap", "Use E on gapcloser", true);
            MenuService.AddBool("Auto_r", "Use R", true);
            MenuService.AddSlider("Auto_r_min_range", "Min R range", 800, 0, 1500);
            MenuService.AddSlider("Auto_r_max_range", "Max R range", 3000, 1500, 5000);

            MenuService.AddSubMenu("Drawing");
            MenuService.AddBool("Drawing_q", "Q Range", true);
            MenuService.AddBool("Drawing_w", "W Range", true);
            MenuService.AddBool("Drawing_e", "E Range", true);
            MenuService.AddBool("Drawing_r_damage", "R Damage Indicator", true);

            MenuService.AddSubMenu("Misc");
            MenuService.AddBool("Misc_q_switch", "Switch to minigun for minions", true);
            MenuService.AddBool("Misc_q_laneclear", "Use Q in laneclear on champions", true);
            MenuService.AddSlider("Misc_w_min_range", "W minimum range", 500, 0, (int)W.Range);
            MenuService.AddKeyBind("Misc_e", "Use E key", 'T', KeyBindType.Press);

            MenuService.End();
        }

        protected override void Draw()
        {
            if (MenuService.BoolLinks["Drawing_q"].Value) DrawingService.RenderSkillshotRange(Q);
            if (MenuService.BoolLinks["Drawing_w"].Value) DrawingService.RenderSkillshotRange(W);
            if (MenuService.BoolLinks["Drawing_e"].Value) DrawingService.RenderSkillshotRange(E);

            DrawingService.RenderDamageIndicator(MenuService.BoolLinks["Drawing_r_damage"].Value);
        }

        protected override void Update()
        {
            Q.Range = 700 + Q.Level * 25;

            if (MenuService.KeyLinks["Misc_e"].Value.Active)
                CastE();

            if (MenuService.BoolLinks["Misc_q_switch"].Value && IsUsingRockets)
            {
                if (MenuService.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit)
                    Q.Cast(IsPacketCastEnabled);

                if ((MenuService.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed || MenuService.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear) &&
                    !TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical).IsValidTarget(Q.Range))
                {
                    if (!TargetSelector.GetTarget(Player.AttackRange, TargetSelector.DamageType.Physical).IsValidTarget(Player.AttackRange))
                        Q.Cast(IsPacketCastEnabled);
                }
            }

            CastEAuto();
            if (MenuService.BoolLinks["Auto_r"].Value) CastRAuto();
        }

        protected override void Combo()
        {
            if (MenuService.BoolLinks["Combo_q"].Value) CastQ();
            if (MenuService.BoolLinks["Combo_w"].Value) CastW();
            if (MenuService.BoolLinks["Combo_e"].Value) CastE();
            if (MenuService.BoolLinks["Combo_r"].Value) CastRCombo();
        }

        protected override void Harass()
        {
            if (MenuService.BoolLinks["Harass_q"].Value) CastQHarass();
            if (MenuService.BoolLinks["Harass_w"].Value) CastW();
            if (MenuService.BoolLinks["Harass_e"].Value) CastE();
        }

        protected override void Auto()
        {
            if (MenuService.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear && MenuService.BoolLinks["Misc_q_laneclear"].Value)
                CastQHarass();
        }

        private void CastQ()
        {
            if (!Q.IsReady())
                return;

            var target = TargetSelector.GetTarget(Q.Range + 100, TargetSelector.DamageType.Physical);
            if (!target.IsValidTarget(Q.Range + 100))
                return;

            var distance = Player.Position.Distance(target.Position);

            if ((IsUsingRockets && distance <= Player.AttackRange) || (!IsUsingRockets && distance > Player.AttackRange))
                Q.Cast(IsPacketCastEnabled);
        }
        
        private void CastQHarass()
        {
            if (!Q.IsReady())
                return;

            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
            if (!target.IsValidTarget(Q.Range))
                return;

            var distance = Player.Position.Distance(target.Position);

            if (!IsUsingRockets && distance > Player.AttackRange)
                Q.Cast(IsPacketCastEnabled);
        }

        private void CastW()
        {
            if (!W.IsReady())
                return;

            var target = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);
            if (!target.IsValidTarget(W.Range))
                return;

            var distance = Player.Position.Distance(target.Position);
            if (distance < MenuService.SliderLinks["Misc_w_min_range"].Value.Value)
                return;

            if (W.GetPrediction(target).Hitchance >= HitChance.VeryHigh)
                W.Cast(target, IsPacketCastEnabled);
        }

        private void CastE()
        {
            if (!E.IsReady())
                return;

            var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
            if (!target.IsValidTarget(E.Range) || !target.IsMoving)
                return;

            if (E.GetPrediction(target).Hitchance < HitChance.VeryHigh)
                return;

            if (target.IsFacing(Player) && target.HealthPercent > Player.HealthPercent)
                E.Cast(target, IsPacketCastEnabled);
            if (!target.IsFacing(Player) && target.HealthPercent <= Player.HealthPercent)
                E.Cast(target, IsPacketCastEnabled);
        }
        
        private void CastEAuto()
        {
            if (!E.IsReady())
                return;

            var slow = MenuService.BoolLinks["Auto_e_slow"].Value;
            var stun = MenuService.BoolLinks["Auto_e_stun"].Value;

            HeroManager.Enemies
                .Where(enemy => enemy.IsValidTarget(E.Range))
                .Where(enemy => (stun && enemy.HasBuffOfType(BuffType.Stun)) || (slow && enemy.HasBuffOfType(BuffType.Slow) && enemy.GetWaypoints().Count > 1))
                .Where(enemy => E.GetPrediction(enemy).Hitchance >= HitChance.VeryHigh).ToList()
                .ForEach(enemy => E.Cast(enemy, IsPacketCastEnabled));
        }

        private void CastRCombo()
        {
            if (!R.IsReady())
                return;

            var target = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);
            if (!target.IsValidTarget(W.Range))
                return;

            var predictedHealth = HealthPrediction.GetHealthPrediction(target, (int) (R.Delay + (Player.Distance(target)/R.Speed)*1000));
            if (R.GetDamage(target) < predictedHealth || predictedHealth <= 0)
                return;

            if (CanHitR(target))
                R.Cast(target, IsPacketCastEnabled);
        }

        private void CastRAuto()
        {
            if (!R.IsReady())
                return;

            var minRange = MenuService.SliderLinks["Auto_r_min_range"].Value.Value;
            var maxRange = MenuService.SliderLinks["Auto_r_max_range"].Value.Value;

            var targets = HeroManager.Enemies.Where(enemy => enemy.IsValidTarget(maxRange) && enemy.Distance(Player) >= minRange);

            foreach (var target in targets)
            {
                var predictedHealth = HealthPrediction.GetHealthPrediction(target, (int)(R.Delay + (Player.Distance(target) / R.Speed) * 1000));

                if (R.GetDamage(target) < predictedHealth || predictedHealth <= 0)
                    continue;

                if (CanHitR(target))
                    R.Cast(target, IsPacketCastEnabled);
            }
        }


        protected override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (MenuService.BoolLinks["Auto_e_gap"].Value && E.IsReady() && gapcloser.Sender.IsValidTarget(E.Range) && E.GetPrediction(gapcloser.Sender).Hitchance >= HitChance.VeryHigh)
                E.Cast(gapcloser.Sender, IsPacketCastEnabled);
        }

        private float DrawDamage(Obj_AI_Base hero)
        {
            return R.GetDamage(hero);
        }

        private bool CanHitR(Obj_AI_Base hero)
        {
            var predictionOutput = R.GetPrediction(hero);

            if (predictionOutput.Hitchance < HitChance.VeryHigh)
                return false;

            foreach (var enemy in HeroManager.Enemies.Where(enemy => enemy.IsValidTarget() && enemy.SkinName != hero.SkinName))
            {
                var predictedPosition = R.GetPrediction(enemy).CastPosition;

                var v = predictionOutput.CastPosition - Player.Position;
                var w = predictedPosition - Player.Position;

                var c1 = Vector3.Dot(w, v);
                var c2 = Vector3.Dot(v, v);
                var b = c1/c2;

                var length = Vector3.Distance(predictedPosition, Player.Position + (b*v));
                var predictedHealth = HealthPrediction.GetHealthPrediction(enemy, (int)(R.Delay + (Player.Distance(enemy) / R.Speed) * 1000));

                if (R.GetDamage(enemy) < predictedHealth && length < (R.Width + enemy.BoundingRadius/2) && Player.Distance(predictedPosition) < Player.Distance(hero))
                    return false;
            }

            return true;
        }
    }
}
