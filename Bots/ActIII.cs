using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using D3;

using Grind.Common;

namespace Grind.Bots
{
    public class Act3XPSkycrown : BaseBot
    {

       
        public Act3XPSkycrown()
        {

        }

        public override void Execute()
        {
            /* Act III*/
            if (!Game.Ingame)
                return;

            Thread.Sleep(1500);
            //TODO: need to figure out right delay before needsRepair returns the right value.
            Thread.Sleep(5000);

            try
            {
                //if (BotHelper.needsRepair())
                //{
                   //repairAndSell();
                //}
                //else 
                //{
                    //walk(3810, 3706, true);              
                    BotHelper.walk(3815, 3781, true);
                    //BotHelper.killAll(40);
                    BotHelper.walk(3810, 3706, true);
                    Thread.Sleep(500);
                //}
            }
            catch (Exception e)
            {
                Game.Print("Exception thrown: " + e.Message);
                //ExitGame("Exiting due kill mob exception...");
                throw (e);
            }

        }

        /// <summary>
        /// Helper function to encapsulate exiting the game and then sleeping for a set time
        /// </summary>
        public static void ExitGame(string reason)
        {
            Worker.ExitReason = reason;
            BotHelper.ExitGame();
            //Thread.Sleep(3000);
        }

        /// <summary>
        /// This function just encapsulates selling and repairing
        /// </summary>
        protected override void repairAndSell()
        {

            BotHelper.GoTown();

            BotHelper.walk((float)441.93, (float)323.69, true);
            //crashes will null exception all the time
            //while (BotHelper.MoveTo("Vidar") == false)
            //{
            //    Thread.Yield();
            //}
            BotHelper.interact("Vidar", true);
            BotHelper.repairAll();
           
            //UIElement element = UIElement.Get().Where(u => u.Name.Contains("shop_dialog_mainPage")).FirstOrDefault();
            //if (element == default(UIElement)) return;
            //UIElement.Get().Where(u => u.Name.Contains("repair_dialog.RepairAll")).First().Click();
            //NeedRepair = false;
             

        }

    }
}