using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLearnExample.Models
{
    //用户表
    [Serializable]
    public class UserViewModel
    {
        public string name { get; set; }

        public int age { get; set; }
    }
}
