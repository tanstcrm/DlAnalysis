using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuonTdaq
{
    namespace DetectorGeometory
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

        public class TubePosition
        {
            private int TubeNumberPerModule = 24;
            private int DelayChannelNumberPerT1 = 12;
            //private bool t1BoardDirection = false;
            private int[] moduleHeadFpgaChannel;
            private List<TubePosOnT1Board> fpgaChannels2TubePosOnT1 = new List<TubePosOnT1Board>(70);

            private int[] t1TubeOrderingMapper = { -1, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };

            /// <summary>
            /// Set T1 board
            /// </summary>
            /// <param name="mapper"></param>
            public void setT1BoardChannelMapper(int[] mapper )
            {
                t1TubeOrderingMapper = mapper;
            }
            /*
            /// <summary>
            /// Set direction of T1 board, True:Positive position, False:Reverse position
            /// </summary>
            /// <param name="direction"></param>
            public void setT1BoardDirection(bool direction)
            {
                t1BoardDirection = direction;
            }
            */
            public void setModuleHeadChannel(int[] headChannelTable)
            {
                moduleHeadFpgaChannel = headChannelTable;
                for (int i = 0; i <= headChannelTable.Max() + 3; i++ )
                {
                    TubePosOnT1Board pos;
                    pos.moduleNum = -1;
                    pos.delayLineNum = -1;
                    fpgaChannels2TubePosOnT1.Add(pos);
                }
                for(int i=0; i<headChannelTable.Length; i++)
                {
                    int lineAChannelIndex = headChannelTable[i];
                    int lineBChannelIndex = headChannelTable[i] + 2;
                    TubePosOnT1Board posLineA, posLineB;
                    posLineA.moduleNum = i;
                    posLineA.delayLineNum = 0;
                    posLineB.moduleNum = i;
                    posLineB.delayLineNum = 1;

                    fpgaChannels2TubePosOnT1[lineAChannelIndex] = posLineA;
                    fpgaChannels2TubePosOnT1[lineBChannelIndex] = posLineB;
                    fpgaChannels2TubePosOnT1[lineAChannelIndex+1] = posLineA;
                    fpgaChannels2TubePosOnT1[lineBChannelIndex+1] = posLineB;
                }
            }
            public int calcGrandTubeIndex(int channelOfDelayLine, int fpgaChannel)
            {
                int t1LaneOffset = fpgaChannels2TubePosOnT1[fpgaChannel].delayLineNum * DelayChannelNumberPerT1;
                int moduleOffset = fpgaChannels2TubePosOnT1[fpgaChannel].moduleNum    * TubeNumberPerModule;
                int t1TubeIndexIndex = channelOfDelayLine + t1LaneOffset;
                int t1TubeIndex = t1TubeOrderingMapper[t1TubeIndexIndex];
                int grandTubeIndex = t1TubeIndexIndex + moduleOffset;

                return grandTubeIndex;
            }

            private struct TubePosOnT1Board
            {
                public int delayLineNum;
                public int moduleNum;
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
