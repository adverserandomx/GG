using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Grind.Common;
using D3;

namespace Grind.Classes
{
    public static class WitchDoctor
    {
        public static Spell poisonDarts = new Spell(SNOPowerId.Witchdoctor_PoisonDart, 0, 0, 0);
        public static Spell potion = new Spell(SNOPowerId.Axe_Operate_Gizmo, 30, 0, 0, true);
        public static Spell bears = new Spell(SNOPowerId.Witchdoctor_ZombieCharger, 0, 140, 0);


        public static void init() {
        }

        public static bool AttackUnit(Unit _unit, TimeSpan _timeout)
        {
            if (_unit.Life <= 0)
            {
                return false;
            }

            TimeSpan startTime = TimeSpan.FromTicks(System.Environment.TickCount);


            while (_unit.Life > 0)
            {
                try
                {
                    //bears.use(_unit);

                    poisonDarts.use(_unit);

                    if (1.0 * Me.Life / Me.MaxLife < 0.4)
                    {
                        potion.use(Unit.Get().First(i => i.Name.Contains("Health Potion") && i.ItemContainer == Container.Inventory));
                    }

                    Thread.Sleep(200);

                    if (TimeSpan.FromTicks(System.Environment.TickCount).Subtract(startTime) > _timeout)
                    {
                        return false;
                    }
                }
                catch
                {
                    //ignore errors and keep attacking
                }
            }

            return true;

        }
    }
}
