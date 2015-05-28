using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Nebulus.Models;

namespace Nebulus
{
    public class AppConfiguration
    {
        public static NebulusContext NebulusDBContext;

        public static ConfigureModel Settings = new Models.ConfigureModel();
        internal static void ConfigureAppSettings()
        {
            try
            {
                NebulusDBContext = new NebulusContext();
            }
            catch(Exception ex)
            {
                AppLogging.Instance.Error("Error: Connecting to Nebulus Database", ex);
            }

            try
            {

                if (ConfigurationManager.AppSettings["ActiveDirectoryQueryEnabled"] != null)
                {
                    Settings.ActiveDirectoryQueryEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["ActiveDirectoryQueryEnabled"]);
                    Settings.ADConnectionString = ConfigurationManager.AppSettings["ADConnectionString"];
                }

                if(ConfigurationManager.AppSettings["ComputerTAGsEnabled"] != null)
                {
                    Settings.ComputerTAGsEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["ComputerTAGsEnabled"]);
                    Settings.ComputerTAGsDateSourceType = (TAGsDateSourceType)Enum.Parse(typeof(TAGsDateSourceType), ConfigurationManager.AppSettings["ComputerTAGsDateSourceType"]);
                    Settings.ComputerTAGsDateSource = ConfigurationManager.AppSettings["ComputerTAGsDateSource"];
                }

                if(ConfigurationManager.AppSettings["GroupTAGsEnabled"] != null)
                {
                    Settings.GroupTAGsEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["GroupTAGsEnabled"]);
                    Settings.GroupTAGsDateSourceType = (TAGsDateSourceType)Enum.Parse(typeof(TAGsDateSourceType), ConfigurationManager.AppSettings["GroupTAGsDateSourceType"]);
                    Settings.GroupTAGsDateSource = ConfigurationManager.AppSettings["GroupTAGsDateSource"];
                }

                if (ConfigurationManager.AppSettings["UserTAGsEnabled"] != null)
                {
                    Settings.UserTAGsEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["UserTAGsEnabled"]);
                    Settings.UserTAGsDateSourceType = (TAGsDateSourceType)Enum.Parse(typeof(TAGsDateSourceType), ConfigurationManager.AppSettings["UserTAGsDateSourceType"]);
                    Settings.UserTAGsDateSource = ConfigurationManager.AppSettings["UserTAGsDateSource"];
                }

                if (ConfigurationManager.AppSettings["SubNetTAGsEnabled"] != null)
                {
                    Settings.SubNetTAGsEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["SubNetTAGsEnabled"]);
                    Settings.SubNetTAGsDateSourceType = (TAGsDateSourceType)Enum.Parse(typeof(TAGsDateSourceType), ConfigurationManager.AppSettings["SubNetTAGsDateSourceType "]);
                    Settings.UserTAGsDateSource = ConfigurationManager.AppSettings["UserTAGsDateSource"];
                }

                Settings.ServiceBUSQueueName = ConfigurationManager.AppSettings["ServiceBUSQueueName"];

                Settings.ServiceBUSConenctionString = ConfigurationManager.ConnectionStrings["NebulusHUBConnectionString"].ConnectionString;
                Settings.DatabaseConnectionString = ConfigurationManager.ConnectionStrings["NebulusContext"].ConnectionString;
            }
            catch(Exception ex)
            {
                AppLogging.Instance.Error("Error: Configuring App Settings", ex);
            }
        }

        internal static void SaveSettings()
        {
            if (Settings.ActiveDirectoryQueryEnabled != null)
            {
                ConfigurationManager.AppSettings["ActiveDirectoryQueryEnabled"] = Settings.ActiveDirectoryQueryEnabled.ToString();
                ConfigurationManager.AppSettings["ADConnectionString"] = Settings.ADConnectionString;
            }

            if (Settings.ComputerTAGsEnabled != null)
            {
                ConfigurationManager.AppSettings["ComputerTAGsEnabled"] = Settings.ComputerTAGsEnabled.ToString();
                Settings.ComputerTAGsDateSourceType = (TAGsDateSourceType)Enum.Parse(typeof(TAGsDateSourceType), ConfigurationManager.AppSettings["ComputerTAGsDateSourceType"]);
                Settings.ComputerTAGsDateSource = ConfigurationManager.AppSettings["ComputerTAGsDateSource"];
            }

            if (ConfigurationManager.AppSettings["GroupTAGsEnabled"] != null)
            {
                Settings.GroupTAGsEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["GroupTAGsEnabled"]);
                Settings.GroupTAGsDateSourceType = (TAGsDateSourceType)Enum.Parse(typeof(TAGsDateSourceType), ConfigurationManager.AppSettings["GroupTAGsDateSourceType"]);
                Settings.GroupTAGsDateSource = ConfigurationManager.AppSettings["GroupTAGsDateSource"];
            }

            if (ConfigurationManager.AppSettings["UserTAGsEnabled"] != null)
            {
                Settings.UserTAGsEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["UserTAGsEnabled"]);
                Settings.UserTAGsDateSourceType = (TAGsDateSourceType)Enum.Parse(typeof(TAGsDateSourceType), ConfigurationManager.AppSettings["UserTAGsDateSourceType"]);
                Settings.UserTAGsDateSource = ConfigurationManager.AppSettings["UserTAGsDateSource"];
            }

            if (ConfigurationManager.AppSettings["SubNetTAGsEnabled"] != null)
            {
                Settings.SubNetTAGsEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["SubNetTAGsEnabled"]);
                Settings.SubNetTAGsDateSourceType = (TAGsDateSourceType)Enum.Parse(typeof(TAGsDateSourceType), ConfigurationManager.AppSettings["SubNetTAGsDateSourceType "]);
                Settings.UserTAGsDateSource = ConfigurationManager.AppSettings["UserTAGsDateSource"];
            }

            Settings.ServiceBUSQueueName = ConfigurationManager.AppSettings["ServiceBUSQueueName"];

            Settings.ServiceBUSConenctionString = ConfigurationManager.ConnectionStrings["NebulusHUBConnectionString"].ConnectionString;
            Settings.DatabaseConnectionString = ConfigurationManager.ConnectionStrings["NebulusContext"].ConnectionString;
        }
    }
}