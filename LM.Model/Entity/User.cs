using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LM.Model.Entity
{
    public class User
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Pwd { get; set; }

        public DateTime SaveAt { get; set; }
        
        public DateTime LastLoginAt { get; set; }
        
    }
}
