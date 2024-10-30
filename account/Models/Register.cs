using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace account.Models
{
    class Register
    {
        public string Key { get; set; }  //涵蓋一切的
        public string UID { get; set; }  //使用者帳號
        public string UName { get; set; } //使用者名字
        public string UPwd { get; set; }  //使用者密碼

        public int UScore { get; set; }  //使用者獲得的分數

        public int UPoint { get; set; }  //使用者的點數

        public int ULevel { get; set; }  //使用者的等級
    }
}
