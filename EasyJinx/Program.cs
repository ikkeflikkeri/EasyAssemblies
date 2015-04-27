using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;

namespace EasyJinx
{
    class Program
    {
        public static int LastUpdateTick = Environment.TickCount;
        public static int UpdateTick = 1000 / 20;

        public static Menu Menu;
        public static Obj_AI_Hero Player;
        public static Orbwalking.Orbwalker Orbwalker;

        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;
        
        public static Color GreenColor = Color.FromArgb(100, 0, 255, 0);
        public static Color RedColor = Color.FromArgb(100, 255, 0, 0);

        public static void Main()
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Player = ObjectManager.Player;

            InitSpells();
            InitMenu();

            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void InitMenu()
        {
            Menu = new Menu("EasyJinx", "EasyJinx2", true);

            var curMenu = Menu.AddSubMenu(new Menu("Target Selector", "Target Selector"));
            TargetSelector.AddToMenu(curMenu);

            curMenu = Menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new Orbwalking.Orbwalker(curMenu);

            curMenu = Menu.AddSubMenu(new Menu("Combo", "Combo"));
            curMenu.AddItem(new MenuItem("Combo_q", "Use Q").SetValue(true));
            curMenu.AddItem(new MenuItem("Combo_w", "Use W").SetValue(true));
            curMenu.AddItem(new MenuItem("Combo_e", "Use E").SetValue(true));

            curMenu = Menu.AddSubMenu(new Menu("Harass", "Harass"));
            curMenu.AddItem(new MenuItem("Harass_q", "Use Q").SetValue(true));
            curMenu.AddItem(new MenuItem("Harass_w", "Use W").SetValue(false));
            curMenu.AddItem(new MenuItem("Harass_e", "Use E").SetValue(false));

            curMenu = Menu.AddSubMenu(new Menu("Auto", "Auto"));
            curMenu.AddItem(new MenuItem("Auto_eslow", "Use E on slowed enemies").SetValue(true));
            curMenu.AddItem(new MenuItem("Auto_estun", "Use E on stunned enemies").SetValue(true));
            curMenu.AddItem(new MenuItem("Auto_egap", "Use E on gapcloser").SetValue(true));
            curMenu.AddItem(new MenuItem("Auto_r", "Use R").SetValue(true));
            curMenu.AddItem(new MenuItem("Auto_minrange", "Min R range").SetValue(new Slider(200, 0, 1500)));
            curMenu.AddItem(new MenuItem("Auto_maxrange", "Max R range").SetValue(new Slider(3000, 1500, 5000)));

            curMenu = Menu.AddSubMenu(new Menu("Drawing", "Drawing"));
            curMenu.AddItem(new MenuItem("Drawing_q", "Q Range").SetValue(true));
            curMenu.AddItem(new MenuItem("Drawing_w", "W Range").SetValue(true));
            curMenu.AddItem(new MenuItem("Drawing_e", "E Range").SetValue(true));
            curMenu.AddItem(new MenuItem("Drawing_rdamage", "R Damage Indicator").SetValue(true));

            curMenu = Menu.AddSubMenu(new Menu("Misc", "Misc"));
            curMenu.AddItem(new MenuItem("Misc_qswitch", "Switch to minigun for minions").SetValue(true));
            curMenu.AddItem(new MenuItem("Misc_wrange", "W minimum range").SetValue(new Slider(500, 0, (int)W.Range)));

            Menu.AddToMainMenu();
        }

        private static void InitSpells()
        {
            Q = new Spell(SpellSlot.Q, 725f);
            W = new Spell(SpellSlot.W, 1500f);
            E = new Spell(SpellSlot.E, 900f);
            R = new Spell(SpellSlot.R);

            W.SetSkillshot(0.6f, 60f, 3300f, true, SkillshotType.SkillshotLine);
            E.SetSkillshot(0.9f, 40f, 1750f, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(0.6f, 140f, 1700f, false, SkillshotType.SkillshotLine);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Menu.Item("Drawing_q").GetValue<bool>()) Render.Circle.DrawCircle(Player.Position, Q.Range, (Q.IsReady() ? GreenColor : RedColor));
            if (Menu.Item("Drawing_w").GetValue<bool>()) Render.Circle.DrawCircle(Player.Position, W.Range, (W.IsReady() ? GreenColor : RedColor));
            if (Menu.Item("Drawing_e").GetValue<bool>()) Render.Circle.DrawCircle(Player.Position, E.Range, (E.IsReady() ? GreenColor : RedColor));
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Utils.TickCount < LastUpdateTick + UpdateTick) return;
            LastUpdateTick = Utils.TickCount;

            Update();

            var minionBlock = MinionManager.GetMinions(Player.Position, Player.AttackRange, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.None)
                    .Where(x => HealthPrediction.GetHealthPrediction(x, 3000) <= Player.GetAutoAttackDamage(x))
                    .ToList().Count > 0;

            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo: Combo(); break;
                case Orbwalking.OrbwalkingMode.Mixed: if (!minionBlock) Harass(); break;
                default: if (!minionBlock) Auto(); break;
            }
        }

        private static void Update()
        {
            Q.Range = 700 + Q.Level * 25;

            if (Menu.Item("Misc_qswitch").GetValue<bool>() && Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear || Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit || Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
            {
                if (IsUsingRockets()) Q.Cast();
            }
        }

        private static void Auto()
        {

        }

        private static void Harass()
        {
            if (Menu.Item("Harass_e").GetValue<bool>()) CastE();
            if (Menu.Item("Harass_w").GetValue<bool>()) CastW();
            if (Menu.Item("Harass_q").GetValue<bool>()) CastQ();
        }

        private static void Combo()
        {
            if (Menu.Item("Combo_e").GetValue<bool>()) CastE();
            if (Menu.Item("Combo_w").GetValue<bool>()) CastW();
            if (Menu.Item("Combo_q").GetValue<bool>()) CastQ();
        }

        private static void CastQ()
        {
            if (!Q.IsReady()) return;

            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
            if (!target.IsValidTarget(Q.Range)) return;

            var distance = Player.Position.Distance(target.Position);

            if (IsUsingRockets())
            {
                if (distance <= Player.AttackRange) Q.Cast();
            }
            else
            {
                if (distance > Player.AttackRange) Q.Cast();
            }
        }

        private static void CastW()
        {
            if (!W.IsReady()) return;

            var target = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);
            if (!target.IsValidTarget(W.Range)) return;

            var distance = Player.Position.Distance(target.Position);
            if (distance < Menu.Item("Misc_wrange").GetValue<Slider>().Value) return;

            if (W.GetPrediction(target).Hitchance >= HitChance.VeryHigh)
                W.Cast(target, true);
        }

        private static void CastE()
        {
            if (!E.IsReady()) return;

            var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
            if (!target.IsValidTarget(E.Range) || target.GetWaypoints().Count == 1) return;

            if (E.GetPrediction(target).Hitchance >= HitChance.VeryHigh)
                E.Cast(target, true);
        }

        public static void CastR()
        {
            if (!R.IsReady()) return;
        }

        private static bool IsUsingRockets()
        {
            return Player.AttackRange > 525;
        }
    }
}
