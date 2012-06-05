#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using D3;

namespace Grind.Common
{
public static class Logger
    {

        private static string _dirPath;
        private static string _filePath;
        private static string _name;

        /// <summary>
        /// Disabled file creation code.. for now.. While it does create the file, it then immediately crashes d3
        /// </summary>
        static Logger()
        {
            _dirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Logs";
            _name = Assembly.GetExecutingAssembly().GetName().Name;
             _filePath += _dirPath + "\\" + _name + ".txt";

            #if DEBUG
            Game.Print("File Path = " + _filePath);
            #endif

            try
            {
                if (Directory.Exists(_dirPath) == false)
                {
                    //Game.Print(_dirPath + " does not exist - unable to log to disk.");
                    //Directory.CreateDirectory(_dirPath);
                }
                else if (!File.Exists(_filePath))
                {
                    Game.Print(_filePath + " does not exist - unable to log to disk. You must manually create this file and directory.");
                    //File.Create(_filePath);
                    //Game.Print("Creating log file " + _filePath);
                }
                else
                {
                    Game.Print("Logging file Path: " + _filePath);
                }
            }
            catch (Exception e)
            {
                //this exception handler doesn't work. Trying to create files causes d3 to crash terribly.
                Game.Print("File Creation Error:  " + e.ToString());
            }

          
        }

        public static void Log(string message)
        {

            if (!File.Exists(_filePath))
                return; //don't log if file Doesn't exist

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("[{0}] {1}: {2}{3}", DateTime.Now.ToShortTimeString(), _name, message, System.Environment.NewLine);
            try
            {
                File.AppendAllText(_filePath, sb.ToString());
            #if DEBUG
                Game.Print(sb.ToString());
            #endif
            }
            catch (Exception e)
            {
                Game.Print("Logging error! " + e.ToString());
            }

        }

        public static void Log(string message, params object[] args)
        {
            Log(String.Format(message, args));
        }

        public static void Log(Exception e)
        {
            Log("***Exception***");
            Log(String.Format("{0}{1}", e.ToString(), System.Environment.NewLine));
        }

        public static void Log(Exception e, string message)
        {
            Log("***Exception***");
            Log(String.Format("{0}{1}{2}{1}", message, System.Environment.NewLine, e.ToString()));
        }

        public static void Log(Exception e, string format, params object[] args)
        {
            Log("***Exception***");
            Log(e, String.Format(format, args));
        }
    }
}
