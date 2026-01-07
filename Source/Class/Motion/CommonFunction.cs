using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M1Wide
{
    public enum SearchingDir
    {
        PtoN,
        NtoP,
    }
    public enum SettingType
    {
        Path,
        General,
        SoftWareLimit,
        InspectionPosSetting,
        HomeSetting,
    }
    public class StaticVariables
    {
        public static bool isSafetyOn = false;
        public static bool FromTester = false;
        public static bool isMotionBusyAxis0 = false;
        public static bool isMotionLimitAxis0 = false;
        public static bool isMotionBusyAxis1 = false;
        public static bool isMotionLimitAxis1 = false;
        public static bool isAutoRun = false;
    }
    public class PublicFunctions
    {
        public static void Wait(uint Millisec)
        {
            double rDblStart;
            rDblStart = DateTime.Now.Ticks;
            while (((DateTime.Now.Ticks - rDblStart) / 10000000f) <= (Millisec / 1000f))
            {
                Application.DoEvents();
            }
        }

        [DllImport("kernel32")]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport("kernel32")]
        public static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        public void SaveProfile(string sApplicationName, string sKeyName, string sKeyValue, string sFileName)
        {
            bool isbool = false;
            isbool = WritePrivateProfileString(sApplicationName, sKeyName, sKeyValue, sFileName);
        }
        public string LoadProfile(string sApplicationName, string sKeyName, string sFileName)
        {
            StringBuilder ret = new StringBuilder(102400);
            long NC;
            try
            {
                NC = GetPrivateProfileString(sApplicationName, sKeyName, "Pass", ret, 102400, sFileName);
                if (ret.ToString() == "Pass")
                {
                    return "0";
                }
                else
                {
                    return ret.ToString();
                }
            }
            catch
            {
                return "0";
            }
        }


        public string[,] ParameterString = new string[15, 5] { { "General", "Velocity", "14.4", "14.4" ,"deg/s" },
                                                              {"",         "Acceleration", "28.8", "28.8","deg/s\xB2"},
                                                              {"",         "Deceleration", "28.8", "28.8","deg/s\xB2" },
                                                              //{"",         "RepeatCountInPos", "1","0" ,"cnt" },
                                                              //{"",         "RepeatCountInFullTest", "1","0", "cnt" },
                                                              {"Home Setting",  "Set Offset P", "0.0", "0.0","deg"},
                                                              {"",              "Set Offset N", "0.0", "0.0","deg"},
                                                              {"",              "Clear Time", "1000.0", "1000.0","m/s"},
                                                              {"",              "1st Velocity", "7.2", "7.2","deg/s"},
                                                              {"",              "2nd Velocity", "7.2", "7.2","deg/s"},
                                                              {"",              "3rd Velocity", "1.44", "1.44","deg/s"},
                                                              {"",              "4th Velocity", "0.072", "0.072","deg/s"},
                                                              {"",              "1st Acceleration", "28.8", "28.8","deg/s\xB2"},
                                                              {"",              "2nd Acceleration", "57.6", "57.6","deg/s\xB2"},
                                                              {"Limit Setting", "SW Limit N", "0.0", "0.0","deg"},
                                                              {"",              "SW Limit P", "0.0", "0.0","deg"},
                                                              {"Position Setting",  "Test Position", "0.0", "0.0","deg"},

        };

        public static void TypingOnlyNumber(object sender, KeyPressEventArgs e, bool includePoint, bool includeMinus)
        {
            bool isValidInput = false;
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                if (includePoint == true) { if (e.KeyChar == '.') isValidInput = true; }
                if (includeMinus == true) { if (e.KeyChar == '-') isValidInput = true; }
                if (isValidInput == false) e.Handled = true;
            }

            if (includePoint == true)
            {
                if (e.KeyChar == '.' && (string.IsNullOrEmpty((sender as TextBox).Text.Trim()) || (sender as TextBox).Text.IndexOf('.') > -1)) e.Handled = true;
            }

        }


        public static int SC_CLOSE = 0xF060;

        private const int MF_ENABLED = 0x0;

        public static int MF_GRAYED = 0x1;

        private const int MF_DISABLED = 0x2;

        [DllImport("user32.dll")]

        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]

        public static extern int EnableMenuItem(IntPtr hMenu, int wIDEnableItem, int wEnable);

    }
}
