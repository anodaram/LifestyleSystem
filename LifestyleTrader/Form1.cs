using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifestyleTrader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            foreach (var eMode in (RUN_MODE[])Enum.GetValues(typeof(RUN_MODE)))
            {
                if (eMode == RUN_MODE.NONE) continue;
                cmb_mode.Items.Add(eMode.ToString());
            }
            btn_stop.Enabled = false;
            Manager.Init(this);
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            btn_start.Enabled = false;
            string sRunMode = cmb_mode.Text;
            string sSymbol = cmb_symbol.Text;
            if (sRunMode.Length < 1) DisplayState("Please select MODE");
            else if (sSymbol.Length < 1) DisplayState("Please select symbol");
            else Manager.Start((RUN_MODE)Enum.Parse(typeof(RUN_MODE), sRunMode, true),
                sSymbol, dtpicker_st.Value, dtpicker_en.Value);
            btn_stop.Enabled = true;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            btn_stop.Enabled = false;
            Manager.Stop();
            btn_start.Enabled = true;
        }

        private void btn_chart_Click(object sender, EventArgs e)
        {
            bool bRlt = Manager.ConnectChart();
            Manager.PutLog("Connect Chart " + (bRlt ? "Success" : "Failed"));
            if (bRlt)
            {
                btn_chart.Enabled = false;
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            lock (txt_log)
            {
                txt_log.Invoke((MethodInvoker)delegate
                {
                    txt_log.Text = "";
                });
            }
        }

        public void DisplayLog(string sLog)
        {
            lock (txt_log)
            {
                txt_log.Invoke((MethodInvoker)delegate
                {
                    txt_log.Text += sLog + "\r\n";
                    if (txt_log.Text.Length > TraderGlobal.MAX_LOG_DISPLAY_LENGTH)
                    {
                        txt_log.Text = txt_log.Text.Substring(TraderGlobal.MAX_LOG_DISPLAY_LENGTH / 2);
                    }
                    txt_log.SelectionStart = txt_log.Text.Length;
                    txt_log.ScrollToCaret();
                });
            }
        }

        public void DisplayState(string sState)
        {
            lock (txt_state)
            {
                txt_state.Invoke((MethodInvoker)delegate
                {
                    txt_state.Text = sState;
                });
            }
        }

        public void DisplayPerformance(double dPercent)
        {
            lock (txt_perf)
            {
                txt_perf.Invoke((MethodInvoker)delegate
                {
                    txt_perf.Text = ((int)(dPercent * 100 + 0.5)).ToString() + " %";
                });
            }
        }

        public void SetSymbolList(List<string> lstSymbol)
        {
            cmb_symbol.Items.Clear();
            foreach (var sSymbol in lstSymbol)
            {
                cmb_symbol.Items.Add(sSymbol);
            }
        }
    }
}
