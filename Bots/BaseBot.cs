using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using D3;
using Grind.Common;

namespace Grind.Bots
{
    public abstract class BaseBot
    {
        abstract public void Execute();

        /// <summary>
        /// implement this method to handle repairing and selling. This will vary depending on what act you are in
        /// </summary>
        abstract protected void repairAndSell();
    }
}
