using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Web.Security;

namespace M1Wide
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool isNew;

            Mutex mutex = new Mutex(true, "OIS_Test", out isNew);
            if (isNew)
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                PKGRelease();
                Application.Run(new F_Main());
                mutex.ReleaseMutex();
                System.Diagnostics.Process.GetCurrentProcess().Kill();




            }
            else
            {
                MessageBox.Show("Still Running Process .....");
                Application.Exit();
            }
        }

        static void PKGRelease()
        {
            string DonotTouchDir = STATIC.RootDir;
            string RcpDir = STATIC.BaseDir + "\\Recipe\\";
            string SpecDir = STATIC.BaseDir + "\\Spec\\";
            string PIDDir = STATIC.BaseDir + "\\DriverIC\\";
            string PKGDir = STATIC.BaseDir + "\\PKG\\";
            string dstFile = string.Empty;
            if (!Directory.Exists(RcpDir))
                Directory.CreateDirectory(RcpDir);
            if (!Directory.Exists(SpecDir))
                Directory.CreateDirectory(SpecDir);
            if (!Directory.Exists(DonotTouchDir))
                Directory.CreateDirectory(DonotTouchDir);
            if (!Directory.Exists(PIDDir))
                Directory.CreateDirectory(PIDDir);
            if (!Directory.Exists(PKGDir))
                Directory.CreateDirectory(PKGDir);

            string[] RcpArray = Directory.GetFiles(PKGDir, "*.rcp");
            string[] SpecArray = Directory.GetFiles(PKGDir, "*.spc");
            string[] txtArray = Directory.GetFiles(PKGDir, "*.txt");

            string LastRcpName = string.Empty;
            string LastSpcName = string.Empty;
            string LastPIDName = string.Empty;

            for (int i = 0; i < RcpArray.Length; i++)
            {
                dstFile = RcpDir + RcpArray[i].Substring(PKGDir.Length);
                if (File.Exists(dstFile))
                    File.Delete(dstFile);
                File.Move(RcpArray[i], dstFile);
                LastRcpName = RcpArray[i].Substring(PKGDir.Length);

            }
            for (int i = 0; i < SpecArray.Length; i++)
            {
                dstFile = SpecDir + SpecArray[i].Substring(PKGDir.Length);
                if (File.Exists(dstFile))
                    File.Delete(dstFile);
                File.Move(SpecArray[i], dstFile);
                LastSpcName = SpecArray[i].Substring(PKGDir.Length);

            }
            for (int i = 0; i < txtArray.Length; i++)
            {
                if (!txtArray[i].Contains("OptionState"))
                {
                    dstFile = PIDDir + txtArray[i].Substring(PKGDir.Length);
                    if (File.Exists(dstFile))
                        File.Delete(dstFile);
                    File.Move(txtArray[i], dstFile);
                    LastPIDName = dstFile;
                }
                else
                {
                    dstFile = DonotTouchDir + txtArray[i].Substring(PKGDir.Length);
                    if (File.Exists(dstFile))
                        File.Delete(dstFile);
                    File.Move(txtArray[i], dstFile);
                }
            }

            string CurrentPath = STATIC.RootDir + "CurrentPath.txt";
            List<string> ReadList = STATIC.GetTextAll(CurrentPath);
            int index = 0;
            string Conditionname = ReadList[index++];
            string SpecName = ReadList[index++];
            string PIDName = ReadList[index++];

            if (LastRcpName != string.Empty)
                Conditionname = LastRcpName;
            if (LastSpcName != string.Empty)
                SpecName = LastSpcName;
            if (LastPIDName != string.Empty)
                PIDName = LastPIDName;
            ReadList.Clear();
            ReadList.Add(Conditionname);
            ReadList.Add(SpecName);
            ReadList.Add(PIDName);
            STATIC.SetTextLine(CurrentPath, ReadList);
            STATIC.Rcp.Current.Read();
            STATIC.Rcp.Condition.Read(RcpDir + STATIC.Rcp.Current.ConditionName);
            STATIC.Rcp.Spec.Read(SpecDir + STATIC.Rcp.Current.SpecName);
            STATIC.Rcp.Option.Read(DonotTouchDir + "OptionState.txt");


        }
    }
}
