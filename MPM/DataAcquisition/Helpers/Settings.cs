using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPM.DataAcquisition.Helpers
{
    //class Settings
    //{
    //    public uint udVersion;
    //    public float fCarrierFreq;
    //    public Encoding encoding;
    //    public float fSampleFreq;
    //    public Mode eMode;
    //}

    enum Encoding
    {
        ENCODING_NORMAL = 1,     // Normal encoding
        ENCODING_RESERVED        // Begin reserved (unused) section
    }

    enum Mode
    {
        MODE_1 = 1,              // Mode 1 packets
        MODE_2,                  // Mode 2 packets
        MODE_RESERVED            // Begin reserved (unused) section
    }
}
