using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace LogMerge
{

    class Hit 
    {
        public String TypeOfFile { get; set; }
        public DateTime Time { get; set; }
        public String Verb { get; set; }
        public String Location { get; set; }
        public String Server { get; set; }
        public String Status { get; set; }


    }
}
