using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Data
{
    public class CEventDPointText: EventArgs
    {
        public string m_sText { get; set; }
        public CCommonTypes.TELEMETRY_TYPE m_ttSource { get; set; }
    }
}
