using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPM.DataAcquisition.Helpers;

namespace MPM.DataAcquisition
{
    public class Raw
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public long Id { get; set; }

        public int Count { get; private set; }
        public short[] Data { get; private set; }
        PacketT _packet;


        public Raw(PacketT packet)
        {
            // Get the count of short values
            Count = packet.DataLengthShort;
            _packet = packet;

            //// Create the array and fill it
            Data = new short[Count];
            for (int i = 0; i < Count; i++)
                Data[i] = packet.DataAsShort(i);
        }

        public short this[int index, bool checkBoundaries = true]
        {
            get
            {
                if (checkBoundaries)
                    return (index >= 0 && index < Count)
                            ? _packet.DataAsShort(index)
                            : (short)0;
                else
                    return _packet.DataAsShort(index, false);
            }
        }
    }

    public class Filtered
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public long Id { get; set; }

        public int Count { get; set; }
        public float[] Data { get; private set; }

        public Filtered(PacketT packet)
        {
            Count = packet.DataLengthFloat;

            // Create the array and fill it.
            Data = new float[Count];
            for (int i = 0; i < Count; i++)
                Data[i] = packet.DataAsFloat(i);
        }

        public float this[int index]
        {
            get
            {
                return (index >= 0 && index < Count)
                        ? Data[index]
                        : 0F;
            }
        }
    }
}
