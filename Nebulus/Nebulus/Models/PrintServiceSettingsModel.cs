using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Nebulus.Models
{
    public class PrintServiceSettingsModel
    {
        [Key]
        public int Id { get; set; }

        public IEnumerable<string> PrintServerNamesList { get; set; }

        public string PrintServerNames { get; set; }

        public string printServerServiceAccount { get; set; }

        public string printServerServiceAccountPassword { get; set; }
    }
}
