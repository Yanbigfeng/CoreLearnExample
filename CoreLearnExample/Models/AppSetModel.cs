using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLearnExample.Models
{
    //读取json配置类多层
    public class AppSetModel
    {
        public string name { get; set; }
        public PeserModel peserModel { get; set; }
    }
    //读取json配置类单层
    public class PeserModel
    {
        public int age { get; set; }
    }
    //读取json配置类数组
    public class AppSetArrayModel {

        public string key { get; set; }
        public string[] arrayVlue { get; set; }
    }
}
