using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Grind.Common;
using D3;

namespace Grind.Classes {
    public static class DemonHunter {
        public static Spell hungeringArrow = new Spell(SNOPowerId.DemonHunter_HungeringArrow, 0, 0, 0, true);
        public static Spell entanglingShot = new Spell(SNOPowerId.DemonHunter_EntanglingShot, 0, 0, 0, true);
        public static Spell mulitShot = new Spell(SNOPowerId.DemonHunter_Multishot,0, 40, 0, true);
        public static Spell impale = new Spell(SNOPowerId.DemonHunter_Impale, 15, 10, 0, true);

        public static Spell potion = new Spell(SNOPowerId.Axe_Operate_Gizmo, 30, 0, 0, true);

        public static void init() {
            hungeringArrow.init();
            impale.init();
        }

        public static bool AttackUnit(Unit _unit, TimeSpan _timeout) {
            if (_unit.Life <= 0) {
                return false;
            }

            TimeSpan startTime = TimeSpan.FromTicks(System.Environment.TickCount);

            while (_unit.Life > 0) {

                if (1.0 * Me.Life / Me.MaxLife < 0.4) {
                    potion.use(Unit.Get().First(i => i.Name.Contains("Health Potion") && i.ItemContainer == Container.Inventory));
                }

                entanglingShot.use(_unit);
                mulitShot.use(_unit);
                //if (!impale.use(_unit))
                //    hungeringArrow.use(_unit);

                Thread.Sleep(200);

                if (TimeSpan.FromTicks(System.Environment.TickCount).Subtract(startTime) > _timeout) {
                    return false;
                }
            }

            return true;
        }
    }
}
