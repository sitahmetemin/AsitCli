using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsitCli.Console.src.Utility
{
    internal class Definition
    {
        public enum ProjectLayers
        {
            Core,
            Domain,
            Business,
            Common
        }

        public enum ProjectType
        {
            [Display(Name = "webapi")]
            webapi,
            [Display(Name = "mvc")]
            mvc
        }
    }
}
