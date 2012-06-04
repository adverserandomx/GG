using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using D3;

namespace Grind.Common
{
    public class Spell
    {
        protected bool available = false;
        protected bool forced = false;
        protected int lastTick = System.Int32.MinValue;
        protected SNOPowerId id;
        protected int cooldown = -1;
        protected int primCost = 0;
        protected int secCost = 0;

        public Spell(SNOPowerId _id, int _cooldown, int _cost1, int _cost2, bool _available)
        {
            id = _id;
            cooldown = _cooldown;
            primCost = _cost1;
            secCost = _cost2;
            available = _available;
            forced = true;
            init();
        }

        public Spell(SNOPowerId _id, int _cooldown, int _cost1, int _cost2)
        {
            id = _id;
            cooldown = _cooldown;
            primCost = _cost1;
            secCost = _cost2;
            init();
        }

        public void init()
        {
            if (!forced)
                available = Me.Skills.Contains(id);
        }

        public bool use(Unit _target)
        {
            if (available && Me.PrimaryResource >= primCost && Me.SecondaryResource >= secCost && System.Environment.TickCount > lastTick + cooldown * 1000 + 100)
            {
                Me.UsePower(id, _target);
                lastTick = System.Environment.TickCount;
                Thread.Sleep(75);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool use(float _x, float _y)
        {

            if (available && System.Environment.TickCount > lastTick + cooldown * 1000 + 100)
            {
                Me.UsePower(id, _x, _y, Me.Z);
                lastTick = System.Environment.TickCount;
                Thread.Sleep(75);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool use()
        {
            if (available && System.Environment.TickCount > lastTick + cooldown * 1000 + 100)
            {
                Me.UsePower(id);
                lastTick = System.Environment.TickCount;
                Thread.Sleep(75);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
