using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Domain
{
    public class Outstanding
    {
        public string DyeingSupervisor { get; set; }

        public decimal Total { get; set; }

        public decimal CurrentMonth { get; set; }

        public decimal PreviousMonth { get; set; }

        public decimal PreviousToPreviousMonth { get; set; }
    }
}
