using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPTScreenSaver
{
    public static class AppLogging
    {
        public static Logger Instance { get; private set; }

        static AppLogging()
        {
            Instance = LogManager.GetCurrentClassLogger();
        }
    }
}