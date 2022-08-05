// author: hoan chau
// purpose: record format of the data stored in the license database

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License
{
    public class CLicenseRecord
    {
        public int iDate;  // format is yyyymmdd. this is the date when record was generated
        public int iTime;  // format is hhmmss.  this is the time when record was generated
        public string sComputer;  // computer name
        public string sUser;  // name of user to whom license is issued
        public string sOrganization;  // name of company where user works
        public string sMACAddress;  // unique identifier of the machine associated with the license
        public string sLicense;  // the encrypted license text
        public int iDaysValid;  // number of days the license is valid.  currently not enforced
        public int iActive;  // either 1 or 0 for active or not active
        public int iRemovedDate;
        public int iRemovedTime;
    }
}
