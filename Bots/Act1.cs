/*
 * Make sure you start your character in Act 1 Quest3, Northwest Gate
 * go to the gate west of Old Ruins waypoint and make sure you get the checkpoint
 */ 
#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using D3;

using Grind.Common;

namespace Grind.Bots
{
    public class Act1GoldSarkoth : BaseBot
    {
  
        public Act1GoldSarkoth()
        {

        }

        public override void Execute()
        {
            Thread.Sleep(2000); 
            //if you set this sleep too low, the inventory detection will fail in RepairAndSell();

            try
            {
                if (Game.Ingame)
                    this.repairAndSell();
            }
            catch (Exception)
            {
                ExitGame("Repair and Sell Exception");
                return;
            }

            #region dummycheck
            //if ((uint)Me.LevelArea != 0x163FD || Me.X > 1992 || Me.X < 1991 || Me.Y < 2653 || Me.Y > 2654)
            //{
            //    Game.Print("You must be in Act1, Quest3, Northwest Gate.  You also must have checkpointed from the gate.");
            //    Game.Print("Please unload and reload when you have properly setup your character.");
            //    Thread.Sleep(10000);
            //    BotHelper.ExitGame();
            //}
            #endregion //todo:

            /* Find Cellar */
            //make sure we are in game otherwise don't do any of the stuff below
            try // hack. if this check fails, the cellar isn't there.  I need to properly learn how to detect this condition.. but i'm lazy
            {
                if (!Game.Ingame)
                    return;
                BotHelper.walk(1995, 2603, true);
                //Me.UsePower(SNOPowerId.DemonHunter_SmokeScreen);
                BotHelper.walk(2025, 2563, true);
                //Me.UsePower(SNOPowerId.DemonHunter_SmokeScreen);
                BotHelper.walk(2057, 2528, true);
                //Me.UsePower(SNOPowerId.DemonHunter_SmokeScreen);
                BotHelper.walk(2081, 2487, false);

                Game.Print("Checking for cellar presence");
                var cellar = Unit.Get().Where(u => u.Name.Contains("Dank Cellar")).FirstOrDefault();
                if (cellar != null)
                {
                    BotHelper.walk(2065, 2476, true);
                    BotHelper.interact(cellar, true);
                }
                else
                {
                    Game.Print("Exiting game can't find Cellar");
                    ExitGame("Exiting game can't find Cellar");
                    return;
                }

            }
            //sometimes when doing the linq query, an object reference not found exception is thrown.  
            //I'm not sure if this is something underlying with D3a.  We catch this so we don't have abnormal termination of the bot.
            catch (Exception) 
            {
                Game.Print("Exiting game can't find Cellar");
                ExitGame("Exiting game can't find Cellar");
                return;
            }

   
            /* Kill mobs in Cellar */
            try
            {
                if (!Game.Ingame)
                    return;
          
                //1900x1080 screen support
                //BotHelper.walk(108, 158, true);
                //BotHelper.walk(129, 143, true);
                //BotHelper.walk(122, 145, false);

                //1440x900 screen
                BotHelper.walk(123, 157, true);
                BotHelper.walk(120, 142, true);  // ranged chars will want to attack from here. Sarkoth will stand still as long as the toon does not enter the room
                BotHelper.walk(120, 137, true);
                BotHelper.killAll();
            }
            //again sometimes an exception gets thrown...
            catch (Exception)
            {
                try
                {
                    //try one more time
                    BotHelper.killAll();
                }
                catch (Exception e)
                {
                    Game.Print("Exception thrown when trying to kill mobs: " + e.Message);
                    ExitGame("Exiting due kill mob exception...");
                    throw (e);
                }
            }

            /* Get Loot */
            //it helps a ton to have lots of gold pickup yards in your items
            try
            {
                if (!Game.Ingame)
                    return;
                // walk and use pickup radius to get gold before snagging
                BotHelper.walk(137, 94, true);
                //i put this in to get the gold on the edge.  Sometimes SnagItems doesn't seem to get the full gold drops
                BotHelper.walk(95, 98, true);
                //BotHelper.walk(113, 102, true);
                SnagIt.SnagItems();
            }
            catch (Exception)
            {
                try
                {
                    //try again if it fails the first time
                    SnagIt.SnagItems();
                }
                catch (Exception e)
                {
                    Game.Print("Exception thrown when trying to snag loot: " + e.Message);
                    //Game.Print(e.StackTrace);
                    ExitGame("Exiting due to snag item exception");
                    throw (e);
                }
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
        /// Detect whether inventory is really full. The method of using inventory count is in accurate as there are a lot of one slot items that get picked up (gems, etc.)
        /// </summary>
        /// <returns></returns>
        private bool isInventoryFull()
        {
            int invLoad = 0;
            foreach (Unit item in Unit.Get().Where(u => u.ItemContainer == Container.Inventory))
            {
                //counting belts, rings and ammy's as two for now.. don't
                if (item.Name.Contains("Topaz")
                    || item.Name.Contains("Amethyst")
                    || item.Name.Contains("Emerald")
                    || item.Name.Contains("Ruby")
                    || item.Name.Contains("Book ")
                    || item.Name.Contains("Tome")
                    || item.Name.Contains("Mythic")
                    )
                {
                    invLoad++;
                }
                else
                {
                    invLoad += 2;
                }
            }
            Game.Print("Inventory Load: " + invLoad);
            if (invLoad >= 58) // this may need adjusting
                return true;

            else
                return false;
        }

        /// <summary>
        /// This function just encapsulates selling and repairing
        /// </summary>
        protected override void repairAndSell()
        {
            if (!Game.Ingame)
                return;

            if (BotHelper.needsRepair() == false && isInventoryFull() == false)
                return;

            if (BotHelper.needsRepair())
                Game.Print("Heading to town to repair.");

            if (isInventoryFull())
                Game.Print("No free stash space. Heading to town to sell.");

            Thread.Sleep(500);

            //selling
            try
            {
                BotHelper.GoTown();
                BotHelper.walk(2935, 2803, true);
                BotHelper.walk(2897, 2786, true);

                UIElement.Get(0xDAECD77F9F0DCFB).Click();

                //Thread.Sleep(1000);

                //foreach (Unit item in Unit.Get().Where(u => u.ItemContainer == Container.Inventory))
                //{

                //    /* Unit.IdemIdentified LIES! And then casting IdentifyWithCast doesn't work. Oh well.
                //     * 
                //     * if (item.ItemDescription[0].Contains("UNIDENTIFIED"))
                //    {
                //        Game.Print(string.Format("Identifying {0}", item.ItemDescription));
                //        Me.UsePower(SNOPowerId.IdentifyWithCast, item);
                //        Thread.Sleep(3000);
                //    }
                //     */
                //}

                // 0002B8E3: 00000001(00000000) # {c:ffffffff}Tashun the Miner{/c} = 0x2F424FF
                BotHelper.interact(Unit.Get().First(u => (uint)u.ActorId == 0x2B8E3), true);
                Thread.Sleep(500);

            }
            catch (Exception e)
            {
                Game.Print("Exception when selling: " + e.Message);
                
            }

            //repairing
            try
            {
                if (BotHelper.needsRepair())
                {
                    BotHelper.repairAll();
                    if (Me.Life <= .75 * Me.MaxLife)
                        ExitGame("Life is too small after repair");
                }

                foreach (Unit item in Unit.Get().Where(u => u.ItemContainer == Container.Inventory))
                {
                    if (item.ItemQuality >= UnitItemQuality.Magic1 && item.ItemQuality < UnitItemQuality.Legendary)
                    {
                        float wpMax = item.GetAttributeInteger(UnitAttribute.Damage_Weapon_Max_Total_All);
                        float wpMin = item.GetAttributeInteger(UnitAttribute.Damage_Weapon_Min_Total_All);
                        float mf = item.GetAttributeReal(UnitAttribute.Magic_Find);
                        float gf = item.GetAttributeReal(UnitAttribute.Gold_Find);


                        if (!(wpMax / wpMin >= 700 || mf >= 0.15 || gf >= 0.15))
                            item.SellItem();
                    }

                }

            }
            catch (Exception e)
            {
                Game.Print("Exception when repairing:  " + e.Message);
            }

            //townportal
            try
            {

                BotHelper.walk(2896, 2787, true);
                BotHelper.walk(2977, 2799, true);
                BotHelper.TakePortal();
            }
            catch (Exception e)
            {
                Game.Print("Exception when heading back to TP: " + e.Message);
            }
        }



    }
}