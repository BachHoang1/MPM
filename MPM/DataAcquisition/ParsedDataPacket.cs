using MPM.DataAcquisition.Helpers;
using MPM.Data;

namespace MPM.DataAcquisition
{
    public class ParsedDataPacket
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        private int ChangeCount { get; set; }

        public long BitRunId { get; set; }

        public Command Command { get; set; }

        // Reference types, that have corresponding tables
        public Raw Raw { get; set; }
        public Filtered Filtered { get; set; }
        //public BitHoleDepth             DepthValues             { get; set; }
        //public DepthExtraInfo           DepthTrackerExtraInfo   { get; set; }
        //public GammaValues              GammaValues             { get; set; }
        //public PressureValues           PressureValues          { get; set; }

        public string UnicodeString { get; set; }

        // Special Value types that need to have their own properties
        //public ToolFaceType ToolFaceType    { get; set; }
        public double SignalToNoiseRatio { get; set; }

        public int ValueInt { get; set; }
        public double ValueDouble { get; set; }
        public long ValueLong { get; set; }
        public short ValueShort { get; set; }
        public byte ValueByte { get; set; }

        public PacketT Packet { get; set; }

        public void Clear()
        {
            ChangeCount = 0;

            BitRunId = -1;
            Command = Command.COMMAND_RESP_ERROR;
            Raw = null;
            //Filtered                = null;
            //DepthValues             = null;
            //DepthTrackerExtraInfo   = null;
            //GammaValues             = null;
            //PressureValues          = null;
            UnicodeString = string.Empty;
            SignalToNoiseRatio = 0.0;
            //ToolFaceType            = ToolFaceType.NoValue;

            ValueInt = 0;
            ValueLong = 0;
            ValueDouble = 0.0;
        }

        public ParsedDataPacket(Command command)
        {
            Clear();
            Command = command;
        }

        public string ValueAsString
        {
            get
            {
                string value = string.Empty;

                switch (Command)
                {
                    case Command.COMMAND_RESP_NULL:
                    case Command.COMMAND_RESP_RAW:
                    case Command.COMMAND_RESP_FILTERED:

                    case Command.COMMAND_REQ_STATUS:
                    case Command.COMMAND_ACK_STATUS:

                    case Command.COMMAND_RESP_ERROR:
                    case Command.COMMAND_RESP_TFTYPE:
                    case Command.COMMAND_RESP_TF:
                    case Command.COMMAND_RESP_INCLINATION:
                    case Command.COMMAND_RESP_AZIMUTH:
                    case Command.COMMAND_RESP_GT:
                    case Command.COMMAND_RESP_BT:
                    case Command.COMMAND_RESP_TEMPERATURE:
                    case Command.COMMAND_RESP_CRC_GOOD:
                    case Command.COMMAND_RESP_CRC_BAD:
                    case Command.COMMAND_RESP_GX:
                    case Command.COMMAND_RESP_GY:
                    case Command.COMMAND_RESP_GZ:
                    case Command.COMMAND_RESP_BX:
                    case Command.COMMAND_RESP_BY:
                    case Command.COMMAND_RESP_BZ:
                    case Command.COMMAND_RESP_PACKET_TYPE:
                    case Command.COMMAND_RESP_deltaMTF:
                    case Command.COMMAND_RESP_GAMMA:
                    case Command.COMMAND_RESP_B_PRESSURE:
                    case Command.COMMAND_RESP_A_PRESSURE:
                    case Command.COMMAND_RESP_A_VOLT:
                    case Command.COMMAND_RESP_POWER:
                    case Command.COMMAND_RESP_BATTERY:
                    case Command.COMMAND_PING:
                    case Command.COMMAND_ACK_PING:
                    case Command.COMMAND_MDecl:
                    case Command.COMMAND_TFO:
                    case Command.COMMAND_SIG_STRENGTH:
                    case Command.COMMAND_RESP_LOG_STRING:
                    case Command.COMMAND_PASON_HOLE_DEPTH:
                    case Command.COMMAND_RESP_VIBX:
                    case Command.COMMAND_RESP_VIBY:
                    case Command.COMMAND_RESP_VIB:
                    case Command.COMMAND_RESP_NOISE:
                    case Command.COMMAND_RESP_MAGDEC:
                    case Command.COMMAND_RESP_QUIET:
                    case Command.COMMAND_RESP_RIGCODE:
                    case Command.COMMAND_RESP_TFO:
                    case Command.COMMAND_RESP_BATCAPA:
                    case Command.COMMAND_RESP_BATCAPB:
                    case Command.COMMAND_RESP_GAMMACOR:
                    case Command.COMMAND_RESP_GAMMATOBIT:
                    case Command.COMMAND_RESP_BX16:
                    case Command.COMMAND_RESP_BY16:
                    case Command.COMMAND_RESP_BZ16:
                    case Command.COMMAND_RESP_GAMMAVB:
                    case Command.COMMAND_RESP_VIBXY:
                    case Command.COMMAND_RESP_TV_DIAG:
                    case Command.COMMAND_RESP_SHOCKLEVEL_AXIAL:          // Shock Level From Bins
                    case Command.COMMAND_RESP_SHOCKLEVEL_TRANSVERSE:     // Shock Level From Bins
                    case Command.COMMAND_RESP_SHOCKAVG_AXIAL:            // Shock Average Axial
                    case Command.COMMAND_RESP_SHOCKAVG_TRANSVERSE:       // Shock Average Transverse
                    case Command.COMMAND_RESP_SHOCKMAX_AXIAL:            // Shock Max Axial
                    case Command.COMMAND_RESP_SHOCKMAX_TRANSVERSE:       // Sock Max Transverse
                    case Command.COMMAND_RESP_RMSVIB_AXIAL:              // RMS Vib Axial
                    case Command.COMMAND_RESP_RMSVIB_TRANSVERSE:         // RMS Vib Transverse
                    case Command.COMMAND_RESP_SHOCKLEVEL:                // Shock Light Indicator (0=green, 1=yellow, 2=orange, 3=red)
                    case Command.COMMAND_RESP_VIBLEVEL:                  // Vib Light Indicator (0=green, 1=yellow, 2=orange, 3=red)
                    case Command.COMMAND_RESP_PULSER_BAUD:

                        value = Command.ToString();
                        break;

                    case Command.COMMAND_PASON_DEPTH:
                        //value = DepthValues.BitDepth.ToString();
                        break;

                    case Command.COMMAND_RESP_FORMRES:
                        value = ValueLong.ToString();
                        break;
                    case Command.COMMAND_RESP_XMITPOWER:
                        value = ValueLong.ToString();
                        break;

                    case Command.COMMAND_RESP_LOG_STRING_UNICODE:
                        value = UnicodeString;
                        break;

                    case Command.COMMAND_DEPTHTRACKER_EXTRAINFO:
                        // value = DepthTrackerExtraInfo.BlockHeight.ToString();
                        break;
                }

                return string.Format("Command:  {0}   Value: {1}", Command, value);
            }
        }

        // private void Save(DrillContext drillContext, bool saveChanges)
        //{
        //     if( saveChanges )
        //         drillContext.SaveChanges();
        // }
    }
}
