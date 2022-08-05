// author: unknown
// purpose: wrapper class for various bits of information about the drilling job at hand
// note: borrowed from virtual display project

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.Data
{
    class CDrillContext
    {
    
        public CWellJob WellJobs { get; set; }

        public CWellPath WellPaths { get; set; }

        public CBitRun BitRuns { get; set; }

        public CDrillingBitDepth DataDepth { get; set; }

        public CGamma DataGamma { get; set; }

        public CPressure DataPressure { get; set; }

        //public CMasterSurveyLog DataMasterSurveyLog { get; set; }

        //public DbSet<VirtualDrill.DataTemperature> DataTemperature { get; set; }

        //public DbSet<VirtualDrill.DataGravity> DataGravity { get; set; }

        //public DbSet<VirtualDrill.DataMagnetic> DataMagnetic { get; set; }

        public CDrillContext()          
        {
        }
    }
}

