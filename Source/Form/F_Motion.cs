using System;
using System.Net.Sockets;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using M1Wide;
using DavinciIRISTester;

namespace M1Wide
{
    public partial class F_Motion : Form
    {
        public const int ParameterCount = 15;
        public const double DegreeToPulse = 0.072;
        public double AxisZStroke = 1;
        public double AxisXStroke = 1;
        public double AxisZCurrent = 0.0;
        public double AxisXCurrent = 0.0;
        public bool isFirstHomeSearchingAxis0 = true;
        public bool isFirstHomeSearchingAxis1 = true;
        public bool bEmgStatus = false;
        int beforeCheckStatus = 0;
        int CurrentInspCount = 0;
        int AxisCount;
        bool EmgUse = true;
        bool UserBreak = false;
        string CurrentInspDegreeAxis0 = string.Empty;
        string CurrentInspDegreeAxis1 = string.Empty;
        bool JogMode = false;
        F_MotionMsg mesCon = new F_MotionMsg();
        public Process Process { get { return STATIC.Process; } }
        public DLN Dln { get { return STATIC.Dln; } }

        public F_Motion()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            STATIC.State = (int)STATIC.STATE.Main;
        }

        private void F_Motion_Load(object sender, EventArgs e)
        {
            if (!STATIC.Rcp.Option.isPosture)
                return;
            //AxisXStroke = 2.01 * 2;
            //AxisZStroke = 3.01;

            if(STATIC.Rcp.Option.is1CH_MC)
            {
                AxisXStroke = 4;
                AxisZStroke = 3;
            }
            else
            {
                AxisXStroke = 4;
                AxisZStroke = 4;
            }
        
            uint err = AjinLibrary.AxlOpenNoReset(7);
            if (err == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {

                InitParameterViewer();
                InitInspPosViewer();
                InitControls();
                if (!Directory.Exists(STATIC.BaseDir + "\\MotionData\\DataFile"))
                    Directory.CreateDirectory(STATIC.BaseDir + "\\MotionData\\DataFile");

                if (File.Exists(MotionSettingData.SystemPath))
                    MotionSettingData.LoadSystemPath();
                else
                    MotionSettingData.SaveSystemPath();

                if (File.Exists(MotionSettingData.LastDatPath))
                {
                    LoadData();
                    string s = MotionSettingData.LastDatPath.Substring(MotionSettingData.LastDatPath.LastIndexOf("\\") + 1);
                    tb_CurrJob.Text = s;
                }
                else
                {
                    MotionSettingData.DefaultSetting();
                }

                AddAxisInfo();
            }
            else
            {
                MessageBox.Show(string.Format("Fail Motion : {0}", err));
            }
            ParameterViewer.CellValueChanged += ParameterViewer_CellValueChanged;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(10);
                    if (InvokeRequired)
                    {
                        BeginInvoke((MethodInvoker)delegate
                        {
                            DisplayTimer_Tick();
                        });
                    }
                }

            });


        }

        private void ParameterViewer_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dg = (DataGridView)sender;
            if (dg.CurrentCellAddress.X == 2)
            {
                switch (dg.CurrentCellAddress.Y)
                {
                    case 0:
                        MotionSettingData.VelocityAxis0 = Convert.ToDouble(ParameterViewer[2, 0].Value);
                        break;
                    case 1:
                        MotionSettingData.AccAxis0 = Convert.ToDouble(ParameterViewer[2, 1].Value);
                        break;
                    case 2:
                        MotionSettingData.DecAxis0 = Convert.ToDouble(ParameterViewer[2, 2].Value);
                        break;
                    case 3:
                        MotionSettingData.HomeSetOffsetPAxis0 = Convert.ToDouble(ParameterViewer[2, 3].Value);
                        break;
                    case 4:
                        MotionSettingData.HomeSetOffsetNAxis0 = Convert.ToDouble(ParameterViewer[2, 4].Value);
                        break;
                    case 5:
                        MotionSettingData.HomeClrTimeAxis0 = Convert.ToDouble(ParameterViewer[2, 5].Value);
                        break;
                    case 6:
                        MotionSettingData.Home1stVelAxis0 = Convert.ToDouble(ParameterViewer[2, 6].Value);
                        break;
                    case 7:
                        MotionSettingData.Home2ndVelAxis0 = Convert.ToDouble(ParameterViewer[2, 7].Value);
                        break;
                    case 8:
                        MotionSettingData.Home3rdVelAxis0 = Convert.ToDouble(ParameterViewer[2, 8].Value);
                        break;
                    case 9:
                        MotionSettingData.Home4thVelAxis0 = Convert.ToDouble(ParameterViewer[2, 9].Value);
                        break;
                    case 10:
                        MotionSettingData.Home1stAccAxis0 = Convert.ToDouble(ParameterViewer[2, 10].Value);
                        break;
                    case 11:
                        MotionSettingData.Home2ndAccAxis0 = Convert.ToDouble(ParameterViewer[2, 11].Value);
                        break;
                    case 12:
                        MotionSettingData.SWLimitNAxis0 = Convert.ToDouble(ParameterViewer[2, 12].Value);
                        if (MotionSettingData.SWLimitUseAxis0)
                            AjinLibrary.AxmSignalSetSoftLimit(0, 1, 1, 0, MotionSettingData.SWLimitPAxis0 * AxisZStroke / DegreeToPulse, MotionSettingData.SWLimitNAxis0 * AxisZStroke / DegreeToPulse);
                        break;
                    case 13:
                        MotionSettingData.SWLimitPAxis0 = Convert.ToDouble(ParameterViewer[2, 13].Value);
                        if (MotionSettingData.SWLimitUseAxis0)
                            AjinLibrary.AxmSignalSetSoftLimit(0, 1, 1, 0, MotionSettingData.SWLimitPAxis0 * AxisZStroke / DegreeToPulse, MotionSettingData.SWLimitNAxis0 * AxisZStroke / DegreeToPulse);
                        break;
                    case 14:
                        MotionSettingData.ManualPosAxis0 = Convert.ToDouble(ParameterViewer[2, 14].Value);
                        break;
                }
            }
            else if (dg.CurrentCellAddress.X == 3)
            {
                switch (dg.CurrentCellAddress.Y)
                {
                    case 0:
                        MotionSettingData.VelocityAxis1 = Convert.ToDouble(ParameterViewer[3, 0].Value);
                        break;
                    case 1:
                        MotionSettingData.AccAxis1 = Convert.ToDouble(ParameterViewer[3, 1].Value);
                        break;
                    case 2:
                        MotionSettingData.DecAxis1 = Convert.ToDouble(ParameterViewer[3, 2].Value);
                        break;
                    case 3:
                        MotionSettingData.HomeSetOffsetPAxis1 = Convert.ToDouble(ParameterViewer[3, 3].Value);
                        break;
                    case 4:
                        MotionSettingData.HomeSetOffsetNAxis1 = Convert.ToDouble(ParameterViewer[3, 4].Value);
                        break;
                    case 5:
                        MotionSettingData.HomeClrTimeAxis1 = Convert.ToDouble(ParameterViewer[3, 5].Value);
                        break;
                    case 6:
                        MotionSettingData.Home1stVelAxis1 = Convert.ToDouble(ParameterViewer[3, 6].Value);
                        break;
                    case 7:
                        MotionSettingData.Home2ndVelAxis1 = Convert.ToDouble(ParameterViewer[3, 7].Value);
                        break;
                    case 8:
                        MotionSettingData.Home3rdVelAxis1 = Convert.ToDouble(ParameterViewer[3, 8].Value);
                        break;
                    case 9:
                        MotionSettingData.Home4thVelAxis1 = Convert.ToDouble(ParameterViewer[3, 9].Value);
                        break;
                    case 10:
                        MotionSettingData.Home1stAccAxis1 = Convert.ToDouble(ParameterViewer[3, 10].Value);
                        break;
                    case 11:
                        MotionSettingData.Home2ndAccAxis1 = Convert.ToDouble(ParameterViewer[3, 11].Value);
                        break;
                    case 12:
                        MotionSettingData.SWLimitNAxis1 = Convert.ToDouble(ParameterViewer[3, 12].Value);
                        if (MotionSettingData.SWLimitUseAxis1)
                            AjinLibrary.AxmSignalSetSoftLimit(1, 1, 1, 0, MotionSettingData.SWLimitPAxis1 * AxisXStroke / DegreeToPulse, MotionSettingData.SWLimitNAxis1 * AxisXStroke / DegreeToPulse);
                        break;
                    case 13:
                        MotionSettingData.SWLimitPAxis1 = Convert.ToDouble(ParameterViewer[3, 13].Value);
                        if (MotionSettingData.SWLimitUseAxis1)
                            AjinLibrary.AxmSignalSetSoftLimit(1, 1, 1, 0, MotionSettingData.SWLimitPAxis1 * AxisXStroke / DegreeToPulse, MotionSettingData.SWLimitNAxis1 * AxisXStroke / DegreeToPulse);
                        break;
                    case 14:
                        MotionSettingData.ManualPosAxis1 = Convert.ToDouble(ParameterViewer[3, 14].Value);
                        break;

                }
            }
        }
        void AddInspPos()
        {
            lstPosition.Items.Add("Position " + (InspectionPosViewer.RowCount).ToString());
            InspectionPosViewer.Rows.Add("Position " + (InspectionPosViewer.RowCount).ToString(), "0.0", "0.0", "deg");
            InspectionPosViewer[0, InspectionPosViewer.RowCount - 2].Style.BackColor = Color.Lavender;
            InspectionPosViewer[3, InspectionPosViewer.RowCount - 2].Style.BackColor = Color.Lavender;


            InspectionPosViewer[0, InspectionPosViewer.RowCount - 2].Style.Font = new Font("Calibri", 9, FontStyle.Bold);
            InspectionPosViewer[1, InspectionPosViewer.RowCount - 2].Style.Font = new Font("Calibri", 9, FontStyle.Regular);
            InspectionPosViewer[2, InspectionPosViewer.RowCount - 2].Style.Font = new Font("Calibri", 9, FontStyle.Regular);
            InspectionPosViewer[3, InspectionPosViewer.RowCount - 2].Style.Font = new Font("Calibri", 9, FontStyle.Regular);


            InspectionPosViewer[0, InspectionPosViewer.RowCount - 2].ReadOnly = true;
            InspectionPosViewer[3, InspectionPosViewer.RowCount - 2].ReadOnly = true;
            if (cbInspPosEdit.Checked)
            {
                InspectionPosViewer[1, InspectionPosViewer.RowCount - 2].Style.BackColor = Color.White;
                InspectionPosViewer[2, InspectionPosViewer.RowCount - 2].Style.BackColor = Color.White;
                InspectionPosViewer[1, InspectionPosViewer.RowCount - 2].ReadOnly = false;
                InspectionPosViewer[2, InspectionPosViewer.RowCount - 2].ReadOnly = false;
            }
            else
            {
                InspectionPosViewer[1, InspectionPosViewer.RowCount - 2].Style.BackColor = Color.LightGray;
                InspectionPosViewer[2, InspectionPosViewer.RowCount - 2].Style.BackColor = Color.LightGray;
                InspectionPosViewer[1, InspectionPosViewer.RowCount - 2].ReadOnly = true;
                InspectionPosViewer[2, InspectionPosViewer.RowCount - 2].ReadOnly = true;
            }

        }
        void ReMovePos()
        {
            if (InspectionPosViewer.RowCount - 1 > 0)
            {
                lstPosition.Items.RemoveAt(InspectionPosViewer.RowCount - 2);
                InspectionPosViewer.Rows.Remove(InspectionPosViewer.Rows[InspectionPosViewer.RowCount - 2]);
            }


        }
        void InitInspPosViewer()
        {

            InspectionPosViewer.ColumnCount = 4;
            InspectionPosViewer.Font = new Font("Calibri", 10, FontStyle.Bold);
            for (int i = 0; i < InspectionPosViewer.ColumnCount; i++)
            {
                InspectionPosViewer.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            InspectionPosViewer.RowHeadersVisible = false;
            InspectionPosViewer.BackgroundColor = Color.LightGray;

            InspectionPosViewer.Columns[0].Name = "Poisition";
            InspectionPosViewer.Columns[1].Name = "Block θ";
            InspectionPosViewer.Columns[2].Name = "Socket θ";
            InspectionPosViewer.Columns[3].Name = "unit";

            for (int i = 0; i < InspectionPosViewer.ColumnCount; i++)
                InspectionPosViewer.Columns[i].DefaultCellStyle.Font = new Font("Calibri", 10, FontStyle.Bold);


            InspectionPosViewer.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            InspectionPosViewer.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            InspectionPosViewer.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            InspectionPosViewer.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            InspectionPosViewer.Columns[0].Width = 130;
            InspectionPosViewer.Columns[1].Width = 110;
            InspectionPosViewer.Columns[2].Width = 110;
            InspectionPosViewer.Columns[3].Width = 75;

            InspectionPosViewer.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            InspectionPosViewer.ColumnHeadersHeight = 24;

            for (int i = 1; i < InspectionPosViewer.ColumnCount - 1; i++)
            {
                for (int j = 0; j < InspectionPosViewer.RowCount; j++)
                {
                    InspectionPosViewer[i, j].Style.BackColor = Color.LightGray;

                }

            }

            InspectionPosViewer.ReadOnly = true;
        }
        void InitParameterViewer()
        {
            PublicFunctions m_pub = new PublicFunctions();
            ParameterViewer.ColumnCount = 5;
            ParameterViewer.Font = new Font("Calibri", 10, FontStyle.Bold);
            for (int i = 0; i < ParameterViewer.ColumnCount; i++)
            {
                ParameterViewer.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            ParameterViewer.RowHeadersVisible = false;
            ParameterViewer.BackgroundColor = Color.LightGray;


            ParameterViewer.Columns[0].Name = "Menu";
            ParameterViewer.Columns[1].Name = "Parameter";
            ParameterViewer.Columns[2].Name = "Block θ";
            ParameterViewer.Columns[3].Name = "Socket θ";
            ParameterViewer.Columns[4].Name = "unit";

            for (int i = 0; i < ParameterViewer.ColumnCount; i++)
                ParameterViewer.Columns[i].DefaultCellStyle.Font = new Font("Calibri", 10, FontStyle.Bold);

            ParameterViewer.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ParameterViewer.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            ParameterViewer.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ParameterViewer.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ParameterViewer.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            ParameterViewer.Columns[0].Width = 100;
            ParameterViewer.Columns[1].Width = 105;
            ParameterViewer.Columns[2].Width = 85;
            ParameterViewer.Columns[3].Width = 85;
            ParameterViewer.Columns[4].Width = 50;


            bool bColorChange = true;
            string colTitle = "";

            for (int i = 0; i < ParameterCount; i++)
            {
                if (m_pub.ParameterString[i, 0] != "")
                    colTitle = m_pub.ParameterString[i, 0];

                ParameterViewer.Rows.Add(colTitle, m_pub.ParameterString[i, 1], m_pub.ParameterString[i, 2], m_pub.ParameterString[i, 3], m_pub.ParameterString[i, 4]);
                if (colTitle != "")
                    colTitle = "";

                if (m_pub.ParameterString[i, 0].Length > 0)
                    bColorChange = !bColorChange;

                if (bColorChange)
                {
                    ParameterViewer[0, i].Style.BackColor = Color.Lavender;
                    ParameterViewer[1, i].Style.BackColor = Color.Lavender;
                    ParameterViewer[2, i].Style.BackColor = Color.Lavender;
                    ParameterViewer[3, i].Style.BackColor = Color.Lavender;
                    ParameterViewer[4, i].Style.BackColor = Color.Lavender;


                }
                else
                {
                    ParameterViewer[0, i].Style.BackColor = Color.White;
                    ParameterViewer[1, i].Style.BackColor = Color.White;
                    ParameterViewer[2, i].Style.BackColor = Color.White;
                    ParameterViewer[3, i].Style.BackColor = Color.White;
                    ParameterViewer[4, i].Style.BackColor = Color.White;

                }


            }

            ParameterViewer.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ParameterViewer.ColumnHeadersHeight = 24;

            for (int i = 0; i < ParameterViewer.RowCount; i++)
            {
                ParameterViewer.Rows[i].Height = 22;
                ParameterViewer.Rows[i].Resizable = DataGridViewTriState.False;
                ParameterViewer.Rows[i].DefaultCellStyle.Font = new Font("Calibri", 9);
                ParameterViewer[0, i].Style.Font = new Font("Calibri", 9, FontStyle.Bold);
                ParameterViewer[1, i].Style.Font = new Font("Calibri", 9, FontStyle.Bold);

            }
            for (int i = 2; i < ParameterViewer.ColumnCount - 1; i++)
            {
                for (int j = 0; j < ParameterViewer.RowCount; j++)
                {
                    ParameterViewer[i, j].Style.BackColor = Color.LightGray;

                }

            }
            ParameterViewer.ReadOnly = true;

            //DataGridViewRow rows = ParameterViewer.Rows[1];


        }
        void InitControls()
        {
            cb_Edit.Checked = false;
            cbInspPosEdit.Checked = false;
            cbAxis0SWLimitUse.Checked = false;
            cbAxis1SWLimitUse.Checked = false;
        }
        void SaveData()
        {
            if (MotionSettingData.InspPosCount != InspectionPosViewer.Rows.Count - 1)
                File.Delete(MotionSettingData.LastDatPath);

            MotionSettingData.InspPosCount = InspectionPosViewer.Rows.Count - 1;
            int i = 0;
            MotionSettingData.VelocityAxis0 = Convert.ToDouble(ParameterViewer[2, i++].Value);
            MotionSettingData.AccAxis0 = Convert.ToDouble(ParameterViewer[2, i++].Value);
            MotionSettingData.DecAxis0 = Convert.ToDouble(ParameterViewer[2, i++].Value);

            MotionSettingData.HomeSetOffsetPAxis0 = Convert.ToDouble(ParameterViewer[2, i++].Value);
            MotionSettingData.HomeSetOffsetNAxis0 = Convert.ToDouble(ParameterViewer[2, i++].Value);
            MotionSettingData.HomeClrTimeAxis0 = Convert.ToDouble(ParameterViewer[2, i++].Value);
            MotionSettingData.Home1stVelAxis0 = Convert.ToDouble(ParameterViewer[2, i++].Value);
            MotionSettingData.Home2ndVelAxis0 = Convert.ToDouble(ParameterViewer[2, i++].Value);
            MotionSettingData.Home3rdVelAxis0 = Convert.ToDouble(ParameterViewer[2, i++].Value);
            MotionSettingData.Home4thVelAxis0 = Convert.ToDouble(ParameterViewer[2, i++].Value);
            MotionSettingData.Home1stAccAxis0 = Convert.ToDouble(ParameterViewer[2, i++].Value);
            MotionSettingData.Home2ndAccAxis0 = Convert.ToDouble(ParameterViewer[2, i++].Value);
            MotionSettingData.SWLimitNAxis0 = Convert.ToDouble(ParameterViewer[2, i++].Value);
            MotionSettingData.SWLimitPAxis0 = Convert.ToDouble(ParameterViewer[2, i++].Value);

            MotionSettingData.ManualPosAxis0 = Convert.ToDouble(ParameterViewer[2, i++].Value);
            i = 0;
            MotionSettingData.VelocityAxis1 = Convert.ToDouble(ParameterViewer[3, i++].Value);
            MotionSettingData.AccAxis1 = Convert.ToDouble(ParameterViewer[3, i++].Value);
            MotionSettingData.DecAxis1 = Convert.ToDouble(ParameterViewer[3, i++].Value);

            MotionSettingData.HomeSetOffsetPAxis1 = Convert.ToDouble(ParameterViewer[3, i++].Value);
            MotionSettingData.HomeSetOffsetNAxis1 = Convert.ToDouble(ParameterViewer[3, i++].Value);
            MotionSettingData.HomeClrTimeAxis1 = Convert.ToDouble(ParameterViewer[3, i++].Value);
            MotionSettingData.Home1stVelAxis1 = Convert.ToDouble(ParameterViewer[3, i++].Value);
            MotionSettingData.Home2ndVelAxis1 = Convert.ToDouble(ParameterViewer[3, i++].Value);
            MotionSettingData.Home3rdVelAxis1 = Convert.ToDouble(ParameterViewer[3, i++].Value);
            MotionSettingData.Home4thVelAxis1 = Convert.ToDouble(ParameterViewer[3, i++].Value);
            MotionSettingData.Home1stAccAxis1 = Convert.ToDouble(ParameterViewer[3, i++].Value);
            MotionSettingData.Home2ndAccAxis1 = Convert.ToDouble(ParameterViewer[3, i++].Value);

            MotionSettingData.SWLimitNAxis1 = Convert.ToDouble(ParameterViewer[3, i++].Value);
            MotionSettingData.SWLimitPAxis1 = Convert.ToDouble(ParameterViewer[3, i++].Value);
            MotionSettingData.ManualPosAxis1 = Convert.ToDouble(ParameterViewer[3, i++].Value);


            MotionSettingData.SaveData(MotionSettingData.LastDatPath);

            PublicFunctions pubFun = new PublicFunctions();
            for (int j = 0; j < MotionSettingData.InspPosCount; j++)
            {
                pubFun.SaveProfile(SettingType.InspectionPosSetting.ToString(), "Position" + (j + 1).ToString() + "Axis0", (string)InspectionPosViewer[1, j].Value, MotionSettingData.LastDatPath);
                pubFun.SaveProfile(SettingType.InspectionPosSetting.ToString(), "Position" + (j + 1).ToString() + "Axis1", (string)InspectionPosViewer[2, j].Value, MotionSettingData.LastDatPath);
            }
        }
        void LoadData()
        {

            MotionSettingData.LoadData(MotionSettingData.LastDatPath);

            int i = 0;
            ParameterViewer[2, i++].Value = MotionSettingData.VelocityAxis0.ToString();
            ParameterViewer[2, i++].Value = MotionSettingData.AccAxis0.ToString();
            ParameterViewer[2, i++].Value = MotionSettingData.DecAxis0.ToString();

            ParameterViewer[2, i++].Value = MotionSettingData.HomeSetOffsetPAxis0.ToString();
            ParameterViewer[2, i++].Value = MotionSettingData.HomeSetOffsetNAxis0.ToString();
            ParameterViewer[2, i++].Value = MotionSettingData.HomeClrTimeAxis0.ToString();
            ParameterViewer[2, i++].Value = MotionSettingData.Home1stVelAxis0.ToString();
            ParameterViewer[2, i++].Value = MotionSettingData.Home2ndVelAxis0.ToString();
            ParameterViewer[2, i++].Value = MotionSettingData.Home3rdVelAxis0.ToString();
            ParameterViewer[2, i++].Value = MotionSettingData.Home4thVelAxis0.ToString();
            ParameterViewer[2, i++].Value = MotionSettingData.Home1stAccAxis0.ToString();
            ParameterViewer[2, i++].Value = MotionSettingData.Home2ndAccAxis0.ToString();

            ParameterViewer[2, i++].Value = MotionSettingData.SWLimitNAxis0.ToString();
            ParameterViewer[2, i++].Value = MotionSettingData.SWLimitPAxis0.ToString();
            ParameterViewer[2, i++].Value = MotionSettingData.ManualPosAxis0.ToString();


            i = 0;
            ParameterViewer[3, i++].Value = MotionSettingData.VelocityAxis1.ToString();
            ParameterViewer[3, i++].Value = MotionSettingData.AccAxis1.ToString();
            ParameterViewer[3, i++].Value = MotionSettingData.DecAxis1.ToString();

            ParameterViewer[3, i++].Value = MotionSettingData.HomeSetOffsetPAxis1.ToString();
            ParameterViewer[3, i++].Value = MotionSettingData.HomeSetOffsetNAxis1.ToString();
            ParameterViewer[3, i++].Value = MotionSettingData.HomeClrTimeAxis1.ToString();
            ParameterViewer[3, i++].Value = MotionSettingData.Home1stVelAxis1.ToString();
            ParameterViewer[3, i++].Value = MotionSettingData.Home2ndVelAxis1.ToString();
            ParameterViewer[3, i++].Value = MotionSettingData.Home3rdVelAxis1.ToString();
            ParameterViewer[3, i++].Value = MotionSettingData.Home4thVelAxis1.ToString();
            ParameterViewer[3, i++].Value = MotionSettingData.Home1stAccAxis1.ToString();
            ParameterViewer[3, i++].Value = MotionSettingData.Home2ndAccAxis1.ToString();

            ParameterViewer[3, i++].Value = MotionSettingData.SWLimitNAxis1.ToString();
            ParameterViewer[3, i++].Value = MotionSettingData.SWLimitPAxis1.ToString();
            ParameterViewer[3, i++].Value = MotionSettingData.ManualPosAxis1.ToString();
      
            if (MotionSettingData.SWLimitUseAxis0)
                cbAxis0SWLimitUse.Checked = true;
            else
                cbAxis0SWLimitUse.Checked = false;

            if (MotionSettingData.SWLimitUseAxis0)
                cbAxis1SWLimitUse.Checked = true;
            else
                cbAxis1SWLimitUse.Checked = false;

            PublicFunctions pubFun = new PublicFunctions();
            int temp = InspectionPosViewer.RowCount - 1;
            if (MotionSettingData.InspPosCount != temp)
            {
                if (MotionSettingData.InspPosCount > temp)
                {
                    for (int k = 0; k < MotionSettingData.InspPosCount - temp; k++)
                    {
                        AddInspPos();
                    }
                }
                else
                {
                    for (int k = 0; k < temp - MotionSettingData.InspPosCount; k++)
                    {
                        ReMovePos();
                    }
                }
            }
            lstPosition.Items.Clear();

            for (int j = 0; j < MotionSettingData.InspPosCount; j++)
            {

                InspectionPosViewer[1, j].Value = pubFun.LoadProfile(SettingType.InspectionPosSetting.ToString(), "Position" + (j + 1).ToString() + "Axis0", MotionSettingData.LastDatPath);
                InspectionPosViewer[2, j].Value = pubFun.LoadProfile(SettingType.InspectionPosSetting.ToString(), "Position" + (j + 1).ToString() + "Axis1", MotionSettingData.LastDatPath);
                lstPosition.Items.Add("Position " + (j + 1).ToString());
            }
        }
        void AddAxisInfo()
        {
            AjinLibrary.AxmInfoGetAxisCount(ref AxisCount);

            if (EmgUse)
            {
                AjinLibrary.AxmSignalSetStop(0, 0, 1);
                AjinLibrary.AxmSignalSetStop(1, 0, 1);
            }
            else
            {
                AjinLibrary.AxmSignalSetStop(0, 0, 2);
                AjinLibrary.AxmSignalSetStop(1, 0, 2);
            }

            if (STATIC.Rcp.Option.isPosture && STATIC.Rcp.Option.is1CH_MC)
                AjinLibrary.AxmMotSetPulseOutMethod(0, 4);
            else AjinLibrary.AxmMotSetPulseOutMethod(0, 6);
            AjinLibrary.AxmMotSetPulseOutMethod(1, 6);

            if (MotionSettingData.SWLimitUseAxis0)
                AjinLibrary.AxmSignalSetSoftLimit(0, 1, 1, 0, MotionSettingData.SWLimitPAxis0 * AxisZStroke / DegreeToPulse, MotionSettingData.SWLimitNAxis0 * AxisZStroke / DegreeToPulse);
            else
                AjinLibrary.AxmSignalSetSoftLimit(0, 0, 1, 0, MotionSettingData.SWLimitPAxis0 * AxisZStroke / DegreeToPulse, MotionSettingData.SWLimitNAxis0 * AxisZStroke / DegreeToPulse);

            if (MotionSettingData.SWLimitUseAxis1)
                AjinLibrary.AxmSignalSetSoftLimit(1, 1, 1, 0, MotionSettingData.SWLimitPAxis1 * AxisXStroke / DegreeToPulse, MotionSettingData.SWLimitNAxis1 * AxisXStroke / DegreeToPulse);
            else
                AjinLibrary.AxmSignalSetSoftLimit(1, 0, 1, 0, MotionSettingData.SWLimitPAxis1 * AxisXStroke / DegreeToPulse, MotionSettingData.SWLimitNAxis1 * AxisXStroke / DegreeToPulse);
        }
        void buttonDisable(bool isAuto)
        {
            if (isAuto)
            {
                cbAxis0SWLimitUse.Enabled = false;
                cbAxis1SWLimitUse.Enabled = false;
                btnMoveSetPos.Enabled = false;
                btnAddInspPos.Enabled = false;
                btnMinusInspPos.Enabled = false;
            
                btnPositionClearAxis0.Enabled = false;
                btnPositionClearAxis1.Enabled = false;
                btnMoveManualPosAxis0.Enabled = false;
                btnMoveManualPosAxis1.Enabled = false;
                btnJogNAxis0.Enabled = false;
                btnJogNAxis1.Enabled = false;
                btnJogPAxis0.Enabled = false;
                btnJogPAxis1.Enabled = false;

                btnMoveHomeAxis0.Enabled = false;
                btnMoveHomeAxis1.Enabled = false;
                btnOpen.Enabled = false;
                cb_Edit.Checked = false;
                cb_Edit.Enabled = false;
                cbInspPosEdit.Checked = false;
                cbInspPosEdit.Enabled = false;
                StaticVariables.isAutoRun = true;
            }
            else
            {
                cbAxis0SWLimitUse.Enabled = true;
                cbAxis1SWLimitUse.Enabled = true;
                btnMoveSetPos.Enabled = true;
                btnAddInspPos.Enabled = true;
                btnMinusInspPos.Enabled = true;
             
                btnPositionClearAxis0.Enabled = true;
                btnPositionClearAxis1.Enabled = true;
                btnMoveManualPosAxis0.Enabled = true;
                btnMoveManualPosAxis1.Enabled = true;
                btnJogNAxis0.Enabled = true;
                btnJogNAxis1.Enabled = true;
                btnJogPAxis0.Enabled = true;
                btnJogPAxis1.Enabled = true;

                btnMoveHomeAxis0.Enabled = true;
                btnMoveHomeAxis1.Enabled = true;
                btnOpen.Enabled = true;
                cb_Edit.Enabled = true;
                cbInspPosEdit.Enabled = true;
                StaticVariables.isAutoRun = false;
            }
        }
        private void DisplayTimer_Tick()
        {
            int HomeStatus = 7;
            int EmgStatus = 6;
            int[] iDigitReadMotion = { 0, 1, 2, 3, 11 };
            double dcmdPos = 0.0;
            double dVelocity = 0.0;
            int iCheck = 0;
            uint duRetcode = 0;
            uint duStatus = 0;
            uint duHome = 0;

            uint duHomeSearch = 0;

            for (int i = 0; i < AxisCount; i++)
            {
                AjinLibrary.AxmStatusGetCmdPos(i, ref dcmdPos);
                AjinLibrary.AxmStatusReadVel(i, ref dVelocity);
                AjinLibrary.AxmStatusReadMechanical(i, ref duHome);
                AjinLibrary.AxmHomeGetResult(i, ref duHomeSearch);

                duRetcode = AjinLibrary.AxmStatusReadMotion(i, ref duStatus);

                if (i == 0)
                {
                    lblCommandPosAxis0.Text = String.Format("{0:0.000}", AxisZCurrent = dcmdPos * DegreeToPulse / AxisZStroke);
                    lblCommandVelocityAxis0.Text = String.Format("{0:0.000}", dVelocity * DegreeToPulse / AxisZStroke);


                    if (duHomeSearch == (int)AXT_MOTION_HOME_RESULT.HOME_SUCCESS || duHomeSearch == (int)AXT_MOTION_HOME_RESULT.HOME_ERR_UNKNOWN)
                    {
                        lblHomeSearchAxis0.BackColor = Color.Transparent;
                    }
                    else if (duHomeSearch == (int)AXT_MOTION_HOME_RESULT.HOME_SEARCHING)
                    {
                        lblHomeSearchAxis0.BackColor = Color.YellowGreen;
                    }
                    else
                    {
                        lblHomeSearchAxis0.BackColor = Color.Red;
                    }

                    iCheck = ((int)duHome >> HomeStatus & 0x1);
                    if (iCheck == 1)
                    {
                        lblHomeAxis0.BackColor = Color.YellowGreen;
                    }
                    else
                    {
                        lblHomeAxis0.BackColor = Color.Transparent;
                        //   HomeCheckAxis0 = true;
                    }

                    if (EmgUse)
                    {
                        iCheck = ((int)duHome >> EmgStatus & 0x1);
                        if (iCheck == 0)
                        {
                            beforeCheckStatus = 0;
                            lblEmgStopAxis0.BackColor = Color.Transparent;
                            bEmgStatus = false;
                        }
                        else
                        {
                            if (beforeCheckStatus == 0 && StaticVariables.FromTester)
                            {
                             
                                buttonDisable(false);
                                UserBreak = true;
                            }
                            beforeCheckStatus = 1;
                            lblEmgStopAxis0.BackColor = Color.Red;
                            bEmgStatus = true;
                        }
                    }

                    if (MotionSettingData.SWLimitUseAxis0)
                    {
                        if (Convert.ToDouble(lblCommandPosAxis0.Text) >= MotionSettingData.SWLimitPAxis0)
                        {
                            lblMotionLimitAxis0.BackColor = Color.YellowGreen;
                            StaticVariables.isMotionLimitAxis0 = true;
                        }
                        else if (Convert.ToDouble(lblCommandPosAxis0.Text) <= MotionSettingData.SWLimitNAxis0)
                        {
                            lblMotionLimitAxis0.BackColor = Color.Red;
                            StaticVariables.isMotionLimitAxis0 = true;
                        }
                        else
                        {
                            lblMotionLimitAxis0.BackColor = Color.Transparent;
                            StaticVariables.isMotionLimitAxis0 = false;
                        }
                    }
                    else
                    {
                        lblMotionLimitAxis0.BackColor = Color.Transparent;
                        StaticVariables.isMotionLimitAxis0 = false;
                    }


                    if (duRetcode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            iCheck = ((int)duStatus >> iDigitReadMotion[j] & 0x1);

                            switch (j)
                            {
                                case 0:
                                    if (iCheck == 1)
                                    {
                                        StaticVariables.isMotionBusyAxis0 = true;
                                        lblMotionBusyAxis0.BackColor = Color.YellowGreen;
                                    }
                                    else
                                    {
                                        StaticVariables.isMotionBusyAxis0 = false;
                                        lblMotionBusyAxis0.BackColor = Color.Transparent;
                                    }
                                    break;
                                case 1:
                                    if (iCheck == 1)
                                        lblMotionDecelAxis0.BackColor = Color.YellowGreen;
                                    else
                                        lblMotionDecelAxis0.BackColor = Color.Transparent;
                                    break;
                                case 2:
                                    if (iCheck == 1)
                                        lblMotionConstantAxis0.BackColor = Color.YellowGreen;
                                    else
                                        lblMotionConstantAxis0.BackColor = Color.Transparent;
                                    break;
                                case 3:
                                    if (iCheck == 1)
                                        lblMotionAccAxis0.BackColor = Color.YellowGreen;
                                    else
                                        lblMotionAccAxis0.BackColor = Color.Transparent;
                                    break;
                                case 4:
                                    if (lblMotionBusyAxis0.BackColor == Color.YellowGreen)
                                    {
                                        if (iCheck == 1)
                                            lblMotionDirectionAxis0.BackColor = Color.Red;
                                        else
                                            lblMotionDirectionAxis0.BackColor = Color.YellowGreen;
                                        break;
                                    }
                                    else
                                        lblMotionDirectionAxis0.BackColor = Color.Transparent;
                                    break;

                                default:
                                    break;
                            }


                        }
                    }
                }
                else if(i == 1)
                {
                    lblCommandPosAxis1.Text = string.Format("{0:0.000}", AxisXCurrent = dcmdPos * DegreeToPulse / AxisXStroke);
                    lblCommandVelocityAxis1.Text = string.Format("{0:0.000}", dVelocity * DegreeToPulse / AxisXStroke);


                    if (duHomeSearch == (int)AXT_MOTION_HOME_RESULT.HOME_SUCCESS || duHomeSearch == (int)AXT_MOTION_HOME_RESULT.HOME_ERR_UNKNOWN)
                    {

                        lblHomeSearchAxis1.BackColor = Color.Transparent;
                    }
                    else if (duHomeSearch == (int)AXT_MOTION_HOME_RESULT.HOME_SEARCHING)
                    {
                        lblHomeSearchAxis1.BackColor = Color.YellowGreen;
                    }
                    else
                    {
                        lblHomeSearchAxis1.BackColor = Color.Red;
                    }

                    iCheck = ((int)duHome >> HomeStatus & 0x1);
                    if (iCheck == 1)
                    {
                        lblHomeAxis1.BackColor = Color.YellowGreen;

                    }
                    else
                    {
                        lblHomeAxis1.BackColor = Color.Transparent;
                        //HomeCheckAxis1 = true;

                    }


                    if (EmgUse)
                    {
                        iCheck = ((int)duHome >> EmgStatus & 0x1);
                        if (iCheck == 0)
                        {
                            lblEmgStopAxis1.BackColor = Color.Transparent;
                        }
                        else
                        {
                            lblEmgStopAxis1.BackColor = Color.Red;
                        }
                    }



                    if (MotionSettingData.SWLimitUseAxis1)
                    {
                        if (Convert.ToDouble(lblCommandPosAxis1.Text) >= MotionSettingData.SWLimitPAxis1)
                        {
                            lblMotionLimitAxis1.BackColor = Color.YellowGreen;
                            StaticVariables.isMotionLimitAxis1 = true;
                        }
                        else if (Convert.ToDouble(lblCommandPosAxis1.Text) <= MotionSettingData.SWLimitNAxis1)
                        {
                            lblMotionLimitAxis1.BackColor = Color.Red;
                            StaticVariables.isMotionLimitAxis1 = true;
                        }
                        else
                        {
                            lblMotionLimitAxis1.BackColor = Color.Transparent;
                            StaticVariables.isMotionLimitAxis1 = false;
                        }
                    }

                    else
                    {
                        lblMotionLimitAxis1.BackColor = Color.Transparent;
                        StaticVariables.isMotionLimitAxis1 = false;
                    }

                    if (duRetcode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            iCheck = ((int)duStatus >> iDigitReadMotion[j] & 0x1);

                            switch (j)
                            {
                                case 0:
                                    if (iCheck == 1)
                                    {
                                        StaticVariables.isMotionBusyAxis1 = true;
                                        lblMotionBusyAxis1.BackColor = Color.YellowGreen;

                                    }

                                    else
                                    {
                                        StaticVariables.isMotionBusyAxis1 = false;
                                        lblMotionBusyAxis1.BackColor = Color.Transparent;

                                    }

                                    break;
                                case 1:
                                    if (iCheck == 1)
                                        lblMotionDecelAxis1.BackColor = Color.YellowGreen;
                                    else
                                        lblMotionDecelAxis1.BackColor = Color.Transparent;
                                    break;
                                case 2:
                                    if (iCheck == 1)
                                        lblMotionConstantAxis1.BackColor = Color.YellowGreen;
                                    else
                                        lblMotionConstantAxis1.BackColor = Color.Transparent;
                                    break;
                                case 3:
                                    if (iCheck == 1)
                                        lblMotionAccAxis1.BackColor = Color.YellowGreen;
                                    else
                                        lblMotionAccAxis1.BackColor = Color.Transparent;
                                    break;
                                case 4:
                                    if (lblMotionBusyAxis1.BackColor == Color.YellowGreen)
                                    {
                                        if (iCheck == 1)
                                            lblMotionDirectionAxis1.BackColor = Color.Red;
                                        else
                                            lblMotionDirectionAxis1.BackColor = Color.YellowGreen;
                                        break;
                                    }
                                    else
                                        lblMotionDirectionAxis1.BackColor = Color.Transparent;
                                    break;
                                default:
                                    break;
                            }


                        }
                    }
                }

            }
          
        }
        private void F_Motion_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!STATIC.Rcp.Option.isPosture)
                return;
            uint duRetCode = 0;

            PublicFunctions pubFun = new PublicFunctions();

            pubFun.SaveProfile(SettingType.Path.ToString(), "LastDatPath", MotionSettingData.LastDatPath, MotionSettingData.SystemPath);
            duRetCode = AjinLibrary.AxmMoveEStop(0);
            duRetCode = AjinLibrary.AxmMoveEStop(1);

            Thread.Sleep(100);
          
            AjinLibrary.AxlClose();
        }
        void ShowLog(string s)
        {
            string Log = string.Format("{0}: {1}.", DateTime.Now.ToString("T"), s);
            if (MotionLog.InvokeRequired)
            {
                MotionLog.Invoke(new MethodInvoker(delegate ()
                {
                    MotionLog.AppendText(Log + "\r\n");
                }));
            }
            else
            {
                MotionLog.AppendText(Log + "\r\n");
            }


        }
        public double[] MoveSelectedPosition(int PosIndex, bool isManual)
        {
            double[] degree = new double[2];
            uint duRetCode = 0;
            int[] AxisNo = { 0, 1 };
            double[] dMultiPos = { 0.0, 0.0 }, dMultiVel = { 0.0, 0.0 },
                     dMultiAcc = { 0.0, 0.0 }, dMultiDec = { 0.0, 0.0 };
            if (!StaticVariables.isSafetyOn)
            {
              
                if (PosIndex != -1)
                {
                    dMultiPos[0] = (degree[0] = Convert.ToDouble(InspectionPosViewer[1, PosIndex].Value)) * AxisZStroke / DegreeToPulse;
                    dMultiPos[1] = (degree[1] = Convert.ToDouble(InspectionPosViewer[2, PosIndex].Value)) * AxisXStroke / DegreeToPulse;


                    dMultiVel[0] = MotionSettingData.VelocityAxis0 * AxisZStroke / DegreeToPulse;
                    dMultiVel[1] = MotionSettingData.VelocityAxis1 * AxisXStroke / DegreeToPulse;

                    dMultiAcc[0] = MotionSettingData.AccAxis0 * AxisZStroke / DegreeToPulse;
                    dMultiAcc[1] = MotionSettingData.AccAxis1 * AxisXStroke / DegreeToPulse;

                    dMultiDec[0] = MotionSettingData.DecAxis0 * AxisZStroke / DegreeToPulse;
                    dMultiDec[1] = MotionSettingData.DecAxis1 * AxisXStroke / DegreeToPulse;
                    if (isManual)
                        duRetCode = AjinLibrary.AxmMoveStartMultiPos(2, AxisNo, dMultiPos, dMultiVel, dMultiAcc, dMultiDec);
                    else
                        duRetCode = AjinLibrary.AxmMoveMultiPos(2, AxisNo, dMultiPos, dMultiVel, dMultiAcc, dMultiDec);
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        ShowLog(((AXT_FUNC_RESULT)duRetCode).ToString());
                }
                else
                {
                    dMultiPos[0] = (degree[0] = 0) * AxisZStroke / DegreeToPulse;
                    dMultiPos[1] = (degree[1] = 0) * AxisXStroke / DegreeToPulse;


                    dMultiVel[0] = MotionSettingData.VelocityAxis0 * AxisZStroke / DegreeToPulse;
                    dMultiVel[1] = MotionSettingData.VelocityAxis1 * AxisXStroke / DegreeToPulse;

                    dMultiAcc[0] = MotionSettingData.AccAxis0 * AxisZStroke / DegreeToPulse;
                    dMultiAcc[1] = MotionSettingData.AccAxis1 * AxisXStroke / DegreeToPulse;

                    dMultiDec[0] = MotionSettingData.DecAxis0 * AxisZStroke / DegreeToPulse;
                    dMultiDec[1] = MotionSettingData.DecAxis1 * AxisXStroke / DegreeToPulse;
                    if (isManual)
                        duRetCode = AjinLibrary.AxmMoveStartMultiPos(2, AxisNo, dMultiPos, dMultiVel, dMultiAcc, dMultiDec);
                    else
                        duRetCode = AjinLibrary.AxmMoveMultiPos(2, AxisNo, dMultiPos, dMultiVel, dMultiAcc, dMultiDec);
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        ShowLog(((AXT_FUNC_RESULT)duRetCode).ToString());
                }
                
            }

            return degree;
        }
        DialogResult ShowMessageCon(Image imgPath, string message)
        {
            DialogResult dr1 = DialogResult.None;

            mesCon = new F_MotionMsg();
            mesCon.SetMessageCon(imgPath, message);

            dr1 = mesCon.ShowDialog();
            mesCon.TopMost = true;
            return dr1;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "dat files(*.dat)|*.dat|All files(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;
            ofd.InitialDirectory = STATIC.BaseDir + "\\MotionData\\DataFile";
            if (ofd.ShowDialog() != DialogResult.Cancel)
            {
                MotionSettingData.LastDatPath = ofd.FileName;

                LoadData();
                string s = MotionSettingData.LastDatPath.Substring(MotionSettingData.LastDatPath.LastIndexOf("\\") + 1);
                tb_CurrJob.Text = s;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (File.Exists(MotionSettingData.LastDatPath))
                SaveData();
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "dat files(*.dat)|*.dat|All files(*.*)|*.*";
                sfd.FilterIndex = 1;
                sfd.RestoreDirectory = true;
                sfd.FileName = "";
                sfd.InitialDirectory = STATIC.BaseDir + "\\MotionData\\DataFile";

                if (sfd.ShowDialog() != DialogResult.Cancel)
                {
                    MotionSettingData.LastDatPath = sfd.FileName;
                    SaveData();
                    string s = MotionSettingData.LastDatPath.Substring(MotionSettingData.LastDatPath.LastIndexOf("\\") + 1);
                    tb_CurrJob.Text = s;
                }
            }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "dat files(*.dat)|*.dat|All files(*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            sfd.FileName = "";
            sfd.InitialDirectory = STATIC.BaseDir + "\\MotionData\\DataFile";

            if (sfd.ShowDialog() != DialogResult.Cancel)
            {
                MotionSettingData.LastDatPath = sfd.FileName;
                SaveData();
                string s = MotionSettingData.LastDatPath.Substring(MotionSettingData.LastDatPath.LastIndexOf("\\") + 1);
                tb_CurrJob.Text = s;
            }
        }

        private void btnJogNAxis0_MouseDown(object sender, MouseEventArgs e)
        {
            uint duRetCode = 0;
            double dVelocity = 0.0;
            double dAccel = 0.0;
            double dDecel = 0.0;

            Button btn = (Button)sender;
            if (!StaticVariables.isSafetyOn)
            {
            
                switch (btn.Name)
                {

                    case "btnJogPAxis0":
                        dVelocity = MotionSettingData.VelocityAxis0 * AxisZStroke / DegreeToPulse;
                        dAccel = MotionSettingData.AccAxis0 * AxisZStroke / DegreeToPulse;
                        dDecel = MotionSettingData.DecAxis0 * AxisZStroke / DegreeToPulse;
                        duRetCode = AjinLibrary.AxmMoveVel(0, dVelocity, dAccel, dDecel);
                        break;
                    case "btnJogNAxis0":
                        dVelocity = MotionSettingData.VelocityAxis0 * AxisZStroke / DegreeToPulse;
                        dAccel = MotionSettingData.AccAxis0 * AxisZStroke / DegreeToPulse;
                        dDecel = MotionSettingData.DecAxis0 * AxisZStroke / DegreeToPulse;
                        duRetCode = AjinLibrary.AxmMoveVel(0, -dVelocity, dAccel, dDecel);
                        break;
                    case "btnJogPAxis1":
                        dVelocity = MotionSettingData.VelocityAxis1 * AxisXStroke / DegreeToPulse;
                        dAccel = MotionSettingData.AccAxis1 * AxisXStroke / DegreeToPulse;
                        dDecel = MotionSettingData.DecAxis1 * AxisXStroke / DegreeToPulse;
                        duRetCode = AjinLibrary.AxmMoveVel(1, dVelocity, dAccel, dDecel);
                        break;
                    case "btnJogNAxis1":
                        dVelocity = MotionSettingData.VelocityAxis1 * AxisXStroke / DegreeToPulse;
                        dAccel = MotionSettingData.AccAxis1 * AxisXStroke / DegreeToPulse;
                        dDecel = MotionSettingData.DecAxis1 * AxisXStroke / DegreeToPulse;
                        duRetCode = AjinLibrary.AxmMoveVel(1, -dVelocity, dAccel, dDecel);
                        break;
                    default:
                        break;
                }
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ShowLog(((AXT_FUNC_RESULT)duRetCode).ToString());
                    JogMode = false;
                }

                else
                    JogMode = true;
            }

        }

        private void btnJogNAxis0_MouseUp(object sender, MouseEventArgs e)
        {
            uint duRetCode = 0;
            Button btn = (Button)sender;
            if (JogMode)
            {
                switch (btn.Name)
                {
                    case "btnJogPAxis0":
                        duRetCode = AjinLibrary.AxmMoveSStop(0);
                        break;
                    case "btnJogNAxis0":
                        duRetCode = AjinLibrary.AxmMoveSStop(0);
                        break;
                    case "btnJogPAxis1":
                        duRetCode = AjinLibrary.AxmMoveSStop(1);
                        break;
                    case "btnJogNAxis1":
                        duRetCode = AjinLibrary.AxmMoveSStop(1);
                        break;
                    default:
                        break;
                }
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    ShowLog(((AXT_FUNC_RESULT)duRetCode).ToString());

                JogMode = false;
            }
        }

        private void cbInspPosEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (cbInspPosEdit.Checked)
            {
                InspectionPosViewer.ReadOnly = false;
                for (int i = 0; i < InspectionPosViewer.Rows.Count; i++)
                {

                    InspectionPosViewer[0, i].ReadOnly = true;

                    InspectionPosViewer[1, i].Style.BackColor = Color.White;
                    InspectionPosViewer[2, i].Style.BackColor = Color.White;
                }

            }
            else
            {
                InspectionPosViewer.ReadOnly = true;
                for (int i = 0; i < InspectionPosViewer.Rows.Count; i++)
                {
                    InspectionPosViewer[1, i].Style.BackColor = Color.LightGray;
                    InspectionPosViewer[2, i].Style.BackColor = Color.LightGray;
                }

            }
        }

        private void cb_Edit_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_Edit.Checked)
            {
                ParameterViewer.ReadOnly = false;
                for (int i = 0; i < ParameterViewer.Rows.Count; i++)
                {

                    ParameterViewer[0, i].ReadOnly = true;
                    ParameterViewer[1, i].ReadOnly = true;
                    ParameterViewer[2, i].Style.BackColor = Color.White;
                    ParameterViewer[3, i].Style.BackColor = Color.White;
                }

            }
            else
            {
                ParameterViewer.ReadOnly = true;
                for (int i = 0; i < ParameterViewer.Rows.Count; i++)
                {
                    ParameterViewer[2, i].Style.BackColor = Color.LightGray;
                    ParameterViewer[3, i].Style.BackColor = Color.LightGray;
                }

            }
        }

        private void cbAxis0SWLimitUse_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                switch (chk.Name)
                {
                    case "cbAxis0SWLimitUse":
                        cbAxis0SWLimitUse.BackColor = Color.YellowGreen;
                        MotionSettingData.SWLimitUseAxis0 = true;
                        AjinLibrary.AxmSignalSetSoftLimit(0, 1, 1, 0, MotionSettingData.SWLimitPAxis0 * AxisZStroke / DegreeToPulse, MotionSettingData.SWLimitNAxis0 * AxisZStroke / DegreeToPulse);

                        break;
                    case "cbAxis1SWLimitUse":
                        cbAxis1SWLimitUse.BackColor = Color.YellowGreen;
                        MotionSettingData.SWLimitUseAxis1 = true;
                        AjinLibrary.AxmSignalSetSoftLimit(1, 1, 1, 0, MotionSettingData.SWLimitPAxis1 * AxisXStroke / DegreeToPulse, MotionSettingData.SWLimitNAxis1 * AxisXStroke / DegreeToPulse);
                        break;
                    default:
                        break;

                }
            }
            else
            {
                switch (chk.Name)
                {
                    case "cbAxis0SWLimitUse":
                        cbAxis0SWLimitUse.BackColor = Color.Transparent;
                        MotionSettingData.SWLimitUseAxis0 = false;
                        AjinLibrary.AxmSignalSetSoftLimit(0, 0, 1, 0, MotionSettingData.SWLimitPAxis0 * AxisZStroke / DegreeToPulse, MotionSettingData.SWLimitNAxis0 * AxisZStroke / DegreeToPulse);
                        break;
                    case "cbAxis1SWLimitUse":
                        cbAxis1SWLimitUse.BackColor = Color.Transparent;
                        MotionSettingData.SWLimitUseAxis1 = false;
                        AjinLibrary.AxmSignalSetSoftLimit(1, 0, 1, 0, MotionSettingData.SWLimitPAxis1 * AxisXStroke / DegreeToPulse, MotionSettingData.SWLimitNAxis1 * AxisXStroke / DegreeToPulse);
                        break;

                    default:
                        break;
                }
            }

        }

        private void btnMoveManualPosAxis0_Click(object sender, EventArgs e)
        {
            uint duRetCode = 0;
            double dPosition = 0.0;
            double dVelocity = 0.0;
            double dAccel = 0.0;
            double dDecel = 0.0;
            Button btn = (Button)sender;
            if (!StaticVariables.isSafetyOn)
            {
                switch (btn.Name)
                {
                    case "btnMoveManualPosAxis0":
                        dPosition = MotionSettingData.ManualPosAxis0 * AxisZStroke / DegreeToPulse;
                        dVelocity = MotionSettingData.VelocityAxis0 * AxisZStroke / DegreeToPulse;
                        dAccel = MotionSettingData.AccAxis0 * AxisZStroke / DegreeToPulse;
                        dDecel = MotionSettingData.DecAxis0 * AxisZStroke / DegreeToPulse;

                        duRetCode = AjinLibrary.AxmMoveStartPos(0, dPosition, dVelocity, dAccel, dDecel);

                        break;
                    case "btnMoveManualPosAxis1":
                        dPosition = MotionSettingData.ManualPosAxis1 * AxisXStroke / DegreeToPulse;
                        dVelocity = MotionSettingData.VelocityAxis1 * AxisXStroke / DegreeToPulse;
                        dAccel = MotionSettingData.AccAxis1 * AxisXStroke / DegreeToPulse;
                        dDecel = MotionSettingData.DecAxis1 * AxisXStroke / DegreeToPulse;

                        duRetCode = AjinLibrary.AxmMoveStartPos(1, dPosition, dVelocity, dAccel, dDecel);
                        break;
                    default:
                        break;
                }
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    ShowLog(((AXT_FUNC_RESULT)duRetCode).ToString());
            }
        }

        private void btnMoveHomeAxis0_Click(object sender, EventArgs e)
        {
            if (Dln.IsEMG) return;
            bool isValueExist = false;
            uint duRetCode = 999;
            uint duHomeInfo = 0;
            Button btn = (Button)sender;
            CurrentInspCount = 0;
         
            if (!StaticVariables.isSafetyOn)
            {

                switch (btn.Name)
                {
                    case "btnMoveHomeAxis0":

                        int tmpint = (int)Math.Round(Convert.ToDouble(lblCommandPosAxis0.Text));
                        for (int i = 0; i < InspectionPosViewer.RowCount - 1; i++)
                        {
                            if (tmpint == (int)Math.Round(Convert.ToDouble(InspectionPosViewer[1, i].Value)))
                            {
                                isValueExist = true;
                                break;
                            }
                        }
                        if (isValueExist && !isFirstHomeSearchingAxis0)
                        {
                           
                            AjinLibrary.AxmHomeSetVel(0, MotionSettingData.Home1stVelAxis0 * AxisZStroke / DegreeToPulse, MotionSettingData.Home2ndVelAxis0 * AxisZStroke / DegreeToPulse, MotionSettingData.Home3rdVelAxis0 * AxisZStroke / DegreeToPulse, MotionSettingData.Home4thVelAxis0 * AxisZStroke / DegreeToPulse, MotionSettingData.Home1stAccAxis0 * AxisZStroke / DegreeToPulse, MotionSettingData.Home2ndAccAxis0 * AxisZStroke / DegreeToPulse);
                            if (MotionSettingData.HomeSetOffsetNAxis0 > 40 && MotionSettingData.HomeSetOffsetPAxis0 > 40)
                            {
                                if (Convert.ToDouble(lblCommandPosAxis0.Text) < -45)
                                    AjinLibrary.AxmHomeSetMethod(0, 1, 4, 0, MotionSettingData.HomeClrTimeAxis0, MotionSettingData.HomeSetOffsetNAxis0 * AxisZStroke / DegreeToPulse);
                                else
                                    AjinLibrary.AxmHomeSetMethod(0, 0, 4, 0, MotionSettingData.HomeClrTimeAxis0, MotionSettingData.HomeSetOffsetPAxis0 * AxisZStroke / DegreeToPulse);
                            }
                            else if (MotionSettingData.HomeSetOffsetNAxis0 < -40 && MotionSettingData.HomeSetOffsetPAxis0 < -40)
                            {
                                if (Convert.ToDouble(lblCommandPosAxis0.Text) < 45)
                                    AjinLibrary.AxmHomeSetMethod(0, 1, 4, 0, MotionSettingData.HomeClrTimeAxis0, MotionSettingData.HomeSetOffsetNAxis0 * AxisZStroke / DegreeToPulse);
                                else
                                    AjinLibrary.AxmHomeSetMethod(0, 0, 4, 0, MotionSettingData.HomeClrTimeAxis0, MotionSettingData.HomeSetOffsetPAxis0 * AxisZStroke / DegreeToPulse);
                            }
                            else
                            {
                                if (Convert.ToDouble(lblCommandPosAxis0.Text) < 0)
                                    AjinLibrary.AxmHomeSetMethod(0, 1, 4, 0, MotionSettingData.HomeClrTimeAxis0, MotionSettingData.HomeSetOffsetNAxis0 * AxisZStroke / DegreeToPulse);
                                else
                                    AjinLibrary.AxmHomeSetMethod(0, 0, 4, 0, MotionSettingData.HomeClrTimeAxis0, MotionSettingData.HomeSetOffsetPAxis0 * AxisZStroke / DegreeToPulse);
                            }

                            duRetCode = AjinLibrary.AxmHomeSetStart(0);
                            AjinLibrary.AxmHomeGetResult(0, ref duHomeInfo);
                        }
                        else
                        {
                            DialogResult dr = ShowMessageCon(NewIRIS2CAM.Properties.Resources.P1, "Block의 현재위치를 0도 기준으로 방향 확인 후 구동해주세요.");
                            AjinLibrary.AxmHomeSetVel(0, MotionSettingData.Home1stVelAxis0 * AxisZStroke / DegreeToPulse, MotionSettingData.Home2ndVelAxis0 * AxisZStroke / DegreeToPulse, MotionSettingData.Home3rdVelAxis0 * AxisZStroke / DegreeToPulse, MotionSettingData.Home4thVelAxis0 * AxisZStroke / DegreeToPulse, MotionSettingData.Home1stAccAxis0 * AxisZStroke / DegreeToPulse, MotionSettingData.Home2ndAccAxis0 * AxisZStroke / DegreeToPulse);
                            if (dr == DialogResult.Yes)
                            {
                                AjinLibrary.AxmHomeSetMethod(0, 1, 4, 0, MotionSettingData.HomeClrTimeAxis0, MotionSettingData.HomeSetOffsetNAxis0 * AxisZStroke / DegreeToPulse);
                                duRetCode = AjinLibrary.AxmHomeSetStart(0);
                                AjinLibrary.AxmHomeGetResult(0, ref duHomeInfo);

                            }
                            else if (dr == DialogResult.No)
                            {
                                AjinLibrary.AxmHomeSetMethod(0, 0, 4, 0, MotionSettingData.HomeClrTimeAxis0, MotionSettingData.HomeSetOffsetNAxis0 * AxisZStroke / DegreeToPulse);
                                duRetCode = AjinLibrary.AxmHomeSetStart(0);
                                AjinLibrary.AxmHomeGetResult(0, ref duHomeInfo);

                            }

                        }



                        break;
                    case "btnMoveHomeAxis1":
                        int tmpint1 = (int)Math.Round(Convert.ToDouble(lblCommandPosAxis1.Text));
                        for (int i = 0; i < InspectionPosViewer.RowCount - 1; i++)
                        {
                            if (tmpint1 == (int)Math.Round(Convert.ToDouble(InspectionPosViewer[2, i].Value)))
                            {
                                isValueExist = true;

                                break;
                            }
                        }
                        if (isValueExist && !isFirstHomeSearchingAxis1)
                        {
                         
                            AjinLibrary.AxmHomeSetVel(1, MotionSettingData.Home1stVelAxis1 * AxisXStroke / DegreeToPulse, MotionSettingData.Home2ndVelAxis1 * AxisXStroke / DegreeToPulse, MotionSettingData.Home3rdVelAxis1 * AxisXStroke / DegreeToPulse, MotionSettingData.Home4thVelAxis1 * AxisXStroke / DegreeToPulse, MotionSettingData.Home1stAccAxis1 * AxisXStroke / DegreeToPulse, MotionSettingData.Home2ndAccAxis1 * AxisXStroke / DegreeToPulse);
                            if (MotionSettingData.HomeSetOffsetNAxis1 > 40 && MotionSettingData.HomeSetOffsetPAxis1 > 40)
                            {
                                if (Convert.ToDouble(lblCommandPosAxis1.Text) < -45)
                                    AjinLibrary.AxmHomeSetMethod(1, 1, 4, 0, MotionSettingData.HomeClrTimeAxis1, MotionSettingData.HomeSetOffsetNAxis1 * AxisXStroke / DegreeToPulse);
                                else
                                    AjinLibrary.AxmHomeSetMethod(1, 0, 4, 0, MotionSettingData.HomeClrTimeAxis1, MotionSettingData.HomeSetOffsetPAxis1 * AxisXStroke / DegreeToPulse);
                            }
                            else if (MotionSettingData.HomeSetOffsetNAxis1 < -40 && MotionSettingData.HomeSetOffsetPAxis1 < -40)
                            {
                                if (Convert.ToDouble(lblCommandPosAxis1.Text) < 45)
                                    AjinLibrary.AxmHomeSetMethod(1, 1, 4, 0, MotionSettingData.HomeClrTimeAxis1, MotionSettingData.HomeSetOffsetNAxis1 * AxisXStroke / DegreeToPulse);
                                else
                                    AjinLibrary.AxmHomeSetMethod(1, 0, 4, 0, MotionSettingData.HomeClrTimeAxis1, MotionSettingData.HomeSetOffsetPAxis1 * AxisXStroke / DegreeToPulse);
                            }
                            else
                            {
                                if (Convert.ToDouble(lblCommandPosAxis1.Text) < 0)
                                    AjinLibrary.AxmHomeSetMethod(1, 1, 4, 0, MotionSettingData.HomeClrTimeAxis1, MotionSettingData.HomeSetOffsetNAxis1 * AxisXStroke / DegreeToPulse);
                                else
                                    AjinLibrary.AxmHomeSetMethod(1, 0, 4, 0, MotionSettingData.HomeClrTimeAxis1, MotionSettingData.HomeSetOffsetPAxis1 * AxisXStroke / DegreeToPulse);

                            }
                            duRetCode = AjinLibrary.AxmHomeSetStart(1);
                            AjinLibrary.AxmHomeGetResult(1, ref duHomeInfo);
                        }
                        else
                        {
                            DialogResult dr = DialogResult.None;
                            dr = ShowMessageCon(NewIRIS2CAM.Properties.Resources.P2, "Socket의 현재위치를 0도 기준으로 방향 확인 후 구동해주세요");


                            AjinLibrary.AxmHomeSetVel(1, MotionSettingData.Home1stVelAxis1 * AxisXStroke / DegreeToPulse, MotionSettingData.Home2ndVelAxis1 * AxisXStroke / DegreeToPulse, MotionSettingData.Home3rdVelAxis1 * AxisXStroke / DegreeToPulse, MotionSettingData.Home4thVelAxis1 * AxisXStroke / DegreeToPulse, MotionSettingData.Home1stAccAxis1 * AxisXStroke / DegreeToPulse, MotionSettingData.Home2ndAccAxis1 * AxisXStroke / DegreeToPulse);
                            if (dr == DialogResult.Yes)
                            {
                                AjinLibrary.AxmHomeSetMethod(1, 1, 4, 0, MotionSettingData.HomeClrTimeAxis1, MotionSettingData.HomeSetOffsetNAxis1 * AxisXStroke / DegreeToPulse);
                                duRetCode = AjinLibrary.AxmHomeSetStart(1);
                                AjinLibrary.AxmHomeGetResult(1, ref duHomeInfo);

                            }
                            else if (dr == DialogResult.No)
                            {
                                AjinLibrary.AxmHomeSetMethod(1, 0, 4, 0, MotionSettingData.HomeClrTimeAxis1, MotionSettingData.HomeSetOffsetPAxis1 * AxisXStroke / DegreeToPulse);
                                duRetCode = AjinLibrary.AxmHomeSetStart(1);
                                AjinLibrary.AxmHomeGetResult(1, ref duHomeInfo);

                            }


                        }
                        break;
                    default:
                        break;
                }
                if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    switch (btn.Name)
                    {
                        case "btnMoveHomeAxis0":
                            isFirstHomeSearchingAxis0 = false;
                            break;
                        case "btnMoveHomeAxis1":
                            isFirstHomeSearchingAxis1 = false;
                            break;
                    }

                    ShowLog(((AXT_MOTION_HOME_RESULT)duHomeInfo).ToString());
                }


            }
        }

        private void btnMoveStopAxis0_Click(object sender, EventArgs e)
        {
            if (StaticVariables.isMotionBusyAxis0 || StaticVariables.isMotionBusyAxis1)
                buttonDisable(false);
            uint duRetCode = 0;
            uint duHomeInfo = 0;
            uint duHomeInfo1 = 0;
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "btnMoveStopAxis0":
                    AjinLibrary.AxmHomeGetResult(0, ref duHomeInfo);
                    duRetCode = AjinLibrary.AxmMoveSStop(0);
                    AjinLibrary.AxmHomeGetResult(0, ref duHomeInfo1);
                 
                    break;
                case "btnMoveStopAxis1":
                    AjinLibrary.AxmHomeGetResult(1, ref duHomeInfo);
                    duRetCode = AjinLibrary.AxmMoveSStop(1);
                    AjinLibrary.AxmHomeGetResult(1, ref duHomeInfo1);
                   
                    break;
                default:
                    break;

            }


            if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS && duHomeInfo == (uint)AXT_MOTION_HOME_RESULT.HOME_SEARCHING) // home stop er 메세지 출력.
                ShowLog(((AXT_MOTION_HOME_RESULT)duHomeInfo1).ToString());
          
            UserBreak = true;
        }

        private void btnMoveSetPos_Click(object sender, EventArgs e)
        {
            MoveSelectedPosition(lstPosition.SelectedIndex, true);
            CurrentInspCount = lstPosition.SelectedIndex;
        }

        private void btnPositionClearAxis0_Click(object sender, EventArgs e)
        {
            AjinLibrary.AxmStatusSetPosMatch(0, 0.0);
        }

        private void btnPositionClearAxis1_Click(object sender, EventArgs e)
        {
            AjinLibrary.AxmStatusSetPosMatch(1, 0.0);
        }

        private void clearlog_Click(object sender, EventArgs e)
        {
            MotionLog.Clear();
        }

        private void InspectionPosViewer_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = e.Control as TextBox;
            if(tb != null)
                tb.KeyPress += Tb_KeyPress;
        }

        private void Tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            PublicFunctions.TypingOnlyNumber(sender, e, true, true);
        }

        private void ParameterViewer_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = e.Control as TextBox;
            if (tb != null)
                tb.KeyPress += Tb_KeyPress;
        }

        private void btnEmgStop_Click(object sender, EventArgs e)
        {
            EMGStop();
        }
        public void EMGStop()
        {
            if (StaticVariables.isMotionBusyAxis0 || StaticVariables.isMotionBusyAxis1)
                buttonDisable(false);
            uint duRetCode = 0;
            duRetCode = AjinLibrary.AxmMoveEStop(0);
            duRetCode = AjinLibrary.AxmMoveEStop(1);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                ShowLog(((AXT_FUNC_RESULT)duRetCode).ToString());
            UserBreak = true;
          
        }

        private void btnAddInspPos_Click(object sender, EventArgs e)
        {
            AddInspPos();
        }

        private void btnMinusInspPos_Click(object sender, EventArgs e)
        {
            ReMovePos();
        }
    }
}
