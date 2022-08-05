// author: hoan chau
// purpose: to signal that Detect is connected or disconnected.  GUI should show some indicator

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Data
{
    public class CEventDetect: EventArgs
    {
        public enum CONNECTION { CLOSED, OPEN, TRY };
        public CONNECTION m_iConnected;        
    }
}
