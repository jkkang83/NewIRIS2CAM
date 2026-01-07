using Dln;
using MathNet.Numerics.Distributions;
using OpenCvSharp.Flann;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Collections;
using System.Net.Cache;
using System.Net.Sockets;
using System.Drawing;
using System.Web;
using OpenCvSharp.Extensions;
using MathNet.Numerics;
using System.Security.Policy;
using static DavinciIRISTester.XiaomiDll2;
using System.Security.Cryptography;


namespace M1Wide
{
    public delegate void FunctionPointer(int ch, string testItem);
    public class LogEvent : EventArgs
    {
        public static void SaveLog(int ch, string folderPath, string filename, string logMessage)
        {
            System.Threading.Tasks.Task thr = new System.Threading.Tasks.Task(delegate ()
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                while (true)
                {

                    try
                    {
                        using (StreamWriter sw = File.AppendText(folderPath + "\\" + filename + ".txt"))
                        {
                            sw.WriteLine(logMessage);
                            break;
                        }
                    }
                    catch { }
                    finally
                    {

                    }
                }
            });
            thr.Start();
        }
        public class Params
        {
            public int Ch { get; set; }
            public string Msg { get; set; }
        }

        public event EventHandler<Params> Evented = null;
        public virtual void AddLog(int ch, string msg)
        {
            Evented?.Invoke(this, new Params() { Ch = ch, Msg = msg });

        }
        public event EventHandler<Params> VEvented = null;

        public virtual void AddLog(string msg, bool isUse = false)
        {
            VEvented?.Invoke(this, new Params() { Msg = msg });
        }
    }
    public class ChartEvent : EventArgs
    {
        public class Params
        {
            public int Ch { get; set; }
            public string Name { get; set; }
        }

        public event EventHandler<Params> Evented = null;

        public virtual void AddChart(int ch, string name)
        {
            Evented?.Invoke(this, new Params() { Ch = ch, Name = name });
        }
    }
    public class ShowEvent : EventArgs
    {
        public class Params
        {
            public int Ch { get; set; }
            public string Key { get; set; }
        }

        public event EventHandler<Params> Evented = null;

        public virtual void Show(int ch, string key)
        {
            Evented?.Invoke(this, new Params() { Ch = ch, Key = key });
        }
    }
    public class ActItems
    {
        public string Name { get; set; }
        public FunctionPointer Func { get; set; }
        public bool IsMulti { get; set; }
        public bool isPowerReset { get; set; }
        public string Time { get; set; }
    }
    public class Process
    {
        public Camera Cam { get { return STATIC.Camera; } }
        public InspectionApi[] InspApi { get { return STATIC.InspectionApi; } }
        public LogEvent Log { get { return STATIC.LogEvent; } }
        public ShowEvent ShowEvent { get { return STATIC.ShowEvent; } }
        public ChartEvent Chart { get { return STATIC.ChartEvent; } }
        public Condition Condition { get { return STATIC.Rcp.Condition; } }
        public VisionSettingFile VisionFile { get { return STATIC.Rcp.VisionFile; } }
        public DecenterScale Dscale { get { return STATIC.Rcp.Dscale; } }

        public Spec Spec { get { return STATIC.Rcp.Spec; } }
        public DLN Dln { get { return STATIC.Dln; } }
        public DriverIC DrvIC { get { return STATIC.DrvIC; } }
 
        public Option Option { get { return STATIC.Rcp.Option; } }
        public Model Model { get { return STATIC.Rcp.Model; } }

        public ObservableCollection<ActItems> ItemList = new ObservableCollection<ActItems>();
        public class NVMHallParam
        {
            public int XHOffset;
            public int YHOffset;
            public int XHBias;
            public int YHBias;
            public int XHmin;
            public int XHmax;
            public int YHmin;
            public int YHmax;
            public int XHmid;
            public int YHmid;
            public int XDrv_Min;
            public int XDrv_Max;
            public int YDrv_Min;
            public int YDrv_Max;
            public int XNEPAmin;
            public int XNEPAmax;
            public int YNEPAmin;
            public int YNEPAmax;
            public void Clear()
            {
                XHOffset = 0;
                YHOffset = 0;
                XHBias = 0;
                YHBias = 0;
                XHmin = 0;
                XHmax = 0;
                YHmin = 0;
                YHmax = 0;
                XHmid = 0;
                YHmid = 0;
                XDrv_Min = 0;
                XDrv_Max = 0;
                YDrv_Min = 0;
                YDrv_Max = 0;
                XNEPAmin = 0;
                XNEPAmax = 0;
                YNEPAmin = 0;
                YNEPAmax = 0;
            }
        };
        public List<NVMHallParam> HallParam = new List<NVMHallParam>();

        public List<Task> RunTasks = new List<Task>();
        public int RunTaskId1 = 0;
        public int RunTaskId2 = 0;

        public bool m_bAllLEDOn = false;
        public bool IsVirtual = false;
        public bool SuddenStop = false;
        public int RepeatRun = 0;
        public int CurrentRun = 0;
        public int PortCnt = 1;
        public int ChannelCnt = 2;

        public List<bool> IsRun = new List<bool>();
     
        public List<string> errMsg = new List<string>();
        public List<bool> m_ChannelOn = new List<bool>();
        public List<string> m_StrIndex = new List<string>();

     
        public List<List<SettlingData>>[] TimeTest_Ch1 = new List<List<SettlingData>>[6];
        public List<List<SettlingData>>[] TimeTest_Ch2 = new List<List<SettlingData>>[6];
       
 
        public List<List<InspResult>> PosTest = new List<List<InspResult>>();

        public Process()
        {
            for (int i = 0; i < PortCnt; i++)
            {
                IsRun.Add(false);
               
                for (int j = 0; j < 6; j++)
                {
                    TimeTest_Ch1[j] = new List<List<SettlingData>>();
                    TimeTest_Ch1[j].Add(new List<SettlingData>());
                    TimeTest_Ch2[j] = new List<List<SettlingData>>();
                    TimeTest_Ch2[j].Add(new List<SettlingData>());

                }

                int ImgCnt = 0;
                if (Option.Step10Use)
                {
                    ImgCnt = 20;
                    //if (Model.ModelName == "SO1C81")
                    //{
                    //    if (STATIC.Rcp.Option.Code1500InspUse)
                    //        ImgCnt = 22;
                    //    else ImgCnt = 20;
                    //}
                    //else
                    //{
                    //    if (STATIC.Rcp.Option.Code1500InspUse)
                    //        ImgCnt = 20;
                    //    else ImgCnt = 18;
                    //}
                }
                else
                {
                    ImgCnt = 8;
                    //if (STATIC.Rcp.Option.Code1500InspUse)
                    //    ImgCnt = 10;
                    //else ImgCnt = 8;
                }


                for (int j = 0; j < ChannelCnt; j++)
                {
                    PosTest.Add(new List<InspResult>());
                    STATIC.ResMatOnProcess.Add(new List<Mat>());
                
                    for (int k = 0; k < ImgCnt; k++)
                    {
                        STATIC.ResMatOnProcess[j].Add(new Mat());
                    }
                }
            
          
            }
            for (int i = 0; i < PortCnt * ChannelCnt; i++)
            {
                errMsg.Add("");
                m_ChannelOn.Add(false);
                m_StrIndex.Add("");
            }
            ItemList.Add(new ActItems() { Name = "AK IC Init", Func = Act_AKICInit });
            ItemList.Add(new ActItems() { Name = "AK Load PID", Func = Act_LoadPID });
            ItemList.Add(new ActItems() { Name = "IRIS Hall Adjustment", Func = Act_IRISHallAdjustment, IsMulti = true, isPowerReset = true });
            ItemList.Add(new ActItems() { Name = "AK IC Init2", Func = Act_AKICInit });
            ItemList.Add(new ActItems() { Name = "Change Slave Addr", Func = ChangeSlaveAddr, IsMulti = true });
            ItemList.Add(new ActItems() { Name = "IRIS Postion Search", Func = Act_SearchPosition, IsMulti = true });
            ItemList.Add(new ActItems() { Name = "IRIS Postion Test", Func = Act_PostionTest, IsMulti = true });
            ItemList.Add(new ActItems() { Name = "Aging", Func = Aging, IsMulti = true });
            ItemList.Add(new ActItems() { Name = "IRIS Linearity Cal", Func = Act_LinearityCal, IsMulti = true });
            //ItemList.Add(new ActItems() { Name = "IRIS Hall Adjustment2", Func = Act_IRISHallAdjustment2, IsMulti = true });
            //ItemList.Add(new ActItems() { Name = "IRIS Hall Adjustment3", Func = Act_IRISHallAdjustment3, IsMulti = true });
            //ItemList.Add(new ActItems() { Name = "IRIS Hall Adjustment4", Func = Act_IRISHallAdjustment4, IsMulti = true });         
            //
            //ItemList.Add(new ActItems() { Name = "IRIS Linearity Cal_2", Func = Act_LinearityCal_2Dir, IsMulti = true });
            //ItemList.Add(new ActItems() { Name = "IRIS Linearity Cal_3", Func = Act_LinearityCal_2Dir_rev, IsMulti = true });
            //ItemList.Add(new ActItems() { Name = "Linearity Cal Check", Func = CheckLinearity, IsMulti = true });
            //
            ItemList.Add(new ActItems() { Name = "IRIS Data Check", Func = Act_DataCheck });
            ItemList.Add(new ActItems() { Name = "IRIS EPA", Func = Act_EPA });
            //ItemList.Add(new ActItems() { Name = "IRIS PhaseMargin", Func = Act_PhaseMargin, IsMulti = true });
            //ItemList.Add(new ActItems() { Name = "IRIS Settling Time", Func = Act_Settling });


        }
      
        public void LoadTestUnload(int port, bool isFirstPosition, int currentPos)
        {
            try
            {
                int ch = port * 2;
                STATIC.beforeNcal = new int[2];
                STATIC.beforePcal = new int[2];
                STATIC.afterNcal = new int[2];
                STATIC.afterPcal = new int[2];
                STATIC.PIDName = string.Empty;
                STATIC.LastStepArea = new double[2];
                STATIC.LastStep_1Area = new double[2];
                STATIC.LastStep_2Area = new double[2];
                STATIC.OCHallDiff = new int[2];
                STATIC.COHallDiff = new int[2];

                Thread.Sleep(100);

                if (Dln.IsSafeOn)
                {
                    Log.AddLog(ch, "Safe Sensor Detected. Push Start Button Again..");
                    IsRun[port] = false;
                    return;
                }
                Stopwatch stw = new Stopwatch();
                if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
                {
                    //stw.Start();
                    //if (!Dln.IsLoad && Dln.IsCoverUp)
                    //{
                    //    Dln.LoadSocket(true);

                    //    while (true)
                    //    {
                    //        if (Dln.IsLoad) break;
                    //        if (stw.ElapsedMilliseconds > 5000)
                    //        {
                    //            Log.AddLog(ch, "Socket Load Failed..");
                    //            IsRun[port] = false;
                    //            return;
                    //        }
                    //    }

                    //    Dln.CoverUpDown(false);
                    //}
                }
                else
                {
                    Dln.LoadSocket(true);
                    Thread.Sleep(1000);
                }

                DateTime now = DateTime.Now;
                if (now.DayOfYear != STATIC.LastDate.DayOfYear)
                {

                    if (!Directory.Exists(STATIC.YieldPath))
                        Directory.CreateDirectory(STATIC.YieldPath);
                    string yieldfile = STATIC.YieldPath + "Yield_" + STATIC.LastDate.ToString("yy-MM-dd") + ".txt";
                    StreamWriter sw = new StreamWriter(yieldfile);
                    sw.WriteLine(string.Format("{0} = {1}", "TotalTested", Spec.TotlaTested));
                    sw.WriteLine(string.Format("{0} = {1}", "TotalPassed", Spec.TotlaPassed));
                    sw.WriteLine(string.Format("{0} = {1}", "TotalFailed", Spec.TotlaFailed));
                    for (int i = 0; i < Spec.Param.Count; i++)
                    {
                        sw.WriteLine(string.Format("{0} {1} = {2}", Spec.Param[i][0], Spec.Param[i][1], Spec.Param[i][Spec.FAILED]));
                    }
                    sw.Close();
                    Spec.LastSampleNum = 0;
                    Spec.TotlaFailed = 0;
                    Spec.TotlaPassed = 0;
                    Spec.TotlaTested = 0;
                    for (int i = 0; i < Spec.Param.Count; i++)
                    {
                        Spec.Param[i][Spec.FAILED] = 0;
                    }
                    Spec.Save();
                    STATIC.LastDate = now;
                    sw = new StreamWriter(STATIC.LastDateFile);
                    sw.WriteLine(STATIC.LastDate.ToString("yy-MM-dd-HH-mm-ss"));
                    sw.Close();
                }
                if (isFirstPosition)
                {

                    for (int i = 0; i < ChannelCnt; i++)
                        PosTest[i].Clear();


                    if (Option.Step10Use)
                    {
                        for (int j = 0; j < ChannelCnt; j++)
                        {
                            for (int i = 0; i < 20; i++)
                            {

                                PosTest[j].Add(new InspResult());
                                PosTest[j][i].code = 0;


                            }

                        }
                        //if (Model.ModelName == "SO1C81")
                        //{

                        //    for (int j = 0; j < ChannelCnt; j++)
                        //    {
                        //        for (int i = 0; i < 20; i++)
                        //        {

                        //            PosTest[j].Add(new InspResult());
                        //            PosTest[j][i].code = 0;


                        //        }

                        //    }

                        //}
                        //else if (Model.ModelName == "SO1G73")
                        //{
                        //    for (int j = 0; j < ChannelCnt; j++)
                        //    {
                        //        for (int i = 0; i < 18; i++)
                        //        {

                        //            PosTest[j].Add(new InspResult());
                        //            PosTest[j][i].code = 0;


                        //        }

                        //    }
                        //}

                    }
                    else
                    {
                        for (int j = 0; j < ChannelCnt; j++)
                        {
                            for (int i = 0; i < 8; i++)
                            {

                                PosTest[j].Add(new InspResult());
                                PosTest[j][i].code = 0;


                            }

                        }

                    }
                    STATIC.ErrMsg = new List<string[]>() { new string[Condition.ItrCnt * RepeatRun], new string[Condition.ItrCnt * RepeatRun] };
                }
                STATIC.fManage.ClearChart();


                for (int i = 0; i < Condition.ItrCnt; i++)
                {

                    STATIC.fManage.CurrentItrCnt(i + 1);
                    STATIC.SaveImageItrCnt = i;
                    STATIC.fManage.ClearDecenterChart();
                    STATIC.fManage.ClearLog();
                    Thread.Sleep(100);
                    Process_Start(port, i, isFirstPosition, currentPos);
                    if (i < Condition.ItrCnt - 1)
                    {
                        if (Option.SaveLog)
                        {
                            for (int j = 0; j < ChannelCnt; j++)
                                STATIC.fManage.SaveLog(j);
                        }
                    }

                }
                if (currentPos >= RepeatRun)
                {
                    if (!Option.isOQC)
                    {
                        //List<byte[]> writeData = new List<byte[]>() { new byte[6], new byte[6] };
                        //byte[] SplNum_high = new byte[ChannelCnt];
                        //byte[] SplNum_low = new byte[ChannelCnt];
                        //byte Month = (byte)(STATIC.LastDate.Month);
                        //byte date = (byte)(STATIC.LastDate.Day);

                        //for (int i = 0; i < ChannelCnt; i++)
                        //{
                        //    if (!STATIC.isNonSpecError[i])
                        //    {
                        //        SplNum_high[i] = (byte)((Convert.ToInt32(m_StrIndex[i]) >> 8) & 0xFF);
                        //        SplNum_low[i] = (byte)((Convert.ToInt32(m_StrIndex[i])) & 0xFF);

                        //        writeData[i][0] = (byte)(Month << 4 | ((date >> 1) & 0x0F));
                        //        writeData[i][1] = (byte)((date << 7 | SplNum_high[i]) & 0xFF);
                        //        writeData[i][2] = SplNum_low[i];
                        //        writeData[i][3] = (byte)(Convert.ToInt32(STATIC.Rcp.Model.TesterNo) << 4 | (i + 1));
                        //        writeData[i][4] = (byte)(STATIC.WriteICDateTime[i].Hour);
                        //        writeData[i][5] = (byte)(STATIC.WriteICDateTime[i].Minute);
                        //        if (Condition.SortNo == 0)
                        //            writeData[i][5] = (byte)(STATIC.WriteICDateTime[i].Minute);
                        //        else writeData[i][5] = (byte)(Condition.SortNo);
                        //    }

                        //}

                        List<byte[]> SendArr = new List<byte[]>() { new byte[4], new byte[4] };
                        byte[] rbuf = new byte[1];

                        //if (Model.ModelName == "SO1G73")
                        //{
                        //    for (int i = 0; i < ChannelCnt; i++)
                        //    {
                        //        if (m_ChannelOn[i] && !STATIC.isNonSpecError[i])
                        //        {

                        //            if (Option.Step10Use)
                        //            {
                        //                int a = 0;
                        //                a = (int)Math.Round(STATIC.COScanLinearityDiff[i] * 10);
                        //                if (a < 0)
                        //                    a = 256 + a;
                        //                SendArr[i][0] = (byte)a;

                        //                a = (int)Math.Round(STATIC.COScanLinearityMax[i] * 10);
                        //                if (a < 0)
                        //                    a = 256 + a;
                        //                SendArr[i][1] = (byte)a;

                        //                a = (int)Math.Round(STATIC.OCScanLinearityDiff[i] * 10);
                        //                if (a < 0)
                        //                    a = 256 + a;
                        //                SendArr[i][2] = (byte)a;

                        //                a = (int)Math.Round(STATIC.OCScanLinearityMax[i] * 10);
                        //                if (a < 0)
                        //                    a = 256 + a;
                        //                SendArr[i][3] = (byte)a;
                        //                //SendArr[i][0] = (byte)(((PosTest[i][5].code << 4) >> 8) & 0xff);
                        //                //SendArr[i][1] = (byte)(((PosTest[i][5].code << 4) & 0xff) | (PosTest[i][8].code >> 8) & 0x0ff);
                        //                //SendArr[i][2] = (byte)(PosTest[i][8].code & 0xff);
                        //                //SendArr[i][3] = (byte)(((PosTest[i][12].code << 4) >> 8) & 0xff);
                        //                //SendArr[i][4] = (byte)(((PosTest[i][12].code << 4) & 0xff) | (PosTest[i][15].code >> 8) & 0x0ff);
                        //                //SendArr[i][5] = (byte)(PosTest[i][15].code & 0xff);
                        //                //SendArr[i][6] = (byte)(((PosTest[i][17].code << 4) >> 8) & 0xff);
                        //                //SendArr[i][7] = (byte)(((PosTest[i][17].code << 4) & 0xff) | (PosTest[i][2].code >> 8) & 0x0ff);
                        //                //SendArr[i][8] = (byte)(PosTest[i][2].code & 0xff);


                        //            }
                        //            else
                        //            {
                        //                int a = 0;
                        //                a = (int)Math.Round(STATIC.COScanLinearityDiff[i] * 10);
                        //                if (a < 0)
                        //                    a = 256 + a;
                        //                SendArr[i][0] = (byte)a;

                        //                a = (int)Math.Round(STATIC.COScanLinearityMax[i] * 10);
                        //                if (a < 0)
                        //                    a = 256 + a;
                        //                SendArr[i][1] = (byte)a;

                        //                a = (int)Math.Round(STATIC.OCScanLinearityDiff[i] * 10);
                        //                if (a < 0)
                        //                    a = 256 + a;
                        //                SendArr[i][2] = (byte)a;

                        //                a = (int)Math.Round(STATIC.OCScanLinearityMax[i] * 10);
                        //                if (a < 0)
                        //                    a = 256 + a;
                        //                SendArr[i][3] = (byte)a;

                        //                //SendArr[i][0] = (byte)(((PosTest[i][2].code << 4) >> 8) & 0xff);
                        //                //SendArr[i][1] = (byte)(((PosTest[i][2].code << 4) & 0xff) | (PosTest[i][3].code >> 8) & 0x0ff);
                        //                //SendArr[i][2] = (byte)(PosTest[i][3].code & 0xff);
                        //                //SendArr[i][3] = (byte)(((PosTest[i][5].code << 4) >> 8) & 0xff);
                        //                //SendArr[i][4] = (byte)(((PosTest[i][5].code << 4) & 0xff) | (PosTest[i][6].code >> 8) & 0x0ff);
                        //                //SendArr[i][5] = (byte)(PosTest[i][6].code & 0xff);
                        //                //SendArr[i][6] = (byte)(((PosTest[i][7].code << 4) >> 8) & 0xff);
                        //                //SendArr[i][7] = (byte)(((PosTest[i][7].code << 4) & 0xff) | (PosTest[i][1].code >> 8) & 0x0ff);
                        //                //SendArr[i][8] = (byte)(PosTest[i][1].code & 0xff);
                        //            }


                        //        }
                        //    }
                        //}
                        List<byte[]> writeData = new List<byte[]>() { new byte[4], new byte[4] };
                        byte[] SplNum_high = new byte[ChannelCnt];
                        byte[] SplNum_low = new byte[ChannelCnt];
                        byte Month = (byte)(STATIC.LastDate.Month);
                        byte date = (byte)(STATIC.LastDate.Day);

                        for (int i = 0; i < ChannelCnt; i++)
                        {
                            if (!STATIC.isNonSpecError[i])
                            {
                                Dln.WriteArray(i, DrvIC.AkSlave[i], 0xAE, 1, new byte[1] { 0x3B });

                                string errmsg = "";
                                string PassType = "PASS";
                                for (int j = 0; j < Condition.ItrCnt * RepeatRun; j++)
                                {
                                    if (STATIC.ErrMsg[i][j] != null && !STATIC.ErrMsg[i][j].Contains("PASS") && errmsg == "")
                                        errmsg = STATIC.ErrMsg[i][j];
                                    //if (STATIC.ErrMsg[i][j] != null && STATIC.ErrMsg[i][j].Contains("A PASS") && PassType != "B PASS" && PassType != "C PASS")
                                    //    PassType = "A PASS";
                                    //if (STATIC.ErrMsg[i][j] != null && STATIC.ErrMsg[i][j].Contains("B PASS") && PassType != "C PASS")
                                    //    PassType = "B PASS";
                                    //if (STATIC.ErrMsg[i][j] != null && STATIC.ErrMsg[i][j].Contains("C PASS"))
                                    //    PassType = "C PASS";
                                }
                                byte[] wData = new byte[12];
                                wData[0] = (byte)(PosTest[i][0].code >> 4); 
                                wData[1] = (byte)(((PosTest[i][0].code & 0x0F) << 4) | ((PosTest[i][1].code >> 8) & 0x0F));
                                wData[2] = (byte)(PosTest[i][1].code);
                                wData[3] = (byte)(PosTest[i][2].code >> 4);
                                wData[4] = (byte)(((PosTest[i][2].code & 0x0F) << 4) | ((PosTest[i][3].code >> 8) & 0x0F));
                                wData[5] = (byte)(PosTest[i][3].code);

                                wData[6] = (byte)(PosTest[i][7].code >> 4);
                                wData[7] = (byte)(((PosTest[i][7].code & 0x0F) << 4) | ((PosTest[i][6].code >> 8) & 0x0F));
                                wData[8] = (byte)(PosTest[i][6].code);
                                wData[9] = (byte)(PosTest[i][5].code >> 4);
                                wData[10] = (byte)(((PosTest[i][5].code & 0x0F) << 4) | ((PosTest[i][4].code >> 8) & 0x0F));
                                wData[11] = (byte)(PosTest[i][4].code);

                                for (int j = 0; j < wData.Length; j++)
                                {
                                    Dln.WriteArray(i, DrvIC.AkSlave[i], 0xEC + j, 1, new byte[] { wData[j] });
                                    Thread.Sleep(10);
                                }
                                if (!STATIC.isNonSpecError[i])
                                {
                                    SplNum_high[i] = (byte)((Convert.ToInt32(m_StrIndex[i]) >> 8) & 0xFF);
                                    SplNum_low[i] = (byte)((Convert.ToInt32(m_StrIndex[i])) & 0xFF);

                                    writeData[i][0] = (byte)(Month << 4 | ((date >> 1) & 0x0F));
                                    writeData[i][1] = (byte)((date << 7 | SplNum_high[i]) & 0xFF);
                                    writeData[i][2] = SplNum_low[i];
                                    writeData[i][3] = (byte)(Convert.ToInt32(STATIC.Rcp.Model.TesterNo) << 4 | (i + 1));
                                   
                                }
                                for (int j = 0; j < writeData[i].Length; j++)
                                {
                                    Dln.WriteArray(i, DrvIC.AkSlave[i], 0xFA + j, 1, new byte[] { writeData[i][j] });
                                    Thread.Sleep(10);

                                }
                                if(errmsg != "")
                                {
                                    DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xFF, 1, new byte[1] { 0x08 });
                                    Thread.Sleep(10);
                                }
                                else
                                {
                                    DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xFF, 1, new byte[1] { 0x02 });
                                    Thread.Sleep(10);
                                }




                                //if (Option.PassFailSaveMode)
                                //{
                                //    if (errmsg != "")
                                //    {
                                //        DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xF0, 1, new byte[1] { 0x08 });
                                //        Thread.Sleep(10);
                                //        DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xEF, 1, new byte[1] { 0x09 });
                                //    }

                                //    else
                                //    {
                                //        DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xF0, 1, new byte[1] { 0x02 });
                                //        Thread.Sleep(10);
                                //        if (PassType == "A PASS") DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xEF, 1, new byte[1] { 0x02 });
                                //        else if (PassType == "B PASS") DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xEF, 1, new byte[1] { 0x03 });
                                //        else if (PassType == "C PASS") DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xEF, 1, new byte[1] { 0x04 });
                                //        else DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xEF, 1, new byte[1] { 0x01 });
                                //    }

                                //}
                                //else
                                //{
                                //    if (errmsg != "")
                                //    {
                                //        DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xF0, 1, new byte[1] { 0x09 });
                                //        Thread.Sleep(10);
                                //        DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xEF, 1, new byte[1] { 0x09 });

                                //    }

                                //    else
                                //    {
                                //        DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xF0, 1, new byte[1] { 0x01 });
                                //        Thread.Sleep(10);
                                //        if (PassType == "A PASS") DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xEF, 1, new byte[1] { 0x02 });
                                //        else if (PassType == "B PASS") DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xEF, 1, new byte[1] { 0x03 });
                                //        else if (PassType == "C PASS") DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xEF, 1, new byte[1] { 0x04 });
                                //        else DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xEF, 1, new byte[1] { 0x01 });
                                //    }

                                //}

                                //Thread.Sleep(10);
                                //if (Model.ModelName == "SO1C81")
                                //{
                                //    DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xF1, 1, new byte[1] { (byte)STATIC.OC_F20_CCircleAccu[i] }); Thread.Sleep(10);
                                //    DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xF2, 1, new byte[1] { (byte)STATIC.CO_F20_CCircleAccu[i] }); Thread.Sleep(10);
                                //    DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xF3, 1, new byte[1] { (byte)STATIC.OC_F40_CCircleAccu[i] }); Thread.Sleep(10);
                                //    DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xF4, 1, new byte[1] { (byte)STATIC.CO_F40_CCircleAccu[i] }); Thread.Sleep(10);
                                //    DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xF5, 1, new byte[1] { (byte)STATIC.OC_F20_CDecenter[i] }); Thread.Sleep(10);
                                //    DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xF6, 1, new byte[1] { (byte)STATIC.CO_F20_CDecenter[i] }); Thread.Sleep(10);
                                //}
                                //else
                                //{
                                //    for (int j = 0; j < SendArr[i].Length; j++)
                                //    {
                                //        DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xF1 + j, 1, new byte[1] { SendArr[i][j] });
                                //        Thread.Sleep(10);
                                //    }
                                //}


                                //for (int j = 0; j < writeData[i].Length; j++)
                                //{
                                //    DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xFA + j, 1, new byte[1] { writeData[i][j] });
                                //    Thread.Sleep(10);

                                //}

                                AK7316_memory_upadate(i, 1);
                                AK7316_memory_upadate(i, 2);
                                AK7316_memory_upadate(i, 3);
                                AK7316_memory_upadate(i, 4);
                                AK7316_memory_upadate(i, 5);
                                DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0xAE, 1, new byte[1] { 0x00 });
                                Thread.Sleep(10);
                            }

                        }
                       
                        string tmpstr = string.Empty;
                        for (int i = 0; i < ChannelCnt; i++)
                        {
                            if (!STATIC.isNonSpecError[i])
                            {
                                tmpstr = string.Empty;
                                byte[] bBuffer = new byte[1];
                                for (int j = 0; j < 256; j++)
                                {
                                    DrvIC.Dln.ReadArray(i, DrvIC.AkSlave[i], j, 1, bBuffer);
                                    tmpstr += bBuffer[0].ToString("X2");
                                    if (j % 16 == 15)
                                    {
                                        Log.AddLog(i, string.Format("0x{0} ~ {1} : {2}", (j - 15).ToString("X2"), (j).ToString("X2"), tmpstr));
                                        tmpstr = string.Empty;
                                    }


                                }
                            }


                        }
                    }
                    for (int i = 0; i < ChannelCnt; i++)
                    {
                        if (m_ChannelOn[i] && !STATIC.isNonSpecError[i])
                        {
                            DrvIC.Dln.WriteArray(i, DrvIC.AkSlave[i], 0x02, 1, new byte[1] { 0x00 });
                        }
                    }

                    if (Option.ShowResultToImage/* && Option.ActroDllUse*/)
                    {
                        int[] showIndex = new int[1];
                        //if (Option.Code1500InspUse)
                        //{
                           
                        //    if (STATIC.Rcp.Option.Step10Use)
                        //    {
                        //        if (STATIC.Rcp.Model.ModelName == "SO1C81")
                        //        {
                        //            showIndex = new int[20] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 ,19, 21 };
                        //        }
                        //        else if (STATIC.Rcp.Model.ModelName == "SO1G73")
                        //            showIndex = new int[18] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 19 };
                        //    }
                        //    else
                        //    {
                        //        showIndex = new int[8] { 0, 2, 3, 4, 5, 6, 7, 9 };
                        //    }
                        //}
                      
                        for (int i = 0; i < ChannelCnt; i++)
                        {
                            if (m_ChannelOn[i] && !STATIC.isNonSpecError[i])
                            {
                                for (int j = 0; j < PosTest[i].Count; j++)
                                    PosTest[i][j].img = new Bitmap(STATIC.ResMatOnProcess[i][j].ToBitmap());

                                //if (Option.Code1500InspUse)
                                //{
                                //    for (int j = 0; j < PosTest[i].Count; j++)
                                //    {
                                //        PosTest[i][j].img = new Bitmap(STATIC.ResMatOnProcess[i][showIndex[j]].ToBitmap());
                                //    }
                                //}
                                //else
                                //{
                                //    for (int j = 0; j < PosTest[i].Count; j++)
                                //        PosTest[i][j].img = new Bitmap(STATIC.ResMatOnProcess[i][j].ToBitmap());
                                //}
                                ShowEvent.Show(i, "Image");
                            }

                        }
                    }
                }


                if (Option.SaveLog)
                {
                    for (int i = 0; i < ChannelCnt; i++)
                    {

                        STATIC.fManage.SaveLog(i);
                    }
                }
                Dln.powerOnOff(false);
                if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
                {
                    //if (Dln.IsLoad && !Dln.IsCoverUp)
                    //{
                    //    Dln.CoverUpDown(true);
                    //    stw.Restart(); 
                    //    while (true)
                    //    {
                    //        if (Dln.IsCoverUp) break;
                    //        if (stw.ElapsedMilliseconds > 5000)
                    //        {
                    //            Log.AddLog(ch, "Socket Cover Open Failed..");
                    //            IsRun[port] = false;
                    //            return;
                    //        }
                    //    }

                    //    Dln.LoadSocket(false);
                    //    stw.Restart();
                    //    while (true)
                    //    {
                    //        if (!Dln.IsLoad) break;
                    //        if (stw.ElapsedMilliseconds > 5000)
                    //        {
                    //            Log.AddLog(ch, "Socket UnLoad Failed..");
                    //            IsRun[port] = false;
                    //            return;
                    //        }
                    //    }
                    //}

                }
                else
                {
                    Dln.LoadSocket(false);
                }
                IsRun[port] = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
          
        }
        private void Process_Function(int port, string testItem)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int index = 0;
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (testItem == ItemList[i].Name)
                {
                    index = i; break;
                }
            }

            int ch = port * 2;


            if (!m_ChannelOn[ch] && !m_ChannelOn[ch + 1])
                return;

            for (int k = ch; k < ch + ChannelCnt; k++)
            {
                if (m_ChannelOn[k])
                {
                    m_StrIndex[k] = (Spec.LastSampleNum + k + 1).ToString();
                    Log.AddLog(k, m_StrIndex[k] + ">> " + testItem + " Start");
                }
            }

            try
            {
                Task Func1 = null, Func2 = null;

                if (!ItemList[index].IsMulti)
                {
                    Func1 = new Task(() => ItemList[index].Func(port, testItem));
                    Func1.Start();
                    if (Func1 != null) Task.WaitAll(Func1);
                }
                else
                {
                    if (m_ChannelOn[ch])
                    {
                        Func1 = new Task(() => ItemList[index].Func(ch, testItem));
                        Func1.Start();
                        Log.AddLog(ch, testItem + " Start");
                    }
                    if (ChannelCnt > 1)
                    {
                        if (m_ChannelOn[ch + 1])
                        {
                            Func2 = new Task(() => ItemList[index].Func(ch + 1, testItem));
                            Func2.Start();
                            Log.AddLog(ch + 1, testItem + " Start");
                        }
                    }

                    if (Func1 != null && Func2 != null) Task.WaitAll(Func1, Func2);
                    else
                    {
                        if (Func1 != null) Task.WaitAll(Func1);
                        if (Func2 != null) Task.WaitAll(Func2);
                    }
                }
                if (ItemList[index].isPowerReset)
                {
                    Dln.powerOnOff(false);
                    Thread.Sleep(100);
                    Dln.powerOnOff(true);
                    Thread.Sleep(200);
                }
            }
            catch (Exception e)
            {
                for (int k = ch; k < ch + ChannelCnt; k++)
                {
                    Log.AddLog(k, testItem + " Exception : " + e.ToString() + " ch : " + k.ToString());
                    errMsg[k] = testItem + " Error";
                    m_ChannelOn[k] = false;
                }
            }

            for (int k = ch; k < ch + ChannelCnt; k++)
            {
                if (m_ChannelOn[k])
                {
                    double ellipse = (double)sw.ElapsedMilliseconds / 1000;
                    Log.AddLog(k, string.Format("{0}\t{1:0.000} sec", testItem, ellipse));
                    ItemList[index].Time = ellipse.ToString("F3");
                }
            }
            sw.Stop();
        }
        private void Process_Start(int port, int itrcnt, bool isFirstPosition, int CurrentPos)
        {
            Dln.powerOnOff(true);
            Thread.Sleep(200);
            LEDs_All_On(true);
            int ch = port * 2;
            try
            {

                if (itrcnt != 0)
                {
                    if (ChannelCnt == 1)
                    {
                        if (!m_ChannelOn[ch] || STATIC.isNonSpecError[ch])
                            return;
                    }
                    else
                    {
                        if ((!m_ChannelOn[ch] && !m_ChannelOn[ch + 1]) || (STATIC.isNonSpecError[ch] && STATIC.isNonSpecError[ch + 1]))
                            return;
                    }
                }
            

                int count = Condition.ToDoList.Count;
                if (count == 0)
                {
                    for (int i = ch; i < ch + ChannelCnt; i++)
                        errMsg[i] = "Test Item is Empty"; 
                    return;
                }
                for (int k = ch; k < ch + ChannelCnt; k++)
                {
                    if (itrcnt == 0)
                    {
                        m_ChannelOn[k] = true;
                        errMsg[k] = "";
                        Spec.PassFails[k].FirstFailIndex = 0;
                    }

                    else
                    {
                        if (m_ChannelOn[k])
                        {
                            errMsg[k] = "";
                            Spec.PassFails[k].FirstFailIndex = 0;
                        }

                    }

                }
            

                Stopwatch sw = new Stopwatch();
                sw.Start();

                bool loopContinue = true;
                string tmpS = string.Empty;
                int todoCnt = 0;
             
                SuddenStop = false;
                if (itrcnt == 0 && isFirstPosition)
                {
                    while (todoCnt < count)
                    {
                        string testItem = Condition.ToDoList[todoCnt];
                        tmpS = testItem.Replace(" ", "_");
                        if (STATIC.isTmpLog)
                            STATIC.CommonLog += string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"), "O", "-", m_StrIndex[ch], "Run", "INSPECTION_UNIT:" + tmpS, "Start", "");
                        Process_Function(port, testItem);

                        for (int k = ch; k < ch + ChannelCnt; k++)
                        {
                            if (errMsg[k] != "")
                            {
                                // loopContinue = false;
                                Log.AddLog(k, errMsg[k]);
                            }
                        }
                        if (errMsg[0] != "" && errMsg[1] != "")
                            loopContinue = false;

                        if (SuddenStop)
                        {
                            loopContinue = false;
                            errMsg[ch] = errMsg[ch + 1] = "SuddenStop !";
                            Log.AddLog(ch, errMsg[ch]);
                            Log.AddLog(ch + 1, errMsg[ch + 1]);
                        }

                        if (STATIC.isTmpLog)
                            STATIC.CommonLog += string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"), "O", "-", m_StrIndex[ch], "Run", "INSPECTION_UNIT:" + tmpS, "End", "");

                        if (!loopContinue) break;
                        else todoCnt++;
                        Thread.Sleep(100);
                    }
                }
                else
                {


                    string testItem = Condition.ToDoList[count - 1];
                    tmpS = testItem.Replace(" ", "_");
                    if (STATIC.isTmpLog)
                        STATIC.CommonLog += string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"), "O", "-", m_StrIndex[ch], "Run", "INSPECTION_UNIT:" + tmpS, "Start", "");
                    Process_Function(port, testItem);

                    for (int k = ch; k < ch + ChannelCnt; k++)
                    {
                        if (errMsg[k] != "")
                        {
                            // loopContinue = false;
                            Log.AddLog(k, errMsg[k]);
                        }
                    }
                    if (errMsg[0] != "" && errMsg[1] != "")
                        loopContinue = false;
                    if (SuddenStop)
                    {
                        loopContinue = false;
                        errMsg[ch] = errMsg[ch + 1] = "SuddenStop !";
                        Log.AddLog(ch, errMsg[ch]);
                        Log.AddLog(ch + 1, errMsg[ch + 1]);
                    }
                    if (STATIC.isTmpLog)
                        STATIC.CommonLog += string.Format("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"), "O", "-", m_StrIndex[ch], "Run", "INSPECTION_UNIT:" + tmpS, "End", "");
                    Thread.Sleep(100);
                }


                double ellipse = (double)sw.ElapsedMilliseconds / 1000;
                sw.Stop();
                if((CurrentPos >= RepeatRun) && itrcnt == Condition.ItrCnt - 1)
                {
                    for (int k = ch; k < ch + ChannelCnt; k++)
                    {
                        if (!errMsg[k].Contains("Socket Empty")) Spec.LastSampleNum++;
                        Log.AddLog(k, string.Format("Total Test Time\t{0:0.000} sec", ellipse));
                        Spec.PassFails[k].TotalTime = ellipse.ToString("F3");

                    }

                }
                for (int i = 0; i < ChannelCnt; i++)
                {
                    WriteResult(i, CurrentPos, itrcnt);
                }

                for (int i = 0; i < ChannelCnt; i++)
                {
                    STATIC.ErrMsg[i][itrcnt + (CurrentPos - 1) * Condition.ItrCnt] = errMsg[i];
                }
                bool[] splPassed = new bool[2] { false, false };


                if (itrcnt == Condition.ItrCnt - 1 && CurrentPos >= RepeatRun)
                {
                    for (int i = 0; i < ChannelCnt; i++)
                    {
                        if (STATIC.isTmpLog)
                        {
                            string s = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff");
                            STATIC.InspLog += string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}\r\n", s, m_StrIndex[ch],
                                    PosTest[ch][0].CArea.ToString("F3"), PosTest[ch][0].CDecenterR.ToString("F3"), PosTest[ch][0].current.ToString("F3"), PosTest[ch][0].code - PosTest[ch][0].IrisHall,
                                    PosTest[ch][1].CArea.ToString("F3"), PosTest[ch][1].CDecenterR.ToString("F3"), PosTest[ch][1].current.ToString("F3"), PosTest[ch][1].code - PosTest[ch][1].IrisHall,
                                    PosTest[ch][2].CArea.ToString("F3"), PosTest[ch][2].CDecenterR.ToString("F3"), PosTest[ch][2].current.ToString("F3"), PosTest[ch][2].code - PosTest[ch][2].IrisHall,
                                    PosTest[ch][3].CArea.ToString("F3"), PosTest[ch][3].CDecenterR.ToString("F3"), PosTest[ch][3].current.ToString("F3"), PosTest[ch][3].code - PosTest[ch][3].IrisHall);

                            STATIC.InspLog += string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}\r\n", s, m_StrIndex[ch],
                                   PosTest[ch][7].CArea.ToString("F3"), PosTest[ch][7].CDecenterR.ToString("F3"), PosTest[ch][7].current.ToString("F3"), PosTest[ch][7].code - PosTest[ch][7].IrisHall,
                                   PosTest[ch][6].CArea.ToString("F3"), PosTest[ch][6].CDecenterR.ToString("F3"), PosTest[ch][6].current.ToString("F3"), PosTest[ch][6].code - PosTest[ch][6].IrisHall,
                                   PosTest[ch][5].CArea.ToString("F3"), PosTest[ch][5].CDecenterR.ToString("F3"), PosTest[ch][5].current.ToString("F3"), PosTest[ch][5].code - PosTest[ch][5].IrisHall,
                                   PosTest[ch][4].CArea.ToString("F3"), PosTest[ch][4].CDecenterR.ToString("F3"), PosTest[ch][4].current.ToString("F3"), PosTest[ch][4].code - PosTest[ch][4].IrisHall);
                        }



                        string passStr = "PASS";
                        string errmsg = "";
                        for (int j = 0; j < Condition.ItrCnt * RepeatRun; j++)
                        {
                            if (STATIC.ErrMsg[i][j] != null && !STATIC.ErrMsg[i][j].Contains("PASS") && errmsg == "")
                                errmsg = STATIC.ErrMsg[i][j];
                            //if (STATIC.ErrMsg[i][j] != null && STATIC.ErrMsg[i][j].Contains("A PASS") && passStr != "B PASS" && passStr != "C PASS")
                            //    passStr = "A PASS";
                            //if (STATIC.ErrMsg[i][j] != null && STATIC.ErrMsg[i][j].Contains("B PASS") && passStr != "C PASS")
                            //    passStr = "B PASS";
                            //if (STATIC.ErrMsg[i][j] != null && STATIC.ErrMsg[i][j].Contains("C PASS"))
                            //    passStr = "C PASS";
                        }
                        if (errmsg == "")
                        {
                          
                            splPassed[i] = true;
                        }

                        WriteResult_Area(i, splPassed[i], passStr);

                    }
                

                }
                ShowEvent.Show(0, string.Format("Yield"));
            
                //Thread.Sleep(200);
                return;

            }
            catch (Exception e)
            {
                for (int k = ch; k < ch + ChannelCnt; k++)
                {
                    Log.AddLog(k, "Process_Start Exception : " + e.ToString() + " ch : " + k.ToString());
                }
            }
        }




        public void DualInnerLed(bool isOn)
        {
            if(isOn)
            {
                Drive_LEDs(0, VisionFile.TopInnerLED_L);
                Drive_LEDs(2, VisionFile.TopInnerLED_R);
            }
            else
            {
                Drive_LEDs(0, 0);
                Drive_LEDs(2, 0);
            }
        
         
        }

        public void DualOuterLed(bool isOn)
        {
            if(isOn)
            {
                if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
                    Drive_LEDs(1, VisionFile.TopOuterLED_L);// Drive_LEDs_BTM_Posture(0, VisionFile.BtmLED_L);
              
            }
            else
            {
                if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
                    Drive_LEDs(1, 0);// Drive_LEDs_BTM_Posture(0, VisionFile.BtmLED_L);
              
            }

           


        }
        public void BtmLed(bool isOn)
        {
            if(isOn)
            {
                if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
                    Drive_LEDs_BTM_Posture(0, VisionFile.BtmLED_L);
                else
                {
                    Drive_LEDs(1, VisionFile.BtmLED_L);
                    Drive_LEDs(3, VisionFile.BtmLED_R);

                }
            }
            else
            {
                if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
                    Drive_LEDs_BTM_Posture(0, 0);
                else
                {
                    Drive_LEDs(1, 0);
                    Drive_LEDs(3, 0);

                }

            }
        }

        //public void LEDByChannle(int ch, bool isOn)
        //{
        //    if(isOn)
        //    {
        //        if (ch == 0)
        //        {
        //            Drive_LEDs(0, VisionFile.TopLED_L);
        //            if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
        //                Drive_LEDs_BTM_Posture(0, VisionFile.BtmLED_L);
        //            else
        //                Drive_LEDs(1, VisionFile.BtmLED_L);
        //        }
        //        else
        //        {
                    
        //            Drive_LEDs(2, VisionFile.TopLED_R);
        //            if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
        //                Drive_LEDs_BTM_Posture(0, VisionFile.BtmLED_L);
        //            else
        //                Drive_LEDs(3, VisionFile.BtmLED_R);
        //        }
        //    }
        //    else
        //    {
        //        if (ch == 0)
        //        {
        //            Drive_LEDs(0, 0.0);
                   
        //            if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
        //                Drive_LEDs_BTM_Posture(0, 0.0);
        //            else
        //                Drive_LEDs(1, 0.0);
        //        }
        //        else
        //        {
                   
        //            Drive_LEDs(2, 0.0);
        //            if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
        //                Drive_LEDs_BTM_Posture(0, 0.0);
        //            else
        //                Drive_LEDs(3, 0.0);
        //        }
        //    }
        //}
        public void LEDs_All_On(bool isOn)
        {
            if (m_bAllLEDOn = isOn)
            {

                Drive_LEDs(0, VisionFile.TopInnerLED_L);
                Drive_LEDs(2, VisionFile.TopInnerLED_R);
                if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
                    Drive_LEDs_BTM_Posture(0, VisionFile.BtmLED_L);
                else
                {
                    Drive_LEDs(1, VisionFile.BtmLED_L);
                    Drive_LEDs(3, VisionFile.BtmLED_R);

                }
            }
            else
            {
                Drive_LEDs(0, 0.0);
                Drive_LEDs(2, 0.0);
                if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
                    Drive_LEDs_BTM_Posture(0, 0.0);
                else
                {
                    Drive_LEDs(1, 0.0);
                    Drive_LEDs(3, 0.0);
                }
            }
        }
        public void TOPLED_OnOff(bool isOn)
        {
            if (isOn)
            {
                Dln.TopLED_OnOff(true);
            }
            else
            {
                Dln.TopLED_OnOff(false);
            }
        }
        public void Drive_LEDs(int ch, double volt)
        {
            Dln.SetLEDpower(ch, (int)(volt * 1240));
        }
        public void Drive_LEDs_BTM_Posture(int ch, double volt)
        {
            Dln.WriteCurrent(ch, (int)(volt * 1240));
        }

        void Act_DataCheck(int port, string testItem)
        {
            int ch = port * 2;
           
            Thread.Sleep(150);
            for (int k = ch; k < ch + ChannelCnt; k++)
            {
                if (m_ChannelOn[k])
                {
                    if (!DrvIC.IRIS_Adjustment_DataCheck(k))
                    {

                        errMsg[k] = testItem + " Error";
                        m_ChannelOn[k] = false;
                        STATIC.isNonSpecError[k] = true;
                    }
                }

            }
        }

        private void Act_AKICInit(int port, string testItem)
        {
            int ch = port * 2;
          
          
            for (int k = ch; k < ch + ChannelCnt; k++)
            {
                if (m_ChannelOn[k])
                {
                    if (!DrvIC.IRIS_IC_Init(k))
                    {

                        if(!DrvIC.IRIS_IC_Init(k))
                        {
                            errMsg[k] = testItem + " Error";
                            m_ChannelOn[k] = false;
                            STATIC.isNonSpecError[k] = true;
                        }

                        
                    }
                }
                if (m_ChannelOn[k] && Option.FlagCheckUse)
                {
                   
                    byte[] rBuf = new byte[1];
                    Dln.ReadArray(k, DrvIC.AkSlave[k], 0xEE, 1, rBuf);
                    Log.AddLog(k, string.Format("Flag Check  0xEE = {0}", rBuf[0]));
                    if (rBuf[0] != 0x01)
                    {
                        errMsg[k] = "Flag Check Error";
                        m_ChannelOn[k] = false;
                        STATIC.isNonSpecError[k] = true;
                    }
                }


            }

        }
        private void Act_RohmICInit(int port, string testItem)
        {
            //int ch = port * 2;
            //for (int j = 0; j < 5; j++)
            //{
            //    Dln.DrvicReset();
            //    for (int k = ch; k < ch + ChannelCnt; k++) Log.AddLog(ch, string.Format("Drv - IC Reset OK."));
            //}
            //Thread.Sleep(150);
            //for (int k = ch; k < ch + ChannelCnt; k++)
            //    if(!DrvIC.IC_Init(k))
            //    {
            //        errMsg[k] = testItem + " Error";
            //        m_ChannelOn[k] = false;
            //    }
        }
        private void Act_AFHallAdjustment(int ch, string testItem)
        {
            if (!DrvIC.AF_Adjustment(ch))
            {
                errMsg[ch] = testItem + " Error";
                m_ChannelOn[ch] = false;
            }
        }
        private void Act_OISHallAdjustment(int ch, string testItem)
        {
            if (!DrvIC.OIS_Adjustment(ch))
            {
                errMsg[ch] = testItem + " Error";
                m_ChannelOn[ch] = false;
            }
        }

        void Act_LoadPID(int port, string testItem)
        {
            int ch = port * 2;
            if (m_ChannelOn[ch] || m_ChannelOn[ch + 1])
            {
                if(m_ChannelOn[ch])
                    Log.AddLog(ch, string.Format(STATIC.Rcp.Current.PIDName));
                if (m_ChannelOn[ch + 1])
                    Log.AddLog(ch + 1, string.Format(STATIC.Rcp.Current.PIDName));
                bool b = DrvIC.ReadPID(STATIC.Rcp.Current.PIDName);
                if (!b)
                {
                    Log.AddLog(ch, string.Format("PID Load Fail."));
                    Log.AddLog(ch + 1, string.Format("PID Load Fail."));
                    m_ChannelOn[ch] = false;
                    m_ChannelOn[ch + 1] = false;
                    errMsg[ch] = testItem + " Error";
                    errMsg[ch + 1] = testItem + " Error";
                    STATIC.isNonSpecError[ch] = true;
                    STATIC.isNonSpecError[ch + 1] = true;
                }
                else { STATIC.PIDName = Path.GetFileName(STATIC.Rcp.Current.PIDName); }

            }

        }
        private void Act_IRISHallAdjustment(int ch, string testItem)
        {
            short poscal;
            short negcal;
            if (m_ChannelOn[ch])
            {
                if (!DrvIC.IRIS_Adjustment(ch))
                {
                    errMsg[ch] = testItem + " Error";
                    m_ChannelOn[ch] = false;
                    STATIC.isNonSpecError[ch] = true;
                
                }

            }

        }

        void Act_EPA(int port, string testItem)
        {
            int ch = port * 2;
            for (int k = ch; k < ch + ChannelCnt; k++)
            {
                if (m_ChannelOn[k])
                {
                    ushort PosVt = 0;
                    ushort NegVt = 0;
                    byte[] rbuf = new byte[1];
                    byte backData;

                    Dln.WriteArray(k, DrvIC.AkSlave[k], 0x02, 1, new byte[] { 0x40 });
                    Thread.Sleep(5);
                    Dln.WriteArray(k, DrvIC.AkSlave[k], 0xAE, 1, new byte[] { 0x3B });

                    if(Condition.PosEPANo != 0)
                    {
                        PosVt = (ushort)(((0xFFC << 4) - (16 * (Condition.PosEPANo - 1))));
                        Log.AddLog(k, $"Pos EPA = 0x{PosVt.ToString("X4")}");
                        Dln.WriteArray(k, DrvIC.AkSlave[k], 0xC0, 1, new byte[] { (byte)((PosVt >> 8) & 0xFF), (byte)(PosVt & 0xFF) });
                    }
                    if(Condition.NegEPANo != 0)
                    {
                        NegVt = (ushort)(((16 * (Condition.NegEPANo))));
                        Log.AddLog(k, $"Neg EPA = 0x{NegVt.ToString("X4")}");
                        Dln.WriteArray(k, DrvIC.AkSlave[k], 0xC2, 1, new byte[] { (byte)((NegVt >> 8) & 0xFF), (byte)(NegVt & 0xFF) });
                    }
                    Dln.ReadArray(k, DrvIC.AkSlave[k], 1, 0x0B, rbuf);
                    backData = rbuf[0];
                    Dln.WriteArray(k, DrvIC.AkSlave[k], 1, 0x0B, new byte[] { (byte)(rbuf[0] | 0x80) });//0x0B값 
                    Dln.WriteArray(k, DrvIC.AkSlave[k], 1, 0x03, new byte[] { 0x01 });
                    Thread.Sleep(150);
                    Dln.ReadArray(k, DrvIC.AkSlave[k], 1, 0x4B, rbuf);
                    if ((byte)(rbuf[0] & 0x02) != 0x00)
                    {
                        errMsg[k] = testItem + " Error";
                        m_ChannelOn[k] = false;
                        STATIC.isNonSpecError[k] = true;
                        return;
                    }
                    Dln.WriteArray(k, DrvIC.AkSlave[k], 1, 0x0B, new byte[] { backData });//0x0B값 
                    Dln.WriteArray(k, DrvIC.AkSlave[k], 1, 0xAE, new byte[] { 0x00 });//0x0B값 
                    Dln.WriteArray(k, DrvIC.AkSlave[k], 0x02, 1, new byte[1] { 0x40 });
                }


            }
        }



        private void Act_IRISHallAdjustment2(int ch, string testItem)
        {
            short poscal;
            short negcal;
            if (m_ChannelOn[ch])
            {
                for (int i = 0; i < 6; i++)
                {
                    if (!DrvIC.IRIS_Adjustment_byMode(ch, 0))
                    {
                        errMsg[ch] = testItem + " Error";
                        m_ChannelOn[ch] = false;
                        STATIC.isNonSpecError[ch] = true;
                        break;
                    }
                    else
                    {
                        poscal = (short)(DrvIC.Dln.Read2Byte_Short(ch, DrvIC.AkSlave[ch], 0x04, 1) / 2);
                        negcal = (short)(DrvIC.Dln.Read2Byte_Short(ch, DrvIC.AkSlave[ch], 0x06, 1) / 2);
                        Log.AddLog(ch, string.Format("PosCal - NegCal = {0}, poscal = {1}, negcal = {2}", Math.Abs(poscal - negcal), poscal, negcal));
                   
                        if (Math.Abs(poscal - negcal) > 1500) break;
                        if (i == 5)
                        {
                            errMsg[ch] = testItem + " Error";
                            m_ChannelOn[ch] = false;
                            STATIC.isNonSpecError[ch] = true;
                            break;
                        }

                    }
                }

            }

        }
        private void Act_IRISHallAdjustment3(int ch, string testItem)
        {
            short poscal;
            short negcal;
            if (m_ChannelOn[ch])
            {
                for (int i = 0; i < 6; i++)
                {
                    if (!DrvIC.IRIS_Adjustment_byMode(ch, 1))
                    {
                        errMsg[ch] = testItem + " Error";
                        m_ChannelOn[ch] = false;
                        STATIC.isNonSpecError[ch] = true;
                        break;
                    }
                    else
                    {
                        poscal = (short)(DrvIC.Dln.Read2Byte_Short(ch, DrvIC.AkSlave[ch], 0x04, 1) / 2);
                        negcal = (short)(DrvIC.Dln.Read2Byte_Short(ch, DrvIC.AkSlave[ch], 0x06, 1) / 2);
                        Log.AddLog(ch, string.Format("PosCal - NegCal = {0}, poscal = {1}, negcal = {2}", Math.Abs(poscal - negcal), poscal, negcal));

                        if (Math.Abs(poscal - negcal) > 1500) break;
                        if (i == 5)
                        {
                            errMsg[ch] = testItem + " Error";
                            m_ChannelOn[ch] = false;
                            STATIC.isNonSpecError[ch] = true;
                            break;
                        }

                    }
                }

            }

        }
        private void Act_IRISHallAdjustment4(int ch, string testItem)
        {
            short poscal;
            short negcal;
            if (m_ChannelOn[ch])
            {
                for (int i = 0; i < 6; i++)
                {
                    if (!DrvIC.IRIS_Adjustment_byMode(ch, 2))
                    {
                        errMsg[ch] = testItem + " Error";
                        m_ChannelOn[ch] = false;
                        STATIC.isNonSpecError[ch] = true;
                        break;
                    }
                    else
                    {
                        poscal = (short)(DrvIC.Dln.Read2Byte_Short(ch, DrvIC.AkSlave[ch], 0x04, 1) / 2);
                        negcal = (short)(DrvIC.Dln.Read2Byte_Short(ch, DrvIC.AkSlave[ch], 0x06, 1) / 2);
                        Log.AddLog(ch, string.Format("PosCal - NegCal = {0}, poscal = {1}, negcal = {2}", Math.Abs(poscal - negcal), poscal, negcal));

                        if (Math.Abs(poscal - negcal) > 1500) break;
                        if (i == 5)
                        {
                            errMsg[ch] = testItem + " Error";
                            m_ChannelOn[ch] = false;
                            STATIC.isNonSpecError[ch] = true;
                            break;
                        }

                    }
                }

            }

        }

        private void Act_PhaseMargin(int ch, string testItem)
        {
            //double Phase = 0, Freq = 0;
            //if (m_ChannelOn[ch])
            //{
            //    (Phase, Freq) = DrvIC.AK7316_PhaseMargin(ch, (ushort)Condition.PM_FinalFreq, (ushort)Condition.PM_StartFreq, (ushort)Condition.PM_StepFreq, (ushort)Condition.PM_Amp, (ushort)Condition.PM_Gainthr);
            //    Spec.PassFails[ch].Results[(int)SpecItem.PhaseMargin].Val = Phase;
            //    Spec.PassFails[ch].Results[(int)SpecItem.PM_Freq].Val = Freq;
            //    Spec.SetResult(ch, (int)SpecItem.PhaseMargin, (int)SpecItem.PM_Freq);
            //    ShowEvent.Show(ch, "PM");
            //}
             
        }



        private void Act_PostionTest(int ch, string testItem)
        {
            
            if (!m_ChannelOn[ch])
                return;

         //   LEDByChannle(ch, true);

            Process_ScanCodeTest(ch, testItem);
            Process_CalcCodeTest(ch, testItem);

        //    LEDByChannle(ch, false);
        }


        void Act_Settling(int port, string testItem)
        {
            int ch = port * 2;
            if (!m_ChannelOn[ch] && !m_ChannelOn[ch + 1])
                return;
            Process_ScanTimeTest(port, testItem);
            Process_CalcTimeTest(port, testItem);
        }



        private void Act_LinearityCal(int ch, string testItem)
        {
            if (!m_ChannelOn[ch])
                return;

         //   LEDs_All_On(true);
            AK7316_LinearityComp(ch, Condition.LinCompStart, Condition.LinCompEnd, Condition.LinCompCount, 0, 0, 0, 0, 0);
        //    LEDs_All_On(false);
        }

        private void Act_LinearityCal_2Dir(int ch, string testItem)
        {
            if (!m_ChannelOn[ch])
                return;

       //    LEDs_All_On(true);
            AK7316_LinearityComp_2Dir(ch, Condition.LinCompStart, Condition.LinCompEnd, Condition.LinCompCount, 0, 0, 0, 0, 0, true);
         //   LEDs_All_On(false);
        }
        private void Act_LinearityCal_2Dir_rev(int ch, string testItem)
        {
            if (!m_ChannelOn[ch])
                return;

            //    LEDs_All_On(true);
            AK7316_LinearityComp_2Dir(ch, Condition.LinCompStart, Condition.LinCompEnd, Condition.LinCompCount, 0, 0, 0, 0, 0, false);
            //   LEDs_All_On(false);
        }

        void DrivingByOption(int ch, int index, bool isSoftLanding, bool isBackStep, int beforeHall, int CurrentHall, int BackStepCode)
        {
           // int[] StepCondition = new int[4] { Condition.SoftLandingStep1, Condition.SoftLandingStep2, Condition.SoftLandingStep3, Condition.SoftLandingStep4 };
            if (isSoftLanding)
            {
                //if (isBackStep)
                //{
                //    DrvIC.Move(ch, DriverIC.AK, CurrentHall);
                //    Wait(Condition.BackStepDelay1);//Thread.Sleep(Condition.BackStepDelay1);

                //    int Stepsize = (int)Math.Ceiling((BackStepCode - CurrentHall) / (double)Condition.BackStep_Step);
                //    for (int k = 0; k < Condition.BackStep_Step; k++)
                //    {
                //        int a = 0;

                //        a = CurrentHall + (Stepsize * (k + 1));
                //        if (k == Condition.BackStep_Step - 1)
                //            DrvIC.Move(ch, DriverIC.AK, BackStepCode);
                //        else
                //            DrvIC.Move(ch, DriverIC.AK, a);

                //        Wait(Condition.BackStepDelay2);
                     
                //    }
                //}
                //else
                //{
                //    if (Math.Abs(beforeHall - CurrentHall) > Condition.SoftLandingCode)
                //    {

                //        for (int k = 0; k < 4; k++)
                //        {
                //            int Stepsize = (int)((CurrentHall - beforeHall) * StepCondition[k] / 100.0);
                //            int a = 0;
                //            a = beforeHall + Stepsize;
                //            DrvIC.Move(ch, DriverIC.AK, a);
                //            Wait(Condition.SoftLandingDelay);
                //            //   Thread.Sleep(Condition.SoftLandingDelay);
                //        }
                //        DrvIC.Move(ch, DriverIC.AK, CurrentHall);
                //    }
                //    else
                //        DrvIC.Move(ch, DriverIC.AK, CurrentHall);
                //}




            }
            else
            {
                if (isBackStep)
                {
                    //DrvIC.Move(ch, DriverIC.AK, CurrentHall);
                    //Wait(Condition.BackStepDelay1);

                    //int Stepsize = (int)Math.Ceiling((PosTest[ch][index].code - CurrentHall) / (double)Condition.BackStep_Step);
                    //for (int k = 0; k < Condition.BackStep_Step; k++)
                    //{
                    //    int a = 0;

                    //    a = CurrentHall + (Stepsize * (k + 1));
                    //    if (k == Condition.BackStep_Step - 1)
                    //        DrvIC.Move(ch, DriverIC.AK, BackStepCode);
                    //    else
                    //        DrvIC.Move(ch, DriverIC.AK, a);

                    //    Wait(Condition.BackStepDelay2);

                    //}
                }
                
                else
                    DrvIC.Move(ch, DriverIC.AK, BackStepCode);
            }
        }


        void Process_ScanCodeTest(int ch, string name)
        {
            InspResult CRes = new InspResult();
            InspResult Res = new InspResult();
            InspResult CoverRes = new InspResult();

            Stopwatch sw = new Stopwatch();
            sw.Reset(); sw.Start();

            List<InspResult> tmpRes = new List<InspResult>();


            int beforeHall;
            int afterHall;
            int[] Open_OpenLoopIndex = new int[2];
            int[] Close_OpenLoopIndx = new int[2];
            int[] F20_Index = new int[2];
            int[] F28_Index = new int[2];
            double DecenterScale = Dscale.DecenterScale_L;
            if (ch == 1)
                DecenterScale = Dscale.DecenterScale_R;


            //InspResult tmp = new InspResult();
            //tmp.code = 1500;
            tmpRes = PosTest[ch].ToList();
            //if (Option.Code1500InspUse)
            //{
            //    tmpRes.Insert(1, new InspResult());
            //    tmpRes[1].code = 1500;
            //    tmpRes.Insert(tmpRes.Count - 1, new InspResult());
            //    tmpRes[tmpRes.Count - 2].code = 1500;

            //}

            if (Option.Step10Use)
            {
                Open_OpenLoopIndex = new int[2] { 0, 19 };
                Close_OpenLoopIndx = new int[2] { 9, 10 };
                F20_Index = new int[2] { 3, 16 };
                F28_Index = new int[2] { 6, 13 };

                //if (Model.ModelName == "SO1C81")
                //{
                //    if (Option.Code1500InspUse)
                //    {

                //        Open_OpenLoopIndex = new int[2] { 0, 21 };
                //        Close_OpenLoopIndx = new int[2] { 10, 11 };
                //        F20_Index = new int[2] { 4, 17 };
                //        F28_Index = new int[2] { 7, 14 };

                //    }
                //    else
                //    {
                //        Open_OpenLoopIndex = new int[2] { 0, 19 };
                //        Close_OpenLoopIndx = new int[2] { 9, 10 };
                //        F20_Index = new int[2] { 3, 16 };
                //        F28_Index = new int[2] { 6, 13 };
                //    }
                //}
                //else if (Model.ModelName == "SO1G73")
                //{
                //    if (Option.Code1500InspUse)
                //    {
                //        Open_OpenLoopIndex = new int[2] { 0, 19 };
                //        Close_OpenLoopIndx = new int[2] { 9, 10 };
                //        F20_Index = new int[2] { 3, 16 };
                //        F28_Index = new int[2] { 6, 13 };
                //    }
                //    else
                //    {
                //        Open_OpenLoopIndex = new int[2] { 0, 17 };
                //        Close_OpenLoopIndx = new int[2] { 8, 9 };
                //        F20_Index = new int[2] { 2, 15 };
                //        F28_Index = new int[2] { 5, 12 };

                //    }
                //}
            }
            else
            {
                Open_OpenLoopIndex = new int[2] { 0, 7 };
                Close_OpenLoopIndx = new int[2] { 3, 4 };
                F20_Index = new int[2] { 1, 6 };
                F28_Index = new int[2] { 2, 5 };
                //if (Option.Code1500InspUse)
                //{
                //    Open_OpenLoopIndex = new int[2] { 0, 9 };
                //    Close_OpenLoopIndx = new int[2] { 4, 5 };
                //    F20_Index = new int[2] { 2, 7 };
                //    F28_Index = new int[2] { 3, 6 };
                //}
                //else
                //{

                //    Open_OpenLoopIndex = new int[2] { 0, 7 };
                //    Close_OpenLoopIndx = new int[2] { 3, 4 };
                //    F20_Index = new int[2] { 1, 6 };
                //    F28_Index = new int[2] { 2, 5 };
                //}

            }

            //if (Model.ModelName == "SO1G73" && STATIC.SaveImageItrCnt == 0)
            //{
            //    if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
            //    {
            //        Log.AddLog(ch, string.Format("OpenLoop Aging"));
            //        AK7316_F14OpenLoop(ch, false, true);
            //        AK7316_F14OpenLoop(ch, true, true);

            //    }

            //}

            if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
            {
                if (Option.ReverseDrv)
                    DrvIC.Move(ch, DriverIC.AK, Condition.ReadyPosOffset14);
                else
                    DrvIC.Move(ch, DriverIC.AK, 4095 - Condition.ReadyPosOffset14);

                Wait(Condition.iDrvStepInterval);
                //  Thread.Sleep(Condition.iDrvStepInterval * 5);
                Log.AddLog(ch, string.Format("Move Ready Position, Read Hall = {0}", DrvIC.ReadHall(ch, DriverIC.AK)));

            }
            //if (Model.ModelName == "SO1G73")
            //{
            //    if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
            //        TemperCheck(ch);

            //}

            //Code Grab ===============

            for (int i = 0; i < tmpRes.Count; i++)
            {
                if (m_ChannelOn[ch] && i == Close_OpenLoopIndx[1] && !STATIC.isNonSpecError[ch])
                {
                    if (Option.ReverseDrv)
                        DrvIC.Move(ch, DriverIC.AK, 4095 - Condition.ReadyPosOffset40);
                    else
                        DrvIC.Move(ch, DriverIC.AK, Condition.ReadyPosOffset40);
                    Wait(Condition.iDrvStepInterval);
                    Log.AddLog(ch, string.Format("Move Ready Position, Read Hall = {0}", DrvIC.ReadHall(ch, DriverIC.AK)));
                }
                if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
                {
                    if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
                    {
                        DrvIC.Move(ch, DriverIC.AK, tmpRes[i].code);
                        Wait(Condition.iDrvStepInterval);
                        Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x02, 1, new byte[] { 0x40 });

                    }
                    else
                    {
                        if (i == 1)
                        {
                            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x02, 1, new byte[] { 0x00 });
                            Thread.Sleep(30);
                        }
                        
                        DrvIC.Move(ch, DriverIC.AK, tmpRes[i].code);
                    }

                }
                Wait(Condition.iDrvStepInterval);

                if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
                {
                    //if (i == F20_Index[0] || i == F20_Index[1] || i == F28_Index[0] || i == F28_Index[1])
                    //{

                    //    if (Model.ModelName == "SO1G73")
                    //        TemperCheck(ch);
                    //}
                    tmpRes[i].current = Dln.GetCurrent(ch);
                    tmpRes[i].IrisHall = DrvIC.ReadHall(ch, DriverIC.AK);
                }

                Cam.CamList[ch].Acquire();

                string path = string.Empty;

                if (Option.SaveImage)
                {
                    string dateDir = STATIC.CreateDateDir();
                    dateDir += "Position_DrvImg\\";
                    if (!Directory.Exists(dateDir))
                        Directory.CreateDirectory(dateDir);

                    path = string.Format("{0}{1}Position_{2}_{3}_{4}.bmp", dateDir, m_StrIndex[ch], i, STATIC.SaveImageCurrentPos, STATIC.SaveImageItrCnt + 1);
                    STATIC.InspMat[ch].SaveImage(path);

                }

                //if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
                //{
                    
                //}
                //else
                //{
                 
                //}
            //    InspApi[ch].NewFineCOG(ch, i, STATIC.InspMat[ch].Clone(), InspectionType.FindCover, CoverRes, Option.ShowResultToImage, false, false, STATIC.SaveImageCurrentPos, STATIC.SaveImageItrCnt + 1);
                if (Option.AreaDecenterUse)
                    InspApi[ch].NewFineCOG(ch, i, STATIC.InspMat[ch].Clone(), InspectionType.Area_InspAll, Res, Option.ShowResultToImage, false, false, STATIC.SaveImageCurrentPos, STATIC.SaveImageItrCnt + 1);

                else
                    InspApi[ch].NewFineCOG(ch, i, STATIC.InspMat[ch].Clone(), InspectionType.InCircle_InspAll, Res, Option.ShowResultToImage, false, false, STATIC.SaveImageCurrentPos, STATIC.SaveImageItrCnt + 1);
                //if (Option.ActroDllUse)
                //{

                //}
                STATIC.GapSpacerPos[ch] = new System.Drawing.PointF((float)Res.Cover_cx, (float)Res.Cover_cy);
                //            STATIC.C_GapSpacerPos[ch] = new System.Drawing.PointF((float)CRes.cx, (float)CRes.cy);

                //if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
                //{

                //    if (Option.BaseOnSpacer)
                //    {
                //        if (i == Open_OpenLoopIndex[0])
                //        {
                //            STATIC.GapSpacerPos[ch] = new System.Drawing.PointF((float)Res.cx, (float)Res.cy);
                //            STATIC.C_GapSpacerPos[ch] = new System.Drawing.PointF((float)CRes.cx, (float)CRes.cy);


                //        }
                //    }
                //}
                //else
                //{
                //    if (!Option.BaseOnSpacer)
                //    {

                //        STATIC.GapSpacerPos[ch] = new System.Drawing.PointF((float)CoverRes.Cover_cx, (float)CoverRes.Cover_cy);
                //        STATIC.C_GapSpacerPos[ch] = new System.Drawing.PointF((float)CRes.Cover_cx, (float)CRes.Cover_cy);

                //    }

                //}

                if (!STATIC.isNonSpecError[ch])
                {
                    double X = 0;
                    double Y = 0;
                    double C_X = 0;
                    double C_Y = 0;


                    if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
                    {
                        if (Option.is1CH_MC)
                        {
                            STATIC.DecenterX_Pos1 = X = STATIC.ScaleResolution[ch] * 1000 * (STATIC.GapSpacerPos[ch].X - Res.cx) * DecenterScale;
                            STATIC.DecenterY_Pos1 = Y = STATIC.ScaleResolution[ch] * 1000 * (STATIC.GapSpacerPos[ch].Y - Res.cy) * DecenterScale;
                            //if (STATIC.SaveImageCurrentPos == 1)
                            //{
                               
                            //}
                            //else
                            //{
                            //    X = STATIC.DecenterX_Pos1;
                            //    Y = STATIC.DecenterY_Pos1;
                            //}


                            //if (STATIC.SaveImageCurrentPos == 1)
                            //{
                            //    STATIC.CDecenterX_Pos1 = C_X = 1000 * (STATIC.C_CoverPos[ch].X - CRes.cx);
                            //    STATIC.CDecenterY_Pos1 = C_Y = 1000 * (STATIC.C_CoverPos[ch].Y - CRes.cy);
                            //}
                            //else
                            //{
                            //    C_X = STATIC.CDecenterX_Pos1;
                            //    C_Y = STATIC.CDecenterY_Pos1;
                            //}
                            //if (STATIC.Rcp.Model.ModelName == "SO1G73")
                            //{
                            //    if (Option.isActMode)
                            //    {
                            //        if (STATIC.SaveImageCurrentPos == 1)
                            //        {
                            //            STATIC.CDecenterX_Pos1 = C_X = 1000 * (STATIC.C_CoverPos[ch].X - CRes.cx);
                            //            STATIC.CDecenterY_Pos1 = C_Y = 1000 * (STATIC.C_CoverPos[ch].Y - CRes.cy);
                            //        }
                            //        else
                            //        {
                            //            C_X = STATIC.CDecenterX_Pos1;
                            //            C_Y = STATIC.CDecenterY_Pos1;
                            //        }

                            //    }
                            //    else
                            //    {
                            //        C_X = 1000 * (STATIC.C_GapSpacerPos[ch].X - (float)CRes.cx);
                            //        C_Y = 1000 * (STATIC.C_GapSpacerPos[ch].Y - (float)CRes.cy);
                            //    }


                            //}
                            //else
                            //{
                            //    if (STATIC.SaveImageCurrentPos == 1)
                            //    {
                            //        STATIC.CDecenterX_Pos1 = C_X = 1000 * (STATIC.C_CoverPos[ch].X - CRes.cx);
                            //        STATIC.CDecenterY_Pos1 = C_Y = 1000 * (STATIC.C_CoverPos[ch].Y - CRes.cy);
                            //    }
                            //    else
                            //    {
                            //        C_X = STATIC.CDecenterX_Pos1;
                            //        C_Y = STATIC.CDecenterY_Pos1;
                            //    }


                            //}
                        }
                        else
                        {
                            X = STATIC.ScaleResolution[ch] * 1000 * (STATIC.GapSpacerPos[ch].X - Res.cx) * DecenterScale;
                            Y = STATIC.ScaleResolution[ch] * 1000 * (STATIC.GapSpacerPos[ch].Y - Res.cy) * DecenterScale;

                            //C_X = 1000 * (STATIC.C_CoverPos[ch].X - CRes.cx);
                            //C_Y = 1000 * (STATIC.C_CoverPos[ch].Y - CRes.cy);
                            //if (STATIC.Rcp.Model.ModelName == "SO1G73")
                            //{
                            //    C_X = 1000 * (STATIC.C_GapSpacerPos[ch].X - (float)CRes.cx) * STATIC.ScaleResolution[ch];
                            //    C_Y = 1000 * (STATIC.C_GapSpacerPos[ch].Y - (float)CRes.cy) * STATIC.ScaleResolution[ch];

                            //}
                            //else
                            //{
                               
                            //}
                        }



                    }
                    else
                    {
                        X = STATIC.ScaleResolution[ch] * 1000 * (STATIC.GapSpacerPos[ch].X - Res.cx);
                        Y = STATIC.ScaleResolution[ch] * 1000 * (STATIC.GapSpacerPos[ch].Y - Res.cy);
                        //if (Model.ModelName == "SO1C81")
                        //{
                        //    C_X = 1000 * (STATIC.C_GapSpacerPos[ch].X - CRes.cx);
                        //    C_Y = 1000 * (STATIC.C_GapSpacerPos[ch].Y - CRes.cy);
                        //}
                        //else
                        //{
                        //    if (Option.is1CH_MC)
                        //    {
                        //        C_X = 1000 * (STATIC.C_GapSpacerPos[ch].X - CRes.cx);
                        //        C_Y = 1000 * (STATIC.C_GapSpacerPos[ch].Y - CRes.cy);

                        //    }
                        //    else
                        //    {
                        //        C_X = 1000 * (STATIC.C_CoverPos[ch].X - CRes.cx);
                        //        C_Y = 1000 * (STATIC.C_CoverPos[ch].Y - CRes.cy);
                        //    }


                        //}

                    }

                    //X = -X;
                    //Y = -Y;
                    //C_X = -C_X;
                    //C_Y = -C_Y;
                    if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
                    {
                        if (Option.is1CH_MC && Option.ActOpenAreaOp)
                            tmpRes[i].Area = STATIC.F40CoverDia / 2.0 * STATIC.F40CoverDia / 2.0 * Math.PI * STATIC.Rcp.F16Scale.F16Scale;
                        else tmpRes[i].Area = Res.Area;
                    }
                    else tmpRes[i].Area = Res.Area;
                    tmpRes[i].DecenterR = Math.Sqrt(X * X + Y * Y);
                    tmpRes[i].DecenterX = X;
                    tmpRes[i].DecenterY = Y;
                    if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
                    { tmpRes[i].CircleAcuraccy = Res.ShapeAccuracy; }
                    else { tmpRes[i].CircleAcuraccy = Res.CircleAcuraccy; }

                    tmpRes[i].ShapeAccuracy = Res.ShapeAccuracy;

                    //if (Option.ActroDllUse)
                    //{
                    //                    }
                    //if (Option.CDllUse)
                    //{
                    //    if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
                    //    {
                    //        if (Model.ModelName == "SO1C81" || Option.is1CH_MC)
                    //        {
                    //            if (Option.is1CH_MC && Option.ActOpenAreaOp)
                    //                tmpRes[i].CArea = STATIC.F40CoverDia / 2.0 * STATIC.F40CoverDia / 2.0 * Math.PI * STATIC.Rcp.F16Scale.F16Scale;
                    //            else tmpRes[i].CArea = CRes.CArea;
                    //            tmpRes[i].CDecenterR = Math.Sqrt(C_X * C_X + C_Y * C_Y);
                    //            tmpRes[i].CDecenterX = C_X;
                    //            tmpRes[i].CDecenterY = C_Y;
                    //            if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
                    //            { tmpRes[i].CCircleAcuraccy = CRes.CShapeAccuracy * 1000; }
                    //            else { tmpRes[i].CCircleAcuraccy = CRes.CCircleAcuraccy * 1000; }

                    //            tmpRes[i].CShapeAccuracy = CRes.CShapeAccuracy * 1000;
                    //        }
                    //        else
                    //        {
                    //            tmpRes[i].CArea = CRes.Area;
                    //            tmpRes[i].CDecenterR = Math.Sqrt(C_X * C_X + C_Y * C_Y);
                    //            tmpRes[i].CDecenterX = C_X;
                    //            tmpRes[i].CDecenterY = C_Y;
                    //            tmpRes[i].CCircleAcuraccy = CRes.ShapeAccuracy;
                    //            tmpRes[i].CShapeAccuracy = CRes.ShapeAccuracy;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        tmpRes[i].CArea = CRes.CArea;
                    //        tmpRes[i].CDecenterR = Math.Sqrt(C_X * C_X + C_Y * C_Y);
                    //        tmpRes[i].CDecenterX = C_X;
                    //        tmpRes[i].CDecenterY = C_Y;
                    //        if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
                    //        { tmpRes[i].CCircleAcuraccy = CRes.CShapeAccuracy * 1000; }
                    //        else { tmpRes[i].CCircleAcuraccy = CRes.CCircleAcuraccy * 1000; }

                    //        tmpRes[i].CShapeAccuracy = CRes.CShapeAccuracy * 1000;
                    //    }
                    //}



                }



                //Area 측정
                if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
                {
                    Log.AddLog(ch, String.Format("Target Code {0}, Current {1:0.00}, Read {2}, Area {3:0.00} ",
                         tmpRes[i].code, tmpRes[i].current, tmpRes[i].IrisHall, tmpRes[i].Area));
                    //if (Option.ActroDllUse)
                    //{
                       
                    //}
                    //if (!Option.ActroDllUse && Option.CDllUse)
                    //{
                    //    Log.AddLog(ch, String.Format("Target Code {0}, Current {1:0.00}, Read {2}, Area {3:0.00} ",
                    // tmpRes[i].code, tmpRes[i].current, tmpRes[i].IrisHall, tmpRes[i].CArea));
                    //}

                    //string length = "";
                    //for (int k = 0; k < PosTest[port][i].lengthPtoC[j].Length; k++)
                    //    length += string.Format(" {0:0.000} ", PosTest[port][i].lengthPtoC[j][k]);
                    //if (i != 0 && i != 19)
                    //    Log.AddLog(j, string.Format("Shape Length [{0}]", length));

                    Log.AddLog(ch, string.Format("\r\n"));

                }
                if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
                {
                    if (Option.Pos14OpenLoopUse)
                    {
                        if (!STATIC.isNonSpecError[ch])
                        {
                            AK7316_F14OpenLoop(ch, true, false);

                        }


                    }

                }

                if (i == Close_OpenLoopIndx[0] || i == Close_OpenLoopIndx[1])
                {
                    if (Option.Pos40OpenLoopUse)
                    {
                        if (!STATIC.isNonSpecError[ch])
                        {
                            AK7316_F40OpenLoop(ch, true);

                        }

                    }
                }


            }

            sw.Stop();

            if (!STATIC.isNonSpecError[ch])
            {
                if (Option.ReverseDrv)
                    DrvIC.Move(ch, DriverIC.AK, Condition.ReadyPosOffset14);
                else
                    DrvIC.Move(ch, DriverIC.AK, 4095 - Condition.ReadyPosOffset14);
            }



            //if (Option.Code1500InspUse)
            //{
            //    Spec.PassFails[ch].Results[(int)SpecItem.CodeOC1500_Area].Val = tmpRes[1].Area;
            //    Spec.PassFails[ch].Results[(int)SpecItem.CodeOC1500_CArea].Val = tmpRes[1].CArea;
            //    Spec.PassFails[ch].Results[(int)SpecItem.CodeCO1500_Area].Val = tmpRes[tmpRes.Count - 2].Area;
            //    Spec.PassFails[ch].Results[(int)SpecItem.CodeCO1500_CArea].Val = tmpRes[tmpRes.Count - 2].CArea;

            //    Spec.SetResult(ch, (int)SpecItem.CodeOC1500_Area, (int)SpecItem.CodeCO1500_CArea);
            //    ShowEvent.Show(ch, string.Format("{0}", "Code 1500"));

            //    tmpRes.RemoveAt(tmpRes.Count - 2);
            //    tmpRes.RemoveAt(1);
            //}
            PosTest[ch].Clear();
            PosTest[ch] = tmpRes.ToList();

            if (STATIC.isTmpLog)
            {
                STATIC.ResolutionLog += string.Format("{0},{1},{2},{3}\r\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"), m_StrIndex[ch],
                        PosTest[ch][0].CDecenterX.ToString("F3"), PosTest[ch][0].CDecenterY.ToString("F3"));
                STATIC.RepeatLog += string.Format("{0},{1},{2},{3}\r\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"), m_StrIndex[ch],
                      PosTest[ch][0].CDecenterX.ToString("F3"), PosTest[ch][0].CDecenterY.ToString("F3"));


            }
            //  Vision.MilList[port].CommonToReplayBuf(name, PosTest[port].Count);
        }

        //void Process_ScanCodeTest(int ch, string name)
        //{
           
        //    InspResult CRes = new InspResult();
        //    InspResult Res = new InspResult();
        //    InspResult CoverRes = new InspResult();

        //    Stopwatch sw = new Stopwatch();
        //    sw.Reset(); sw.Start();

        //    List<InspResult> tmpRes = new List<InspResult>();
           
         
        //    int beforeHall;
        //    int afterHall;
        //    int[] Open_OpenLoopIndex = new int[2];
        //    int[] Close_OpenLoopIndx = new int[2];
        //    int[] F20_Index = new int[2];
        //    int[] F28_Index = new int[2];
        //    double DecenterScale = Dscale.DecenterScale_L;
        //    if (ch == 1)
        //        DecenterScale = Dscale.DecenterScale_R;


        //    //InspResult tmp = new InspResult();
        //    //tmp.code = 1500;
        //    tmpRes = PosTest[ch].ToList();
        //    if(Option.Code1500InspUse)
        //    {
        //        tmpRes.Insert(1, new InspResult());
        //        tmpRes[1].code = 1500;
        //        tmpRes.Insert(tmpRes.Count - 1, new InspResult());
        //        tmpRes[tmpRes.Count - 2].code = 1500;

        //    }

        //    if (Option.Step10Use)
        //    {
        //        if(Model.ModelName == "SO1C81")
        //        {
        //            if(Option.Code1500InspUse)
        //            {
                       
        //                Open_OpenLoopIndex = new int[2] { 0, 21 };
        //                Close_OpenLoopIndx = new int[2] { 10, 11 };
        //                F20_Index = new int[2] { 4, 17 };
        //                F28_Index = new int[2] { 7, 14 };

        //            }
        //            else
        //            {
        //                Open_OpenLoopIndex = new int[2] { 0, 19 };
        //                Close_OpenLoopIndx = new int[2] { 9, 10 };
        //                F20_Index = new int[2] { 3, 16 };
        //                F28_Index = new int[2] { 6, 13 };
        //            }
        //        }
        //        else if(Model.ModelName == "SO1G73")
        //        {
        //            if (Option.Code1500InspUse)
        //            {
        //                Open_OpenLoopIndex = new int[2] { 0, 19 };
        //                Close_OpenLoopIndx = new int[2] { 9, 10 };
        //                F20_Index = new int[2] { 3, 16 };
        //                F28_Index = new int[2] { 6, 13 };
        //            }
        //            else
        //            {
        //                Open_OpenLoopIndex = new int[2] { 0, 17 };
        //                Close_OpenLoopIndx = new int[2] { 8, 9 };
        //                F20_Index = new int[2] { 2, 15 };
        //                F28_Index = new int[2] { 5, 12 };

        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (Option.Code1500InspUse)
        //        {
        //            Open_OpenLoopIndex = new int[2] { 0, 9 };
        //            Close_OpenLoopIndx = new int[2] { 4, 5 };
        //            F20_Index = new int[2] { 2, 7 };
        //            F28_Index = new int[2] { 3, 6 };
        //        }
        //        else
        //        {

        //            Open_OpenLoopIndex = new int[2] { 0, 7 };
        //            Close_OpenLoopIndx = new int[2] { 3, 4 };
        //            F20_Index = new int[2] { 1, 6 };
        //            F28_Index = new int[2] { 2, 5 };
        //        }

        //    }

        //    if (Model.ModelName == "SO1G73" && STATIC.SaveImageItrCnt == 0)
        //    {
        //        if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //        {
        //            Log.AddLog(ch, string.Format("OpenLoop Aging"));
        //            AK7316_F14OpenLoop(ch, false, true);
        //            AK7316_F14OpenLoop(ch, true, true);

        //        }

        //    }

        //    if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //    {
        //        if (Option.ReverseDrv)
        //            DrvIC.Move(ch, DriverIC.AK, Condition.ReadyPosOffset14);
        //        else
        //            DrvIC.Move(ch, DriverIC.AK, 4095 - Condition.ReadyPosOffset14);

        //        Wait(Condition.iDrvStepInterval);
        //        //  Thread.Sleep(Condition.iDrvStepInterval * 5);
        //        Log.AddLog(ch, string.Format("Move Ready Position, Read Hall = {0}", DrvIC.ReadHall(ch, DriverIC.AK)));

        //    }
        //    if (Model.ModelName == "SO1G73")
        //    {
        //        if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //            TemperCheck(ch);

        //    }

        //    //Code Grab ===============

        //    for (int i = 0; i < tmpRes.Count; i++)
        //    {
        //        if (m_ChannelOn[ch] && i == Close_OpenLoopIndx[1] && !STATIC.isNonSpecError[ch])
        //        {
        //            if (Option.ReverseDrv)
        //                DrvIC.Move(ch, DriverIC.AK, 4095 - Condition.ReadyPosOffset40);
        //            else
        //                DrvIC.Move(ch, DriverIC.AK, Condition.ReadyPosOffset40);
        //            Wait(Condition.iDrvStepInterval);
        //            Log.AddLog(ch, string.Format("Move Ready Position, Read Hall = {0}", DrvIC.ReadHall(ch, DriverIC.AK)));
        //        }
        //        if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //        {
        //            if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
        //            {
        //                if (Option.Pos14OpenLoopUse)
        //                {
        //                    if (Model.ModelName == "SO1G73")
        //                        AK7316_F14OpenLoop(ch, false, false);
        //                    else
        //                        AK7316_iris_open_OL2(ch, 0);
        //                }
        //                //
        //                else
        //                {
        //                    if (i == Open_OpenLoopIndex[0])
        //                    {
        //                        if (Option.ReverseDrv)
        //                            beforeHall = Condition.ReadyPosOffset14;
        //                        else beforeHall = 4095 - Condition.ReadyPosOffset14;
        //                    }
        //                    else beforeHall = tmpRes[i - 1].code;
        //                    afterHall = tmpRes[i].code;
        //                    DrivingByOption(ch, i, Option.SoftLandingUse, false, beforeHall, afterHall, tmpRes[i].code);
        //                }

        //            }
        //            else if (i == Close_OpenLoopIndx[0] || i == Close_OpenLoopIndx[1])
        //            {
        //                if (Option.Pos40OpenLoopUse)
        //                    AK7316_F40OpenLoop(ch, false);
        //                else
        //                {
        //                    if (i == Close_OpenLoopIndx[0])
        //                    {
        //                        if (Option.BackStepUse)
        //                        {
        //                            int currentHall = 0;
        //                            if (Option.ReverseDrv)
        //                            {
        //                                currentHall = tmpRes[i].code + Condition.BackStep_Code;
        //                                if (currentHall > 4095 - Condition.BackStep40Offset)
        //                                    currentHall = 4095 - Condition.BackStep40Offset;

        //                            }
        //                            else
        //                            {
        //                                currentHall = tmpRes[i].code - Condition.BackStep_Code;
        //                                if (currentHall < Condition.BackStep40Offset)
        //                                    currentHall = Condition.BackStep40Offset;
        //                            }
        //                            DrivingByOption(ch, i, Option.SoftLandingUse, true, 0, currentHall, tmpRes[i].code);


        //                        }
        //                        else
        //                        {
        //                            beforeHall = tmpRes[i - 1].code;
        //                            afterHall = tmpRes[i].code;
        //                            DrivingByOption(ch, i, Option.SoftLandingUse, false, beforeHall, afterHall, tmpRes[i].code);

        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (Option.ReverseDrv)
        //                            beforeHall = 4095 - Condition.ReadyPosOffset40;
        //                        else beforeHall = Condition.ReadyPosOffset40;

        //                        afterHall = tmpRes[i].code;
        //                        DrivingByOption(ch, i, Option.SoftLandingUse, false, beforeHall, afterHall, tmpRes[i].code);

        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (i == F20_Index[0] || i == F20_Index[1] || i == F28_Index[0] || i == F28_Index[1])
        //                {

        //                    if (Model.ModelName == "SO1G73")
        //                        TemperCheck(ch);
        //                }
        //                if (i < Close_OpenLoopIndx[1])
        //                {
        //                    if (Option.BackStepUse)
        //                    {
        //                        int currentHall = 0;
        //                        if (Option.ReverseDrv)
        //                        {
        //                            currentHall = tmpRes[i].code + Condition.BackStep_Code;
        //                            if (currentHall > 4095 - Condition.BackStep40Offset)
        //                                currentHall = 4095 - Condition.BackStep40Offset;
        //                        }

        //                        else
        //                        {
        //                            currentHall = tmpRes[i].code - Condition.BackStep_Code;
        //                            if (currentHall < Condition.BackStep40Offset)
        //                                currentHall = Condition.BackStep40Offset;
        //                        }
        //                        DrivingByOption(ch, i, Option.SoftLandingUse, true, 0, currentHall, tmpRes[i].code);


        //                    }
        //                    else
        //                    {
        //                        if (i == 1)
        //                        {
        //                            if (Option.Pos14OpenLoopUse)
        //                            {
        //                                if (Option.ReverseDrv)
        //                                    beforeHall = Condition.ReadyPosOffset14;
        //                                else beforeHall = 4095 - Condition.ReadyPosOffset14;
        //                            }
        //                            else beforeHall = tmpRes[i - 1].code;
        //                        }
        //                        else beforeHall = tmpRes[i - 1].code;
        //                        afterHall = tmpRes[i].code;

        //                        DrivingByOption(ch, i, Option.SoftLandingUse, false, beforeHall, afterHall, tmpRes[i].code);
        //                    }
        //                }
        //                else
        //                {
        //                    beforeHall = tmpRes[i - 1].code;

        //                    if (i == F28_Index[1] && !Option.Step10Use && Condition.F4028MoveCode != 0)
        //                    {
        //                        beforeHall = Condition.F4028MoveCode;
        //                        DrvIC.Move(ch, DriverIC.AK, Condition.F4028MoveCode);
        //                        Thread.Sleep(Condition.F4028MoveDelay);
        //                    }
        //                    afterHall = tmpRes[i].code;
        //                    DrivingByOption(ch, i, Option.SoftLandingUse, false, beforeHall, afterHall, tmpRes[i].code);

        //                }

        //            }

        //        }
        //        Wait(Condition.iDrvStepInterval);

        //        if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //        {
        //            if (i == F20_Index[0] || i == F20_Index[1] || i == F28_Index[0] || i == F28_Index[1])
        //            {

        //                if (Model.ModelName == "SO1G73")
        //                    TemperCheck(ch);
        //            }
        //            tmpRes[i].current = Dln.GetCurrent(ch);
        //            tmpRes[i].IrisHall = DrvIC.ReadHall(ch, DriverIC.AK);
        //        }

        //        Cam.CamList[ch].Acquire();
             
        //        string path = string.Empty;
               
        //        if (Option.SaveImage)
        //        {
        //            string dateDir = STATIC.CreateDateDir();
        //            dateDir += "Position_DrvImg\\";
        //            if (!Directory.Exists(dateDir))
        //                Directory.CreateDirectory(dateDir);

        //            path = string.Format("{0}{1}Position_{2}_{3}_{4}.bmp", dateDir, m_StrIndex[ch], i, STATIC.SaveImageCurrentPos, STATIC.SaveImageItrCnt + 1);
        //            STATIC.InspMat[ch].SaveImage(path);
               
        //        }

        //        if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
        //        {
        //            if (Option.CDllUse)
        //            {
        //                if (Model.ModelName == "SO1C81")
        //                    InspApi[ch].TestCDll2(ch, STATIC.InspMat[ch].Clone(), false, 1, CRes, false);
        //                else
        //                {
        //                    if (Option.is1CH_MC)
        //                        InspApi[ch].TestCDll2(ch, STATIC.InspMat[ch].Clone(), false, 1, CRes, false);
        //                    else
        //                    {
        //                        if (Option.AreaDecenterUse)
        //                            InspApi[ch].NewFineCOG(ch, i, STATIC.InspMat[ch].Clone(), InspectionType.Area_InspAll, CRes, false, true, false, STATIC.SaveImageCurrentPos, STATIC.SaveImageItrCnt + 1);

        //                        else
        //                            InspApi[ch].NewFineCOG(ch, i, STATIC.InspMat[ch].Clone(), InspectionType.InCircle_InspAll, CRes, false, true, false, STATIC.SaveImageCurrentPos, STATIC.SaveImageItrCnt + 1);
        //                    }
        //                }

        //            }


        //        }
        //        else
        //        {
        //            if (Option.BaseOnSpacer)
        //            {
        //                if (Option.CDllUse)
        //                    InspApi[ch].TestCDll2(ch, STATIC.InspMat[ch].Clone(), false, 1, CRes, false);
        //            }
        //            else
        //            {
        //                if (Option.ActroDllUse)
        //                    InspApi[ch].NewFineCOG(ch, i, STATIC.InspMat[ch].Clone(), InspectionType.FindCover, CoverRes, Option.ShowResultToImage, false, false, STATIC.SaveImageCurrentPos, STATIC.SaveImageItrCnt + 1);
        //                if (Option.CDllUse)
        //                    InspApi[ch].TestCDll2(ch, STATIC.InspMat[ch].Clone(), false, 3, CRes, false);
        //            }
        //        }

        //        if (Option.ActroDllUse)
        //        {
        //            if (i <= F20_Index[0] || i >= F20_Index[1])
        //            {

        //                if (Option.AreaDecenterUse)
        //                    InspApi[ch].NewFineCOG(ch, i, STATIC.InspMat[ch].Clone(), InspectionType.Area_InspAll, Res, Option.ShowResultToImage, true, false, STATIC.SaveImageCurrentPos, STATIC.SaveImageItrCnt + 1);

        //                else
        //                    InspApi[ch].NewFineCOG(ch, i, STATIC.InspMat[ch].Clone(), InspectionType.InCircle_InspAll, Res, Option.ShowResultToImage, true, false, STATIC.SaveImageCurrentPos, STATIC.SaveImageItrCnt + 1);
        //            }
        //            else
        //            {
        //                if (Option.AreaDecenterUse)
        //                    InspApi[ch].NewFineCOG(ch, i, STATIC.InspMat[ch].Clone(), InspectionType.Area_InspAll, Res, Option.ShowResultToImage, false, false, STATIC.SaveImageCurrentPos, STATIC.SaveImageItrCnt + 1);

        //                else
        //                    InspApi[ch].NewFineCOG(ch, i, STATIC.InspMat[ch].Clone(), InspectionType.InCircle_InspAll, Res, Option.ShowResultToImage, false, false, STATIC.SaveImageCurrentPos, STATIC.SaveImageItrCnt + 1);
        //            }
        //        }


        //        if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
        //        {

        //            if (Option.BaseOnSpacer)
        //            {
        //                if (i == Open_OpenLoopIndex[0])
        //                {
        //                    STATIC.GapSpacerPos[ch] = new System.Drawing.PointF((float)Res.cx, (float)Res.cy);
        //                    STATIC.C_GapSpacerPos[ch] = new System.Drawing.PointF((float)CRes.cx, (float)CRes.cy);


        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (!Option.BaseOnSpacer)
        //            {

        //                STATIC.GapSpacerPos[ch] = new System.Drawing.PointF((float)CoverRes.Cover_cx, (float)CoverRes.Cover_cy);
        //                STATIC.C_GapSpacerPos[ch] = new System.Drawing.PointF((float)CRes.Cover_cx, (float)CRes.Cover_cy);

        //            }

        //        }

        //        if (!STATIC.isNonSpecError[ch])
        //        {
        //            double X = 0;
        //            double Y = 0;
        //            double C_X = 0;
        //            double C_Y = 0;


        //            if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
        //            {
        //                if(Option.is1CH_MC)
        //                {
                          
        //                    if(STATIC.SaveImageCurrentPos == 1)
        //                    {
        //                        STATIC.DecenterX_Pos1 = X = STATIC.ScaleResolution[ch] * 1000 * (STATIC.CoverPos[ch].X - Res.cx) * DecenterScale;
        //                        STATIC.DecenterY_Pos1 = Y = STATIC.ScaleResolution[ch] * 1000 * (STATIC.CoverPos[ch].Y - Res.cy) * DecenterScale;
        //                    }
        //                    else
        //                    {
        //                        X = STATIC.DecenterX_Pos1;
        //                        Y = STATIC.DecenterY_Pos1;
        //                    }
                         


        //                    if (STATIC.Rcp.Model.ModelName == "SO1G73")
        //                    {
        //                        if(Option.isActMode)
        //                        {
        //                            if (STATIC.SaveImageCurrentPos == 1)
        //                            {
        //                                STATIC.CDecenterX_Pos1 = C_X = 1000 * (STATIC.C_CoverPos[ch].X - CRes.cx);
        //                                STATIC.CDecenterY_Pos1 = C_Y = 1000 * (STATIC.C_CoverPos[ch].Y - CRes.cy);
        //                            }
        //                            else
        //                            {
        //                                C_X = STATIC.CDecenterX_Pos1;
        //                                C_Y = STATIC.CDecenterY_Pos1;
        //                            }
                 
        //                        }
        //                        else
        //                        {
        //                            C_X = 1000 * (STATIC.C_GapSpacerPos[ch].X - (float)CRes.cx);
        //                            C_Y = 1000 * (STATIC.C_GapSpacerPos[ch].Y - (float)CRes.cy);
        //                        }
                         

        //                    }
        //                    else
        //                    {
        //                        if(STATIC.SaveImageCurrentPos == 1)
        //                        {
        //                            STATIC.CDecenterX_Pos1 = C_X = 1000 * (STATIC.C_CoverPos[ch].X - CRes.cx);
        //                            STATIC.CDecenterY_Pos1 = C_Y = 1000 * (STATIC.C_CoverPos[ch].Y - CRes.cy);
        //                        }
        //                        else
        //                        {
        //                            C_X = STATIC.CDecenterX_Pos1;
        //                            C_Y = STATIC.CDecenterY_Pos1;
        //                        }

                               
        //                    }
        //                }
        //                else
        //                {
        //                    X = STATIC.ScaleResolution[ch] * 1000 * (STATIC.CoverPos[ch].X - Res.cx) * DecenterScale;
        //                    Y = STATIC.ScaleResolution[ch] * 1000 * (STATIC.CoverPos[ch].Y - Res.cy) * DecenterScale;


        //                    if (STATIC.Rcp.Model.ModelName == "SO1G73")
        //                    {
        //                        C_X = 1000 * (STATIC.C_GapSpacerPos[ch].X - (float)CRes.cx) * STATIC.ScaleResolution[ch];
        //                        C_Y = 1000 * (STATIC.C_GapSpacerPos[ch].Y - (float)CRes.cy) * STATIC.ScaleResolution[ch];

        //                    }
        //                    else
        //                    {
        //                        C_X = 1000 * (STATIC.C_CoverPos[ch].X - CRes.cx);
        //                        C_Y = 1000 * (STATIC.C_CoverPos[ch].Y - CRes.cy);
        //                    }
        //                }
                  
                   

        //            }
        //            else
        //            {
        //                X = STATIC.ScaleResolution[ch] * 1000 * (STATIC.GapSpacerPos[ch].X - Res.cx);
        //                Y = STATIC.ScaleResolution[ch] * 1000 * (STATIC.GapSpacerPos[ch].Y - Res.cy);
        //                if (Model.ModelName == "SO1C81")
        //                {
        //                    C_X = 1000 * (STATIC.C_GapSpacerPos[ch].X - CRes.cx);
        //                    C_Y = 1000 * (STATIC.C_GapSpacerPos[ch].Y - CRes.cy);
        //                }
        //                else
        //                {
        //                    if (Option.is1CH_MC)
        //                    {
        //                        C_X = 1000 * (STATIC.C_GapSpacerPos[ch].X - CRes.cx);
        //                        C_Y = 1000 * (STATIC.C_GapSpacerPos[ch].Y - CRes.cy);
                            
        //                    }
        //                    else
        //                    {
        //                        C_X = 1000 * (STATIC.C_CoverPos[ch].X - CRes.cx);
        //                        C_Y = 1000 * (STATIC.C_CoverPos[ch].Y - CRes.cy);
        //                    }


        //                }

        //            }

        //            //X = -X;
        //            //Y = -Y;
        //            //C_X = -C_X;
        //            //C_Y = -C_Y;
        //            if(Option.ActroDllUse)
        //            {
        //                if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
        //                {
        //                    if (Option.is1CH_MC && Option.ActOpenAreaOp)
        //                        tmpRes[i].Area = STATIC.F40CoverDia / 2.0 * STATIC.F40CoverDia / 2.0 * Math.PI * STATIC.Rcp.F16Scale.F16Scale;
        //                    else tmpRes[i].Area = Res.Area;
        //                }
        //                else tmpRes[i].Area = Res.Area;
        //                tmpRes[i].DecenterR = Math.Sqrt(X * X + Y * Y);
        //                tmpRes[i].DecenterX = X;
        //                tmpRes[i].DecenterY = Y;
        //                if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
        //                { tmpRes[i].CircleAcuraccy = Res.ShapeAccuracy; }
        //                else { tmpRes[i].CircleAcuraccy = Res.CircleAcuraccy; }

        //                tmpRes[i].ShapeAccuracy = Res.ShapeAccuracy;
        //            }
        //            if(Option.CDllUse)
        //            {
        //                if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
        //                {
        //                    if (Model.ModelName == "SO1C81" || Option.is1CH_MC)
        //                    {
        //                        if (Option.is1CH_MC && Option.ActOpenAreaOp)
        //                            tmpRes[i].CArea = STATIC.F40CoverDia / 2.0 * STATIC.F40CoverDia / 2.0 * Math.PI * STATIC.Rcp.F16Scale.F16Scale;
        //                        else tmpRes[i].CArea = CRes.CArea;
        //                        tmpRes[i].CDecenterR = Math.Sqrt(C_X * C_X + C_Y * C_Y);
        //                        tmpRes[i].CDecenterX = C_X;
        //                        tmpRes[i].CDecenterY = C_Y;
        //                        if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
        //                        { tmpRes[i].CCircleAcuraccy = CRes.CShapeAccuracy * 1000; }
        //                        else { tmpRes[i].CCircleAcuraccy = CRes.CCircleAcuraccy * 1000; }

        //                        tmpRes[i].CShapeAccuracy = CRes.CShapeAccuracy * 1000;
        //                    }
        //                    else
        //                    {
        //                        tmpRes[i].CArea = CRes.Area;
        //                        tmpRes[i].CDecenterR = Math.Sqrt(C_X * C_X + C_Y * C_Y);
        //                        tmpRes[i].CDecenterX = C_X;
        //                        tmpRes[i].CDecenterY = C_Y;
        //                        tmpRes[i].CCircleAcuraccy = CRes.ShapeAccuracy;
        //                        tmpRes[i].CShapeAccuracy = CRes.ShapeAccuracy;
        //                    }
        //                }
        //                else
        //                {
        //                    tmpRes[i].CArea = CRes.CArea;
        //                    tmpRes[i].CDecenterR = Math.Sqrt(C_X * C_X + C_Y * C_Y);
        //                    tmpRes[i].CDecenterX = C_X;
        //                    tmpRes[i].CDecenterY = C_Y;
        //                    if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
        //                    { tmpRes[i].CCircleAcuraccy = CRes.CShapeAccuracy * 1000; }
        //                    else { tmpRes[i].CCircleAcuraccy = CRes.CCircleAcuraccy * 1000; }

        //                    tmpRes[i].CShapeAccuracy = CRes.CShapeAccuracy * 1000;
        //                }
        //            }

                   

        //        }



        //        //Area 측정
        //        if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //        {
        //            if(Option.ActroDllUse)
        //            {
        //                Log.AddLog(ch, String.Format("Target Code {0}, Current {1:0.00}, Read {2}, Area {3:0.00} ",
        //                  tmpRes[i].code, tmpRes[i].current, tmpRes[i].IrisHall, tmpRes[i].Area));
        //            }
        //            if(!Option.ActroDllUse && Option.CDllUse)
        //            {
        //                Log.AddLog(ch, String.Format("Target Code {0}, Current {1:0.00}, Read {2}, Area {3:0.00} ",
        //             tmpRes[i].code, tmpRes[i].current, tmpRes[i].IrisHall, tmpRes[i].CArea));
        //            }
            
        //            //string length = "";
        //            //for (int k = 0; k < PosTest[port][i].lengthPtoC[j].Length; k++)
        //            //    length += string.Format(" {0:0.000} ", PosTest[port][i].lengthPtoC[j][k]);
        //            //if (i != 0 && i != 19)
        //            //    Log.AddLog(j, string.Format("Shape Length [{0}]", length));

        //            Log.AddLog(ch, string.Format("\r\n"));

        //        }
        //        if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
        //        {
        //            if (Option.Pos14OpenLoopUse)
        //            {
        //                if (!STATIC.isNonSpecError[ch])
        //                {
        //                    AK7316_F14OpenLoop(ch, true, false);

        //                }


        //            }

        //        }
            
        //        if (i==Close_OpenLoopIndx[0] || i == Close_OpenLoopIndx[1])
        //        {
        //            if (Option.Pos40OpenLoopUse)
        //            {
        //                if (!STATIC.isNonSpecError[ch])
        //                {
        //                    AK7316_F40OpenLoop(ch, true);

        //                }

        //            }
        //        }
                

        //    }
         
        //    sw.Stop();

        //    if (!STATIC.isNonSpecError[ch])
        //    {
        //        if (Option.ReverseDrv)
        //            DrvIC.Move(ch, DriverIC.AK, Condition.ReadyPosOffset14);
        //        else
        //            DrvIC.Move(ch, DriverIC.AK, 4095 - Condition.ReadyPosOffset14);
        //    }



        //    if(Option.Code1500InspUse)
        //    {
        //        Spec.PassFails[ch].Results[(int)SpecItem.CodeOC1500_Area].Val = tmpRes[1].Area;
        //        Spec.PassFails[ch].Results[(int)SpecItem.CodeOC1500_CArea].Val = tmpRes[1].CArea;
        //        Spec.PassFails[ch].Results[(int)SpecItem.CodeCO1500_Area].Val = tmpRes[tmpRes.Count - 2].Area;
        //        Spec.PassFails[ch].Results[(int)SpecItem.CodeCO1500_CArea].Val = tmpRes[tmpRes.Count - 2].CArea;

        //        Spec.SetResult(ch, (int)SpecItem.CodeOC1500_Area, (int)SpecItem.CodeCO1500_CArea);
        //        ShowEvent.Show(ch, string.Format("{0}", "Code 1500"));

        //        tmpRes.RemoveAt(tmpRes.Count - 2);
        //        tmpRes.RemoveAt(1);
        //    }
        //    PosTest[ch].Clear();
        //    PosTest[ch] = tmpRes.ToList();

        //    if (STATIC.isTmpLog)
        //    {
        //        STATIC.ResolutionLog += string.Format("{0},{1},{2},{3}\r\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"), m_StrIndex[ch],
        //                PosTest[ch][0].CDecenterX.ToString("F3"), PosTest[ch][0].CDecenterY.ToString("F3"));
        //        STATIC.RepeatLog += string.Format("{0},{1},{2},{3}\r\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"), m_StrIndex[ch],
        //              PosTest[ch][0].CDecenterX.ToString("F3"), PosTest[ch][0].CDecenterY.ToString("F3"));
             

        //    }
        //    //  Vision.MilList[port].CommonToReplayBuf(name, PosTest[port].Count);
        //}

   
        private void Process_CalcCodeTest(int ch, string name)
        {
           
            int[] F20_Index = new int[2];
            int[] showIndex = new int[1];



            STATIC.OC_F20_CCircleAccu[ch] = 0;
            STATIC.CO_F20_CCircleAccu[ch] = 0;//new double[2];

            STATIC.OC_F40_CCircleAccu[ch] = 0;// new double[2];
            STATIC.CO_F40_CCircleAccu[ch] = 0;//new double[2];

            STATIC.OC_F20_CDecenter[ch] = 0;// new double[2];
            STATIC.CO_F20_CDecenter[ch] = 0;// new double[2];
      

            if (Option.Step10Use)
            {
                showIndex = new int[20] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };

                F20_Index = new int[2] { 3, 16 };

                if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
                {

                    STATIC.OC_F20_CCircleAccu[ch] = PosTest[ch][3].CCircleAcuraccy;
                    STATIC.CO_F20_CCircleAccu[ch] = PosTest[ch][16].CCircleAcuraccy;


                    STATIC.OC_F40_CCircleAccu[ch] = PosTest[ch][9].CCircleAcuraccy;
                    STATIC.CO_F40_CCircleAccu[ch] = PosTest[ch][10].CCircleAcuraccy;

                    STATIC.OC_F20_CDecenter[ch] = PosTest[ch][3].CDecenterR;
                    STATIC.CO_F20_CDecenter[ch] = PosTest[ch][16].CDecenterR;


                }

                //if (Model.ModelName == "SO1C81")
                //{
    
                //}
                //else if (Model.ModelName == "SO1G73")
                //{
                //    showIndex = new int[18] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 19 };
                 
                //    F20_Index = new int[2] { 2, 15 };
                //    if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
                //    {

                //        STATIC.OC_F20_CCircleAccu[ch] = PosTest[ch][2].CCircleAcuraccy;
                //        STATIC.CO_F20_CCircleAccu[ch] = PosTest[ch][15].CCircleAcuraccy;


                //        STATIC.OC_F40_CCircleAccu[ch] = PosTest[ch][8].CCircleAcuraccy;
                //        STATIC.CO_F40_CCircleAccu[ch] = PosTest[ch][9].CCircleAcuraccy;


                //        STATIC.OC_F20_CDecenter[ch] = PosTest[ch][2].CDecenterR;
                //        STATIC.CO_F20_CDecenter[ch] = PosTest[ch][15].CDecenterR;


                //    }
                //}
            }
            else
            {
                showIndex = new int[8] { 0, 3, 6, 9, 10, 13, 16, 19 };
            
                F20_Index = new int[2] { 1, 6 };
                if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
                {

                    STATIC.OC_F20_CCircleAccu[ch] = PosTest[ch][1].CCircleAcuraccy;
                    STATIC.CO_F20_CCircleAccu[ch] = PosTest[ch][6].CCircleAcuraccy;


                    STATIC.OC_F40_CCircleAccu[ch] = PosTest[ch][3].CCircleAcuraccy;
                    STATIC.CO_F40_CCircleAccu[ch] = PosTest[ch][4].CCircleAcuraccy;


                    STATIC.OC_F20_CDecenter[ch] = PosTest[ch][1].CDecenterR;
                    STATIC.CO_F20_CDecenter[ch] = PosTest[ch][6].CDecenterR;


                }
            }

            if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
            {
                for (int k = 0; k < PosTest[ch].Count; k++)
                {
                    Spec.PassFails[ch].Results[(int)SpecItem.POS1_Hall + 14 * showIndex[k]].Val = PosTest[ch][k].code - PosTest[ch][k].IrisHall;
                    Spec.PassFails[ch].Results[(int)SpecItem.POS1_Area + 14 * showIndex[k]].Val = PosTest[ch][k].Area;
                    Spec.PassFails[ch].Results[(int)SpecItem.POS1_Current + 14 * showIndex[k]].Val = PosTest[ch][k].current;
                    Spec.PassFails[ch].Results[(int)SpecItem.POS1_DecenterR + 14 * showIndex[k]].Val = PosTest[ch][k].DecenterR;
                    Spec.PassFails[ch].Results[(int)SpecItem.POS1_DecenterX + 14 * showIndex[k]].Val = PosTest[ch][k].DecenterX;
                    Spec.PassFails[ch].Results[(int)SpecItem.POS1_DecenterY + 14 * showIndex[k]].Val = PosTest[ch][k].DecenterY;
                    Spec.PassFails[ch].Results[(int)SpecItem.POS1_CircleAccuracy + 14 * showIndex[k]].Val = PosTest[ch][k].CircleAcuraccy;
                    Spec.PassFails[ch].Results[(int)SpecItem.POS1_ShapeAccuracy + 14 * showIndex[k]].Val = PosTest[ch][k].ShapeAccuracy;

                    Spec.PassFails[ch].Results[(int)SpecItem.POS1_CArea + 14 * showIndex[k]].Val = PosTest[ch][k].CArea;
                    Spec.PassFails[ch].Results[(int)SpecItem.POS1_CDecenterR + 14 * showIndex[k]].Val = PosTest[ch][k].CDecenterR;
                    Spec.PassFails[ch].Results[(int)SpecItem.POS1_CDecenterX + 14 * showIndex[k]].Val = PosTest[ch][k].CDecenterX;
                    Spec.PassFails[ch].Results[(int)SpecItem.POS1_CDecenterY + 14 * showIndex[k]].Val = PosTest[ch][k].CDecenterY;
                    Spec.PassFails[ch].Results[(int)SpecItem.POS1_CCircleAccuracy + 14 * showIndex[k]].Val = PosTest[ch][k].CCircleAcuraccy;
                    Spec.PassFails[ch].Results[(int)SpecItem.POS1_CShapeAccuracy + 14 * showIndex[k]].Val = PosTest[ch][k].CShapeAccuracy;
                    Spec.PassFails[ch].Results[(int)SpecItem.POS1_CShapeAccuracy + 14 * showIndex[k]].Val = PosTest[ch][k].CShapeAccuracy;
                    Spec.SetResult(ch, (int)SpecItem.POS1_Hall + 14 * showIndex[k], (int)SpecItem.POS1_CShapeAccuracy + 14 * showIndex[k]);

                    if (Option.Step10Use)
                    {
                        ShowEvent.Show(ch, string.Format("{0}", STATIC.Step10Name[k]));
                        //if (Model.ModelName == "SO1C81")
                           
                        //else if (Model.ModelName == "SO1G73")
                        //    ShowEvent.Show(ch, string.Format("{0}", STATIC.Step9Name[k]));
                    }
                    else
                    {
                        ShowEvent.Show(ch, string.Format("{0}", STATIC.Step4Name_N2[k]));
                        //if (Model.ModelName == "SO1C81")
                            
                        //else if (Model.ModelName == "SO1G73")
                        //    ShowEvent.Show(ch, string.Format("{0}", STATIC.Step4Name_N1[k]));
                    }

                }
               
                Log.AddLog(ch, string.Format("F20 OC Decenter X  = {0 : 0.000}\r\nF20 CO Decenter X = {1 : 0.000}\r\nF20 OC Decenter Y = {2 : 0.000}\r\nF20 CO Decneter Y = {3 : 0.000}",
                PosTest[ch][F20_Index[0]].DecenterX, PosTest[ch][F20_Index[1]].DecenterX, PosTest[ch][F20_Index[0]].DecenterY, PosTest[ch][F20_Index[1]].DecenterY));
                Log.AddLog(ch, string.Format("F20 OC CDecenter X  = {0 : 0.000}\r\nF20 CO CDecenter X = {1 : 0.000}\r\nF20 OC CDecenter Y = {2 : 0.000}\r\nF20 CO CDecneter Y = {3 : 0.000}",
                    PosTest[ch][F20_Index[0]].CDecenterX, PosTest[ch][F20_Index[1]].CDecenterX, PosTest[ch][F20_Index[0]].CDecenterY, PosTest[ch][F20_Index[1]].CDecenterY));

                Spec.PassFails[ch].Results[(int)SpecItem.F20_Decenter_Diff].Val = Math.Sqrt(Math.Pow(PosTest[ch][F20_Index[0]].DecenterX - PosTest[ch][F20_Index[1]].DecenterX, 2) + Math.Pow(PosTest[ch][F20_Index[0]].DecenterY - PosTest[ch][F20_Index[1]].DecenterY, 2));
                Spec.PassFails[ch].Results[(int)SpecItem.F20_CDecnter_Diff].Val = Math.Sqrt(Math.Pow(PosTest[ch][F20_Index[0]].CDecenterX - PosTest[ch][F20_Index[1]].CDecenterX, 2) + Math.Pow(PosTest[ch][F20_Index[0]].CDecenterY - PosTest[ch][F20_Index[1]].CDecenterY, 2));
                Spec.SetResult(ch, (int)SpecItem.F20_Decenter_Diff, (int)SpecItem.F20_CDecnter_Diff);
                ShowEvent.Show(ch, string.Format("{0}", "F20"));

                if (Option.ChartVisible)
                {
                    if (Option.Step10Use)
                    {
                        double[] x = new double[8] { PosTest[ch][0].DecenterX, PosTest[ch][3].DecenterX, PosTest[ch][6].DecenterX, PosTest[ch][9].DecenterX,
                                                    PosTest[ch][10].DecenterX, PosTest[ch][13].DecenterX, PosTest[ch][16].DecenterX, PosTest[ch][19].DecenterX};
                        double[] y = new double[8] { PosTest[ch][0].DecenterY, PosTest[ch][3].DecenterY, PosTest[ch][6].DecenterY, PosTest[ch][9].DecenterY,
                                                    PosTest[ch][10].DecenterY, PosTest[ch][13].DecenterY, PosTest[ch][16].DecenterY, PosTest[ch][19].DecenterY};
                        STATIC.fManage.DrawDecenterChart(ch, x, y);
                        //if (Model.ModelName == "SO1C81")
                        //{
                        //    double[] x = new double[8] { PosTest[ch][0].DecenterX, PosTest[ch][3].DecenterX, PosTest[ch][6].DecenterX, PosTest[ch][9].DecenterX,
                        //                            PosTest[ch][10].DecenterX, PosTest[ch][13].DecenterX, PosTest[ch][16].DecenterX, PosTest[ch][19].DecenterX};
                        //    double[] y = new double[8] { PosTest[ch][0].DecenterY, PosTest[ch][3].DecenterY, PosTest[ch][6].DecenterY, PosTest[ch][9].DecenterY,
                        //                            PosTest[ch][10].DecenterY, PosTest[ch][13].DecenterY, PosTest[ch][16].DecenterY, PosTest[ch][19].DecenterY};
                        //    STATIC.fManage.DrawDecenterChart(ch, x, y);
                        //}
                        //else if (Model.ModelName == "SO1G73")
                        //{
                        //    double[] x = new double[8] { PosTest[ch][0].DecenterX, PosTest[ch][2].DecenterX, PosTest[ch][5].DecenterX, PosTest[ch][8].DecenterX,
                        //                            PosTest[ch][9].DecenterX, PosTest[ch][12].DecenterX, PosTest[ch][15].DecenterX, PosTest[ch][17].DecenterX};
                        //    double[] y = new double[8] { PosTest[ch][0].DecenterY, PosTest[ch][2].DecenterY, PosTest[ch][5].DecenterY, PosTest[ch][8].DecenterY,
                        //                            PosTest[ch][9].DecenterY, PosTest[ch][12].DecenterY, PosTest[ch][15].DecenterY, PosTest[ch][17].DecenterY};
                        //    STATIC.fManage.DrawDecenterChart(ch, x, y);
                        //}

                    }
                    else
                    {
                        double[] x = new double[8] { PosTest[ch][0].DecenterX, PosTest[ch][1].DecenterX, PosTest[ch][2].DecenterX, PosTest[ch][3].DecenterX,
                                                    PosTest[ch][4].DecenterX, PosTest[ch][5].DecenterX, PosTest[ch][6].DecenterX, PosTest[ch][7].DecenterX};
                        double[] y = new double[8] { PosTest[ch][0].DecenterY, PosTest[ch][1].DecenterY, PosTest[ch][2].DecenterY, PosTest[ch][3].DecenterY,
                                                    PosTest[ch][4].DecenterY, PosTest[ch][5].DecenterY, PosTest[ch][6].DecenterY, PosTest[ch][7].DecenterY};
                        STATIC.fManage.DrawDecenterChart(ch, x, y);

                    }

                }
            }
        }


        void Wait(long ms)
        {
            ms = ms * 1000;
            Stopwatch startNew = Stopwatch.StartNew();

            long usDelayTick = (ms * Stopwatch.Frequency) / 1000000;

            while (startNew.ElapsedTicks < usDelayTick) ;
        }

        private void Process_ScanTimeTest(int ch, string name)
        {
            
            int[] HallPosCh1 = new int[8] { PosTest[ch][0].code, PosTest[ch][3].code, PosTest[ch][6].code, PosTest[ch][9].code,
                                            PosTest[ch][10].code, PosTest[ch][13].code, PosTest[ch][16].code, PosTest[ch][19].code};

            int[] HallPosCh2 = new int[8]{ PosTest[ch][0].code, PosTest[ch][3].code, PosTest[ch][6].code, PosTest[ch][9].code,
                                           PosTest[ch][10].code, PosTest[ch][13].code, PosTest[ch][16].code, PosTest[ch][19].code};
            int[] initHall = new int[2];
            int[] MoveHall = new int[2];
            for (int i = 0; i < 6; i++)
            {
             
                if (m_ChannelOn[ch])
                {
                    if( i > 2)
                    {
                        initHall[ch] = HallPosCh1[i + 1];
                        MoveHall[ch] = HallPosCh1[i + 2];
                    }
                    else
                    {
                        initHall[ch] = HallPosCh1[i];
                        MoveHall[ch] = HallPosCh1[i + 1];
                    }
                    Log.AddLog(ch, string.Format(">> Try {0}\t{1}\t{2}", i + 1, initHall[ch], MoveHall[ch]));
                }
                if (m_ChannelOn[ch + 1])
                {
                    if (i > 2)
                    {
                        initHall[ch + 1] = HallPosCh2[i + 1];
                        MoveHall[ch + 1] = HallPosCh2[i + 2];
                    }
                    else
                    {
                        initHall[ch + 1] = HallPosCh2[i];
                        MoveHall[ch + 1] = HallPosCh2[i + 1];
                    }
                    Log.AddLog(ch + 1, string.Format(">> Try {0}\t{1}\t{2}", i + 1, initHall[ch + 1], MoveHall[ch + 1]));
                }

                TimeTest_Ch1[i][ch].Clear();
                TimeTest_Ch2[i][ch].Clear();

                //Time Grab ===============
                Task[] task = new Task[2];
                int index = 0;
                int finishIndex = 0;
                if (i == 0)
                {
                    if(Option.Pos14OpenLoopUse)
                    {
                        if (m_ChannelOn[ch])
                            AK7316_iris_open_OL2(ch, 0);
                        if (m_ChannelOn[ch + 1])
                            AK7316_iris_open_OL2(ch + 1, 0);
                    }
                    else
                    {
                        if (m_ChannelOn[ch])
                            DrvIC.Move(ch, DriverIC.AK, initHall[ch], true);
                        if (m_ChannelOn[ch + 1])
                            DrvIC.Move(ch + 1, DriverIC.AK, initHall[ch + 1], true);
                    }
               
                }
                else
                {
                    if (m_ChannelOn[ch])
                        DrvIC.Move(ch, DriverIC.AK, initHall[ch], true);
                    if (m_ChannelOn[ch + 1])
                        DrvIC.Move(ch + 1, DriverIC.AK, initHall[ch + 1], true);
                }

                Thread.Sleep(300);
                Stopwatch sw = new Stopwatch();
                bool cntStart = false;
          
                if (m_ChannelOn[ch])
                {
                    cntStart = false;
                    index = 0;
                    finishIndex = 0;

                    sw.Reset(); sw.Start();
                    task[0] = Task.Factory.StartNew(() =>
                    {
                        while (true)
                        {
                            TimeTest_Ch1[i][ch].Add(new SettlingData());
                            if (finishIndex >= 100) break;
                            if (sw.ElapsedMilliseconds > 2000) break;
                            TimeTest_Ch1[i][ch][index].Time = (double)sw.ElapsedMilliseconds / 1000.0;
                            TimeTest_Ch1[i][ch][index].ReadHall = DrvIC.ReadHall(ch, DriverIC.AK, true);
                            if (!cntStart)
                            {
                                //if (TimeTest_Ch1[i][ch][index].ReadHall >= Condition.SettlingUnderPer / 100.0)
                                //    cntStart = true;
                            }
                            else
                                finishIndex++;
                            index++;
                            Thread.Sleep(1);

                        }
                    });

                    task[1] = Task.Factory.StartNew(() =>
                    {

                        Thread.Sleep(30);
                        if (Option.Pos14OpenLoopUse)
                        {
                            if (i == 0)
                            {
                                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xA6, 1, new byte[1] { 0x00 });
                                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[1] { 0x00 });
                              
                            }

                            if (i == 5)
                                AK7316_iris_open_OL2(ch, 0, true);
                            else
                                DrvIC.Move(ch, DriverIC.AK, MoveHall[ch], true);
                        }
                        else
                            DrvIC.Move(ch, DriverIC.AK, MoveHall[ch], true);


                        TimeTest_Ch1[i][ch][0].InitialTime = (double)sw.ElapsedMilliseconds / 1000.0;
                        TimeTest_Ch1[i][ch][0].InitCode = initHall[ch];
                        TimeTest_Ch1[i][ch][0].FinalCode = MoveHall[ch];


                    });
                    Task.WaitAll(task);
                    sw.Stop();
                }

                if (m_ChannelOn[ch + 1])
                {
                    cntStart = false;
                    index = 0;
                    finishIndex = 0;
                    sw.Reset(); sw.Start();
                    task[0] = Task.Factory.StartNew(() =>
                    {

                        //  IsScan[port] = true;
                        while (true)
                        {
                            if (finishIndex >= 100) break;
                            if (sw.ElapsedMilliseconds > 2000) break;
                            TimeTest_Ch2[i][ch].Add(new SettlingData());
                            TimeTest_Ch2[i][ch][index].Time = (double)sw.ElapsedMilliseconds / 1000.0;
                            TimeTest_Ch2[i][ch][index].ReadHall = DrvIC.ReadHall(ch + 1, DriverIC.AK, true);
                            if (!cntStart)
                            {
                                //if (TimeTest_Ch2[i][ch][index].ReadHall >= Condition.SettlingUnderPer / 100.0)
                                //    cntStart = true;
                            }
                            else
                                finishIndex++;
                            index++;
                            Thread.Sleep(1);
                           
                        }
                    });

                    task[1] = Task.Factory.StartNew(() =>
                    {

                        Thread.Sleep(30);
                        if (Option.Pos14OpenLoopUse)
                        {
                            if (i == 0)
                            {
                                Dln.WriteArray(ch + 1, DrvIC.AkSlave[ch + 1], 0xA6, 1, new byte[1] { 0x00 });
                                Dln.WriteArray(ch + 1, DrvIC.AkSlave[ch + 1], 0xAE, 1, new byte[1] { 0x00 });

                            }
                            if (i == 5)
                                AK7316_iris_open_OL2(ch + 1, 0, true);
                            else
                                DrvIC.Move(ch + 1, DriverIC.AK, MoveHall[ch + 1], true);
                        }
                        else
                            DrvIC.Move(ch + 1, DriverIC.AK, MoveHall[ch + 1], true);

                        TimeTest_Ch2[i][ch][0].InitialTime = (double)sw.ElapsedMilliseconds / 1000.0;
                        TimeTest_Ch2[i][ch][0].InitCode = initHall[ch + 1];
                        TimeTest_Ch2[i][ch][0].FinalCode = MoveHall[ch + 1];

                    });
                    Task.WaitAll(task);
                    sw.Stop();
                }

           
            }

            if (Option.Pos14OpenLoopUse)
            {
                if (m_ChannelOn[ch])
                {
                    Dln.WriteArray(ch, DrvIC.AkSlave[ch + 1], 0xA6, 1, new byte[1] { 0x00 });
                    Dln.WriteArray(ch, DrvIC.AkSlave[ch + 1], 0xAE, 1, new byte[1] { 0x00 });
                }
                if (m_ChannelOn[ch + 1])
                {
                    Dln.WriteArray(ch + 1, DrvIC.AkSlave[ch + 1], 0xA6, 1, new byte[1] { 0x00 });
                    Dln.WriteArray(ch + 1, DrvIC.AkSlave[ch + 1], 0xAE, 1, new byte[1] { 0x00 });
                }
            }


        }
        private void Process_CalcTimeTest(int port, string name)
        {
            try
            {
                string[] SettlingKey = new string[6] { "F1420", "F2028", "F2840", "F4028", "F2820", "F2014" };
                int ch = port * 2;
                for (int k = 0; k < 6; k++)
                {
                    if (m_ChannelOn[ch])
                    {
                        for (int i = 0; i < TimeTest_Ch1[k][port].Count - 1; i++)
                            TimeTest_Ch1[k][port][i].Time = TimeTest_Ch1[k][port][i].Time - TimeTest_Ch1[k][port][0].InitialTime;
                    }
                    if (m_ChannelOn[ch + 1])
                    {
                        for (int i = 0; i < TimeTest_Ch2[k][port].Count - 1; i++)
                            TimeTest_Ch2[k][port][i].Time = TimeTest_Ch2[k][port][i].Time - TimeTest_Ch2[k][port][0].InitialTime;
                    }


                }
                for (int k = 0; k < 6; k++)
                {

                    double settleHallVal = 0;

                    if (m_ChannelOn[ch])
                    {
                        //if (TimeTest_Ch1[k][port][0].FinalCode - TimeTest_Ch1[k][port][0].InitCode >= 0)
                        //    settleHallVal = TimeTest_Ch1[k][port][0].InitCode + Math.Abs((TimeTest_Ch1[k][port][0].FinalCode - TimeTest_Ch1[k][port][0].InitCode)) * Condition.SettlingUnderPer / 100.0;
                        //else
                        //    settleHallVal = TimeTest_Ch1[k][port][0].InitCode - Math.Abs((TimeTest_Ch1[k][port][0].FinalCode - TimeTest_Ch1[k][port][0].InitCode)) * Condition.SettlingUnderPer / 100.0;

                        for (int i = 0; i < TimeTest_Ch1[k][port].Count - 1; i++)
                        {
                            if (TimeTest_Ch1[k][port][0].FinalCode - TimeTest_Ch1[k][port][0].InitCode >= 0)
                            {
                                if (TimeTest_Ch1[k][port][i].ReadHall >= settleHallVal)
                                {
                                    TimeTest_Ch1[k][port][0].SettlingTime = TimeTest_Ch1[k][port][i].Time;
                                    break;
                                }

                            }
                            else
                            {
                                if (TimeTest_Ch1[k][port][i].ReadHall <= settleHallVal)
                                {
                                    TimeTest_Ch1[k][port][0].SettlingTime = TimeTest_Ch1[k][port][i].Time;
                                    break;
                                }


                            }
                        }

                    }

                    if (m_ChannelOn[ch + 1])
                    {
                        //if (TimeTest_Ch2[k][port][0].FinalCode - TimeTest_Ch2[k][port][0].InitCode >= 0)
                        //    settleHallVal = TimeTest_Ch2[k][port][0].InitCode + Math.Abs((TimeTest_Ch2[k][port][0].FinalCode - TimeTest_Ch2[k][port][0].InitCode)) * Condition.SettlingUnderPer / 100.0;
                        //else
                        //    settleHallVal = TimeTest_Ch2[k][port][0].InitCode - Math.Abs((TimeTest_Ch2[k][port][0].FinalCode - TimeTest_Ch2[k][port][0].InitCode)) * Condition.SettlingUnderPer / 100.0;

                        for (int i = 0; i < TimeTest_Ch2[k][port].Count - 1; i++)
                        {
                            if (TimeTest_Ch2[k][port][0].FinalCode - TimeTest_Ch2[k][port][0].InitCode >= 0)
                            {
                                if (TimeTest_Ch2[k][port][i].ReadHall >= settleHallVal)
                                {
                                    TimeTest_Ch2[k][port][0].SettlingTime = TimeTest_Ch2[k][port][i].Time;
                                    break;
                                }

                            }
                            else
                            {
                                if (TimeTest_Ch2[k][port][i].ReadHall <= settleHallVal)
                                {
                                    TimeTest_Ch2[k][port][0].SettlingTime = TimeTest_Ch2[k][port][i].Time;
                                    break;
                                }
                            }
                        }
                    }


                }

                if (Option.SaveRawData)
                {
                    for (int k = 0; k < 6; k++)
                    {

                        if (m_ChannelOn[ch])
                        {
                            string dateDir = STATIC.CreateDateDir();
                            dateDir += "SettlingData\\";
                            if (!Directory.Exists(dateDir))
                                Directory.CreateDirectory(dateDir);
                            List<string> arry = new List<string>();
                            //   arry.Add(DateTime.Now.ToString("MM:dd:hh:mm:ss"));
                            string path = "";
                            path = string.Format("{0}{1}_{2}_{3}.csv", dateDir, SettlingKey[k], m_StrIndex[ch], k);
                            arry.Add("index,Time,ReadHall");
                            for (int i = 0; i < TimeTest_Ch1[k][port].Count - 1; i++)
                            {
                                string data = string.Format("{0},{1:0.000},{2}", i, TimeTest_Ch1[k][port][i].Time, TimeTest_Ch1[k][port][i].ReadHall);
                                arry.Add(data);
                            }
                            if (path != "") STATIC.SetTextLine(path, arry);
                        }
                        if (m_ChannelOn[ch + 1])
                        {
                            string dateDir = STATIC.CreateDateDir();
                            dateDir += "SettlingData\\";
                            if (!Directory.Exists(dateDir))
                                Directory.CreateDirectory(dateDir);
                            List<string> arry = new List<string>();
                          
                            string path = "";
                            path = string.Format("{0}{1}_{2}_{3}.csv", dateDir, SettlingKey[k], m_StrIndex[ch + 1], k);
                            arry.Add("index,Time,ReadHall");
                            for (int i = 0; i < TimeTest_Ch2[k][port].Count - 1; i++)
                            {
                                string data = string.Format("{0},{1:0.000},{2}", i, TimeTest_Ch2[k][port][i].Time, TimeTest_Ch2[k][port][i].ReadHall);
                                arry.Add(data);
                            }
                            if (path != "") STATIC.SetTextLine(path, arry);

                        }

                    }

                }

                for (int j = 0; j < 2; j++)
                {
                    if (!m_ChannelOn[j]) continue;
                    for (int i = 0; i < 6; i++)
                    {

                        if (j == 0)
                            Spec.PassFails[j].Results[(int)SpecItem.F1420_Settling + i].Val = TimeTest_Ch1[i][port][0].SettlingTime;
                        else
                            Spec.PassFails[j].Results[(int)SpecItem.F1420_Settling + i].Val = TimeTest_Ch2[i][port][0].SettlingTime;
                        Spec.SetResult(ch, (int)SpecItem.F1420_Settling + i, (int)SpecItem.F1420_Settling + i);
                        ShowEvent.Show(j, string.Format("{0}", SettlingKey[i]));
                    }
                    //if (j == 0)
                    //{
                    //    STATIC.fManage.DrawSettlingChart(j, 0, TimeTest_Ch1[0][port].ToList(), TimeTest_Ch1[5][port].ToList(), PosTest[port][0].code[0], PosTest[port][3].code[0], PosTest[port][16].code[0], PosTest[port][19].code[0]);
                    //    STATIC.fManage.DrawSettlingChart(j, 1, TimeTest_Ch1[1][port].ToList(), TimeTest_Ch1[4][port].ToList(), PosTest[port][3].code[0], PosTest[port][6].code[0], PosTest[port][13].code[0], PosTest[port][16].code[0]);
                    //    STATIC.fManage.DrawSettlingChart(j, 2, TimeTest_Ch1[2][port].ToList(), TimeTest_Ch1[3][port].ToList(), PosTest[port][6].code[0], PosTest[port][9].code[0], PosTest[port][10].code[0], PosTest[port][13].code[0]);
                    //}
                    //else
                    //{
                    //    STATIC.fManage.DrawSettlingChart(j, 0, TimeTest_Ch2[0][port].ToList(), TimeTest_Ch2[5][port].ToList(), PosTest[port][0].code[1], PosTest[port][3].code[1], PosTest[port][16].code[1], PosTest[port][19].code[1]);
                    //    STATIC.fManage.DrawSettlingChart(j, 1, TimeTest_Ch2[1][port].ToList(), TimeTest_Ch2[4][port].ToList(), PosTest[port][3].code[1], PosTest[port][6].code[1], PosTest[port][13].code[1], PosTest[port][16].code[1]);
                    //    STATIC.fManage.DrawSettlingChart(j, 2, TimeTest_Ch2[2][port].ToList(), TimeTest_Ch2[3][port].ToList(), PosTest[port][6].code[1], PosTest[port][9].code[1], PosTest[port][10].code[1], PosTest[port][13].code[1]);
                    //}

                }
            }
            catch 
            { 
            
            }
          
          
        }

       


        public void Act_SearchPosition(int ch, string testItem)
        {
            if (!m_ChannelOn[ch])
                return;

            int codeStep = Condition.IRISCalCodeStep;
            int interval = Condition.IRISDrvInterval;
            int codeindex;
            int startCode = 0;
            int endCode = 0;
            int codeRange;

            int[] Open_OpenLoopIndex = new int[2];
            int[] Close_OpenLoopIndx = new int[2];
            int[] F20Index = new int[2];
            int[] F28Index = new int[2];

            int[] Search_Open_OpenLoopIndex = new int[2];
            int[] Search_Close_OpenLoopIndx = new int[2];


            double Linearity = 0;
            List<List<InspResult>> SearchPos = new List<List<InspResult>>();

            SearchPos.Add(new List<InspResult>());
            SearchPos.Add(new List<InspResult>());
            PosTest[ch].Clear();
            double[] CalArea = new double[1];
            double[] SearchCalArea = new double[1];
            List<InspResult> Postest_SearchOp = new List<InspResult>();
            if (Option.Step10Use)
            {
                CalArea = new double[20] {Condition.IRISCalAreaF14, Condition.IRISCalAreaF1416, Condition.IRISCalAreaF1618, Condition.IRISCalAreaF1820,
                                          Condition.IRISCalAreaF2022, Condition.IRISCalAreaF2225, Condition.IRISCalAreaF2528, Condition.IRISCalAreaF2832,
                                          Condition.IRISCalAreaF3235, Condition.IRISCalAreaF3540, Condition.IRISCalAreaF40, Condition.IRISCalAreaF4035,
                                            Condition.IRISCalAreaF3532, Condition.IRISCalAreaF3228, Condition.IRISCalAreaF2825, Condition.IRISCalAreaF2522,
                                            Condition.IRISCalAreaF2220, Condition.IRISCalAreaF2018, Condition.IRISCalAreaF1816, Condition.IRISCalAreaF1614};

                Open_OpenLoopIndex = new int[2] { 0, 19 };
                Close_OpenLoopIndx = new int[2] { 9, 10 };
                F20Index = new int[2] { 3, 16 };
                F28Index = new int[2] { 6, 13 };
                //if (Model.ModelName == "SO1C81")
                //{
                //    CalArea = new double[20] {Condition.IRISCalAreaF14, Condition.IRISCalAreaF1416, Condition.IRISCalAreaF1618, Condition.IRISCalAreaF1820,
                //                          Condition.IRISCalAreaF2022, Condition.IRISCalAreaF2225, Condition.IRISCalAreaF2528, Condition.IRISCalAreaF2832,
                //                          Condition.IRISCalAreaF3235, Condition.IRISCalAreaF3540, Condition.IRISCalAreaF40, Condition.IRISCalAreaF4035,
                //                            Condition.IRISCalAreaF3532, Condition.IRISCalAreaF3228, Condition.IRISCalAreaF2825, Condition.IRISCalAreaF2522,
                //                            Condition.IRISCalAreaF2220, Condition.IRISCalAreaF2018, Condition.IRISCalAreaF1816, Condition.IRISCalAreaF1614};

                //    Open_OpenLoopIndex = new int[2] { 0, 19 };
                //    Close_OpenLoopIndx = new int[2] { 9, 10 };
                //    F20Index = new int[2] { 3, 16 };
                //    F28Index = new int[2] { 6, 13 };
                //}
                //else if (Model.ModelName == "SO1G73")
                //{
                //    CalArea = new double[18] {Condition.IRISCalAreaF14, Condition.IRISCalAreaF1618, Condition.IRISCalAreaF1820,
                //                          Condition.IRISCalAreaF2022, Condition.IRISCalAreaF2225, Condition.IRISCalAreaF2528, Condition.IRISCalAreaF2832,
                //                          Condition.IRISCalAreaF3235, Condition.IRISCalAreaF3540, Condition.IRISCalAreaF40, Condition.IRISCalAreaF4035,
                //                            Condition.IRISCalAreaF3532, Condition.IRISCalAreaF3228, Condition.IRISCalAreaF2825, Condition.IRISCalAreaF2522,
                //                            Condition.IRISCalAreaF2220, Condition.IRISCalAreaF2018,  Condition.IRISCalAreaF1614};

                //    Open_OpenLoopIndex = new int[2] { 0, 17 };
                //    Close_OpenLoopIndx = new int[2] { 8, 9 };
                //    F20Index = new int[2] { 2, 15 };
                //    F28Index = new int[2] { 5, 12 };

                //}
            }
            else
            {
                CalArea = new double[8] {Condition.IRISCalAreaF14, Condition.IRISCalAreaF1820,
                                           Condition.IRISCalAreaF2528, Condition.IRISCalAreaF3540, Condition.IRISCalAreaF40,
                                            Condition.IRISCalAreaF3228, Condition.IRISCalAreaF2220, Condition.IRISCalAreaF1614};
                Open_OpenLoopIndex = new int[2] { 0, 7 };
                Close_OpenLoopIndx = new int[2] { 3, 4 };
                F20Index = new int[2] { 1, 6 };
                F28Index = new int[2] { 2, 5 };
            }

            if (Option.Step10SearchUse)
            {
                SearchCalArea = new double[20] {Condition.IRISCalAreaF14, Condition.IRISCalAreaF1416, Condition.IRISCalAreaF1618, Condition.IRISCalAreaF1820,
                                          Condition.IRISCalAreaF2022, Condition.IRISCalAreaF2225, Condition.IRISCalAreaF2528, Condition.IRISCalAreaF2832,
                                          Condition.IRISCalAreaF3235, Condition.IRISCalAreaF3540, Condition.IRISCalAreaF40, Condition.IRISCalAreaF4035,
                                            Condition.IRISCalAreaF3532, Condition.IRISCalAreaF3228, Condition.IRISCalAreaF2825, Condition.IRISCalAreaF2522,
                                            Condition.IRISCalAreaF2220, Condition.IRISCalAreaF2018, Condition.IRISCalAreaF1816, Condition.IRISCalAreaF1614};

                Search_Open_OpenLoopIndex = new int[2] { 0, 19 };
                Search_Close_OpenLoopIndx = new int[2] { 9, 10 };
                //if (Model.ModelName == "SO1C81")
                //{
                //    SearchCalArea = new double[20] {Condition.IRISCalAreaF14, Condition.IRISCalAreaF1416, Condition.IRISCalAreaF1618, Condition.IRISCalAreaF1820,
                //                          Condition.IRISCalAreaF2022, Condition.IRISCalAreaF2225, Condition.IRISCalAreaF2528, Condition.IRISCalAreaF2832,
                //                          Condition.IRISCalAreaF3235, Condition.IRISCalAreaF3540, Condition.IRISCalAreaF40, Condition.IRISCalAreaF4035,
                //                            Condition.IRISCalAreaF3532, Condition.IRISCalAreaF3228, Condition.IRISCalAreaF2825, Condition.IRISCalAreaF2522,
                //                            Condition.IRISCalAreaF2220, Condition.IRISCalAreaF2018, Condition.IRISCalAreaF1816, Condition.IRISCalAreaF1614};

                //    Search_Open_OpenLoopIndex = new int[2] { 0, 19 };
                //    Search_Close_OpenLoopIndx = new int[2] { 9, 10 };

                //}
                //else if (Model.ModelName == "SO1G73")
                //{
                //    SearchCalArea = new double[18] {Condition.IRISCalAreaF14, Condition.IRISCalAreaF1618, Condition.IRISCalAreaF1820,
                //                          Condition.IRISCalAreaF2022, Condition.IRISCalAreaF2225, Condition.IRISCalAreaF2528, Condition.IRISCalAreaF2832,
                //                          Condition.IRISCalAreaF3235, Condition.IRISCalAreaF3540, Condition.IRISCalAreaF40, Condition.IRISCalAreaF4035,
                //                            Condition.IRISCalAreaF3532, Condition.IRISCalAreaF3228, Condition.IRISCalAreaF2825, Condition.IRISCalAreaF2522,
                //                            Condition.IRISCalAreaF2220, Condition.IRISCalAreaF2018,  Condition.IRISCalAreaF1614};

                //    Search_Open_OpenLoopIndex = new int[2] { 0, 17 };
                //    Search_Close_OpenLoopIndx = new int[2] { 8, 9 };


                //}
            }
            else
            {
                SearchCalArea = new double[8] {Condition.IRISCalAreaF14, Condition.IRISCalAreaF1820,
                                           Condition.IRISCalAreaF2528, Condition.IRISCalAreaF3540, Condition.IRISCalAreaF40,
                                            Condition.IRISCalAreaF3228, Condition.IRISCalAreaF2220, Condition.IRISCalAreaF1614};
                Search_Open_OpenLoopIndex = new int[2] { 0, 7 };
                Search_Close_OpenLoopIndx = new int[2] { 3, 4 };

            }

            for (int i = 0; i < CalArea.Length; i++)
            {
                PosTest[ch].Add(new InspResult());
                if (Option.ReverseDrv)
                {
                    PosTest[ch][i].code = 4095 - Condition.Maximum40Pos;
                    PosTest[ch][i].code = 4095 - Condition.Maximum40Pos;
                }
                else
                {
                    PosTest[ch][i].code = Condition.Maximum40Pos;
                    PosTest[ch][i].code = Condition.Maximum40Pos;
                }

                if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
                {
                    if (Option.ReverseDrv)
                    {
                        PosTest[ch][i].code = Condition.Maximum14Pos;
                        PosTest[ch][i].code = Condition.Maximum14Pos;
                    }
                    else
                    {
                        PosTest[ch][i].code = 4095 - Condition.Maximum14Pos;
                        PosTest[ch][i].code = 4095 - Condition.Maximum14Pos;
                    }
                }
                if (i == Close_OpenLoopIndx[0] || i == Close_OpenLoopIndx[1])
                {
                    if (Option.ReverseDrv)
                    {
                        PosTest[ch][i].code = 4095 - Condition.Maximum40Pos;
                        PosTest[ch][i].code = 4095 - Condition.Maximum40Pos;
                    }
                    else
                    {
                        PosTest[ch][i].code = Condition.Maximum40Pos;
                        PosTest[ch][i].code = Condition.Maximum40Pos;
                    }
                }

            }

            for (int i = 0; i < SearchCalArea.Length; i++)
            {
                Postest_SearchOp.Add(new InspResult());
                if (Option.ReverseDrv)
                {
                    Postest_SearchOp[i].code = 4095 - Condition.Maximum40Pos;
                    Postest_SearchOp[i].code = 4095 - Condition.Maximum40Pos;
                }
                else
                {
                    Postest_SearchOp[i].code = Condition.Maximum40Pos;
                    Postest_SearchOp[i].code = Condition.Maximum40Pos;
                }

                if (i == Search_Open_OpenLoopIndex[0] || i == Search_Open_OpenLoopIndex[1])
                {
                    if (Option.ReverseDrv)
                    {
                        Postest_SearchOp[i].code = Condition.Maximum14Pos;
                        Postest_SearchOp[i].code = Condition.Maximum14Pos;
                    }
                    else
                    {
                        Postest_SearchOp[i].code = 4095 - Condition.Maximum14Pos;
                        Postest_SearchOp[i].code = 4095 - Condition.Maximum14Pos;
                    }
                }
                if (i == Search_Close_OpenLoopIndx[0] || i == Search_Close_OpenLoopIndx[1])
                {
                    if (Option.ReverseDrv)
                    {
                        Postest_SearchOp[i].code = 4095 - Condition.Maximum40Pos;
                        Postest_SearchOp[i].code = 4095 - Condition.Maximum40Pos;
                    }
                    else
                    {
                        Postest_SearchOp[i].code = Condition.Maximum40Pos;
                        Postest_SearchOp[i].code = Condition.Maximum40Pos;
                    }
                }

            }


            int FromCode = Condition.IRISCalArea1PosCode;
            int ToCode = Condition.IRISCalArea1NegCode;

            List<string> arry = new List<string>();
            string path = "";

            string dateDir = STATIC.CreateDateDir();
            dateDir += "SearchPosition\\";
            if (!Directory.Exists(dateDir))
                Directory.CreateDirectory(dateDir);
            path = string.Format("{0}{1}_{2}.csv", dateDir, m_StrIndex[ch], "SearchPosition");

            if (m_ChannelOn[ch])
            {
                Log.AddLog(ch, string.Format("\r\n"));
                Log.AddLog(ch, string.Format(">> Start {0}\t End {1}", FromCode, ToCode));
            }
            //   LEDByChannle(ch, true);

            startCode = FromCode;
            endCode = ToCode;
            codeRange = Math.Abs(startCode - endCode);
            if (codeRange % codeStep == 0)
                codeindex = (codeRange / codeStep) + 1;
            else
                codeindex = (codeRange / codeStep) + 2;
            int[] Targetpos = new int[codeindex];
            int[] targetpos2 = new int[codeindex];

            if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
            {
                //STATIC.COScanLinearityDiff[ch] = double.MaxValue;
                //STATIC.COScanLinearityMax[ch] = double.MinValue;
                //STATIC.OCScanLinearityDiff[ch] = double.MinValue;
                //STATIC.OCScanLinearityMax[ch] = double.MaxValue;

                for (int i = 0; i < Targetpos.Length; i++)
                {
                    string log = "";

                    SearchPos[0].Add(new InspResult());
                    if (Option.ReverseDrv)
                    {
                        Targetpos[i] = endCode - (codeStep * i);
                        if (Targetpos[i] < startCode)
                            Targetpos[i] = startCode;
                    }
                    else
                    {
                        Targetpos[i] = startCode + (codeStep * i);
                        if (Targetpos[i] > endCode)
                            Targetpos[i] = endCode;
                    }
                    DrvIC.Move(ch, DriverIC.AK, Targetpos[i]);
                    Wait(interval);
                    SearchPos[0][i].current = Dln.GetCurrent(ch);
                    SearchPos[0][i].IrisHall = DrvIC.ReadHall(ch, DriverIC.AK);

                    Cam.CamList[ch].Acquire();
                    SearchPos[0][i].Area = InspApi[ch].JustCheckArea(ch, STATIC.InspMat[ch].Clone(), false);


                    Log.AddLog(ch, String.Format("Targetpos {0},current {1:0.00}, IrisHall {2}, area {3:0.00} ", Targetpos[i], SearchPos[0][i].current, SearchPos[0][i].IrisHall, SearchPos[0][i].Area));
                    log += string.Format("{0},{1},{2:0.0000},{3},,", Targetpos[i], SearchPos[0][i].IrisHall, SearchPos[0][i].Area, SearchPos[0][i].current);

                    ////if (i > 0 && i > Condition.ScanLinearExclude && i < Targetpos.Length - Condition.ScanLinearExclude - 1)
                    //{
                    //    double a = SearchPos[0][i].Area - SearchPos[0][i - 1].Area;

                    //    //if (STATIC.COScanLinearityDiff[ch] > a)
                    //    //{
                    //    //    STATIC.COScanLinearityDiff[ch] = a;
                    //    //    STATIC.COScanLinearityDiffHall[ch] = Targetpos[i];
                    //    //}


                    //    //if (STATIC.COScanLinearityMax[ch] < a)
                    //    //{
                    //    //    STATIC.COScanLinearityMax[ch] = a;
                    //    //    STATIC.COScanLinearityMaxHall[ch] = Targetpos[i];
                    //    //}

                    //    if (a < 0 && Math.Abs(a) > Condition.ScanLinearSpec)
                    //    {
                    //        //if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
                    //        //{
                    //        //    m_ChannelOn[ch] = false;
                    //        //    STATIC.isNonSpecError[ch] = true;
                    //        //    errMsg[ch] = "CO Scan Linearity Rev NG";
                    //        //}

                    //    }

                    //    if (a > 0 && Math.Abs(a) > Condition.ScanLinearMaxSpec)
                    //    {
                    //        //if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
                    //        //{
                    //        //    m_ChannelOn[ch] = false;
                    //        //    STATIC.isNonSpecError[ch] = true;
                    //        //    errMsg[ch] = "CO Scan Linearity Max NG";
                    //        //}

                    //    }
                    //}
                    arry.Add(log);

                }


                Array.Copy(Targetpos, targetpos2, Targetpos.Length);
                Array.Reverse(targetpos2);
                arry.Add("\r\n");
                Log.AddLog(ch, "\r\n");

                for (int i = 0; i < targetpos2.Length; i++)
                {
                    string log = "";

                    SearchPos[1].Add(new InspResult());

                    DrvIC.Move(ch, DriverIC.AK, targetpos2[i]);
                    Wait(interval);

                    SearchPos[1][i].current = Dln.GetCurrent(ch);
                    SearchPos[1][i].IrisHall = DrvIC.ReadHall(ch, DriverIC.AK);

                    Cam.CamList[ch].Acquire();
                    SearchPos[1][i].Area = InspApi[ch].JustCheckArea(ch, STATIC.InspMat[ch].Clone(), false);

                    Log.AddLog(ch, String.Format("Targetpos2 {0},current {1:0.00}, IrisHall {2}, area {3:0.00} ", targetpos2[i], SearchPos[1][i].current, SearchPos[1][i].IrisHall, SearchPos[1][i].Area));
                    log += string.Format("{0},{1},{2:0.0000},{3},,", targetpos2[i], SearchPos[1][i].IrisHall, SearchPos[1][i].Area, SearchPos[1][i].current);

                    //if (i > 0 && i > Condition.ScanLinearExclude && i < targetpos2.Length - Condition.ScanLinearExclude - 1)
                    //{
                    //    double a = SearchPos[1][i].Area - SearchPos[1][i - 1].Area;

                    //    //if (STATIC.OCScanLinearityDiff[ch] < a)
                    //    //{
                    //    //    STATIC.OCScanLinearityDiff[ch] = a;
                    //    //    STATIC.OCScanLinearityDiffHall[ch] = targetpos2[i];

                    //    //}
                    //    //if (STATIC.OCScanLinearityMax[ch] > a)
                    //    //{
                    //    //    STATIC.OCScanLinearityMax[ch] = a;
                    //    //    STATIC.OCScanLinearityMaxHall[ch] = targetpos2[i];

                    //    //}
                    //    if (a > 0 && Math.Abs(a) > Condition.ScanLinearSpec)
                    //    {
                    //        //if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
                    //        //{
                    //        //    m_ChannelOn[ch] = false;
                    //        //    STATIC.isNonSpecError[ch] = true;
                    //        //    errMsg[ch] = "OC Scan Linearity Rev NG";
                    //        //}
                    //    }
                    //    if (a < 0 && Math.Abs(a) > Condition.ScanLinearMaxSpec)
                    //    {
                    //        //if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
                    //        //{
                    //        //    m_ChannelOn[ch] = false;
                    //        //    STATIC.isNonSpecError[ch] = true;
                    //        //    errMsg[ch] = "OC Scan Linearity Max NG";
                    //        //}

                    //    }

                    //}
                    arry.Add(log);

                }
                if (path != "" && Option.SaveRawData) STATIC.SetTextLine(path, arry);
                Log.AddLog(ch, "\r\n");
            }

            if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
            {
                STATIC.LastStepArea[ch] = SearchPos[1][targetpos2.Length - 1].Area;
                STATIC.LastStep_1Area[ch] = SearchPos[1][targetpos2.Length - 2].Area;
                STATIC.LastStep_2Area[ch] = SearchPos[1][targetpos2.Length - 3].Area;

                for (int i = 0; i < targetpos2.Length; i++)
                {
                    if (i != 0)
                    {
                        for (int q = 0; q < CalArea.Length / 2; q++)
                        {
                            if (CalArea[q] <= SearchPos[1][i].Area && CalArea[q] >= SearchPos[1][i - 1].Area)
                            {
                                PosTest[ch][q].code = (int)(SearchPos[1][i - 1].IrisHall + (SearchPos[1][i].IrisHall - SearchPos[1][i - 1].IrisHall)
                                    * (CalArea[q] - SearchPos[1][i - 1].Area) / (SearchPos[1][i].Area - SearchPos[1][i - 1].Area));


                                if (q == Open_OpenLoopIndex[0])
                                {
                                    if (Option.ReverseDrv)
                                        PosTest[ch][q].code = Condition.Maximum14Pos;
                                    else
                                        PosTest[ch][q].code = 4095 - Condition.Maximum14Pos;

                                }
                                if (q == Close_OpenLoopIndx[0])
                                {
                                    if (Option.ReverseDrv)
                                    {
                                        if (PosTest[ch][q].code > 4095 - Condition.Maximum40Pos)
                                            PosTest[ch][q].code = 4095 - Condition.Maximum40Pos;
                                    }
                                    else
                                    {

                                        if (PosTest[ch][q].code < Condition.Maximum40Pos)
                                            PosTest[ch][q].code = Condition.Maximum40Pos;
                                    }

                                }



                            }
                            else if (CalArea[q] >= SearchPos[1][i].Area && CalArea[q] <= SearchPos[1][i - 1].Area)
                            {
                                PosTest[ch][q].code = (int)(SearchPos[1][i - 1].IrisHall + (SearchPos[1][i].IrisHall - SearchPos[1][i - 1].IrisHall)
                                    * (CalArea[q] - SearchPos[1][i - 1].Area) / (SearchPos[1][i].Area - SearchPos[1][i - 1].Area));

                                //     Log.AddLog(j, string.Format("Target Area {0}, Search Code = {1}", CalArea[q], PosTest[port][q].code[j]));
                                //if (q == Open_OpenLoopIndex[0])
                                //{
                                //    if (Option.ReverseDrv)
                                //        PosTest[ch][q].code = Condition.Maximum14Pos;
                                //    else
                                //        PosTest[ch][q].code = 4095 - Condition.Maximum14Pos;
                                //}
                                //if (q == Close_OpenLoopIndx[0])
                                //{
                                //    if (Option.ReverseDrv)
                                //    {
                                //        if (PosTest[ch][q].code > 4095 - Condition.Maximum40Pos)
                                //            PosTest[ch][q].code = 4095 - Condition.Maximum40Pos;
                                //    }
                                //    else
                                //    {

                                //        if (PosTest[ch][q].code < Condition.Maximum40Pos)
                                //            PosTest[ch][q].code = Condition.Maximum40Pos;
                                //    }

                                //}

                            }
                        }
                        for (int q = 0; q < SearchCalArea.Length / 2; q++)
                        {
                            if (SearchCalArea[q] <= SearchPos[1][i].Area && SearchCalArea[q] >= SearchPos[1][i - 1].Area)
                            {
                                Postest_SearchOp[q].code = (int)(SearchPos[1][i - 1].IrisHall + (SearchPos[1][i].IrisHall - SearchPos[1][i - 1].IrisHall)
                                    * (SearchCalArea[q] - SearchPos[1][i - 1].Area) / (SearchPos[1][i].Area - SearchPos[1][i - 1].Area));


                                //if (q == Search_Open_OpenLoopIndex[0])
                                //{
                                //    if (Option.ReverseDrv)
                                //        Postest_SearchOp[q].code = Condition.Maximum14Pos;
                                //    else
                                //        Postest_SearchOp[q].code = 4095 - Condition.Maximum14Pos;

                                //}
                                //if (q == Search_Close_OpenLoopIndx[0])
                                //{
                                //    if (Option.ReverseDrv)
                                //    {
                                //        if (Postest_SearchOp[q].code > 4095 - Condition.Maximum40Pos)
                                //            Postest_SearchOp[q].code = 4095 - Condition.Maximum40Pos;
                                //    }
                                //    else
                                //    {

                                //        if (Postest_SearchOp[q].code < Condition.Maximum40Pos)
                                //            Postest_SearchOp[q].code = Condition.Maximum40Pos;
                                //    }

                                //}



                            }
                            else if (SearchCalArea[q] >= SearchPos[1][i].Area && SearchCalArea[q] <= SearchPos[1][i - 1].Area)
                            {
                                Postest_SearchOp[q].code = (int)(SearchPos[1][i - 1].IrisHall + (SearchPos[1][i].IrisHall - SearchPos[1][i - 1].IrisHall)
                                    * (SearchCalArea[q] - SearchPos[1][i - 1].Area) / (SearchPos[1][i].Area - SearchPos[1][i - 1].Area));

                                //     Log.AddLog(j, string.Format("Target Area {0}, Search Code = {1}", CalArea[q], PosTest[port][q].code[j]));
                                //if (q == Search_Open_OpenLoopIndex[0])
                                //{
                                //    if (Option.ReverseDrv)
                                //        Postest_SearchOp[q].code = Condition.Maximum14Pos;
                                //    else
                                //        Postest_SearchOp[q].code = 4095 - Condition.Maximum14Pos;
                                //}
                                //if (q == Search_Close_OpenLoopIndx[0])
                                //{
                                //    if (Option.ReverseDrv)
                                //    {
                                //        if (Postest_SearchOp[q].code > 4095 - Condition.Maximum40Pos)
                                //            Postest_SearchOp[q].code = 4095 - Condition.Maximum40Pos;
                                //    }
                                //    else
                                //    {

                                //        if (Postest_SearchOp[q].code < Condition.Maximum40Pos)
                                //            Postest_SearchOp[q].code = Condition.Maximum40Pos;
                                //    }

                                //}

                            }
                        }
                    }
                }
                for (int i = 0; i < Targetpos.Length; i++)
                {
                    if (i != 0)
                    {
                        for (int q = Close_OpenLoopIndx[1]; q < CalArea.Length; q++)
                        {
                            if (CalArea[q] <= SearchPos[0][i].Area && CalArea[q] >= SearchPos[0][i - 1].Area)
                            {
                                PosTest[ch][q].code = (int)(SearchPos[0][i - 1].IrisHall + (SearchPos[0][i].IrisHall - SearchPos[0][i - 1].IrisHall)
                                    * (CalArea[q] - SearchPos[0][i - 1].Area) / (SearchPos[0][i].Area - SearchPos[0][i - 1].Area));

                                //  Log.AddLog(j, string.Format("Target Area {0}, Search Code = {1}", CalArea[q], PosTest[port][q].code[j]));
                                //if (q == Open_OpenLoopIndex[1])
                                //{
                                //    if (Option.ReverseDrv)
                                //        PosTest[ch][q].code = Condition.Maximum14Pos;
                                //    else
                                //        PosTest[ch][q].code = 4095 - Condition.Maximum14Pos;
                                //}
                                //if (q == Close_OpenLoopIndx[1])
                                //{
                                //    if (Option.ReverseDrv)
                                //    {
                                //        if (PosTest[ch][q].code > 4095 - Condition.Maximum40Pos)
                                //            PosTest[ch][q].code = 4095 - Condition.Maximum40Pos;
                                //    }
                                //    else
                                //    {

                                //        if (PosTest[ch][q].code < Condition.Maximum40Pos)
                                //            PosTest[ch][q].code = Condition.Maximum40Pos;
                                //    }

                                //}

                            }
                            else if (CalArea[q] >= SearchPos[0][i].Area && CalArea[q] <= SearchPos[0][i - 1].Area)
                            {
                                PosTest[ch][q].code = (int)(SearchPos[0][i - 1].IrisHall + (SearchPos[0][i].IrisHall - SearchPos[0][i - 1].IrisHall)
                                    * (CalArea[q] - SearchPos[0][i - 1].Area) / (SearchPos[1][i].Area - SearchPos[0][i - 1].Area));

                                //   Log.AddLog(j, string.Format("Target Area {0}, Search Code = {1}", CalArea[q], PosTest[port][q].code[j]));
                                //if (q == Open_OpenLoopIndex[1])
                                //{
                                //    if (Option.ReverseDrv)
                                //        PosTest[ch][q].code = Condition.Maximum14Pos;
                                //    else
                                //        PosTest[ch][q].code = 4095 - Condition.Maximum14Pos;
                                //}
                                //if (q == Close_OpenLoopIndx[1])
                                //{
                                //    if (Option.ReverseDrv)
                                //    {
                                //        if (PosTest[ch][q].code > 4095 - Condition.Maximum40Pos)
                                //            PosTest[ch][q].code = 4095 - Condition.Maximum40Pos;
                                //    }
                                //    else
                                //    {

                                //        if (PosTest[ch][q].code < Condition.Maximum40Pos)
                                //            PosTest[ch][q].code = Condition.Maximum40Pos;
                                //    }

                                //}

                            }
                        }
                        for (int q = Search_Close_OpenLoopIndx[1]; q < SearchCalArea.Length; q++)
                        {
                            if (SearchCalArea[q] <= SearchPos[0][i].Area && SearchCalArea[q] >= SearchPos[0][i - 1].Area)
                            {
                                Postest_SearchOp[q].code = (int)(SearchPos[0][i - 1].IrisHall + (SearchPos[0][i].IrisHall - SearchPos[0][i - 1].IrisHall)
                                    * (SearchCalArea[q] - SearchPos[0][i - 1].Area) / (SearchPos[0][i].Area - SearchPos[0][i - 1].Area));

                                //  Log.AddLog(j, string.Format("Target Area {0}, Search Code = {1}", CalArea[q], PosTest[port][q].code[j]));
                                //if (q == Search_Open_OpenLoopIndex[1])
                                //{
                                //    if (Option.ReverseDrv)
                                //        Postest_SearchOp[q].code = Condition.Maximum14Pos;
                                //    else
                                //        Postest_SearchOp[q].code = 4095 - Condition.Maximum14Pos;
                                //}
                                //if (q == Search_Close_OpenLoopIndx[1])
                                //{
                                //    if (Option.ReverseDrv)
                                //    {
                                //        if (Postest_SearchOp[q].code > 4095 - Condition.Maximum40Pos)
                                //            Postest_SearchOp[q].code = 4095 - Condition.Maximum40Pos;
                                //    }
                                //    else
                                //    {

                                //        if (Postest_SearchOp[q].code < Condition.Maximum40Pos)
                                //            Postest_SearchOp[q].code = Condition.Maximum40Pos;
                                //    }

                                //}

                            }
                            else if (SearchCalArea[q] >= SearchPos[0][i].Area && SearchCalArea[q] <= SearchPos[0][i - 1].Area)
                            {
                                Postest_SearchOp[q].code = (int)(SearchPos[0][i - 1].IrisHall + (SearchPos[0][i].IrisHall - SearchPos[0][i - 1].IrisHall)
                                    * (SearchCalArea[q] - SearchPos[0][i - 1].Area) / (SearchPos[1][i].Area - SearchPos[0][i - 1].Area));

                                //   Log.AddLog(j, string.Format("Target Area {0}, Search Code = {1}", CalArea[q], PosTest[port][q].code[j]));
                                //if (q == Search_Open_OpenLoopIndex[1])
                                //{
                                //    if (Option.ReverseDrv)
                                //        Postest_SearchOp[q].code = Condition.Maximum14Pos;
                                //    else
                                //        Postest_SearchOp[q].code = 4095 - Condition.Maximum14Pos;
                                //}
                                //if (q == Search_Close_OpenLoopIndx[1])
                                //{
                                //    if (Option.ReverseDrv)
                                //    {
                                //        if (Postest_SearchOp[q].code > 4095 - Condition.Maximum40Pos)
                                //            Postest_SearchOp[q].code = 4095 - Condition.Maximum40Pos;
                                //    }
                                //    else
                                //    {

                                //        if (Postest_SearchOp[q].code < Condition.Maximum40Pos)
                                //            Postest_SearchOp[q].code = Condition.Maximum40Pos;
                                //    }

                                //}

                            }
                        }
                    }
                }


                Log.AddLog(ch, string.Format("Find Position"));
                for (int i = 0; i < CalArea.Length; i++)
                    Log.AddLog(ch, string.Format("Target Area {0}, Search Code = {1}", CalArea[i], PosTest[ch][i].code));
                Log.AddLog(ch, "\r\n");

                for (int i = 0; i < SearchCalArea.Length; i++)
                    Log.AddLog(ch, string.Format("Target Area {0}, Search Code(Search Op) = {1}", SearchCalArea[i], Postest_SearchOp[i].code));


                //if (Model.ModelName == "SO1G73")
                //{

                //    if (Option.Step10SearchUse)
                //    {

                //        STATIC.OCHallDiff[ch] = Math.Abs(Postest_SearchOp[6].code - Postest_SearchOp[7].code);
                //        Log.AddLog(ch, string.Format("OC Hall Diff = {0}", STATIC.OCHallDiff[ch]));
                //        STATIC.COHallDiff[ch] = Math.Abs(Postest_SearchOp[10].code - Postest_SearchOp[11].code);
                //        Log.AddLog(ch, string.Format("CO Hall Diff = {0}", STATIC.COHallDiff[ch]));
                //        //if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
                //        //{
                //        //    if (Math.Abs(Postest_SearchOp[6].code - Postest_SearchOp[7].code) <= Condition.SearchHallDiffError)
                //        //    {
                //        //        m_ChannelOn[ch] = false;
                //        //        STATIC.isNonSpecError[ch] = true;
                //        //        errMsg[ch] = testItem + " Hall Diff Error";
                //        //    }
                //        //    if (Math.Abs(Postest_SearchOp[10].code - Postest_SearchOp[11].code) <= Condition.SearchHallDiffError)
                //        //    {
                //        //        m_ChannelOn[ch] = false;
                //        //        STATIC.isNonSpecError[ch] = true;
                //        //        errMsg[ch] = testItem + " Hall Diff Error";
                //        //    }
                //        //}
                //    }
                //}

                if (Condition.SearchOption == 1)
                {
                    Log.AddLog(ch, string.Format("Search Option 1"));
                    for (int i = 0; i < CalArea.Length; i++)
                    {
                        Log.AddLog(ch, string.Format("Target Area {0}, Search Code = {1}", CalArea[i], PosTest[ch][i].code));
                    }
                }
                else if (Condition.SearchOption == 2)
                {
                    Log.AddLog(ch, string.Format("Search Option 2"));

                    for (int i = 0; i < CalArea.Length / 2; i++)
                        PosTest[ch][i].code = PosTest[ch][CalArea.Length - 1 - i].code = (PosTest[ch][i].code + PosTest[ch][CalArea.Length - 1 - i].code) / 2;


                    for (int i = 0; i < CalArea.Length; i++)
                        Log.AddLog(ch, string.Format("Target Area {0}, Search Code = {1}", CalArea[i], PosTest[ch][i].code));

                }
                else if (Condition.SearchOption == 3)
                {

                    Log.AddLog(ch, string.Format("Search Option 3"));
                    for (int i = 0; i < CalArea.Length / 2; i++)
                        PosTest[ch][0].code = PosTest[ch][CalArea.Length - 1 - i].code;

                    for (int i = 0; i < CalArea.Length; i++)
                    {
                        Log.AddLog(ch, string.Format("Target Area {0}, Search Code = {1}", CalArea[i], PosTest[ch][i].code));
                    }
                }
                else if (Condition.SearchOption == 4)
                {
                    Log.AddLog(ch, string.Format("Search Option 4"));
                    for (int i = 0; i < CalArea.Length / 2; i++)
                    {
                        if (i == Close_OpenLoopIndx[0])
                            PosTest[ch][i].code = PosTest[ch][CalArea.Length - 1 - i].code = (int)(PosTest[ch][CalArea.Length - 1 - i].code + ((PosTest[ch][i].code - PosTest[ch][CalArea.Length - 1 - i].code) * Condition.SearchOption4Percent2 / 100));
                        else PosTest[ch][i].code = PosTest[ch][CalArea.Length - 1 - i].code = (int)(PosTest[ch][CalArea.Length - 1 - i].code + ((PosTest[ch][i].code - PosTest[ch][CalArea.Length - 1 - i].code) * Condition.SearchOption4Percent / 100));

                    }


                    for (int i = 0; i < CalArea.Length; i++)
                    {
                        if (Option.ReverseDrv)
                        {
                            if (PosTest[ch][i].code > 4095 - Condition.Maximum40Pos)
                                PosTest[ch][i].code = 4095 - Condition.Maximum40Pos;

                        }
                        else
                        {
                            if (PosTest[ch][i].code > 4095 - Condition.Maximum14Pos)
                                PosTest[ch][i].code = 4095 - Condition.Maximum14Pos;
                        }

                    }


                    //if (Model.ModelName == "SO1G73")
                    //{
                    //    for (int i = 0; i < CalArea.Length; i++)
                    //    {
                    //        if (i == F20Index[1] || i == F28Index[1])
                    //            PosTest[ch][i].code = PosTest[ch][i].code + Condition.F20_28Offset;
                    //    }
                    //}



                    for (int i = 0; i < CalArea.Length; i++)
                    {
                        Log.AddLog(ch, string.Format("Target Area {0}, Search Code = {1}", CalArea[i], PosTest[ch][i].code));
                    }

                }

            }

            if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
            {
                //if (Model.ModelName == "SO1C81")
                //{
                //    //for (int i = 0; i < PosTest[ch].Count; i++)
                //    //{
                //    //    if (!m_ChannelOn[ch] || STATIC.isNonSpecError[ch])
                //    //        break;
                //    //    if (i != 0 && i != Close_OpenLoopIndx[1])
                //    //    {
                //    //        if (Math.Abs(PosTest[ch][i - 1].code - PosTest[ch][i].code) <= Condition.SearchHallDiffError)
                //    //        {
                //    //            m_ChannelOn[ch] = false;
                //    //            STATIC.isNonSpecError[ch] = true;
                //    //            errMsg[ch] = testItem + " Hall Diff Error";
                //    //        }
                //    //    }

                //    //}
                //}

                if (Option.Step10Use)
                {
                    //if (Model.ModelName == "SO1C81")
                    //{
                    //    if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
                    //    {
                    //        if (PosTest[ch][1].code <= Condition.F16F18CodeLimit || PosTest[ch][18].code <= Condition.F16F18CodeLimit)
                    //        {
                    //            m_ChannelOn[ch] = false;
                    //            STATIC.isNonSpecError[ch] = true;
                    //            errMsg[ch] = testItem + " F16 Code Limit Error";
                    //        }
                    //        else if (PosTest[ch][9].code <= Condition.F40CodeLimit || PosTest[ch][10].code <= Condition.F40CodeLimit)
                    //        {
                    //            m_ChannelOn[ch] = false;
                    //            STATIC.isNonSpecError[ch] = true;
                    //            errMsg[ch] = testItem + " F40 Code Limit Error";
                    //        }
                    //    }


                    //}
                    //else if (Model.ModelName == "SO1G73")
                    //{
                    //    if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
                    //    {
                    //        if (PosTest[ch][1].code <= Condition.F16F18CodeLimit || PosTest[ch][16].code <= Condition.F16F18CodeLimit)
                    //        {
                    //            m_ChannelOn[ch] = false;
                    //            STATIC.isNonSpecError[ch] = true;
                    //            errMsg[ch] = testItem + " F18 Code Limit Error";
                    //        }
                    //        else if (PosTest[ch][8].code <= Condition.F40CodeLimit || PosTest[ch][9].code <= Condition.F40CodeLimit)
                    //        {
                    //            m_ChannelOn[ch] = false;
                    //            STATIC.isNonSpecError[ch] = true;
                    //            errMsg[ch] = testItem + " F40 Code Limit Error";
                    //        }
                    //    }

                    //}
                }
                else
                {
                    if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
                    {
                        //if (PosTest[ch][3].code <= Condition.F40CodeLimit || PosTest[ch][4].code <= Condition.F40CodeLimit)
                        //{
                        //    m_ChannelOn[ch] = false;
                        //    STATIC.isNonSpecError[ch] = true;
                        //    errMsg[ch] = testItem + " F40 Code Limit Error";
                        //}
                    }

                }
            }

            InspResult res = new InspResult();
            InspResult res2 = new InspResult();


            Cam.CamList[ch].Acquire();
            InspApi[ch].NewFineCOG(ch, 0, STATIC.InspMat[ch].Clone(), InspectionType.FindCover, res, false, false, false);

            //if (Option.ActroDllUse)
             
            //InspApi[ch].TestCDll2(ch, STATIC.InspMat[ch].Clone(), false, 2, res2, false);



            STATIC.F40CoverDia = res2.CCover_dia;

            //if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
            //{
            //    STATIC.CoverPos[ch] = new System.Drawing.PointF((float)(res.Cover_cx), (float)(res.Cover_cy));
            //    STATIC.C_CoverPos[ch] = new System.Drawing.PointF((float)(res2.Cover_cx), (float)(res2.Cover_cy));
            //}

            if (Option.isReadTargetVal)
            {

                byte[] ReadArr = new byte[12];
                int[] ReadCode = new int[8];

                byte[] rbuf = new byte[1];
                if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
                {
                    for (int i = 0; i < 12; i++)
                    {
                        DrvIC.Dln.ReadArray(ch, DrvIC.AkSlave[ch], 0xEC + i, 1, rbuf);
                        ReadArr[i] = rbuf[0];

                    }
                }
                if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
                {
                    ReadCode[0] = (ReadArr[0] << 4) | ((ReadArr[1] >> 4) & 0x0F); // F14 OC
                    ReadCode[1] = ((ReadArr[1] & 0x0f) << 8) | ReadArr[2]; // F20 OC
                    ReadCode[2] = (ReadArr[3] << 4) | ((ReadArr[4] >> 4) & 0x0F); // F28 OC
                    ReadCode[3] = ((ReadArr[4] & 0x0f) << 8) | ReadArr[5]; // F40 OC

                    ReadCode[4] = (ReadArr[6] << 4) | ((ReadArr[7] >> 4) & 0x0F); // F14 CO
                    ReadCode[5] = ((ReadArr[7] & 0x0f) << 8) | ReadArr[8]; // F20 CO
                    ReadCode[6] = (ReadArr[9] << 4) | ((ReadArr[10] >> 4) & 0x0F); // F28 CO
                    ReadCode[7] = ((ReadArr[10] & 0x0f) << 8) | ReadArr[11]; // F40 CO




                    //ReadCode[0] = ReadArr[0] << 4 | ReadArr[1] >> 4 & 0xff; //2528, 
                    //ReadCode[1] = (ReadArr[1] << 4 & 0xff) << 4 | ReadArr[2]; //3540, 40
                    //ReadCode[2] = ReadArr[3] << 4 | ReadArr[4] >> 4 & 0xff; //3228
                    //ReadCode[3] = (ReadArr[4] << 4 & 0xff) << 4 | ReadArr[5]; //2220
                    //ReadCode[4] = ReadArr[6] << 4 | ReadArr[7] >> 4 & 0xff; //1614, 14
                    //ReadCode[5] = (ReadArr[7] << 4 & 0xff) << 4 | ReadArr[8]; // 1820


                    if (Option.Step10Use)
                    {
                        PosTest[ch][6].code = ReadCode[0]; //2528
                        PosTest[ch][9].code = ReadCode[1]; //3540
                        PosTest[ch][10].code = ReadCode[1]; //40
                        PosTest[ch][13].code = ReadCode[2]; //3228
                        PosTest[ch][16].code = ReadCode[3]; //2220
                        PosTest[ch][19].code = ReadCode[4]; //1614
                        PosTest[ch][0].code = ReadCode[4]; //14
                        PosTest[ch][3].code = ReadCode[5]; //1820
                        //if (Model.ModelName == "SO1C81")
                        //{
                            
                        //}
                        //else if (Model.ModelName == "SO1G73")
                        //{
                        //    PosTest[ch][5].code = ReadCode[0]; //2528
                        //    PosTest[ch][8].code = ReadCode[1]; //3540
                        //    PosTest[ch][9].code = ReadCode[1]; //40
                        //    PosTest[ch][12].code = ReadCode[2]; //3228
                        //    PosTest[ch][15].code = ReadCode[3]; //2220
                        //    PosTest[ch][17].code = ReadCode[4]; //1817
                        //    PosTest[ch][0].code = ReadCode[4]; //17
                        //    PosTest[ch][2].code = ReadCode[5]; //1820
                        //}
                    }
                    else
                    {
                        PosTest[ch][0].code = ReadCode[0]; 
                        PosTest[ch][1].code = ReadCode[1];
                        PosTest[ch][2].code = ReadCode[2];
                        PosTest[ch][3].code = ReadCode[3];
                        PosTest[ch][4].code = ReadCode[7];
                        PosTest[ch][5].code = ReadCode[6];
                        PosTest[ch][6].code = ReadCode[5];
                        PosTest[ch][7].code = ReadCode[4];
                       
                    }

                    Log.AddLog(ch, String.Format("OQC, Re Setting Search Pos..."));

                    for (int j = 0; j < PosTest[ch].Count; j++)
                        Log.AddLog(ch, String.Format("Read Target Code = {0}", PosTest[ch][j].code));
                }

            }

            if (Option.ChartVisible)
            {
                if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
                {
                    double[] data = new double[SearchPos[0].Count];
                    double[] data2 = new double[SearchPos[1].Count];

                    for (int i = 0; i < SearchPos[0].Count; i++)
                    {
                        data[i] = SearchPos[0][i].Area;
                        data2[i] = SearchPos[1][i].Area;
                    }
                    STATIC.fManage.DrawLinearityChart(ch, Targetpos, data, targetpos2, data2);
                }

            }

            if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
            {
                DriverIC.sPoint[] FwdPoint = new DriverIC.sPoint[Targetpos.Length];
                for (int i = 0; i < Targetpos.Length; i++)
                {
                    FwdPoint[i].x = Targetpos[i];
                    FwdPoint[i].y = SearchPos[0][i].Area;
                }

                DriverIC.sPoint[] BwdPoint = new DriverIC.sPoint[targetpos2.Length];
                for (int i = 0; i < targetpos2.Length; i++)
                {
                    BwdPoint[i].x = targetpos2[i];
                    BwdPoint[i].y = SearchPos[1][i].Area;
                }
                DriverIC.sLine LinRes = DrvIC.Line_fitting(FwdPoint, Targetpos.Length);
                DriverIC.sLine LinRes2 = DrvIC.Line_fitting(BwdPoint, targetpos2.Length);

                double b = LinRes.dSlope;         //   FWD Sensitivity
                double c = LinRes.dYintercept;

                double d = LinRes2.dSlope;         //   FWD Sensitivity
                double e = LinRes2.dYintercept;
                double newS = 0;
                for (int i = 0; i < Targetpos.Length; i++)
                {

                    newS = b * Targetpos[i] + c;
                    if (Linearity < Math.Abs(newS - SearchPos[0][i].Area))
                        Linearity = Math.Abs(newS - SearchPos[0][i].Area);
                }

                for (int i = 0; i < targetpos2.Length; i++)
                {

                    newS = d * targetpos2[i] + e;
                    if (Linearity < Math.Abs(newS - SearchPos[1][i].Area))
                        Linearity = Math.Abs(newS - SearchPos[1][i].Area);
                }
                Spec.PassFails[ch].Results[(int)SpecItem.Search_Linearity].Val = Linearity;

                Spec.SetResult(ch, (int)SpecItem.Search_Linearity, (int)SpecItem.Search_Linearity);
                ShowEvent.Show(ch, string.Format("{0}", "Linearity"));
            }

            //    LEDByChannle(ch, false);
        }
        //public void Act_SearchPosition(int ch, string testItem)
        //{
        //    if (!m_ChannelOn[ch])
        //        return;

        //    int codeStep = Condition.IRISCalCodeStep;
        //    int interval = Condition.IRISDrvInterval;
        //    int codeindex;
        //    int startCode = 0;
        //    int endCode = 0;
        //    int codeRange;

        //    int[] Open_OpenLoopIndex = new int[2];
        //    int[] Close_OpenLoopIndx = new int[2];
        //    int[] F20Index = new int[2];
        //    int[] F28Index = new int[2];

        //    int[] Search_Open_OpenLoopIndex = new int[2];
        //    int[] Search_Close_OpenLoopIndx = new int[2];


        //    double Linearity = 0;
        //    List<List<InspResult>> SearchPos = new List<List<InspResult>>();

        //    SearchPos.Add(new List<InspResult>());
        //    SearchPos.Add(new List<InspResult>());
        //    PosTest[ch].Clear();
        //    double[] CalArea = new double[1];
        //    double[] SearchCalArea = new double[1];
        //    List<InspResult> Postest_SearchOp = new List<InspResult>();
        //    if (Option.Step10Use)
        //    {
        //        if (Model.ModelName == "SO1C81")
        //        {
        //            CalArea = new double[20] {Condition.IRISCalAreaF14, Condition.IRISCalAreaF1416, Condition.IRISCalAreaF1618, Condition.IRISCalAreaF1820,
        //                                  Condition.IRISCalAreaF2022, Condition.IRISCalAreaF2225, Condition.IRISCalAreaF2528, Condition.IRISCalAreaF2832,
        //                                  Condition.IRISCalAreaF3235, Condition.IRISCalAreaF3540, Condition.IRISCalAreaF40, Condition.IRISCalAreaF4035,
        //                                    Condition.IRISCalAreaF3532, Condition.IRISCalAreaF3228, Condition.IRISCalAreaF2825, Condition.IRISCalAreaF2522,
        //                                    Condition.IRISCalAreaF2220, Condition.IRISCalAreaF2018, Condition.IRISCalAreaF1816, Condition.IRISCalAreaF1614};

        //            Open_OpenLoopIndex = new int[2] { 0, 19 };
        //            Close_OpenLoopIndx = new int[2] { 9, 10 };
        //            F20Index = new int[2] { 3, 16 };
        //            F28Index = new int[2] { 6, 13 };
        //        }
        //        else if (Model.ModelName == "SO1G73")
        //        {
        //            CalArea = new double[18] {Condition.IRISCalAreaF14, Condition.IRISCalAreaF1618, Condition.IRISCalAreaF1820,
        //                                  Condition.IRISCalAreaF2022, Condition.IRISCalAreaF2225, Condition.IRISCalAreaF2528, Condition.IRISCalAreaF2832,
        //                                  Condition.IRISCalAreaF3235, Condition.IRISCalAreaF3540, Condition.IRISCalAreaF40, Condition.IRISCalAreaF4035,
        //                                    Condition.IRISCalAreaF3532, Condition.IRISCalAreaF3228, Condition.IRISCalAreaF2825, Condition.IRISCalAreaF2522,
        //                                    Condition.IRISCalAreaF2220, Condition.IRISCalAreaF2018,  Condition.IRISCalAreaF1614};

        //            Open_OpenLoopIndex = new int[2] { 0, 17 };
        //            Close_OpenLoopIndx = new int[2] { 8, 9 };
        //            F20Index = new int[2] { 2, 15 };
        //            F28Index = new int[2] { 5, 12 };

        //        }
        //    }
        //    else
        //    {
        //        CalArea = new double[8] {Condition.IRISCalAreaF14, Condition.IRISCalAreaF1820,
        //                                   Condition.IRISCalAreaF2528, Condition.IRISCalAreaF3540, Condition.IRISCalAreaF40,
        //                                    Condition.IRISCalAreaF3228, Condition.IRISCalAreaF2220, Condition.IRISCalAreaF1614};
        //        Open_OpenLoopIndex = new int[2] { 0, 7 };
        //        Close_OpenLoopIndx = new int[2] { 3, 4 };
        //        F20Index = new int[2] { 1, 6 };
        //        F28Index = new int[2] { 2, 5 };
        //    }

        //    if (Option.Step10SearchUse)
        //    {
        //        if (Model.ModelName == "SO1C81")
        //        {
        //            SearchCalArea = new double[20] {Condition.IRISCalAreaF14, Condition.IRISCalAreaF1416, Condition.IRISCalAreaF1618, Condition.IRISCalAreaF1820,
        //                                  Condition.IRISCalAreaF2022, Condition.IRISCalAreaF2225, Condition.IRISCalAreaF2528, Condition.IRISCalAreaF2832,
        //                                  Condition.IRISCalAreaF3235, Condition.IRISCalAreaF3540, Condition.IRISCalAreaF40, Condition.IRISCalAreaF4035,
        //                                    Condition.IRISCalAreaF3532, Condition.IRISCalAreaF3228, Condition.IRISCalAreaF2825, Condition.IRISCalAreaF2522,
        //                                    Condition.IRISCalAreaF2220, Condition.IRISCalAreaF2018, Condition.IRISCalAreaF1816, Condition.IRISCalAreaF1614};

        //            Search_Open_OpenLoopIndex = new int[2] { 0, 19 };
        //            Search_Close_OpenLoopIndx = new int[2] { 9, 10 };

        //        }
        //        else if (Model.ModelName == "SO1G73")
        //        {
        //            SearchCalArea = new double[18] {Condition.IRISCalAreaF14, Condition.IRISCalAreaF1618, Condition.IRISCalAreaF1820,
        //                                  Condition.IRISCalAreaF2022, Condition.IRISCalAreaF2225, Condition.IRISCalAreaF2528, Condition.IRISCalAreaF2832,
        //                                  Condition.IRISCalAreaF3235, Condition.IRISCalAreaF3540, Condition.IRISCalAreaF40, Condition.IRISCalAreaF4035,
        //                                    Condition.IRISCalAreaF3532, Condition.IRISCalAreaF3228, Condition.IRISCalAreaF2825, Condition.IRISCalAreaF2522,
        //                                    Condition.IRISCalAreaF2220, Condition.IRISCalAreaF2018,  Condition.IRISCalAreaF1614};

        //            Search_Open_OpenLoopIndex = new int[2] { 0, 17 };
        //            Search_Close_OpenLoopIndx = new int[2] { 8, 9 };


        //        }
        //    }
        //    else
        //    {
        //        SearchCalArea = new double[8] {Condition.IRISCalAreaF14, Condition.IRISCalAreaF1820,
        //                                   Condition.IRISCalAreaF2528, Condition.IRISCalAreaF3540, Condition.IRISCalAreaF40,
        //                                    Condition.IRISCalAreaF3228, Condition.IRISCalAreaF2220, Condition.IRISCalAreaF1614};
        //        Search_Open_OpenLoopIndex = new int[2] { 0, 7 };
        //        Search_Close_OpenLoopIndx = new int[2] { 3, 4 };

        //    }



        //    for (int i = 0; i < CalArea.Length; i++)
        //    {
        //        PosTest[ch].Add(new InspResult());
        //        if (Option.ReverseDrv)
        //        {
        //            PosTest[ch][i].code = 4095 - Condition.Maximum40Pos;
        //            PosTest[ch][i].code = 4095 - Condition.Maximum40Pos;
        //        }
        //        else
        //        {
        //            PosTest[ch][i].code = Condition.Maximum40Pos;
        //            PosTest[ch][i].code = Condition.Maximum40Pos;
        //        }

        //        if (i == Open_OpenLoopIndex[0] || i == Open_OpenLoopIndex[1])
        //        {
        //            if (Option.ReverseDrv)
        //            {
        //                PosTest[ch][i].code = Condition.Maximum14Pos;
        //                PosTest[ch][i].code = Condition.Maximum14Pos;
        //            }
        //            else
        //            {
        //                PosTest[ch][i].code = 4095 - Condition.Maximum14Pos;
        //                PosTest[ch][i].code = 4095 - Condition.Maximum14Pos;
        //            }
        //        }
        //        if (i == Close_OpenLoopIndx[0] || i == Close_OpenLoopIndx[1])
        //        {
        //            if (Option.ReverseDrv)
        //            {
        //                PosTest[ch][i].code = 4095 - Condition.Maximum40Pos;
        //                PosTest[ch][i].code = 4095 - Condition.Maximum40Pos;
        //            }
        //            else
        //            {
        //                PosTest[ch][i].code = Condition.Maximum40Pos;
        //                PosTest[ch][i].code = Condition.Maximum40Pos;
        //            }
        //        }

        //    }

        //    for (int i = 0; i < SearchCalArea.Length; i++)
        //    {
        //        Postest_SearchOp.Add(new InspResult());
        //        if (Option.ReverseDrv)
        //        {
        //            Postest_SearchOp[i].code = 4095 - Condition.Maximum40Pos;
        //            Postest_SearchOp[i].code = 4095 - Condition.Maximum40Pos;
        //        }
        //        else
        //        {
        //            Postest_SearchOp[i].code = Condition.Maximum40Pos;
        //            Postest_SearchOp[i].code = Condition.Maximum40Pos;
        //        }

        //        if (i == Search_Open_OpenLoopIndex[0] || i == Search_Open_OpenLoopIndex[1])
        //        {
        //            if (Option.ReverseDrv)
        //            {
        //                Postest_SearchOp[i].code = Condition.Maximum14Pos;
        //                Postest_SearchOp[i].code = Condition.Maximum14Pos;
        //            }
        //            else
        //            {
        //                Postest_SearchOp[i].code = 4095 - Condition.Maximum14Pos;
        //                Postest_SearchOp[i].code = 4095 - Condition.Maximum14Pos;
        //            }
        //        }
        //        if (i == Search_Close_OpenLoopIndx[0] || i == Search_Close_OpenLoopIndx[1])
        //        {
        //            if (Option.ReverseDrv)
        //            {
        //                Postest_SearchOp[i].code = 4095 - Condition.Maximum40Pos;
        //                Postest_SearchOp[i].code = 4095 - Condition.Maximum40Pos;
        //            }
        //            else
        //            {
        //                Postest_SearchOp[i].code = Condition.Maximum40Pos;
        //                Postest_SearchOp[i].code = Condition.Maximum40Pos;
        //            }
        //        }

        //    }


        //    int FromCode = Condition.IRISCalArea1PosCode;
        //    int ToCode = Condition.IRISCalArea1NegCode;

        //    List<string> arry = new List<string>();
        //    string path = "";

        //    string dateDir = STATIC.CreateDateDir();
        //    dateDir += "SearchPosition\\";
        //    if (!Directory.Exists(dateDir))
        //        Directory.CreateDirectory(dateDir);
        //    path = string.Format("{0}{1}_{2}.csv", dateDir, m_StrIndex[ch], "SearchPosition");

        //    if (m_ChannelOn[ch])
        //    {
        //        Log.AddLog(ch, string.Format("\r\n"));
        //        Log.AddLog(ch, string.Format(">> Start {0}\t End {1}", FromCode, ToCode));
        //    }
        //    //   LEDByChannle(ch, true);

        //    startCode = FromCode;
        //    endCode = ToCode;
        //    codeRange = Math.Abs(startCode - endCode);
        //    if (codeRange % codeStep == 0)
        //        codeindex = (codeRange / codeStep) + 1;
        //    else
        //        codeindex = (codeRange / codeStep) + 2;
        //    int[] Targetpos = new int[codeindex];
        //    int[] targetpos2 = new int[codeindex];

        //    if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //    {
        //        STATIC.COScanLinearityDiff[ch] = double.MaxValue;
        //        STATIC.COScanLinearityMax[ch] = double.MinValue;
        //        STATIC.OCScanLinearityDiff[ch] = double.MinValue;
        //        STATIC.OCScanLinearityMax[ch] = double.MaxValue;

        //        for (int i = 0; i < Targetpos.Length; i++)
        //        {
        //            string log = "";

        //            SearchPos[0].Add(new InspResult());
        //            if (Option.ReverseDrv)
        //            {
        //                Targetpos[i] = endCode - (codeStep * i);
        //                if (Targetpos[i] < startCode)
        //                    Targetpos[i] = startCode;
        //            }
        //            else
        //            {
        //                Targetpos[i] = startCode + (codeStep * i);
        //                if (Targetpos[i] > endCode)
        //                    Targetpos[i] = endCode;
        //            }
        //            DrvIC.Move(ch, DriverIC.AK, Targetpos[i]);
        //            Wait(interval);
        //            SearchPos[0][i].current = Dln.GetCurrent(ch);
        //            SearchPos[0][i].IrisHall = DrvIC.ReadHall(ch, DriverIC.AK);

        //            Cam.CamList[ch].Acquire();
        //            SearchPos[0][i].Area = InspApi[ch].JustCheckArea(ch, STATIC.InspMat[ch].Clone(), false);


        //            Log.AddLog(ch, String.Format("Targetpos {0},current {1:0.00}, IrisHall {2}, area {3:0.00} ", Targetpos[i], SearchPos[0][i].current, SearchPos[0][i].IrisHall, SearchPos[0][i].Area));
        //            log += string.Format("{0},{1},{2:0.0000},{3},,", Targetpos[i], SearchPos[0][i].IrisHall, SearchPos[0][i].Area, SearchPos[0][i].current);

        //            if (i > 0 && i > Condition.ScanLinearExclude && i < Targetpos.Length - Condition.ScanLinearExclude - 1)
        //            {
        //                double a = SearchPos[0][i].Area - SearchPos[0][i - 1].Area;

        //                if (STATIC.COScanLinearityDiff[ch] > a)
        //                {
        //                    STATIC.COScanLinearityDiff[ch] = a;
        //                    STATIC.COScanLinearityDiffHall[ch] = Targetpos[i];
        //                }


        //                if (STATIC.COScanLinearityMax[ch] < a)
        //                {
        //                    STATIC.COScanLinearityMax[ch] = a;
        //                    STATIC.COScanLinearityMaxHall[ch] = Targetpos[i];
        //                }

        //                if (a < 0 && Math.Abs(a) > Condition.ScanLinearSpec)
        //                {
        //                    if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //                    {
        //                        m_ChannelOn[ch] = false;
        //                        STATIC.isNonSpecError[ch] = true;
        //                        errMsg[ch] = "CO Scan Linearity Rev NG";
        //                    }

        //                }

        //                if (a > 0 && Math.Abs(a) > Condition.ScanLinearMaxSpec)
        //                {
        //                    if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //                    {
        //                        m_ChannelOn[ch] = false;
        //                        STATIC.isNonSpecError[ch] = true;
        //                        errMsg[ch] = "CO Scan Linearity Max NG";
        //                    }

        //                }
        //            }
        //            arry.Add(log);

        //        }


        //        Array.Copy(Targetpos, targetpos2, Targetpos.Length);
        //        Array.Reverse(targetpos2);
        //        arry.Add("\r\n");
        //        Log.AddLog(ch, "\r\n");

        //        for (int i = 0; i < targetpos2.Length; i++)
        //        {
        //            string log = "";

        //            SearchPos[1].Add(new InspResult());

        //            DrvIC.Move(ch, DriverIC.AK, targetpos2[i]);
        //            Wait(interval);

        //            SearchPos[1][i].current = Dln.GetCurrent(ch);
        //            SearchPos[1][i].IrisHall = DrvIC.ReadHall(ch, DriverIC.AK);

        //            Cam.CamList[ch].Acquire();
        //            SearchPos[1][i].Area = InspApi[ch].JustCheckArea(ch, STATIC.InspMat[ch].Clone(), false);

        //            Log.AddLog(ch, String.Format("Targetpos2 {0},current {1:0.00}, IrisHall {2}, area {3:0.00} ", targetpos2[i], SearchPos[1][i].current, SearchPos[1][i].IrisHall, SearchPos[1][i].Area));
        //            log += string.Format("{0},{1},{2:0.0000},{3},,", targetpos2[i], SearchPos[1][i].IrisHall, SearchPos[1][i].Area, SearchPos[1][i].current);

        //            if (i > 0 && i > Condition.ScanLinearExclude && i < targetpos2.Length - Condition.ScanLinearExclude - 1)
        //            {
        //                double a = SearchPos[1][i].Area - SearchPos[1][i - 1].Area;

        //                if (STATIC.OCScanLinearityDiff[ch] < a)
        //                {
        //                    STATIC.OCScanLinearityDiff[ch] = a;
        //                    STATIC.OCScanLinearityDiffHall[ch] = targetpos2[i];

        //                }
        //                if (STATIC.OCScanLinearityMax[ch] > a)
        //                {
        //                    STATIC.OCScanLinearityMax[ch] = a;
        //                    STATIC.OCScanLinearityMaxHall[ch] = targetpos2[i];

        //                }
        //                if (a > 0 && Math.Abs(a) > Condition.ScanLinearSpec)
        //                {
        //                    if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //                    {
        //                        m_ChannelOn[ch] = false;
        //                        STATIC.isNonSpecError[ch] = true;
        //                        errMsg[ch] = "OC Scan Linearity Rev NG";
        //                    }
        //                }
        //                if (a < 0 && Math.Abs(a) > Condition.ScanLinearMaxSpec)
        //                {
        //                    if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //                    {
        //                        m_ChannelOn[ch] = false;
        //                        STATIC.isNonSpecError[ch] = true;
        //                        errMsg[ch] = "OC Scan Linearity Max NG";
        //                    }


        //                }

        //            }
        //            arry.Add(log);

        //        }
        //        if (path != "" && Option.SaveRawData) STATIC.SetTextLine(path, arry);
        //        Log.AddLog(ch, "\r\n");
        //    }

        //    if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //    {
        //        STATIC.LastStepArea[ch] = SearchPos[1][targetpos2.Length - 1].Area;
        //        STATIC.LastStep_1Area[ch] = SearchPos[1][targetpos2.Length - 2].Area;
        //        STATIC.LastStep_2Area[ch] = SearchPos[1][targetpos2.Length - 3].Area;

        //        for (int i = 0; i < targetpos2.Length; i++)
        //        {
        //            if (i != 0)
        //            {
        //                for (int q = 0; q < CalArea.Length / 2; q++)
        //                {
        //                    if (CalArea[q] <= SearchPos[1][i].Area && CalArea[q] >= SearchPos[1][i - 1].Area)
        //                    {
        //                        PosTest[ch][q].code = (int)(SearchPos[1][i - 1].IrisHall + (SearchPos[1][i].IrisHall - SearchPos[1][i - 1].IrisHall)
        //                            * (CalArea[q] - SearchPos[1][i - 1].Area) / (SearchPos[1][i].Area - SearchPos[1][i - 1].Area));


        //                        if (q == Open_OpenLoopIndex[0])
        //                        {
        //                            if (Option.ReverseDrv)
        //                                PosTest[ch][q].code = Condition.Maximum14Pos;
        //                            else
        //                                PosTest[ch][q].code = 4095 - Condition.Maximum14Pos;

        //                        }
        //                        if (q == Close_OpenLoopIndx[0])
        //                        {
        //                            if (Option.ReverseDrv)
        //                            {
        //                                if (PosTest[ch][q].code > 4095 - Condition.Maximum40Pos)
        //                                    PosTest[ch][q].code = 4095 - Condition.Maximum40Pos;
        //                            }
        //                            else
        //                            {

        //                                if (PosTest[ch][q].code < Condition.Maximum40Pos)
        //                                    PosTest[ch][q].code = Condition.Maximum40Pos;
        //                            }

        //                        }



        //                    }
        //                    else if (CalArea[q] >= SearchPos[1][i].Area && CalArea[q] <= SearchPos[1][i - 1].Area)
        //                    {
        //                        PosTest[ch][q].code = (int)(SearchPos[1][i - 1].IrisHall + (SearchPos[1][i].IrisHall - SearchPos[1][i - 1].IrisHall)
        //                            * (CalArea[q] - SearchPos[1][i - 1].Area) / (SearchPos[1][i].Area - SearchPos[1][i - 1].Area));

        //                        //     Log.AddLog(j, string.Format("Target Area {0}, Search Code = {1}", CalArea[q], PosTest[port][q].code[j]));
        //                        if (q == Open_OpenLoopIndex[0])
        //                        {
        //                            if (Option.ReverseDrv)
        //                                PosTest[ch][q].code = Condition.Maximum14Pos;
        //                            else
        //                                PosTest[ch][q].code = 4095 - Condition.Maximum14Pos;
        //                        }
        //                        if (q == Close_OpenLoopIndx[0])
        //                        {
        //                            if (Option.ReverseDrv)
        //                            {
        //                                if (PosTest[ch][q].code > 4095 - Condition.Maximum40Pos)
        //                                    PosTest[ch][q].code = 4095 - Condition.Maximum40Pos;
        //                            }
        //                            else
        //                            {

        //                                if (PosTest[ch][q].code < Condition.Maximum40Pos)
        //                                    PosTest[ch][q].code = Condition.Maximum40Pos;
        //                            }

        //                        }

        //                    }
        //                }
        //                for (int q = 0; q < SearchCalArea.Length / 2; q++)
        //                {
        //                    if (SearchCalArea[q] <= SearchPos[1][i].Area && SearchCalArea[q] >= SearchPos[1][i - 1].Area)
        //                    {
        //                        Postest_SearchOp[q].code = (int)(SearchPos[1][i - 1].IrisHall + (SearchPos[1][i].IrisHall - SearchPos[1][i - 1].IrisHall)
        //                            * (SearchCalArea[q] - SearchPos[1][i - 1].Area) / (SearchPos[1][i].Area - SearchPos[1][i - 1].Area));


        //                        if (q == Search_Open_OpenLoopIndex[0])
        //                        {
        //                            if (Option.ReverseDrv)
        //                                Postest_SearchOp[q].code = Condition.Maximum14Pos;
        //                            else
        //                                Postest_SearchOp[q].code = 4095 - Condition.Maximum14Pos;

        //                        }
        //                        if (q == Search_Close_OpenLoopIndx[0])
        //                        {
        //                            if (Option.ReverseDrv)
        //                            {
        //                                if (Postest_SearchOp[q].code > 4095 - Condition.Maximum40Pos)
        //                                    Postest_SearchOp[q].code = 4095 - Condition.Maximum40Pos;
        //                            }
        //                            else
        //                            {

        //                                if (Postest_SearchOp[q].code < Condition.Maximum40Pos)
        //                                    Postest_SearchOp[q].code = Condition.Maximum40Pos;
        //                            }

        //                        }



        //                    }
        //                    else if (SearchCalArea[q] >= SearchPos[1][i].Area && SearchCalArea[q] <= SearchPos[1][i - 1].Area)
        //                    {
        //                        Postest_SearchOp[q].code = (int)(SearchPos[1][i - 1].IrisHall + (SearchPos[1][i].IrisHall - SearchPos[1][i - 1].IrisHall)
        //                            * (SearchCalArea[q] - SearchPos[1][i - 1].Area) / (SearchPos[1][i].Area - SearchPos[1][i - 1].Area));

        //                        //     Log.AddLog(j, string.Format("Target Area {0}, Search Code = {1}", CalArea[q], PosTest[port][q].code[j]));
        //                        if (q == Search_Open_OpenLoopIndex[0])
        //                        {
        //                            if (Option.ReverseDrv)
        //                                Postest_SearchOp[q].code = Condition.Maximum14Pos;
        //                            else
        //                                Postest_SearchOp[q].code = 4095 - Condition.Maximum14Pos;
        //                        }
        //                        if (q == Search_Close_OpenLoopIndx[0])
        //                        {
        //                            if (Option.ReverseDrv)
        //                            {
        //                                if (Postest_SearchOp[q].code > 4095 - Condition.Maximum40Pos)
        //                                    Postest_SearchOp[q].code = 4095 - Condition.Maximum40Pos;
        //                            }
        //                            else
        //                            {

        //                                if (Postest_SearchOp[q].code < Condition.Maximum40Pos)
        //                                    Postest_SearchOp[q].code = Condition.Maximum40Pos;
        //                            }

        //                        }

        //                    }
        //                }
        //            }
        //        }
        //        for (int i = 0; i < Targetpos.Length; i++)
        //        {
        //            if (i != 0)
        //            {
        //                for (int q = Close_OpenLoopIndx[1]; q < CalArea.Length; q++)
        //                {
        //                    if (CalArea[q] <= SearchPos[0][i].Area && CalArea[q] >= SearchPos[0][i - 1].Area)
        //                    {
        //                        PosTest[ch][q].code = (int)(SearchPos[0][i - 1].IrisHall + (SearchPos[0][i].IrisHall - SearchPos[0][i - 1].IrisHall)
        //                            * (CalArea[q] - SearchPos[0][i - 1].Area) / (SearchPos[0][i].Area - SearchPos[0][i - 1].Area));

        //                        //  Log.AddLog(j, string.Format("Target Area {0}, Search Code = {1}", CalArea[q], PosTest[port][q].code[j]));
        //                        if (q == Open_OpenLoopIndex[1])
        //                        {
        //                            if (Option.ReverseDrv)
        //                                PosTest[ch][q].code = Condition.Maximum14Pos;
        //                            else
        //                                PosTest[ch][q].code = 4095 - Condition.Maximum14Pos;
        //                        }
        //                        if (q == Close_OpenLoopIndx[1])
        //                        {
        //                            if (Option.ReverseDrv)
        //                            {
        //                                if (PosTest[ch][q].code > 4095 - Condition.Maximum40Pos)
        //                                    PosTest[ch][q].code = 4095 - Condition.Maximum40Pos;
        //                            }
        //                            else
        //                            {

        //                                if (PosTest[ch][q].code < Condition.Maximum40Pos)
        //                                    PosTest[ch][q].code = Condition.Maximum40Pos;
        //                            }

        //                        }

        //                    }
        //                    else if (CalArea[q] >= SearchPos[0][i].Area && CalArea[q] <= SearchPos[0][i - 1].Area)
        //                    {
        //                        PosTest[ch][q].code = (int)(SearchPos[0][i - 1].IrisHall + (SearchPos[0][i].IrisHall - SearchPos[0][i - 1].IrisHall)
        //                            * (CalArea[q] - SearchPos[0][i - 1].Area) / (SearchPos[1][i].Area - SearchPos[0][i - 1].Area));

        //                        //   Log.AddLog(j, string.Format("Target Area {0}, Search Code = {1}", CalArea[q], PosTest[port][q].code[j]));
        //                        if (q == Open_OpenLoopIndex[1])
        //                        {
        //                            if (Option.ReverseDrv)
        //                                PosTest[ch][q].code = Condition.Maximum14Pos;
        //                            else
        //                                PosTest[ch][q].code = 4095 - Condition.Maximum14Pos;
        //                        }
        //                        if (q == Close_OpenLoopIndx[1])
        //                        {
        //                            if (Option.ReverseDrv)
        //                            {
        //                                if (PosTest[ch][q].code > 4095 - Condition.Maximum40Pos)
        //                                    PosTest[ch][q].code = 4095 - Condition.Maximum40Pos;
        //                            }
        //                            else
        //                            {

        //                                if (PosTest[ch][q].code < Condition.Maximum40Pos)
        //                                    PosTest[ch][q].code = Condition.Maximum40Pos;
        //                            }

        //                        }

        //                    }
        //                }
        //                for (int q = Search_Close_OpenLoopIndx[1]; q < SearchCalArea.Length; q++)
        //                {
        //                    if (SearchCalArea[q] <= SearchPos[0][i].Area && SearchCalArea[q] >= SearchPos[0][i - 1].Area)
        //                    {
        //                        Postest_SearchOp[q].code = (int)(SearchPos[0][i - 1].IrisHall + (SearchPos[0][i].IrisHall - SearchPos[0][i - 1].IrisHall)
        //                            * (SearchCalArea[q] - SearchPos[0][i - 1].Area) / (SearchPos[0][i].Area - SearchPos[0][i - 1].Area));

        //                        //  Log.AddLog(j, string.Format("Target Area {0}, Search Code = {1}", CalArea[q], PosTest[port][q].code[j]));
        //                        if (q == Search_Open_OpenLoopIndex[1])
        //                        {
        //                            if (Option.ReverseDrv)
        //                                Postest_SearchOp[q].code = Condition.Maximum14Pos;
        //                            else
        //                                Postest_SearchOp[q].code = 4095 - Condition.Maximum14Pos;
        //                        }
        //                        if (q == Search_Close_OpenLoopIndx[1])
        //                        {
        //                            if (Option.ReverseDrv)
        //                            {
        //                                if (Postest_SearchOp[q].code > 4095 - Condition.Maximum40Pos)
        //                                    Postest_SearchOp[q].code = 4095 - Condition.Maximum40Pos;
        //                            }
        //                            else
        //                            {

        //                                if (Postest_SearchOp[q].code < Condition.Maximum40Pos)
        //                                    Postest_SearchOp[q].code = Condition.Maximum40Pos;
        //                            }

        //                        }

        //                    }
        //                    else if (SearchCalArea[q] >= SearchPos[0][i].Area && SearchCalArea[q] <= SearchPos[0][i - 1].Area)
        //                    {
        //                        Postest_SearchOp[q].code = (int)(SearchPos[0][i - 1].IrisHall + (SearchPos[0][i].IrisHall - SearchPos[0][i - 1].IrisHall)
        //                            * (SearchCalArea[q] - SearchPos[0][i - 1].Area) / (SearchPos[1][i].Area - SearchPos[0][i - 1].Area));

        //                        //   Log.AddLog(j, string.Format("Target Area {0}, Search Code = {1}", CalArea[q], PosTest[port][q].code[j]));
        //                        if (q == Search_Open_OpenLoopIndex[1])
        //                        {
        //                            if (Option.ReverseDrv)
        //                                Postest_SearchOp[q].code = Condition.Maximum14Pos;
        //                            else
        //                                Postest_SearchOp[q].code = 4095 - Condition.Maximum14Pos;
        //                        }
        //                        if (q == Search_Close_OpenLoopIndx[1])
        //                        {
        //                            if (Option.ReverseDrv)
        //                            {
        //                                if (Postest_SearchOp[q].code > 4095 - Condition.Maximum40Pos)
        //                                    Postest_SearchOp[q].code = 4095 - Condition.Maximum40Pos;
        //                            }
        //                            else
        //                            {

        //                                if (Postest_SearchOp[q].code < Condition.Maximum40Pos)
        //                                    Postest_SearchOp[q].code = Condition.Maximum40Pos;
        //                            }

        //                        }

        //                    }
        //                }
        //            }
        //        }


        //        Log.AddLog(ch, string.Format("Find Position"));
        //        for (int i = 0; i < CalArea.Length; i++)
        //            Log.AddLog(ch, string.Format("Target Area {0}, Search Code = {1}", CalArea[i], PosTest[ch][i].code));
        //        Log.AddLog(ch, "\r\n");

        //        for (int i = 0; i < SearchCalArea.Length; i++)
        //            Log.AddLog(ch, string.Format("Target Area {0}, Search Code(Search Op) = {1}", SearchCalArea[i], Postest_SearchOp[i].code));


        //        if (Model.ModelName == "SO1G73")
        //        {

        //            if (Option.Step10SearchUse)
        //            {

        //                STATIC.OCHallDiff[ch] = Math.Abs(Postest_SearchOp[6].code - Postest_SearchOp[7].code);
        //                Log.AddLog(ch, string.Format("OC Hall Diff = {0}", STATIC.OCHallDiff[ch]));
        //                STATIC.COHallDiff[ch] = Math.Abs(Postest_SearchOp[10].code - Postest_SearchOp[11].code);
        //                Log.AddLog(ch, string.Format("CO Hall Diff = {0}", STATIC.COHallDiff[ch]));
        //                if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //                {
        //                    if (Math.Abs(Postest_SearchOp[6].code - Postest_SearchOp[7].code) <= Condition.SearchHallDiffError)
        //                    {
        //                        m_ChannelOn[ch] = false;
        //                        STATIC.isNonSpecError[ch] = true;
        //                        errMsg[ch] = testItem + " Hall Diff Error";
        //                    }
        //                    if (Math.Abs(Postest_SearchOp[10].code - Postest_SearchOp[11].code) <= Condition.SearchHallDiffError)
        //                    {
        //                        m_ChannelOn[ch] = false;
        //                        STATIC.isNonSpecError[ch] = true;
        //                        errMsg[ch] = testItem + " Hall Diff Error";
        //                    }
        //                }
        //            }
        //        }

        //        if (Condition.SearchOption == 1)
        //        {
        //            Log.AddLog(ch, string.Format("Search Option 1"));
        //            for (int i = 0; i < CalArea.Length; i++)
        //            {
        //                Log.AddLog(ch, string.Format("Target Area {0}, Search Code = {1}", CalArea[i], PosTest[ch][i].code));
        //            }
        //        }
        //        else if (Condition.SearchOption == 2)
        //        {
        //            Log.AddLog(ch, string.Format("Search Option 2"));

        //            for (int i = 0; i < CalArea.Length / 2; i++)
        //                PosTest[ch][i].code = PosTest[ch][CalArea.Length - 1 - i].code = (PosTest[ch][i].code + PosTest[ch][CalArea.Length - 1 - i].code) / 2;


        //            for (int i = 0; i < CalArea.Length; i++)
        //                Log.AddLog(ch, string.Format("Target Area {0}, Search Code = {1}", CalArea[i], PosTest[ch][i].code));

        //        }
        //        else if (Condition.SearchOption == 3)
        //        {

        //            Log.AddLog(ch, string.Format("Search Option 3"));
        //            for (int i = 0; i < CalArea.Length / 2; i++)
        //                PosTest[ch][0].code = PosTest[ch][CalArea.Length - 1 - i].code;

        //            for (int i = 0; i < CalArea.Length; i++)
        //            {
        //                Log.AddLog(ch, string.Format("Target Area {0}, Search Code = {1}", CalArea[i], PosTest[ch][i].code));
        //            }
        //        }
        //        else if (Condition.SearchOption == 4)
        //        {
        //            Log.AddLog(ch, string.Format("Search Option 4"));
        //            for (int i = 0; i < CalArea.Length / 2; i++)
        //            {
        //                if (i == Close_OpenLoopIndx[0])
        //                    PosTest[ch][i].code = PosTest[ch][CalArea.Length - 1 - i].code = (int)(PosTest[ch][CalArea.Length - 1 - i].code + ((PosTest[ch][i].code - PosTest[ch][CalArea.Length - 1 - i].code) * Condition.SearchOption4Percent2 / 100));
        //                else PosTest[ch][i].code = PosTest[ch][CalArea.Length - 1 - i].code = (int)(PosTest[ch][CalArea.Length - 1 - i].code + ((PosTest[ch][i].code - PosTest[ch][CalArea.Length - 1 - i].code) * Condition.SearchOption4Percent / 100));

        //            }


        //            for (int i = 0; i < CalArea.Length; i++)
        //            {
        //                if (Option.ReverseDrv)
        //                {
        //                    if (PosTest[ch][i].code > 4095 - Condition.Maximum40Pos)
        //                        PosTest[ch][i].code = 4095 - Condition.Maximum40Pos;

        //                }
        //                else
        //                {
        //                    if (PosTest[ch][i].code > 4095 - Condition.Maximum14Pos)
        //                        PosTest[ch][i].code = 4095 - Condition.Maximum14Pos;
        //                }

        //            }


        //            if (Model.ModelName == "SO1G73")
        //            {
        //                for (int i = 0; i < CalArea.Length; i++)
        //                {
        //                    if (i == F20Index[1] || i == F28Index[1])
        //                        PosTest[ch][i].code = PosTest[ch][i].code + Condition.F20_28Offset;
        //                }
        //            }



        //            for (int i = 0; i < CalArea.Length; i++)
        //            {
        //                Log.AddLog(ch, string.Format("Target Area {0}, Search Code = {1}", CalArea[i], PosTest[ch][i].code));
        //            }

        //        }

        //    }

        //    if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //    {
        //        if (Model.ModelName == "SO1C81")
        //        {
        //            for (int i = 0; i < PosTest[ch].Count; i++)
        //            {
        //                if (!m_ChannelOn[ch] || STATIC.isNonSpecError[ch])
        //                    break;
        //                if (i != 0 && i != Close_OpenLoopIndx[1])
        //                {
        //                    if (Math.Abs(PosTest[ch][i - 1].code - PosTest[ch][i].code) <= Condition.SearchHallDiffError)
        //                    {
        //                        m_ChannelOn[ch] = false;
        //                        STATIC.isNonSpecError[ch] = true;
        //                        errMsg[ch] = testItem + " Hall Diff Error";
        //                    }
        //                }

        //            }
        //        }

        //        if (Option.Step10Use)
        //        {
        //            if (Model.ModelName == "SO1C81")
        //            {
        //                if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //                {
        //                    if (PosTest[ch][1].code <= Condition.F16F18CodeLimit || PosTest[ch][18].code <= Condition.F16F18CodeLimit)
        //                    {
        //                        m_ChannelOn[ch] = false;
        //                        STATIC.isNonSpecError[ch] = true;
        //                        errMsg[ch] = testItem + " F16 Code Limit Error";
        //                    }
        //                    else if (PosTest[ch][9].code <= Condition.F40CodeLimit || PosTest[ch][10].code <= Condition.F40CodeLimit)
        //                    {
        //                        m_ChannelOn[ch] = false;
        //                        STATIC.isNonSpecError[ch] = true;
        //                        errMsg[ch] = testItem + " F40 Code Limit Error";
        //                    }
        //                }


        //            }
        //            else if (Model.ModelName == "SO1G73")
        //            {
        //                if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //                {
        //                    if (PosTest[ch][1].code <= Condition.F16F18CodeLimit || PosTest[ch][16].code <= Condition.F16F18CodeLimit)
        //                    {
        //                        m_ChannelOn[ch] = false;
        //                        STATIC.isNonSpecError[ch] = true;
        //                        errMsg[ch] = testItem + " F18 Code Limit Error";
        //                    }
        //                    else if (PosTest[ch][8].code <= Condition.F40CodeLimit || PosTest[ch][9].code <= Condition.F40CodeLimit)
        //                    {
        //                        m_ChannelOn[ch] = false;
        //                        STATIC.isNonSpecError[ch] = true;
        //                        errMsg[ch] = testItem + " F40 Code Limit Error";
        //                    }
        //                }

        //            }
        //        }
        //        else
        //        {
        //            if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //            {
        //                //if (PosTest[ch][3].code <= Condition.F40CodeLimit || PosTest[ch][4].code <= Condition.F40CodeLimit)
        //                //{
        //                //    m_ChannelOn[ch] = false;
        //                //    STATIC.isNonSpecError[ch] = true;
        //                //    errMsg[ch] = testItem + " F40 Code Limit Error";
        //                //}
        //            }

        //        }
        //    }

        //    InspResult res = new InspResult();
        //    InspResult res2 = new InspResult();


        //    Cam.CamList[ch].Acquire();
        //    if (Option.ActroDllUse)
        //        InspApi[ch].NewFineCOG(ch, 0, STATIC.InspMat[ch].Clone(), InspectionType.FindCover, res, false, false, false);
        //    InspApi[ch].TestCDll2(ch, STATIC.InspMat[ch].Clone(), false, 2, res2, false);



        //    STATIC.F40CoverDia = res2.CCover_dia;

        //    if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //    {
        //        STATIC.CoverPos[ch] = new System.Drawing.PointF((float)(res.Cover_cx), (float)(res.Cover_cy));
        //        STATIC.C_CoverPos[ch] = new System.Drawing.PointF((float)(res2.Cover_cx), (float)(res2.Cover_cy));
        //    }

        //    if (Option.isReadTargetVal)
        //    {



        //        byte[] ReadArr = new byte[9];
        //        int[] ReadCode = new int[6];

        //        byte[] rbuf = new byte[1];
        //        if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //        {
        //            for (int i = 0; i < 9; i++)
        //            {
        //                DrvIC.Dln.ReadArray(ch, DrvIC.AkSlave[ch], 0xF1 + i, 1, rbuf);
        //                ReadArr[i] = rbuf[0];

        //            }
        //        }
        //        if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //        {
        //            ReadCode[0] = ReadArr[0] << 4 | ReadArr[1] >> 4 & 0xff; //2528, 
        //            ReadCode[1] = (ReadArr[1] << 4 & 0xff) << 4 | ReadArr[2]; //3540, 40
        //            ReadCode[2] = ReadArr[3] << 4 | ReadArr[4] >> 4 & 0xff; //3228
        //            ReadCode[3] = (ReadArr[4] << 4 & 0xff) << 4 | ReadArr[5]; //2220
        //            ReadCode[4] = ReadArr[6] << 4 | ReadArr[7] >> 4 & 0xff; //1614, 14
        //            ReadCode[5] = (ReadArr[7] << 4 & 0xff) << 4 | ReadArr[8]; // 1820


        //            if (Option.Step10Use)
        //            {
        //                if (Model.ModelName == "SO1C81")
        //                {
        //                    PosTest[ch][6].code = ReadCode[0]; //2528
        //                    PosTest[ch][9].code = ReadCode[1]; //3540
        //                    PosTest[ch][10].code = ReadCode[1]; //40
        //                    PosTest[ch][13].code = ReadCode[2]; //3228
        //                    PosTest[ch][16].code = ReadCode[3]; //2220
        //                    PosTest[ch][19].code = ReadCode[4]; //1614
        //                    PosTest[ch][0].code = ReadCode[4]; //14
        //                    PosTest[ch][3].code = ReadCode[5]; //1820
        //                }
        //                else if (Model.ModelName == "SO1G73")
        //                {
        //                    PosTest[ch][5].code = ReadCode[0]; //2528
        //                    PosTest[ch][8].code = ReadCode[1]; //3540
        //                    PosTest[ch][9].code = ReadCode[1]; //40
        //                    PosTest[ch][12].code = ReadCode[2]; //3228
        //                    PosTest[ch][15].code = ReadCode[3]; //2220
        //                    PosTest[ch][17].code = ReadCode[4]; //1817
        //                    PosTest[ch][0].code = ReadCode[4]; //17
        //                    PosTest[ch][2].code = ReadCode[5]; //1820
        //                }
        //            }
        //            else
        //            {
        //                PosTest[ch][2].code = ReadCode[0]; //2028
        //                PosTest[ch][3].code = ReadCode[1]; //2840
        //                PosTest[ch][4].code = ReadCode[1]; //40
        //                PosTest[ch][5].code = ReadCode[2]; //4028
        //                PosTest[ch][6].code = ReadCode[3]; //2820
        //                PosTest[ch][7].code = ReadCode[4]; //2017
        //                PosTest[ch][0].code = ReadCode[4]; //17
        //                PosTest[ch][1].code = ReadCode[5]; //1820
        //            }

        //            Log.AddLog(ch, String.Format("OQC, Re Setting Search Pos..."));

        //            for (int j = 0; j < PosTest[ch].Count; j++)
        //                Log.AddLog(ch, String.Format("Read Target Code = {0}", PosTest[ch][j].code));
        //        }

        //    }

        //    if (Option.ChartVisible)
        //    {
        //        if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //        {
        //            double[] data = new double[SearchPos[0].Count];
        //            double[] data2 = new double[SearchPos[1].Count];

        //            for (int i = 0; i < SearchPos[0].Count; i++)
        //            {
        //                data[i] = SearchPos[0][i].Area;
        //                data2[i] = SearchPos[1][i].Area;
        //            }
        //            STATIC.fManage.DrawLinearityChart(ch, Targetpos, data, targetpos2, data2);
        //        }

        //    }

        //    if (m_ChannelOn[ch] && !STATIC.isNonSpecError[ch])
        //    {
        //        DriverIC.sPoint[] FwdPoint = new DriverIC.sPoint[Targetpos.Length];
        //        for (int i = 0; i < Targetpos.Length; i++)
        //        {
        //            FwdPoint[i].x = Targetpos[i];
        //            FwdPoint[i].y = SearchPos[0][i].Area;
        //        }

        //        DriverIC.sPoint[] BwdPoint = new DriverIC.sPoint[targetpos2.Length];
        //        for (int i = 0; i < targetpos2.Length; i++)
        //        {
        //            BwdPoint[i].x = targetpos2[i];
        //            BwdPoint[i].y = SearchPos[1][i].Area;
        //        }
        //        DriverIC.sLine LinRes = DrvIC.Line_fitting(FwdPoint, Targetpos.Length);
        //        DriverIC.sLine LinRes2 = DrvIC.Line_fitting(BwdPoint, targetpos2.Length);

        //        double b = LinRes.dSlope;         //   FWD Sensitivity
        //        double c = LinRes.dYintercept;

        //        double d = LinRes2.dSlope;         //   FWD Sensitivity
        //        double e = LinRes2.dYintercept;
        //        double newS = 0;
        //        for (int i = 0; i < Targetpos.Length; i++)
        //        {

        //            newS = b * Targetpos[i] + c;
        //            if (Linearity < Math.Abs(newS - SearchPos[0][i].Area))
        //                Linearity = Math.Abs(newS - SearchPos[0][i].Area);
        //        }

        //        for (int i = 0; i < targetpos2.Length; i++)
        //        {

        //            newS = d * targetpos2[i] + e;
        //            if (Linearity < Math.Abs(newS - SearchPos[1][i].Area))
        //                Linearity = Math.Abs(newS - SearchPos[1][i].Area);
        //        }
        //        Spec.PassFails[ch].Results[(int)SpecItem.Search_Linearity].Val = Linearity;

        //        Spec.SetResult(ch, (int)SpecItem.Search_Linearity, (int)SpecItem.Search_Linearity);
        //        ShowEvent.Show(ch, string.Format("{0}", "Linearity"));
        //    }

        //    //    LEDByChannle(ch, false);
        //}
        void AK7316_memory_upadate(int ch, int mode)
        {
            byte[] rbuf = new byte[1];
            byte temp, val;
            ushort time;
            switch (mode)
            {
                case 1: val = 0x01; time = 120; break;
                case 2: val = 0x02; time = 200; break;
                case 3: val = 0x04; time = 170; break;
                case 4: val = 0x08; time = 180; break;
                case 5: val = 0x10; time = 20; break;
                default: return;
            }
            for (temp = 0; temp < 5; temp++)
            {
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x03, 1, new byte[1] { val });
                Thread.Sleep(time);
                Dln.ReadArray(ch, DrvIC.AkSlave[ch], 0x4B, 1, rbuf);
                if ((rbuf[0] & 0x04) == 0x00)
                    break;
            }
            if (temp > 4) { Log.AddLog(ch, string.Format("Memory update NG.")); }
        }

        public void WriteResult_Area(int ch, bool splPass, string PassType)
        {
           
            try
            {
                int Step = 10;
                int[] showIndex = new int[1]; 
                if (Option.Step10Use)
                {
                    showIndex = new int[20] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
                    Step = 10;
                    //if (Model.ModelName == "SO1C81")
                    //{
                    //    showIndex = new int[20] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
                    //    Step = 10;
                    //}
                    //else if (Model.ModelName == "SO1G73")
                    //{
                    //    showIndex = new int[18] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 19 };
                    //    Step = 9;
                    //}
                }
                else
                {
                    showIndex = new int[8] { 0, 3, 6, 9, 10, 13, 16, 19 };
                    Step = 4;
                }
                string dateDir = STATIC.CreateDateDir();
                string AreaPath = string.Format("{0}res_{1}Step_Pass_{2}_{3}.csv", dateDir, Step.ToString(), "ST1C30", DateTime.Now.ToString("yyMMdd"));
                if (!Directory.Exists(dateDir))
                    Directory.CreateDirectory(dateDir);

             
                if (!File.Exists(AreaPath))
                    AddHeadArea(AreaPath);

                
                StreamWriter sw1 = File.AppendText(AreaPath);

                string AreaLog = "";
                if (errMsg[ch] == "Socket Empty") { Spec.PassFails[ch].FirstFailIndex = -2; }
                else if (errMsg[ch] != "")
                {
                    for (int k = 0; k < ItemList.Count; k++)
                    {
                        if (errMsg[ch].Contains(ItemList[k].Name))
                        {
                            Spec.PassFails[ch].FirstFailIndex = (-(k + 2));
                        }
                    }
                }


                AreaLog += string.Format("'{0},{1},{2},{3},",
                                   DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), m_StrIndex[ch], STATIC.Rcp.Model.TesterNo, ch + 1);

            //    Spec.TotlaTested++;

                //1st Fail Item
                if (Spec.PassFails[ch].FirstFailIndex > 0)
                {
                    //errMsg[ch] = Spec.PassFails[ch].FirstFail;
                    //Spec.TotlaFailed++;
                    ////    Log.AddLog(ch, "Fail : " + errMsg[ch]);

                }
                else if (Spec.PassFails[ch].FirstFailIndex < 0)
                {
                   // Spec.TotlaTested--;

                }
                else
                {
                    if (m_ChannelOn[ch])
                    {

                     //   Spec.TotlaPassed++;
                        AreaLog += PassType + ",";

                    }

                }
                double[] s = new double[1];
                if (Option.Step10Use)
                {
                    s = new double[20] { PosTest[ch][0].code, PosTest[ch][1].code, PosTest[ch][2].code, PosTest[ch][3].code, PosTest[ch][4].code, PosTest[ch][5].code, PosTest[ch][6].code,
                                                PosTest[ch][7].code, PosTest[ch][8].code, PosTest[ch][9].code, PosTest[ch][10].code, PosTest[ch][11].code, PosTest[ch][12].code, PosTest[ch][13].code,
                                                PosTest[ch][14].code, PosTest[ch][15].code, PosTest[ch][16].code, PosTest[ch][17].code, PosTest[ch][18].code, PosTest[ch][19].code};
                    //if (Model.ModelName == "SO1C81")
                    //{
                    //    s = new double[20] { PosTest[ch][0].code, PosTest[ch][1].code, PosTest[ch][2].code, PosTest[ch][3].code, PosTest[ch][4].code, PosTest[ch][5].code, PosTest[ch][6].code,
                    //                            PosTest[ch][7].code, PosTest[ch][8].code, PosTest[ch][9].code, PosTest[ch][10].code, PosTest[ch][11].code, PosTest[ch][12].code, PosTest[ch][13].code,
                    //                            PosTest[ch][14].code, PosTest[ch][15].code, PosTest[ch][16].code, PosTest[ch][17].code, PosTest[ch][18].code, PosTest[ch][19].code};

                    //}

                    //else if (Model.ModelName == "SO1G73")
                    //{
                    //    s = new double[18] { PosTest[ch][0].code, PosTest[ch][1].code, PosTest[ch][2].code, PosTest[ch][3].code, PosTest[ch][4].code, PosTest[ch][5].code, PosTest[ch][6].code,
                    //                            PosTest[ch][7].code, PosTest[ch][8].code, PosTest[ch][9].code, PosTest[ch][10].code, PosTest[ch][11].code, PosTest[ch][12].code, PosTest[ch][13].code,
                    //                            PosTest[ch][14].code, PosTest[ch][15].code, PosTest[ch][16].code, PosTest[ch][17].code};

                    //}

                }
                else
                {
                    s = new double[8] { PosTest[ch][0].code, PosTest[ch][1].code, PosTest[ch][2].code, PosTest[ch][3].code, PosTest[ch][4].code, PosTest[ch][5].code, PosTest[ch][6].code, PosTest[ch][7].code };


                }

                if (splPass)
                {

                    double[] s2 = new double[20]
                    {
                        Spec.PassFails[ch].Results[(int)SpecItem.POS1_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS2_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS3_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS4_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS5_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS6_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS7_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS8_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS9_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS10_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS11_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS12_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS13_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS14_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS15_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS16_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS17_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS18_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS19_Area].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS20_Area].Val
                    };
                    double[] s3 = new double[20]
                    {
                        Spec.PassFails[ch].Results[(int)SpecItem.POS1_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS2_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS3_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS4_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS5_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS6_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS7_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS8_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS9_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS10_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS11_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS12_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS13_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS14_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS15_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS16_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS17_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS18_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS19_DecenterR].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS20_DecenterR].Val
                    };
                    double[] s4 = new double[20]
                    {
                        Spec.PassFails[ch].Results[(int)SpecItem.POS1_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS2_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS3_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS4_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS5_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS6_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS7_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS8_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS9_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS10_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS11_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS12_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS13_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS14_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS15_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS16_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS17_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS18_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS19_DecenterX].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS20_DecenterX].Val
                    };
                    double[] s5 = new double[20]
                    {
                        Spec.PassFails[ch].Results[(int)SpecItem.POS1_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS2_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS3_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS4_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS5_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS6_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS7_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS8_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS9_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS10_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS11_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS12_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS13_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS14_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS15_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS16_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS17_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS18_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS19_DecenterY].Val,
                        Spec.PassFails[ch].Results[(int)SpecItem.POS20_DecenterY].Val
                    };
                    int Index = 0;
                    for (int i = 0; i < 20; i++)
                    {
                        if(showIndex.Contains(i))
                        {
                            AreaLog += string.Format("{0:0.000},", s[Index]);
                            AreaLog += string.Format("{0:0.000},", s2[i]);
                            AreaLog += string.Format("{0:0.000},", s3[i]);
                            AreaLog += string.Format("{0:0.000},", s4[i]);
                            AreaLog += string.Format("{0:0.000},", s5[i]);
                            Index++;
                        }
                      

                    }
                    sw1.WriteLine(AreaLog);
                }

                sw1.Close();
            }
            catch (Exception ex)
            {
                Log.AddLog(ch, "WriteResult Exception : " + ex.ToString() + " ch : " + ch.ToString());


            }
        }
        public void WriteResult(int ch, int currentPos, int itrCnt)
        {
            
            int[] F20_Index = new int[2] { 0, 0 };
            int[] F40_Index = new int[2] { 0, 0 };
            try
            {
                string dateDir = STATIC.CreateDateDir();

                int Step = 10;
                int[] showIndex = new int[1]; 
                if (Option.Step10Use)
                {
                    showIndex = new int[20] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
                    Step = 10;
                    //if (Model.ModelName == "SO1C81")
                    //{
                    //    showIndex = new int[20] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
                    //    Step = 10;
                    //}                    
                    //else if (Model.ModelName == "SO1G73")
                    //{
                    //    showIndex = new int[18] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 19 };
                    //    Step = 9;
                    //}
                }
                else
                {
                    showIndex = new int[8] { 0, 3, 6, 9, 10, 13, 16, 19 };
                    Step = 4;
                }
                  

                string path = string.Format("{0}res_{1}Step_{2}_{3}_CH{4}.csv", dateDir, Step.ToString(), "ST1C30", DateTime.Now.ToString("yyMMdd"), ch + 1);
               
                if (!Directory.Exists(dateDir))
                    Directory.CreateDirectory(dateDir);

                if (!File.Exists(path))
                    AddHeadResult(path);
                StreamWriter sw = File.AppendText(path);

                string log = "";

                if (errMsg[ch] == "Socket Empty") { Spec.PassFails[ch].FirstFailIndex = -2; }
                else if (errMsg[ch] != "")
                {
                    for (int k = 0; k < ItemList.Count; k++)
                    {
                        if (errMsg[ch].Contains(ItemList[k].Name))
                        {
                            Spec.PassFails[ch].FirstFailIndex = (-(k + 2));
                        }
                    }
                }

                Log.AddLog(ch, string.Format("ch : {0}, msg : {1}, PassFail : {2}", ch, errMsg[ch], Spec.PassFails[ch].FirstFailIndex));
                STATIC.WriteICDateTime[ch] = DateTime.Now;
                log += string.Format("'{0},{1},POS{2},{3},{4},{5},{6},",
                                   STATIC.WriteICDateTime[ch].ToString("yyyy-MM-dd HH:mm:ss.fff"), m_StrIndex[ch], currentPos, itrCnt + 1, STATIC.Rcp.Model.TesterNo, ch + 1, Spec.PassFails[ch].TotalFail);

                Spec.TotlaTested++;

                //1st Fail Item

                if (STATIC.isNonSpecError[ch])
                {
                    if (currentPos == 1 && itrCnt + 1 == 1)
                        log += errMsg[ch] + ",";
                    else
                        log += STATIC.ErrMsg[ch][0] + ",";// "NONE" + ",";
                }
                else
                {
                    if (Spec.PassFails[ch].FirstFailIndex > 0)
                    {
                        errMsg[ch] = Spec.PassFails[ch].FirstFail;
                        Spec.TotlaFailed++;
                        Log.AddLog(ch, "Fail : " + errMsg[ch]);
                        log += errMsg[ch] + ",";  //  First Fail Item
                    }
                    else if (Spec.PassFails[ch].FirstFailIndex < 0)
                    {
                        Spec.TotlaTested--;
                        log += errMsg[ch] + ",";
                    }
                    else
                    {
                        if (m_ChannelOn[ch])
                        {

                            Spec.TotlaPassed++;
                            F20_Index = new int[2] { 3, 16 };
                            F40_Index = new int[2] { 9, 10 };


                            //if (Spec.PassFails[ch].Results[(int)SpecItem.POS1_CDecenterR + 14 * F20_Index[0]].Val <= Condition.F20SGradeDecenter
                            //    && Spec.PassFails[ch].Results[(int)SpecItem.POS1_CDecenterR + 14 * F20_Index[1]].Val <= Condition.F20SGradeDecenter
                            //    && Spec.PassFails[ch].Results[(int)SpecItem.POS1_CCircleAccuracy + 14 * F20_Index[0]].Val <= Condition.F20SGradeCA
                            //    && Spec.PassFails[ch].Results[(int)SpecItem.POS1_CCircleAccuracy + 14 * F20_Index[1]].Val <= Condition.F20SGradeCA
                            //    && Spec.PassFails[ch].Results[(int)SpecItem.POS1_CCircleAccuracy + 14 * F40_Index[0]].Val <= Condition.F40SGradeCA
                            //    && Spec.PassFails[ch].Results[(int)SpecItem.POS1_CCircleAccuracy + 14 * F40_Index[1]].Val <= Condition.F40SGradeCA)
                            //{
                            //    errMsg[ch] = "S PASS";
                            //}
                            //else if (Spec.PassFails[ch].Results[(int)SpecItem.POS1_CDecenterR + 14 * F20_Index[0]].Val <= Condition.F20AGradeDecenter
                            //    && Spec.PassFails[ch].Results[(int)SpecItem.POS1_CDecenterR + 14 * F20_Index[1]].Val <= Condition.F20AGradeDecenter
                            //    && Spec.PassFails[ch].Results[(int)SpecItem.POS1_CCircleAccuracy + 14 * F20_Index[0]].Val <= Condition.F20AGradeCA
                            //    && Spec.PassFails[ch].Results[(int)SpecItem.POS1_CCircleAccuracy + 14 * F20_Index[1]].Val <= Condition.F20AGradeCA
                            //    && Spec.PassFails[ch].Results[(int)SpecItem.POS1_CCircleAccuracy + 14 * F40_Index[0]].Val <= Condition.F40AGradeCA
                            //    && Spec.PassFails[ch].Results[(int)SpecItem.POS1_CCircleAccuracy + 14 * F40_Index[1]].Val <= Condition.F40AGradeCA)
                            //{
                            //    errMsg[ch] = "A PASS";
                            //}
                            //else errMsg[ch] = "B PASS";

                            //double OCAreaLimit = Condition.IRISCalAreaF1820 * Condition.F20SABGradeCArea / 100.0;
                            //double COAreaLimit = Condition.IRISCalAreaF2220 * Condition.F20SABGradeCArea / 100.0;


                            //if (Spec.PassFails[ch].Results[(int)SpecItem.POS1_CArea + 14 * F20_Index[0]].Val >= Condition.IRISCalAreaF1820 - OCAreaLimit
                            //    && Spec.PassFails[ch].Results[(int)SpecItem.POS1_CArea + 14 * F20_Index[0]].Val <= Condition.IRISCalAreaF1820 + OCAreaLimit
                            //    && Spec.PassFails[ch].Results[(int)SpecItem.POS1_CArea + 14 * F20_Index[1]].Val >= Condition.IRISCalAreaF2220 - COAreaLimit
                            //    && Spec.PassFails[ch].Results[(int)SpecItem.POS1_CArea + 14 * F20_Index[1]].Val <= Condition.IRISCalAreaF2220 + COAreaLimit)
                            //{ }
                            //else
                            //{
                            //    errMsg[ch] = "C PASS";
                            //}
                            errMsg[ch] = "PASS";
                            log += errMsg[ch] + ",";
                        }
                        else
                        {
                            log += "NONE" + ",";
                        }
                    }
                }
               
                double[] s = new double[1];
                if (Option.Step10Use)
                {
                    s = new double[20] { PosTest[ch][0].code, PosTest[ch][1].code, PosTest[ch][2].code, PosTest[ch][3].code, PosTest[ch][4].code, PosTest[ch][5].code, PosTest[ch][6].code,
                                                PosTest[ch][7].code, PosTest[ch][8].code, PosTest[ch][9].code, PosTest[ch][10].code, PosTest[ch][11].code, PosTest[ch][12].code, PosTest[ch][13].code,
                                                PosTest[ch][14].code, PosTest[ch][15].code, PosTest[ch][16].code, PosTest[ch][17].code, PosTest[ch][18].code, PosTest[ch][19].code};
                    //if (Model.ModelName == "SO1C81")
                    //{
                    //    s = new double[20] { PosTest[ch][0].code, PosTest[ch][1].code, PosTest[ch][2].code, PosTest[ch][3].code, PosTest[ch][4].code, PosTest[ch][5].code, PosTest[ch][6].code,
                    //                            PosTest[ch][7].code, PosTest[ch][8].code, PosTest[ch][9].code, PosTest[ch][10].code, PosTest[ch][11].code, PosTest[ch][12].code, PosTest[ch][13].code,
                    //                            PosTest[ch][14].code, PosTest[ch][15].code, PosTest[ch][16].code, PosTest[ch][17].code, PosTest[ch][18].code, PosTest[ch][19].code};

                    //}

                    //else if (Model.ModelName == "SO1G73")
                    //{
                    //    s = new double[18] { PosTest[ch][0].code, PosTest[ch][1].code, PosTest[ch][2].code, PosTest[ch][3].code, PosTest[ch][4].code, PosTest[ch][5].code, PosTest[ch][6].code,
                    //                            PosTest[ch][7].code, PosTest[ch][8].code, PosTest[ch][9].code, PosTest[ch][10].code, PosTest[ch][11].code, PosTest[ch][12].code, PosTest[ch][13].code,
                    //                            PosTest[ch][14].code, PosTest[ch][15].code, PosTest[ch][16].code, PosTest[ch][17].code};

                    //}

                }
                else
                {
                    s = new double[8] { PosTest[ch][0].code, PosTest[ch][1].code, PosTest[ch][2].code, PosTest[ch][3].code, PosTest[ch][4].code, PosTest[ch][5].code, PosTest[ch][6].code, PosTest[ch][7].code };


                }

                for (int i = 0; i < showIndex.Length; i++)
                {
                    log += string.Format("{0:0.000},", s[i]);
                    log += string.Format("{0:0.000},", Spec.PassFails[ch].Results[(int)SpecItem.POS1_Hall + 14 * showIndex[i]].Val);
                    log += string.Format("{0:0.000},", Spec.PassFails[ch].Results[(int)SpecItem.POS1_Area + 14 * showIndex[i]].Val);
                    log += string.Format("{0:0.000},", Spec.PassFails[ch].Results[(int)SpecItem.POS1_Current + 14 * showIndex[i]].Val);
                    log += string.Format("{0:0.000},", Spec.PassFails[ch].Results[(int)SpecItem.POS1_DecenterR + 14 * showIndex[i]].Val);
                    log += string.Format("{0:0.000},", Spec.PassFails[ch].Results[(int)SpecItem.POS1_DecenterX + 14 * showIndex[i]].Val);
                    log += string.Format("{0:0.000},", Spec.PassFails[ch].Results[(int)SpecItem.POS1_DecenterY + 14 * showIndex[i]].Val);
                    log += string.Format("{0:0.000},", Spec.PassFails[ch].Results[(int)SpecItem.POS1_CircleAccuracy + 14 * showIndex[i]].Val);
                    log += string.Format("{0:0.000},", Spec.PassFails[ch].Results[(int)SpecItem.POS1_ShapeAccuracy + 14 * showIndex[i]].Val);
                    log += string.Format("{0:0.000},", Spec.PassFails[ch].Results[(int)SpecItem.POS1_CArea + 14 * showIndex[i]].Val);
                    log += string.Format("{0:0.000},", Spec.PassFails[ch].Results[(int)SpecItem.POS1_CDecenterR + 14 * showIndex[i]].Val);
                    log += string.Format("{0:0.000},", Spec.PassFails[ch].Results[(int)SpecItem.POS1_CDecenterX + 14 * showIndex[i]].Val);
                    log += string.Format("{0:0.000},", Spec.PassFails[ch].Results[(int)SpecItem.POS1_CDecenterY + 14 * showIndex[i]].Val);
                    log += string.Format("{0:0.000},", Spec.PassFails[ch].Results[(int)SpecItem.POS1_CCircleAccuracy + 14 * showIndex[i]].Val);
                    log += string.Format("{0:0.000},", Spec.PassFails[ch].Results[(int)SpecItem.POS1_CShapeAccuracy + 14 * showIndex[i]].Val);
              
                }


           
                for (int i = (int)SpecItem.F20_Decenter_Diff; i <= (int)SpecItem.Search_Linearity; i++)
                    log += string.Format("{0:0.000},", Spec.PassFails[ch].Results[i].Val);

                log += STATIC.beforePcal[ch] + "," + STATIC.beforeNcal[ch] + "," + STATIC.afterPcal[ch] + "," + STATIC.afterNcal[ch] + "," + STATIC.LastStepArea[ch].ToString("F3") + "," + STATIC.LastStep_1Area[ch].ToString("F3") + ","
                    + STATIC.LastStep_2Area[ch].ToString("F3") + "," + STATIC.OCHallDiff[ch] + "," + STATIC.COHallDiff[ch] + "," 
                    + "" + "," + "" + "," + "" + "," + "" + ","
                    + "" + "," + "" + "," + "" + "," + "" + ","
                    + STATIC.PIDName + ",";
                sw.WriteLine(log);
                sw.Close();
          
            }
            catch (Exception ex)
            {
                Log.AddLog(ch, "WriteResult Exception : " + ex.ToString() + " ch : " + ch.ToString());
            }

        }
        public void AddHeadResult(string sFilePath)
        {
            StreamWriter writer;
            writer = File.AppendText(sFilePath);
            int[] showIndex = new int[1];
            string[] s = new string[1];
            if(Option.Step10Use)
            {
                showIndex = new int[20] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
                s = new string[20];
                Array.Copy(STATIC.Step10Name, s, 20);
                //if (Model.ModelName == "SO1C81")
                //{
                //    showIndex = new int[20] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
                //    s = new string[20];
                //    Array.Copy(STATIC.Step10Name, s, 20);
                //}
                //else if(Model.ModelName == "SO1G73")
                //{
                //    showIndex = new int[18] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 19 };
                //    s = new string[18];
                //    Array.Copy(STATIC.Step9Name, s, 18);
                //}

            }
            else
            {
                showIndex = new int[8] { 0, 3, 6, 9, 10, 13, 16, 19 };
                s = new string[8];
                Array.Copy(STATIC.Step4Name_N2, s, 8);
                //if (Model.ModelName == "SO1C81")
                //{
                //    s = new string[8];
                //    Array.Copy(STATIC.Step4Name_N2, s, 8);
                //}
                //else if (Model.ModelName == "SO1G73")
                //{
                //    s = new string[8];
                //    Array.Copy(STATIC.Step4Name_N1, s, 8);
                //}

            }

            string sHeader;
            sHeader = "Time, ID, Position, ItrCnt,  M/C Num, Port, FailIndex, PassFail,";

            string sParam = "";
          

            for (int i = 0; i < showIndex.Length; i++)
            {
                sParam += string.Format("Target {0},", s[i]);
                sParam += string.Format("{0} {1},", Spec.Param[(int)SpecItem.POS1_Hall + 14 * showIndex[i]][1], s[i]);
                sParam += string.Format("{0} {1},", Spec.Param[(int)SpecItem.POS1_Area + 14 * showIndex[i]][1], s[i]);
                sParam += string.Format("{0} {1},", Spec.Param[(int)SpecItem.POS1_Current + 14 * showIndex[i]][1], s[i]);
                sParam += string.Format("{0} {1},", Spec.Param[(int)SpecItem.POS1_DecenterR + 14 * showIndex[i]][1], s[i]);
                sParam += string.Format("{0} {1},", Spec.Param[(int)SpecItem.POS1_DecenterX + 14 * showIndex[i]][1], s[i]);
                sParam += string.Format("{0} {1},", Spec.Param[(int)SpecItem.POS1_DecenterY + 14 * showIndex[i]][1], s[i]);
                sParam += string.Format("{0} {1},", Spec.Param[(int)SpecItem.POS1_CircleAccuracy + 14 * showIndex[i]][1], s[i]);
                sParam += string.Format("{0} {1},", Spec.Param[(int)SpecItem.POS1_ShapeAccuracy + 14 * showIndex[i]][1], s[i]);
                sParam += string.Format("{0} {1},", Spec.Param[(int)SpecItem.POS1_CArea + 14 * showIndex[i]][1], s[i]);
                sParam += string.Format("{0} {1},", Spec.Param[(int)SpecItem.POS1_CDecenterR + 14 * showIndex[i]][1], s[i]);
                sParam += string.Format("{0} {1},", Spec.Param[(int)SpecItem.POS1_CDecenterX + 14 * showIndex[i]][1], s[i]);
                sParam += string.Format("{0} {1},", Spec.Param[(int)SpecItem.POS1_CDecenterY + 14 * showIndex[i]][1], s[i]);
                sParam += string.Format("{0} {1},", Spec.Param[(int)SpecItem.POS1_CCircleAccuracy + 14 * showIndex[i]][1], s[i]);
                sParam += string.Format("{0} {1},", Spec.Param[(int)SpecItem.POS1_CShapeAccuracy + 14 * showIndex[i]][1], s[i]);
            }


            //for (int i = (int)SpecItem.POS1_Hall; i <= (int)SpecItem.POS20_CShapeAccuracy; i++)
            //{
            //    if (showIndex.Contains(i / 14))
            //    {
            //        if (i % 14 == 1)
            //        {
            //            currentIndex++;
            //            sParam += string.Format("Target {0},", s[currentIndex]);
            //        }
            //        sParam += string.Format("{0} {1},", Spec.Param[i][1], s[currentIndex]);
                   
            //    }
               
            //}




            for (int i = (int)SpecItem.F20_Decenter_Diff; i <= (int)SpecItem.Search_Linearity; i++)
            {
                sParam += string.Format("{0} {1},", Spec.Param[i][0], Spec.Param[i][1]);
            }

            sParam += "Before PCal, Before NCal, After PCal, After NCal, LStep SearchArea, LStep-1 SearchArea, LStep-2 SearchArea, OCHall Diff, COHall Diff, CO Scan Reverse, CO Scan Reverse Hall, CO Scan Max, CO Scan Max Hall, OC Scan Reverse, OC Scan Reverse Hall, OC Scan Max, OC Scan Max Hall, PIDName,";

            sHeader += sParam;
            writer.WriteLine(sHeader);


            //Spec Unit
            sHeader = "uint,,,,,,,,";
            sParam = "";

            for (int i = 0; i < showIndex.Length; i++)
            {
                sParam += " ,";
                sParam += string.Format("({0}),", Spec.Param[(int)SpecItem.POS1_Hall + 14 * showIndex[i]][4]);
                sParam += string.Format("({0}),", Spec.Param[(int)SpecItem.POS1_Area + 14 * showIndex[i]][4]);
                sParam += string.Format("({0}),", Spec.Param[(int)SpecItem.POS1_Current + 14 * showIndex[i]][4]);
                sParam += string.Format("({0}),", Spec.Param[(int)SpecItem.POS1_DecenterR + 14 * showIndex[i]][4]);
                sParam += string.Format("({0}),", Spec.Param[(int)SpecItem.POS1_DecenterX + 14 * showIndex[i]][4]);
                sParam += string.Format("({0}),", Spec.Param[(int)SpecItem.POS1_DecenterY + 14 * showIndex[i]][4]);
                sParam += string.Format("({0}),", Spec.Param[(int)SpecItem.POS1_CircleAccuracy + 14 * showIndex[i]][4]);
                sParam += string.Format("({0}),", Spec.Param[(int)SpecItem.POS1_ShapeAccuracy + 14 * showIndex[i]][4]);
                sParam += string.Format("({0}),", Spec.Param[(int)SpecItem.POS1_CArea + 14 * showIndex[i]][4]);
                sParam += string.Format("({0}),", Spec.Param[(int)SpecItem.POS1_CDecenterR + 14 * showIndex[i]][4]);
                sParam += string.Format("({0}),", Spec.Param[(int)SpecItem.POS1_CDecenterX + 14 * showIndex[i]][4]);
                sParam += string.Format("({0}),", Spec.Param[(int)SpecItem.POS1_CDecenterY + 14 * showIndex[i]][4]);
                sParam += string.Format("({0}),", Spec.Param[(int)SpecItem.POS1_CCircleAccuracy + 14 * showIndex[i]][4]);
                sParam += string.Format("({0}),", Spec.Param[(int)SpecItem.POS1_CShapeAccuracy + 14 * showIndex[i]][4]);
            }

            //for (int i = (int)SpecItem.POS1_Hall; i <= (int)SpecItem.POS20_CShapeAccuracy; i++)
            //{
            //    if (showIndex.Contains(i / 14))
            //    {
            //        if (i % 14 == 1) sParam += " ,";
            //        sParam += string.Format("({0}),", Spec.Param[i][4]);
            //    }
                
                
            
            //}
            for (int i = (int)SpecItem.F20_Decenter_Diff; i <= (int)SpecItem.Search_Linearity; i++)
            {
                sParam += string.Format("({0}),", Spec.Param[i][4]);
            }

            sHeader += sParam;
            writer.WriteLine(sHeader);

            //Spec Min
            sHeader = "Spec Min,,,,,,,,";
            sParam = "";

            for (int i = 0; i < showIndex.Length; i++)
            {
                sParam += " ,";
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_Hall + 14 * showIndex[i]][2]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_Area + 14 * showIndex[i]][2]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_Current + 14 * showIndex[i]][2]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_DecenterR + 14 * showIndex[i]][2]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_DecenterX + 14 * showIndex[i]][2]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_DecenterY + 14 * showIndex[i]][2]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_CircleAccuracy + 14 * showIndex[i]][2]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_ShapeAccuracy + 14 * showIndex[i]][2]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_CArea + 14 * showIndex[i]][2]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_CDecenterR + 14 * showIndex[i]][2]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_CDecenterX + 14 * showIndex[i]][2]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_CDecenterY + 14 * showIndex[i]][2]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_CCircleAccuracy + 14 * showIndex[i]][2]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_CShapeAccuracy + 14 * showIndex[i]][2]);
            }


            //for (int i = (int)SpecItem.POS1_Hall; i <= (int)SpecItem.POS20_CShapeAccuracy; i++)
            //{
            //    if (showIndex.Contains(i / 14))
            //    {
            //        if (i % 14 == 1) sParam += " ,";
            //        sParam += string.Format("{0},", Spec.Param[i][2]);
            //    }
                     
            //}
            for (int i = (int)SpecItem.F20_Decenter_Diff; i <= (int)SpecItem.Search_Linearity; i++)
            {
                sParam += string.Format("{0},", Spec.Param[i][2]);
            }
            sHeader += sParam;
            writer.WriteLine(sHeader);

            //Spec Max
            sHeader = "Spec Max,,,,,,,,";
            sParam = "";

            for (int i = 0; i < showIndex.Length; i++)
            {
                sParam += " ,";
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_Hall + 14 * showIndex[i]][3]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_Area + 14 * showIndex[i]][3]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_Current + 14 * showIndex[i]][3]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_DecenterR + 14 * showIndex[i]][3]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_DecenterX + 14 * showIndex[i]][3]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_DecenterY + 14 * showIndex[i]][3]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_CircleAccuracy + 14 * showIndex[i]][3]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_ShapeAccuracy + 14 * showIndex[i]][3]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_CArea + 14 * showIndex[i]][3]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_CDecenterR + 14 * showIndex[i]][3]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_CDecenterX + 14 * showIndex[i]][3]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_CDecenterY + 14 * showIndex[i]][3]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_CCircleAccuracy + 14 * showIndex[i]][3]);
                sParam += string.Format("{0},", Spec.Param[(int)SpecItem.POS1_CShapeAccuracy + 14 * showIndex[i]][3]);
            }

            //for (int i = (int)SpecItem.POS1_Hall; i <= (int)SpecItem.POS20_CShapeAccuracy; i++)
            //{
            //    if (showIndex.Contains(i / 14))
            //    {
            //        if (i % 14 == 1) sParam += " ,";
            //        sParam += string.Format("{0},", Spec.Param[i][3]);
            //    }
             
            //}
            for (int i = (int)SpecItem.F20_Decenter_Diff; i <= (int)SpecItem.Search_Linearity; i++)
            {
                sParam += string.Format("{0},", Spec.Param[i][3]);
            }
            sHeader += sParam;
            writer.WriteLine(sHeader);
            writer.Close();
        }
     
        public void AddHeadArea(string sFilePath)
        {
            StreamWriter writer;
            writer = File.AppendText(sFilePath);
            string[] s = new string[1];
            if (Option.Step10Use)
            {
                s = new string[20];
                Array.Copy(STATIC.Step10Name, s, 20);
                //if (Model.ModelName == "SO1C81")
                //{
                //    s = new string[20];
                //    Array.Copy(STATIC.Step10Name, s, 20);
                //}
                //else if (Model.ModelName == "SO1G73")
                //{
                //    s = new string[18];
                //    Array.Copy(STATIC.Step9Name, s, 18);
                //}

            }
            else
            {
                s = new string[8];
                Array.Copy(STATIC.Step4Name_N2, s, 8);
                //if (Model.ModelName == "SO1C81")
                //{
                //    s = new string[8];
                //    Array.Copy(STATIC.Step4Name_N2, s, 8);
                //}
                //else if (Model.ModelName == "SO1G73")
                //{
                //    s = new string[8];
                //    Array.Copy(STATIC.Step4Name_N1, s, 8);
                //}

            }



            string sHeader;
            sHeader = "Time, ID, M/C Num, Port, PassFail,";

            string sParam = "";
           
            for (int i = 0; i < s.Length; i++)
            {
                sParam += string.Format("Target {0},", s[i]);
                sParam += string.Format("Area {0},", s[i]);
                sParam += string.Format("Decenter R {0},", s[i]);
                sParam += string.Format("Decenter X {0},", s[i]);
                sParam += string.Format("Decenter Y {0},", s[i]);
            }
            sHeader += sParam;
            writer.WriteLine(sHeader);

            writer.Close();
        }

        public void Aging(int ch, string testItem)
        {

            if (m_ChannelOn[ch])
            {
                AK7316_iris_IC_open_sequence(ch, true);
                for (int i = 0; i < Condition.AgingCnt; i++)
                {
                    AK7316_iris_Write_power(ch, 4095);
                    Thread.Sleep(50);
                    AK7316_iris_Write_power(ch, 0);
                    Thread.Sleep(50);
                }
                AK7316_iris_IC_open_sequence(ch, false);
            }
        }

        void AK7316_iris_Write_power(int ch, short power)
        {
            if (power > 4095) power = 4095;
            if (power < 0) power = 0;

            byte[] tmp = new byte[2] { (byte)(power >> 4), (byte)(power << 4) };
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x00, 1, tmp);

        }


        void AK7316_iris_IC_mode(int ch, bool OnOff)
        {
            if (OnOff)
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x02, 1, new byte[1] { 0x00 });
            else
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x02, 1, new byte[1] { 0x40 });

        }

        public void AK7316_iris_open_OL2(int ch, int OpenLoopDir, bool isSettling = false)
        {
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[1] { 0x3B });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x02, 1, new byte[1] { 0x00 });
            if(OpenLoopDir == 0)
            {
                if (Option.ReverseDrv)
                    AK7316_iris_Write_power(ch, 0);
                else
                    AK7316_iris_Write_power(ch, 4095);
            }
            else
            {
                if (Option.ReverseDrv)
                    AK7316_iris_Write_power(ch, 4095);
                else
                    AK7316_iris_Write_power(ch, 0);

            }
            
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xA6, 1, new byte[1] { 0x7B });

            if (!isSettling)
                Wait(Condition.OpenLoopDelay);
            //    Thread.Sleep(Condition.OpenLoopDelay);

            if (OpenLoopDir == 0)
                AK7316_iris_Write_power(ch, (short)Condition.OpenLoopDownCurrent);
            else
                AK7316_iris_Write_power(ch, (short)Condition.OpenLoopDownCurrent40);
            if (!isSettling)
                Wait(100);
               // Thread.Sleep(100);

        }
        public void AK7316_F14OpenLoop(int ch, bool isAfterSeq, bool FirstMoveMode)
        {
            int DefaultCurrent = 4095, DefaultCurrent2 = 0;
            if (Option.ReverseDrv)
            {
                DefaultCurrent = 0;
                DefaultCurrent2 = 2048;
            }

            if (!isAfterSeq)
            {
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x02, 1, new byte[1] { 0x00 });
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x00, 1, new byte[2] { (byte)((int)DefaultCurrent>> 4), (byte)((int)DefaultCurrent << 4) }); //옵션//고정
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[1] { 0x3B });
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xA6, 1, new byte[1] { 0x7B });
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x00, 1, new byte[2] { (byte)((int)Condition.OpenLoopDownCurrent_2 >> 4), (byte)((int)Condition.OpenLoopDownCurrent_2 << 4) });
                Wait(100);
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x00, 1, new byte[2] { (byte)((int)Condition.OpenLoopDownCurrent >> 4), (byte)((int)Condition.OpenLoopDownCurrent << 4) });
                if (FirstMoveMode)
                    Wait(100);
            }
            else
            {
                //0x7F, 0xF0
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xC2, 1, new byte[2] { (byte)(DefaultCurrent2 >> 4), (byte)(DefaultCurrent2 << 4) });

                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x00, 1, new byte[2] { (byte)(DefaultCurrent >> 4), (byte)(DefaultCurrent << 4) }); //옵션
                Wait(Condition.OpenLoopOptionDelay);

                //}
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x38, 1, new byte[1] { (byte)(0 >> 4) });
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x3A, 1, new byte[1] { (byte)(0 << 4) });
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x00, 1, new byte[2] { (byte)(DefaultCurrent >> 4), (byte)(DefaultCurrent << 4) }); //옵션
                if (!FirstMoveMode)
                    Wait(Condition.OpenLoopOptionDelay);
                else
                    Wait(Condition.OpenLoopAgingDelay);
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xA6, 1, new byte[1] { 0x00 });
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xC2, 1, new byte[2] { 0, 0 });
                Wait(Condition.OpenLoopOptionDelay);
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[1] { 0x00 });
                Wait(Condition.OCDelay);
            }

        }
        public void AK7316_F40OpenLoop(int ch, bool isAfterSeq)
        {
            int DefaultCurrent = 0, DefaultCurrent2 = 0;
            if (Option.ReverseDrv)
            {
                DefaultCurrent = 4095;
                DefaultCurrent2 = 2047;
            }

            if (!isAfterSeq)
            {
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x02, 1, new byte[1] { 0x00 });
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x00, 1, new byte[2] { (byte)((int)DefaultCurrent >> 4), (byte)((int)DefaultCurrent << 4) }); //옵션
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[1] { 0x3B });
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xA6, 1, new byte[1] { 0x7B });
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x00, 1, new byte[2] { (byte)((int)Condition.OpenLoopDownCurrent40_2 >> 4), (byte)((int)Condition.OpenLoopDownCurrent40_2 << 4) }); //옵션
                Wait(100);
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x00, 1, new byte[2] { (byte)((int)Condition.OpenLoopDownCurrent40 >> 4), (byte)((int)Condition.OpenLoopDownCurrent40 << 4) });
              
            }
            else
            {
                //0x7F, 0xF0
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xC0, 1, new byte[2] { (byte)(DefaultCurrent2 >> 4), (byte)(DefaultCurrent2 << 4) });

                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x00, 1, new byte[2] { (byte)(DefaultCurrent >> 4), (byte)(DefaultCurrent << 4) }); //옵션
                Wait(Condition.OpenLoopOptionDelay);//20

                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x38, 1, new byte[1] { (byte)(DefaultCurrent >> 4) });
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x3A, 1, new byte[1] { (byte)(DefaultCurrent << 4) });
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x00, 1, new byte[2] { (byte)(DefaultCurrent >> 4), (byte)(DefaultCurrent << 4) }); //옵션
                Wait(Condition.OpenLoopOptionDelay);//20
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xA6, 1, new byte[1] { 0x00 });
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xC0, 1, new byte[2] { 0, 0 });
                //     Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x00, 1, new byte[2] { (byte)(DefaultCurrent >> 4), (byte)(DefaultCurrent << 4) });
                Wait(Condition.OpenLoopOptionDelay);//20
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[1] { 0x00 });
                Wait(Condition.OCDelay);
            }

        }
        void AK7316_iris_IC_open_sequence(int ch, bool OnOff)
        {
            byte[] backupData = new byte[1];
            if (OnOff)
            {
                Log.AddLog(ch, string.Format("Open sequence start"));
                AK7316_iris_IC_mode(ch, false);
                Dln.ReadArray(ch, DrvIC.AkSlave[ch], 0x0B, 1, backupData);
                Log.AddLog(ch, string.Format("Backupdata = {0}", backupData[0].ToString("X2")));
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[1] { 0x3B });
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x1A, 1, new byte[1] { 0x00 });
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x02, 1, new byte[1] { (byte)(backupData[0] & 0x7F) });
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xA6, 1, new byte[1] { 0x7B });
                AK7316_iris_IC_mode(ch, true);

            }
            else
            {
                Log.AddLog(ch, string.Format("Open sequence finish"));
                AK7316_iris_IC_mode(ch, false);
                Dln.ReadArray(ch, DrvIC.AkSlave[ch], 0x0B, 1, backupData);
                Log.AddLog(ch, string.Format("Backupdata = {0}", backupData[0].ToString("X2")));
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xA6, 1, new byte[1] { 0x00 });
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[1] { 0x00 });
                AK7316_iris_IC_mode(ch, true);
            }
        }

       
        #region LinComp
       

        public void CheckLinearity(int ch, string testitem)
        {
            int step = Condition.LinCompCount;
            int start = Condition.LinCompStart;
            int end = Condition.LinCompEnd;
           
            float[] targPos = new float[step + 1];
            float[] targPos2 = new float[step + 1];
            int[] autoThresh = new int[4];
      
            int valueStepsize = step;

            int step_size = step_size = (end - start) / step;
            int temp_table = 0;// start;
            if (Option.ReverseDrv)
                temp_table = end;
            else
                temp_table = start;
            string dateDir = STATIC.CreateDateDir();
            string path = string.Empty;
            dateDir += "LinearityData\\";
            if (!Directory.Exists(dateDir))
                Directory.CreateDirectory(dateDir);
            path = string.Format("{0}{1}_{2}.csv", dateDir, m_StrIndex[ch], "After_Linearity");
            string log = string.Empty;
            List<string> arry = new List<string>();

            int i;
            List<InspResult> res = new List<InspResult>();
       
         //   LEDs_All_On(true);
            for (i = 0; i <= step; i++)
            {
                log = string.Empty;
                res.Add(new InspResult());
                targPos[i] = temp_table;
                if (m_ChannelOn[ch])
                    DrvIC.Move(ch, DriverIC.AK, (int)targPos[i]);
                Thread.Sleep(Condition.LinCompDelay);

                if (m_ChannelOn[ch])
                {
                    res[i].current = Dln.GetCurrent(ch);
                    res[i].IrisHall = DrvIC.ReadHall(ch, DriverIC.AK);
                }
              
                if(!Option.isActMode)
                {
                    Cam.CamList[ch].Acquire();
                    res[i].Area = InspApi[ch].JustCheckArea(ch, STATIC.InspMat[ch].Clone(), false);

               //     Vision.MilList[port].GrabCommon(i);
               ////     Vision.MilList[port].InvertImage(i, false, true, ref autoThresh);
               //     isError = Vision.MilList[port].NewFineCOG(false, i, InspectionType.Area, res[i], false, false, 0);
                }
                else
                {
                //    Vision.MilList[port].GrabCommon(i);
                ////    Vision.MilList[port].InvertImage(i, false, true, ref autoThresh);
                //    isError = Vision.MilList[port].NewFineCOG(false, i, InspectionType.Area, res[i], false, false, 0);

                //    if (res[i].area[0][0] > 15)
                //    {
                //        Vision.MilList[port].GrabCommon(i);
                //        isError = Vision.MilList[port].FineCOG(i, "OpenHexagon", res[i], false);
                //    }
                    //Vision.MilList[port].GrabCommon(i);
                    //isError = Vision.MilList[port].FineCOG(i, "OpenHexagon", res[i], false);

                }


                if (m_ChannelOn[ch])
                {
                    Log.AddLog(ch, String.Format("Targetpos {0}, current {1:0.00}, IrisHall {2}, area {3:0.00} ", targPos[i], res[i].current, res[i].IrisHall, res[i].Area));
                    log += string.Format("{0},{1},{2:0.0000},{3:0.0000},,", targPos[i], res[i].current, res[i].IrisHall, res[i].Area);

                }
                if (Option.ReverseDrv)
                    temp_table -= step_size;
                else
                    temp_table += step_size;
                arry.Add(log);
            }
            arry.Add("\r\n");
            Log.AddLog(0, string.Format("\r\n"));
            Log.AddLog(1, string.Format("\r\n"));

            Array.Copy(targPos, targPos2, targPos.Length);
            Array.Reverse(targPos2);
            res.Clear();
            for (i = 0; i < targPos2.Length; i++)
            {
                log = string.Empty;
                res.Add(new InspResult());

                if (m_ChannelOn[ch])
                    DrvIC.Move(ch, DriverIC.AK, (int)targPos2[i]);
                Thread.Sleep(Condition.LinCompDelay);

                if (m_ChannelOn[ch])
                {
                    res[i].current = Dln.GetCurrent(ch);
                    res[i].IrisHall = DrvIC.ReadHall(ch, DriverIC.AK);
                }
              
                if(!Option.isActMode)
                {
                    Cam.CamList[ch].Acquire();
                    res[i].Area = InspApi[ch].JustCheckArea(ch, STATIC.InspMat[ch].Clone(), false);
                    //  Vision.MilList[port].GrabCommon(i);
                    ////  Vision.MilList[port].InvertImage(i, false, true, ref autoThresh);
                    //  isError = Vision.MilList[port].NewFineCOG(false, i, InspectionType.Area, res[i], false, false, 0);
                }
                else
                {
                //    Vision.MilList[port].GrabCommon(i);
                //////    Vision.MilList[port].InvertImage(i, false, true, ref autoThresh);
                ////    isError = Vision.MilList[port].NewFineCOG(false, i, InspectionType.Area, res[i], false, false, 0);

                ////    if (res[i].area[0][0] > 15)
                ////    {
                ////        Vision.MilList[port].GrabCommon(i);
                ////        isError = Vision.MilList[port].FineCOG(i, "OpenHexagon", res[i], false);
                ////    }
                //    Vision.MilList[port].GrabCommon(i);
                //    isError = Vision.MilList[port].FineCOG(i, "OpenHexagon", res[i], false);
                }

                if (m_ChannelOn[ch])
                {
                    Log.AddLog(ch, string.Format("Targetpos {0}, current {1:0.00}, IrisHall {2}, area {3:0.00} ", targPos2[i], res[i].current, res[i].IrisHall, res[i].Area));
                    log += string.Format("{0},{1},{2:0.0000},{3:0.0000},,", targPos2[i], res[i].current, res[i].IrisHall, res[i].Area);
                }
                arry.Add(log);
            }
            if (path != "" && Option.SaveRawData) STATIC.SetTextLine(path, arry);
          //  LEDs_All_On(false);

        }

     
        public bool AK7316_LinearityComp(int ch, int start, int end, int step, int margin_start, int margin_end, int s_value, int e_value, int linear_spec)
        {
           
            float[] targPos = new float[step + 1];
            float[] targPos2 = new float[step + 1];
            float[] targPos3 = new float[step + 1];
            float[] tmpArea = new float[step + 1];
            //float[] ReadHall = new float[step + 1];
            float[] Area = new float[step + 1];
            //List<float[]> Area = new List<float[]>() { new float[step + 1], new float[step + 1] };
            int[] autoThresh = new int[4];
          
            int valueStepsize = step;
            float[] refLensPosi = new float[step + 1];
            float[] valueLensPosi = new float[valueStepsize + 1];
            float valueStep, valuegap;
            float max_valuegap = 0;
            int ignInf = 0;                        // Invalid data range (Infinity Side)
            int ignMac = 0;                        // Invalid data range (Macro Side)

            int numLinCompData;                // Data number
          
            short pVt = 0;
            short nVt = 0;

            if (m_ChannelOn[ch])
            {
                pVt = (short)((DrvIC.Dln.Read2Byte(ch, DrvIC.AkSlave[ch], 0xC0, 1) >> 6) & 0x03FF);
                nVt = (short)((DrvIC.Dln.Read2Byte(ch, DrvIC.AkSlave[ch], 0xC2, 1) >> 6) & 0x03FF);
            }
;
            int[] linCoef = new int[27];        // Array for storing line compensation coefficients
                                          // Recalculation "NEGVT" after linearity compensation
            float resError = 0;                        // Variable for storing residual error after linearity compensation
            int i, result;
            int step_size = (end - start) / step;        // modified by D.W '19/10/11 to fix linearity compensation variation
            int temp_table = 0;// start;
            if (Option.ReverseDrv)
                temp_table = end;
            else
                temp_table = start;
            string dateDir = STATIC.CreateDateDir();
            string path = string.Empty;
            dateDir += "LinearityData\\";
            if (!Directory.Exists(dateDir))
                Directory.CreateDirectory(dateDir);
        
                path = string.Format("{0}{1}_{2}.csv", dateDir, m_StrIndex[ch], "Before_Linearity");

            string log = string.Empty;
            List<string> arry = new List<string>();
            if (m_ChannelOn[ch])
            {
                DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x02, 1, new byte[1] { 0x00 });
                Log.AddLog(ch, string.Format("IRIS Mode ON."));
                DrvIC.Move(ch, DriverIC.AK, start);
            }
            Thread.Sleep(100);
            if (m_ChannelOn[ch])
            {
               
                DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[1] { 0x3B });
                for (i = 0; i < 27; i++)
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xC5 + i, 1, new byte[1] { 0x00 });
                DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[1] { 0x00 });
            
            }

            List<InspResult> res = new List<InspResult>();
         
            for (i = 0; i <= step; i++)
            {
                log = string.Empty;
                res.Add(new InspResult());
                targPos[i] = temp_table;
                if (m_ChannelOn[ch])
                    DrvIC.Move(ch, DriverIC.AK, (int)targPos[i]);
                Wait(Condition.LinCompDelay);

                if (m_ChannelOn[ch])
                {
                    res[i].current = Dln.GetCurrent(ch);
                    res[i].IrisHall = DrvIC.ReadHall(ch, DriverIC.AK);
                }

                if (!Option.isActMode)
                {
                    Cam.CamList[ch].Acquire();
                    res[i].Area = InspApi[ch].JustCheckArea(ch, STATIC.InspMat[ch].Clone(), false);
                   // Vision.MilList[port].GrabCommon(i);
                   //// Vision.MilList[port].InvertImage(i, false, true, ref autoThresh);
                   // Vision.MilList[port].NewFineCOG(false, i, InspectionType.Area, res[i], false, false, 0);
                }
                else
                {
                    Cam.CamList[ch].Acquire();
                    InspApi[ch].TestCDll2(ch, STATIC.InspMat[ch].Clone(), false, 1, res[i], true);
                    // Vision.MilList[port].GrabCommon(i);
                    //// Vision.MilList[port].InvertImage(i, false, true, ref autoThresh);
                    // Vision.MilList[port].NewFineCOG(false, i, InspectionType.Area, res[i], false, false, 0);

                    // if (res[i].area[0][0] > 15)
                    // {
                    //     Vision.MilList[port].GrabCommon(i);
                    //     Vision.MilList[port].FineCOG(i, "OpenHexagon", res[i], false);
                    // }
                    //Vision.MilList[port].GrabCommon(i);
                    //Vision.MilList[port].FineCOG(i, "OpenHexagon", res[i], false);
                }

                if (m_ChannelOn[ch])
                {
                    Area[i] = (float)res[i].Area;
                    Log.AddLog(ch, String.Format("Targetpos {0}, current {1:0.00}, IrisHall {2}, area {3:0.00} ", targPos[i], res[i].current, res[i].IrisHall, Area[i]));
                    log += string.Format("{0},{1},{2:0.0000},{3:0.0000},,", targPos[i], res[i].current, res[i].IrisHall, Area[i]);

                }
                if (Option.ReverseDrv)
                    temp_table -= step_size;
                else
                    temp_table += step_size;
                arry.Add(log);
            }

            arry.Add("\r\n");

            Log.AddLog(0, string.Format("\r\n"));
            Log.AddLog(1, string.Format("\r\n"));

            Array.Copy(targPos, targPos2, targPos.Length);
            Array.Reverse(targPos2);
            res.Clear();
            for (i = 0; i < targPos2.Length; i++)
            {
                log = string.Empty;
                res.Add(new InspResult());

                if (m_ChannelOn[ch])
                    DrvIC.Move(ch, DriverIC.AK, (int)targPos2[i]);
                Wait(Condition.LinCompDelay);
                if (m_ChannelOn[ch])
                {
                    res[i].current = Dln.GetCurrent(ch);
                    res[i].IrisHall = DrvIC.ReadHall(ch, DriverIC.AK);
                }


                if (!Option.isActMode)
                {
                    Cam.CamList[ch].Acquire();
                    res[i].Area = InspApi[ch].JustCheckArea(ch, STATIC.InspMat[ch].Clone(), false);
                    // Vision.MilList[port].GrabCommon(i);
                    //// Vision.MilList[port].InvertImage(i, false, true, ref autoThresh);
                    // Vision.MilList[port].NewFineCOG(false, i, InspectionType.Area, res[i], false, false, 0);
                }
                else
                {
                    Cam.CamList[ch].Acquire();
                    InspApi[ch].TestCDll2(ch, STATIC.InspMat[ch].Clone(), false, 1, res[i], true);
                    // Vision.MilList[port].GrabCommon(i);
                    //// Vision.MilList[port].InvertImage(i, false, true, ref autoThresh);
                    // Vision.MilList[port].NewFineCOG(false, i, InspectionType.Area, res[i], false, false, 0);

                    // if (res[i].area[0][0] > 15)
                    // {
                    //     Vision.MilList[port].GrabCommon(i);
                    //     Vision.MilList[port].FineCOG(i, "OpenHexagon", res[i], false);
                    // }
                    //Vision.MilList[port].GrabCommon(i);
                    //Vision.MilList[port].FineCOG(i, "OpenHexagon", res[i], false);
                }


                if (m_ChannelOn[ch])
                {
                    // Area[j][i] = (float)res[i].area[0][j];
                    Log.AddLog(ch, String.Format("Targetpos {0}, current {1:0.00}, IrisHall {2}, area {3:0.00} ", targPos2[i], res[i].current, res[i].IrisHall, res[i].Area));
                    log += string.Format("{0},{1},{2:0.0000},{3:0.0000},,", targPos2[i], res[i].current, res[i].IrisHall, res[i].Area);
                }
                arry.Add(log);

            }
            if (path != "" && Option.SaveRawData) STATIC.SetTextLine(path, arry);
            if (m_ChannelOn[ch])
            {
                valueStep = (Area[step - e_value] - Area[s_value]) / (valueStepsize);
                valueLensPosi[0] = Area[s_value];
                valueLensPosi[valueStepsize] = Area[s_value + valueStepsize];

                for (i = 1; i < valueStepsize; i++)
                {
                    valueLensPosi[i] = valueLensPosi[i - 1] + valueStep;

                    valuegap = valueLensPosi[i] - Area[i + s_value];

                    if (valuegap >= 0) { }

                    else { valuegap *= -1; }
                    if (max_valuegap < valuegap)
                        max_valuegap = valuegap;
                }
                if (max_valuegap > linear_spec)

                {
                    // Main funciton of linearity compensation coefficient calculation.
                    if (targPos.Length == Area.Length)
                    {
                        numLinCompData = targPos.Length;//sizeof(targPosi) / sizeof(float);
                        if (Option.ReverseDrv)
                        {
                            Array.Copy(targPos, targPos3, targPos.Length);
                            Array.Reverse(targPos3);
                            Array.Copy(Area, tmpArea, Area.Length);
                            Array.Reverse(tmpArea);
                            result = InspApi[ch].LinCompMain(targPos3, tmpArea, numLinCompData, pVt, nVt, ignInf, ignMac, linCoef, ref resError, Option.ReverseDrv);
                        }
                        else
                            result = InspApi[ch].LinCompMain(targPos, Area, numLinCompData, pVt, nVt, ignInf, ignMac, linCoef, ref resError, Option.ReverseDrv);




                        if (result != 0)
                            return false;
                    }
                    else
                        return false;

                    DrvIC.Move(ch, DriverIC.AK, 2048);
                    Thread.Sleep(50);
                
                    // save coefficient
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[1] { 0x3B });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xC5, 1, new byte[1] { (byte)linCoef[0] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xC6, 1, new byte[1] { (byte)linCoef[1] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xC7, 1, new byte[1] { (byte)linCoef[2] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xC8, 1, new byte[1] { (byte)linCoef[3] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xC9, 1, new byte[1] { (byte)linCoef[4] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xCA, 1, new byte[1] { (byte)linCoef[5] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xCB, 1, new byte[1] { (byte)linCoef[6] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xCC, 1, new byte[1] { (byte)linCoef[7] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xCD, 1, new byte[1] { (byte)linCoef[8] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xCE, 1, new byte[1] { (byte)linCoef[9] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xCF, 1, new byte[1] { (byte)linCoef[10] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD0, 1, new byte[1] { (byte)linCoef[11] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD1, 1, new byte[1] { (byte)linCoef[12] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD2, 1, new byte[1] { (byte)linCoef[13] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD3, 1, new byte[1] { (byte)linCoef[14] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD4, 1, new byte[1] { (byte)linCoef[15] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD5, 1, new byte[1] { (byte)linCoef[16] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD6, 1, new byte[1] { (byte)linCoef[17] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD7, 1, new byte[1] { (byte)linCoef[18] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD8, 1, new byte[1] { (byte)linCoef[19] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD9, 1, new byte[1] { (byte)linCoef[20] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xDA, 1, new byte[1] { (byte)linCoef[21] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xDB, 1, new byte[1] { (byte)linCoef[22] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xDC, 1, new byte[1] { (byte)linCoef[23] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xDD, 1, new byte[1] { (byte)linCoef[24] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xDE, 1, new byte[1] { (byte)linCoef[25] });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xDF, 1, new byte[1] { (byte)linCoef[26] });

                    for (int k = 0; k < 27; k++)
                    {
                        Log.AddLog(ch, string.Format("mAddr: 0x{0:X2} \t Wdata: {1}", 197 + k, (byte)linCoef[k]));
                    }

                    AK7316_memory_upadate(ch, 4);
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[1] { 0x00 });
                    Thread.Sleep(50);
                }
            }

         

            return true;
        }
        public bool AK7316_LinearityComp_2Dir(int ch, int start, int end, int step, int margin_start, int margin_end, int s_value, int e_value, int linear_spec, bool isRev)
        {
            try
            {
               
                float[] targPos = new float[step + 1];
                float[] targPos2 = new float[step + 1];
                float[] targPos3 = new float[step + 1];
                float[] AreaAvgTmp = new float[step + 1];
                //float[] ReadHall = new float[step + 1];

                float[] Area = new float[step + 1];
                float[] Area2 = new float[step + 1];
                float[] Area_Avg = new float[step + 1];

              
               
                int valueStepsize = step;
                float[] refLensPosi = new float[step + 1];
                float[] valueLensPosi = new float[valueStepsize + 1];
                float valueStep, valuegap;
                float max_valuegap = 0;
                int ignInf = 0;                        // Invalid data range (Infinity Side)
                int ignMac = 0;                        // Invalid data range (Macro Side)

                int numLinCompData;                // Data number

                short pVt = 0;
                short nVt = 0;

                if (m_ChannelOn[ch])
                {
                    pVt = (short)((DrvIC.Dln.Read2Byte(ch, DrvIC.AkSlave[ch], 0xC0, 1) >> 6) & 0x03FF);
                    nVt = (short)((DrvIC.Dln.Read2Byte(ch, DrvIC.AkSlave[ch], 0xC2, 1) >> 6) & 0x03FF);
                }
;
                int[] linCoef = new int[27];        // Array for storing line compensation coefficients
               
                float resError = 0;                        // Variable for storing residual error after linearity compensation
                int i, result;
                int step_size = (end - start) / step;        // modified by D.W '19/10/11 to fix linearity compensation variation
                int temp_table = 0;// start;
                if (Option.ReverseDrv)
                    temp_table = end;
                else
                    temp_table = start;


                string dateDir = STATIC.CreateDateDir();
                string path = string.Empty;
                dateDir += "LinearityData\\";
                if (!Directory.Exists(dateDir))
                    Directory.CreateDirectory(dateDir);

                path = string.Format("{0}{1}_{2}.csv", dateDir, m_StrIndex[ch], "Before_Linearity");
                string log = string.Empty;

                List<string> arry = new List<string>();
                if (m_ChannelOn[ch])
                {
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x02, 1, new byte[1] { 0x00 });
                    Log.AddLog(ch, string.Format("IRIS Mode ON."));
                    DrvIC.Move(ch, DriverIC.AK, start);
                }
                Thread.Sleep(100);
                if (m_ChannelOn[ch])
                {
                   
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[1] { 0x3B });
                    for (i = 0; i < 27; i++)
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xC5 + i, 1, new byte[1] { 0x00 });
                    DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[1] { 0x00 });
                    Thread.Sleep(50);
                }
                List<InspResult> res = new List<InspResult>();
            
                for (i = 0; i <= step; i++)
                {
                    log = string.Empty;
                    res.Add(new InspResult());
                    targPos[i] = temp_table;
                    if (m_ChannelOn[ch])
                        DrvIC.Move(ch, DriverIC.AK, (int)targPos[i]);
                    Wait(Condition.LinCompDelay);
                    if (m_ChannelOn[ch])
                    {
                        res[i].current = Dln.GetCurrent(ch);
                        res[i].IrisHall = DrvIC.ReadHall(ch, DriverIC.AK);
                    }


                    if (!Option.isActMode)
                    {
                        Cam.CamList[ch].Acquire();
                    
                       
                        res[i].Area = InspApi[ch].JustCheckArea(ch, STATIC.InspMat[ch].Clone(), false);
                       

                        //Vision.MilList[port].GrabCommon(i);
                        ////Vision.MilList[port].InvertImage(i, false, true, ref autoThresh);
                        ////Vision.MilList[port].FineCOG(i, "CheckArea", res[i], false, true);
                        //Vision.MilList[port].NewFineCOG(false, i, InspectionType.Area, res[i], false, false, 0);
                    }
                    else
                    {
                        Cam.CamList[ch].Acquire();
                        InspApi[ch].TestCDll2(ch, STATIC.InspMat[ch].Clone(), false, 1,res[i], true);
                        //Vision.MilList[port].GrabCommon(i);
                        ////Vision.MilList[port].InvertImage(i, false, true, ref autoThresh);
                        ////Vision.MilList[i].FineCOG(0, "CheckArea", res[i], false, true);
                        //Vision.MilList[port].NewFineCOG(false, i, InspectionType.Area, res[i], false, false, 0);

                        //if (res[i].area[0][0] > 15)
                        //{
                        //    Vision.MilList[port].GrabCommon(i);
                        //    Vision.MilList[port].FineCOG(i, "OpenHexagon", res[i], false);
                        //}
                        //Vision.MilList[port].GrabCommon(i);
                        //Vision.MilList[port].FineCOG(i, "OpenHexagon", res[i], false);
                    }

                    if (m_ChannelOn[ch])
                    {
                        Area[i] = (float)res[i].Area;
                        Log.AddLog(ch, String.Format("Targetpos {0}, current {1:0.00}, IrisHall {2}, area {3:0.00} ", targPos[i], res[i].current, res[i].IrisHall, Area[i]));
                        log += string.Format("{0},{1},{2:0.0000},{3:0.0000},,", targPos[i], res[i].current, res[i].IrisHall, Area[i]);

                    }
                    if (Option.ReverseDrv)
                        temp_table -= step_size;
                    else
                        temp_table += step_size;
                    arry.Add(log);
                }

                arry.Add("\r\n");

                Log.AddLog(ch, string.Format("\r\n"));
             
                Array.Copy(targPos, targPos2, targPos.Length);
                Array.Reverse(targPos2);
                res.Clear();
                for (i = 0; i < targPos2.Length; i++)
                {
                    log = string.Empty;
                    res.Add(new InspResult());

                    if (m_ChannelOn[ch])
                        DrvIC.Move(ch, DriverIC.AK, (int)targPos2[i]);
                    Wait(Condition.LinCompDelay);
                    if (m_ChannelOn[ch])
                    {
                        res[i].current = Dln.GetCurrent(ch);
                        res[i].IrisHall = DrvIC.ReadHall(ch, DriverIC.AK);
                    }

                    if (!Option.isActMode)
                    {
                        Cam.CamList[ch].Acquire();
                        
                        
                        res[i].Area = InspApi[ch].JustCheckArea(ch, STATIC.InspMat[ch].Clone(), false);
                      
                        
                    //    Vision.MilList[port].GrabCommon(i);
                    ////    Vision.MilList[port].InvertImage(i, false, true, ref autoThresh);
                    //    //  Vision.MilList[port].FineCOG(i, "CheckArea", res[i], false, true);
                    //    Vision.MilList[port].NewFineCOG(false, i, InspectionType.Area, res[i], false, false, 0);
                    }
                    else
                    {
                        Cam.CamList[ch].Acquire();
                        InspApi[ch].TestCDll2(ch, STATIC.InspMat[ch].Clone(), false, 1, res[i], true);
                        //Vision.MilList[port].GrabCommon(i);
                        ////Vision.MilList[port].InvertImage(i, false, true, ref autoThresh);
                        ////Vision.MilList[i].FineCOG(0, "CheckArea", res[i], false, true);
                        //Vision.MilList[port].NewFineCOG(false, i, InspectionType.Area, res[i], false, false, 0);
                        //if (res[i].area[0][0] > 15)
                        //{
                        //    Vision.MilList[port].GrabCommon(i);
                        //    Vision.MilList[port].FineCOG(i, "OpenHexagon", res[i], false);
                        //}
                        //Vision.MilList[port].GrabCommon(i);
                        //Vision.MilList[port].FineCOG(i, "OpenHexagon", res[i], false);
                    }
                    if (m_ChannelOn[ch])
                    {
                        Area2[i] = (float)res[i].Area;
                        Log.AddLog(ch, String.Format("Targetpos {0}, current {1:0.00}, IrisHall {2}, area {3:0.00} ", targPos2[i], res[i].current, res[i].IrisHall, Area2[i]));
                        log += string.Format("{0},{1},{2:0.0000},{3:0.0000},,", targPos2[i], res[i].current, res[i].IrisHall, Area2[i]);
                    }
                    arry.Add(log);
                }
                if (path != "" && Option.SaveRawData) STATIC.SetTextLine(path, arry);
                if (m_ChannelOn[ch])
                    Array.Reverse(Area2);

                for (int k = 0; k < Area_Avg.Length; k++)
                {
                    if (m_ChannelOn[ch])
                    {
                       // Area_Avg[k] = (float)((Area[k] + Area2[k]) / Condition.Lintemp);
                        Log.AddLog(ch, String.Format("area Avg {0:0.00} ", Area_Avg[k]));
                    }

                }



                if (m_ChannelOn[ch])
                {
                    valueStep = (Area_Avg[step - e_value] - Area_Avg[s_value]) / (valueStepsize);
                    valueLensPosi[0] = Area_Avg[s_value];
                    valueLensPosi[valueStepsize] = Area_Avg[s_value + valueStepsize];

                    for (i = 1; i < valueStepsize; i++)
                    {
                        valueLensPosi[i] = valueLensPosi[i - 1] + valueStep;

                        valuegap = valueLensPosi[i] - Area_Avg[i + s_value];

                        if (valuegap >= 0) { }

                        else { valuegap *= -1; }
                        if (max_valuegap < valuegap)
                            max_valuegap = valuegap;
                    }
                    if (max_valuegap > linear_spec)

                    {
                        // Main funciton of linearity compensation coefficient calculation.
                        if (targPos.Length == Area_Avg.Length)

                        {
                            numLinCompData = targPos.Length;//sizeof(targPosi) / sizeof(float);

                            if (Option.ReverseDrv)
                            {
                                Array.Copy(targPos, targPos3, targPos.Length);
                                Array.Reverse(targPos3);

                                Array.Copy(Area_Avg, AreaAvgTmp, Area_Avg.Length);
                                Array.Reverse(AreaAvgTmp);
                                result = InspApi[ch].LinCompMain(targPos3, AreaAvgTmp, numLinCompData, pVt, nVt, ignInf, ignMac, linCoef, ref resError, isRev);
                            }
                            else
                                result = InspApi[ch].LinCompMain(targPos, Area_Avg, numLinCompData, pVt, nVt, ignInf, ignMac, linCoef, ref resError, Option.ReverseDrv);

                            //   printf("numLinCompData = %d\n", numLinCompData);


                            if (result != 0)
                                return false;
                        }
                        else
                            return false;

                        DrvIC.Move(ch, DriverIC.AK, 2048);
                        Thread.Sleep(50);

                       
                        // save coefficient
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[1] { 0x3B });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xC5, 1, new byte[1] { (byte)linCoef[0] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xC6, 1, new byte[1] { (byte)linCoef[1] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xC7, 1, new byte[1] { (byte)linCoef[2] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xC8, 1, new byte[1] { (byte)linCoef[3] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xC9, 1, new byte[1] { (byte)linCoef[4] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xCA, 1, new byte[1] { (byte)linCoef[5] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xCB, 1, new byte[1] { (byte)linCoef[6] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xCC, 1, new byte[1] { (byte)linCoef[7] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xCD, 1, new byte[1] { (byte)linCoef[8] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xCE, 1, new byte[1] { (byte)linCoef[9] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xCF, 1, new byte[1] { (byte)linCoef[10] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD0, 1, new byte[1] { (byte)linCoef[11] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD1, 1, new byte[1] { (byte)linCoef[12] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD2, 1, new byte[1] { (byte)linCoef[13] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD3, 1, new byte[1] { (byte)linCoef[14] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD4, 1, new byte[1] { (byte)linCoef[15] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD5, 1, new byte[1] { (byte)linCoef[16] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD6, 1, new byte[1] { (byte)linCoef[17] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD7, 1, new byte[1] { (byte)linCoef[18] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD8, 1, new byte[1] { (byte)linCoef[19] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xD9, 1, new byte[1] { (byte)linCoef[20] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xDA, 1, new byte[1] { (byte)linCoef[21] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xDB, 1, new byte[1] { (byte)linCoef[22] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xDC, 1, new byte[1] { (byte)linCoef[23] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xDD, 1, new byte[1] { (byte)linCoef[24] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xDE, 1, new byte[1] { (byte)linCoef[25] });
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xDF, 1, new byte[1] { (byte)linCoef[26] });

                        for (int k = 0; k < 27; k++)
                        {
                            Log.AddLog(ch, string.Format("mAddr: 0x{0:X2} \t Wdata: {1}", 197 + k, (byte)linCoef[k]));
                        }

                        AK7316_memory_upadate(ch, 4);
                        DrvIC.Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[1] { 0x00 });
                        Thread.Sleep(50);
                       
                    }
                }

                return true;
            }
            catch(Exception ex)
            { 
                MessageBox.Show(ex.ToString());
                return false;
            }
         
        }

      
        #endregion
        void TemperCheck(int ch)
        {
            byte[] Temp = new byte[3];
            for (int i = 0; i < 3; i++)
            {
                byte[] rBuf = new byte[1];
                byte cTemp, cBackUp;
                Dln.ReadArray(ch, DrvIC.AkSlave[ch], 0x90, 1, rBuf);
                cBackUp = cTemp = rBuf[0];
                cBackUp &= 0x80;
                cTemp = (byte)((cTemp << 1) & 0x7E);
                cTemp |= cBackUp;
                Dln.ReadArray(ch, DrvIC.AkSlave[ch], 0x91, 1, rBuf);
                cBackUp = rBuf[0];
                cBackUp &= 0x80;
                cTemp |= (byte)(cBackUp >> 7);
                Temp[i] = cTemp;
                Thread.Sleep(10);
            }
            Log.AddLog(ch, string.Format("Temp = 0x{0:X2}, 0x{1:X2}, 0x{2:X2}", Temp[0], Temp[1], Temp[2]));
        }
        void ChangeSlaveAddr(int ch, string testItem)
        {
            byte[] addr = { 0x18, 0x1E, 0xE8, 0x98 }; byte icAddr_temp = 0xFF;
            byte temp;
            byte[] i2c_2nd = new byte[2];
            byte[] rbuf = new byte[1];

            Dln.ReadArray(ch, DrvIC.AkSlave[ch], 0x03, 1, rbuf);
            if (rbuf[0] == 0x13)
            {
                Log.AddLog(ch, $"IC address check OK");
                return;
            }
            for (int i = 0; i < 4; i++)
            {
                rbuf[0] = 0xFF;
                Dln.ReadArray(ch, addr[i] >> 1, 0x03, 1, rbuf);
                if (rbuf[0] == 0x1C)
                {
                    icAddr_temp = (byte)(addr[i] >> 1);
                    break;
                }
                //else Log.AddLog(ch, "i2c NG");

            }
            if (icAddr_temp != 0Xff)
            {
                Dln.WriteArray(ch, icAddr_temp, 0x02, 1, new byte[] { 0x40 });
                Thread.Sleep(5);
                Dln.WriteArray(ch, icAddr_temp, 0xAE, 1, new byte[] { 0x3B });
                Dln.WriteArray(ch, icAddr_temp, 0x0A, 1, new byte[] { 0x80 });
            }
            AK7316_memory_upadate(ch, 1);
            AK7316_memory_upadate(ch, 5);
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[] { 0x00 });

        }

        bool HallAdjustment(int ch)
        {
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x02, 1, new byte[] { 0x40 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[] { 0x3B });
        
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x0B, 1, new byte[] { 0xF2 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x0A, 1, new byte[] { 0x80 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x0E, 1, new byte[] { 0x80 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x0F, 1, new byte[] { 0xC1 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x08, 1, new byte[] { 0x12 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x09, 1, new byte[] { 0x20 });

            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x10, 1, new byte[] { 0x37 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x11, 1, new byte[] { 0x50 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x12, 1, new byte[] { 0x50 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x13, 1, new byte[] { 0x2A });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x14, 1, new byte[] { 0x1B });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x15, 1, new byte[] { 0x00 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x16, 1, new byte[] { 0x20 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x17, 1, new byte[] { 0x64 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x18, 1, new byte[] { 0x99 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x1A, 1, new byte[] { 0x00 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x1B, 1, new byte[] { 0x61 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x1C, 1, new byte[] { 0xDC });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x1D, 1, new byte[] { 0xFB });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x1E, 1, new byte[] { 0x0F });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x1F, 1, new byte[] { 0x28 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x20, 1, new byte[] { 0x00 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x21, 1, new byte[] { 0x50 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x22, 1, new byte[] { 0x00 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x23, 1, new byte[] { 0x64 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x24, 1, new byte[] { 0x00 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x25, 1, new byte[] { 0xDC });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x26, 1, new byte[] { 0xC1 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x27, 1, new byte[] { 0x3A });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x28, 1, new byte[] { 0x7E });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x29, 1, new byte[] { 0xC4 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x2A, 1, new byte[] { 0x16 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x2B, 1, new byte[] { 0x74 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x2C, 1, new byte[] { 0xD2 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x2D, 1, new byte[] { 0x5E });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x2E, 1, new byte[] { 0x17 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x2F, 1, new byte[] { 0x30 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x30, 1, new byte[] { 0x00 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x31, 1, new byte[] { 0x00 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x32, 1, new byte[] { 0x00 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x33, 1, new byte[] { 0x00 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x34, 1, new byte[] { 0x00 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x35, 1, new byte[] { 0x00 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x36, 1, new byte[] { 0x00 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x3C, 1, new byte[] { 0x50 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x3D, 1, new byte[] { 0x09 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x52, 1, new byte[] { 0x14 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x53, 1, new byte[] { 0x50 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x54, 1, new byte[] { 0x14 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x55, 1, new byte[] { 0x32 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x50, 1, new byte[] { 0x46 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x51, 1, new byte[] { 0xD8 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x56, 1, new byte[] { 0x23 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x57, 1, new byte[] { 0xB4 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x58, 1, new byte[] { 0x40 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x59, 1, new byte[] { 0x0B });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x5A, 1, new byte[] { 0x00 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x5B, 1, new byte[] { 0x02 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x5C, 1, new byte[] { 0x32 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x5E, 1, new byte[] { 0x00 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x5F, 1, new byte[] { 0x00 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xC4, 1, new byte[] { 0x32 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x5D, 1, new byte[] { 0x00 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x02, 1, new byte[] { 0x80 });
            Thread.Sleep(1);
            byte[] rbuf = new byte[1];
            byte backup, temp;

            Dln.ReadArray(ch, DrvIC.AkSlave[ch], 0x70, 1, rbuf);
            backup = temp = rbuf[0];
            Log.AddLog(ch, string.Format("1 Reg 0x70 : 0x{0:X2}", temp));
            backup &= 0x80;
            temp = (byte)((temp << 1) & 0x7E);
            temp |= backup;
            Dln.ReadArray(ch, DrvIC.AkSlave[ch], 0x71, 1, rbuf);
            Log.AddLog(ch, string.Format("1 Reg 0x71 : 0x{0:X2}", rbuf[0]));
            backup = rbuf[0];
            backup &= 0x80;
            temp |= (byte)(backup >> 7);
            Log.AddLog(ch, string.Format("Temp : 0x{0:X2}", temp));
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x5D, 1, new byte[] { temp });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x0C, 1, new byte[] { 0x62 });
            Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x02, 1, new byte[] { 0x18 });
            Thread.Sleep(150);
            Dln.ReadArray(ch, DrvIC.AkSlave[ch], 0x19, 1, rbuf);
            rbuf[0] = (byte)((rbuf[0] - 0x80) * 2 + 0x80);
            if (0x80 <= rbuf[0] && rbuf[0] <= 0xB0)
            {
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x19, 1, rbuf);
                //Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xF3, 1, new byte[] { 0x1C });
                //Thread.Sleep(30);
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x03, 1, new byte[] { 0x01 });
                Thread.Sleep(100);
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x03, 1, new byte[] { 0x02 });
                Thread.Sleep(180);
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x03, 1, new byte[] { 0x04 });
                Thread.Sleep(150);
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x03, 1, new byte[] { 0x08 });
                Thread.Sleep(160);

                Dln.ReadArray(ch, DrvIC.AkSlave[ch], 0x4B, 1, rbuf);
                if ((rbuf[0] & 0x02) != 0x00)
                {
                    return false;
                }
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xAE, 1, new byte[] { 0x00 });
                
                Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0x02, 1, new byte[] { 0x40 });
            }
            else
            {
                return false;

            }
            return true;
        }
    }
}