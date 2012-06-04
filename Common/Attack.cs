﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using D3;

namespace Grind.Common
{
    public static class Attack
    {
        private static TimeSpan defaultTimeout = TimeSpan.FromSeconds(20);

        public delegate bool UnitCheckCallback(Unit unit);

        static public void init()
        {
            Classes.Barbarian.init();
            Classes.WitchDoctor.init();
            Classes.Wizard.init();
            Classes.Monk.init();
            Classes.DemonHunter.init();
        }

        static public bool AttackUnit(Unit unit, TimeSpan timeout)
        {
            if (unit.Type == UnitType.Monster
                && unit.GetAttributeInteger(UnitAttribute.Is_NPC) == 0
                && unit.GetAttributeInteger(UnitAttribute.Is_Helper) == 0
                && unit.GetAttributeInteger(UnitAttribute.Invulnerable) == 0)
            {
                switch(Me.SNOId)
                {
                    case SNOActorId.Barbarian_Male:
                    case SNOActorId.Barbarian_Female:
                        return Classes.Barbarian.AttackUnit(unit, timeout);
                    case SNOActorId.WitchDoctor_Male:
                    case SNOActorId.WitchDoctor_Female:
                        return Classes.WitchDoctor.AttackUnit(unit, timeout);
                    case SNOActorId.Wizard_Male:
                    case SNOActorId.Wizard_Female:
                        return Classes.Wizard.AttackUnit(unit, timeout);
                    case SNOActorId.Demonhunter_Male:
                    case SNOActorId.Demonhunter_Female:
                        return Classes.DemonHunter.AttackUnit(unit, timeout);
                    case SNOActorId.Monk_Male:
                    case SNOActorId.Monk_Female:
                        return Classes.Monk.AttackUnit(unit, timeout);
                    default:
                        break;
                }
            }
            return false;
        }

        static public bool AttackUnit(Unit unit)
        {
            return AttackUnit(unit, defaultTimeout);
        }
    }
}
