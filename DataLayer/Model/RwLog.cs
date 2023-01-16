using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class RwLog : BaseModel
    {
        public string Name { get; set; }
        public string  Category { get; set; }
        public int  Amount { get; set; }
    }


    public class Ratings : BaseModel
    {
        public string Name { get; set; }
        public int Rates { get; set; }
        public string Comments { get; set; }
        public string Table { get; set; }
    }
}
