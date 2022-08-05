using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace MPM.Data
{
    public class CWellPath
    {   
        public virtual ICollection<CBitRun> BitRuns { get; set; }

        public CWellPath()
        {
            this.BitRuns = (ICollection<CBitRun>)new List<CBitRun>();
            this.SetInitialValues();
        }

        public CWellPath(CWellPath source)
        {
            this.Id = source.Id;
            this.Copy(source);
            foreach (CBitRun bitRun in (IEnumerable<CBitRun>)source.BitRuns)
                this.BitRuns.Add(new CBitRun(bitRun));
        }

        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Key]
        public int Id { get; set; }

        public string WellPathId { get; set; }

        public string Comments { get; set; }

        public double MeasuredDepth { get; set; }

        public double TrueVerticalDepth { get; set; }

        public double Inclination { get; set; }

        public double Azimuth { get; set; }

        public double NorthSouth { get; set; }

        public double EastWest { get; set; }

        public double VerticalSection { get; set; }

        public double VSA { get; set; }

        public void Clear()
        {
            this.WellPathId = string.Empty;
            this.Comments = string.Empty;
            this.SetInitialValues();
        }

        private void SetInitialValues()
        {
            this.MeasuredDepth = 0.0;
            this.TrueVerticalDepth = 0.0;
            this.Inclination = 0.0;
            this.Azimuth = 0.0;
            this.NorthSouth = 0.0;
            this.EastWest = 0.0;
            this.VerticalSection = 0.0;
            this.VSA = 0.0;
        }

        internal void Copy(CWellPath source)
        {
            this.WellPathId = source.WellPathId;
            this.Comments = source.Comments;
            this.MeasuredDepth = source.MeasuredDepth;
            this.TrueVerticalDepth = source.TrueVerticalDepth;
            this.Inclination = source.Inclination;
            this.Azimuth = source.Azimuth;
            this.NorthSouth = source.NorthSouth;
            this.EastWest = source.EastWest;
            this.VerticalSection = source.VerticalSection;
            this.VSA = source.VSA;
        }
    }
}

