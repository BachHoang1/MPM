// author: hoan chau
// purpose: an event object that gets passed from CSurvey object to a form that displays the record

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Data
{
    public class CEventSurvey: EventArgs
    {
        public int m_iDatabaseID { get; set; }  // number out of the total number of surveys
        public CSurvey.REC rec;
    }

    public class CEventDeleteSurvey: EventArgs
    {
        public int ID;  // survey id to be deleted
    }

    public class CEventUpdateSurvey: EventArgs
    {
        public int ID;
        public int iBHA;
        public CSurvey.REC rec;
    }

    public class CEventECD: EventArgs
    {
        public float fAnnularPressure;
    }

}
