using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MuonTdaq.DetectorGeometory;

namespace Cs_DL_Analysis_Form
{
    public partial class Form1 : Form
    {
        List<string> filelist = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            foreach (string item in (string[])e.Data.GetData(DataFormats.FileDrop))
            {
                listBox1.Items.Add(System.IO.Path.GetFileName(item));
                filelist.Add(item);
            }
        }

        void analyse(string filename, int[] validChs, int refCh)
        {
            if (filelist.Count < 1) { return; }
            StreamReader sr = new StreamReader(filename);

            MuData temp = new MuData();
            List<MuData> buff = new List<MuData>();
            List<MuData> timeGroupeBuff = new List<MuData>();

            ChannelFilter.setValidChannels(validChs);

            StreamWriter sw = new StreamWriter(Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar + "out_" + Path.GetFileName(filename));
            StreamWriter sw2 = new StreamWriter(Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar + "spect_" + Path.GetFileName(filename));
            StreamWriter sw3 = new StreamWriter(Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar + "raw_" + Path.GetFileName(filename));


            do
            {
                temp = new MuData(sr.ReadLine());
                if (ChannelFilter.check(temp.ch))
                {
                    buff.Add(temp);
                }
            } while (!sr.EndOfStream);

            bool tgFlg = false;
            for (int i = 1; i < buff.Count; i++)
            {
                long timeDiff = Math.Abs((buff[i].time - buff[i - 1].time));
                if ( (timeDiff) < 5000 )
                {
                    timeGroupeBuff.Add(buff[i - 1]);
                    tgFlg = true;
                }
                else
                {
                    if (tgFlg == true)
                    {
                        timeGroupeBuff.Add(buff[i-1]);
                        MuData[] timeGroupe = timeGroupeBuff.ToArray();
                        Array.Sort(timeGroupe);
                        timeGroupeBuff = new List<MuData>();
                        MudSet tgSet = new MudSet(timeGroupe);

                        if (calcDelayTimeCbx.Checked == true)
                        {
                            tgSet.calcDelayTime(validChs);
                        }

                        if (vthCorrectionCbx.Checked == true)
                        {
                            tgSet.generateVthData(validChs);
                        }
                        else
                        {
                            tgSet.dataBuffer = tgSet.dataSet;
                        }

                        tgSet.generateDriftTimeSpectrum(refCh);

                        if (tgSet.driftTimeSpectrums != null)
                        {
                            foreach (MuData data in tgSet.driftTimeSpectrums)
                            {
                                sw2.WriteLine(data.ToString());
                            }
                        }

                        List<MuData> output = tgSet.dataBuffer;


                        foreach (MuData data in output)
                        {
                            sw.WriteLine(data.ToString());
                        }


                        // debug code
                        List<MuData> output2 = tgSet.dataSet;
                        foreach (MuData data in output2)
                        {
                            sw3.WriteLine(data.ToString());
                        }
                        // debug code end


                    }
                }
            }

            sw.Close();
            sw2.Close();
            sw3.Close();
        }

        private void analyse2(string filename, int[] validChs, int refCh)
        {
            if (filelist.Count < 1) { return; }
            StreamReader sr = new StreamReader(filename);

            MuData temp = new MuData();
            List<MuData> buff = new List<MuData>();
            List<MuData> timeGroupeBuff = new List<MuData>();

            ChannelFilter.setValidChannels(validChs);

            DelayTimeConverter dtc = new DelayTimeConverter();
            TubePosition tubepos = new TubePosition();

            int[] t1DelayChannelTubeIndexMapper = { -1, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            tubepos.setT1BoardChannelMapper(t1DelayChannelTubeIndexMapper);
            int[] moduleNumber2fpgaChannelMapper = new int[30];
            // 2 axis measurement
            moduleNumber2fpgaChannelMapper[0] = 14;
            moduleNumber2fpgaChannelMapper[8] = 18;
            moduleNumber2fpgaChannelMapper[16] = 25;
            moduleNumber2fpgaChannelMapper[4] = 36;
            moduleNumber2fpgaChannelMapper[12] = 40;
            moduleNumber2fpgaChannelMapper[20] = 47;

            tubepos.setModuleHeadChannel(moduleNumber2fpgaChannelMapper);

            StreamWriter sw = new StreamWriter(Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar + "out2_" + Path.GetFileName(filename));

            do
            {
                temp = new MuData(sr.ReadLine());
                if (ChannelFilter.check(temp.ch))
                {
                    buff.Add(temp);
                }
            } while (!sr.EndOfStream);

            bool tgFlg = false;
            for (int i = 1; i < buff.Count; i++)
            {
                long timeDiff = Math.Abs((buff[i].time - buff[i - 1].time));
                if ((timeDiff) < 5000)
                {
                    timeGroupeBuff.Add(buff[i - 1]);
                    tgFlg = true;
                }
                else
                {
                    if (tgFlg == true)
                    {
                        timeGroupeBuff.Add(buff[i - 1]);
                        MuData[] timeGroupe = timeGroupeBuff.ToArray();
                        Array.Sort(timeGroupe);
                        timeGroupeBuff = new List<MuData>();
                        MudSet tgSet = new MudSet(timeGroupe);

                        tgSet.delayTimeConv = dtc;
                        tgSet.tubePosition = tubepos;

                        if (calcDelayTimeCbx.Checked == true)
                        {
                            tgSet.calcDelayTime(validChs);
                        }

                        if (vthCorrectionCbx.Checked == true)
                        {
                            tgSet.generateVthData(validChs);
                        }
                        else
                        {
                            tgSet.dataBuffer = tgSet.dataSet;
                        }

                        // check coincidence num
                        if ( tgSet.dataBuffer.Count != 6) { continue; }

                        tgSet.convertDelayTime2Position();

                        // check coincidence num
                        if (tgSet.dataBuffer.Count != 6) { continue; }

                        List<MuData> output = tgSet.dataBuffer;
                        foreach (MuData data in output)
                        {
                            sw.WriteLine(data.ToStringChTime());
                        }


                    }
                }
            }

            sw.Close();
        }

        private void analyseBtn_Click(object sender, EventArgs e)
        {
            List<int> validChslist = new List<int>(70);
            int refCh = 0;
            if (true)
            {
                int.TryParse(refChTbx.Text, out refCh);
                if (refCh <  0) { return; }
                if (refCh > 65) { return; }
            }
            string[] str = validChTbx.Text.Split(',');
            for(int i=0; i < str.Length ; i++ ){
                int tmp;
                int.TryParse(str[i], out tmp);
                validChslist.Add(tmp);
            }
            int[] validChs = validChslist.ToArray();

            foreach(string filename in filelist)
            {
                analyse(filename, validChs, refCh);
            }
        }

        private void analyse2Btn_Click(object sender, EventArgs e)
        {
            List<int> validChslist = new List<int>(70);
            int refCh = 0;
            if (true)
            {
                int.TryParse(refChTbx.Text, out refCh);
                if (refCh < 0) { return; }
                if (refCh > 65) { return; }
            }
            string[] str = validChTbx.Text.Split(',');
            for (int i = 0; i < str.Length; i++)
            {
                int tmp;
                int.TryParse(str[i], out tmp);
                validChslist.Add(tmp);
            }
            int[] validChs = validChslist.ToArray();

            foreach (string filename in filelist)
            {
                analyse2(filename, validChs, refCh);
            }
        }

        private void listClearBtn_Click(object sender, EventArgs e)
        {
            filelist = new List<string>();
            listBox1.Items.Clear();
        }

        private void fileIntegrateBtn_Click(object sender, EventArgs e)
        {
            if (filelist.Count < 1) { return; }
            StreamWriter sw = new StreamWriter(Path.GetDirectoryName(filelist[0]) + Path.DirectorySeparatorChar + "integrated.csv");
            foreach (string filename in filelist)
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    sw.Write(sr.ReadToEnd());
                }
            }
            sw.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dbgBtn_Click(object sender, EventArgs e)
        {
            foreach (string filename in filelist)
            {
                dbg_process(filename);
            }
        }

        private void dbg_process(string filename)
        {
            if (filelist.Count < 1) { return; }
            StreamReader sr = new StreamReader(filename);

            MuData temp = new MuData();
            List<MuData> buff = new List<MuData>();
            StreamWriter sw = new StreamWriter(Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar + "HexGhx_" + Path.GetFileName(filename));

            do
            {
                temp = new MuData(sr.ReadLine());
                buff.Add(temp);
            } while (!sr.EndOfStream);

            foreach(MuData tpp in buff)
            {
                sw.WriteLine(tpp.ToString());
            }
            sw.Close();
            sr.Close();
        }


    }
}