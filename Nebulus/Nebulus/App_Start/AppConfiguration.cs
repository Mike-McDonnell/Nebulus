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
        public static PrincipalContext ADPrincipalContext;

        public static ConfigureModel Settings = new Models.ConfigureModel();
        public static AuthenticationType AuthMethod = AuthenticationType.WindowsOnly;  

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

            LoadSettings();

            try
            {
                if (Settings.ADSsytemName != string.Empty)
                {
                    ADPrincipalContext = new PrincipalContext(ContextType.Domain, Settings.ADSsytemName, Settings.ADConnectionString);
                }
                else
                    ADPrincipalContext = new PrincipalContext(ContextType.Domain);
            }
            catch(Exception ex)
            {
                AppLogging.Instance.Error("Error: Loading Pricipal Context", ex);
            }
            try
            {
          
               
            }
            catch (System.Exception ex)
            {
                AppLogging.Instance.Error("Error: Loading user groups from database", ex);
            }
            try
            {
                AppConfiguration.Settings.PrintServiceSettings = NebulusDBContext.PrintServiceConfiguration.FirstOrDefault();

                if(AppConfiguration.Settings.PrintServiceSettings == null) 
                {
                    AppConfiguration.Settings.PrintServiceSettings = new PrintServiceSettingsModel();
                    NebulusDBContext.PrintServiceConfiguration.Add(AppConfiguration.Settings.PrintServiceSettings);
                    NebulusDBContext.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                AppLogging.Instance.Error("Error: Configuring PrintService Settings", ex);
            }

            try
            {
                AuthMethod = (AuthenticationType)Nebulus.Properties.Settings.Default.AuthenticationType;
            }
            catch(Exception ex)
            {
                AppLogging.Instance.Error("Error: Paresing Authentication Type propertiy", ex);
            }

        }

        internal static void LoadSettings()
        {
            try
            {
                Settings.ActiveDirectoryQueryEnabled = ConfigurationManager.AppSettings["ActiveDirectoryQueryEnabled"] != null ? Convert.ToBoolean(ConfigurationManager.AppSettings["ActiveDirectoryQueryEnabled"]) : false;
                Settings.ADSsytemName = ConfigurationManager.AppSettings["ADSsytemName"] != null ? ConfigurationManager.AppSettings["ADSsytemName"] : string.Empty;
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
                Settings.SubNetTAGsDateSource = ConfigurationManager.AppSettings["SubNetTAGsDateSource"] != null ? ConfigurationManager.AppSettings["SubNetTAGsDateSource"] : string.Empty;

                Settings.WindowsClientsUseNotificationHub = ConfigurationManager.AppSettings["WindowsClientsUseNotificationHub"] != null ? Convert.ToBoolean(ConfigurationManager.AppSettings["WindowsClientsUseNotificationHub"]) : false;

                Settings.ServiceBUSQueueName = ConfigurationManager.AppSettings["ServiceBUSQueueName"] != null ? ConfigurationManager.AppSettings["ServiceBUSQueueName"] : string.Empty;
                Settings.ServiceBUSConenctionString = ConfigurationManager.ConnectionStrings["ServiceBUSConenctionString"] != null ? ConfigurationManager.ConnectionStrings["ServiceBUSConenctionString"].ConnectionString : String.Empty;
                
                Settings.DatabaseConnectionString = ConfigurationManager.ConnectionStrings["NebulusContext"] != null ? ConfigurationManager.ConnectionStrings["NebulusContext"].ConnectionString : string.Empty;

                Settings.NotificationHubName = ConfigurationManager.AppSettings["NotificationHubName"] != null ? ConfigurationManager.AppSettings["NotificationHubName"] : string.Empty;
                Settings.NotificationHubServerConenctionString = ConfigurationManager.ConnectionStrings["NotificationHubServerConenctionString"] != null ? ConfigurationManager.ConnectionStrings["NotificationHubServerConenctionString"].ConnectionString : string.Empty;
                Settings.NotificationHubClientConenctionString = ConfigurationManager.ConnectionStrings["NotificationHubClientConenctionString"] != null ? ConfigurationManager.ConnectionStrings["NotificationHubClientConenctionString"].ConnectionString : string.Empty;
            }
            catch (Exception ex)
            {
                AppLogging.Instance.Error("Error: Configuring App Settings", ex);
            }
        }

        internal static void SaveSettings()
        {
            try
            {
                var cfg = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(@"/");

                if (cfg.AppSettings.Settings["ActiveDirectoryQueryEnabled"] == null)
                    cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("ActiveDirectoryQueryEnabled", ""));
                cfg.AppSettings.Settings["ActiveDirectoryQueryEnabled"].Value = Settings.ActiveDirectoryQueryEnabled.ToString();
                if (Settings.ActiveDirectoryQueryEnabled)
                {
                    if (cfg.AppSettings.Settings["ADSsytemName"] == null)
                        cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("ADSsytemName", ""));
                    cfg.AppSettings.Settings["ADSsytemName"].Value = Settings.ADSsytemName;
                    if (cfg.AppSettings.Settings["ADConnectionString"] == null)
                        cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("ADConnectionString", ""));
                    cfg.AppSettings.Settings["ADConnectionString"].Value = Settings.ADConnectionString;
                }

                if (cfg.AppSettings.Settings["ComputerTAGsEnabled"] == null)
                    cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("ComputerTAGsEnabled", ""));
                cfg.AppSettings.Settings["ComputerTAGsEnabled"].Value = Settings.ComputerTAGsEnabled.ToString();
                if (Settings.ComputerTAGsEnabled)
                {
                    if (cfg.AppSettings.Settings["ComputerTAGsDateSourceType"] == null)
                        cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("ComputerTAGsDateSourceType", ""));
                    cfg.AppSettings.Settings["ComputerTAGsDateSourceType"].Value = Settings.ComputerTAGsDateSourceType.ToString();
                    if (cfg.AppSettings.Settings["ComputerTAGsDateSource"] == null)
                        cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("ComputerTAGsDateSource", ""));
                    cfg.AppSettings.Settings["ComputerTAGsDateSource"].Value = Settings.ComputerTAGsDateSource;
                }
                if (cfg.AppSettings.Settings["GroupTAGsEnabled"] == null)
                    cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("GroupTAGsEnabled", ""));
                cfg.AppSettings.Settings["GroupTAGsEnabled"].Value = Settings.GroupTAGsEnabled.ToString();
                if (Settings.GroupTAGsEnabled)
                {
                    if (cfg.AppSettings.Settings["GroupTAGsDateSourceType"] == null)
                        cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("GroupTAGsDateSourceType", ""));
                    cfg.AppSettings.Settings["GroupTAGsDateSourceType"].Value = Settings.GroupTAGsDateSourceType.ToString();
                    if (cfg.AppSettings.Settings["GroupTAGsDateSource"] == null)
                        cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("GroupTAGsDateSource", ""));
                    cfg.AppSettings.Settings["GroupTAGsDateSource"].Value = Settings.GroupTAGsDateSource;
                }

                if (cfg.AppSettings.Settings["UserTAGsEnabled"] == null)
                    cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("UserTAGsEnabled", ""));
                cfg.AppSettings.Settings["UserTAGsEnabled"].Value = Settings.UserTAGsEnabled.ToString();
                if (Settings.UserTAGsEnabled)
                {
                    if (cfg.AppSettings.Settings["UserTAGsDateSourceType"] == null)
                        cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("UserTAGsDateSourceType", ""));
                    cfg.AppSettings.Settings["UserTAGsDateSourceType"].Value = Settings.UserTAGsDateSourceType.ToString();
                    if (cfg.AppSettings.Settings["UserTAGsDateSource"] == null)
                        cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("UserTAGsDateSource", ""));
                    cfg.AppSettings.Settings["UserTAGsDateSource"].Value = Settings.UserTAGsDateSource;
                }

                if (cfg.AppSettings.Settings["SubNetTAGsEnabled"] == null)
                    cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("SubNetTAGsEnabled", ""));
                cfg.AppSettings.Settings["SubNetTAGsEnabled"].Value = Settings.SubNetTAGsEnabled.ToString();
                if (Settings.SubNetTAGsEnabled)
                {
                    if (cfg.AppSettings.Settings["SubNetTAGsDateSourceType"] == null)
                        cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("SubNetTAGsDateSourceType", ""));     
                    cfg.AppSettings.Settings["SubNetTAGsDateSourceType"].Value = Settings.SubNetTAGsDateSourceType.ToString();
                    
                    if (cfg.AppSettings.Settings["SubNetTAGsDateSource"] == null)
                        cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("SubNetTAGsDateSource", ""));
                    cfg.AppSettings.Settings["SubNetTAGsDateSource"].Value = Settings.SubNetTAGsDateSource;
                }

                if (cfg.AppSettings.Settings["ServiceBUSQueueName"] == null)
                    cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("ServiceBUSQueueName", ""));
                cfg.AppSettings.Settings["ServiceBUSQueueName"].Value = Settings.ServiceBUSQueueName;

                if (cfg.AppSettings.Settings["NotificationHubName"] == null)
                    cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("NotificationHubName", ""));
                cfg.AppSettings.Settings["NotificationHubName"].Value = Settings.NotificationHubName;

                if (cfg.AppSettings.Settings["WindowsClientsUseNotificationHub"] == null)
                    cfg.AppSettings.Settings.Add(new KeyValueConfigurationElement("WindowsClientsUseNotificationHub", ""));
                cfg.AppSettings.Settings["WindowsClientsUseNotificationHub"].Value = Settings.WindowsClientsUseNotificationHub.ToString();

                if (cfg.ConnectionStrings.ConnectionStrings["ServiceBUSConenctionString"] == null)
                    cfg.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("ServiceBUSConenctionString", ""));
                cfg.ConnectionStrings.ConnectionStrings["ServiceBUSConenctionString"].ConnectionString = Settings.ServiceBUSConenctionString;
                if (cfg.ConnectionStrings.ConnectionStrings["NebulusContext"] == null)
                    cfg.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("NebulusContext", ""));
                cfg.ConnectionStrings.ConnectionStrings["NebulusContext"].ConnectionString = Settings.DatabaseConnectionString;

                if (cfg.ConnectionStrings.ConnectionStrings["NotificationHubServerConenctionString"] == null)
                    cfg.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("NotificationHubServerConenctionString", ""));
                cfg.ConnectionStrings.ConnectionStrings["NotificationHubServerConenctionString"].ConnectionString = Settings.NotificationHubServerConenctionString;
                if (cfg.ConnectionStrings.ConnectionStrings["NotificationHubClientConenctionString"] == null)
                    cfg.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("NotificationHubClientConenctionString", ""));
                cfg.ConnectionStrings.ConnectionStrings["NotificationHubClientConenctionString"].ConnectionString = Settings.NotificationHubClientConenctionString;

                cfg.Save();

                Security.Roles.UpdateSaveRoles();
            }
            catch(Exception ex)
            {
                AppLogging.Instance.Error("Error: Save App Settings", ex);
            }
        }
    }
}