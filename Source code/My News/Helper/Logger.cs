using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace My_News
{
    /// <summary>
    /// Basic logger class
    /// </summary>
    /// <example>
    /// <code>
    /// // Get instance for first
    /// Logger l = Logger.GetInstance("MyClass", true, true)
    /// l.LogMessage("my message");
    /// </code>
    /// </example>
    class Logger
    {
        /// <summary>
        /// Is debugging on or off
        /// </summary>
        private bool Debug;

        /// <summary>
        /// Send debug messages to file or console
        /// </summary>
        private bool File;

        /// <summary>
        /// Name of log file
        /// </summary>
        private TextWriter LogFilename;

        /// <summary>
        /// Current instance of logger
        /// </summary>
        public static Logger Instance;

        /// <summary>
        /// Class, from where logger was called
        /// </summary>
        private string From;

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// Creates instance of <see cref="Logger"/>
        /// </summary>
        /// <param name="inFile">If true then log to file, otherwise to console</param>
        /// <param name="debugOn">If true then displan log messages</param>
        private Logger(bool inFile, bool debugOn)
        {
            this.File = inFile;
            this.Debug = debugOn;
            this.From = "Logger";

            if (this.File == false && this.Debug == true)
            {
                Console.Title = "debug";
                Console.SetWindowSize(100, 20);
            }
            else if (this.File == true || this.Debug == false)
            {
                //this.HideWindow(false, Console.Title);
            }

            this.Log("Logger", "created.");
        }

        /// <summary>
        /// Hides window with specified <paramref name="title"/>
        /// </summary>
        /// <param name="visible">Set visibility</param>
        /// <param name="title">Window title</param>
        private void HideWindow(bool visible, string title)
        {
            IntPtr hWnd = Logger.FindWindow(null, title);

            if (hWnd != IntPtr.Zero)
            {
                if (!visible)
                    //Hide the window                    
                    Logger.ShowWindow(hWnd, 0); // 0 = SW_HIDE                
                else
                    //Show window again                    
                    Logger.ShowWindow(hWnd, 1); //1 = SW_SHOWNORMA
            }
        }

        /// <summary>
        /// Logs the <paramref name="message"/>
        /// </summary>
        /// <param name="from">From what class message comes</param>
        /// <param name="message">Message, that should be displayed.</param>
        public void Log(string from, string message)
        {
            try
            {
                using (this.LogFilename = (File) ? new StreamWriter("log.txt", true) : Console.Out)
                {
                    if (this.Debug)
                        this.LogFilename.WriteLine("[{0}] {1}: {2}", DateTime.Now.ToString(), from, message);
                }
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// Gets instance of <see cref="Logger"/>
        /// </summary>
        /// <param name="from">Class, from where logger was called</param>
        /// <param name="inFile">Send debug messages to file or console</param>
        /// <param name="debugOn">Is debugging on or off</param>
        /// <returns>A <see cref="Logger"/> instance</returns>
        public static Logger GetInstance(string from, bool inFile, bool debugOn)
        {
            if (Logger.Instance == null)
                Logger.Instance = new Logger(inFile, debugOn);

            Logger.Instance.From = from;
            return Logger.Instance;
        }
    }
}
