using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Nebulus.Models;
using System.DirectoryServices.AccountManagement;

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
                Settings.ActiveDirectoryQueryEnabled = ConfigurationManager.AppSettings["ActiveDirectoryQueryEnabled"] != null ? Convert.ToBoolean(ConfigurationManager.AppSettings["ActiveDirectoryQueryEnabled"]) : false;
                Settings.ADConnectionString = ConfigurationManager.AppSettings["ADConnectionString"] != null ? ConfigurationManager.AppSettings["ADConnectionString"] : string.Empty;

                Settings.ComputerTAGsEnabled = ConfigurationManager.AppSettings["ComputerTAGsEnabled"] != null ? Convert.ToBoolean(ConfigurationManager.AppSettings["ComputerTAGsEnabled"]) : false;
                Settings.ComputerTAGsDateSourceType = ConfigurationManager.AppSettings["ComputerTAGsDateSourceType"] != null ? (TAGsDateSourceType)Enum.Parse(typeof(TAGsDateSourceType), ConfigurationManager.AppSettings["ComputerTAGsDateSourceType"]) : TAGsDateSourceType.ActiveDirectory;
                Settings.ComputerTAGsDateSource = ConfigurationManager.AppSettings["ComputerTAGsDateSource"] != null ? ConfigurationManager.AppSettings["ComputerTAGsDateSource"] : string.Empty;

                Settings.GroupTAGsEnabled = ConfigurationManager.AppSettings["GroupTAGsEnabled"] != null ? Convert.ToBoolean(ConfigurationManager.AppSettings["GroupTAGsEnabled"]) : false;
                Settings.GroupTAGsDateSourceType = ConfigurationManager.AppSettings["GroupTAGsDateSourceType"] != null ? (TAGsDateSourceType)Enum.Parse(typeof(TAGsDateSourceType), ConfigurationManager.AppSettings["GroupTAGsDateSourceType"]) : TAGsDateSourceType.ActiveDirectory;
                Settings.GroupTAGsDateSource = ConfigurationManager.AppSettings["GroupTAGsDateSource"] != null ? ConfigurationManager.AppSettings["GroupTAGsDateSource"] : String.Empty;

                Settings.UserTAGsEnabled = ConfigurationManager.AppSettings["UserTAGsEnabled"] != null ? Convert.ToBoolean(ConfigurationManager.AppSettings["UserTAGsEnabled"]) : false;
                Settings.UserTAGsDateSourceType = ConfigurationManager.AppSettings["UserTAGsDateSourceType"] != null ? (TAGsDateSourceType)Enum.Parse(typeof(TAGsDateSourceType), ConfigurationManager.AppSettings["UserTAGsDateSourceType"]) : TAGsDateSourceType.ActiveDirectory;
                Settings.UserTAGsDateSource = ConfigurationManager.AppSettings["UserTAGsDateSource"] != null ? ConfigurationManager.AppSettings["UserTAGsDateSource"] : string.Empty;

                Settings.SubNetTAGsEnabled = ConfigurationManager.AppSettings["SubNetTAGsEnabled"] != null ? Convert.ToBoolean(ConfigurationManager.AppSettings["SubNetTAGsEnabled"]) : false;
                Settings.SubNetTAGsDateSourceType = ConfigurationManager.AppSettings["SubNetTAGsDateSourceType "] != null ? (TAGsDateSourceType)Enum.Parse(typeof(TAGsDateSourceType), ConfigurationManager.AppSettings["SubNetTAGsDateSourceType "]) : TAGsDateSourceType.DataBase;
                Settings.UserTAGsDateSource = ConfigurationManager.AppSettings["UserTAGsDateSource"] != null ? ConfigurationManager.AppSettings["UserTAGsDateSource"] : string.Empty;

                Settings.ServiceBUSQueueName = ConfigurationManager.AppSettings["ServiceBUSQueueName"] != null ? ConfigurationManager.AppSettings["ServiceBUSQueueName"] : string.Empty;

                Settings.ServiceBUSConenctionString = ConfigurationManager.ConnectionStrings["NebulusHUBConnectionString"] != null ? ConfigurationManager.ConnectionStrings["NebulusHUBConnectionString"].ConnectionString : String.Empty;
                Settings.DatabaseConnectionString = ConfigurationManager.ConnectionStrings["NebulusContext"] != null ? ConfigurationManager.ConnectionStrings["NebulusContext"].ConnectionString : string.Empty;
            }
            catch (Exception ex)
            {
                AppLogging.Instance.Error("Error: Configuring App Settings", ex);
            }

            try
            {
                Settings.PrintServiceSettings = NebulusDBContext.PrintServiceConfiguration.FirstOrDefault();
                if(Settings.PrintServiceSettings == null)
                {
                    Settings.PrintServiceSettings = new PrintServiceSettingsModel();
                    Settings.PrintServiceSettings.PrintServerNames = new List<string>();
                }
            }
            catch(Exception ex)
            {
                AppLogging.Instance.Error("Error: Configuring PrintService Settings", ex);
            }
        }

        internal static void SaveSettings()
        {
            try
            {
                ConfigurationManager.AppSettings["ActiveDirectoryQueryEnabled"] = Settings.ActiveDirectoryQueryEnabled.ToString();
                if (Settings.ActiveDirectoryQueryEnabled)
                {
                    ConfigurationManager.AppSettings["ADConnectionString"] = Settings.ADConnectionString;
                }

                ConfigurationManager.AppSettings["ComputerTAGsEnabled"] = Settings.ComputerTAGsEnabled.ToString();
                if (Settings.ComputerTAGsEnabled)
                {
                    Settings.ComputerTAGsDateSourceType = (TAGsDateSourceType)Enum.Parse(typeof(TAGsDateSourceType), ConfigurationManager.AppSettings["ComputerTAGsDateSourceType"]);
                    Settings.ComputerTAGsDateSource = ConfigurationManager.AppSettings["ComputerTAGsDateSource"];
                }

                ConfigurationManager.AppSettings["GroupTAGsEnabled"] = Settings.GroupTAGsEnabled.ToString();
                if (Settings.GroupTAGsEnabled)
                {
                    ConfigurationManager.AppSettings["GroupTAGsDateSourceType"] = Settings.GroupTAGsDateSourceType.ToString();
                    ConfigurationManager.AppSettings["GroupTAGsDateSource"] = Settings.GroupTAGsDateSource;
                }

                ConfigurationManager.AppSettings["UserTAGsEnabled"] = Settings.UserTAGsEnabled.ToString();
                if (Settings.UserTAGsEnabled)
                {
                    ConfigurationManager.AppSettings["UserTAGsDateSourceType"] = Settings.UserTAGsDateSourceType.ToString();
                    ConfigurationManager.AppSettings["UserTAGsDateSource"] = Settings.UserTAGsDateSource;
                }

                ConfigurationManager.AppSettings["SubNetTAGsEnabled"] = Settings.SubNetTAGsEnabled.ToString();
                if (Settings.SubNetTAGsEnabled)
                {
                    ConfigurationManager.AppSettings["SubNetTAGsDateSourceType "] = Settings.SubNetTAGsDateSourceType.ToString();
                    ConfigurationManager.AppSettings["UserTAGsDateSource"] = Settings.UserTAGsDateSource;
                }

                ConfigurationManager.AppSettings["ServiceBUSQueueName"] = Settings.ServiceBUSQueueName;

                var cfg = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(@"/");

                bool updated = false;
                if (cfg.ConnectionStrings.ConnectionStrings["NebulusHUBConnectionString"].ConnectionString != Settings.ServiceBUSConenctionString || cfg.ConnectionStrings.ConnectionStrings["NebulusContext"].ConnectionString != Settings.DatabaseConnectionString)
                {
                    updated = true;
                }
                cfg.ConnectionStrings.ConnectionStrings["NebulusHUBConnectionString"].ConnectionString = Settings.ServiceBUSConenctionString;
                cfg.ConnectionStrings.ConnectionStrings["NebulusContext"].ConnectionString = Settings.DatabaseConnectionString;

                if (updated)
                {
                    cfg.Save();
                }

            }
            catch(Exception ex)
            {
                AppLogging.Instance.Error("Error: Save App Settings", ex);
            }
        }
    }
}