using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using D3;

using rndWalker.Common;

namespace rndWalker.Bots
{
    class a4chest : Bot
    {
        public bool DO_VASES = true;

        public a4chest()
        {
        }

        public static void I_Interact(Unit u)
        {
            Me.UsePower(u.Type == UnitType.Gizmo || u.Type == UnitType.Item ? SNOPowerId.Axe_Operate_Gizmo : SNOPowerId.Axe_Operate_NPC, u);
        }


        override public void Execute()
        {
            if ((uint)Me.LevelArea != 0x301ED)// || (uint)Me.QuestId != 0x001C0D5 || Me.QuestStep != 24)
            {
               // changeQuest(24);
                //Thread.Sleep(1000);
                startGame();
                Thread.Sleep(2000);
            }
            Game.Print("Going to waypoint");
            Unit waypoint = Unit.Get().Where(x => x.Name.Contains("Waypoint")).FirstOrDefault();
            I_Interact(waypoint);
            I_Interact(waypoint);
            //Game.Print("Interacted to waypoint");
            //var asdf = UIElement.Get().FirstOrDefault(x => x.Name.Contains("3.wrapper.button"));
            //asdf.Click();

            //interact("Waypoint",true);
            //Thread.Sleep(250);
            // 28206C7C: 51F9617972AEB7B4 Root.NormalLayer.waypoints_dialog_mainPage.tree.stackpanel.folder 1.stackpanel.entry 3.wrapper.button (Visible: True)
          
            //useWaypoint(3); // works, but slow
            //UIElement.Get(0x51F9617972AEB7B4).Click();
            waitForArea(0x01ABCA);

            walk(1018, 341, true);
            walk(1037, 342, true);
            walk(1044, 348, true);

            // is there a chest?
            var chest = Unit.Get().FirstOrDefault(u => u.Name.Equals("Chest") && GetDistance(u.X, u.Y) < 100);
            if (chest != default(Unit))
            {
                interact(chest, true);
                Thread.Sleep(500);
                SnagIt.SnagItems();
                Thread.Sleep(800);
            }

            if (DO_VASES)
            {
                Game.Print("doing vases...");
                var vases = Unit.Get().Where(u => u.Name.Equals("Vase") && GetDistance(u.X, u.Y) < 100).ToArray();
                Game.Print(string.Format("found: {0}",vases.Length));
                killThese(vases);
//                Thread.Sleep(300);
                SnagIt.SnagItems();

            }
            Thread.Sleep(1000);
            ExitGame();
        }
    }
}
