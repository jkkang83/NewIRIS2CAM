using DavinciIRISTester;
using OpenCvSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace M1Wide
{

    public enum InspectionType
    {
        Area,
        FindCover,
        InCircle_Center,
        Area_Center,
        InCircle_Decenter,
        Area_Decenter,
        Area_CircleAccuracy,
        Area_ShapeAccuracy,
        InCircle_CircleAccuracy,
        InCircle_ShapeAccuracy,
        JustFind_Vertex,
        Area_InspAll,
        InCircle_InspAll,
        C_Dll,
    }
    public static class STATIC
    {
        public static int CamDefaultWidth = 3088;
        public static int CamDefaultHeight = 2064;
        public static int DefaultWidth = 1800;
        public static int DefaultHeight = 1800;
        public static double DefaultResolution = 0.0048;
        public static F_Vision fVision = new F_Vision();
        public static F_Manage fManage = new F_Manage();
        public static F_Motion fMotion = new F_Motion();
        public static DateTime LastDate = new DateTime();
        public static string VerInfo = "IRIS Tester : Ver. 25122901";
        public static Mat[] ResMat = new Mat[2] { new Mat(), new Mat() };

        public static List<List<Mat>> ResMatOnProcess = new List<List<Mat>>();
        public static List<string[]> ErrMsg = new List<string[]>() { new string[1], new string[1] };
     
        public static double[] ScaleResolution = new double[2] { DefaultResolution, DefaultResolution };
      
        public static byte[] Read_Temp = new byte[2] { 0x00, 0x00 };
        public static byte ReadData = 0x00;
        public static DateTime[] WriteICDateTime = new DateTime[2] { new DateTime(), new DateTime() };
        //public static List<PointF> CoverPos = new List<PointF>() { new PointF(), new PointF() };
        //public static List<PointF> C_CoverPos = new List<PointF>() { new PointF(), new PointF() };
        public static List<PointF> GapSpacerPos = new List<PointF>() { new PointF(), new PointF() };
        public static List<PointF> C_GapSpacerPos = new List<PointF>() { new PointF(), new PointF() };
        public static string ManualDrivePath = string.Empty;
        public static int SaveImageItrCnt = 0;
        public static int SaveImageCurrentPos = 0;

        public static double[] OC_F20_CCircleAccu = new double[2];
        public static double[] CO_F20_CCircleAccu = new double[2];
     
        public static double[] OC_F40_CCircleAccu = new double[2];
        public static double[] CO_F40_CCircleAccu = new double[2];
      
        public static double[] OC_F20_CDecenter = new double[2];
        public static double[] CO_F20_CDecenter = new double[2];

        public static bool[] isNonSpecError = new bool[2] { false, false };
       

        public static string[] Step10Name = new string[20] { "F14", "F1416", "F1618", "F1820", "F2022", "F2225", "F2528", "F2832", "F3235", "F3540",
                                                            "F40", "F4035", "F3532", "F3228", "F2825", "F2522", "F2220", "F2018", "F1816", "F1614" };

        public static string[] Step9Name = new string[18] { "F17", "F1718", "F1820", "F2022", "F2225", "F2528", "F2832", "F3235", "F3540",
                                                            "F40", "F4035", "F3532", "F3228", "F2825", "F2522", "F2220", "F2018", "F1817"};

        public static string[] Step4Name_N2 = new string[8] { "F14","F1420", "F2028", "F2840", "F40", "F4028", "F2820", "F2014" };


        public static string[] Step4Name_N1 = new string[8] { "F17", "F1720", "F2028", "F2840", "F40", "F4028", "F2820", "F2017" };

        public static bool isTmpLog = false;
        public static string CommonLog = string.Empty;
        public static string ContactLog = string.Empty;
        public static string ResolutionLog = string.Empty;
        public static string RepeatLog = string.Empty;
        public static string InspLog = string.Empty;
        public static double F40CoverDia = 0;
    
        public static int[] beforePcal = new int[2];
        public static int[] beforeNcal = new int[2];
        public static int[] afterPcal = new int[2];
        public static int[] afterNcal = new int[2];
        public static string PIDName = string.Empty;
        public static double[] LastStepArea = new double[2];
        public static double[] LastStep_1Area = new double[2];
        public static double[] LastStep_2Area = new double[2];
        public static bool isYieldStretch = false;
        public static int[] OCHallDiff = new int[2];
        public static int[] COHallDiff = new int[2];
        public static double DecenterX_Pos1 = 0;
        public static double DecenterY_Pos1 = 0;
        public static double CDecenterX_Pos1 = 0;
        public static double CDecenterY_Pos1 = 0;

        //public static double[] COScanLinearityDiff = new double[2];
        //public static double[] COScanLinearityMax = new double[2];
        //public static double[] OCScanLinearityDiff = new double[2];
        //public static double[] OCScanLinearityMax = new double[2];

        //public static int[] COScanLinearityDiffHall = new int[2];
        //public static int[] COScanLinearityMaxHall = new int[2];
        //public static int[] OCScanLinearityDiffHall = new int[2];
        //public static int[] OCScanLinearityMaxHall = new int[2];
        public enum STATE
        {
            Manage,
            Main,
            Vision,
            Motion,
        }
        private static int state = 0;
        public static int State
        {
            get { return state; }
            set { if (state != value) state = value; StateChange?.Invoke(null, EventArgs.Empty); }
        }

        public static event EventHandler StateChange = null;

        public static string BaseDir = "C:\\IRISData";
        public static string RootDir = BaseDir + "\\DoNotTouch\\";
        public static string DataDir = BaseDir + "\\Data";
        public static string UserScriptDir = BaseDir + "\\DriverIC\\FW\\";
        public static string LastDateFile = RootDir + "LastDate.txt";
        public static string YieldPath = DataDir + "\\Yield\\";
      
        public static void SetTextLine(string path, List<string> list)
        {
            try
            {
                string FilePath = path;
                //if (!File.Exists(FilePath)) return;
                StreamWriter sw = new StreamWriter(FilePath);
                for (int i = 0; i < list.Count; i++)
                { sw.WriteLine(list[i]); }
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void SetTextLine(string path, string msg)
        {
            try
            {
                string FilePath = path;
                StreamWriter sw = new StreamWriter(FilePath);
                sw.WriteLine(msg);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static List<string> GetTextAll(string path)
        {
            List<string> result = new List<string>();
            string FilePath = path;
            if (!File.Exists(FilePath)) return null;
            StreamReader sr = new StreamReader(FilePath);
            while (sr.Peek() >= 0)
            {
                result.Add(sr.ReadLine());
            }
            sr.Close();
            return result;
        }
        public static string OpenFile(string InitDir, string ext, bool save = false)
        {
            FileDialog op;
            if (save) op = new SaveFileDialog();
            else op = new OpenFileDialog();

            op.InitialDirectory = InitDir;
            if (ext != "") ext = ext.Remove(0, 1);
            op.Filter = "*." + ext + "|*." + ext;
            if (op.ShowDialog() == DialogResult.OK)
                return op.FileName;
            else return null;
        }
        public static string CreateDateDir()
        {
            DateTime dt = DateTime.Now;
            string dir = string.Format("{0}\\{1}\\{2}\\{3}\\", DataDir, dt.Year, dt.Month, dt.Day);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }

        public static LogEvent LogEvent = new LogEvent();
        public static ShowEvent ShowEvent = new ShowEvent();
        public static ChartEvent ChartEvent = new ChartEvent();
        public static Recipe Rcp = new Recipe();
        public static Process Process = new Process();
        public static DLN Dln = new DLN();
        public static DriverIC DrvIC { get; set; }
        public static Camera Camera;
        public static InspectionApi[] InspectionApi = new InspectionApi[2] { new InspectionApi(), new InspectionApi() };
       
        public static Mat[] InspMat = new Mat[2] { new Mat(), new Mat()};


        #region Lincomp Const
        public const int PRE_ERROR = 5000; // initial error vaule	
        public const int ERROR_TH = 0; // threshold error vaule	
        public const int VT = 1;
        public const int OptParamX = 0;    // 0: Value, 1: Slope
        public const int OptXFix = 31; // OptXCandidate num
        public const int OptXCan = 32; // OptXCandidate num
        public const int OptALL = 1;   // OptParamALL ON/OFF
        public const int OptALLNum = 2;    // optimize y param
        public const int OptALLTimes = 200;   // optimize y param times
        public const int RANGE = 4;    // optimize y param range
        public const int MAX_NUM_DATA = 65;    // maximum number of data after Interpolation
        public const int MAX_NUM_DATA2 = 65;   // maximum number of data after Averaging	
        public const int MIN_NUM_DATA = 6; // minimum number of data
        public const int NUM_COEF = 27;    // size of output data array
        public const int NUM_COEF2_1 = 35;     // size of output data array
        public const int NUM_COEF2_2 = 38; // size of output data array
        public const int BIT_COEF2 = 7;
        public const int BIT_COEF2G = 2;
        public const int ERROR_PREDATA = 1;    // PreData() error
        public const int ERROR_CONVDATA = 2;   // ConvData() error
        public const int ERROR_CALCOEF2 = 3;   // CalLinCompCoef2() error
        public const int ERROR_CONVREG = 4;// ConvReg() error
        #endregion
    }
}
