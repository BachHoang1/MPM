using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace MPM.Data
{
    class CTrueVerticalDepthManager
    {
   
        private List<KeyValuePair<double, double>> depthTVD = new List<KeyValuePair<double, double>>();
        private CDrillContext _context;

        private static int CompareKeyValuePair(KeyValuePair<double, double> x, KeyValuePair<double, double> y)
        {
            if (x.Key < y.Key)
                return -1;
            return x.Key == y.Key ? 0 : 1;
        }

        internal CTrueVerticalDepthManager(CDrillContext context, long bitRunId, double startDepth, double endDepth)
        {
            this._context = context;
            //CMasterSurveyLog dataMasterSurveyLog1 = context.DataMasterSurveyLog;
            //Expression<Func<CMasterSurveyLog, bool>> predicate = (Expression<Func<CMasterSurveyLog, bool>>)(s => s.BitRunId == bitRunId && s.MeasuredDepth >= startDepth && s.MeasuredDepth <= endDepth && s.TrueVerticalDepth != 0.0);
            // TODO/FIX
            //foreach (CMasterSurveyLog dataMasterSurveyLog2 in (IEnumerable<CMasterSurveyLog>)dataMasterSurveyLog1.Where<CMasterSurveyLog>(predicate))
            //    this.depthTVD.Add(new KeyValuePair<double, double>(dataMasterSurveyLog2.MeasuredDepth, dataMasterSurveyLog2.TrueVerticalDepth));
            if (this.depthTVD.Count <= 0)
                return;
            this.depthTVD.Sort(new Comparison<KeyValuePair<double, double>>(CTrueVerticalDepthManager.CompareKeyValuePair));
        }

        internal double GetTVD(double measuredDepth)
        {
            if (this.depthTVD.Count == 0 || measuredDepth < this.depthTVD.First<KeyValuePair<double, double>>().Key || measuredDepth > this.depthTVD.Last<KeyValuePair<double, double>>().Key)
                return 0.0;
            KeyValuePair<double, double> lowerNumber = this.depthTVD.First<KeyValuePair<double, double>>();
            double num = 0.0;
            foreach (KeyValuePair<double, double> pair in this.depthTVD)
            {
                if (measuredDepth > pair.Key)
                {
                    lowerNumber = pair;
                }
                else
                {
                    num = this.Interpolate(measuredDepth, lowerNumber, pair);
                    break;
                }
            }
            return num;
        }

        private double Interpolate(double measuredDepth, KeyValuePair<double, double> lowerNumber, KeyValuePair<double, double> pair)
        {
            double num1 = pair.Key - lowerNumber.Key;
            double num2 = pair.Value - lowerNumber.Value;
            return (measuredDepth - lowerNumber.Key) / num1 * num2 + lowerNumber.Value;
        }
    }
}

