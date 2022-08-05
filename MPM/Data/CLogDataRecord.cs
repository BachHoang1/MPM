// author: hoan chau
// purpose: holds information about a record that is saved to or loaded from the log database

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPM.Data
{
    public class CLogDataRecord
    {
        public int iMessageCode;  // not necessarily a unique identifier
        public string sCreated;
        public int iDate;  // yyyymmdd deprecated by dtCreated
        public int iTime;  // hhmmss deprcated by dtCreated
        public string sName;  // APS name that comes from XMLFileProtocolCommands.xml
        public string sValue;
        public string sUnit;
        public bool bParityError;
        public float fDepth;
        public string sTelemetry;
    }
}
