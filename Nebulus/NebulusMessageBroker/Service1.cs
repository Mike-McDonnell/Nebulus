using Nebulus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace NebulusMessageBroker
{
    public partial class Service1 : ServiceBase
    {
        private Service_Start.ServiceConfig ServiceConfiguration;
        private NebulusContext DataBaseContext;


        public Service1()
        {
            InitializeComponent();
        }

        protected override async void OnStart(string[] args)
        {
            AppLogging.Instance.Info("Starting Messaging Broker Service");
            this.ServiceConfiguration = await Service_Start.ServiceConfiguration.GetServiceConfiguration();

            AppLogging.Instance.Info("Connecting to Database, " + ServiceConfiguration.DatabaseConnectionString);
            this.DataBaseContext = new NebulusContext();

        }

        protected override void OnStop()
        {
        }
    }
}
