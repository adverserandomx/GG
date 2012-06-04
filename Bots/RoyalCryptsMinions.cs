using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using D3;

using rndWalker.Common;

namespace rndWalker.Bots
{
    class RoyalCryptsMinions : Bot
    {
        public RoyalCryptsMinions()
        {
        }

        override public void Execute()
        {
            startGame();
            if (needsRepair())
            {
                Thread.Sleep(3000);
                GoTown();
                walk(2935, 2803, true);
                walk(2897, 2786, true);
                // 0002B8E3: 00000001(00000000) # {c:ffffffff}Tashun the Miner{/c} = 0x2F424FF
                interact(Unit.Get().First(u => (uint)u.ActorId == 0x2B8E3), true);
                Thread.Sleep(500);
                repairAll();
                walk(2896, 2787, true);
                walk(2977, 2799, true);
                TakePortal();
            }

            if ((uint)Me.LevelArea != 0x163FD || Me.X > 1992 || Me.X < 1991 || Me.Y < 2653 || Me.Y > 2654)
            {
                if (Game.Ingame)
                {
                    ExitGame();
                    while (Game.Ingame) Thread.Sleep(583);
                }
                startGame();
                while (!Game.Ingame) Thread.Sleep(612);
                if ((uint)Me.LevelArea != 0x163FD)
                {
                    Thread.Sleep(3000);
                }
            }
            walk(1995, 2603, true);
            Me.UsePower(SNOPowerId.DemonHunter_SmokeScreen);
            walk(2025, 2563, true);
            Me.UsePower(SNOPowerId.DemonHunter_SmokeScreen);
            walk(2057, 2528, true);
            Me.UsePower(SNOPowerId.DemonHunter_SmokeScreen);
            walk(2081, 2487, false);
            var cellar = Unit.Get().FirstOrDefault(u => u.Name.Contains("Dank Cellar"));
            if (cellar == default(Unit))
            {
                ExitGame();
                while (Game.Ingame) Thread.Sleep(527);
                return;
            }
            Me.UsePower(SNOPowerId.DemonHunter_SmokeScreen);
            walk(2081, 2487, true);
            Me.UsePower(SNOPowerId.DemonHunter_Preparation);
            walk(2066, 2477, true);
            Me.UsePower(SNOPowerId.DemonHunter_Companion);

            interact(cellar, true);
            Me.UsePower(SNOPowerId.DemonHunter_SmokeScreen);
            walk(108, 158, true);
            walk(129, 143, true);
            // ranged chars will want to attack from here. Sarkoth will stand still as long as the toon does not enter the room
            walk(120, 109, false);
            killAll();
            Me.UsePower(SNOPowerId.DemonHunter_SmokeScreen);
            SnagIt.SnagItems();
            killAll(); //?!
            SnagIt.SnagItems();
            ExitGame();
            Thread.Sleep(200);
        }
    }
}