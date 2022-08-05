using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using RoundLAB.eddi.Classes.Annotations;

namespace RoundLAB.eddi.Classes.Survey
{
    public class SurveyTrajectoryItem : INotifyPropertyChanged
    {
        private int _legId;
        private int _bhaNumber;
        private int _stationNumber;
        private int _surveyId;
        private string _correctionType;
        private double _depth;
        private double _inc;
        private double _azm;
        private double _tvd;
        private double _sstvd;
        private double _dl;
        private double _dls;
        private double _northing;
        private double _easting;
        private double _verticalSection;
        private double? _closureDistance;
        private double? _closureAzimuth;
        private double? _directionalDifference;
        private double? _lat;
        private double? _long;
        private double? _utmNorthing;
        private double? _utmEasting;
        private DateTime _dateTimeUtc;
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int LegId
        {
            get => _legId;
            set
            {
                if (value == _legId) return;
                _legId = value;
                OnPropertyChanged();
            }
        }

        public int BhaNumber
        {
            get => _bhaNumber;
            set
            {
                if (value == _bhaNumber) return;
                _bhaNumber = value;
                OnPropertyChanged();
            }
        }

        public int StationNumber
        {
            get => _stationNumber;
            set
            {
                if (value == _stationNumber) return;
                _stationNumber = value;
                OnPropertyChanged();
            }
        }

        public string CorrectionType
        {
            get => _correctionType;
            set
            {
                if (value == _correctionType) return;
                _correctionType = value;
                OnPropertyChanged();
            }
        }

        public double Depth
        {
            get => _depth;
            set
            {
                if (value.Equals(_depth)) return;
                _depth = value;
                OnPropertyChanged();
            }
        }

        public double Inc
        {
            get => _inc;
            set
            {
                if (value.Equals(_inc)) return;
                _inc = value;
                OnPropertyChanged();
            }
        }

        public double Azm
        {
            get => _azm;
            set
            {
                if (value.Equals(_azm)) return;
                _azm = value;
                OnPropertyChanged();
            }
        }

        public double TVD
        {
            get => _tvd;
            set
            {
                if (value.Equals(_tvd)) return;
                _tvd = value;
                OnPropertyChanged();
            }
        }

        public double SSTVD
        {
            get => _sstvd;
            set
            {
                if (value.Equals(_sstvd)) return;
                _sstvd = value;
                OnPropertyChanged();
            }
        }

        public double DL
        {
            get => _dl;
            set
            {
                if (value.Equals(_dl)) return;
                _dl = value;
                OnPropertyChanged();
            }
        }

        public double DLS
        {
            get => _dls;
            set
            {
                if (value.Equals(_dls)) return;
                _dls = value;
                OnPropertyChanged();
            }
        }

        public double Northing
        {
            get => _northing;
            set
            {
                if (value.Equals(_northing)) return;
                _northing = value;
                OnPropertyChanged();
            }
        }

        public double Easting
        {
            get => _easting;
            set
            {
                if (value.Equals(_easting)) return;
                _easting = value;
                OnPropertyChanged();
            }
        }

        public double VerticalSection
        {
            get => _verticalSection;
            set
            {
                if (value.Equals(_verticalSection)) return;
                _verticalSection = value;
                OnPropertyChanged();
            }
        }

        public double? ClosureDistance
        {
            get => _closureDistance;
            set
            {
                if (Nullable.Equals(value, _closureDistance)) return;
                _closureDistance = value;
                OnPropertyChanged();
            }
        }

        public double? ClosureAzimuth
        {
            get => _closureAzimuth;
            set
            {
                if (Nullable.Equals(value, _closureAzimuth)) return;
                _closureAzimuth = value;
                OnPropertyChanged();
            }
        }

        public double? DirectionalDifference
        {
            get => _directionalDifference;
            set
            {
                if (Nullable.Equals(value, _directionalDifference)) return;
                _directionalDifference = value;
                OnPropertyChanged();
            }
        }

        public double? Lat
        {
            get => _lat;
            set
            {
                if (Nullable.Equals(value, _lat)) return;
                _lat = value;
                OnPropertyChanged();
            }
        }

        public double? Long
        {
            get => _long;
            set
            {
                if (Nullable.Equals(value, _long)) return;
                _long = value;
                OnPropertyChanged();
            }
        }

        public double? UTMNorthing
        {
            get => _utmNorthing;
            set
            {
                if (Nullable.Equals(value, _utmNorthing)) return;
                _utmNorthing = value;
                OnPropertyChanged();
            }
        }

        public double? UTMEasting
        {
            get => _utmEasting;
            set
            {
                if (Nullable.Equals(value, _utmEasting)) return;
                _utmEasting = value;
                OnPropertyChanged();
            }
        }

        public int SurveyId
        {
            get => _surveyId;
            set
            {
                _surveyId = value; 
                OnPropertyChanged();
            }
        }

        public DateTime DateTimeUtc
        {
            get => _dateTimeUtc;
            set
            {
                if (value.Equals(_dateTimeUtc)) return;
                _dateTimeUtc = value;
                OnPropertyChanged();
            }
        }
    }
}
