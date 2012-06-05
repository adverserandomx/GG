/*Based on rndWalker code from K_OS
 * http://www.blizzhackers.cc/viewtopic.php?f=237&t=489786
 * 
 * And modifications from nupogodi.
 * 
 * 
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using D3;
using Grind.Bots;
using Grind.Common;

namespace Grind {
    
    public class Worker 
    {
        //public const string verString = "1.6";
        public static Worker Instance { get; set; }
        public static bool Ingame
        {
            get
            {
                return Game.Ingame;
            }
        }
        public static string ExitReason { get; set; }

        private Thread thread;

        private int gameCounter = 0;
        private BaseBot bot;
        private int _frameCount = 0;

        private bool exceptionError = false;

        private bool deathSignal = false;
        private AutoResetEvent resetEvent;

        public Worker() {
            this.thread = new Thread(this.DoWork);
            resetEvent = new AutoResetEvent(true);


            Game.OnDrawEvent += new DrawEventHandler(Game_OnDrawEvent);
            Game.OnTickEvent += new TickEventHandler(Game_OnTickEvent);
            Game.OnGameLeaveEvent += new GameLeaveEventHandler(Game_OnGameLeaveEvent);
            Game.OnGameEnterEvent += new GameEnterEventHandler(Game_OnGameEnterEvent);
        }

        void Game_OnTickEvent(EventArgs e)
        {
            //using signal to handle race conditions
            if (deathSignal == false && Game.Ingame)
            {
                if (Me.WorldId !=0 && Me.Life == 0) //Me.WorldId makes sure you are in an actual game world...
                {
                    try
                    {
                        BotHelper.ExitGame();
                        ExitReason = "Exiting game due to death";
                        resetEvent.Reset();
                        // hard reset
                        //Worker.Instance.Restart(); //horrible thread abort
                        deathSignal = true; //make sure OnTickEvent doesn't call this twice.
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Only do DEBUGING Draw's here nothing else!! Race issues will follow...
        /// </summary>
        /// <param name="e"></param>
        void Game_OnDrawEvent(EventArgs e)
        {
            _frameCount++;
            if (Game.Ingame)
            {
                Draw.DrawText(10.0f, 10.0f, 0x16A, 0x16, 0xFFFFFFFF, String.Format("{0} - Area: {1}; Me Pos: {2}, {3}", Assembly.GetExecutingAssembly().GetName().Version, Me.LevelArea, Me.X, Me.Y));
            }
        }
        void Game_OnGameEnterEvent(EventArgs e)
        {
            deathSignal = false; //reset signal for handling actions in OnTickEvent
            resetEvent.Set(); //thread should continue
            BotHelper.signal = false; //reset signal for end game
        }

        void Game_OnGameLeaveEvent(EventArgs e)
        {
            resetEvent.Reset();
            BotHelper.signal = false; //reset signal for create game
            Game.Print(ExitReason);
            Logger.Log(ExitReason);
        }

        /// <summary>
        /// TODO: This method is for recoverign from fatal exceptions.  Not used anywhere now.
        /// </summary>
        private void HandleExceptionError()
        {
            //moved thread restart out of execution.  If there is ever an exception and things fail, we will restart after a 3 second delay
            if (exceptionError)
            {
                Game.Print("Restarting in...");
                for (int i = 3; i >= 0; i++)
                {
                    Thread.Sleep(1000);
                    Game.Print(i.ToString());
                }
                // hard reset
                //GrindGold.Instance.Restart();
                Logger.Log("Restarted bot due to exception at " + DateTime.Now);
            }
        }

        public void Start() {
            this.thread.Start();
        }

        public void Stop() {
            try
            {
                this.thread.Abort();
            }
            catch (Exception e)
            {
                Logger.Log(e, "Thread Abort Exception");
            }
        }

        //public void Restart() {
        //    this.Stop();
        //    try
        //    {
        //        this.thread = new Thread(this.ExecutingThread);
        //        this.thread.Start();
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Log(e, "Thread Restart Exception");
        //    }
        //}

        private void DoWork() 
        {
            //Thread.Sleep(500);
            
            //bot = new Act1GoldSarkoth();
            bot = new Act3XPSkycrown();

            //start game
            //execute logic
            //endgame


            while (true)
            {
                int start = System.Environment.TickCount;
   
                try
                {
                    BotHelper.StartGame();
                    resetEvent.Reset(); //thread should block on this call

                    if (Game.Ingame)
                    {
                        bot.Execute();
                        int diff = System.Environment.TickCount - start;

                        gameCounter++;
                        Game.Print(string.Format("Run took {0} seconds. Total Runs: {1}", diff / 1000, gameCounter));
                        Logger.Log(string.Format("Run took {0} seconds. Total Runs: {1}", diff / 1000, gameCounter));

                        BotHelper.ExitGame();
                        Thread.Sleep(500);
                        ExitReason = ("Exiting completed full run");                       
                    }


                }
                catch (Exception e)
                {
                    Logger.Log(e, "Exception: "); 
                    ExitReason = "Exception Exit";
                    BotHelper.ExitGame();
                    exceptionError = true;
                }
                //Thread.Sleep(1000);
            }
        }
    }
}