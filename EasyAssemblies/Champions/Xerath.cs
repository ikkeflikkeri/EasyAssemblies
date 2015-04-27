using System.Linq;
using EasyAssemblies.Services;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Common.Data;

namespace EasyAssemblies.Champions
{
    class Xerath : Champion
    {
        private Spell Q { get; set; }
        private Spell W { get; set; }
        private Spell WCenter { get; set; }
        private Spell E { get; set; }
        private Spell R { get; set; }

        private Items.Item BlueTrinket1 { get; set; }
        private Items.Item BlueTrinket2 { get; set; }

        private bool IsChargingUltimate
        {
            get { return Player.HasBuff("XerathLocusOfPower2", true) || (Player.LastCastedSpellName() == "XerathLocusOfPower2" && Utils.TickCount - Player.LastCastedSpellT() < 500); }
        }

        protected override void Initialize()
        {
            DrawingService.SetDamageIndicator(DrawDamage);

            BlueTrinket1 = ItemData.Scrying_Orb_Trinket.GetItem();
            BlueTrinket2 = ItemData.Farsight_Orb_Trinket.GetItem();
        }

        protected override void InitializeSpells()
        {
            Q = new Spell(SpellSlot.Q, 1600f);
            W = new Spell(SpellSlot.W, 1000f);
            WCenter = new Spell(SpellSlot.W, 1000f);
            E = new Spell(SpellSlot.E, 1150f);
            R = new Spell(SpellSlot.R, 2950f);

            Q.SetSkillshot(0.6f, 100f, float.MaxValue, false, SkillshotType.SkillshotLine);
            Q.SetCharged("XerathArcanopulseChargeUp", "XerathArcanopulseChargeUp", 750, 1550, 1.5f);
            W.SetSkillshot(0.7f, 200f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            WCenter.SetSkillshot(0.7f, 50f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            E.SetSkillshot(0.2f, 60, 1400f, true, SkillshotType.SkillshotLine);
            R.SetSkillshot(0.7f, 120f, float.MaxValue, false, SkillshotType.SkillshotCircle);
        }

        protected override void InitializeMenu()
        {
            MenuService.Begin();

            MenuService.AddSubMenu("Combo");
            MenuService.AddBool("Combo_q", "Use Q", true);
            MenuService.AddBool("Combo_w", "Use W", true);
            MenuService.AddBool("Combo_e", "Use E", true);

            MenuService.AddSubMenu("Harass");
            MenuService.AddBool("Harass_q", "Use Q", true);
            MenuService.AddBool("Harass_w", "Use W", true);
            MenuService.AddBool("Harass_e", "Use E", true);

            MenuService.AddSubMenu("Auto");
            MenuService.AddBool("Auto_e", "Use E", false);
            MenuService.AddBool("Auto_e_interrupt", "Use E to interrupt", true);
            MenuService.AddBool("auto_e_slows", "Use E on slows", true);
            MenuService.AddBool("auto_e_stuns", "Use E on stuns", true);
            MenuService.AddBool("auto_e_gapclosers", "Use E on gapclosers", true);

            MenuService.AddSubMenu("Drawing");
            MenuService.AddBool("Drawing_q", "Q Range", true);
            MenuService.AddBool("Drawing_w", "W Range", true);
            MenuService.AddBool("Drawing_e", "E Range", true);
            MenuService.AddBool("Drawing_r", "R Range", true);
            MenuService.AddBool("Drawing_r_map", "R Range on minimap", true);
            MenuService.AddBool("Drawing_r_damage", "R Damage Indicator", true);

            MenuService.AddSubMenu("Misc");
            MenuService.AddKeyBind("Misc_e", "Use E key", 'T', KeyBindType.Press);
            MenuService.AddBool("Misc_w_center", "Use W centered", true);
            MenuService.AddBool("Misc_r", "Use R charges when ult is pressed", true);
            MenuService.AddSlider("Misc_r_min_delay", "R min delay between charges", 800, 0, 1500);
            MenuService.AddSlider("Misc_r_max_delay", "R max delay between charges", 1750, 1500, 3000);
            MenuService.AddSlider("Misc_r_dash", "R delay after flash/dash", 1000, 0, 2000);
            MenuService.AddBool("Misc_r_blue", "Use Blue Trinket when ulting", true);

            MenuService.End();
        }

        protected override void Update()
        {
            Orbwalking.Attack = !Q.IsCharging;

            if (R.Level > 0)
                R.Range = 1750 + R.Level * 1200;

            if (MenuService.KeyLinks["Misc_e"].Value.Active)
                CastE();

            if (MenuService.BoolLinks["Misc_r"].Value && IsChargingUltimate)
                CastR();
            else
                _rTarget = null;
        }

        protected override void Draw()
        {
            if (MenuService.BoolLinks["Drawing_q"].Value) DrawingService.RenderSkillshotRange(Q);
            if (MenuService.BoolLinks["Drawing_w"].Value) DrawingService.RenderSkillshotRange(W);
            if (MenuService.BoolLinks["Drawing_e"].Value) DrawingService.RenderSkillshotRange(E);
            if (MenuService.BoolLinks["Drawing_r"].Value) DrawingService.RenderSkillshotRange(R);
            if (MenuService.BoolLinks["Drawing_r_map"].Value) DrawingService.RenderSkillshotRange(R, true);

            DrawingService.RenderDamageIndicator(MenuService.BoolLinks["Drawing_r_damage"].Value);
        }

        protected override void EndScene()
        {
            if (MenuService.BoolLinks["Drawing_r_map"].Value) DrawingService.RenderSkillshotRange(R, true);
        }

        protected override void Combo()
        {
            if (MenuService.BoolLinks["Combo_q"].Value) CastQ();
            if (MenuService.BoolLinks["Combo_w"].Value) CastW();
            if (MenuService.BoolLinks["Combo_e"].Value) CastE();
        }

        protected override void Harass()
        {
            if (MenuService.BoolLinks["Harass_q"].Value) CastQ();
            if (MenuService.BoolLinks["Harass_w"].Value) CastW();
            if (MenuService.BoolLinks["Harass_e"].Value) CastE();
        }

        protected override void Auto()
        {
            CastEAuto();
            if (MenuService.BoolLinks["Auto_e"].Value) CastE();
        }

        private void CastQ()
        {
            if (!Q.IsReady())
                return;

            var target = TargetSelector.GetTarget(Q.ChargedMaxRange, TargetSelector.DamageType.Magical);
            if (!target.IsValidTarget(Q.ChargedMaxRange))
                return;

            if(!Q.IsCharging)
                Q.StartCharging();
            else
            {
                if (Q.GetPrediction(target).Hitchance >= HitChance.VeryHigh)
                    Q.Cast(target, IsPacketCastEnabled);
                else
                {
                    var distance = Player.Distance(target) + target.BoundingRadius * 2;

                    if (distance > Q.ChargedMaxRange)
                        distance = Q.ChargedMaxRange;

                    if (Q.Range >= distance && Q.GetPrediction(target).Hitchance >= HitChance.High)
                        Q.Cast(target, IsPacketCastEnabled);
                }
            }
        }

        private void CastW()
        {
            if (!W.IsReady())
                return;

            var target = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Magical);
            if (!target.IsValidTarget(W.Range))
                return;

            if (MenuService.BoolLinks["Misc_w_center"].Value)
            {
                if (WCenter.GetPrediction(target).Hitchance >= HitChance.VeryHigh)
                    WCenter.Cast(target, IsPacketCastEnabled);
            }
            else
            {
                if (W.GetPrediction(target).Hitchance >= HitChance.VeryHigh)
                    W.Cast(target, IsPacketCastEnabled, true);
            }
        }

        private void CastE()
        {
            if (!E.IsReady())
                return;

            var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);
            if (!target.IsValidTarget(E.Range) || target.GetWaypoints().Count == 1)
                return;

            if (E.GetPrediction(target).Hitchance >= HitChance.VeryHigh)
                E.Cast(target, IsPacketCastEnabled);
        }

        private void CastEAuto()
        {
            if (!E.IsReady())
                return;

            var slow = MenuService.BoolLinks["auto_e_slows"].Value;
            var stun = MenuService.BoolLinks["auto_e_stuns"].Value;

            HeroManager.Enemies
                .Where(enemy => enemy.IsValidTarget(E.Range))
                .Where(enemy => (stun && enemy.HasBuffOfType(BuffType.Stun)) || (slow && enemy.HasBuffOfType(BuffType.Slow) && enemy.GetWaypoints().Count > 1))
                .Where(enemy => E.GetPrediction(enemy).Hitchance >= HitChance.VeryHigh).ToList()
                .ForEach(enemy => E.Cast(enemy, IsPacketCastEnabled));
        }

        private Obj_AI_Hero _rTarget;
        private int _rTargetChangedWaitTime;
        private int _dashWaitTime;

        private void CastR()
        {
            if (!R.IsReady())
                return;

            var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);
            if (!target.IsValidTarget(R.Range))
                return;

            var minDelay = MenuService.SliderLinks["Misc_r_min_delay"].Value.Value;
            var maxDelay = MenuService.SliderLinks["Misc_r_max_delay"].Value.Value;
            var dashDelay = MenuService.SliderLinks["Misc_r_dash"].Value.Value;

            if (_rTarget == null)
                _rTarget = target;
            else if(_rTarget.NetworkId != target.NetworkId)
            {
                var time = _rTarget.Distance(target);
                if (time > 3000) time = 3000;

                _rTargetChangedWaitTime = Utils.TickCount + (int)time;
                _rTarget = target;
            }

            if ((Player.LastCastedSpellName().ToLower() == "summonerflash" && Player.LastCastedSpellT() > Utils.TickCount - 100) || _rTarget.IsDashing())
                _dashWaitTime = Utils.TickCount + dashDelay;

            if (Utils.TickCount >= _rTargetChangedWaitTime &&
                Utils.TickCount >= _dashWaitTime &&
                Utils.TickCount >= Player.LastCastedSpellT() + minDelay)
            {
                if (R.GetPrediction(_rTarget).Hitchance >= HitChance.VeryHigh)
                    R.Cast(_rTarget, IsPacketCastEnabled);
                if (R.GetPrediction(_rTarget).Hitchance >= HitChance.High && Utils.TickCount >= Player.LastCastedSpellT() + maxDelay)
                    R.Cast(_rTarget, IsPacketCastEnabled);
            }

        }

        private float DrawDamage(Obj_AI_Base hero)
        {
            return R.GetDamage(hero) * 3;
        }

        protected override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (MenuService.BoolLinks["auto_e_gapclosers"].Value && E.IsReady() && gapcloser.Sender.IsValidTarget(E.Range) && E.GetPrediction(gapcloser.Sender).Hitchance >= HitChance.VeryHigh)
                E.Cast(gapcloser.Sender, IsPacketCastEnabled);
        }

        protected override void OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (MenuService.BoolLinks["Auto_e_interrupt"].Value && E.IsReady() && sender.IsValidTarget(E.Range) && args.DangerLevel >= Interrupter2.DangerLevel.Medium && E.GetPrediction(sender).Hitchance >= HitChance.VeryHigh)
                E.Cast(sender, IsPacketCastEnabled);
        }

        protected override void OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (sender.Owner.IsMe && args.Slot == SpellSlot.R && MenuService.BoolLinks["misc_r_blue"].Value)
            {
                var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);
                if (target == null || !target.IsValidTarget(R.Range)) return;

                if (_rTarget == null)
                    _rTarget = target;

                if ((BlueTrinket1.IsOwned() && BlueTrinket1.IsReady()) || (BlueTrinket2.IsOwned() && BlueTrinket2.IsReady()))
                {
                    if (BlueTrinket1.IsOwned() && BlueTrinket1.IsReady() && (Player.Level >= 9 ? 3500f : BlueTrinket1.Range) >= Player.Distance(_rTarget))
                    {
                        BlueTrinket1.Cast(_rTarget.Position);
                        Utility.DelayAction.Add(175, () => R.Cast(IsPacketCastEnabled));
                    }
                    if (BlueTrinket2.IsOwned() && BlueTrinket2.IsReady() && BlueTrinket2.Range >= Player.Distance(_rTarget))
                    {
                        BlueTrinket2.Cast(_rTarget.Position);
                        Utility.DelayAction.Add(175, () => R.Cast(IsPacketCastEnabled));
                    }
                }
                else
                    args.Process = true;
            }
        }

    }
}
