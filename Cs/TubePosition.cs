using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuonTdaq
{
    namespace TubePosition
    {
        /// <summary>
        /// DelayTimeTable has data to calcurate channel position from delay time.
        /// After the initialization, delayTime2Channel can return channel number of the delay line.
        /// </summary>
        public class DelayTimeConverter
        { 
            public int[,] separationBorder = new int[12,2];
            public int delayTimeOffset = 0;
            private int[] separationList = new int[2000];

            /// <summary>
            /// initialize time table list
            /// </summary>
            /// <param name="separationBorder"></param>
            public DelayTimeConverter(int[,] separationBorder)
            {
                this.separationBorder = separationBorder;
                setSeparationList();
            }

            /// <summary>
            /// initialize using default time separation data
            /// </summary>
            public DelayTimeConverter()
            {
                // debug code 
                setDefaultBorder();
                // debug code end 
                setSeparationList();
            }

            /// <summary>
            /// set separationList. using separationBorder. 
            /// </summary>
            private void setSeparationList()
            {
                // initialize list (initial value is -8 (lower than -2) because -1 may be used for actual channel)
                for (int i = 0; i < separationList.Length; i++) { separationList[i] = -8; }
                for (int dlChannel = 0; dlChannel < (separationBorder.Length / separationBorder.Rank); dlChannel++)
                {
                    for (int j = separationBorder[dlChannel, 0]; j < separationBorder[dlChannel, 1]; j++)
                    {
                        separationList[j] = dlChannel;
                    }
                }
            }

            /// <summary>
            /// for debugging or testing method
            /// </summary>
            private void setDefaultBorder()
            {
                int[,] defaultBorder = new int[,]
                {
                    { 470, 555 },
                    { 560, 630 },
                    { 635, 725 },
                    { 726, 815 },
                    { 830, 890 },
                    { 925, 980 },
                    { 1025, 1070 },
                    { 1115, 1165 },
                    { 1200, 1255 },
                    { 1285, 1355 },
                    { 1360, 1445 },
                    { 1446, 1555 },
                };
                separationBorder = defaultBorder;
            }

            /// <summary>
            /// convert delay time to channel number. return "-8" as invalid delay time or failed to convert.
            /// </summary>
            /// <param name="delayTime"></param>
            /// <returns></returns>
            public int delayTime2Channel( int delayTime)
            {
                if (delayTime<0 || delayTime>2000) { return -8; }
                return separationList[delayTime];
            }

        }

        public class GeometricaIdentifier
        {
            public int tubesPerModule = 24;

            public int calcGeometoricalChannel( int moduleNumber, int GeometoricalBoardChannel)
            {
                return moduleNumber * 24 + GeometoricalBoardChannel;
            }
        }

        public class T1boardIdentifier
        {
            public int[] delayChannelConvertTable;
            public int[] fpgaChannelConvertTable;

            public void setDelayChannelConvertTable ( int[] table )
            {
                delayChannelConvertTable = table;
            }
            public int boardChannel2GeometoricalBoardChannel(int boardChannel)
            {
                return delayChannelConvertTable[boardChannel];
            }

            public void setFpgaChannelConvertTable (int[] table)
            {
                fpgaChannelConvertTable = table;
            }
            public int fpgaChannel2ModuleNumber( int fpgaChannel)
            {
                return fpgaChannelConvertTable[fpgaChannel];
            }
        }

    }
}
