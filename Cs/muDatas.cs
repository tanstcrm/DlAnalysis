using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuonTdaq.TubePosition;

namespace Cs_DL_Analysis_Form
{
    class MudSet
    {
        //public MuData[] dataSet = null;
        public List<MuData> dataSet = new List<MuData>();
        MuData buff = new MuData();
        public List<MuData> dataBuffer = new List<MuData>();
        public List<List<MuData>> vthDataSet = new List<List<MuData>>();
        public List<MuData> driftTimeSpectrums = null;
        public int[] vthHeadList = { 1, 2, 5, 7, 9, 12, 14, 16, 18, 20, 23, 25, 27, 29, 31 };
        static public int vthA { get; set; } = 145;
        static public int vthB { get; set; } = 208;

        public DelayTimeConverter delayTimeConv;
        public GeometricaIdentifier geomericaId;
        public T1boardIdentifier t1boardId;

        MudSet() { }
        public MudSet(MuData[] datas)
        {
            foreach (MuData data in datas)
            {
                if (data.dt1 < 1000)
                {
                    dataSet.Add(data);
                }
            }
        }

        public MudSet(List<MuData> datas)
        {
            foreach (MuData data in datas)
            {
                if (data.dt1 < 1000)
                {
                    dataSet.Add(data);
                }
            }
        }

        /// <summary>
        /// Update "dataSet" calculate dt1 and dt2 from serial (without drifttime information (dt1)) data. not for regular data.
        /// </summary>
        /// <param name="chlist">Valid Channel List</param>
        public void calcDelayTime(int[] chlist)
        {
            List<MuData> calculatedData = new List<MuData>();
            MuData headData = new MuData();
            MuData tmpBuff = new MuData();
            int pulseCnt = 0;
            

            foreach(int ch in chlist)
            {
                tmpBuff = new MuData();
                headData = new MuData();
                pulseCnt = 0;
                foreach (MuData tmp in dataSet)
                {
                    if(tmp.ch != ch) { continue; }
                    if(headData.time == 0)
                    {
                        headData = tmp;
                        continue;
                    }
                    if( (tmp.time - headData.time) < 5000)
                    {
                        if(pulseCnt == 0)
                        {
                            tmpBuff.ch = headData.ch;
                            // debug test code : changed headData.time -> tmp.time
                            tmpBuff.time = tmp.time;
                            tmpBuff.dt1 = (int)(tmp.time - headData.time);
                            pulseCnt++;
                            calculatedData.Add((MuData)tmpBuff.Clone());
                            //tmpBuff = new MuData();
                        }
                        else if(pulseCnt == 1)
                        {
                            tmpBuff.ch = headData.ch;
                            // debug test code : changed headData.time -> tmp.time
                            tmpBuff.time = tmp.time;
                            tmpBuff.dt2 = (int)(tmp.time - headData.time);
                            pulseCnt++;
                            // update latest data of List
                            calculatedData[calculatedData.Count - 1] = (MuData)tmpBuff.Clone();
                            tmpBuff = new MuData();
                        }
                        else
                        {
                            pulseCnt = 0;
                            tmpBuff = new MuData();
                            break;
                        }
                    }
                    else
                    { 
                        pulseCnt = 0;
                        headData = tmp;
                    }
                }
            }
            dataSet = calculatedData;
        }


        void vthSet (List<MuData> datas, int[] chlist)
        {
            List<MuData> tmp = new List<MuData>(4);

            foreach(int ch in chlist)
            {
                // FPGA の仕様変更に要注意！！！
                int debug = Array.IndexOf(vthHeadList, ch);
                if (debug < 0) { continue; } // 工夫してバイナリーサーチにするとO(n) から O(log n)高速化します。
                
                foreach(MuData data in datas)
                {
                    if ( data.ch == ch)
                    {
                        tmp.Add(data);
                        break;
                    }
                }
                foreach (MuData data in datas)
                {
                    if (data.ch == ch+1)
                    {
                        tmp.Add(data);
                        break;
                    }
                }
                if(tmp.Count < 2) { continue; }
                else { vthDataSet.Add(tmp); }
                tmp = new List<MuData>();
            }
        }

        void vthSet (int[] chlist)
        {
            List<MuData> datas = dataSet;
            vthSet(datas, chlist);
        }

        void vthCalc()
        {
            long timeZero;
            int  dtimeZero;
            foreach(List<MuData> tmp in vthDataSet)
            {
                MuData data1 = tmp[0];
                MuData data2 = tmp[1];
                // for abs time
                long timeDiff = data2.time - data1.time;
                double slope = (double)timeDiff / (double)(vthB - vthA);
                timeZero = data1.time - (long) ((double)vthA * slope);
                // for delay time
                double dslope = (double) ( (data2.dt1 - data1.dt1) / (double)(vthB - vthA) );
                dtimeZero = (int) ( (int)data1.dt1 - (int) ((double)vthA * dslope));
                //debug code
                dtimeZero = data1.dt1;

                buff.ch = data1.ch;
                buff.time = timeZero;
                buff.dt1 = dtimeZero;
                buff.dt2 = 0;
                dataBuffer.Add((MuData)buff.Clone());
            }
        }


        public void generateVthData(int[] chlist)
        {
            vthSet(chlist);
            vthCalc();
        }

        public void generateDriftTimeSpectrum( int referenceChannel)
        {
            //ulong chBin = 0;
            long refTime = 0;
            bool existRefch = false;

            if(dataBuffer.Count < 2) { return; }
            
            foreach(MuData data in dataBuffer)
            {
                if (data.ch == referenceChannel)
                {
                    refTime = data.time;
                    existRefch = true;
                }
            }

            if(existRefch == false) { return; }

            List<MuData> spectrum = new List<MuData>();
            foreach (MuData data in dataBuffer)
            {
                if(data.ch == referenceChannel) { continue; }
                MuData tmp = new MuData();
                tmp.ch = data.ch;
                //tmp.time = refTime - data.time - refTime;
                tmp.time = data.time - refTime;
                spectrum.Add(tmp);
            }
            driftTimeSpectrums = spectrum;
            return;
        }

        public void convertDelayTime2Position()
        {
            int[] delayFromCh1 = {0,0, 70, 165, 255, 345, 435, 525, 615, 705, 795, 885, 970};
            bool badDatasetFlg = false;
            foreach (MuData data in dataBuffer)
            {
                int ch = delayTimeConv.delayTime2Channel(data.dt1);
                if (ch < -1)
                {
                    badDatasetFlg = true;
                    break;
                }
                data.time = data.time - delayFromCh1[ch];

                if ( data.ch == 14 || data.ch == 25 || data.ch == 29) { ch += 12; }
                int moduleNumber = t1boardId.fpgaChannel2ModuleNumber(data.ch);
                int geoCh = t1boardId.boardChannel2GeometoricalBoardChannel(ch);
                int geopos = geomericaId.calcGeometoricalChannel(moduleNumber, geoCh);

                data.ch = geopos;
            }
            if (badDatasetFlg == true)
            {
                dataBuffer.Clear();
            }
        }

                
    }

    public class MuData : IComparable, ICloneable
    {
        public string str;
        public int ch, dt1, dt2;
        public long time;
        public bool valid;

        /*
        /// <summary>
        /// dataSplitter Ver.1
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool dataSplitter(string str)
        {
            if (str == null) { return false; }
            string[] tmp = str.Split(',');
            if (tmp.Length < 5) { return false; }
            if (tmp[4] == "") { return false; }
            this.ch = int.Parse(tmp[1], System.Globalization.NumberStyles.HexNumber) & 0xFFFF;
            this.time = long.Parse(tmp[2], System.Globalization.NumberStyles.HexNumber);
            this.dt1 = int.Parse(tmp[3]);
            this.dt2 = int.Parse(tmp[4]);
            return true;
        }

    */

        public MuData()
        {
        }

        public MuData (string str)
        {
            dataSplitter(str);
        }

        /// <summary>
        /// splitter Ver.2
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool dataSplitter(string str)
        {
            if (str == null) { return false; }
            string[] tmp = str.Split(',');
            if (tmp.Length < 4) { return false; }
            this.ch = int.Parse(tmp[0]);

            // debug
            //this.time = long.Parse(tmp[1]);  // ToDo remove comment out
            //long planeTime = long.Parse(tmp[1]);
            
            long planeTime = long.Parse(tmp[1]);
            int timeGhz    = (int) planeTime & 0x7F;
            long timeMHz = planeTime / 128;
            this.time = timeMHz * 100 + timeGhz;
            
            //this.time = (int)planeTime & 0xFF;



            // debug end


            this.dt1 = int.Parse(tmp[2]);
            this.dt2 = int.Parse(tmp[3]);
            return true;
        }

        public object Clone()
        {
            MuData ret = new MuData();

            ret.ch = this.ch;
            ret.dt1 = this.dt1;
            ret.dt2 = this.dt2;
            ret.time = this.time;
            ret.str = this.str;
            ret.valid = this.valid;

            return ret;
        }

        public override string ToString()
        {
            return ch.ToString() + "," + time.ToString() + "," + dt1.ToString() + "," + dt2.ToString();
        }

        public string ToStringChTime()
        {
            return ch.ToString() + "," + time.ToString();
        }

        public int CompareTo(object obj)
        {
            return time.CompareTo(((MuData)obj).time);
        }
    }
}
