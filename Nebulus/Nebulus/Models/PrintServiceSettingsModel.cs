using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nebulus.Models
{
    public class PrintServiceSettingsModel
    {
        public int Id { get; set; }

        public IEnumerable<string> PrintServerNames { get; set; }

        public string printServerServiceAccount { get; set; }

        public string printServerServiceAccountPassword { get; set; }
    }
}
