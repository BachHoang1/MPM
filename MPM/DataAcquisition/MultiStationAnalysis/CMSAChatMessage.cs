using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.DataAcquisition.MultiStationAnalysis
{    
    public class CMSAChatMessage : IEquatable<CMSAChatMessage>, IEqualityComparer<CMSAChatMessage>, IComparable
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Base64Content { get; set; }
        public DateTime DateTimeUtc { get; set; }

        public string MessageString => this.GetMessageString();

        public CMSAChatMessage()
        {
            this.DateTimeUtc = DateTime.UtcNow;
        }
        public CMSAChatMessage(string message)
            : this()
        {
            this.Base64Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
        }

        public CMSAChatMessage(string message, string from, string to = null)
            : this(message)
        {
            this.From = from;
            this.To = to;
        }

        /* Decodes the Base64 String */
        public string GetMessageString()
        {
            if (string.IsNullOrWhiteSpace(this.Base64Content))
                return null;
            return Encoding.UTF8.GetString(Convert.FromBase64String(this.Base64Content));
        }

        public bool Equals(CMSAChatMessage other)
        {
            if (other == null)
                return false;

            return DateTimeUtc == other.DateTimeUtc
                   && Base64Content == other.Base64Content;
        }

        public bool Equals(CMSAChatMessage x, CMSAChatMessage y)
        {
            if (x == null && y != null)
                return false;

            if (y == null && x != null)
                return false;

            if (x == null && y == null)
                return true;

            return x.DateTimeUtc == y.DateTimeUtc
                   && x.Base64Content == y.Base64Content;
        }

        public int GetHashCode(CMSAChatMessage obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            return obj.DateTimeUtc.GetHashCode() + obj.Base64Content.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            CMSAChatMessage target = obj as CMSAChatMessage;
            if (target == null)
                throw new AggregateException("The supplied argument is not a ChatMessage object");
            int dateCompare = this.DateTimeUtc.CompareTo(target.DateTimeUtc);
            if (dateCompare == 0)
            {
                return string.Compare(this.Base64Content, target.Base64Content, StringComparison.Ordinal);
            }

            return dateCompare;
        }
    }
}
