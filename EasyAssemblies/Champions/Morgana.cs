using System.Linq;
using EasyAssemblies.Data;
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

            Q.SetSkillshot(0.25f, 75f, 1200f, true, SkillshotType.SkillshotLine);
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
            MenuService.AddBool("Auto_q_slow", "Use Q on slowed enemies", true);
            MenuService.AddBool("Auto_q_stun", "Use Q on stunned enemies", true);
            MenuService.AddBool("Auto_q_gap", "Use Q on gapcloser", true);
            MenuService.AddBool("Auto_w", "Use W", true);
            HeroManager.Enemies.Concat(new[] {Player}).ToList().ForEach(hero => MenuService.AddBool("Auto_e_" + hero.ChampionName, "Use E on " + hero.ChampionName, true));

            MenuService.AddSubMenu("Drawing");
            MenuService.AddBool("Drawing_q", "Q Range", true);
            MenuService.AddBool("Drawing_w", "W Range", true);
            MenuService.AddBool("Drawing_e", "E Range", true);
            MenuService.AddBool("Drawing_r", "R Range", true);

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

        protected override void Update()
        {
            CastQAuto();
            if (MenuService.BoolLinks["Auto_w"].Value) CastW();
        }

        private void CastQ()
        {
            if (!Q.IsReady())
                return;

            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
            if (!target.IsValidTarget(Q.Range) || !target.IsMoving)
                return;

            if (Q.GetPrediction(target).Hitchance >= HitChance.High)
                Q.Cast(target, IsPacketCastEnabled);
        }

        private void CastQAuto()
        {
            if (!Q.IsReady())
                return;

            var slow = MenuService.BoolLinks["Auto_q_slow"].Value;
            var stun = MenuService.BoolLinks["Auto_q_stun"].Value;

            HeroManager.Enemies
                .Where(enemy => enemy.IsValidTarget(Q.Range))
                .Where(enemy => (stun && enemy.HasBuffOfType(BuffType.Stun)) || (slow && enemy.HasBuffOfType(BuffType.Slow) && enemy.GetWaypoints().Count > 1))
                .Where(enemy => Q.GetPrediction(enemy).Hitchance >= HitChance.High).ToList()
                .ForEach(enemy => Q.Cast(enemy, IsPacketCastEnabled));
        }

        private void CastW()
        {
            if (!W.IsReady())
                return;

            HeroManager.Enemies
                .Where(enemy => enemy.IsValidTarget(W.Range))
                .Where(enemy => W.GetPrediction(enemy).Hitchance >= HitChance.Immobile).ToList()
                .ForEach(enemy => W.Cast(enemy, IsPacketCastEnabled));
        }

        protected override void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!E.IsReady() || Player.Type != sender.Type || !sender.IsEnemy)
                return;

            var attacker = HeroManager.Enemies.First(x => x.NetworkId == sender.NetworkId);
            foreach (var ally in HeroManager.Enemies.Concat(new[] {Player}).ToList().Where(x => x.IsValidTarget(E.Range, false)).OrderBy(TargetSelector.GetPriority))
            {
                if (!MenuService.BoolLinks["Auto_e_" + ally.ChampionName].Value)
                    continue;

                var spell = SpellDatabase.DangerousSpells.FirstOrDefault(x => x.ChampionName == attacker.ChampionName && x.Slot == attacker.GetSpellSlot(args.SData.Name));

                if (spell == null) continue;

                switch (spell.Type)
                {
                    case SpellType.Unknown:
                        var detectRange = ally.ServerPosition + (args.End - ally.ServerPosition).Normalized() * ally.Distance(args.End);
                        if (detectRange.Distance(ally.ServerPosition) > ally.AttackRange - ally.BoundingRadius)
                            continue;

                        E.CastOnUnit(ally, IsPacketCastEnabled);
                        break;

                    case SpellType.Targeted:
                        if (args.Target.IsAlly) E.CastOnUnit(args.Target as Obj_AI_Base, IsPacketCastEnabled);
                        break;
                }
                
            }
        }

        protected override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (MenuService.BoolLinks["Auto_q_gap"].Value && Q.IsReady() && gapcloser.Sender.IsValidTarget(Q.Range) && Q.GetPrediction(gapcloser.Sender).Hitchance >= HitChance.High)
                Q.Cast(gapcloser.Sender, IsPacketCastEnabled);
        }
    }
}
