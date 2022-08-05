using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using RoundLAB.eddi.Classes.Enums;

namespace RoundLAB.eddi.Classes.Survey
{
    /// <summary>
    /// Represents a row in the SurveyData table
    /// </summary>    
    public class SurveyMeasurementDetails : INotifyPropertyChanged, IEquatable<SurveyMeasurementDetails>,
        IEqualityComparer<SurveyMeasurementDetails>, IComparable
    {
        private double? _suppliedInclination;
        private double? _suppliedAzimuth;
        private double? _activeAxScaleFactor;
        private double? _activeAxBias;
        private double? _activeAyScaleFactor;
        private double? _activeAyBias;
        private double? _activeAzScaleFactor;
        private double? _activeAzBias;
        private double? _activeMxScaleFactor;
        private double? _activeMxBias;
        private double? _activeMyScaleFactor;
        private double? _activeMyBias;
        private double? _activeMzScaleFactor;
        private double? _activeMzBias;
        private double? _activeCorrectedInclination;
        private double? _activeCorrectedAzimuth;
        private double? _activeCorrectedGravToolface;
        private double? _activeCorrectedMagToolface;
        private double? _activeCorrectedTotalG;
        private double? _activeCorrectedTotalB;
        private double? _activeCorrectedDipAngle;
        private MSASurveyAdminState _adminState;
        private SurveyType _surveyType;
        private double _surveyDepth;
        private DateTime _dateTimeRecordedUtc;
        private int? _stationNumber;
        private bool _isCommitted = true;
        private bool _isLocked = false;
        private double? _ax;
        private double? _ay;
        private double? _az;
        private double? _mx;
        private double? _my;
        private double? _mz;


        public int JobId { get; set; }
        public int LegId { get; set; }
        public int BhaNumber { get; set; }
        public int SurveyId { get; set; }

        public int? StationNumber
        {
            get => _stationNumber;
            set
            {
                if (value == _stationNumber) return;
                _stationNumber = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateTimeRecordedUtc
        {
            get => _dateTimeRecordedUtc;
            set
            {
                if (value.Equals(_dateTimeRecordedUtc)) return;
                _dateTimeRecordedUtc = value;
                OnPropertyChanged();
            }
        }

        public double SurveyDepth
        {
            get => _surveyDepth;
            set
            {
                if (value.Equals(_surveyDepth)) return;
                _surveyDepth = value;
                OnPropertyChanged();
            }
        }

        public double? Ax
        {
            get => _ax;
            set
            {
                if (value.Equals(_ax)) return;
                _ax = value;
                OnPropertyChanged();
            }
        }

        public double? Ay
        {
            get => _ay;
            set
            {
                if (value.Equals(_ay)) return;
                _ay = value;
                OnPropertyChanged();
            }
        }

        public double? Az
        {
            get => _az;
            set
            {
                if (value.Equals(_az)) return;
                _az = value;
                OnPropertyChanged();
            }
        }

        public double? Mx
        {
            get => _mx;
            set
            {
                if (value.Equals(_mx)) return;
                _mx = value;
                OnPropertyChanged();
            }
        }

        public double? My
        {
            get => _my;
            set
            {
                if (value.Equals(_my)) return;
                _my = value;
                OnPropertyChanged();
            }
        }

        public double? Mz
        {
            get => _mz;
            set
            {
                if (value.Equals(_mz)) return;
                _mz = value;
                OnPropertyChanged();
            }
        }

        public double? SuppliedInclination
        {
            get => _suppliedInclination;
            set
            {
                if (Nullable.Equals(value, _suppliedInclination)) return;
                _suppliedInclination = value;
                OnPropertyChanged();
            }
        }

        public double? SuppliedAzimuth
        {
            get => _suppliedAzimuth;
            set
            {
                if (Nullable.Equals(value, _suppliedAzimuth)) return;
                _suppliedAzimuth = value;
                OnPropertyChanged();
            }
        }

        public double? UncorrectedCalculatedGravToolface { get; set; }
        public double? UncorrectedCalculatedMagToolface { get; set; }
        public double? UncorrectedTotalG { get; set; }
        public double? UncorrectedTotalB { get; set; }
        public double? UncorrectedDip { get; set; }
        public double? CalculatedInclination { get; set; }
        public double? CalculatedAzimuth { get; set; }

        public double? ActiveAxScaleFactor
        {
            get => _activeAxScaleFactor;
            set
            {
                if (Nullable.Equals(value, _activeAxScaleFactor)) return;
                _activeAxScaleFactor = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveAxBias
        {
            get => _activeAxBias;
            set
            {
                if (Nullable.Equals(value, _activeAxBias)) return;
                _activeAxBias = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveAyScaleFactor
        {
            get => _activeAyScaleFactor;
            set
            {
                if (Nullable.Equals(value, _activeAyScaleFactor)) return;
                _activeAyScaleFactor = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveAyBias
        {
            get => _activeAyBias;
            set
            {
                if (Nullable.Equals(value, _activeAyBias)) return;
                _activeAyBias = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveAzScaleFactor
        {
            get => _activeAzScaleFactor;
            set
            {
                if (Nullable.Equals(value, _activeAzScaleFactor)) return;
                _activeAzScaleFactor = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveAzBias
        {
            get => _activeAzBias;
            set
            {
                if (Nullable.Equals(value, _activeAzBias)) return;
                _activeAzBias = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveMxScaleFactor
        {
            get => _activeMxScaleFactor;
            set
            {
                if (Nullable.Equals(value, _activeMxScaleFactor)) return;
                _activeMxScaleFactor = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveMxBias
        {
            get => _activeMxBias;
            set
            {
                if (Nullable.Equals(value, _activeMxBias)) return;
                _activeMxBias = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveMyScaleFactor
        {
            get => _activeMyScaleFactor;
            set
            {
                if (Nullable.Equals(value, _activeMyScaleFactor)) return;
                _activeMyScaleFactor = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveMyBias
        {
            get => _activeMyBias;
            set
            {
                if (Nullable.Equals(value, _activeMyBias)) return;
                _activeMyBias = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveMzScaleFactor
        {
            get => _activeMzScaleFactor;
            set
            {
                if (Nullable.Equals(value, _activeMzScaleFactor)) return;
                _activeMzScaleFactor = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveMzBias
        {
            get => _activeMzBias;
            set
            {
                if (Nullable.Equals(value, _activeMzBias)) return;
                _activeMzBias = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveCorrectedInclination
        {
            get => _activeCorrectedInclination;
            set
            {
                if (Nullable.Equals(value, _activeCorrectedInclination)) return;
                _activeCorrectedInclination = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveCorrectedAzimuth
        {
            get => _activeCorrectedAzimuth;
            set
            {
                if (Nullable.Equals(value, _activeCorrectedAzimuth)) return;
                _activeCorrectedAzimuth = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveCorrectedGravToolface
        {
            get => _activeCorrectedGravToolface;
            set
            {
                if (Nullable.Equals(value, _activeCorrectedGravToolface)) return;
                _activeCorrectedGravToolface = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveCorrectedMagToolface
        {
            get => _activeCorrectedMagToolface;
            set
            {
                if (Nullable.Equals(value, _activeCorrectedMagToolface)) return;
                _activeCorrectedMagToolface = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveCorrectedTotalG
        {
            get => _activeCorrectedTotalG;
            set
            {
                if (Nullable.Equals(value, _activeCorrectedTotalG)) return;
                _activeCorrectedTotalG = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveCorrectedTotalB
        {
            get => _activeCorrectedTotalB;
            set
            {
                if (Nullable.Equals(value, _activeCorrectedTotalB)) return;
                _activeCorrectedTotalB = value;
                OnPropertyChanged();
            }
        }

        public double? ActiveCorrectedDipAngle
        {
            get => _activeCorrectedDipAngle;
            set
            {
                if (Nullable.Equals(value, _activeCorrectedDipAngle)) return;
                _activeCorrectedDipAngle = value;
                OnPropertyChanged();
            }
        }

        public double? FinalAxScaleFactor { get; set; }
        public double? FinalAxBias { get; set; }
        public double? FinalAyScaleFactor { get; set; }
        public double? FinalAyBias { get; set; }
        public double? FinalAzScaleFactor { get; set; }
        public double? FinalAzBias { get; set; }
        public double? FinalMxScaleFactor { get; set; }
        public double? FinalMxBias { get; set; }
        public double? FinalMyScaleFactor { get; set; }
        public double? FinalMyBias { get; set; }
        public double? FinalMzScaleFactor { get; set; }
        public double? FinalMzBias { get; set; }
        public double? FinalCorrectedInclination { get; set; }
        public double? FinalCorrectedAzimuth { get; set; }
        public double? FinalCorrectedGravToolface { get; set; }
        public double? FinalCorrectedMagToolface { get; set; }
        public double? FinalCorrectedTotalG { get; set; }
        public double? FinalCorrectedTotalB { get; set; }
        public double? FinalCorrectedDipAngle { get; set; }

        public MSASurveyAdminState AdminState
        {
            get => _adminState;
            set
            {
                if (value == _adminState) return;
                _adminState = value;
                OnPropertyChanged();
            }
        }

        public SurveyType SurveyType
        {
            get => _surveyType;
            set
            {
                if (value == _surveyType) return;
                _surveyType = value;
                OnPropertyChanged();
            }
        }

        public int ToolMappingId { get; set; }
        public double ReferenceTotalB { get; set; }
        public double ReferenceTotalG { get; set; }
        public double ReferenceDeclination { get; set; }
        public double ReferenceDip { get; set; }

        /// <summary>
        /// Indicates if the survey measurement has been committed to the database or not.
        /// </summary>
        public bool IsCommitted
        {
            get => _isCommitted;
            set
            {
                if (value == _isCommitted) return;
                _isCommitted = value;
                OnPropertyChanged();
            }
        }

        public bool IsLocked
        {
            get => _isLocked;
            set
            {
                if (value == _isLocked) return;
                _isLocked = value;
                OnPropertyChanged();
            }
        }

        #region Comparers & Interface implentations

        public bool Equals(SurveyMeasurementDetails other)
        {
            if (other == null)
                return false;
            return this.SurveyId == other.SurveyId;
        }

        public bool Equals(SurveyMeasurementDetails x, SurveyMeasurementDetails y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.SurveyId == y.SurveyId;
        }

        public int GetHashCode(SurveyMeasurementDetails obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            return obj.SurveyId.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            SurveyMeasurementDetails target = obj as SurveyMeasurementDetails;
            if (target == null)
                throw new AggregateException("The supplied argument is not a SurveyMeasurementDetails object");
            return this.SurveyId.CompareTo(target.SurveyId);
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
