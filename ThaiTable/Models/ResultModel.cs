using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThaiTable.Models
{
    public class ResultModel
    {
        public bool IsSucces { get; set; }
        public FORM_MODE FormMode { get; set; }
        public string Message { get; set; }
        public bool IsDupplicate { get; set; }
        public bool IsError{ get; set; }
        public string CssClass { get; set; }
    }
}