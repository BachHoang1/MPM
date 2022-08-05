using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPM.DataAcquisition
{
    public sealed class DetectSocketDataSingleton
    {
        private static volatile DetectSocketDataSingleton instance = null;
        private static object syncRoot = new Object();
        private DetectSocketDataSingleton() { instance = null; }

        static DetectSockData m_DetectSockData = null;

        public static DetectSocketDataSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new DetectSocketDataSingleton();

                            m_DetectSockData = new DetectSockData();
                        }
                    }
                }

                return instance;
            }
        }
        public DetectSockData DetectSockData
        {
            get
            {
                return m_DetectSockData;
            }

            set
            {
                m_DetectSockData = value;
            }
        }

        //3/25/21
        public static void Reset()
        {
            instance = null;

            m_DetectSockData.Dispose(); 
        }
        //3/25/21
    }
}
