using Basler.Pylon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M1Wide
{
    public class CameraList
    {
        static List<GrabInfo> GrabberList;

        public static string[] GetCameraModelNames
        {
            get
            {
                string[] sList = new string[GrabberList.Count];
                for (int i = 0; i < GrabberList.Count; i++)
                {
                    sList[i] = $"{GrabberList[i].ModelName}_({GrabberList[i].Info.SerialNumber})";
                }
                return sList.ToArray();
            }
        }
        public static bool CreateList()
        {
            try
            {
                List<ICameraInfo> grabs = CameraFinder.Enumerate();
                if (GrabberList != null)
                {
                    while (GrabberList.Count > 0)
                    {
                        GrabberList[0].Dispose();
                        GrabberList.RemoveAt(0);
                    }
                    GrabberList.Clear();
                }
                else
                {
                    GrabberList = new List<GrabInfo>();
                }
                if (grabs.Count > 0)
                {
                    int iCnt = 0;
                    foreach (ICameraInfo cam in grabs)
                    {
                        GrabberList.Add(new GrabInfo()
                        {
                            ModelName = cam[CameraInfoKey.ModelName],
                            Camera = cam,
                            Info = new CameraInfo
                            {
                                CamIndex = iCnt,
                                IPAddr = cam[CameraInfoKey.DeviceIpAddress],
                                SerialNumber = cam[CameraInfoKey.SerialNumber],
                                ModelName = cam[CameraInfoKey.ModelName],

                            }
                        });
                        iCnt++;
                    }
                }
            }
            catch
            { }
            return true;
        }
        internal static ICameraInfo FindCamera(int camNum)
        {
            if (GrabberList.Count > 0)
                return GrabberList[camNum].Camera;
            else
                return null;
        }
        internal static int FindCameraIndex(string serial)
        {
            if (GrabberList.Count > 0)
            {
                for (int i = 0; i < GrabberList.Count; i++)
                {
                    if (GrabberList[i].Info.SerialNumber == serial)
                        return i;
                }
            }
            return -1;
        }
    }

    public class GrabInfo : IDisposable
    {
        public string ModelName;
        public ICameraInfo Camera;
        public CameraInfo Info;

        public GrabInfo Copy()
        {
            return new GrabInfo()
            {
                ModelName = ModelName,
                Camera = Camera,
                Info = Info.Copy(),
            };
        }

        public void Dispose() { GC.SuppressFinalize(this); }

    }
    public class CameraInfo : IDisposable
    {
        public int CamIndex;
        public string IPAddr;
        public string SerialNumber;
        public string ModelName;
        public string TrigMode;
        public string TrigSource;
        public float ExpTime;
        public float GainOffset;

        public int Width;
        public int Height;
        public int OffsetX;
        public int OffsetY;

        public CameraInfo()
        {

        }
        public CameraInfo Copy()
        {
            return new CameraInfo()
            {
                CamIndex = CamIndex,
                IPAddr = IPAddr,
                SerialNumber = SerialNumber,
                ModelName = ModelName,
                TrigMode = TrigMode,
                TrigSource = TrigSource,
                ExpTime = ExpTime,
                GainOffset = GainOffset,
                Width = Width,
                Height = Height,
                OffsetX = OffsetX,
                OffsetY = OffsetY

            };
        }
        public void Dispose() { GC.SuppressFinalize(this); }

    }
}
