using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MPM.DataAcquisition;

namespace MPM.DataAcquisition.Helpers
{
    class BufferSplitter
    {
        //List<byte> _buffer;
        byte[] _buffer;

        int DataLength { get { return _buffer.Length; } }

        public BufferSplitter(byte[] input)
        {
            //Debug.WriteLine("BufferSplitter constructor, data length: {0}", input.Count);
            _buffer = input;
        }

        public IEnumerator<byte[]> GetEnumerator()
        {
            return NextPacketBytes();
        }

        public IEnumerator<byte[]> NextPacketBytes()
        {
            int packetSize = PacketT.PacketSize(_buffer);
            int bytesRemaining = _buffer.Length;

            while ( IsMore(bytesRemaining, packetSize) )
            {
                //Debug.WriteLine("Bytes left in buffer: {0}, Packet Size: {1}", _buffer.Count, packetSize);

                //byte[] retValue = _buffer.GetRange(0, packetSize).ToArray();
                byte[] retValue = new byte[packetSize];
                Array.ConstrainedCopy(_buffer, 0, retValue, 0, packetSize);

                bytesRemaining = _buffer.Length - packetSize;
                //Debug.WriteLine("bytesRemaining: {0}", bytesRemaining);
                if (bytesRemaining > 0)
                {
                    //Debug.WriteLine("NextPacketBytes: In IF");

                    //_buffer = _buffer.GetRange(packetSize, bytesRemaining);
                    byte[] temp = new byte[bytesRemaining];
                    Array.ConstrainedCopy(_buffer, packetSize, temp, 0, bytesRemaining);
                    _buffer = temp;

                    packetSize = PacketT.PacketSize(_buffer);
                }
                else
                {
                    //Debug.WriteLine("NextPacketBytes: In ELSE");
                    _buffer = null;
                    packetSize = 0;
                }

                yield return retValue;
            }
        }

        private bool IsMore( int bytesRemaining, int packetSize )
        {
            return  _buffer != null
                        && _buffer.Length > 0
                        && bytesRemaining >= packetSize;
        }
    }
}
