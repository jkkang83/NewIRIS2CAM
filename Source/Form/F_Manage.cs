using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace M1Wide
{
    public partial class F_Manage : Form
    {
        public DLN Dln { get { return STATIC.Dln; } }
        public Spec Spec { get { return STATIC.Rcp.Spec; } }
        public Process Process { get { return STATIC.Process; } }
        public Model Model { get { return STATIC.Rcp.Model; } }
        public Option Option { get { return STATIC.Rcp.Option; } }
        public List<Button> mInfoBtn = new List<Button>();
        public List<Button> SmallResBtn = new List<Button>();
        private List<TextBox> mViewLog = new List<TextBox>();
        public bool EmgOn = false;
        public bool isrunning = false;
        public F_Manage()
        {
            InitializeComponent();

            YieldChart.ChartAreas[0].Position.X = 0;
            YieldChart.ChartAreas[0].Position.Y = 0;
            YieldChart.ChartAreas[0].Position.Height = 68;
            YieldChart.ChartAreas[0].Position.Width = 100;
            YieldChart.Titles.Clear();
            YieldChart.Titles.Add("항목별불량비");
            YieldChart.Series[0].LegendText = "#VALX (#PERCENT)";
            YieldChart.Series[0]["PieLabelStyle"] = "Outside";
            YieldChart.Series[0].BorderWidth = 1;
            YieldChart.Series[0].BorderColor = Color.FromArgb(64, 64, 64);
            YieldChart.Legends[0].Enabled = true;
            YieldChart.Legends[0].Docking = Docking.Bottom;
            YieldChart.Legends[0].Alignment = StringAlignment.Far;
        }


        public void ChartVisible()
        {
            if (!STATIC.Rcp.Option.ChartVisible)
            {
                chartLinearity_L.Visible = false;
                chartLinearity_R.Visible = false;
                chartDecenter_L.Visible = false;
                chartDecenter_R.Visible = false;
                chartSettling1420_L.Visible = false;
                chartSettling2028_L.Visible = false;
                chartSettling2840_L.Visible = false;
                chartSettling1420_R.Visible = false;
                chartSettling2028_R.Visible = false;
                chartSettling2840_R.Visible = false;
            }
            else
            {
                chartLinearity_L.Visible = true;
                chartLinearity_R.Visible = true;
                chartDecenter_L.Visible = true;
                chartDecenter_R.Visible = true;
                chartSettling1420_L.Visible = true;
                chartSettling2028_L.Visible = true;
                chartSettling2840_L.Visible = true;
                chartSettling1420_R.Visible = true;
                chartSettling2028_R.Visible = true;
                chartSettling2840_R.Visible = true;

            }
        }
        public void ClearDecenterChart()
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {

                    chartDecenter_L.Series[0].Points.Clear();
                    chartDecenter_R.Series[0].Points.Clear();


                });
            }
            else
            {
                chartDecenter_L.Series[0].Points.Clear();
                chartDecenter_R.Series[0].Points.Clear();
            }
        }
        public void ClearChart()
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    chartLinearity_L.Series[0].Points.Clear();
                    chartLinearity_L.Series[1].Points.Clear();
                    chartLinearity_R.Series[0].Points.Clear();
                    chartLinearity_R.Series[1].Points.Clear();
                    chartDecenter_L.Series[0].Points.Clear();
                    chartDecenter_R.Series[0].Points.Clear();
                    chartSettling1420_L.Series[0].Points.Clear();
                    chartSettling1420_L.Series[1].Points.Clear();
                    chartSettling2028_L.Series[0].Points.Clear();
                    chartSettling2028_L.Series[1].Points.Clear();
                    chartSettling2840_L.Series[0].Points.Clear();
                    chartSettling2840_L.Series[1].Points.Clear();
                    chartSettling1420_R.Series[0].Points.Clear();
                    chartSettling1420_R.Series[1].Points.Clear();
                    chartSettling2028_R.Series[0].Points.Clear();
                    chartSettling2028_R.Series[1].Points.Clear();
                    chartSettling2840_R.Series[0].Points.Clear();
                    chartSettling2840_R.Series[1].Points.Clear();

                });
            }
            else
            {
                chartLinearity_L.Series[0].Points.Clear();
                chartLinearity_L.Series[1].Points.Clear();
                chartLinearity_R.Series[0].Points.Clear();
                chartLinearity_R.Series[1].Points.Clear();
                chartDecenter_L.Series[0].Points.Clear();
                chartDecenter_R.Series[0].Points.Clear();
                chartSettling1420_L.Series[0].Points.Clear();
                chartSettling1420_L.Series[1].Points.Clear();
                chartSettling2028_L.Series[0].Points.Clear();
                chartSettling2028_L.Series[1].Points.Clear();
                chartSettling2840_L.Series[0].Points.Clear();
                chartSettling2840_L.Series[1].Points.Clear();
                chartSettling1420_R.Series[0].Points.Clear();
                chartSettling1420_R.Series[1].Points.Clear();
                chartSettling2028_R.Series[0].Points.Clear();
                chartSettling2028_R.Series[1].Points.Clear();
                chartSettling2840_R.Series[0].Points.Clear();
                chartSettling2840_R.Series[1].Points.Clear();
            }
        }
        public void DrawLinearityChart(int index, int[] x, double[] y, int[] x2, double[] y2)
        {
            if (index == 0)
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate
                    {
                        chartLinearity_L.Series[0].Points.DataBindXY(x, y);
                        chartLinearity_L.Series[1].Points.DataBindXY(x2, y2);
                    });
                }
                else
                {
                    chartLinearity_L.Series[0].Points.DataBindXY(x, y);
                    chartLinearity_L.Series[1].Points.DataBindXY(x2, y2);
                }
            }
            else
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate
                    {
                        chartLinearity_R.Series[0].Points.DataBindXY(x, y);
                        chartLinearity_R.Series[1].Points.DataBindXY(x2, y2);
                    });
                }
                else
                {
                    chartLinearity_R.Series[0].Points.DataBindXY(x, y);
                    chartLinearity_R.Series[1].Points.DataBindXY(x2, y2);
                }
            }




        }
        public void DrawDecenterChart(int index, double[] x, double[] y)
        {
            if (index == 0)
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate
                    {
                        chartDecenter_L.Series[0].Points.AddXY(x[0], y[0]);
                        chartDecenter_L.Series[0].Points[0].Color = Color.Red;
                        chartDecenter_L.Series[0].Points.AddXY(x[1], y[1]);
                        chartDecenter_L.Series[0].Points[1].Color = Color.Orange;
                        chartDecenter_L.Series[0].Points.AddXY(x[2], y[2]);
                        chartDecenter_L.Series[0].Points[2].Color = Color.Yellow;
                        chartDecenter_L.Series[0].Points.AddXY(x[3], y[3]);
                        chartDecenter_L.Series[0].Points[3].Color = Color.Green;
                        chartDecenter_L.Series[0].Points.AddXY(x[4], y[4]);
                        chartDecenter_L.Series[0].Points[4].Color = Color.Blue;
                        chartDecenter_L.Series[0].Points.AddXY(x[5], y[5]);
                        chartDecenter_L.Series[0].Points[5].Color = Color.Indigo;
                        chartDecenter_L.Series[0].Points.AddXY(x[6], y[6]);
                        chartDecenter_L.Series[0].Points[6].Color = Color.Purple;
                        chartDecenter_L.Series[0].Points.AddXY(x[7], y[7]);
                        chartDecenter_L.Series[0].Points[7].Color = Color.DarkGray;

                    });
                }
                else
                {
                    chartDecenter_L.Series[0].Points.AddXY(x[0], y[0]);
                    chartDecenter_L.Series[0].Points[0].Color = Color.Red;
                    chartDecenter_L.Series[0].Points.AddXY(x[1], y[1]);
                    chartDecenter_L.Series[0].Points[1].Color = Color.Orange;
                    chartDecenter_L.Series[0].Points.AddXY(x[2], y[2]);
                    chartDecenter_L.Series[0].Points[2].Color = Color.Yellow;
                    chartDecenter_L.Series[0].Points.AddXY(x[3], y[3]);
                    chartDecenter_L.Series[0].Points[3].Color = Color.Green;
                    chartDecenter_L.Series[0].Points.AddXY(x[4], y[4]);
                    chartDecenter_L.Series[0].Points[4].Color = Color.Blue;
                    chartDecenter_L.Series[0].Points.AddXY(x[5], y[5]);
                    chartDecenter_L.Series[0].Points[5].Color = Color.Indigo;
                    chartDecenter_L.Series[0].Points.AddXY(x[6], y[6]);
                    chartDecenter_L.Series[0].Points[6].Color = Color.Purple;
                    chartDecenter_L.Series[0].Points.AddXY(x[7], y[7]);
                    chartDecenter_L.Series[0].Points[7].Color = Color.DarkGray;
                }
            }
            else
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate
                    {
                        chartDecenter_R.Series[0].Points.AddXY(x[0], y[0]);
                        chartDecenter_R.Series[0].Points[0].Color = Color.Red;
                        chartDecenter_R.Series[0].Points.AddXY(x[1], y[1]);
                        chartDecenter_R.Series[0].Points[1].Color = Color.Orange;
                        chartDecenter_R.Series[0].Points.AddXY(x[2], y[2]);
                        chartDecenter_R.Series[0].Points[2].Color = Color.Yellow;
                        chartDecenter_R.Series[0].Points.AddXY(x[3], y[3]);
                        chartDecenter_R.Series[0].Points[3].Color = Color.Green;
                        chartDecenter_R.Series[0].Points.AddXY(x[4], y[4]);
                        chartDecenter_R.Series[0].Points[4].Color = Color.Blue;
                        chartDecenter_R.Series[0].Points.AddXY(x[5], y[5]);
                        chartDecenter_R.Series[0].Points[5].Color = Color.Indigo;
                        chartDecenter_R.Series[0].Points.AddXY(x[6], y[6]);
                        chartDecenter_R.Series[0].Points[6].Color = Color.Purple;
                        chartDecenter_R.Series[0].Points.AddXY(x[7], y[7]);
                        chartDecenter_R.Series[0].Points[7].Color = Color.DarkGray;
                    });
                }
                else
                {
                    chartDecenter_R.Series[0].Points.AddXY(x[0], y[0]);
                    chartDecenter_R.Series[0].Points[0].Color = Color.Red;
                    chartDecenter_R.Series[0].Points.AddXY(x[1], y[1]);
                    chartDecenter_R.Series[0].Points[1].Color = Color.Orange;
                    chartDecenter_R.Series[0].Points.AddXY(x[2], y[2]);
                    chartDecenter_R.Series[0].Points[2].Color = Color.Yellow;
                    chartDecenter_R.Series[0].Points.AddXY(x[3], y[3]);
                    chartDecenter_R.Series[0].Points[3].Color = Color.Green;
                    chartDecenter_R.Series[0].Points.AddXY(x[4], y[4]);
                    chartDecenter_R.Series[0].Points[4].Color = Color.Blue;
                    chartDecenter_R.Series[0].Points.AddXY(x[5], y[5]);
                    chartDecenter_R.Series[0].Points[5].Color = Color.Indigo;
                    chartDecenter_R.Series[0].Points.AddXY(x[6], y[6]);
                    chartDecenter_R.Series[0].Points[6].Color = Color.Purple;
                    chartDecenter_R.Series[0].Points.AddXY(x[7], y[7]);
                    chartDecenter_R.Series[0].Points[7].Color = Color.DarkGray;
                }
            }




        }

        public void DrawSettlingChart(int ch, int index, List<SettlingData> s, List<SettlingData> s2, int min1, int max1, int min2, int max2)
        {

            int[] val = new int[4] { min1, min2, max1, max2 };
            int max = val.Max() + 300;
            int min = val.Min() - 300;

            switch (index)
            {
                case 0:
                    if (ch == 0)
                    {
                        if (InvokeRequired)
                        {
                            BeginInvoke((MethodInvoker)delegate
                            {

                                chartSettling1420_L.ChartAreas[0].AxisY.Maximum = max;
                                chartSettling1420_L.ChartAreas[0].AxisY.Minimum = min;

                                for (int i = 0; i < s.Count - 1; i++)
                                    chartSettling1420_L.Series[0].Points.AddXY(s[i].Time, s[i].ReadHall);
                                for (int i = 0; i < s2.Count - 1; i++)
                                    chartSettling1420_L.Series[1].Points.AddXY(s2[i].Time, s2[i].ReadHall);

                            });
                        }
                        else
                        {
                            chartSettling1420_L.ChartAreas[0].AxisY.Maximum = max;
                            chartSettling1420_L.ChartAreas[0].AxisY.Minimum = min;
                            for (int i = 0; i < s.Count - 1; i++)
                                chartSettling1420_L.Series[0].Points.AddXY(s[i].Time, s[i].ReadHall);
                            for (int i = 0; i < s2.Count - 1; i++)
                                chartSettling1420_L.Series[1].Points.AddXY(s2[i].Time, s2[i].ReadHall);
                        }
                    }
                    else
                    {
                        if (InvokeRequired)
                        {
                            BeginInvoke((MethodInvoker)delegate
                            {
                                chartSettling1420_R.ChartAreas[0].AxisY.Maximum = max;
                                chartSettling1420_R.ChartAreas[0].AxisY.Minimum = min;
                                for (int i = 0; i < s.Count - 1; i++)
                                    chartSettling1420_R.Series[0].Points.AddXY(s[i].Time, s[i].ReadHall);
                                for (int i = 0; i < s2.Count - 1; i++)
                                    chartSettling1420_R.Series[1].Points.AddXY(s2[i].Time, s2[i].ReadHall);

                            });
                        }
                        else
                        {
                            chartSettling1420_R.ChartAreas[0].AxisY.Maximum = max;
                            chartSettling1420_R.ChartAreas[0].AxisY.Minimum = min;
                            for (int i = 0; i < s.Count - 1; i++)
                                chartSettling1420_R.Series[0].Points.AddXY(s[i].Time, s[i].ReadHall);
                            for (int i = 0; i < s2.Count - 1; i++)
                                chartSettling1420_R.Series[1].Points.AddXY(s2[i].Time, s2[i].ReadHall);
                        }

                    }

                    break;
                case 1:
                    if (ch == 0)
                    {
                        if (InvokeRequired)
                        {
                            BeginInvoke((MethodInvoker)delegate
                            {
                                chartSettling2028_L.ChartAreas[0].AxisY.Maximum = max;
                                chartSettling2028_L.ChartAreas[0].AxisY.Minimum = min;
                                for (int i = 0; i < s.Count - 1; i++)
                                    chartSettling2028_L.Series[0].Points.AddXY(s[i].Time, s[i].ReadHall);
                                for (int i = 0; i < s2.Count - 1; i++)
                                    chartSettling2028_L.Series[1].Points.AddXY(s2[i].Time, s2[i].ReadHall);

                            });
                        }
                        else
                        {
                            chartSettling2028_L.ChartAreas[0].AxisY.Maximum = max;
                            chartSettling2028_L.ChartAreas[0].AxisY.Minimum = min;
                            for (int i = 0; i < s.Count - 1; i++)
                                chartSettling2028_L.Series[0].Points.AddXY(s[i].Time, s[i].ReadHall);
                            for (int i = 0; i < s2.Count - 1; i++)
                                chartSettling2028_L.Series[1].Points.AddXY(s2[i].Time, s2[i].ReadHall);
                        }
                    }
                    else
                    {
                        if (InvokeRequired)
                        {
                            BeginInvoke((MethodInvoker)delegate
                            {
                                chartSettling2028_R.ChartAreas[0].AxisY.Maximum = max;
                                chartSettling2028_R.ChartAreas[0].AxisY.Minimum = min;
                                for (int i = 0; i < s.Count - 1; i++)
                                    chartSettling2028_R.Series[0].Points.AddXY(s[i].Time, s[i].ReadHall);
                                for (int i = 0; i < s2.Count - 1; i++)
                                    chartSettling2028_R.Series[1].Points.AddXY(s2[i].Time, s2[i].ReadHall);

                            });
                        }
                        else
                        {
                            chartSettling2028_R.ChartAreas[0].AxisY.Maximum = max;
                            chartSettling2028_R.ChartAreas[0].AxisY.Minimum = min;
                            for (int i = 0; i < s.Count - 1; i++)
                                chartSettling2028_R.Series[0].Points.AddXY(s[i].Time, s[i].ReadHall);
                            for (int i = 0; i < s2.Count - 1; i++)
                                chartSettling2028_R.Series[1].Points.AddXY(s2[i].Time, s2[i].ReadHall);
                        }

                    }
                    break;
                case 2:
                    if (ch == 0)
                    {
                        if (InvokeRequired)
                        {
                            BeginInvoke((MethodInvoker)delegate
                            {
                                chartSettling2840_L.ChartAreas[0].AxisY.Maximum = max;
                                chartSettling2840_L.ChartAreas[0].AxisY.Minimum = min;
                                for (int i = 0; i < s.Count - 1; i++)
                                    chartSettling2840_L.Series[0].Points.AddXY(s[i].Time, s[i].ReadHall);
                                for (int i = 0; i < s2.Count - 1; i++)
                                    chartSettling2840_L.Series[1].Points.AddXY(s2[i].Time, s2[i].ReadHall);

                            });
                        }
                        else
                        {
                            chartSettling2840_L.ChartAreas[0].AxisY.Maximum = max;
                            chartSettling2840_L.ChartAreas[0].AxisY.Minimum = min;
                            for (int i = 0; i < s.Count - 1; i++)
                                chartSettling2840_L.Series[0].Points.AddXY(s[i].Time, s[i].ReadHall);
                            for (int i = 0; i < s2.Count - 1; i++)
                                chartSettling2840_L.Series[1].Points.AddXY(s2[i].Time, s2[i].ReadHall);
                        }
                    }
                    else
                    {
                        if (InvokeRequired)
                        {
                            BeginInvoke((MethodInvoker)delegate
                            {
                                chartSettling2840_R.ChartAreas[0].AxisY.Maximum = max;
                                chartSettling2840_R.ChartAreas[0].AxisY.Minimum = min;
                                for (int i = 0; i < s.Count - 1; i++)
                                    chartSettling2840_R.Series[0].Points.AddXY(s[i].Time, s[i].ReadHall);
                                for (int i = 0; i < s2.Count - 1; i++)
                                    chartSettling2840_R.Series[1].Points.AddXY(s2[i].Time, s2[i].ReadHall);

                            });
                        }
                        else
                        {
                            chartSettling2840_R.ChartAreas[0].AxisY.Maximum = max;
                            chartSettling2840_R.ChartAreas[0].AxisY.Minimum = min;
                            for (int i = 0; i < s.Count - 1; i++)
                                chartSettling2840_R.Series[0].Points.AddXY(s[i].Time, s[i].ReadHall);
                            for (int i = 0; i < s2.Count - 1; i++)
                                chartSettling2840_R.Series[1].Points.AddXY(s2[i].Time, s2[i].ReadHall);
                        }

                    }
                    break;

            }


        }

        void InitYield()
        {
            LastSampleNum.Text = Spec.LastSampleNum.ToString();
            NewSampleNumber.Text = (Spec.LastSampleNum + 1).ToString();
            List<KeyValuePair<string, double>> item = new List<KeyValuePair<string, double>>();
            List<string> litem = new List<string>();
            List<double> lRatio = new List<double>();


            for (int i = 0; i < Spec.Param.Count; i++)
            {
                int failed = Convert.ToInt32(Spec.Param[i][Spec.FAILED]);
                if (failed > 0)
                {
                    item.Add(new KeyValuePair<string, double>(string.Format("{0} {1}", Spec.Param[i][0], Spec.Param[i][1]), failed / (double)Spec.TotlaTested));
                    //litem.Add(string.Format("{0} {1}", Spec.Param[i][0], Spec.Param[i][1]));
                    //lratio.Add(failed / (double)Spec.TotlaTested);
                }
            }
            item = item.OrderByDescending(d => d.Value).ToList();
            if(STATIC.isYieldStretch)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    litem.Add(item[i].Key);
                    lRatio.Add(item[i].Value);

                }
            }
            else
            {
                if (item.Count > 8)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        litem.Add(item[i].Key);
                        lRatio.Add(item[i].Value);

                    }
                }
                else
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        litem.Add(item[i].Key);
                        lRatio.Add(item[i].Value);

                    }
                }
            }
        


            double lyield = 100;
            if (Spec.TotlaTested > 0)
            {

                lyield = (1 - Spec.TotlaFailed / (double)Spec.TotlaTested) * 100;
                if (litem.Count > 0)
                    YieldChart.Series[0].Points.DataBindXY(litem, lRatio);
                YieldChart.DataManipulator.Sort(PointSortOrder.Descending, YieldChart.Series[0]);
            }
            else
                YieldChart.Series[0].Points.Clear();
            YieldChart.Titles[0].Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            YieldChart.Titles[0].Text = "Yield " + lyield.ToString("F2") + "% \t" + (Spec.TotlaTested - Spec.TotlaFailed).ToString() + " / " + Spec.TotlaTested.ToString();
            YieldChart.Titles[0].Alignment = ContentAlignment.TopLeft;

        }
        private void SafeInitYield()
        {
            if (InvokeRequired)
            {
                InitYield();
            }
            else
            {
                InitYield();
            }
        }




        private void F_Manage_Load(object sender, EventArgs e)
        {
            Dln.SwitchOn += DriverIC_SwitchOn;
            Dln.StopOn += Dln_StopOn;
            Dln.ResetOn += Dln_ResetOn;
            Dln.EMGOn += Dln_EMGOn;
            STATIC.LogEvent.Evented += LogEvent_Evented;
            STATIC.ShowEvent.Evented += ShowEvent_Evented;

            mInfoBtn.Add(btnInfo);
            mInfoBtn.Add(btnInfoR);
            SmallResBtn.Add(btnResCh1);
            SmallResBtn.Add(btnResCh2);
            mViewLog.Add(tbViewLog0);
            mViewLog.Add(tbViewLog1);
            btnInfo.Hide();
            btnInfoR.Hide();
            InitResultData();
            InitYield();
            ChartVisible();
            BtnOQCCon(Option.isOQC);

            if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
            {
                Dln.GpioOnoff(1, DLN.RED_LAMP2, false);
                Dln.GpioOnoff(1, DLN.GREEN_LAMP2, false);
                Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, true);
            }
            else if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
            {
                Dln.GpioOnoff(1, DLN.RED_LAMP2, false);
                Dln.GpioOnoff(1, DLN.GREEN_LAMP2, false);
                Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, true);
            }

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(50);
                    if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
                    {
                        if (Dln.GpioRead(1, DLN.RED_LAMP2))
                        {
                            RedLightOn.BackColor = Color.Red;
                        }
                        else
                        {
                            RedLightOn.BackColor = Color.LightGray;
                        }
                        if (Dln.GpioRead(1, DLN.GREEN_LAMP2))
                        {
                            GreenLightOn.BackColor = Color.Green;
                        }
                        else
                        {
                            GreenLightOn.BackColor = Color.LightGray;
                        }
                        if (Dln.GpioRead(1, DLN.YELLOW_LAMP2))
                        {
                            YellowLightOn.BackColor = Color.Orange;
                        }
                        else
                        {
                            YellowLightOn.BackColor = Color.LightGray;
                        }
                        if (STATIC.fMotion.bEmgStatus && !EmgOn)
                        {
                            EmgOn = true;
                            Dln.GpioOnoff(1, DLN.RED_LAMP2, true);
                            Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, false);
                            Dln.GpioOnoff(1, DLN.GREEN_LAMP2, false);
                            Process.SuddenStop = true;
                        }
                        if (!STATIC.fMotion.bEmgStatus && EmgOn)
                        {
                            EmgOn = false;
                            Dln.GpioOnoff(1, DLN.RED_LAMP2, false);
                            Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, true);
                        }
                    }
                    else if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
                    {
                        if (Dln.GpioRead(1, DLN.RED_LAMP2))
                        {
                            RedLightOn.BackColor = Color.Red;
                        }
                        else
                        {
                            RedLightOn.BackColor = Color.LightGray;
                        }
                        if (Dln.GpioRead(1, DLN.GREEN_LAMP2))
                        {
                            GreenLightOn.BackColor = Color.Green;
                        }
                        else
                        {
                            GreenLightOn.BackColor = Color.LightGray;
                        }
                        if (Dln.GpioRead(1, DLN.YELLOW_LAMP2))
                        {
                            YellowLightOn.BackColor = Color.Orange;
                        }
                        else
                        {
                            YellowLightOn.BackColor = Color.LightGray;
                        }
                        if (STATIC.fMotion.bEmgStatus && !EmgOn)
                        {
                            EmgOn = true;
                            Dln.GpioOnoff(1, DLN.RED_LAMP2, true);
                            Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, false);
                            Dln.GpioOnoff(1, DLN.GREEN_LAMP2, false);
                            Process.SuddenStop = true;
                        }
                        if (!STATIC.fMotion.bEmgStatus && EmgOn)
                        {
                            EmgOn = false;
                            Dln.GpioOnoff(1, DLN.RED_LAMP2, false);
                            Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, true);
                        }
                    }
                    if (Dln.IsSafeOn)
                    {
                        SafeSensor.BackColor = Color.Red;
                    }
                    else SafeSensor.BackColor = Color.LightGray;
                }

            });
        }

        private void Dln_EMGOn(object sender, EventArgs e)
        {
            if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
                Dln.GpioOnoff(1, DLN.RED_LAMP2, true);
            else if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
                Dln.GpioOnoff(1, DLN.RED_LAMP2, true);

            Process.SuddenStop = true;
            //Motion Stop
            STATIC.fMotion.EMGStop();
        }

        private void Dln_ResetOn(object sender, EventArgs e)
        {
            if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
                Dln.GpioOnoff(1, DLN.RED_LAMP2, false);
            else if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
                Dln.GpioOnoff(1, DLN.RED_LAMP2, false);

            Process.SuddenStop = false;
            Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, true);
        }

        private void Dln_StopOn(object sender, EventArgs e)
        {
            STATIC.fMotion.EMGStop();
            if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
            {
                Dln.GpioOnoff(1, DLN.RED_LAMP2, true);
                Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, false);
                Dln.GpioOnoff(1, DLN.GREEN_LAMP2, false);
                Dln.GpioOnoff(1, DLN.BUZZER, false);
            }
            else if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
            {
                Dln.GpioOnoff(1, DLN.RED_LAMP2, true);
                Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, false);
                Dln.GpioOnoff(1, DLN.GREEN_LAMP2, false);
                Dln.GpioOnoff(1, DLN.BUZZER, false);
            }
            Process.SuddenStop = true;
            RepeatEndAction(0, 1, 1);
            STATIC.fManage.isrunning = false;

        }

        public void InitResultData()
        {
            ResultDataGrid.ColumnCount = 8;
            ResultDataGrid.Font = new Font("Calibri", 9, FontStyle.Bold);
            for (int i = 0; i < ResultDataGrid.ColumnCount; i++)
            {
                ResultDataGrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            ResultDataGrid.RowHeadersVisible = false;
            ResultDataGrid.BackgroundColor = Color.LightGray;

            //// Column
            ResultDataGrid.Columns[0].Name = "Axis";
            ResultDataGrid.Columns[1].Name = "Items";
            ResultDataGrid.Columns[2].Name = "Min";
            ResultDataGrid.Columns[3].Name = "Max";
            ResultDataGrid.Columns[4].Name = "Left";
            ResultDataGrid.Columns[5].Name = "Right";
            ResultDataGrid.Columns[6].Name = "unit";

            ResultDataGrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ResultDataGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            ResultDataGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ResultDataGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ResultDataGrid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ResultDataGrid.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ResultDataGrid.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            ResultDataGrid.Columns[0].Width = 57;
            ResultDataGrid.Columns[1].Width = 117;
            ResultDataGrid.Columns[2].Width = 60;
            ResultDataGrid.Columns[3].Width = 60;
            ResultDataGrid.Columns[4].Width = 82;
            ResultDataGrid.Columns[5].Width = 82;
            ResultDataGrid.Columns[6].Width = 50;


            ResultDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ResultDataGrid.ColumnHeadersHeight = 26;

            bool bColorChange = true;
            ResultDataGrid.Rows.Clear();
            for (int i = 0; i < Spec.Param.Count; i++)
            {
                if (i > 0)
                    if (Spec.Param[i - 1][Spec.KEY].ToString() != Spec.Param[i][Spec.KEY].ToString())
                        bColorChange = !bColorChange;
                if (i == Spec.Param.Count - 1)
                {
                    ResultDataGrid.Rows.Add("", Spec.Param[i][Spec.COMMENT], Spec.Param[i][Spec.MIN_VAL], Spec.Param[i][Spec.MAX_VAL], Spec.Param[i][Spec.RESET0], Spec.Param[i][Spec.RESET1], Spec.Param[i][Spec.UNIT]);
                }
                else
                {
                    ResultDataGrid.Rows.Add(Spec.Param[i][Spec.KEY], Spec.Param[i][Spec.COMMENT], Spec.Param[i][Spec.MIN_VAL], Spec.Param[i][Spec.MAX_VAL], Spec.Param[i][Spec.RESET0], Spec.Param[i][Spec.RESET1], Spec.Param[i][Spec.UNIT]);
                }


                if (bColorChange)
                {
                    ResultDataGrid[Spec.KEY, i].Style.BackColor = Color.Lavender;
                    ResultDataGrid[Spec.COMMENT, i].Style.BackColor = Color.Lavender;
                    ResultDataGrid[Spec.UNIT + 2, i].Style.BackColor = Color.Lavender;
                }
                else
                {
                    ResultDataGrid[Spec.KEY, i].Style.BackColor = Color.White;
                    ResultDataGrid[Spec.COMMENT, i].Style.BackColor = Color.White;
                    ResultDataGrid[Spec.UNIT + 2, i].Style.BackColor = Color.White;
                }

                ResultDataGrid.Rows[i].Visible = Convert.ToBoolean(Spec.Param[i][Spec.USE]);
                ResultDataGrid.Rows[i].Height = 20;
                ResultDataGrid.Rows[i].Resizable = DataGridViewTriState.False;
                ResultDataGrid.Rows[i].DefaultCellStyle.Font = new Font("Malgun Gothic", 9, FontStyle.Bold);
                for (int k = 0; k < ResultDataGrid.ColumnCount; k++)
                    ResultDataGrid[k, i].Style.Font = new Font("Malgun Gothic", 9, FontStyle.Bold);

                ResultDataGrid.ReadOnly = true;
            }

            string oldkey = "";
            for (int i = 0; i < Spec.Param.Count - 1; i++)
            {
                if (ResultDataGrid.Rows[i].Visible)
                {
                    string newKey = ResultDataGrid.Rows[i].Cells[0].Value.ToString();
                    if (oldkey == newKey) ResultDataGrid.Rows[i].Cells[0].Value = "";
                    oldkey = newKey;
                }
            }
        }
        private void ShowEvent_Evented(object sender, ShowEvent.Params e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    ShowDataResults(e.Ch, e.Key);
                });
            }
            else
            {
                ShowDataResults(e.Ch, e.Key);
            }
        }
        private void ShowDataResults(int ch, string key)
        {
            if (key == "Image")
            {
                int[] showIndex = new int[8];

                if (STATIC.Rcp.Option.Step10Use)
                {
                    showIndex = new int[8] { 0, 3, 6, 9, 10, 13, 16, 19 };
                    //if (STATIC.Rcp.Model.ModelName == "SO1C81"/*STATIC.Rcp.Model.ModelName.Contains("SO1C81")*/)
                    //    showIndex = new int[8] { 0, 3, 6, 9, 10, 13, 16, 19 };
                    //else if (STATIC.Rcp.Model.ModelName == "SO1G73")
                    //    showIndex = new int[8] { 0, 2, 5, 8, 9, 12, 15, 17 };
                }
                else
                    showIndex = new int[8] { 0, 1, 2, 3, 4, 5, 6, 7 };
                if (ch == 0)
                {
                    pb_L_F14.Image = Process.PosTest[0][showIndex[0]].img;
                    pb_L_F14.SizeMode = PictureBoxSizeMode.StretchImage;

                    pb_L_F1820.Image = Process.PosTest[0][showIndex[1]].img;
                    pb_L_F1820.SizeMode = PictureBoxSizeMode.StretchImage;

                    pb_L_F2528.Image = Process.PosTest[0][showIndex[2]].img;
                    pb_L_F2528.SizeMode = PictureBoxSizeMode.StretchImage;

                    pb_L_F3540.Image = Process.PosTest[0][showIndex[3]].img;
                    pb_L_F3540.SizeMode = PictureBoxSizeMode.StretchImage;

                    pb_L_F40.Image = Process.PosTest[0][showIndex[4]].img;
                    pb_L_F40.SizeMode = PictureBoxSizeMode.StretchImage;

                    pb_L_F3228.Image = Process.PosTest[0][showIndex[5]].img;
                    pb_L_F3228.SizeMode = PictureBoxSizeMode.StretchImage;

                    pb_L_F2220.Image = Process.PosTest[0][showIndex[6]].img;
                    pb_L_F2220.SizeMode = PictureBoxSizeMode.StretchImage;

                    pb_L_F1614.Image = Process.PosTest[0][showIndex[7]].img;
                    pb_L_F1614.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    if (Process.ChannelCnt > 1)
                    {
                        pb_R_F14.Image = Process.PosTest[1][showIndex[0]].img;
                        pb_R_F14.SizeMode = PictureBoxSizeMode.StretchImage;

                        pb_R_F1820.Image = Process.PosTest[1][showIndex[1]].img;
                        pb_R_F1820.SizeMode = PictureBoxSizeMode.StretchImage;

                        pb_R_F2528.Image = Process.PosTest[1][showIndex[2]].img;
                        pb_R_F2528.SizeMode = PictureBoxSizeMode.StretchImage;

                        pb_R_F3540.Image = Process.PosTest[1][showIndex[3]].img;
                        pb_R_F3540.SizeMode = PictureBoxSizeMode.StretchImage;

                        pb_R_F40.Image = Process.PosTest[1][showIndex[4]].img;
                        pb_R_F40.SizeMode = PictureBoxSizeMode.StretchImage;

                        pb_R_F3228.Image = Process.PosTest[1][showIndex[5]].img;
                        pb_R_F3228.SizeMode = PictureBoxSizeMode.StretchImage;

                        pb_R_F2220.Image = Process.PosTest[1][showIndex[6]].img;
                        pb_R_F2220.SizeMode = PictureBoxSizeMode.StretchImage;

                        pb_R_F1614.Image = Process.PosTest[1][showIndex[7]].img;
                        pb_R_F1614.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }



            }
            else if (key == "Yield")
            {
                SafeInitYield();
            }

            else
            {
                for (int i = 0; i < Spec.Param.Count; i++)
                {
                    if (Spec.Param[i][0].ToString() != key) continue;

                    if (Spec.PassFails[ch].Results[i].Val != 0)
                    {
                        if (ResultDataGrid[1, i].Value.ToString().Contains("Hall"))
                            ResultDataGrid[ch + 4, i].Value = Spec.PassFails[ch].Results[i].Val.ToString();
                        else
                            ResultDataGrid[ch + 4, i].Value = Spec.PassFails[ch].Results[i].Val.ToString("F3");

                    }
                    if (Spec.PassFails[ch].Results[i].bPass) ResultDataGrid[ch + 4, i].Style.BackColor = Color.White;
                    else ResultDataGrid[ch + 4, i].Style.BackColor = Color.Orange;
                }
            }




        }
        private void LogEvent_Evented(object sender, LogEvent.Params e)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate
                    {
                        mViewLog[e.Ch].AppendText(e.Msg + "\r\n");
                    });
                }
                else
                {
                    mViewLog[e.Ch].AppendText(e.Msg + "\r\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ToAdmin_Click(object sender, System.EventArgs e)
        {
            STATIC.State = (int)STATIC.STATE.Main;
        }

        private async void DriverIC_SwitchOn(object sender, EventArgs e)
        {
            try
            {
                if(STATIC.State == (int)STATIC.STATE.Vision)
                    STATIC.State = (int)STATIC.STATE.Manage;


                if (!STATIC.Rcp.Condition.ToDoList.Contains("IRIS Postion Test"))
                {
                    MessageBox.Show("Verify test item is not exist.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
               
                if (!isrunning)
                    isrunning = true;
                else return;
                Process.SuddenStop = false;
                STATIC.DecenterX_Pos1 = 0;
                STATIC.DecenterY_Pos1 = 0;
                STATIC.CDecenterX_Pos1 = 0;
                STATIC.CDecenterY_Pos1 = 0;
                //STATIC.COScanLinearityDiff = new double[2];
                //STATIC.COScanLinearityMax = new double[2];
                //STATIC.OCScanLinearityDiff = new double[2];
                //STATIC.OCScanLinearityMax = new double[2];
                //STATIC.COScanLinearityDiffHall = new int[2];
                //STATIC.COScanLinearityMaxHall = new int[2];
                //STATIC.OCScanLinearityDiffHall = new int[2];
                //STATIC.OCScanLinearityMaxHall = new int[2];

                
                if (STATIC.isTmpLog)
                    STATIC.CommonLog += string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"), "M", "-", "-", "Idle", "Event:Start_BtnClick", "Start", "");

                if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
                {
                    if (STATIC.fMotion.bEmgStatus)
                    {
                        isrunning = false;
                        return;
                    }

                    Dln.GpioOnoff(1, DLN.RED_LAMP2, false);
                    Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, false);
                    Dln.GpioOnoff(1, DLN.GREEN_LAMP2, true);
                    Dln.GpioOnoff(1, DLN.BUZZER, false);
                }
                else if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
                {
                    if (STATIC.fMotion.bEmgStatus)
                    {
                        isrunning = false;
                        return;
                    }
                    Dln.GpioOnoff(1, DLN.RED_LAMP2, false);
                    Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, false);
                    Dln.GpioOnoff(1, DLN.BUZZER, false);
                    Dln.GpioOnoff(1, DLN.GREEN_LAMP2, true);
                }

                STATIC.isNonSpecError = new bool[2] { false, false };

                if (STATIC.Rcp.Option.MotionUse && STATIC.Rcp.Option.isPosture)
                {

                    Stopwatch stw = new Stopwatch();
                    Process.RepeatRun = MotionSettingData.InspPosCount;
                    Process.CurrentRun = 1;
                    RepeatStartAction(0);
                    if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
                    {
                        stw.Start();
                        if (!Dln.IsLoad && Dln.IsCoverUp)
                        {
                            Dln.LoadSocket(true);

                            while (true)
                            {
                                if (Dln.IsLoad) break;
                                if (stw.ElapsedMilliseconds > 5000)
                                {
                                    STATIC.LogEvent.AddLog(0, "Socket Load Failed..");
                                    Process.IsRun[0] = false;
                                    isrunning = false;
                                    return;
                                }
                            }

                            Dln.CoverUpDown(false);
                        }
                    }
                    while (true)
                    {
                        STATIC.SaveImageCurrentPos = Process.CurrentRun;
                        double[] degree = STATIC.fMotion.MoveSelectedPosition(Process.CurrentRun - 1, true);

                        Task wait = Task.Factory.StartNew(() =>
                        {
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            while (true)
                            {
                                Thread.Sleep(10);
                                if (((int)STATIC.fMotion.AxisZCurrent + 1 >= degree[0] && (int)STATIC.fMotion.AxisXCurrent + 1 >= degree[1] && (int)STATIC.fMotion.AxisZCurrent - 1 <= degree[0] && (int)STATIC.fMotion.AxisXCurrent - 1 <= degree[1]))
                                {
                                    Thread.Sleep(1000);
                                    break;
                                }

                                if (STATIC.fMotion.bEmgStatus || Dln.IsSafeOn || Process.SuddenStop) break;
                            }
                        });

                        Task finishwatTask = await Task.WhenAny(wait);

                        if (STATIC.fMotion.bEmgStatus || Dln.IsSafeOn)
                        {
                            STATIC.fMotion.EMGStop();
                            if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
                            {
                                Dln.GpioOnoff(1, DLN.RED_LAMP2, true);
                                Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, false);
                                Dln.GpioOnoff(1, DLN.GREEN_LAMP2, false);
                                Dln.GpioOnoff(1, DLN.BUZZER, true);
                            }
                            else if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
                            {
                                Dln.GpioOnoff(1, DLN.RED_LAMP2, true);
                                Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, false);
                                Dln.GpioOnoff(1, DLN.BUZZER, true);
                                Dln.GpioOnoff(1, DLN.GREEN_LAMP2, false);
                            }
                            RepeatEndAction(0, 1, 1);
                            isrunning = false;
                            return;
                        }

                        ClearGraph();
                        ClearLog();
                        if (!Process.IsRun[0])
                        {
                            Task task = null;
                            Process.IsRun[0] = true;
                            RepeatStartAction(0);
                            if (InvokeRequired)
                            {
                                BeginInvoke((MethodInvoker)delegate
                                {

                                    RepeatRunCnt.Text = string.Format("POS {0}", Process.CurrentRun);
                                });
                            }
                            else
                                RepeatRunCnt.Text = string.Format("POS {0}", Process.CurrentRun);

                            if (Process.CurrentRun == 1)
                                task = Task.Factory.StartNew(() => Process.LoadTestUnload(0, true, Process.CurrentRun));
                            else
                                task = Task.Factory.StartNew(() => Process.LoadTestUnload(0, false, Process.CurrentRun));

                            Process.RunTasks.Add(task);
                            Process.RunTaskId1 = task.Id;
                        }

                        while (Process.RunTasks.Count > 0)
                        {
                            Task finishTask = await Task.WhenAny(Process.RunTasks);
                            if (finishTask.Id == Process.RunTaskId1)
                            {
                                //RepeatEndAction(0);
                            }
                            else if (finishTask.Id == Process.RunTaskId2)
                            {
                                //RepeatEndAction(1);
                            }
                            Process.RunTasks.Remove(finishTask);
                        }

                        SaveScreenAction();

                        if (Process.CurrentRun >= Process.RepeatRun || Process.SuddenStop) break;
                        Process.CurrentRun++;
                        Thread.Sleep(1500);
                    }
                    RepeatEndAction(0, Process.RepeatRun, STATIC.Rcp.Condition.ItrCnt);
                    double[] degree2 = STATIC.fMotion.MoveSelectedPosition(-1, true);

                    Task wait2 = Task.Factory.StartNew(() =>
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        while (true)
                        {
                            Thread.Sleep(10);
                            if (((int)STATIC.fMotion.AxisZCurrent == degree2[0] && (int)STATIC.fMotion.AxisXCurrent == degree2[1]))
                                break;
                            if (STATIC.fMotion.bEmgStatus || Dln.IsSafeOn || Process.SuddenStop) break;
                        }
                    });
                    Task finishwatTask2 = await Task.WhenAny(wait2);

                    if (STATIC.fMotion.bEmgStatus || Dln.IsSafeOn)
                    {
                        STATIC.fMotion.EMGStop();
                        if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
                        {
                            Dln.GpioOnoff(1, DLN.RED_LAMP2, true);
                            Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, false);
                            Dln.GpioOnoff(1, DLN.GREEN_LAMP2, false);
                            Dln.GpioOnoff(1, DLN.BUZZER, true);
                        }
                        else if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
                        {
                            Dln.GpioOnoff(1, DLN.RED_LAMP2, true);
                            Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, false);
                            Dln.GpioOnoff(1, DLN.BUZZER, true);
                            Dln.GpioOnoff(1, DLN.GREEN_LAMP2, false);
                        }
                        RepeatEndAction(0, 1, 1);
                        isrunning = false;
                        return;
                    }

                    if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
                    {
                        if (Dln.IsLoad && !Dln.IsCoverUp)
                        {
                            Dln.CoverUpDown(true);
                            stw.Restart();
                            while (true)
                            {
                                if (Dln.IsCoverUp) break;
                                if (stw.ElapsedMilliseconds > 5000)
                                {
                                    STATIC.LogEvent.AddLog(0, "Socket Cover Open Failed..");
                                    Process.IsRun[0] = false;
                                    isrunning = false;
                                    return;
                                }
                            }
                            Dln.LoadSocket(false);
                            stw.Restart();
                            while (true)
                            {
                                if (!Dln.IsLoad) break;
                                if (stw.ElapsedMilliseconds > 5000)
                                {
                                    STATIC.LogEvent.AddLog(0, "Socket UnLoad Failed..");
                                    Process.IsRun[0] = false;
                                    isrunning = false;
                                    return;
                                }
                            }
                        }
                    }

                }
                else
                {
                    Process.RepeatRun = 1;
                    Process.CurrentRun = 1;
                    Stopwatch stw = new Stopwatch();
                    if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
                    {
                        stw.Start();
                        if (!Dln.IsLoad && Dln.IsCoverUp)
                        {
                            Dln.LoadSocket(true);

                            while (true)
                            {
                                if (Dln.IsLoad) break;
                                if (stw.ElapsedMilliseconds > 5000)
                                {
                                    STATIC.LogEvent.AddLog(0, "Socket Load Failed..");
                                    Process.IsRun[0] = false;
                                    isrunning = false;
                                    return;
                                }
                            }

                            Dln.CoverUpDown(false);
                        }
                    }

                    while (true)
                    {
                        STATIC.SaveImageCurrentPos = Process.CurrentRun;
                        ClearGraph();
                        ClearLog();

                        if (!Process.IsRun[0])
                        {
                            Process.IsRun[0] = true;
                            RepeatStartAction(0);
                            if (InvokeRequired)
                            {
                                BeginInvoke((MethodInvoker)delegate
                                {

                                    RepeatRunCnt.Text = string.Format("POS {0}", Process.CurrentRun);
                                });
                            }
                            else
                                RepeatRunCnt.Text = string.Format("POS {0}", Process.CurrentRun);
                            Task task = Task.Factory.StartNew(() => Process.LoadTestUnload(0, true, Process.CurrentRun));
                            Process.RunTasks.Add(task);
                            Process.RunTaskId1 = task.Id;
                        }

                        while (Process.RunTasks.Count > 0)
                        {
                            Task finishTask = await Task.WhenAny(Process.RunTasks);
                            if (finishTask.Id == Process.RunTaskId1)
                            {
                                RepeatEndAction(0, 1, STATIC.Rcp.Condition.ItrCnt);
                            }
                            else if (finishTask.Id == Process.RunTaskId2)
                            {
                                RepeatEndAction(1, 1, STATIC.Rcp.Condition.ItrCnt);
                            }
                            Process.RunTasks.Remove(finishTask);
                        }

                        SaveScreenAction();

                        if (Process.CurrentRun >= Process.RepeatRun || Process.SuddenStop) break;
                        Process.CurrentRun++;
                        Thread.Sleep(1500);

                    }

                    if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
                    {
                        if (Dln.IsLoad && !Dln.IsCoverUp)
                        {
                            Dln.CoverUpDown(true);
                            stw.Restart();
                            while (true)
                            {
                                if (Dln.IsCoverUp) break;
                                if (stw.ElapsedMilliseconds > 5000)
                                {
                                    STATIC.LogEvent.AddLog(0, "Socket Cover Open Failed..");
                                    Process.IsRun[0] = false;
                                    isrunning = false;
                                    return;
                                }
                            }
                            Dln.LoadSocket(false);
                            stw.Restart();
                            while (true)
                            {
                                if (!Dln.IsLoad) break;
                                if (stw.ElapsedMilliseconds > 5000)
                                {
                                    STATIC.LogEvent.AddLog(0, "Socket UnLoad Failed..");
                                    Process.IsRun[0] = false;
                                    isrunning = false;
                                    return;
                                }
                            }
                        }
                    }
                }

                if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
                {
                    Dln.GpioOnoff(1, DLN.GREEN_LAMP2, false);
                    if (!STATIC.fMotion.bEmgStatus) Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, true);
                }
                else if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
                {
                    Dln.GpioOnoff(1, DLN.GREEN_LAMP2, false);
                    if (!STATIC.fMotion.bEmgStatus) Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, true);
                }

                if (STATIC.isTmpLog)
                {
              
                    Task t = new Task(() => WriteDetailLogs());
                    t.Start();
                }

                isrunning = false;
            }
            catch { isrunning = false; }
            finally
            {
                if (!isrunning)
                    Process.LEDs_All_On(false);
            }

        }


        public void WriteDetailLogs()
        {
            DateTime dtNow = DateTime.Now;
            string commonFile = dtNow.ToString("yyyyMMdd") + ".csv";
            string ContactFile = dtNow.ToString("yyyyMMdd") + "_" + "Contact" + ".csv";
            string ResolutionFile = dtNow.ToString("yyyyMMdd") + "_" + "Resolution" + ".csv";
            string RepeatFile = dtNow.ToString("yyyyMMdd") + "_" + "Repeat" + ".csv";
            string InspLogFile = dtNow.ToString("yyyyMMdd") + "_" + "Inspection" + ".csv";
            string dateDir = STATIC.CreateDateDir();
          
            StreamWriter sw = File.AppendText(dateDir + "\\" + commonFile);
            sw.Write(STATIC.CommonLog);
            sw.Close();

            if (!File.Exists(dateDir + "\\" + ContactFile))
            {
                sw = File.AppendText(dateDir + "\\" + ContactFile);
                sw.Write("Time, Product ID,Result\r\n");
                sw.Write(STATIC.ContactLog);
                sw.Close();
            }
            else
            {
                sw = File.AppendText(dateDir + "\\" + ContactFile);
                sw.Write(STATIC.ContactLog);
                sw.Close();
            }
            if (!File.Exists(dateDir + "\\" + ResolutionFile))
            {
                sw = File.AppendText(dateDir + "\\" + ResolutionFile);
                sw.Write("Time,  Product ID, Result X, Result Y\r\n");
                sw.Write(STATIC.ResolutionLog);
                sw.Close();
            }
            else
            {
                sw = File.AppendText(dateDir + "\\" + ResolutionFile);
                sw.Write(STATIC.ResolutionLog);
                sw.Close();
            }
            if (!File.Exists(dateDir + "\\" + RepeatFile))
            {
                sw = File.AppendText(dateDir + "\\" + RepeatFile);
                sw.Write("Time,  Product ID, Result X, Result Y\r\n");
                sw.Write(STATIC.RepeatLog);
                sw.Close();
            }
            else
            {
                sw = File.AppendText(dateDir + "\\" + RepeatFile);
                sw.Write(STATIC.RepeatLog);
                sw.Close();
            }

            if (!File.Exists(dateDir + "\\" + InspLogFile))
            {
                sw = File.AppendText(dateDir + "\\" + InspLogFile);
                sw.Write("Time,  Product ID, " +
                    "F14_Area, F14_Decenter, F14_Current, F14_Hall, " +
                    "F20_Area, F20_Decenter, F20_Current, F20_Hall, " +
                    "F28_Area, F28_Decenter, F28_Current, F28_Hall, " +
                    "F40_Area, F40_Decenter, F40_Current, F40_Hall\r\n");
                sw.Write(STATIC.InspLog);
                sw.Close();
            }
            else
            {
                sw = File.AppendText(dateDir + "\\" + InspLogFile);
                sw.Write(STATIC.InspLog);
                sw.Close();
            }

            STATIC.CommonLog = string.Empty;
            STATIC.ContactLog = string.Empty;
            STATIC.ResolutionLog = string.Empty;
            STATIC.RepeatLog = string.Empty;
            STATIC.InspLog = string.Empty;

        }

        public void CurrentItrCnt(int cnt)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    CurrentRunCnt.Text = string.Format("ITR {0}", cnt);


                });
            }
            else
                CurrentRunCnt.Text = string.Format("ITR {0}", cnt);

        }

        private void SaveScreenAction()
        {
        }

        private void RepeatEndAction(int port, int posCnt, int itrCnt)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    SetInforView(port, posCnt, itrCnt);
                });
            }
            else
            {
                SetInforView(port, posCnt, itrCnt);
            }
            if (port == 0)
            {
                SafeControlView(mInfoBtn[0], true);
                SafeControlView(mInfoBtn[1], true);
                SafeControlView(SmallResBtn[0], true);
                SafeControlView(SmallResBtn[1], true);
            }
            else
            {
                SafeControlView(mInfoBtn[2], true);
                SafeControlView(mInfoBtn[3], true);
            }
        }
        public void BtnOQCCon(bool isOn)
        {
            if (isOn)
            {
                btnIsOQC.Text = "OQC";
                btnIsOQC.BackColor = Color.Red;
            }
            else
            {
                btnIsOQC.Text = "MQC";
                btnIsOQC.BackColor = Color.Blue;
            }
        }
        private void RepeatStartAction(int port)
        {
            //if (InvokeRequired)
            //{
            //    BeginInvoke((MethodInvoker)delegate
            //    {
            //        //    if (RepeatRunCnt.Text == "") RepeatRunCnt.Text = "1";
            //        CurrentRunCnt.Text = (ItrCnt + 1).ToString();
            //        RepeatRunCnt.Text = string.Format("POS {0}", Position); 
            //    });
            //}
            //else
            //{
            //    //  if (RepeatRunCnt.Text == "") RepeatRunCnt.Text = "1";
            //    CurrentRunCnt.Text = (ItrCnt + 1).ToString();
            //    RepeatRunCnt.Text = string.Format("POS {0}", Position);
            //}

            if (port == 0)
            {
                SafeControlView(mInfoBtn[0], false);
                SafeControlView(mInfoBtn[1], false);
                SafeControlView(SmallResBtn[0], false);
                SafeControlView(SmallResBtn[1], false);
                ShowDataResultsInit(0);
                ShowDataResultsInit(1);
            }
            else
            {
                SafeControlView(mInfoBtn[2], false);
                SafeControlView(mInfoBtn[3], false);
                ShowDataResultsInit(2);
                ShowDataResultsInit(3);
            }
        }

        private void SafeControlView(Control con, bool bShow)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    if (bShow) con.Show(); else con.Hide();
                });
            }
            else
            {
                if (bShow) con.Show(); else con.Hide();
            }
        }

        private void SetInforView(int port, int posCnt, int itrcnt)
        {

            if (port == 0)
            {
                for (int i = 0; i < Process.ChannelCnt; i++)
                {
                    string Errmsg = "";
                    string PassMsg = "PASS";
                    for (int j = 0; j < posCnt * itrcnt; j++)
                    {
                        if (STATIC.ErrMsg[i][j] != null && !STATIC.ErrMsg[i][j].Contains("PASS") && Errmsg == "")
                            Errmsg = STATIC.ErrMsg[i][j];
                        //if (STATIC.ErrMsg[i][j] != null && STATIC.ErrMsg[i][j].Contains("A PASS") && PassMsg != "B PASS" && PassMsg != "C PASS")
                        //    PassMsg = "A PASS";
                        //if (STATIC.ErrMsg[i][j] != null && STATIC.ErrMsg[i][j].Contains("B PASS") && PassMsg != "C PASS")
                        //    PassMsg = "B PASS";
                        //if (STATIC.ErrMsg[i][j] != null && STATIC.ErrMsg[i][j].Contains("C PASS"))
                        //    PassMsg = "C PASS";
                    }
                    if (Process.SuddenStop) Errmsg = "Sudden Stop!!";
                    if (STATIC.fMotion.bEmgStatus) Errmsg = "EMG Stop!!";
                    if (Dln.IsSafeOn) Errmsg = "Safety Sensor Stop!!";
                    if (Errmsg == "")
                    {
                        mInfoBtn[i].Text = PassMsg;
                        mInfoBtn[i].Font = new Font("Malgun Gothic", 60, FontStyle.Bold);
                        mInfoBtn[i].ForeColor = Color.Cyan;
                        SmallResBtn[i].Text = PassMsg;
                        SmallResBtn[i].ForeColor = Color.Cyan;
                    }
                    else
                    {
                        mInfoBtn[i].Text = Errmsg;
                        mInfoBtn[i].Font = new Font("Malgun Gothic", 24, FontStyle.Bold);
                        mInfoBtn[i].ForeColor = Color.OrangeRed;
                        SmallResBtn[i].Text = Errmsg;
                        SmallResBtn[i].ForeColor = Color.OrangeRed;
                    }
                }
            }
        }

        public void ShowDataResultsInit(int ch)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    Spec.InitResult(ch);
                    for (int i = 0; i < Spec.Param.Count; i++)
                    {
                        ResultDataGrid[ch + 4, i].Value = Spec.PassFails[ch].Results[i].Val.ToString("F0");
                        ResultDataGrid[ch + 4, i].Style.BackColor = Color.White;
                    }
                    pb_L_F14.Image = null;
                    pb_L_F1820.Image = null;
                    pb_L_F2528.Image = null;
                    pb_L_F3540.Image = null;
                    pb_L_F40.Image = null;
                    pb_L_F3228.Image = null;
                    pb_L_F2220.Image = null;
                    pb_L_F1614.Image = null;
                    pb_R_F14.Image = null;
                    pb_R_F1820.Image = null;
                    pb_R_F2528.Image = null;
                    pb_R_F3540.Image = null;
                    pb_R_F40.Image = null;
                    pb_R_F3228.Image = null;
                    pb_R_F2220.Image = null;
                    pb_R_F1614.Image = null;

                });
            }
            else
            {
                Spec.InitResult(ch);
                for (int i = 0; i < Spec.Param.Count; i++)
                {
                    ResultDataGrid[ch + 4, i].Value = Spec.PassFails[ch].Results[i].Val.ToString("F0");
                    ResultDataGrid[ch + 4, i].Style.BackColor = Color.White;
                }
                pb_L_F14.Image = null;
                pb_L_F1820.Image = null;
                pb_L_F2528.Image = null;
                pb_L_F3540.Image = null;
                pb_L_F40.Image = null;
                pb_L_F3228.Image = null;
                pb_L_F2220.Image = null;
                pb_L_F1614.Image = null;
                pb_R_F14.Image = null;
                pb_R_F1820.Image = null;
                pb_R_F2528.Image = null;
                pb_R_F3540.Image = null;
                pb_R_F40.Image = null;
                pb_R_F3228.Image = null;
                pb_R_F2220.Image = null;
                pb_R_F1614.Image = null;
            }
        }

        private void ToVision_Click(object sender, System.EventArgs e)
        {
            if (!isrunning)
                STATIC.State = (int)STATIC.STATE.Vision;
        }

        public void ClearGraph()
        {

        }

        public void ClearLog()
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    mViewLog[0].Text = "";
                    mViewLog[1].Text = "";
                });
            }
            else
            {
                mViewLog[0].Text = "";
                mViewLog[1].Text = "";
            }

        }

        public void SaveLog(int ch)
        {
            string dateDir = STATIC.CreateDateDir();
            dateDir += "DrivingLog\\";

            string filename = string.Format("{0}_{1}_{2}_", Process.m_StrIndex[ch], ch, DateTime.Now.ToString("yyMMdd_HHmmss"));

            LogEvent.SaveLog(ch, dateDir, filename, string.Format("{0}", mViewLog[ch].Text));
        }

        void Test()
        {
            while(true)
            {
              
                DriverIC_SwitchOn(null, EventArgs.Empty);
            }
            
        }

        private async void btn10Itrtest_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!STATIC.Rcp.Condition.ToDoList.Contains("IRIS Postion Test"))
                //{
                //    MessageBox.Show("Verify test item is not exist.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                if (!isrunning)
                    isrunning = true;
             
                else return;
                STATIC.DecenterX_Pos1 = 0;
                STATIC.DecenterY_Pos1 = 0;
                STATIC.CDecenterX_Pos1 = 0;
                STATIC.CDecenterY_Pos1 = 0;
                //STATIC.COScanLinearityDiff = new double[2];
                //STATIC.COScanLinearityMax = new double[2];
                //STATIC.OCScanLinearityDiff = new double[2];
                //STATIC.OCScanLinearityMax = new double[2];
                //STATIC.COScanLinearityDiffHall = new int[2];
                //STATIC.COScanLinearityMaxHall = new int[2];
                //STATIC.OCScanLinearityDiffHall = new int[2];
                //STATIC.OCScanLinearityMaxHall = new int[2];
             
                if (STATIC.isTmpLog)
                    STATIC.CommonLog += string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"), "M", "-", "-", "Idle", "Event:Start_BtnClick", "Start", "");

                Stopwatch stw = new Stopwatch();
                if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
                {
                    if (STATIC.fMotion.bEmgStatus)
                    {
                        isrunning = false;
                        return;
                    }
                    Dln.GpioOnoff(1, DLN.RED_LAMP2, false);
                    Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, false);
                    Dln.GpioOnoff(1, DLN.GREEN_LAMP2, true);
                }
                else if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
                {
                    if (STATIC.fMotion.bEmgStatus)
                    {
                        isrunning = false;
                        return;
                    }
                    Dln.GpioOnoff(1, DLN.RED_LAMP2, false);
                    Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, false);
                    Dln.GpioOnoff(1, DLN.GREEN_LAMP2, true);
                }

                Process.SuddenStop = false;
                Process.RepeatRun = 1;//int.Parse(RepeatRunCnt.Text);
                Process.CurrentRun = 1;
                if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
                {
                    stw.Start();
                    if (!Dln.IsLoad && Dln.IsCoverUp)
                    {
                        Dln.LoadSocket(true);

                        while (true)
                        {
                            if (Dln.IsLoad) break;
                            if (stw.ElapsedMilliseconds > 5000)
                            {
                                STATIC.LogEvent.AddLog(0, "Socket Load Failed..");
                                Process.IsRun[0] = false;
                                isrunning = false;
                                return;
                            }
                        }

                        Dln.CoverUpDown(false);
                    }
                }

                STATIC.isNonSpecError = new bool[2] { false, false };

                while (true)
                {
                    STATIC.SaveImageCurrentPos = Process.CurrentRun;
                    ClearGraph();
                    ClearLog();

                    if (!Process.IsRun[0])
                    {
                        Process.IsRun[0] = true;
                        RepeatStartAction(0);
                        if (InvokeRequired)
                        {
                            BeginInvoke((MethodInvoker)delegate
                            {

                                RepeatRunCnt.Text = string.Format("POS {0}", Process.CurrentRun);
                            });
                        }
                        else
                            RepeatRunCnt.Text = string.Format("POS {0}", Process.CurrentRun);
                        Task task = Task.Factory.StartNew(() => Process.LoadTestUnload(0, true, 1));
                        Process.RunTasks.Add(task);
                        Process.RunTaskId1 = task.Id;
                    }

                    while (Process.RunTasks.Count > 0)
                    {
                        Task finishTask = await Task.WhenAny(Process.RunTasks);
                        if (finishTask.Id == Process.RunTaskId1)
                        {
                            RepeatEndAction(0, 1, STATIC.Rcp.Condition.ItrCnt);
                        }
                        else if (finishTask.Id == Process.RunTaskId2)
                        {
                            RepeatEndAction(1, 1, STATIC.Rcp.Condition.ItrCnt);
                        }
                        Process.RunTasks.Remove(finishTask);
                    }

                    SaveScreenAction();

                    if (Process.CurrentRun >= Process.RepeatRun || Process.SuddenStop) break;
                    Process.CurrentRun++;
                    Thread.Sleep(1500);
                }

                if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
                {
                    if (Dln.IsLoad && !Dln.IsCoverUp)
                    {
                        Dln.CoverUpDown(true);
                        stw.Restart();
                        while (true)
                        {
                            if (Dln.IsCoverUp) break;
                            if (stw.ElapsedMilliseconds > 5000)
                            {
                                STATIC.LogEvent.AddLog(0, "Socket Cover Open Failed..");
                                Process.IsRun[0] = false;
                                isrunning = false;
                                return;
                            }
                        }

                        Dln.LoadSocket(false);
                        stw.Restart();
                        while (true)
                        {
                            if (!Dln.IsLoad) break;
                            if (stw.ElapsedMilliseconds > 5000)
                            {
                                STATIC.LogEvent.AddLog(0, "Socket UnLoad Failed..");
                                Process.IsRun[0] = false;
                                isrunning = false;
                                return;
                            }
                        }
                    }
                }

                if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
                {
                    Dln.GpioOnoff(1, DLN.GREEN_LAMP2, false);
                    if (!STATIC.fMotion.bEmgStatus) Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, true);
                }
                else if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
                {
                    Dln.GpioOnoff(1, DLN.GREEN_LAMP2, false);
                    if (!STATIC.fMotion.bEmgStatus) Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, true);
                }
                if (STATIC.isTmpLog)
                { 
                    Task t = new Task(() => WriteDetailLogs());
                    t.Start();
                }

                isrunning = false;
            }
            catch { isrunning = false; }
            finally 
            {
                if (!isrunning)
                    Process.LEDs_All_On(false); 
            }

        }


        private void tbViewLog0_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TextBox box = (TextBox)sender;

            if (box.Tag.ToString() == "S")
            {
                box.Size = new System.Drawing.Size(682, 500);
                box.Tag = "L";
                box.BringToFront();
            }
            else
            {
                box.Size = new System.Drawing.Size(682, 162);
                box.Tag = "S";
                box.BringToFront();
            }
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            btnInfo.Hide();
            btnInfoR.Hide();
        }

        private void btnInfoR_Click(object sender, EventArgs e)
        {
            btnInfo.Hide();
            btnInfoR.Hide();
        }



        private void YieldChart_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            STATIC.isYieldStretch = !STATIC.isYieldStretch;
            if(STATIC.isYieldStretch)
            {
                YieldChart.Size = new Size(1209, 632);
            }
            else
            {
                YieldChart.Size = new Size(430, 254);
            }
            InitYield();


            //DialogResult result = MessageBox.Show("Do you wan to Reset and Save Yield Data?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            //if (result == DialogResult.OK)
            //{
            //    Spec.LastSampleNum = 0;
            //    Spec.TotlaTested = 0;
            //    Spec.TotlaFailed = 0;
            //    Spec.TotlaPassed = 0;
            //    for (int i = 0; i < Spec.Param.Count; i++)
            //    {
            //        Spec.Param[i][Spec.FAILED] = 0;
            //    }
            //    InitYield();
            //}
            //Spec.Save();
        }

        private void LightOn_Click(object sender, EventArgs e)
        {
            //Button bt = (Button)sender;

            //if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
            //{
            //    if (bt.Text.Contains("Red"))
            //    {
            //        Dln.GpioOnoff(0, DLN.RED_LAMP1, !Dln.GpioRead(0, DLN.RED_LAMP1));
            //    }
            //    if (bt.Text.Contains("Green"))
            //    {
            //        Dln.GpioOnoff(0, DLN.GREEN_LAMP1, !Dln.GpioRead(0, DLN.GREEN_LAMP1));
            //    }
            //    if (bt.Text.Contains("Yellow"))
            //    {
            //        Dln.GpioOnoff(0, DLN.YELLOW_LAMP1, !Dln.GpioRead(0, DLN.YELLOW_LAMP1));
            //    }
            //}
            //else if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
            //{
            //    if (bt.Text.Contains("Red"))
            //    {
            //        Dln.GpioOnoff(1, DLN.RED_LAMP2, !Dln.GpioRead(1, DLN.RED_LAMP2));
            //    }
            //    if (bt.Text.Contains("Green"))
            //    {
            //        Dln.GpioOnoff(1, DLN.GREEN_LAMP2, !Dln.GpioRead(1, DLN.GREEN_LAMP2));
            //    }
            //    if (bt.Text.Contains("Yellow"))
            //    {
            //        Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, !Dln.GpioRead(1, DLN.YELLOW_LAMP2));
            //    }
            //}
        }

        private void SetSampleNumber_Click(object sender, EventArgs e)
        {
            int NewNum = Convert.ToInt32(NewSampleNumber.Text);
            if (NewNum > 0)
            {
                Spec.LastSampleNum = NewNum - 1;
                LastSampleNum.Text = Spec.LastSampleNum.ToString();
            }
            else NewSampleNumber.Text = "1";
        }

        bool beStarted = false;
        private void button1_Click(object sender, EventArgs e)
        {

         
            if (beStarted)
                beStarted = false;
            else
            {

                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate
                    {
                        tbViewLog0.Clear();
                        tbViewLog1.Clear();
                    });
                }
                else
                {
                    tbViewLog0.Clear();
                    tbViewLog1.Clear();
                }
                beStarted = true;
            }

            // DriverIC_SwitchOn(null, EventArgs.Empty);
            Task t = new Task(() => ContactTest());
            t.Start();
        }
        void ContactTest()
        {
          

            for (int i = 0; i < 1; i++)
            {
                if (!Dln.IsLoad && Dln.IsCoverUp)
                {
                    Dln.LoadSocket(true);

                    while (true) if (Dln.IsLoad) break;
                    Thread.Sleep(150);
                    Dln.CoverUpDown(false);
                    while (true) if (!Dln.IsCoverUp) break;
                    Thread.Sleep(150);
                }

                for (int k = 0; k < Process.ChannelCnt; k++)
                {
                    bool isContacted = false;
                    try
                    {
                        int[] addr = Dln.DLNi2c[k].ScanDevices();
                     
                        foreach (int ad in addr)
                        {

                            if (ad == STATIC.DrvIC.AkSlave[k])
                            {
                                STATIC.LogEvent.AddLog(k, DateTime.Now.ToString() + "\tOK");
                                if (STATIC.isTmpLog)
                                    STATIC.ContactLog += string.Format("{0},{1},{2}\r\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"), k, "OK");


                                isContacted = true;
                                break;
                            }
                        }
                        if (!isContacted)
                        {
                            STATIC.LogEvent.AddLog(k, DateTime.Now.ToString() + "\tNG");
                            if (STATIC.isTmpLog)
                                STATIC.ContactLog += string.Format("{0},{1},{2}\r\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"), k, "NG");
                        }
                           
                    }
                    catch
                    {
                        return;
                    }

                }
                if (Dln.IsLoad && !Dln.IsCoverUp)
                {

                    Dln.CoverUpDown(true);

                    Thread.Sleep(150);

                    Dln.LoadSocket(false);

                    while (true) if (!Dln.IsLoad) break; Thread.Sleep(150);
                }
                if (!beStarted)
                {
                    break;
                }
                    
            }
            if (beStarted)
                beStarted = false;
            if (STATIC.isTmpLog)
            {

                Task t = new Task(() => WriteDetailLogs());
                t.Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Task t = new Task(() => Test());
            t.Start();
        }

        private void btnYieldReset_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you wan to Reset and Save Yield Data?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                Spec.LastSampleNum = 0;
                Spec.TotlaTested = 0;
                Spec.TotlaFailed = 0;
                Spec.TotlaPassed = 0;
                for (int i = 0; i < Spec.Param.Count; i++)
                {
                    Spec.Param[i][Spec.FAILED] = 0;
                }
                InitYield();
            }
            Spec.Save();
        }
    }
}

