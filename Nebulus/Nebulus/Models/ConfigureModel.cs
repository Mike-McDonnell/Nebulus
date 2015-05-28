using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nebulus.Models
{
    public class ConfigureModel
    {
        [Key]
        public int Id { get; set; }
        //General
        public bool ActiveDirectoryQueryEnabled { get; set; }
        public string ADConnectionString { get; set; }

        //Message
        public bool GroupTAGsEnabled { get; set; }
        public TAGsDateSourceType GroupTAGsDateSourceType { get; set; }
        public string GroupTAGsDateSource { get; set; }

        public bool UserTAGsEnabled { get; set; }
        public TAGsDateSourceType UserTAGsDateSourceType { get; set; }
        public string UserTAGsDateSource { get; set; }

        public bool ComputerTAGsEnabled { get; set; }
        public TAGsDateSourceType ComputerTAGsDateSourceType { get; set; }
        public string ComputerTAGsDateSource { get; set; }

        public bool SubNetTAGsEnabled { get; set; }

        public string ServiceBUSQueueName { get; set; }

        public string ServiceBUSConenctionString { get; set; }
        
        //SplashScreen

        //Printing
        public String[] PrintServerNames { get; set; }

        public string[] printServerServiceAccount { get; set; }

        public string[] printServerServiceAccountPassword { get; set; }

        public TAGsDateSourceType SubNetTAGsDateSourceType { get; set; }

        public string DatabaseConnectionString { get; set; }
    }
}