using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuonTdaq
{
    class fpgaChannels
    {
        private List<int> ChannelList = new List<int>(65);
        private ulong ChannelBin;
        private bool[] ChannelFlgs = new bool[100];
        private int channelCount  = 0;

        public fpgaChannels()
        {
            for (int i = 0; i < ChannelFlgs.Length; i++) { ChannelFlgs[i] = false; }
            return;
        }

        private void Clear()
        {
            ChannelList = new List<int>(65);
            ChannelBin = 0;
            for (int i = 0; i < ChannelFlgs.Length; i++) { ChannelFlgs[i] = false; }
            channelCount = 0;
            return;
        }

        public void setChannels(int[] args)
        {
            Clear();
            foreach (int arg in args)
            {
                addChannels(arg);
            }
        }

        public void addChannels( int ch )
        {
            ChannelList.Add(ch);
            ChannelBin |= (ulong)1 << ch;
            ChannelFlgs[ch] = true;
            channelCount++;
        }

        public bool isChannelExist( int ch )
        {
            return ChannelFlgs[ch];
        }

        public bool compareChannel(ulong ChannelBin)
        {
            if (ChannelBin == this.ChannelBin) { return true; }
            else { return false; }
        }

        public int getChannelCount() { return channelCount; }
        public ulong getChannelBin() { return ChannelBin; }
    }

}

namespace Cs_DL_Analysis_Form
{
    /// <summary>
    /// Static class for channel filtering
    /// </summary>
    class ChannelFilter
    {
        public static int[] ChannelList;
        public static ulong ChannelBin;
        public static bool[] ChannelFlgs = new bool[100];
        public static int refelenceChannel = 7;

        public ChannelFilter()
        {
            return;
        }

        static public void setValidChannels(int[] args)
        {
            ChannelList = args;
            for (int i = 0; i < ChannelFlgs.Length; i++) { ChannelFlgs[i] = false; }
            foreach (int arg in args)
            {
                ChannelBin |= (ulong)1 << arg;
                ChannelFlgs[arg] = true;
            }
        }

        static public bool check(int ch)
        {
            if ((((ulong)1 << ch) & ChannelBin) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static public string showValidChannels()
        {
            string str = "Valid Cnannel List\n";
            foreach (int num in ChannelList)
            {
                str += num.ToString("##") + " ";
            }
            return str;
        }
        static public new string ToString()
        {
            string str = "Valid Cnannel List:\n";
            foreach (int num in ChannelList)
            {
                str += num.ToString("##") + " ";
            }
            return str;
        }
    }
}
