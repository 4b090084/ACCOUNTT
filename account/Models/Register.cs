using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace account.Models
{
    class Register
    {
        public string Key { get; set; }
        public string UID { get; set; }
        public string UName { get; set; }
        public string UPwd { get; set; }

        public int UScore { get; set; }

        public int UPoint { get; set; }

        public int ULevel { get; set; }
    }
}
