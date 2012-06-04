/*Majority of code was stolen from rndWalker courtesy of K_OS
 * http://www.blizzhackers.cc/viewtopic.php?f=237&t=489786
 * 
 * I've refactored a lot of the code out of the base bot class since they really are helper functions
 * 
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using D3;
using Grind.Common;

namespace Grind.Common
{
    public static class BotHelper
    {

        private static bool debug = false;

        public static bool signal = false;
        /// <summary>
        /// 
        /// 
        /// </summary>
        /// 

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void StartGame()
        {
            try
            {
                // 27DFC2E4: 51A3923949DC80B7 Root.NormalLayer.BattleNetCampaign_main.LayoutRoot.Menu.PlayGameButton (Visible: True)
                // it's both the resume and the start game button
                UIElement resumeGame = UIElement.Get(0x51A3923949DC80B7);

                while (!Game.Ingame)
                {
                    if (resumeGame != null)
                    {
                        if (signal == false)
                        {
                            resumeGame.Click();
                            signal = true;
                        }

                        for (int i = 0; i < 2 && !Game.Ingame; i++)
                        {
                            Thread.Sleep(500);
                        }
                    }
                    break;
                }

                //don't need this now that I have things synchronizing properly
                //Thread.Sleep(2000);
            }
            catch (Exception e)
            {
                Game.Print("Exception when starting game: " + e.Message);
            }
        }

        /// <summary>
        /// Exits game, will idle until the game exits
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void ExitGame()
        {
            try
            {
                if (signal == false)
                {
                    if (Game.Ingame)
                    {
                        UIElement ui = UIElement.Get((ulong)D3UIButton.ExitButton);
                        if (ui != null)
                        {
                            ui.Click();
                            signal = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Game.Print("Exception when exiting: " + e.Message);
                throw e;
            }
        }

        public static bool GoTown()
        {
            if (Me.InTown == true) { return true; }

            if (Me.UsePower(SNOPowerId.UseStoneOfRecall) == true)
            {
                Thread.Sleep(550);

                while (Me.Mode == UnitMode.Casting
                        || Me.Mode == UnitMode.Warping)
                {
                    Thread.Sleep(250);
                }

                if (Me.InTown == true)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool TakePortal()
        {
            if (Me.InTown == false) { return false; }

            Unit[] units = Unit.Get();

            var unit = (from u in units where u.Type == UnitType.Gizmo && u.ActorId == SNOActorId.hearthPortal select u).FirstOrDefault();

            if (unit == default(Unit))
            {
                return false;
            }

            for (int i = 0; i < 3; i++)
            {
                Me.UsePower(SNOPowerId.Axe_Operate_Gizmo, unit);
                Thread.Sleep(500);

                if (Me.InTown == false)
                {
                    break;
                }
            }

            return Me.InTown == false;
        }

        /// <summary>
        /// Detect whether inventory is really full. The method of using inventory count is in accurate as there are a lot of one slot items that get picked up (gems, etc.)
        /// </summary>
        /// <returns></returns>
        public static bool isInventoryFull()
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
        public static bool needsRepair()
        {
            // 1EFC3224: 0xBD8B3C3679D4F4D9 Root.NormalLayer.DurabilityIndicator (Visible: True)
            var indicator = UIElement.Get((ulong)D3UIButton.RepairIndicator);
            return (indicator != default(UIElement) && indicator.Visible);
        }


        public static void CloseInventory()
        {
            // 303EF3AC: 368FF8C552241695 Root.NormalLayer.inventory_dialog_mainPage.inventory_button_exit (Visible: True)
            var btn = UIElement.Get((ulong)D3UIButton.CloseInventory); //0x368FF8C552241695
            if (btn.Visible)
                btn.Click();
        }

        /// <summary>
        /// clicks the revive button once per second. does not handle the popups yet
        /// </summary>
        public static void revive()
        {
            // 20DDD3F4: BFAAF48BA9316742 Root.NormalLayer.deathmenu_dialog.dialog_main.button_revive_at_checkpoint (Visible: True)
            var btn = UIElement.Get((ulong)D3UIButton.ReviveButton);
            while (btn != default(UIElement) && btn.Visible)
            {
                btn.Click();
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// buy window needs to be open. repair tab needs not be selected though
        /// </summary>
        public static void repairAll()
        {
            // 1EFC12BC: 0x80F5D06A035848A5 Root.NormalLayer.shop_dialog_mainPage.repair_dialog.RepairAll (Visible: True)
            var btn = UIElement.Get((ulong)D3UIButton.RepairAll);
            if (btn != default(UIElement))
            {
                btn.Click();
            }
        }

        /// <summary>
        /// skips conversation pc with npc
        /// </summary>
        public static void skipConversation()
        {
            // 1F05DE84: 942F41B6B5346714 Root.NormalLayer.conversation_dialog_main.button_close (Visible: True)
            var advKey = UIElement.Get((ulong)D3UIButton.SkipConversationPCtoNPC);
            while (advKey.Visible)
            {
                advKey.Click();
                Thread.Sleep(45);
            }
        }


        public static Unit[] waitForMobs(uint _timeout)
        {
            var mobs = Unit.Get().Where(x => x.Type == UnitType.Monster && ((uint)x.MonsterType == 0 || (uint)x.MonsterType == 4) && x.Mode != UnitMode.Warping && x.Life > 0
                && x.GetAttributeInteger(UnitAttribute.Is_NPC) == 0 && x.GetAttributeInteger(UnitAttribute.Is_Helper) == 0
                && x.GetAttributeInteger(UnitAttribute.Invulnerable) == 0).ToArray();
            uint count = 0;
            while (mobs.Length <= 0 && count < 2 * _timeout)
            {
                Thread.Sleep(500);
                mobs = Unit.Get().Where(x => x.Type == UnitType.Monster && ((uint)x.MonsterType == 0 || (uint)x.MonsterType == 4) && x.Mode != UnitMode.Warping && x.Life > 0
                    && x.GetAttributeInteger(UnitAttribute.Is_NPC) == 0 && x.GetAttributeInteger(UnitAttribute.Is_Helper) == 0
                    && x.GetAttributeInteger(UnitAttribute.Invulnerable) == 0).ToArray();
                ++count;
            }
            return mobs;
        }

        public static Unit[] waitForMobs(uint _timeout, float maxDist)
        {
            var mobs = Unit.Get().Where(x => x.Type == UnitType.Monster && ((uint)x.MonsterType == 0 || (uint)x.MonsterType == 4) && x.Mode != UnitMode.Warping && x.Life > 0
                && x.GetAttributeInteger(UnitAttribute.Is_NPC) == 0 && x.GetAttributeInteger(UnitAttribute.Is_Helper) == 0
                && x.GetAttributeInteger(UnitAttribute.Invulnerable) == 0
                && GetDistance(x.X, x.Y, Me.X, Me.Y) < maxDist).ToArray();
            uint count = 0;
            while (mobs.Length <= 0 && count < 2 * _timeout)
            {
                Thread.Sleep(500);
                mobs = Unit.Get().Where(x => x.Type == UnitType.Monster && ((uint)x.MonsterType == 0 || (uint)x.MonsterType == 4) && x.Mode != UnitMode.Warping && x.Life > 0
                    && x.GetAttributeInteger(UnitAttribute.Is_NPC) == 0 && x.GetAttributeInteger(UnitAttribute.Is_Helper) == 0
                    && x.GetAttributeInteger(UnitAttribute.Invulnerable) == 0).ToArray();
                ++count;
            }
            return mobs;
        }

        /// <summary>
        /// buggy?
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_timeout"></param>
        /// <returns></returns>
        public static Unit waitForUnit(string _name, uint _timeout)
        {
            var portal = Unit.Get().Where(x =>/* x.Type == UnitType.Gizmo && */x.Name.Contains(_name)).ToArray();

            uint count = 0;
            while (portal.Length <= 0 && count < 2 * _timeout)
            {
                Thread.Sleep(500);
                portal = Unit.Get().Where(x =>/* x.Type == UnitType.Gizmo && */x.Name.Contains(_name)).ToArray();
                ++count;

            }
            return portal.FirstOrDefault();
        }

        public static void waitForArea(uint _lvlArea)
        {
            while ((uint)Me.LevelArea != _lvlArea)
            {
                Thread.Sleep(567);
            }
        }

        public static void killThese(Unit[] _units)
        {
            _units = _units.OrderBy(u1 => GetDistance(u1.X, u1.Y, Me.X, Me.Y)).ToArray();
            for (uint i = 0; i < _units.Length; ++i)
            {
                if (_units[i].Valid && _units[i].Life > 0)
                {
                    Attack.AttackUnit(_units[i]);
                    Thread.Sleep(154);
                }
            }
        }

        //public static void killThese(Unit[] _units, float maxDist)
        //{
        //    _units = _units.OrderBy(u1 => GetDistance(u1.X, u1.Y, Me.X, Me.Y)).ToArray();
        //    for (uint i = 0; i < _units.Length; ++i)
        //    {
        //        if (GetDistance(_units[i].X, _units[i].Y, Me.X, Me.Y) < maxDist)
        //            continue;
        //        if (_units[i].Valid && _units[i].Life > 0)
        //        {
        //            Attack.AttackUnit(_units[i]);
        //            Thread.Sleep(154);
        //        }
        //    }
        //}

        public static void killAll()
        {
            var mobs = waitForMobs(0);
            while (mobs.Length > 0)
            {
                killThese(mobs);
                mobs = waitForMobs(0);
            }
        }
        
        /// <summary>
        /// Kill all mobs within a distance.
        /// </summary>
        /// <param name="distX"></param>
        /// <param name="distY"></param>
        public static void killAll(float maxDist)
        {
            var mobs = waitForMobs(0, maxDist);
            while (mobs.Length > 0)
            {
                killThese(mobs);
                mobs = waitForMobs(0, maxDist);
            }
        }


        public static float GetDistance(float x, float y)
        {
            return GetDistance(x, y, Me.X, Me.Y);
        }

        public static float GetDistance(float x, float y, float x2, float y2)
        {
            return (float)Math.Sqrt((x - x2) * (x - x2) + (y - y2) * (y - y2));
        }

        /*// <summary>
   /// dont use! buggy
   /// </summary>
   public static void skipNpcConversation() {
       // 2D3E702C: C06278A08ADCF3AA Root.NormalLayer.playinghotkeys_dialog_backgroundScreen.playinghotkeys_conversation_advance (Visible: True)
       var advKey = UIElement.Get(0xC06278A08ADCF3AA);
       while (advKey.Visible) {
           advKey.Click();
           Thread.Sleep(45);
       }
   }*/

        /// <summary>
        /// changes quest to the one with the given index. may be slow so try to give the button hash in advance
        /// </summary>
        /// <param name="_listIndex"></param>
        public static void changeQuest(uint _listIndex)
        {

            changeQuest(_listIndex, 0);
        }

        public static void changeQuest(uint _listIndex, ulong _btn)
        {
            if (Game.Ingame)
            {
                BotHelper.ExitGame();
                while (Game.Ingame)
                {
                    Thread.Sleep(687);
                }
            }
            // 23E844FC: C4A9CC94C0A929B Root.NormalLayer.BattleNetCampaign_main.LayoutRoot.Menu.ChangeQuestButton (Visible: True)
            UIElement changeQuest = UIElement.Get(0xC4A9CC94C0A929B);
            changeQuest.Click();
            Thread.Sleep(469);

            var button = default(UIElement);

            if (_btn == 0)
            {
                button = UIElement.Get().Where(x => x.Name.EndsWith(string.Format("_item{0}", _listIndex))).FirstOrDefault();
            }
            else
            {
                button = UIElement.Get(_btn);
            }
            button.Click();
            Thread.Sleep(478);


            // 2BBDBAC4: 1AE2209980AAEA69 Root.NormalLayer.BattleNetQuestSelection_main.LayoutRoot.OverlayContainer.SelectQuestButton
            UIElement.Get(0x1AE2209980AAEA69).Click(); // select quest
            Thread.Sleep(730);

            // 2440DBEC: B4433DA3F648A992 Root.TopLayer.BattleNetModalNotifications_main.ModalNotification.Buttons.ButtonList.OkButton (Visible: True)
            var ok = UIElement.Get(0xB4433DA3F648A992);
            if (ok != null && ok.Visible)
            { // unfortunately this is always true...
                ok.Click();
            }

            Thread.Sleep(1000);
        }

        public static void SkipSequence()
        {
            UIElement skipSequence = UIElement.Get(0x2289FE26DA955A81);
            UIElement confirmButton = UIElement.Get(0x891D21408238D18E);

            for (int i = 0; i < 10 && skipSequence.Visible == false; i++)
            {
                Thread.Sleep(500);
            }

            if (skipSequence.Visible == false)
            {
                throw new Exception("Skip Sequence UIElement is not visible!");
            }

            skipSequence.Click();

            for (int i = 0; i < 10 && confirmButton.Visible == false; i++)
            {
                Thread.Sleep(500);
            }

            if (skipSequence.Visible == false)
            {
                throw new Exception("Confirm Button UIElement is not visible!");
            }

            confirmButton.Click();

            for (int i = 0; i < 10 && confirmButton.Visible == true; i++)
            {
                Thread.Sleep(500);
            }

            if (confirmButton.Visible == true)
            {
                throw new Exception("Confirm Button UIElement is still visible!");
            }

            // Wait out sequence post effect..
            Thread.Sleep(5500);
        }

        public static bool walk(float _x, float _y)
        {
            return walk(_x, _y, false, 0);
        }

        /// <summary>
        /// walk to given (x,y) position.
        /// </summary>
        /// <param name="_x">X</param>
        /// <param name="_y">Y</param>
        /// <param name="_waitTillThere">whether to wait until toon is there</param>
        /// <returns></returns>
        public static bool walk(float _x, float _y, bool _waitTillThere)
        {
            return walk(_x, _y, _waitTillThere, 3);
        }

        public static bool walk(float _x, float _y, bool _waitTillThere, uint _countOut)
        {
            Game.Print(String.Format("Walking: {0}, {1}", _x, _y));
            Me.UsePower(SNOPowerId.Walk, _x, _y, Me.Z);
            uint count = 0;
            if (_waitTillThere)
            {
                while (BotHelper.GetDistance(_x, _y) > 10)
                {
                    if (Me.Mode != UnitMode.Running)
                    {
                        Me.UsePower(SNOPowerId.Walk, _x, _y, Me.Z);
                        if (count++ >= _countOut)
                        {
                            return false;
                        }
                    }
                    Thread.Sleep(100);
                }
            }
            return true;
        }

        public static void interact(Unit _u, bool _walkThere)
        {
            if (_u.Type == UnitType.Gizmo
                || _u.Type == UnitType.Monster
                || _u.Type == UnitType.Item)
            {
                if (_walkThere)
                {
                    walk(_u.X, _u.Y, true);
                    Thread.Sleep(300);
                }
                Me.UsePower(_u.Type == UnitType.Monster ? SNOPowerId.Axe_Operate_NPC : SNOPowerId.Axe_Operate_Gizmo, _u);
                if (_walkThere)
                {
                    Thread.Sleep(300);
                }
            }
        }

        public static void interact(string _name, bool _walkThere)
        {
            var unit = Unit.Get().Where(x => x.Name.Contains(_name)).FirstOrDefault();
            interact(unit, _walkThere);
            Thread.Sleep(213);
        }

        public static void clickUI(string _name, bool _visible)
        {
            var e = UIElement.Get().FirstOrDefault(x => x.Name.Contains(_name));
            e.Click();
        }

        /// <summary>
        /// waypoint menu needs to be open. if unsure use 'interact("Waypoint");' before this
        /// </summary>
        /// <param name="_listpos"></param>
        public static void useWaypoint(uint _listpos)
        {
            clickUI(string.Format("{0}.wrapper.button", _listpos), true);
        }

        //TODO: need to implement a version of these that movesunti you arrive
        public static bool MoveTo(float x, float y)
        {
            if (debug)
                Game.Print("Moving to " + x + ", " + y);
            if ((float)Math.Sqrt(Math.Pow(Me.X - x, 2) + Math.Pow(Me.Y - y, 2)) < 5)
                return true;
            Me.UsePower(SNOPowerId.Walk, x, y, Me.Z);
            return false;
        }

        public static bool MoveTo(string name)
        {
            var u = Unit.Get().Where(x => x.Name.Contains(name)).FirstOrDefault();
            if (!u.Name.Contains(name))
                return false;
            if (debug)
                Game.Print("Moving to " + u.Name);
            if ((float)Math.Sqrt(Math.Pow(Me.X - u.X, 2) + Math.Pow(Me.Y - u.Y, 2)) < 5)
                return true;
            Me.UsePower(SNOPowerId.Walk, u);
            return false;
        }

        public static bool MoveTo(Unit u)
        {
            if (debug)
                Game.Print("Moving to " + u.Name);
            if ((float)Math.Sqrt(Math.Pow(Me.X - u.X, 2) + Math.Pow(Me.Y - u.Y, 2)) < 5)
                return true;
            Me.UsePower(SNOPowerId.Walk, u);
            return false;
        }
    }
}
