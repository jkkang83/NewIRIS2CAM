using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

namespace M1Wide
{
    public class Recipe
    {
        public CurrentPath Current { get; set; }
        public Condition Condition { get; set; }
        public Spec Spec { get; set; }
        public Option Option { get; set; }
        public Model Model { get; set; }
        public VisionSettingFile VisionFile { get; set; }
        public DecenterScale Dscale { get; set; }
        public F16AreaScale F16Scale { get; set; }
        public Password PW { get; set; }
        public Recipe()
        {
            Current = new CurrentPath();
            Option = new Option();
            Model = new Model();
            Condition = new Condition();
            Spec = new Spec();
            VisionFile = new VisionSettingFile();
            Dscale = new DecenterScale();
            F16Scale = new F16AreaScale();
            PW = new Password();
            Condition.Init(Current.ConditionName, "\\Recipe\\");
            Spec.Init(Current.SpecName, "\\Spec\\");
        }
    }
    public class BaseRecipe
    {
        public List<object[]> Param = new List<object[]>();
        public string CurrentName { get; set; }
        public string FilePath { get; set; }
        public string[] ReadArry { get; set; }
        public bool bChange = false;
        public string InitDir { get; set; }
        public string Ext { get; set; }
        public virtual void Init(string current, string subDir)
        {
            if (!Directory.Exists(STATIC.BaseDir)) Directory.CreateDirectory(STATIC.BaseDir);
            InitDir = STATIC.BaseDir + subDir;
            Ext = Path.GetExtension(current);
            if (!Directory.Exists(InitDir)) Directory.CreateDirectory(InitDir);
            FilePath = STATIC.BaseDir + subDir + current;

            CurrentName = current;
            if (!File.Exists(FilePath)) Save();

            Read();
        }
        public virtual void Save(string filePath = "")
        {
        }
        public virtual void Read(string filePath = "")
        {
            if (!Directory.Exists(STATIC.RootDir)) Directory.CreateDirectory(STATIC.RootDir);
        }
        public virtual void SetParam()
        {
        }
        public virtual void SetParam(string key, string comment, object val)
        {
            for (int i = 0; i < Param.Count; i++)
            {
                if (Param[i][0].ToString() == key && Param[i][1].ToString() == comment)
                {
                    Param[i][2] = val;
                }
                if (Param[i][0].ToString() == key && comment == "")
                {
                    Param[i][1] = val;
                }
            }
        }
    }
    public class Condition : BaseRecipe
    {
        public const int KEY = 0;
        public const int COMMENT = 1;
        public const int VAL = 2;
        public const int UNIT = 3;

    
    
        public int iDrvStepInterval;
    
        //public int iXOIS_POS1;
        //public int iXOIS_POS2;
        //public int iXOIS_POS3;
        //public int iXOIS_POS4;
        //public int iXOIS_POS5;
        //public int iYOIS_POS1;
        //public int iYOIS_POS2;
        //public int iYOIS_POS3;
        //public int iYOIS_POS4;
        //public int iYOIS_POS5;
        //public int iAF_POS;
        public double IRISCalAreaF14;
        public double IRISCalAreaF1416;
        public double IRISCalAreaF1618;
        public double IRISCalAreaF1820;
        public double IRISCalAreaF2022;
        public double IRISCalAreaF2225;
        public double IRISCalAreaF2528;
        public double IRISCalAreaF2832;
        public double IRISCalAreaF3235;
        public double IRISCalAreaF3540;
        public double IRISCalAreaF40;
        public double IRISCalAreaF4035;
        public double IRISCalAreaF3532;
        public double IRISCalAreaF3228;
        public double IRISCalAreaF2825;
        public double IRISCalAreaF2522;
        public double IRISCalAreaF2220;
        public double IRISCalAreaF2018;
        public double IRISCalAreaF1816;
        public double IRISCalAreaF1614;

        public int IRISCalArea1PosCode;
        public int IRISCalArea2PosCode;
        public int IRISCalArea1NegCode;
        public int IRISCalArea2NegCode;
        public int IRISCalCodeStep;
        public int IRISDrvInterval;

        public int LinCompStart;
        public int LinCompEnd;
        public int LinCompCount;
        public int LinCompDelay;

     //   public int CodeOffset_14;

        //public byte CalVal1;
        //public byte CalVal2;
        //public byte CalVal3;
        //public byte CalVal4;
        //public byte CalVal5;
       // public double Lintemp;
        public int ItrCnt;
        public int AgingCnt;
       
        public int Maximum14Pos;
        public int Maximum40Pos;
     
        public int  OpenLoopDelay;
        public double OpenLoopDownCurrent;
        public double OpenLoopDownCurrent_2;
        public double OpenLoopDownCurrent40;
        public double OpenLoopDownCurrent40_2;

        //public int SoftLandingCode;
        //public int SoftLandingStep1;
        //public int SoftLandingStep2;
        //public int SoftLandingStep3;
        //public int SoftLandingStep4;
        //public int SoftLandingDelay;

        //public int BackStep_Code;
        //public int BackStep_Step;
        //public int BackStepDelay1;
        //public int BackStepDelay2;
        //public int BackStep40Offset;

        //public int PM_FinalFreq;
        //public int PM_StartFreq;
        //public int PM_StepFreq;
        //public int PM_Amp;
        //public int PM_Gainthr;

        public int SearchOption;
        public double SearchOption4Percent;
        public double SearchOption4Percent2;
        public int ReadyPosOffset14;
        public int ReadyPosOffset40;
      //  public double SettlingUnderPer;
       // public int F16F18CodeLimit;
        public int F40CodeLimit;
    //    public int SearchHallDiffError;

        //public double F20SGradeDecenter;
        //public double F20AGradeDecenter;
        //public double F20SGradeCA;
        //public double F20AGradeCA;
        //public double F40SGradeCA;
        //public double F40AGradeCA;
        //public double F20SABGradeCArea;

        //public double ScanLinearSpec;
        //public double ScanLinearMaxSpec;
        //public double ScanLinearExclude;

        public int F4028MoveCode;
        public int F4028MoveDelay;

        public int OpenLoopOptionDelay;
        public int OCDelay;
        public int OpenLoopAgingDelay;
        public int F20_28Offset;
        public int SortNo;

        public int PosEPANo;
        public int NegEPANo;






        public ObservableCollection<string> ToDoList = new ObservableCollection<string>();

        public Condition()
        {
          
         
            Param.Add(new object[] { "Iris", "Out Interval", "50", "msec" });        
         
            //Param.Add(new object[] { "OIS X", "POS_1", "0", "code" });
            //Param.Add(new object[] { "OIS X", "POS_2", "0", "code" });
            //Param.Add(new object[] { "OIS X", "POS_3", "0", "code" });
            //Param.Add(new object[] { "OIS X", "POS_4", "0", "code" });
            //Param.Add(new object[] { "OIS X", "POS_5", "0", "code" });
            //Param.Add(new object[] { "OIS Y", "POS_1", "0", "code" });
            //Param.Add(new object[] { "OIS Y", "POS_2", "0", "code" });
            //Param.Add(new object[] { "OIS Y", "POS_3", "0", "code" });
            //Param.Add(new object[] { "OIS Y", "POS_4", "0", "code" });
            //Param.Add(new object[] { "OIS Y", "POS_5", "0", "code" });
            //Param.Add(new object[] { "AF", "POS", "0", "code" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#14", "22.73", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#1416", "13.69", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#1618", "10.82", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#1820", "8.76", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#2022", "7.24", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#2225", "5.61", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#2528", "4.47", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#2832", "3.42", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#3235", "2.86", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#3540", "2.19", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#40", "2.19", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#4035", "2.86", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#3532", "3.42", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#3228", "4.47", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#2825", "5.61", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#2522", "7.24", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#2220", "8.76", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#2018", "10.82", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#1816", "13.69", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal 10Step", "F#1614", "22.73", "mm\xB2" });
            Param.Add(new object[] { "IRIS Search Cal ", "Cal Code From", "0", "code" });
            Param.Add(new object[] { "IRIS Search Cal ", "Cal Code to", "4095", "code" });      
            Param.Add(new object[] { "IRIS Search Cal ", "CodeStep", "32", "code" });
            Param.Add(new object[] { "IRIS Search Cal ", "Interval", "50", "ms" });
      //      Param.Add(new object[] { "IRIS #1.4 Offset", "Offset", "0", "code" });
            Param.Add(new object[] { "IRIS Lin Comp", "Start", "0", "code" });
            Param.Add(new object[] { "IRIS Lin Comp", "End", "4095", "code" });
            Param.Add(new object[] { "IRIS Lin Comp", "Step Count", "63", "cnt" });
            Param.Add(new object[] { "IRIS Lin Comp", "delay", "50", "ms" });
            //Param.Add(new object[] { "cal_Val", "0x0C", "0", "" });
            //Param.Add(new object[] { "cal_Val", "0xC0", "0", "" });
            //Param.Add(new object[] { "cal_Val", "0xC1", "0", "" });
            //Param.Add(new object[] { "cal_Val", "0xC2", "0", "" });
            //Param.Add(new object[] { "cal_Val", "0xC3", "0", "" });
            //Param.Add(new object[] { "cal_Val", "LinTmp", "2", "" });
            Param.Add(new object[] { "Iteration", "count", "3", "cnt" });
            Param.Add(new object[] { "Open Aging", "count", "1", "cnt" });
            Param.Add(new object[] { "Limit #1.4 Pos", "code", "50", "code" });
            Param.Add(new object[] { "Limit #4.0 Pos", "code", "50", "code" });      
            Param.Add(new object[] { "OpenLoop Drv", "delay", "100", "ms" });
            Param.Add(new object[] { "OpenLoop Drv", "#1.4 Down Current", "2048", "code" });
            Param.Add(new object[] { "OpenLoop Drv", "#1.4 Down Current_2(0x00)", "2048", "code" });
            Param.Add(new object[] { "OpenLoop Drv", "#4.0 Down Current", "2048", "code" });
            Param.Add(new object[] { "OpenLoop Drv", "#4.0 Down Current_2(0x00)", "4095", "code" });
            //Param.Add(new object[] { "SoftLanding", "Code Value", "1000", "code" });
            //Param.Add(new object[] { "SoftLanding", "Step1", "60", "%" });
            //Param.Add(new object[] { "SoftLanding", "Step2", "70", "%" });
            //Param.Add(new object[] { "SoftLanding", "Step3", "80", "%" });
            //Param.Add(new object[] { "SoftLanding", "Step4", "90", "%" });
            //Param.Add(new object[] { "SoftLanding", "Delay", "5", "ms" });
            //Param.Add(new object[] { "BackStep", "Code", "500", "code" });
            //Param.Add(new object[] { "BackStep", "Step", "5", "code" });
            //Param.Add(new object[] { "BackStep", "Delay1", "5", "cnt" });
            //Param.Add(new object[] { "BackStep", "Delay2", "5", "ms" });
            //Param.Add(new object[] { "BackStep", "Offset", "50", "code" });
            //Param.Add(new object[] { "P/M", "Start Freq", "150", "Hz" });
            //Param.Add(new object[] { "P/M", "End Freq", "30", "Hz" });
            //Param.Add(new object[] { "P/M", "Step", "10", "Hz" });
            //Param.Add(new object[] { "P/M", "Amp", "100", "" });
            //Param.Add(new object[] { "P/M", "Gain Thr", "10", "" });
            Param.Add(new object[] { "Search Option", "Option", "1", "" });
            Param.Add(new object[] { "Search Option", "Num 4 Percent", "50", "%" });
            Param.Add(new object[] { "Search Option", "Num 4 Percent2", "50", "%" });
            Param.Add(new object[] { "ReadyPos Offset", "#1.4", "50", "code" });
            Param.Add(new object[] { "ReadyPos Offset", "#4.0", "50", "code" });
          //  Param.Add(new object[] { "Settling", "Settling UnderPnt", "90", "%" });
         //   Param.Add(new object[] { "Code Limit", "#16 / #18 Limit", "1500", "code" });
            Param.Add(new object[] { "Code Limit", "#40 Limit", "3700", "code" });
        //    Param.Add(new object[] { "Hall Diff Error", "Code", "50", "code" });
            //Param.Add(new object[] { "F20 S Grade Decenter", "Grade", "50", "um" });
            //Param.Add(new object[] { "F20 A Grade Decenter", "Grade", "50", "um" });
            //Param.Add(new object[] { "F20 S Grade CA", "Grade", "50", "um" });
            //Param.Add(new object[] { "F20 A Grade CA", "Grade", "50", "um" });
            //Param.Add(new object[] { "F40 S Grade CA", "Grade", "50", "um" });
            //Param.Add(new object[] { "F40 A Grade CA", "Grade", "50", "um" });
            //Param.Add(new object[] { "F20 SAB Grade C_AREA", "Grade", "5.6", "%" });
            //Param.Add(new object[] { "ScanLinearity", "Reverse Spec", "0.5", "um" });
            //Param.Add(new object[] { "ScanLinearity", "Max Spec", "0.5", "um" });
            //Param.Add(new object[] { "ScanLinearity", "Exclude", "2", "Step" });
            //Param.Add(new object[] { "F4028Move", "Code", "0", "code" });
            //Param.Add(new object[] { "F4028Move", "delay", "0", "ms" });
            Param.Add(new object[] { "openLoopDelay", "option delay", "0", "ms" });
            Param.Add(new object[] { "openLoopDelay", "open to close Delay", "0", "ms" });
            Param.Add(new object[] { "openLoopDelay", "Open Loop Aging Delay", "0", "ms" });
            Param.Add(new object[] { "F20_28 Offset", "Offset", "0", "ms" });
            Param.Add(new object[] { "SortNo", "Sort No.", "0", "dec" });
            Param.Add(new object[] { "EPA", "Pos EPA No.", "0", "dec" });
            Param.Add(new object[] { "EPA", "Neg EPA No.", "0", "dec" });
        }
        public override void Save(string filePath = "")
        {
            if (filePath != "") FilePath = filePath;
            StreamWriter sw = new StreamWriter(FilePath);

            string data = "";
            for (int i = 0; i < ToDoList.Count; i++)
            {
                data += string.Format("{0}\t", ToDoList[i]);
            }
            if (data != "") data = data.Remove(data.Length - 1);
            sw.WriteLine(data);

            for (int i = 0; i < Param.Count; i++)
            {
                data = string.Format("{0}\t{1}", Param[i][COMMENT], Param[i][VAL]);
                sw.WriteLine(data);
            }
            sw.Close();

            bChange = false;

            Read();
        }
        public override void Read(string filePath = "")
        {
            try
            {
                if (filePath != "")
                {
                    FilePath = filePath;
                    CurrentName = Path.GetFileName(FilePath);
                }
                StreamReader sr = new StreamReader(FilePath);

                ReadArry = sr.ReadToEnd().Split('\r');
                ToDoList.Clear();

                string[] actionArry = ReadArry[0].Split('\t');
                for (int i = 0; i < actionArry.Length; i++)
                {
                    if (actionArry[i] != "") ToDoList.Add(actionArry[i]);
                }

                int Paramindex = 0;

                for (int i = 1; i < ReadArry.Length; i++)
                {
                    string[] arr = ReadArry[i].Split('\t');
                    for (int j = Paramindex; j < Param.Count; j++)
                    {
                        arr[0] = arr[0].Trim();
                        if (arr[0] == Param[j][COMMENT].ToString().Trim())
                        {
                            Param[j][VAL] = arr[1];
                            Paramindex = j + 1;
                            break;
                        }
                        bChange = true;
                    }

                }

                if (Param.Count != ReadArry.Length - 2) bChange = true;

                sr.Close();
                SetParam();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
    
        }
        public override void SetParam()
        {
            int index = 0;
            iDrvStepInterval = Convert.ToInt32(Param[index++][VAL]);
         
            //iXOIS_POS1= Convert.ToInt32(Param[index++][VAL]);
            //iXOIS_POS2 = Convert.ToInt32(Param[index++][VAL]);
            //iXOIS_POS3= Convert.ToInt32(Param[index++][VAL]);
            //iXOIS_POS4= Convert.ToInt32(Param[index++][VAL]);
            //iXOIS_POS5= Convert.ToInt32(Param[index++][VAL]);
            //iYOIS_POS1= Convert.ToInt32(Param[index++][VAL]);
            //iYOIS_POS2= Convert.ToInt32(Param[index++][VAL]);
            //iYOIS_POS3= Convert.ToInt32(Param[index++][VAL]);
            //iYOIS_POS4= Convert.ToInt32(Param[index++][VAL]);
            //iYOIS_POS5 = Convert.ToInt32(Param[index++][VAL]);
            //iAF_POS= Convert.ToInt32(Param[index++][VAL]);
        
            IRISCalAreaF14 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF1416 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF1618 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF1820 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF2022 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF2225 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF2528 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF2832 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF3235 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF3540 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF40 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF4035 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF3532 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF3228 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF2825 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF2522 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF2220 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF2018 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF1816 = Convert.ToDouble(Param[index++][VAL]);
            IRISCalAreaF1614 = Convert.ToDouble(Param[index++][VAL]);
          
            IRISCalArea1PosCode = Convert.ToInt32(Param[index++][VAL]);
            IRISCalArea1NegCode = Convert.ToInt32(Param[index++][VAL]);        
            IRISCalCodeStep = Convert.ToInt32(Param[index++][VAL]);
            IRISDrvInterval = Convert.ToInt32(Param[index++][VAL]);
        //    CodeOffset_14 = Convert.ToInt32(Param[index++][VAL]);
            LinCompStart = Convert.ToInt32(Param[index++][VAL]);
            LinCompEnd = Convert.ToInt32(Param[index++][VAL]);
            LinCompCount = Convert.ToInt32(Param[index++][VAL]);
            LinCompDelay = Convert.ToInt32(Param[index++][VAL]);

            //CalVal1 = Convert.ToByte(Param[index++][VAL]);
            //CalVal2 = Convert.ToByte(Param[index++][VAL]);
            //CalVal3 = Convert.ToByte(Param[index++][VAL]);
            //CalVal4 = Convert.ToByte(Param[index++][VAL]);
            //CalVal5 = Convert.ToByte(Param[index++][VAL]);
            //Lintemp = Convert.ToDouble(Param[index++][VAL]);
            ItrCnt = Convert.ToInt32(Param[index++][VAL]);
            AgingCnt = Convert.ToInt32(Param[index++][VAL]);
         
            Maximum14Pos = Convert.ToInt32(Param[index++][VAL]);
            Maximum40Pos = Convert.ToInt32(Param[index++][VAL]);          
            OpenLoopDelay = Convert.ToInt32(Param[index++][VAL]);
            OpenLoopDownCurrent = Convert.ToDouble(Param[index++][VAL]);
            OpenLoopDownCurrent_2 = Convert.ToDouble(Param[index++][VAL]);
            OpenLoopDownCurrent40 = Convert.ToDouble(Param[index++][VAL]);
            OpenLoopDownCurrent40_2 = Convert.ToDouble(Param[index++][VAL]);

            //SoftLandingCode = Convert.ToInt32(Param[index++][VAL]);
            //SoftLandingStep1 = Convert.ToInt32(Param[index++][VAL]);
            //SoftLandingStep2 = Convert.ToInt32(Param[index++][VAL]);
            //SoftLandingStep3 = Convert.ToInt32(Param[index++][VAL]);
            //SoftLandingStep4 = Convert.ToInt32(Param[index++][VAL]);
            //SoftLandingDelay = Convert.ToInt32(Param[index++][VAL]);
           

            //BackStep_Code = Convert.ToInt32(Param[index++][VAL]);
            //BackStep_Step = Convert.ToInt32(Param[index++][VAL]);
            //BackStepDelay1 = Convert.ToInt32(Param[index++][VAL]);
            //BackStepDelay2 = Convert.ToInt32(Param[index++][VAL]);
            //BackStep40Offset = Convert.ToInt32(Param[index++][VAL]);

            //PM_StartFreq = Convert.ToInt32(Param[index++][VAL]);
            //PM_FinalFreq = Convert.ToInt32(Param[index++][VAL]);
            //PM_StepFreq = Convert.ToInt32(Param[index++][VAL]);
            //PM_Amp = Convert.ToInt32(Param[index++][VAL]);
            //PM_Gainthr = Convert.ToInt32(Param[index++][VAL]);
            SearchOption = Convert.ToInt32(Param[index++][VAL]);
            SearchOption4Percent = Convert.ToDouble(Param[index++][VAL]);
            SearchOption4Percent2 = Convert.ToDouble(Param[index++][VAL]);
            ReadyPosOffset14 = Convert.ToInt32(Param[index++][VAL]);
            ReadyPosOffset40 = Convert.ToInt32(Param[index++][VAL]);
         //   SettlingUnderPer = Convert.ToDouble(Param[index++][VAL]);

          //  F16F18CodeLimit = Convert.ToInt32(Param[index++][VAL]);
            F40CodeLimit = Convert.ToInt32(Param[index++][VAL]);
         //   SearchHallDiffError = Convert.ToInt32(Param[index++][VAL]);
            //F20SGradeDecenter = Convert.ToDouble(Param[index++][VAL]);
            //F20AGradeDecenter = Convert.ToDouble(Param[index++][VAL]);
            //F20SGradeCA = Convert.ToDouble(Param[index++][VAL]);
            //F20AGradeCA = Convert.ToDouble(Param[index++][VAL]);
            //F40SGradeCA = Convert.ToDouble(Param[index++][VAL]);
            //F40AGradeCA = Convert.ToDouble(Param[index++][VAL]);
            //F20SABGradeCArea = Convert.ToDouble(Param[index++][VAL]);

            //ScanLinearSpec = Convert.ToDouble(Param[index++][VAL]);
            //ScanLinearMaxSpec = Convert.ToDouble(Param[index++][VAL]);
            //ScanLinearExclude = Convert.ToDouble(Param[index++][VAL]);

          //  F4028MoveCode = Convert.ToInt32(Param[index++][VAL]);
           // F4028MoveDelay = Convert.ToInt32(Param[index++][VAL]);

            OpenLoopOptionDelay = Convert.ToInt32(Param[index++][VAL]);
            OCDelay = Convert.ToInt32(Param[index++][VAL]);
            OpenLoopAgingDelay = Convert.ToInt32(Param[index++][VAL]);
            F20_28Offset = Convert.ToInt32(Param[index++][VAL]);
            SortNo = Convert.ToInt32(Param[index++][VAL]);
            PosEPANo = Convert.ToInt32(Param[index++][VAL]);
            NegEPANo = Convert.ToInt32(Param[index++][VAL]);
            if (bChange) Save();
        }
    }
    public enum SpecItem
    {
        YEILD,

        POS1_Hall,
        POS1_Area,      
        POS1_Current,
        POS1_DecenterR,
        POS1_DecenterX,
        POS1_DecenterY,
        POS1_CircleAccuracy,
        POS1_ShapeAccuracy,
        POS1_CArea,
        POS1_CDecenterR,
        POS1_CDecenterX,
        POS1_CDecenterY,
        POS1_CCircleAccuracy,
        POS1_CShapeAccuracy,

        POS2_Hall,
        POS2_Area,
        POS2_Current,
        POS2_DecenterR,
        POS2_DecenterX,
        POS2_DecenterY,
        POS2_CircleAccuracy,
        POS2_ShapeAccuracy,
        POS2_CArea,
        POS2_CDecenterR,
        POS2_CDecenterX,
        POS2_CDecenterY,
        POS2_CCircleAccuracy,
        POS2_CShapeAccuracy,

        POS3_Hall,
        POS3_Area,
        POS3_Current,
        POS3_DecenterR,
        POS3_DecenterX,
        POS3_DecenterY,
        POS3_CircleAccuracy,
        POS3_ShapeAccuracy,
        POS3_CArea,
        POS3_CDecenterR,
        POS3_CDecenterX,
        POS3_CDecenterY,
        POS3_CCircleAccuracy,
        POS3_CShapeAccuracy,

        POS4_Hall,
        POS4_Area,
        POS4_Current,
        POS4_DecenterR,
        POS4_DecenterX,
        POS4_DecenterY,
        POS4_CircleAccuracy,
        POS4_ShapeAccuracy,
        POS4_CArea,
        POS4_CDecenterR,
        POS4_CDecenterX,
        POS4_CDecenterY,
        POS4_CCircleAccuracy,
        POS4_CShapeAccuracy,

        POS5_Hall,
        POS5_Area,
        POS5_Current,
        POS5_DecenterR,
        POS5_DecenterX,
        POS5_DecenterY,
        POS5_CircleAccuracy,
        POS5_ShapeAccuracy,
        POS5_CArea,
        POS5_CDecenterR,
        POS5_CDecenterX,
        POS5_CDecenterY,
        POS5_CCircleAccuracy,
        POS5_CShapeAccuracy,

        POS6_Hall,
        POS6_Area,
        POS6_Current,
        POS6_DecenterR,
        POS6_DecenterX,
        POS6_DecenterY,
        POS6_CircleAccuracy,
        POS6_ShapeAccuracy,
        POS6_CArea,
        POS6_CDecenterR,
        POS6_CDecenterX,
        POS6_CDecenterY,
        POS6_CCircleAccuracy,
        POS6_CShapeAccuracy,

        POS7_Hall,
        POS7_Area,       
        POS7_Current,
        POS7_DecenterR,
        POS7_DecenterX,
        POS7_DecenterY,
        POS7_CircleAccuracy,
        POS7_ShapeAccuracy,
        POS7_CArea,
        POS7_CDecenterR,
        POS7_CDecenterX,
        POS7_CDecenterY,
        POS7_CCircleAccuracy,
        POS7_CShapeAccuracy,

        POS8_Hall,
        POS8_Area,
        POS8_Current,
        POS8_DecenterR,
        POS8_DecenterX,
        POS8_DecenterY,
        POS8_CircleAccuracy,
        POS8_ShapeAccuracy,
        POS8_CArea,
        POS8_CDecenterR,
        POS8_CDecenterX,
        POS8_CDecenterY,
        POS8_CCircleAccuracy,
        POS8_CShapeAccuracy,

        POS9_Hall,
        POS9_Area,
        POS9_Current,
        POS9_DecenterR,
        POS9_DecenterX,
        POS9_DecenterY,
        POS9_CircleAccuracy,
        POS9_ShapeAccuracy,
        POS9_CArea,
        POS9_CDecenterR,
        POS9_CDecenterX,
        POS9_CDecenterY,
        POS9_CCircleAccuracy,
        POS9_CShapeAccuracy,

        POS10_Hall,
        POS10_Area,
        POS10_Current,
        POS10_DecenterR,
        POS10_DecenterX,
        POS10_DecenterY,
        POS10_CircleAccuracy,
        POS10_ShapeAccuracy,
        POS10_CArea,
        POS10_CDecenterR,
        POS10_CDecenterX,
        POS10_CDecenterY,
        POS10_CCircleAccuracy,
        POS10_CShapeAccuracy,

        POS11_Hall,
        POS11_Area,
        POS11_Current,
        POS11_DecenterR,
        POS11_DecenterX,
        POS11_DecenterY,
        POS11_CircleAccuracy,
        POS11_ShapeAccuracy,
        POS11_CArea,
        POS11_CDecenterR,
        POS11_CDecenterX,
        POS11_CDecenterY,
        POS11_CCircleAccuracy,
        POS11_CShapeAccuracy,

        POS12_Hall,
        POS12_Area,
        POS12_Current,
        POS12_DecenterR,
        POS12_DecenterX,
        POS12_DecenterY,
        POS12_CircleAccuracy,
        POS12_ShapeAccuracy,
        POS12_CArea,
        POS12_CDecenterR,
        POS12_CDecenterX,
        POS12_CDecenterY,
        POS12_CCircleAccuracy,
        POS12_CShapeAccuracy,

        POS13_Hall,
        POS13_Area,
        POS13_Current,
        POS13_DecenterR,
        POS13_DecenterX,
        POS13_DecenterY,
        POS13_CircleAccuracy,
        POS13_ShapeAccuracy,
        POS13_CArea,
        POS13_CDecenterR,
        POS13_CDecenterX,
        POS13_CDecenterY,
        POS13_CCircleAccuracy,
        POS13_CShapeAccuracy,

        POS14_Hall,
        POS14_Area,
        POS14_Current,
        POS14_DecenterR,
        POS14_DecenterX,
        POS14_DecenterY,
        POS14_CircleAccuracy,
        POS14_ShapeAccuracy,
        POS14_CArea,
        POS14_CDecenterR,
        POS14_CDecenterX,
        POS14_CDecenterY,
        POS14_CCircleAccuracy,
        POS14_CShapeAccuracy,

        POS15_Hall,
        POS15_Area,
        POS15_Current,
        POS15_DecenterR,
        POS15_DecenterX,
        POS15_DecenterY,
        POS15_CircleAccuracy,
        POS15_ShapeAccuracy,
        POS15_CArea,
        POS15_CDecenterR,
        POS15_CDecenterX,
        POS15_CDecenterY,
        POS15_CCircleAccuracy,
        POS15_CShapeAccuracy,

        POS16_Hall,
        POS16_Area,
        POS16_Current,
        POS16_DecenterR,
        POS16_DecenterX,
        POS16_DecenterY,
        POS16_CircleAccuracy,
        POS16_ShapeAccuracy,
        POS16_CArea,
        POS16_CDecenterR,
        POS16_CDecenterX,
        POS16_CDecenterY,
        POS16_CCircleAccuracy,
        POS16_CShapeAccuracy,

        POS17_Hall,
        POS17_Area,
        POS17_Current,
        POS17_DecenterR,
        POS17_DecenterX,
        POS17_DecenterY,
        POS17_CircleAccuracy,
        POS17_ShapeAccuracy,
        POS17_CArea,
        POS17_CDecenterR,
        POS17_CDecenterX,
        POS17_CDecenterY,
        POS17_CCircleAccuracy,
        POS17_CShapeAccuracy,

        POS18_Hall,
        POS18_Area,
        POS18_Current,
        POS18_DecenterR,
        POS18_DecenterX,
        POS18_DecenterY,
        POS18_CircleAccuracy,
        POS18_ShapeAccuracy,
        POS18_CArea,
        POS18_CDecenterR,
        POS18_CDecenterX,
        POS18_CDecenterY,
        POS18_CCircleAccuracy,
        POS18_CShapeAccuracy,

        POS19_Hall,
        POS19_Area,
        POS19_Current,
        POS19_DecenterR,
        POS19_DecenterX,
        POS19_DecenterY,
        POS19_CircleAccuracy,
        POS19_ShapeAccuracy,
        POS19_CArea,
        POS19_CDecenterR,
        POS19_CDecenterX,
        POS19_CDecenterY,
        POS19_CCircleAccuracy,
        POS19_CShapeAccuracy,

        POS20_Hall,
        POS20_Area,
        POS20_Current,
        POS20_DecenterR,
        POS20_DecenterX,
        POS20_DecenterY,
        POS20_CircleAccuracy,
        POS20_ShapeAccuracy,
        POS20_CArea,
        POS20_CDecenterR,
        POS20_CDecenterX,
        POS20_CDecenterY,
        POS20_CCircleAccuracy,
        POS20_CShapeAccuracy,

        F20_Decenter_Diff,
        F20_CDecnter_Diff,

        F1420_Settling,
        F2028_Settling,
        F2840_Settling,
        F4028_Settling,
        F2820_Settling,
        F2014_Settling,

        PhaseMargin,
        PM_Freq,
        CodeOC1500_Area,
        CodeOC1500_CArea,
        CodeCO1500_Area,
        CodeCO1500_CArea,
        Search_Linearity,
   



    };
  
    public class Spec : BaseRecipe
    {
        public const int KEY = 0;
        public const int COMMENT = 1;
        public const int MIN_VAL = 2;
        public const int MAX_VAL = 3;
        public const int UNIT = 4;
        public const int RESET0 = 5;
        public const int RESET1 = 6;
        public const int CPK = 7;
        public const int FAILED = 8;
        public const int USE = 9;
        public class ResultItems
        {
            public double Val = 0;
            public bool bPass = true;
            public string msg = "";
        }
        public class PassFail
        {
            public int FirstFailIndex;
            public string FirstFail;
            public string TotalFail;
            public string TotalTime;
            public List<ResultItems> Results = new List<ResultItems>();
            public List<double> Output = new List<double>();
        }
        public List<PassFail> PassFails = new List<PassFail>();
        public int LastSampleNum;
        public int TotlaTested;
        public int TotlaPassed;
        public int TotlaFailed;
      
        public Spec()
        {
            Param.Add(new object[] { "", "Yeild", "0", "0", "0", "0", "0", "0", "0", "0", false });

            for (int i = 0; i < 20; i++)
            {
                Param.Add(new object[] { "POS" + i, "Hall", "0", "100", "code", "0", "0", "0", "0", true });
                Param.Add(new object[] { "POS" + i, "Area", "0", "100", "mm\xB2", "0", "0", "0", "0", true });
                Param.Add(new object[] { "POS" + i, "Current", "0", "100", "mA", "0", "0", "0", "0", true });
                Param.Add(new object[] { "POS" + i, "Decenter R", "0", "100", "um", "0", "0", "0", "0", true });
                Param.Add(new object[] { "POS" + i, "Decenter X", "0", "100", "um", "0", "0", "0", "0", true });
                Param.Add(new object[] { "POS" + i, "Decenter Y", "0", "100", "um", "0", "0", "0", "0", true });
                if (i != 0 && i != 19)
                    Param.Add(new object[] { "POS" + i, "CircleAccuracy", "0", "100", "um", "0", "0", "0", "0", true });
                else Param.Add(new object[] { "POS" + i, "Intrusion Dist", "0", "100", "um", "0", "0", "0", "0", true });
                Param.Add(new object[] { "POS" + i, "ShapeAccuracy", "0", "100", "um", "0", "0", "0", "0", true });
                Param.Add(new object[] { "POS" + i, "C_Area", "0", "100", "mm\xB2", "0", "0", "0", "0", true });
                Param.Add(new object[] { "POS" + i, "C_Decenter R", "0", "100", "um", "0", "0", "0", "0", true });
                Param.Add(new object[] { "POS" + i, "C_Decenter X", "0", "100", "um", "0", "0", "0", "0", true });
                Param.Add(new object[] { "POS" + i, "C_Decenter Y", "0", "100", "um", "0", "0", "0", "0", true });
                if (i != 0 && i != 19)
                    Param.Add(new object[] { "POS" + i, "C_CircleAccuracy", "0", "100", "um", "0", "0", "0", "0", true });
                else Param.Add(new object[] { "POS" + i, "C_Intrusion Dist", "0", "100", "um", "0", "0", "0", "0", true });
               
                Param.Add(new object[] { "POS" + i, "C_ShapeAccuracy", "0", "100", "um", "0", "0", "0", "0", true });
            }
            Param.Add(new object[] { "F20", "Decenter Diff", "0", "100", "um", "0", "0", "0", "0", true });
            Param.Add(new object[] { "F20", "CDecenter Diff", "0", "100", "um", "0", "0", "0", "0", true });
            Param.Add(new object[] { "F1420", "Settling Time", "0", "100", "sec", "0", "0", "0", "0", true });
            Param.Add(new object[] { "F2028", "Settling Time", "0", "100", "sec", "0", "0", "0", "0", true });
            Param.Add(new object[] { "F2840", "Settling Time", "0", "100", "sec", "0", "0", "0", "0", true });
            Param.Add(new object[] { "F4028", "Settling Time", "0", "100", "sec", "0", "0", "0", "0", true });
            Param.Add(new object[] { "F2820", "Settling Time", "0", "100", "sec", "0", "0", "0", "0", true });
            Param.Add(new object[] { "F2014", "Settling Time", "0", "100", "sec", "0", "0", "0", "0", true });
            Param.Add(new object[] { "PM", "Phase Margin", "0", "100", "deg", "0", "0", "0", "0", true });
            Param.Add(new object[] { "PM", "PM Freq", "0", "100", "Hz", "0", "0", "0", "0", true });
            Param.Add(new object[] { "Code 1500", "OC Area", "0", "100", "mm²", "0", "0", "0", "0", true });
            Param.Add(new object[] { "Code 1500", "OC C_Area", "0", "100", "mm²", "0", "0", "0", "0", true });
            Param.Add(new object[] { "Code 1500", "CO Area", "0", "100", "mm²", "0", "0", "0", "0", true });
            Param.Add(new object[] { "Code 1500", "CO C_Area", "0", "100", "mm²", "0", "0", "0", "0", true });
            Param.Add(new object[] { "Linearity", "Search Linearity", "0", "100", "mm²", "0", "0", "0", "0", true });
        
            for (int i = 0; i < 2; i++)
            {
                PassFails.Add(new PassFail());
                for (int j = 0; j < Param.Count; j++) PassFails[i].Results.Add(new ResultItems());
                for (int j = 0; j < 100; j++) PassFails[i].Output.Add(new double());
            }
        }
        public override void Save(string filePath = "")
        {
            if (filePath != "") FilePath = filePath;
            StreamWriter sw = new StreamWriter(FilePath);

            for (int i = 0; i < Param.Count; i++)
            {
                string data;
                if (i == 0)
                {
                    data = string.Format("{0}\t{1}\t{2}\t{3}\t{4}",
                        Param[i][1],
                        Param[i][2] = LastSampleNum,
                        Param[i][3] = TotlaTested,
                        Param[i][4] = TotlaPassed,
                        Param[i][5] = TotlaFailed);
                     
                        
                }
                else
                {
                    data = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", Param[i][COMMENT], Param[i][MIN_VAL], Param[i][MAX_VAL], Param[i][FAILED], Param[i][USE]);
                }
                sw.WriteLine(data);
            }
            sw.Close();

            Read();

            bChange = true;
        }
        public override void Read(string filePath = "")
        {
            try
            {
                if (filePath != "")
                {
                    FilePath = filePath;
                    CurrentName = Path.GetFileName(FilePath);
                }
                StreamReader sr = new StreamReader(FilePath);

                ReadArry = sr.ReadToEnd().Split('\r');

                int Arryindex = 0;
                int Paramindex = 0;
                while (true)
                {
                    string[] arry = ReadArry[Arryindex].Split('\t');
                    for (int i = 0; i < arry.Length; i++) arry[i] = arry[i].Trim();
                    if (Paramindex == 0 && arry[0] != "Yeild") { Paramindex++; continue; }
                    if (arry[0] == "") { Arryindex++; continue; }
                    if (arry[0] == Param[Paramindex][1].ToString())
                    {
                        if (Paramindex == 0)
                        {
                            LastSampleNum = Convert.ToInt32(arry[1]);
                            TotlaTested = Convert.ToInt32(arry[2]);
                            TotlaPassed = Convert.ToInt32(arry[3]);
                            TotlaFailed = Convert.ToInt32(arry[4]);
                            Param[Paramindex][USE] = false;
                        }
                        else
                        {
                            Param[Paramindex][MIN_VAL] = arry[1];
                            Param[Paramindex][MAX_VAL] = arry[2];
                            Param[Paramindex][FAILED] = arry[3];
                            Param[Paramindex][USE] = arry[4];
                        }
                        Arryindex++;
                    }
                    else
                    {
                        Param[Paramindex][USE] = false;
                    }
                    Paramindex++;
                    if (Paramindex == Param.Count)
                        break;
                }
                sr.Close();
            }
            catch(Exception ex)
            { MessageBox.Show(ex.ToString()); }
     
        }
        public void InitResult(int ch)
        {
            PassFails[ch].TotalFail = "";
            PassFails[ch].FirstFail = "";
            PassFails[ch].FirstFailIndex = 0;
            for (int i = 0; i < Param.Count; i++)
            {
                PassFails[ch].Results[i].Val = 0;
                PassFails[ch].Results[i].msg = ""; PassFails[ch].Results[i].bPass = true;
            }
        }
        public void SetResult(int ch, int start, int end)
        {
            for (int i = start; i < end + 1; i++)
            {
                if (!Convert.ToBoolean(Param[i][USE])) continue;

                double lmin, lmax;
                lmin = Convert.ToDouble(Param[i][MIN_VAL]);
                lmax = Convert.ToDouble(Param[i][MAX_VAL]);

                if (PassFails[ch].Results[i].Val < lmin || PassFails[ch].Results[i].Val > lmax || double.IsNaN(PassFails[ch].Results[i].Val))
                {
                    PassFails[ch].Results[i].msg = Param[i][KEY] + "_" + Param[i][COMMENT];
                    PassFails[ch].Results[i].bPass = false;
                    PassFails[ch].TotalFail += string.Format("{0}'", i + 1);
                }
                else
                {
                    PassFails[ch].Results[i].msg = ""; PassFails[ch].Results[i].bPass = true;
                }
            }
            for (int i = start; i < end + 1; i++)
            {
                if (!PassFails[ch].Results[i].bPass)
                {
                    if (PassFails[ch].FirstFailIndex == 0)
                    {
                        PassFails[ch].FirstFailIndex = (i + 1);
                        PassFails[ch].FirstFail = PassFails[ch].Results[i].msg;
                    }

                    int failCnt = Convert.ToInt32(Param[i][FAILED]); failCnt++;
                    Param[i][FAILED] = failCnt;
                }
            }
        }
    }
    public class CurrentPath : BaseRecipe
    {
        public List<string> List = new List<string>();
        public string ConditionName;
        public string SpecName;
        public string PIDName;
        
        public CurrentPath()
        {
            FilePath = STATIC.RootDir + "CurrentPath.txt";

            List.Add(ConditionName = "Default.rcp");
            List.Add(SpecName = "Default.spc");
            List.Add(PIDName = "Default.txt");
            Read();
        }
        public override void Read(string Path = "")
        {
            base.Read();

            if (!File.Exists(FilePath))
            {
                STATIC.SetTextLine(FilePath, List);
            }
            else
            {
                List<string> ReadList = STATIC.GetTextAll(FilePath);
                if (List.Count != ReadList.Count)
                {
                    STATIC.SetTextLine(FilePath, List);
                }
                else
                {
                    List = ReadList;
                }
                int index = 0;
                ConditionName = List[index++];
                SpecName = List[index++];
                PIDName = List[index++];
            }
        }
        public override void Save(string Path = "")
        {
            List.Clear();
            List.Add(ConditionName);
            List.Add(SpecName);
            List.Add(PIDName);
            STATIC.SetTextLine(FilePath, List);
        }
    }
    public class Option : BaseRecipe
    {
        
        public bool SaveImage;
        public bool ShowResultToImage;
        public bool SaveRawData;
        public bool SaveLog;
        //public bool SoftLandingUse;
        //public bool BackStepUse;
        public bool isPosture;
        public bool MotionUse;
        public bool Pos14OpenLoopUse;
        public bool Pos40OpenLoopUse;
        public bool ReverseDrv;
        public bool isActMode;
        //public bool DllLogSave;
        public bool AreaDecenterUse;
        public bool BaseOnSpacer;
        public bool isOQC;
        public bool ChartVisible;
    //    public bool PassFailSaveMode;
    //    public bool UseAddr19;
        public bool Step10Use;
        public bool SafeSensor;
        public bool is1CH_MC;
        public bool isReadTargetVal;
        public bool FlagCheckUse;
        //public bool ActroDllUse;
        //public bool CDllUse;
        //public bool Code1500InspUse;
        public bool ActOpenAreaOp;
        public bool Step10SearchUse;

        public Option()
        {
            FilePath = STATIC.RootDir + "OptionState.txt";
            Param.Add(new object[] { "SaveImage", false });
            Param.Add(new object[] { "ShowResultToImage", false });
            Param.Add(new object[] { "SaveRawData", false });
            Param.Add(new object[] { "SaveLog", false });
            //Param.Add(new object[] { "SoftLandingUse", false });
            //Param.Add(new object[] { "BackStepUse", false });
            Param.Add(new object[] { "Posture M/C", false });
            Param.Add(new object[] { "Motion Use Flag", false });
            Param.Add(new object[] { "#1.4 OpenLoopUse", false });
            Param.Add(new object[] { "#4.0 OpenLoopUse", false });
            Param.Add(new object[] { "Reverse Driving", false });
            Param.Add(new object[] { "Act Mode", false });
            //Param.Add(new object[] { "Dll Log Save", false });
            Param.Add(new object[] { "Area Decenter Use", false });
            Param.Add(new object[] { "Base On Spacer", false });
            Param.Add(new object[] { "OQC Insp", false });
            Param.Add(new object[] { "Chart Enable", false });
            //Param.Add(new object[] { "P/F Save Mode", false });
            //Param.Add(new object[] { "Use Addr 0x19", false });
            Param.Add(new object[] { "10Step(9Step) Use", false });
            Param.Add(new object[] { "Safe Sensor Enable", false });
            Param.Add(new object[] { "1CH M/C", false });
            Param.Add(new object[] { "Read Target Value", false });
            Param.Add(new object[] { "Flag Check Use", false });
         //   Param.Add(new object[] { "Actro Dll Use", false });
          //  Param.Add(new object[] { "C Dll Use", false });
           // Param.Add(new object[] { "1500code Insp Use", false });
            Param.Add(new object[] { "Act F16 Option", false });
            Param.Add(new object[] { "10Step(9Step) Search", false });
            if (!File.Exists(FilePath)) Save();

            Read();
        }
        public override void Read(string filePath = "")
        {
            StreamReader sr = new StreamReader(FilePath);

            string[] readArry = sr.ReadToEnd().Split('\r');

            int index = 0;
            if (readArry.Length > index) SaveImage = SetParam(readArry[index], index++);
            if (readArry.Length > index) ShowResultToImage = SetParam(readArry[index], index++);
            if (readArry.Length > index) SaveRawData = SetParam(readArry[index], index++);
            if (readArry.Length > index) SaveLog = SetParam(readArry[index], index++);
            //if (readArry.Length > index) SoftLandingUse = SetParam(readArry[index], index++);
            //if (readArry.Length > index) BackStepUse = SetParam(readArry[index], index++);
            if (readArry.Length > index) isPosture = SetParam(readArry[index], index++);          
            if (readArry.Length > index) MotionUse = SetParam(readArry[index], index++);
            if (readArry.Length > index) Pos14OpenLoopUse = SetParam(readArry[index], index++);
            if (readArry.Length > index) Pos40OpenLoopUse = SetParam(readArry[index], index++);
            if (readArry.Length > index) ReverseDrv = SetParam(readArry[index], index++);
            if (readArry.Length > index) isActMode = SetParam(readArry[index], index++);
          //  if (readArry.Length > index) DllLogSave = SetParam(readArry[index], index++);
            if (readArry.Length > index) AreaDecenterUse = SetParam(readArry[index], index++);
            if (readArry.Length > index) BaseOnSpacer = SetParam(readArry[index], index++);
            if (readArry.Length > index) isOQC = SetParam(readArry[index], index++);
            if (readArry.Length > index) ChartVisible = SetParam(readArry[index], index++);
            //if (readArry.Length > index) PassFailSaveMode = SetParam(readArry[index], index++);
            //if (readArry.Length > index) UseAddr19 = SetParam(readArry[index], index++);
            if (readArry.Length > index) Step10Use = SetParam(readArry[index], index++);
            if (readArry.Length > index) SafeSensor = SetParam(readArry[index], index++);
            if (readArry.Length > index) is1CH_MC = SetParam(readArry[index], index++);
            if (readArry.Length > index) isReadTargetVal = SetParam(readArry[index], index++);
            if (readArry.Length > index) FlagCheckUse = SetParam(readArry[index], index++);
            //if (readArry.Length > index) ActroDllUse = SetParam(readArry[index], index++);
            //if (readArry.Length > index) CDllUse = SetParam(readArry[index], index++);
            //if (readArry.Length > index) Code1500InspUse = SetParam(readArry[index], index++);
            if (readArry.Length > index) ActOpenAreaOp = SetParam(readArry[index], index++);
            if (readArry.Length > index) Step10SearchUse = SetParam(readArry[index], index++);
            sr.Close();
        }
        public override void Save(string filePath = "")
        {
            if (filePath != "") FilePath = filePath;
            StreamWriter sw = new StreamWriter(FilePath);

            for (int i = 0; i < Param.Count; i++)
            {
                string data = string.Format("{0}\t{1}", Param[i][0], Param[i][1]);
                sw.WriteLine(data);
            }
            sw.Close();

            Read();
        }
        public bool SetParam(string Src, int index)
        {
            string[] arry = Src.Split('\t');
            for (int i = 0; i < arry.Length; i++) arry[i] = arry[i].Trim();

            if (arry[0] == Param[index][0].ToString())
            {
                bool ret = Convert.ToBoolean(arry[1]);
                Param[index][1] = ret;
                return ret;
            }
            return false;
        }
    }
    public class Model : BaseRecipe
    {
        public string Virtaul;
        public string SocketNo;
        public string TesterNo;
        public string ProductID;
     //   public string ModelName;
        public string LotID;
        public string OperatorName;

        public List<string> List = new List<string>();

        public List<string> ModelList = new List<string>();

        public Model()
        {
            FilePath = STATIC.RootDir + "Model.txt";
     
            //ModelList.Add("SO1C81");        
            //ModelList.Add("SO1G73");

            Read();
        }
        public override void Read(string filePath = "")
        {
            base.Read();
            if (!File.Exists(FilePath))
            {
                List.Add("0");
                List.Add("0");
                List.Add("0");
                List.Add("0");
                List.Add("SO1C81");
                List.Add("Test");
                List.Add("Operator");
                STATIC.SetTextLine(FilePath, List);
                SetParam();
            }
            else
            {
                List = STATIC.GetTextAll(FilePath);
                SetParam();
            }
        }
        public override void Save(string filePath = "")
        {
            List.Clear();
            List.Add(Virtaul);
            List.Add(SocketNo);
            List.Add(TesterNo);
            List.Add(ProductID);
          //  List.Add(ModelName);
            List.Add(LotID);
            List.Add(OperatorName);
            STATIC.SetTextLine(FilePath, List);
        }

        public override void SetParam()
        {
            base.SetParam();
            int index = 0;
            Virtaul = List[index++];
            SocketNo = List[index++];
            TesterNo = List[index++];
            ProductID = List[index++];
           // ModelName = List[index++];
            LotID = List[index++];
            OperatorName = List[index++];
        }
    }
    public class VisionSettingFile : BaseRecipe
    {
        public double TopInnerLED_L;
        public double TopOuterLED_L;
        public double BtmLED_L;
        public double TopInnerLED_R;
        public double TopOuterLED_R;
        public double BtmLED_R;
        public double Scale_L;
        public double Scale_R;
  

       

        public List<string> List = new List<string>();

       

        public VisionSettingFile()
        {
            FilePath = STATIC.RootDir + "VisonSettingFile.txt";

            List.Add("2.8"); //i
            List.Add("2.8"); //o
            List.Add("2.8"); //b
            List.Add("2.8"); //i
            List.Add("2.8"); //o
            List.Add("2.8"); //b
            List.Add("1.0");
            List.Add("1.0");

            Read();
        }
        public override void Read(string filePath = "")
        {
            base.Read();
            if (!File.Exists(FilePath))
                STATIC.SetTextLine(FilePath, List);
            else
            {
                List = STATIC.GetTextAll(FilePath);
                SetParam();
            }
        }
        public override void Save(string filePath = "")
        {
            List.Clear();
            List.Add(TopInnerLED_L.ToString());
            List.Add(TopOuterLED_L.ToString());
            List.Add(BtmLED_L.ToString());
            List.Add(TopInnerLED_R.ToString());
            List.Add(TopOuterLED_R.ToString());
            List.Add(BtmLED_R.ToString());
            List.Add(Scale_L.ToString());
            List.Add(Scale_R.ToString());
         
          
            STATIC.SetTextLine(FilePath, List);
        }

        public override void SetParam()
        {
            base.SetParam();
            int index = 0;
            TopInnerLED_L = Convert.ToDouble(List[index++]);
            TopOuterLED_L = Convert.ToDouble(List[index++]);
            BtmLED_L = Convert.ToDouble(List[index++]);
            TopInnerLED_R = Convert.ToDouble(List[index++]);
            TopOuterLED_R = Convert.ToDouble(List[index++]);
            BtmLED_R = Convert.ToDouble(List[index++]);
            Scale_L = Convert.ToDouble(List[index++]);
            Scale_R = Convert.ToDouble(List[index++]);


        }
    }
    public class DecenterScale : BaseRecipe
    {
    
        public double DecenterScale_L;
        public double DecenterScale_R;



        public List<string> List = new List<string>();



        public DecenterScale()
        {
            FilePath = STATIC.RootDir + "DecenterScale.txt";

         
            List.Add("1.0");
            List.Add("1.0");

            Read();
        }
        public override void Read(string filePath = "")
        {
            base.Read();
            if (!File.Exists(FilePath))
                STATIC.SetTextLine(FilePath, List);
            else
            {
                List = STATIC.GetTextAll(FilePath);
                SetParam();
            }
        }
        public override void Save(string filePath = "")
        {
            List.Clear();
          
            List.Add(DecenterScale_L.ToString());
            List.Add(DecenterScale_R.ToString());
            STATIC.SetTextLine(FilePath, List);
        }

        public override void SetParam()
        {
            base.SetParam();
            int index = 0;
         
            DecenterScale_L = Convert.ToDouble(List[index++]);
            DecenterScale_R = Convert.ToDouble(List[index++]);

        }
    }

    public class F16AreaScale : BaseRecipe
    {

        public double F16Scale;
        



        public List<string> List = new List<string>();



        public F16AreaScale()
        {
            FilePath = STATIC.RootDir + "F16AreaScale.txt";


            List.Add("1.0");
        
            Read();
        }
        public override void Read(string filePath = "")
        {
            base.Read();
            if (!File.Exists(FilePath))
                STATIC.SetTextLine(FilePath, List);
            else
            {
                List = STATIC.GetTextAll(FilePath);
                SetParam();
            }
        }
        public override void Save(string filePath = "")
        {
            List.Clear();

            List.Add(F16Scale.ToString());
         
            STATIC.SetTextLine(FilePath, List);
        }

        public override void SetParam()
        {
            base.SetParam();
            int index = 0;

            F16Scale = Convert.ToDouble(List[index++]);
         
        }
    }
    public class Password : BaseRecipe
    {

        public string useFlag;
        public string PassWord;



        public List<string> List = new List<string>();



        public Password()
        {
            FilePath = STATIC.RootDir + "Password.txt";


            List.Add("false");
            List.Add("1");

            Read();
        }
        public override void Read(string filePath = "")
        {
            base.Read();
            if (!File.Exists(FilePath))
                STATIC.SetTextLine(FilePath, List);
            else
            {
                List = STATIC.GetTextAll(FilePath);
                SetParam();
            }
        }
        public override void Save(string filePath = "")
        {
            List.Clear();

            List.Add(useFlag.ToString());
            List.Add(PassWord.ToString());
            STATIC.SetTextLine(FilePath, List);
        }

        public override void SetParam()
        {
            base.SetParam();
            int index = 0;

            useFlag = List[index++];
            PassWord = List[index++];

        }
    }

}