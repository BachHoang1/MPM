using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MPM.DataAcquisition.Helpers
{
    //  The following is the data structure that was used in the original version of Virtual Drill
    //  to represent the packets coming across the wire.
    //
    //      typedef struct {
    //          command_e eCommand;          // Command as enumerated below
    //          u32   udTimeStamp;           // Timestamp from timeGetTime()
    //          u32   udSize;                // Fill in with size of union used
    //          union {
    //              struct  {
    //              u32   udVersion;          // Version of protocol used
    //              float fCarrierFreq;       // Carrier frequency
    //              encoding_e  eEncoding;    // Encoding type
    //              float fSampleFreq;        // Sampling frequency
    //              mode_e  eMode;            // Mode
    //             } sSettings;

    //          u16   uwData[ SAMP_FREQ ];  // Raw or filtered data

    //          u32   udInfo;               // Used for many things

    //          double uddValue;            //added this in the 1.02 version. is bad! messes up the alignment.
    //          } d;
    //      } packet_t;

    public class PacketT
    {
        public static int PositionCommand = 0;
        public static int PositionTimeStamp = 4;
        public static int PositionUDSize = 8;

        public static int BufferSize = 5000;
        public static int HeaderSize = sizeof(Command) + (2 * Utilities.SizeOfInt);
        public static int StartingPositionForData = HeaderSize;

        public Command Command { get; private set; }
        public DateTime TimeReceived { get; private set; }   // We're using this instead
        public uint DataSizeInBytes { get; private set; }

        //private uint    _timeStamp      { get; set; }   // This is never used.

        // The byteData array holds only the data part of the packet; not any part of the header.
        private byte[] byteData { get; set; }
        public byte DataByte(int index) { return byteData[index]; }

        public double ValueDouble { get; set; }

        public int Info { get; set; }  // udInfo

        public int DataLength { get { return byteData.Length; } }
        public int DataLengthShort { get { return DataLength / Utilities.SizeOfShort; } }
        public int DataLengthFloat { get { return DataLength / Utilities.SizeOfFloat; } }

        public static Command CommandFromByteArray(byte[] input)
        {
            return (Command)Utilities.BytesToInt(input, PositionCommand);
        }

        public PacketT(byte[] inputBytes)
        {
            Command = CommandFromByteArray(inputBytes);
            //Debug.WriteLine("Command: {0}", eCommand);

            TimeReceived = DateTime.Now;
            //_timeStamp = (uint)Utilities.BytesToInt(inputBytes, PositionTimeStamp);  // This is never used.

            DataSizeInBytes = (uint)Utilities.BytesToInt(inputBytes, PositionUDSize);

            // Load the byte data
            LoadByteData(inputBytes);

            ////if (UsesInfoValue(Command))
            //if (DataLength >= Utilities.SizeOfInt)
            //{
            //    Info = DataAsInt(0);

            //    //// Load the double value, if the command uses it.
            //    //if (UsesDoubleValue(Command))
            //    //    ValueDouble = Info / 10000.0;
            //}

            Info = (DataLength >= Utilities.SizeOfInt)
                        ? DataAsInt(0)
                        : -9999999;

            ValueDouble = -9999999.0;
        }

        public int DataAsInt(int index)
        {
            return Utilities.BytesToInt(byteData, index * Utilities.SizeOfInt);
        }

        private void LoadByteData(byte[] inputBytes)
        {
            byteData = new byte[DataSizeInBytes];

            Array.ConstrainedCopy(inputBytes, StartingPositionForData, byteData, 0, (int)DataSizeInBytes);
        }

        public short DataAsShort(int index, bool checkBoundaries = true)
        {
            return Utilities.BytesToShort(byteData, index * Utilities.SizeOfShort, checkBoundaries);
        }

        public byte DataAsByte(int index)
        {
            return index < byteData.Length
                    ? byteData[index]
                    : (byte)0;
        }

        public float DataAsFloat(int index)
        {
            return System.BitConverter.ToSingle(byteData, index * Utilities.SizeOfFloat);
        }

        public static ushort PacketSize(byte[] bytes)
        {
            return (ushort)(Utilities.BytesToUShort(bytes, PositionUDSize) + HeaderSize);
        }

        public static int PacketSize(int udSize)
        {
            return HeaderSize + udSize;
        }

        public bool UsesDoubleValue(Command command)
        {
            switch (command)
            {
                case Command.COMMAND_RESP_GT:
                    return true;
            }

            return false;
        }

        //public bool UsesInfoValue(Command command)
        //{
        //    if (UsesDoubleValue(command))
        //        return true;

        //    switch (command)
        //    {
        //        case Command.COMMAND_RESP_RETURN:
        //        case Command.COMMAND_RESP_RETURN2:
        //        case Command.COMMAND_RESP_RETURN3:
        //        case Command.COMMAND_RESP_RETURN4:
        //        case Command.COMMAND_RESP_RETURN5:
        //        case Command.COMMAND_RESP_TFTYPE:
        //        case Command.COMMAND_RESP_TF:
        //        case Command.COMMAND_RESP_INCLINATION:
        //        case Command.COMMAND_RESP_AZIMUTH:
        //        case Command.COMMAND_RESP_GT:
        //        case Command.COMMAND_RESP_BT:
        //        case Command.COMMAND_RESP_TEMPERATURE:
        //        case Command.COMMAND_RESP_GX:
        //        case Command.COMMAND_RESP_GY:
        //        case Command.COMMAND_RESP_GZ:
        //        case Command.COMMAND_RESP_BX:
        //        case Command.COMMAND_RESP_BY:
        //        case Command.COMMAND_RESP_BZ:
        //        case Command.COMMAND_RESP_PACKET_TYPE:
        //        case Command.COMMAND_RESP_deltaMTF:
        //        case Command.COMMAND_RESP_GAMMA:
        //        case Command.COMMAND_RESP_A_PRESSURE:
        //        case Command.COMMAND_RESP_B_PRESSURE:
        //        case Command.COMMAND_RESP_VIBX:
        //        case Command.COMMAND_RESP_VIBY:
        //        case Command.COMMAND_RESP_POWER:
        //        case Command.COMMAND_SIG_STRENGTH:
        //        case Command.COMMAND_RESP_NOISE:
        //        case Command.COMMAND_RESP_BATTERY:
        //        case Command.COMMAND_RESP_FORMRES:
        //        case Command.COMMAND_RESP_XMITPOWER:
        //            return true;

        //        case Command.COMMAND_TFO:
        //        case Command.COMMAND_MDecl:
        //        case Command.COMMAND_RESP_VIB:
        //            return false;
        //    }

        //    return false;
        //}
    }
}
