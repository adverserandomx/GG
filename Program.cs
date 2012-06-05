#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3;
using Grind.Common;

namespace Grind
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);

                Worker.Instance = new Worker();
                Logger.Log("***New Start ***");
                Worker.Instance.Start();
            }
            catch (Exception e)
            {
                Logger.Log(e, "Main Exception!");
            }
        }

        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            try
            {
               //todo: decide whether we exit due to horrible thread death.
                Worker.Instance.Stop();
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Main Exception!");
            }
        }
    }
}
