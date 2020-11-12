using System;
using System.Collections.Generic;
using System.Text;

namespace RatesApi.Model
{
    public class ResponseModel
    {
        public string disclaimer { get; set; }
        public string license { get; set; }
        public int timestamp { get; set; }
        public string @base { get; set; }       
        public Dictionary<string,decimal> rates { get; set; }
    }
}
