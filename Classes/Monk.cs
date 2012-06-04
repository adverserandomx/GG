using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using D3;
using Grind.Common;

namespace Grind.Classes {
    public static class Monk {
        public static Spell mantraOfHealing = new Spell(SNOPowerId.Monk_MantraOfHealing, 60, 50, 0, true); // for some reason mantras are not found
        public static Spell mantraOfEvasion = new Spell(SNOPowerId.Monk_MantraOfEvasion, 60, 50, 0); // and thus have to be set manually....
        public static Spell blindingFlash = new Spell(SNOPowerId.Monk_BlindingFlash, 15, 10, 0);
        public static Spell breathOfHeaven = new Spell(SNOPowerId.Monk_BreathOfHeaven, 15, 25, 0);
        public static Spell serenity = new Spell(SNOPowerId.Monk_Serenity, 20, 10, 0);
        public static Spell sevenSidedStrike = new Spell(SNOPowerId.Monk_SevenSidedStrike, 30, 50, 0);
        public static Spell wayOfTheHundredFists = new Spell(SNOPowerId.Monk_WayOfTheHundredFists, 0, 0, 0);
        public static Spell potion = new Spell(SNOPowerId.Axe_Operate_Gizmo, 30, 0, 0, true);

        public static void init() {
            mantraOfHealing.init();
            mantraOfEvasion.init();
            blindingFlash.init();
            breathOfHeaven.init();
            serenity.init();
            sevenSidedStrike.init();
            wayOfTheHundredFists.init();
        }

        public static bool AttackUnit(Unit _unit, TimeSpan _timeout) {
            if (_unit.Life <= 0) {
                return false;
            }

            TimeSpan startTime = TimeSpan.FromTicks(System.Environment.TickCount);

            mantraOfHealing.use(_unit);
            mantraOfEvasion.use(_unit);

            while (_unit.Life > 0) {
                if (Me.MaxLife - Me.Life > 10000) {
                    breathOfHeaven.use();
                }
                if (1.0 * Me.Life / Me.MaxLife < 0.4) {
                    if (!breathOfHeaven.use() && !serenity.use() && !sevenSidedStrike.use(_unit))
                        if (1.0 * Me.Life / Me.MaxLife < 0.3)
                            potion.use(Unit.Get().First(i => i.Name.Contains("Health Potion") && i.ItemContainer == Container.Inventory)); // doesn't work?!
                }

                blindingFlash.use();
                if (!sevenSidedStrike.use(_unit))
                    wayOfTheHundredFists.use(_unit);

                Thread.Sleep(200);

                if (TimeSpan.FromTicks(System.Environment.TickCount).Subtract(startTime) > _timeout) {
                    return false;
                }
            }

            return true;
        }
    }
}
