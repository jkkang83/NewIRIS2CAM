using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M1Wide
{
    public class MotionSettingData
    {
        //General

        public static string LastDatPath;
        public static string SystemPath = STATIC.BaseDir + "\\MotionData\\SystemPath.ini";

        public static double VelocityAxis0;
        public static double AccAxis0;
        public static double DecAxis0;

        public static double VelocityAxis1;
        public static double AccAxis1;
        public static double DecAxis1;
        public static int RepeatCountInPos;
        public static int RepeatCountInFullTest;

        //HomeSetting

        public static double HomeSetOffsetPAxis0;
        public static double HomeSetOffsetNAxis0;
        public static double HomeClrTimeAxis0;
        public static double Home1stVelAxis0;
        public static double Home2ndVelAxis0;
        public static double Home3rdVelAxis0;
        public static double Home4thVelAxis0;
        public static double Home1stAccAxis0;
        public static double Home2ndAccAxis0;


        public static double HomeSetOffsetPAxis1;
        public static double HomeSetOffsetNAxis1;
        public static double HomeClrTimeAxis1;
        public static double Home1stVelAxis1;
        public static double Home2ndVelAxis1;
        public static double Home3rdVelAxis1;
        public static double Home4thVelAxis1;
        public static double Home1stAccAxis1;
        public static double Home2ndAccAxis1;

        //SwLimit
        public static bool SWLimitUseAxis0;
        public static double SWLimitNAxis0;
        public static double SWLimitPAxis0;

        public static bool SWLimitUseAxis1;
        public static double SWLimitNAxis1;
        public static double SWLimitPAxis1;

        //Position Setting
        public static double ManualPosAxis0;


        public static double ManualPosAxis1;

        //InspPosCount
        public static int InspPosCount;



        public static void SaveSystemPath()
        {
            PublicFunctions pubFun = new PublicFunctions();
            if (LastDatPath != null)
            {
                pubFun.SaveProfile(SettingType.Path.ToString(), "LastDatPath", LastDatPath.ToString(), SystemPath);
            }
            else
            {
                pubFun.SaveProfile(SettingType.Path.ToString(), "LastDatPath", "None", SystemPath);
            }

        }
        public static void SaveData(string datPath)
        {
            PublicFunctions pubFun = new PublicFunctions();

            pubFun.SaveProfile(SettingType.General.ToString(), "VelocityAxis0", VelocityAxis0.ToString(), datPath);
            pubFun.SaveProfile(SettingType.General.ToString(), "AccAxis0", AccAxis0.ToString(), datPath);
            pubFun.SaveProfile(SettingType.General.ToString(), "DecAxis0", DecAxis0.ToString(), datPath);

            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "HomeSetOffsetPAxis0", HomeSetOffsetPAxis0.ToString(), datPath);
            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "HomeSetOffsetNAxis0", HomeSetOffsetNAxis0.ToString(), datPath);
            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "HomeClrTimeAxis0", HomeClrTimeAxis0.ToString(), datPath);
            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "Home1stVelAxis0", Home1stVelAxis0.ToString(), datPath);
            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "Home2ndVelAxis0", Home2ndVelAxis0.ToString(), datPath);
            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "Home3rdVelAxis0", Home3rdVelAxis0.ToString(), datPath);
            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "Home4thVelAxis0", Home4thVelAxis0.ToString(), datPath);
            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "Home1stAccAxis0", Home1stAccAxis0.ToString(), datPath);
            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "Home2ndAccAxis0", Home2ndAccAxis0.ToString(), datPath);

            pubFun.SaveProfile(SettingType.SoftWareLimit.ToString(), "SWLimitUseAxis0", SWLimitUseAxis0.ToString(), datPath);
            pubFun.SaveProfile(SettingType.SoftWareLimit.ToString(), "SWLimitNAxis0", SWLimitNAxis0.ToString(), datPath);
            pubFun.SaveProfile(SettingType.SoftWareLimit.ToString(), "SWLimitPAxis0", SWLimitPAxis0.ToString(), datPath);

            pubFun.SaveProfile(SettingType.InspectionPosSetting.ToString(), "ManualPosAxis0", ManualPosAxis0.ToString(), datPath);





            pubFun.SaveProfile(SettingType.General.ToString(), "VelocityAxis1", VelocityAxis1.ToString(), datPath);
            pubFun.SaveProfile(SettingType.General.ToString(), "AccAxis1", AccAxis1.ToString(), datPath);
            pubFun.SaveProfile(SettingType.General.ToString(), "DecAxis1", DecAxis1.ToString(), datPath);


            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "HomeSetOffsetPAxis1", HomeSetOffsetPAxis1.ToString(), datPath);
            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "HomeSetOffsetNAxis1", HomeSetOffsetNAxis1.ToString(), datPath);
            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "HomeClrTimeAxis1", HomeClrTimeAxis1.ToString(), datPath);
            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "Home1stVelAxis1", Home1stVelAxis1.ToString(), datPath);
            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "Home2ndVelAxis1", Home2ndVelAxis1.ToString(), datPath);
            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "Home3rdVelAxis1", Home3rdVelAxis1.ToString(), datPath);
            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "Home4thVelAxis1", Home4thVelAxis1.ToString(), datPath);
            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "Home1stAccAxis1", Home1stAccAxis1.ToString(), datPath);
            pubFun.SaveProfile(SettingType.HomeSetting.ToString(), "Home2ndAccAxis1", Home2ndAccAxis1.ToString(), datPath);


            pubFun.SaveProfile(SettingType.SoftWareLimit.ToString(), "SWLimitUseAxis1", SWLimitUseAxis1.ToString(), datPath);
            pubFun.SaveProfile(SettingType.SoftWareLimit.ToString(), "SWLimitNAxis1", SWLimitNAxis1.ToString(), datPath);
            pubFun.SaveProfile(SettingType.SoftWareLimit.ToString(), "SWLimitPAxis1", SWLimitPAxis1.ToString(), datPath);

            pubFun.SaveProfile(SettingType.InspectionPosSetting.ToString(), "ManualPosAxis1", ManualPosAxis1.ToString(), datPath);

            pubFun.SaveProfile(SettingType.InspectionPosSetting.ToString(), "InspPosCount", InspPosCount.ToString(), datPath);

            pubFun.SaveProfile(SettingType.General.ToString(), "RepeatCountInPos", RepeatCountInPos.ToString(), datPath);
            pubFun.SaveProfile(SettingType.General.ToString(), "RepeatCountInFullTest", RepeatCountInFullTest.ToString(), datPath);


        }
        public static void LoadSystemPath()
        {
            PublicFunctions pubFun = new PublicFunctions();
            LastDatPath = pubFun.LoadProfile(SettingType.Path.ToString(), "LastDatPath", SystemPath);
        }
        public static void LoadData(string datPath)
        {
            PublicFunctions pubFun = new PublicFunctions();
            VelocityAxis0 = Convert.ToDouble(pubFun.LoadProfile(SettingType.General.ToString(), "VelocityAxis0", datPath));
            AccAxis0 = Convert.ToDouble(pubFun.LoadProfile(SettingType.General.ToString(), "AccAxis0", datPath));
            DecAxis0 = Convert.ToDouble(pubFun.LoadProfile(SettingType.General.ToString(), "DecAxis0", datPath));


            HomeSetOffsetPAxis0 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "HomeSetOffsetPAxis0", datPath));
            HomeSetOffsetNAxis0 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "HomeSetOffsetNAxis0", datPath));
            HomeClrTimeAxis0 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "HomeClrTimeAxis0", datPath));
            Home1stVelAxis0 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "Home1stVelAxis0", datPath));
            Home2ndVelAxis0 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "Home2ndVelAxis0", datPath));
            Home3rdVelAxis0 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "Home3rdVelAxis0", datPath));
            Home4thVelAxis0 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "Home4thVelAxis0", datPath));
            Home1stAccAxis0 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "Home1stAccAxis0", datPath));
            Home2ndAccAxis0 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "Home2ndAccAxis0", datPath));

            SWLimitUseAxis0 = Convert.ToBoolean(pubFun.LoadProfile(SettingType.SoftWareLimit.ToString(), "SWLimitUseAxis0", datPath));


            SWLimitNAxis0 = Convert.ToDouble(pubFun.LoadProfile(SettingType.SoftWareLimit.ToString(), "SWLimitNAxis0", datPath));
            SWLimitPAxis0 = Convert.ToDouble(pubFun.LoadProfile(SettingType.SoftWareLimit.ToString(), "SWLimitPAxis0", datPath));

            ManualPosAxis0 = Convert.ToDouble(pubFun.LoadProfile(SettingType.InspectionPosSetting.ToString(), "ManualPosAxis0", datPath));



            VelocityAxis1 = Convert.ToDouble(pubFun.LoadProfile(SettingType.General.ToString(), "VelocityAxis1", datPath));
            AccAxis1 = Convert.ToDouble(pubFun.LoadProfile(SettingType.General.ToString(), "AccAxis1", datPath));
            DecAxis1 = Convert.ToDouble(pubFun.LoadProfile(SettingType.General.ToString(), "DecAxis1", datPath));


            HomeSetOffsetPAxis1 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "HomeSetOffsetPAxis1", datPath));
            HomeSetOffsetNAxis1 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "HomeSetOffsetNAxis1", datPath));
            HomeClrTimeAxis1 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "HomeClrTimeAxis1", datPath));
            Home1stVelAxis1 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "Home1stVelAxis1", datPath));
            Home2ndVelAxis1 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "Home2ndVelAxis1", datPath));
            Home3rdVelAxis1 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "Home3rdVelAxis1", datPath));
            Home4thVelAxis1 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "Home4thVelAxis1", datPath));
            Home1stAccAxis1 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "Home1stAccAxis1", datPath));
            Home2ndAccAxis1 = Convert.ToDouble(pubFun.LoadProfile(SettingType.HomeSetting.ToString(), "Home2ndAccAxis1", datPath));

            SWLimitUseAxis1 = Convert.ToBoolean(pubFun.LoadProfile(SettingType.SoftWareLimit.ToString(), "SWLimitUseAxis1", datPath));
            SWLimitNAxis1 = Convert.ToDouble(pubFun.LoadProfile(SettingType.SoftWareLimit.ToString(), "SWLimitNAxis1", datPath));
            SWLimitPAxis1 = Convert.ToDouble(pubFun.LoadProfile(SettingType.SoftWareLimit.ToString(), "SWLimitPAxis1", datPath));

            ManualPosAxis1 = Convert.ToDouble(pubFun.LoadProfile(SettingType.InspectionPosSetting.ToString(), "ManualPosAxis1", datPath));

            InspPosCount = Convert.ToInt16(pubFun.LoadProfile(SettingType.InspectionPosSetting.ToString(), "InspPosCount", datPath));
            RepeatCountInPos = Convert.ToUInt16(pubFun.LoadProfile(SettingType.General.ToString(), "RepeatCountInPos", datPath));
            RepeatCountInFullTest = Convert.ToUInt16(pubFun.LoadProfile(SettingType.General.ToString(), "RepeatCountInFullTest", datPath));


        }
        public static void DefaultSetting()
        {
            VelocityAxis0 = 14.4;
            AccAxis0 = 28.8;
            DecAxis0 = 28.8;

            VelocityAxis1 = 14.4;
            AccAxis1 = 28.8;
            DecAxis1 = 28.8;

            Home1stVelAxis0 = 7.2;
            Home2ndVelAxis0 = 7.2;
            Home3rdVelAxis0 = 1.44;
            Home4thVelAxis0 = 0.072;

            Home1stVelAxis1 = 7.2;
            Home2ndVelAxis1 = 7.2;
            Home3rdVelAxis1 = 1.44;
            Home4thVelAxis1 = 0.072;

            Home1stAccAxis0 = 28.8;
            Home2ndAccAxis0 = 57.6;

            Home1stAccAxis1 = 28.8;
            Home2ndAccAxis1 = 57.6;

            RepeatCountInPos = 1;
            RepeatCountInFullTest = 1;

        }


    }
}
