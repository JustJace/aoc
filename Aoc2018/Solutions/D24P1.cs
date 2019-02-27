using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Aoc2018.Solutions
{
    public class D24P1 : Solver<int>
    {
        public override int Day => 24;
        public override int Part => 1;
        protected override string Filename => @"Inputs\d24.input";

        protected override int GetAnswer(string input)
        {
            var (immune, infection) = CreateArmies();

            Battle(immune, infection);

            //Print(new Dictionary<Regiment, Regiment>(), immune, infection);

            if (immune.IsAlive())
            {
                return immune.Regiments.Sum(r => r.UnitCount);
            }
            else if (infection.IsAlive())
            {
                return infection.Regiments.Sum(r => r.UnitCount);
            }
            else
            {
                throw new Exception("Both armies alive after battle?! cowards...");
            }

            // 3748 too low
        }

        private Tuple<Army, Army> CreateArmies()
        {
            var immune = new Army() { Name = "Immune System" };
            immune.Regiments.Add(new Regiment(immune, 4592, 2061, 4, 9, Attack.F, new Attack[] { Attack.S, Attack.R }, new Attack[] { Attack.C }));
            immune.Regiments.Add(new Regiment(immune, 1383, 3687, 26, 15, Attack.R, new Attack[] { }, new Attack[] { }));
            immune.Regiments.Add(new Regiment(immune, 2736, 6429, 20, 2, Attack.S, new Attack[] { Attack.S }, new Attack[] { }));
            immune.Regiments.Add(new Regiment(immune, 777, 3708, 39, 4, Attack.C, new Attack[] { Attack.R, Attack.C }, new Attack[] { Attack.S, Attack.F }));
            immune.Regiments.Add(new Regiment(immune, 6761, 2792, 3, 17, Attack.R, new Attack[] { Attack.B, Attack.F, Attack.C, Attack.S }, new Attack[] { }));
            immune.Regiments.Add(new Regiment(immune, 6028, 5537, 7, 6, Attack.R, new Attack[] { Attack.S }, new Attack[] { }));
            immune.Regiments.Add(new Regiment(immune, 2412, 2787, 9, 20, Attack.B, new Attack[] { }, new Attack[] { }));
            immune.Regiments.Add(new Regiment(immune, 6042, 7747, 12, 12, Attack.S, new Attack[] { Attack.R }, new Attack[] { }));
            immune.Regiments.Add(new Regiment(immune, 1734, 7697, 38, 10, Attack.C, new Attack[] { }, new Attack[] { Attack.R, Attack.C }));
            immune.Regiments.Add(new Regiment(immune, 4391, 3250, 7, 19, Attack.C, new Attack[] { }, new Attack[] { }));

            //immune.Regiments.Add(new Regiment(immune, 17, 5390, 4507, 2, Attack.Fire, new Attack[] { }, new Attack[] { Attack.Radiation, Attack.Bludgeoning }));
            //immune.Regiments.Add(new Regiment(immune, 989, 1274, 25, 3, Attack.Slashing, new Attack[] { Attack.Fire }, new Attack[] { Attack.Bludgeoning, Attack.Slashing }));

            var infection = new Army() { Name = "Infection" };
            infection.Regiments.Add(new Regiment(infection, 820, 46229, 106, 18, Attack.S, new Attack[] { Attack.C, Attack.B }, new Attack[] { }));
            infection.Regiments.Add(new Regiment(infection, 723, 30757, 80, 3, Attack.F, new Attack[] { }, new Attack[] { Attack.B }));
            infection.Regiments.Add(new Regiment(infection, 2907, 51667, 32, 1, Attack.F, new Attack[] { Attack.B }, new Attack[] { Attack.S }));
            infection.Regiments.Add(new Regiment(infection, 2755, 49292, 34, 5, Attack.F, new Attack[] { }, new Attack[] { Attack.B }));
            infection.Regiments.Add(new Regiment(infection, 5824, 24708, 7, 11, Attack.B, new Attack[] { Attack.B, Attack.C, Attack.R, Attack.S }, new Attack[] { }));
            infection.Regiments.Add(new Regiment(infection, 7501, 6943, 1, 8, Attack.R, new Attack[] { Attack.S }, new Attack[] { Attack.C }));
            infection.Regiments.Add(new Regiment(infection, 573, 10367, 30, 16, Attack.R, new Attack[] { }, new Attack[] { Attack.C, Attack.S }));
            infection.Regiments.Add(new Regiment(infection, 84, 31020, 639, 14, Attack.S, new Attack[] { }, new Attack[] { Attack.C }));
            infection.Regiments.Add(new Regiment(infection, 2063, 31223, 25, 13, Attack.C, new Attack[] { Attack.B }, new Attack[] { Attack.R }));
            infection.Regiments.Add(new Regiment(infection, 214, 31088, 271, 7, Attack.S, new Attack[] { }, new Attack[] { Attack.F }));

            //infection.Regiments.Add(new Regiment(infection, 801, 4706, 116, 1, Attack.Bludgeoning, new Attack[0], new Attack[] { Attack.Radiation }));
            //infection.Regiments.Add(new Regiment(infection, 4485, 2961, 12, 4, Attack.Slashing, new Attack[] { Attack.Radiation }, new Attack[] { Attack.Fire, Attack.Cold }));

            return new Tuple<Army, Army>(immune, infection);
        }

        private void Battle(Army a1, Army a2)
        {
            while (a1.IsAlive() && a2.IsAlive())
            {
                var targets = new Dictionary<Regiment, Regiment>();
                TargetPhase(targets, a1, a2);
                TargetPhase(targets, a2, a1);

                //Print(targets, a1, a2);

                var totalUnitsLost = AttackPhase(targets);
                if (totalUnitsLost == 0)
                    break;
            }
        }

        private void Print(Dictionary<Regiment, Regiment> targets, Army a1, Army a2)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"--- { a1.Name }---".PadRight(Console.WindowWidth - 1));
            sb.AppendLine("".PadRight(Console.BufferWidth - 1));
            foreach (var regiment in a1.Regiments.OrderByDescending(r => r.EffectivePower).ThenByDescending(r => r.Initiative))
            {
                sb.AppendLine(regiment.ToString().PadRight(Console.WindowWidth - 1));
                //foreach (var enemy in a2.Regiments)
                //{
                //    var msg = $"--> #{enemy.Counter} I:{enemy.Initiative} EPWR: {enemy.EffectivePower} ({CalculateDamage(regiment, enemy)})=>({CalculateUnitsLost(regiment, enemy)})";
                //    if (targets.ContainsKey(regiment) && targets[regiment] == enemy)
                //        msg += " x ";

                //    sb.AppendLine(msg.PadRight(Console.WindowWidth - 1));
                //}
                //sb.AppendLine("".PadRight(Console.WindowWidth - 1));
            }

            sb.AppendLine("".PadRight(Console.BufferWidth - 1));
            sb.AppendLine($"--- { a2.Name } ---".PadRight(Console.WindowWidth - 1));
            sb.AppendLine("".PadRight(Console.BufferWidth - 1));
            foreach (var regiment in a2.Regiments.OrderByDescending(r => r.EffectivePower).ThenByDescending(r => r.Initiative))
            {
                sb.AppendLine(regiment.ToString().PadRight(Console.WindowWidth - 1));
                //foreach (var enemy in a1.Regiments)
                //{
                //    var msg = $"--> #{enemy.Counter} I:{enemy.Initiative}  EPWR: {enemy.EffectivePower} ({CalculateDamage(regiment, enemy)})=>({CalculateUnitsLost(regiment, enemy)})";
                //    if (targets.ContainsKey(regiment) && targets[regiment] == enemy)
                //        msg += " x ";

                //    sb.AppendLine(msg.PadRight(Console.WindowWidth - 1));
                //}
                //sb.AppendLine("".PadRight(Console.WindowWidth - 1));
            }

            for (var i = 0; i < 25; i++)
                sb.AppendLine("".PadRight(Console.BufferWidth - 1));

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(sb.ToString());
            Console.ReadLine();
        }

        private int CalculateDamage(Regiment attacking, Regiment defending)
        {
            if (defending.IsImmuneTo(attacking.Attack))
                return 0;

            var damage = attacking.EffectivePower;

            if (defending.IsWeakTo(attacking.Attack))
                damage *= 2;

            return damage;
        }

        private int CalculateUnitsLost(Regiment attacker, Regiment defender)
        {
            return CalculateDamage(attacker, defender) / defender.UnitHP;
        }

        private int ApplyDamage(Regiment attacker, Regiment defender)
        {
            var unitsLost = CalculateUnitsLost(attacker, defender);
            defender.UnitCount -= unitsLost;
            return unitsLost;
        }

        private int AttackPhase(Dictionary<Regiment, Regiment> targets)
        {
            var totalUnitsLost = 0;

            foreach (var attacker in targets.Keys.OrderByDescending(a => a.Initiative))
            {
                var defender = targets[attacker];

                if (attacker.UnitCount <= 0)
                    continue;

                totalUnitsLost += ApplyDamage(attacker, defender);
                if (defender.UnitCount <= 0)
                    defender.Army.Regiments.Remove(defender);
            }

            return totalUnitsLost;
        }

        private void TargetPhase(Dictionary<Regiment, Regiment> targets, Army attacking, Army defending)
        {
            foreach (var regiment in attacking.Regiments.OrderByDescending(r => r.EffectivePower).ThenByDescending(r => r.Initiative))
            {
                var target = defending.Regiments
                    .Where(r => !targets.ContainsValue(r))
                    .Where(r => CalculateDamage(regiment, r) > 0)
                    .OrderByDescending(r => CalculateDamage(regiment, r))
                    .ThenByDescending(r => r.EffectivePower)
                    .ThenByDescending(r => r.Initiative)
                    .FirstOrDefault();

                if (target != null)
                    targets[regiment] = target;
            }
        }

        private class Regiment
        {
            private static int _counter = 0;
            public int Counter { get; }
            public Regiment(Army army, int startUnits, int startHP, int attackDamage, int initiative, Attack attack, Attack[] immunities, Attack[] weaknesses)
            {
                Counter = _counter++;
                Army = army;
                Initiative = initiative;
                UnitHP = startHP;
                AttackDamage = attackDamage;
                Attack = attack;
                Immunities = immunities;
                ImmunitiesString = Immunities.Any() ? Immunities.Select(i => i.ToString()).Aggregate((s1, s2) => $"{s1}{s2}") : "";
                Weaknesses = weaknesses;
                WeaknessesString = Weaknesses.Any() ? Weaknesses.Select(w => w.ToString()).Aggregate((s1, s2) => $"{s1}{s2}") : "";
                UnitCount = startUnits;
            }
            public int Initiative { get; }
            public int UnitHP { get; }
            public int EffectivePower => UnitCount * AttackDamage;
            public int UnitCount { get; set; }
            public int AttackDamage { get; }
            public Attack Attack { get; }
            public Attack[] Immunities { get; }
            public string ImmunitiesString { get; }
            public Attack[] Weaknesses { get; }
            public string WeaknessesString { get; }
            public bool IsImmuneTo(Attack attack) => Immunities.Contains(attack);
            public bool IsWeakTo(Attack attack) => Weaknesses.Contains(attack);
            public Army Army { get; }
            public override string ToString()
            {
                var counter = Counter.ToString().PadLeft(3);
                var unithp = UnitHP.ToString().PadLeft(5);
                var units = UnitCount.ToString().PadLeft(5);
                var epwr = EffectivePower.ToString().PadLeft(8);
                var init = Initiative.ToString().PadLeft(3);
                var attack = Attack.ToString().PadLeft(3);
                var imm = ImmunitiesString.PadLeft(5);
                var weak = WeaknessesString.PadLeft(5);
                return $"#{counter} UNITS:{units} UHP:{unithp} EPWR:{epwr} INIT:{init} ATK:{attack} IMM:{imm} WEAK:{weak}";
            }
        }

        private class Army
        {
            public string Name { get; set; }
            public List<Regiment> Regiments { get; } = new List<Regiment>();

            public bool IsAlive()
            {
                return Regiments.Any(r => r.UnitCount > 0);
            }
        }

        private enum Attack { S, B, R, C, F }
    }
}