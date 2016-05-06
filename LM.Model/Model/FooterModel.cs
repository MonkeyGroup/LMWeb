using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LM.Model.Model
{
    public class FooterModel
    {
        public string Address { get; set; }

        public string Telphone { get; set; }

        public string Email { get; set; }

        public List<CompanyModel> companies { get; set; }
    }
}
