using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Apax.Models
{
    public class Results
    {
        public int Total { get; set; }
        public IEnumerable Result { get; set; }
    }
}