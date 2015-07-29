using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nebulus.Models
{
    public class SecurityRoleEntity
    {
        public int SecurityRoleEntityID { get; set; }
        public string Name { get; set; }
        public int Access { get; set; }
    }
}