using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MPM.DataAcquisition.Helpers;


namespace MPM.DataAcquisition
{
    public class DataParser
    {
        const int arrayCount = 3;

        // Check the input we have so far and see if we can parse it.
        public ParsedDataPacket ParseInput(List<byte> input)
        {
            BufferSplitter splitter = new BufferSplitter(input.ToArray<byte>());
            foreach (byte[] bArray in splitter)
            {
                PacketT packet = new PacketT(bArray);

                return ProcessPacket(packet);
                //Debug.WriteLine("Command: {0}, Size: {1}", packet.eCommand, packet.udSize);
            }

            return null;
        }

        private ParsedDataPacket ProcessPacket(PacketT packet)
        {
            //Debug.WriteLine("Size {0}", packet.udSize);

            ParsedDataPacket returnValue = null;

            //if (packet.Command != Command.COMMAND_RESP_RAW &&
            //    packet.Command != Command.COMMAND_RESP_FILTERED)
            //{
            //    Debug.WriteLine(packet.Command);
            //}

            switch (packet.Command)
            {
                case Command.COMMAND_RESP_NULL:
                case Command.COMMAND_REQ_STATUS:
                case Command.COMMAND_ACK_STATUS:
                case Command.COMMAND_RESP_LOG_STRING:
                    returnValue = ProcessCommandJustCommand(packet);
                    break;

                case Command.COMMAND_RESP_LOG_STRING_UNICODE:
                    returnValue = ProcessCommandRespLogStringUnicode(packet);
                    break;

                case Command.COMMAND_RESP_RAW:
                case Command.COMMAND_RESP_FILTERED:
                    returnValue = ProcessCommandRespRawFiltered(packet);
                    break;

                case Command.COMMAND_RESP_ERROR:
                    returnValue = ProcessCommandJustCommand(packet);
                    break;

                case Command.COMMAND_RESP_TFTYPE:
                    // returnValue = ProcessCommandRespTFType(packet);
                    break;

                case Command.COMMAND_RESP_TF:
                    // returnValue = ProcessCommandRespTF(packet);
                    break;

                case Command.COMMAND_RESP_INCLINATION:
                    // returnValue = ProcessCommandRespInclination(packet);
                    break;

                case Command.COMMAND_RESP_AZIMUTH:
                    // returnValue = ProcessCommandRespAzimuth(packet);
                    break;

                case Command.COMMAND_RESP_GT:
                    //  returnValue = ProcessCommandRespGT(packet);
                    break;

                case Command.COMMAND_RESP_BT:
                    // returnValue = ProcessCommandRespBT(packet);
                    break;

                case Command.COMMAND_RESP_TEMPERATURE:
                    //  returnValue = ProcessCommandRespTemperature(packet);
                    break;

                case Command.COMMAND_RESP_CRC_GOOD:
                case Command.COMMAND_RESP_CRC_BAD:
                    returnValue = ProcessCRC(packet);
                    //Debug.WriteLine(string.Format("CRC Message:  {0}", packet.Command));
                    break;

                case Command.COMMAND_RESP_GX:
                case Command.COMMAND_RESP_GY:
                case Command.COMMAND_RESP_GZ:
                    //  returnValue = ProcessCommandRespGs(packet);
                    break;

                case Command.COMMAND_RESP_BX:
                case Command.COMMAND_RESP_BY:
                case Command.COMMAND_RESP_BZ:
                    //  returnValue = ProcessCommandRespBs(packet);
                    break;

                case Command.COMMAND_RESP_PACKET_TYPE:
                    returnValue = ProcessCommandRespPacketType(packet);
                    break;

                case Command.COMMAND_RESP_deltaMTF:
                    returnValue = ProcessCommandRespDeltaMTF(packet);
                    break;

                case Command.COMMAND_RESP_GAMMA:
                    //   returnValue = ProcessCommandRespGamma(packet);
                    //UpdateStaticStringsForLanguageChange();
                    //            ViewModelGammaData.UpdateRecords();//.ObjectGammaProperty.ReferenceEquals(1, 2);
                    //            ViewModelGammaData::ObjectGammaProperty.ReferenceEquals(1, 2);
                    //MainWindow.
                    //ModelMain
                    //ViewModelMain
                    //ViewModelMainWindow.
                    //DataGamma
                    //GammaValues
                    //ViewGammaData.
                    //ViewModelGammaData.a
                    //WindowGammaData.
                    //ViewGammaData
                    //            GammaValues.AppendDataUpdate();

                    break;

                case Command.COMMAND_RESP_A_PRESSURE:
                case Command.COMMAND_RESP_B_PRESSURE:
                    returnValue = ProcessCommandInfoToDouble(packet);
                    break;                                    
                case Command.COMMAND_RESP_VIBX:
                case Command.COMMAND_RESP_VIBY:
                    //  returnValue = ProcessCommandRespVibs(packet);
                    break;
                case Command.COMMAND_RESP_NBINC:
                case Command.COMMAND_RESP_NB_AZIMUTH:
                case Command.COMMAND_RESP_NB_GAMMAUP:                    
                case Command.COMMAND_RESP_NB_GAMMADOWN:
                case Command.COMMAND_RESP_NB_SHOCK:
                case Command.COMMAND_RESP_NB_ROTATION:
                    returnValue = ProcessCommandRespNBGamma(packet);
                    break;
                case Command.COMMAND_RESP_VIB:
                    returnValue = ProcessCommandRespVib(packet);
                    break;

                case Command.COMMAND_RESP_POWER:
                    returnValue = ProcessCommandRespPower(packet);
                    break;

                case Command.COMMAND_SIG_STRENGTH:
                case Command.COMMAND_RESP_NOISE:
                    returnValue = ProcessCommandSigStrengthNoise(packet);
                    break;

                case Command.COMMAND_RESP_BATTERY:
                    returnValue = ProcessCommandRespBattery(packet);
                    break;

                case Command.COMMAND_RESP_MAGDEC:
                case Command.COMMAND_RESP_TFO:
                case Command.COMMAND_RESP_GAMMACOR:
                case Command.COMMAND_RESP_GAMMATOBIT:
                    break;
                case Command.COMMAND_RESP_DIRTOBIT:
                    returnValue = ProcessCommandInfoToDouble(packet);
                    break;

                case Command.COMMAND_ACK_PING:
                    returnValue = ProcessCommandJustCommand(packet);
                    break;

                case Command.COMMAND_MDecl:
                    returnValue = ProcessCommandMDecl(packet);
                    break;

                case Command.COMMAND_TFO:
                    ///  returnValue = ProcessCommandTFO(packet);
                    break;

                case Command.COMMAND_PASON_DEPTH:
                    // returnValue = ProcessCommandPasonDepth(packet);
                    break;

                case Command.COMMAND_DEPTHTRACKER_EXTRAINFO:
                    // returnValue = ProcessCommandDepthTrackerExtraInfo(packet);
                    break;

                case Command.COMMAND_RESP_FORMRES:
                    returnValue = ProcessCommandRespFormRes(packet);
                    break;

                case Command.COMMAND_RESP_XMITPOWER:
                    returnValue = ProcessCommandRespXmitPower(packet);
                    break;



                case Command.COMMAND_RESP_DIPANGLE:
                    //  returnValue = ProcessDipAngle(packet);
                    break;

                case Command.COMMAND_RESP_AUX_AZIMUTH:
                case Command.COMMAND_RESP_AUX_INC:
                case Command.COMMAND_RESP_AUX_TOOLFACE:
                case Command.COMMAND_RESP_AUX_PRESSURE:
                case Command.COMMAND_RESP_PUMP_PRESSURE:
                case Command.COMMAND_RESP_AUX_TEMP:
                case Command.COMMAND_RESP_AUX_GAMMA:
                case Command.COMMAND_RESP_AUX_SHOCK:
                    //  returnValue = ProcessAuxiliaryCommands(packet);
                    break;

                case Command.COMMAND_RESP_SHOCKLEVEL_AXIAL:
                case Command.COMMAND_RESP_SHOCKLEVEL_TRANSVERSE:
                case Command.COMMAND_RESP_SHOCKLEVEL:
                    returnValue = ProcessCommandRespLights(packet);
                    break;
                case Command.COMMAND_RESP_RMSVIB_AXIAL:
                case Command.COMMAND_RESP_RMSVIB_TRANSVERSE:
                case Command.COMMAND_RESP_VIBLEVEL:
                    returnValue = ProcessCommandRespLights(packet);
                    break;

                case Command.COMMAND_RESP_SHOCKAVG_AXIAL:            // Shock Average Axial
                case Command.COMMAND_RESP_SHOCKAVG_TRANSVERSE:       // Shock Average Transverse
                    returnValue = ProcessCommandJustCommand(packet);
                    break;
                case Command.COMMAND_RESP_SHOCKMAX_AXIAL:            // Shock Max Axial
                case Command.COMMAND_RESP_SHOCKMAX_TRANSVERSE:       // Sock Max Transverse
                                                                     // Do nothing for now
                                                                     //                    returnValue = 0;
                    returnValue = ProcessCommandInfoToDouble(packet);    // Get rid of the Red background because of null return
                    break;

                case Command.COMMAND_RESP_PULSER_BAUD:
                    returnValue = ProcessCommandRespPulserBaud(packet);
                    break;

                default:
                    //Exception ex = new Exception("Unknown Command type");
                    //throw ex;
                    string message = string.Format("Unknown Command type:  {0}", packet.Command);
                    Debug.WriteLine(message);
                    returnValue = ProcessCommandJustCommand(packet);    // Get rid of the Red background because of null return
                    break;
            }

            return returnValue;
        }

        /*private ParsedDataPacket ProcessDipAngle(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_RESP_DIPANGLE);
            pdp.ValueDouble = packet.Info / Factor.DipAngle;

            Debug.WriteLine("DataParser.ProcessingDipAngle:  {0}", pdp.ValueDouble);

            return pdp;
        }

        private ParsedDataPacket ProcessCBGCommands(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(packet.Command);
            pdp.ValueDouble = packet.Info / Factor.CbgResA;

            return pdp;
        }*/

        private ParsedDataPacket ProcessCRC(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(packet.Command);

            return pdp;
        }

        private static ParsedDataPacket ProcessCommandRespRawFiltered(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(packet.Command);
            pdp.Packet = packet;

            switch (packet.Command)
            {
                case Command.COMMAND_RESP_FILTERED:
                    pdp.Filtered = new Filtered(packet);
                    break;
                case Command.COMMAND_RESP_RAW:
                    pdp.Raw = new Raw(packet);
                    break;
            }

            return pdp;
        }

        private static ParsedDataPacket ProcessCommandNone(PacketT packet)
        {
            // Do Nothing.
            //Debug.WriteLine("Invalid command");
            ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_RESP_NULL);
            return pdp;
        }

        private static ParsedDataPacket ProcessCommandRespXmitPower(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_RESP_XMITPOWER);
            pdp.ValueLong = packet.Info;

            return pdp;
        }

        private static ParsedDataPacket ProcessCommandRespFormRes(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_RESP_FORMRES);
            pdp.ValueLong = packet.Info / 10;

            return pdp;
        }

        /* private ParsedDataPacket ProcessCommandPasonDepth(PacketT packet)
         {
             // Read and interpret the packet.
             DepthPacket dp = new DepthPacket(packet);

             // Create the object to return.
             BitHoleDepth      values = new BitHoleDepth();
             ParsedDataPacket pdp    = new ParsedDataPacket(Command.COMMAND_PASON_DEPTH);
             pdp.DepthValues         = values;

             // Store the Data
             values.SurveyTime   = DateTime.Now;
             values.BitDepth     = dp.BitDepth;
             values.HoleDepth    = dp.HoleDepth;

             return pdp;
         }*/

        /*  private ParsedDataPacket ProcessCommandDepthTrackerExtraInfo(PacketT packet)
          {
              // Read and interpret the packet.
              DepthExtraInfo extraInfo = new DepthExtraInfo(packet);

              // Create the object to return.
              ParsedDataPacket pdp      = new ParsedDataPacket(Command.COMMAND_DEPTHTRACKER_EXTRAINFO);
              pdp.DepthTrackerExtraInfo = extraInfo;

              // Save the data.
              extraInfo.SurveyTime = DateTime.Now;

              // Extra values
              extraInfo.BlockHeight   = extraInfo.BlockHeight;
              extraInfo.ROP           = extraInfo.ROP;
              extraInfo.HookLoad      = extraInfo.HookLoad;
              extraInfo.WOB           = extraInfo.WOB;
              extraInfo.Pressure      = extraInfo.Pressure;
              extraInfo.TrippingRate  = extraInfo.TrippingRate;

              return pdp;
          }*/

        private static ParsedDataPacket ProcessCommandRespNBGamma(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(packet.Command);
            pdp.ValueInt = (int)packet.ValueDouble;

            return pdp;
        }

        private static ParsedDataPacket ProcessCommandRespVib(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_RESP_VIB);
            pdp.ValueInt = (int)packet.ValueDouble;

            return pdp;
        }

        private static ParsedDataPacket ProcessCommandTFO(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_TFO);
            pdp.ValueDouble = packet.ValueDouble;

            //Debug.WriteLine("TFO Value: {0}", pdp.TFOff);

            return pdp;
        }

        private static ParsedDataPacket ProcessCommandMDecl(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_MDecl);
            pdp.ValueDouble = packet.ValueDouble;

            return pdp;
        }

        /*private static ParsedDataPacket ProcessCommandRespReturns(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(packet.Command);

            double value = packet.Info / Factor.MagDec;

            switch (packet.Command)
            {
                case Command.COMMAND_RESP_MAGDEC:
                case Command.COMMAND_RESP_TFO:
                case Command.COMMAND_RESP_GAMMACOR:
                case Command.COMMAND_RESP_GAMMATOBIT:
                case Command.COMMAND_RESP_DIRTOBIT:
                    pdp.ValueDouble = value;
                    //string message = string.Format("{0} CValue: {1},   value: {2}, value: {3}", packet.Command, (int)packet.Command, value, packet.Info);
                    //Debug.WriteLine(message);
                    break;
            }


            return pdp;
        }
*/

        // Kin
        private static ParsedDataPacket ProcessCommandRespLights(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(packet.Command);

            double value = packet.Info; // / Factor.MagDec;

            switch (packet.Command)
            {
                case Command.COMMAND_RESP_SHOCKLEVEL_AXIAL:
                case Command.COMMAND_RESP_SHOCKLEVEL_TRANSVERSE:
                case Command.COMMAND_RESP_RMSVIB_AXIAL:
                case Command.COMMAND_RESP_RMSVIB_TRANSVERSE:
                    if (value < 4.0)
                        pdp.ValueInt = 0;
                    else if (value < 6.0)
                        pdp.ValueInt = 1;
                    else if (value < 8.0)
                        pdp.ValueInt = 2;
                    else
                        pdp.ValueInt = 3;
                    break;
                case Command.COMMAND_RESP_SHOCKLEVEL:
                case Command.COMMAND_RESP_VIBLEVEL:
                    //string message = string.Format("{0} CValue: {1},   value: {2}, value: {3}", packet.Command, (int)packet.Command, value, packet.Info);
                    //Debug.WriteLine(message);
                    //pdp.ValueDouble = value;
                    pdp.ValueInt = (int)value;
                    break;
            }


            return pdp;
        }


        private static ParsedDataPacket ProcessCommandRespBattery(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_RESP_BATTERY);
            pdp.ValueInt = packet.Info;

            return pdp;
        }


        private static ParsedDataPacket ProcessCommandSigStrengthNoise(PacketT packet)
        {
            const float factorStrength = 10000.0f;
            const float factorNoise = 1000.0f;

            ParsedDataPacket pdp = new ParsedDataPacket(packet.Command);
            switch (packet.Command)
            {
                case Command.COMMAND_SIG_STRENGTH:
                    pdp.ValueDouble = (double)packet.Info / (double)factorStrength;
                    break;
                case Command.COMMAND_RESP_NOISE:
                    pdp.ValueDouble = (double)packet.Info / (double)factorNoise;
                    break;
            }

            return pdp;
        }

        private static ParsedDataPacket ProcessCommandRespPower(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_RESP_POWER);
            pdp.ValueDouble = packet.Info;

            return pdp;
        }


        /* private static ParsedDataPacket ProcessCommandRespVibs(PacketT packet)
         {
             ParsedDataPacket pdp = new ParsedDataPacket(packet.Command);

             switch (packet.Command)
             {
                 case Command.COMMAND_RESP_VIBX:
                 case Command.COMMAND_RESP_VIBY:
                     pdp.ValueDouble = packet.Info / Factor.VibX;
                     break;
             }

             return pdp;
         }

         //NB Data
         private static ParsedDataPacket ProcessCommandRespNBs(PacketT packet)
         {
             ParsedDataPacket pdp = new ParsedDataPacket(packet.Command);

             switch (packet.Command)
             {
                 case Command.COMMAND_RESP_NB_GAMMAUP:
                 case Command.COMMAND_RESP_NB_GAMMADOWN:
                 case Command.COMMAND_RESP_NB_GAMMATOTAL:
                     pdp.ValueDouble = packet.Info;// / Factor.VibX;
                     break;
             }

             return pdp;
         }*/

         // generic function
         private static ParsedDataPacket ProcessCommandInfoToDouble(PacketT packet)
         {
             ParsedDataPacket pdp = new ParsedDataPacket(packet.Command);
             pdp.ValueDouble = packet.Info;

             return pdp;
         }

         // Bore Pressure
         

        /*  private static ParsedDataPacket ProcessAuxiliaryCommands(PacketT packet)
          {
              ParsedDataPacket pdp = new ParsedDataPacket(packet.Command);

              switch (packet.Command)
              {
                  case Command.COMMAND_RESP_AUX_TEMP:
                      pdp.ValueDouble = packet.Info / Factor.AuxTemp;
                      break;

                  case Command.COMMAND_RESP_AUX_INC:
                      pdp.ValueDouble = packet.Info / Factor.AuxInc;
                      break;

                  case Command.COMMAND_RESP_AUX_TOOLFACE:
                      pdp.ValueDouble = packet.Info / Factor.AuxToolface;
                      Debug.WriteLine("Aux Toolface: {0}", pdp.ValueDouble);
                      break;

                  case Command.COMMAND_RESP_AUX_AZIMUTH:
                      double value = packet.Info / Factor.AuxAzimuth;
                      pdp.ValueDouble = CAzimuthVal(value);
                      //CalculateAzimuthValue(packet, pdp, 1.0);
                      Debug.WriteLine("Aux Azimuth: {0}", pdp.ValueDouble);
                      break;

                  case Command.COMMAND_RESP_PUMP_PRESSURE:
                  case Command.COMMAND_RESP_AUX_PRESSURE:
                  case Command.COMMAND_RESP_AUX_GAMMA:
                      pdp.ValueDouble = packet.Info;
                      break;

                  case Command.COMMAND_RESP_AUX_SHOCK:
                      pdp.ValueDouble = packet.Info / Factor.AuxShock;
                      break;
              }

              //Debug.WriteLine("{0}:  {1}, {2}, {3}", packet.Command, packet.Info, pdp.ValueDouble, packet.DataLength);

              return pdp;
          }*/

        /* private static ParsedDataPacket ProcessCommandRespGamma(PacketT packet)
         {
             ParsedDataPacket pdp     = new ParsedDataPacket(Command.COMMAND_RESP_GAMMA);

             GammaValues values       = new GammaValues();
             values.Gamma             = packet.Info;
             values.GammaOffset       = packet.Info;
             values.TrueVerticalDepth = 0.0;

             pdp.GammaValues = values;
 values.AppendDataUpdate();

             return pdp;
         }*/

        private static ParsedDataPacket ProcessCommandRespDeltaMTF(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_RESP_deltaMTF);
            pdp.ValueDouble = packet.Info;

            return pdp;
        }

        private static ParsedDataPacket ProcessCommandRespPacketType(PacketT packet)
        {
            // DEH 08282003-this is what decides the survey type.
            // this needs to be kept in the SURVEY//Log struct.
            // Also maybe this should also be the trigger to start
            // a new survey record.
            //FinishOldSurveyRecord( WireLine );
            //AfxBeginThread(FinishOldSurveyRecordThreadProc, mainFrame,THREAD_PRIORITY_NORMAL,0,0,NULL);

            ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_RESP_PACKET_TYPE);
            pdp.ValueInt = packet.Info;

            return pdp;
        }

        private static ParsedDataPacket ProcessCommandRespPulserBaud(PacketT packet)
        {

            ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_RESP_PULSER_BAUD);
            pdp.ValueInt = packet.Info;

            return pdp;
        }

        /*  private static ParsedDataPacket ProcessCommandRespBs(PacketT packet)
          {
              ParsedDataPacket pdp = new ParsedDataPacket(packet.Command);

              //int count = Extract12LSb(packet.Info);
              //double value = (count * 40.0) / 1024;
              //value /= 100;

              //switch (packet.Command)
              //{
              //    case Command.COMMAND_RESP_BX:
              //    case Command.COMMAND_RESP_BY:
              //    case Command.COMMAND_RESP_BZ:
              //        pdp.ValueDouble = value;
              //        //Debug.WriteLine("Command: {0}, Value: {1}", pdp.Command, pdp.ValueDouble);
              //        break;
              //}

              pdp.ValueDouble = packet.Info / Factor.BX;

              return pdp;
          }

          private static ParsedDataPacket ProcessCommandRespGs(PacketT packet)
          {
              ParsedDataPacket pdp = new ParsedDataPacket(packet.Command);

              //int count = Extract12LSb(packet.Info);
              //double value = (count * 0.6) / 1024;

              //switch (packet.Command)
              //{
              //    case Command.COMMAND_RESP_GX:
              //    case Command.COMMAND_RESP_GY:
              //    case Command.COMMAND_RESP_GZ:
              //        pdp.ValueDouble = value;
              //        //Debug.WriteLine("Command: {0}, Value: {1}", pdp.Command, pdp.ValueDouble);
              //        break;
              //}

              pdp.ValueDouble = packet.Info / Factor.GX;

              return pdp;
          }*/

        static int Extract12LSb(int value)
        {
            uint uCount = (uint)value | 0xFFFFF000;

            int count = ((value & (1 << 11)) > 0)
                        ? (int)uCount
                        : value;
            return count;
        }

        public static int ReorderBytes(byte b0, byte b1, byte b2, byte b3)
        {
            int int0 = b0;
            int int1 = b1 << 8;
            int int2 = b2 << 16;
            int int3 = b3 << 24;

            return int0 + int1 + int2 + int3;
        }

        private static ParsedDataPacket ProcessCommandJustCommand(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(packet.Command);

            return pdp;
        }

        /* private static ParsedDataPacket ProcessCommandRespTemperature(PacketT packet)
         {
             ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_RESP_TEMPERATURE);

             pdp.ValueDouble = packet.Info / Factor.Temperature;

             return pdp;
         }

         // Total Magnetic
         private static ParsedDataPacket ProcessCommandRespBT(PacketT packet)
         {
             ParsedDataPacket pdp    = new ParsedDataPacket(Command.COMMAND_RESP_BT);
             pdp.ValueDouble = packet.Info / Factor.BT;

             //Debug.WriteLine("DataParser.ProcessCommandRespBT -- Total Magnetic:  {0}", pdp.ValueDouble);

             return pdp;
         }

         // Total Gravity
         private static ParsedDataPacket ProcessCommandRespGT(PacketT packet)
         {
             ParsedDataPacket pdp    = new ParsedDataPacket(Command.COMMAND_RESP_GT);
             pdp.ValueDouble = packet.Info / Factor.GT;

             //Debug.WriteLine("DataParser.ProcessCommandRespBT -- Total Gravity:  {0}", pdp.ValueDouble);

             return pdp;
         }

         private ParsedDataPacket ProcessCommandRespAzimuth(PacketT packet)
         {
             ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_RESP_AZIMUTH);
             double value = packet.Info / Factor.Azimuth;
             pdp.ValueDouble = CAzimuthVal(value);

             //CalculateAzimuthValue(packet, pdp, Factor.Azimuth);

             return pdp;
         }

         //private static void CalculateAzimuthValue(PacketT packet, ParsedDataPacket pdp, double divisionFactor)
         //{
         //    pdp.ValueDouble = packet.Info / divisionFactor;
         //    if (pdp.ValueDouble > 360)
         //        pdp.ValueDouble -= 360;
         //}*/

        private static double CAzimuthVal(double value)
        {
            return (value > 360)
                        ? value - 360
                        : value;
        }

        /*private ParsedDataPacket ProcessCommandRespInclination(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_RESP_INCLINATION);

            pdp.ValueDouble = packet.Info / Factor.Inclination;

            return pdp;
        }

        private ParsedDataPacket ProcessCommandRespTF(PacketT packet)
        {
            ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_RESP_TF);

            pdp.ValueDouble = packet.Info / Factor.TF;

            return pdp;
        }
        */
        /* private static ParsedDataPacket ProcessCommandRespTFType(PacketT packet)
         {
             ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_RESP_TFTYPE);
             pdp.ToolFaceType = (packet.Info == 1)
                                     ? ToolFaceType.Magnetic
                                     : ToolFaceType.Gravity;
             return pdp;
         }*/


        private static ParsedDataPacket ProcessCommandRespLogStringUnicode(PacketT packet)
        {
            StringBuilder sb = new StringBuilder();

            int length = packet.DataLengthShort;
            //Debug.WriteLine("Unicode length:  {0}", length);

            for (int i = 0; i < length; i++)
                sb.Append((char)packet.DataAsShort(i));
            //sb.Append((char)packet.uwData[i]);

            string s = sb.ToString();
            //Debug.WriteLine(s);

            ParsedDataPacket pdp = new ParsedDataPacket(Command.COMMAND_RESP_LOG_STRING_UNICODE);
            pdp.UnicodeString = s;

            return pdp;
        }
    }
}
