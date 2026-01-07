using System;
using Basler.Pylon;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Collections.Generic;


namespace M1Wide
{
   
    public class Grabber
    {
        public delegate void NewFrame(int camIndex, Mat newFrame);
        public event NewFrame OnNewFrame;
        [NonSerialized] Basler.Pylon.Camera m_Grabber;
        [NonSerialized] Bitmap m_LastFrame = null;
        [NonSerialized] bool m_bLiveMode = false;
        [NonSerialized] bool m_bGrabFinish = true;
        [NonSerialized] PixelDataConverter m_Converter = new PixelDataConverter();
        public CameraInfo CamInfo;

        public void OpenCamera(int CamNum, bool isNew)
        {
            if (isCreated) DestoryCamera();
            if (CamNum == -1) return;
            ICameraInfo camInf = CameraList.FindCamera(CamNum);
            if (camInf != null)
            {
                m_Grabber = new Basler.Pylon.Camera(camInf);
                if (!isOpen)
                {
                    m_Grabber.Open();
                    m_Grabber.Parameters[PLCamera.ReverseX].SetValue(true);
                    m_Grabber.Parameters[PLCamera.ReverseY].SetValue(true);
                    m_Grabber.Parameters[PLCamera.OffsetX].SetValue(0);
                    m_Grabber.Parameters[PLCamera.OffsetY].SetValue(0);
                    m_Grabber.Parameters[PLCamera.Width].SetValue(STATIC.DefaultWidth);
                    m_Grabber.Parameters[PLCamera.Height].SetValue(STATIC.DefaultHeight);
                    m_Grabber.Parameters[PLCamera.GammaEnable].SetValue(true);
                    m_Grabber.Parameters[PLCamera.Gamma].SetValue(0.8);
                    m_Grabber.Parameters[PLCamera.GevSCPSPacketSize].SetValue(9000);
                    Thread.Sleep(100);
                    m_Grabber.CameraOpened += Configuration.SoftwareTrigger;
                    if (m_Grabber.CanWaitForFrameTriggerReady)
                    {
                        m_Grabber.StreamGrabber.ImageGrabbed += StreamGrabber_ImageGrabbed;
                        m_Grabber.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                    }
                }
                if (isNew)
                {
                    CamInfo = new CameraInfo();
                    CamInfo.CamIndex = CamNum;
                    CamInfo.IPAddr = camInf[CameraInfoKey.DeviceIpAddress];
                    CamInfo.SerialNumber = camInf[CameraInfoKey.SerialNumber];
                    CamInfo.ModelName = camInf[CameraInfoKey.ModelName];
                    CamInfo.TrigMode = TriggerMode;
                    CamInfo.TrigSource = TriggerSource;
                    CamInfo.ExpTime = ExposureTime;
                    CamInfo.GainOffset = GainOffset;
                    CamInfo.Width = Width;
                    CamInfo.Height = Height;
                    CamInfo.OffsetX = OffsetX;
                    CamInfo.OffsetY = OffsetY;

                }
                this.ShutterMode = "Rolling";

            }


        }

        private void StreamGrabber_ImageGrabbed(object sender, ImageGrabbedEventArgs e)
        {
            try
            {
                IGrabResult grabResult = e.GrabResult;
                if (grabResult.GrabSucceeded)
                {
                    using (m_LastFrame = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb))
                    {
                        BitmapData bmpData = m_LastFrame.LockBits(new Rectangle(0, 0, m_LastFrame.Width, m_LastFrame.Height), ImageLockMode.ReadWrite, m_LastFrame.PixelFormat);
                        m_Converter.OutputPixelFormat = PixelType.BGRA8packed;
                        IntPtr ptrBmp = bmpData.Scan0;
                        m_Converter.Convert(ptrBmp, bmpData.Stride * m_LastFrame.Height, grabResult);
                        m_LastFrame.UnlockBits(bmpData);

                        using (Mat newFrame = m_LastFrame.ToMat())
                        {
                            Mat sendMat = new Mat();
                            Cv2.CvtColor(newFrame, sendMat, ColorConversionCodes.BGR2GRAY);
                            OnNewFrame?.Invoke(CamInfo.CamIndex, sendMat);
                        }

                    }
                }
            }
            catch
            { }
            m_bGrabFinish = true;
        }

        public void OpenCamera(string Serial, bool isNew)
        {
            OpenCamera(CameraList.FindCameraIndex(Serial), isNew);
        }
        public void GrabStart()
        {
            Configuration.AcquireContinuous(m_Grabber, null);
            m_Grabber.StreamGrabber.Start();
        }

        public void Acquire()
        {
            try
            {
                SoftwareTrigger();
                while (!m_bGrabFinish)
                    Thread.Sleep(1);
            }
            catch { }
            finally { }
            
        }
        public void Live(bool status)
        {
            m_bLiveMode = status;
            if (status)
            {
                new Thread(new ThreadStart(() =>
                {
                    while (m_bLiveMode)
                    {
                        if (m_bGrabFinish)
                        {
                            m_bGrabFinish = false;

                            SoftwareTrigger();
                        }

                        Thread.Sleep(10);
                    }
                })).Start();
            }
        }
        void SoftwareTrigger()
        {
            m_bGrabFinish = false;
            if (m_Grabber.WaitForFrameTriggerReady(1000, TimeoutHandling.ThrowException))
            {
                m_Grabber.ExecuteSoftwareTrigger();
            }
        }
        public void DestoryCamera()
        {
            if (isGrabbing) GrabStop();
            if (isOpen) CloseCamera();
            if (isCreated)
            {
                m_Grabber.Dispose();
                m_Grabber = null;
            }
        }
        public void CloseCamera()
        {
            if (isOpen) m_Grabber.Close();
            if (m_LastFrame != null) m_LastFrame.Dispose();
            m_LastFrame = null;

        }
        public void GrabStop()
        {
            m_Grabber.StreamGrabber.Stop();
        }
        public bool isCreated
        {
            get { return m_Grabber != null; }
        }
        public bool isOpen
        {
            get { return isCreated && m_Grabber.IsOpen; }
        }
        public bool isGrabbing
        {
            get { return isOpen && m_Grabber.StreamGrabber.IsGrabbing; }
        }
        public int Width
        {
            get
            {
                if (m_Grabber == null) return 0;
                return (int)m_Grabber.Parameters[PLCamera.Width].GetValue();
            }
            set
            {
                //if (m_Grabber != null)
                //    m_Grabber.Parameters[PLCamera.Width].TrySetValue(1000);
                //if (m_Grabber != null)
                //{

                //    if (m_Grabber.Parameters[PLCamera.Width].IsWritable && m_Grabber.Parameters[PLCamera.Width].IsReadable)
                //        m_Grabber.Parameters[PLCamera.Width].TrySetValue(value);

                //    Thread.Sleep(100);
                //}



            }

        }
        public int Height
        {
            get
            {
                if (m_Grabber == null) return 0;
                return (int)m_Grabber.Parameters[PLCamera.Height].GetValue();
            }
            set
            {
                //if (m_Grabber != null)
                //    m_Grabber.Parameters[PLCamera.Height].TrySetValue(1000);
                //if (m_Grabber != null)
                //{
                //    m_Grabber.Parameters[PLCamera.Height].TrySetValue(value);
                //    Thread.Sleep(100);
                //}
            }

        }
        public int OffsetX
        {
            get
            {
                if (m_Grabber == null) return 0;
                return (int)m_Grabber.Parameters[PLCamera.OffsetX].GetValue();
            }
            set { if (m_Grabber != null) m_Grabber.Parameters[PLCamera.OffsetX].TrySetValue(value); }
        }
        public int OffsetY
        {
            get
            {
                if (m_Grabber == null) return 0;
                return (int)m_Grabber.Parameters[PLCamera.OffsetY].GetValue();
            }
            set { if (m_Grabber != null) m_Grabber.Parameters[PLCamera.OffsetY].TrySetValue(value); }
        }
        public float ExposureTime
        {
            get
            {
                if (m_Grabber == null) return 0;
                IParameterCollection parameters = m_Grabber.Parameters;
                if (parameters.Contains(PLCamera.ExposureTimeAbs))
                {
                    if (m_Grabber.Parameters[PLCamera.ExposureTimeAbs].IsWritable && m_Grabber.Parameters[PLCamera.ExposureTimeAbs].IsReadable)
                        return (float)m_Grabber.Parameters[PLCamera.ExposureTimeAbs].GetValue();
                    else
                        return 38;
                }
                else
                {
                    if (m_Grabber.Parameters[PLCamera.ExposureTime].IsWritable && m_Grabber.Parameters[PLCamera.ExposureTime].IsReadable)
                        return (float)m_Grabber.Parameters[PLCamera.ExposureTime].GetValue();
                    else
                        return 38;
                }

            }
            set
            {
                if (m_Grabber != null)
                {
                    IParameterCollection parameters = m_Grabber.Parameters;
                    if (parameters.Contains(PLCamera.ExposureTimeAbs))
                    {
                        if (m_Grabber.Parameters[PLCamera.ExposureTimeAbs].IsWritable && m_Grabber.Parameters[PLCamera.ExposureTimeAbs].IsReadable)
                            m_Grabber.Parameters[PLCamera.ExposureTimeAbs].TrySetValue(value < 1000 ? 1000 : value);
                    }
                    else
                    {
                        if (m_Grabber.Parameters[PLCamera.ExposureTime].IsWritable && m_Grabber.Parameters[PLCamera.ExposureTime].IsReadable)
                            m_Grabber.Parameters[PLCamera.ExposureTime].TrySetValue(value < 1000 ? 1000 : value);
                    }
                }
            }
        }
        public float GainOffset
        {
            get
            {
                if (m_Grabber == null) return 0;
                IParameterCollection parameters = m_Grabber.Parameters;
                if (parameters.Contains(PLCamera.Gain))
                {
                    if (m_Grabber.Parameters[PLCamera.Gain].IsWritable && m_Grabber.Parameters[PLCamera.Gain].IsReadable)
                    {
                        return (float)m_Grabber.Parameters[PLCamera.Gain].GetValue();
                    }
                    else
                        return 0;
                }
                else
                {
                    if (m_Grabber.Parameters[PLCamera.GainRaw].IsWritable && m_Grabber.Parameters[PLCamera.GainRaw].IsReadable)
                    {
                        return (float)m_Grabber.Parameters[PLCamera.GainRaw].GetValue();
                    }
                    else
                    {
                        return 0;
                    }
                }

            }
            set
            {
                if (m_Grabber != null)
                {
                    m_Grabber.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Off);

                    IParameterCollection parameters = m_Grabber.Parameters;
                    if (parameters.Contains(PLCamera.Gain))
                    {
                        if (m_Grabber.Parameters[PLCamera.Gain].IsWritable && m_Grabber.Parameters[PLCamera.Gain].IsReadable)
                        {
                            m_Grabber.Parameters[PLCamera.Gain].TrySetValue(value);
                        }
                    }
                    else
                    {
                        if (m_Grabber.Parameters[PLCamera.GainRaw].IsWritable && m_Grabber.Parameters[PLCamera.GainRaw].IsReadable)
                        {
                            m_Grabber.Parameters[PLCamera.GainRaw].TrySetValue((int)value);
                        }
                    }
                }
            }
        }



        public string TriggerMode
        {
            get
            {
                if (m_Grabber == null) return "OFF";
                if (m_Grabber.Parameters[PLCamera.TriggerMode].IsWritable && m_Grabber.Parameters[PLCamera.TriggerMode].IsReadable)
                {
                    string tm = m_Grabber.Parameters[PLCamera.TriggerMode].GetValue();
                    if (tm == "On") return "ON";
                    else return "OFF";
                }
                return "OFF";
            }
            set
            {
                if (m_Grabber != null)
                {
                    if (m_Grabber.Parameters[PLCamera.TriggerMode].IsWritable && m_Grabber.Parameters[PLCamera.TriggerMode].IsReadable)
                    {
                        if (value == "ON")
                        {
                            //m_Grabber.Parameters[PLCamera.TriggerSelector].SetValue("AcquisitionStart");
                            m_Grabber.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                        }
                        else
                        {
                            m_Grabber.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                        }
                    }
                }
            }
        }
        public string TriggerSource
        {
            get
            {
                if (m_Grabber == null) return "SOFTWARE";
                if (m_Grabber.Parameters[PLCamera.TriggerSource].IsWritable && m_Grabber.Parameters[PLCamera.TriggerSource].IsReadable)
                {
                    string ts = m_Grabber.Parameters[PLCamera.TriggerSource].GetValue();
                    if (ts == "Line1") return "SOFTWARE";
                    else return "SOFTWARE";
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (m_Grabber != null)
                {
                    if (m_Grabber.Parameters[PLCamera.TriggerSource].IsWritable && m_Grabber.Parameters[PLCamera.TriggerSource].IsReadable)
                    {
                        if (value == "LINE1") m_Grabber.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
                        else if (value == "SOFTWARE") m_Grabber.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
                    }
                }
            }
        }
        public string ShutterMode
        {

            set
            {
                if (m_Grabber != null)
                {
                    if (m_Grabber.Parameters[PLCamera.ShutterMode].IsWritable && m_Grabber.Parameters[PLCamera.ShutterMode].IsReadable)
                    {
                        m_Grabber.Parameters[PLCamera.ShutterMode].TrySetValue(PLCamera.ShutterMode.Rolling);

                    }
                }
            }
        }

        public string GetPixelFormat
        {
            get { return m_Grabber.Parameters[PLCamera.PixelFormat].GetValue(); }
        }
    }

}
