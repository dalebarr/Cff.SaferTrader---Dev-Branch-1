﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Cff.SaferTrader.Core
{
    public class CffOnlineFormApplicationExt : CffOnlineFormApplication
    {
        [ScriptIgnoreAttribute]
        public HttpPostedFileBase[] Files { get; set; }
    }
}
