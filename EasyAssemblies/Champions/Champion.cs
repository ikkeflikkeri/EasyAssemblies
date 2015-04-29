using System;
using System.Linq;
using EasyAssemblies.Services;
using LeagueSharp;
using LeagueSharp.Common;

namespace EasyAssemblies.Champions
{
    abstract class Champion
    {
        protected Obj_AI_Hero Player;

        private const int UpdateTick = 1000 / 20;
        private int _lastUpdateTick;

        public void Start()
        {
            Player = ObjectManager.Player;

            InitializeSpells();
            InitializeMenu();
            Initialize();

            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += (x => Draw());
            Drawing.OnEndScene += (x => EndScene());
            AntiGapcloser.OnEnemyGapcloser += OnEnemyGapcloser;
            Interrupter2.OnInterruptableTarget += OnInterruptableTarget;
            Spellbook.OnCastSpell += OnCastSpell;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;

            Game.PrintChat("EasyJinx loaded!");
        }

        private void OnUpdate(EventArgs args)
        {
            if (Utils.TickCount < _lastUpdateTick + UpdateTick) return;
            _lastUpdateTick = Utils.TickCount;

            Update();

            if (Player.IsWindingUp || Player.IsDashing()) return;

            var minionBlock = MinionManager.GetMinions(Player.Position, Player.AttackRange, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.None)
                    .Where(x => HealthPrediction.GetHealthPrediction(x, 3000) <= Player.GetAutoAttackDamage(x))
                    .ToList().Count > 0;

            switch (MenuService.Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo: Combo(); break;
                case Orbwalking.OrbwalkingMode.Mixed: if (!minionBlock) Harass(); break;
                default: if (!minionBlock) Auto(); break;
            }
        }

        protected virtual void Initialize() { }
        protected abstract void InitializeSpells();
        protected abstract void InitializeMenu();

        protected abstract void Draw();
        protected virtual void EndScene() { }
        protected virtual void Update() { }

        protected abstract void Combo();
        protected abstract void Harass();
        protected abstract void Auto();

        protected virtual void OnEnemyGapcloser(ActiveGapcloser gapcloser) { }
        protected virtual void OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args) { }
        protected virtual void OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args) { }
        protected virtual void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args) { }

        ~Champion()
        {
            Game.OnUpdate -= OnUpdate;
            Drawing.OnDraw -= (x => Draw());
            Drawing.OnEndScene -= (x => EndScene());
        }

        public bool IsPacketCastEnabled
        {
            get { return MenuService.BoolLinks["packets"].Value; }
        }
    }
}
