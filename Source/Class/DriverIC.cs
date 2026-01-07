using Dln;
using Dln.Gpio;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace M1Wide
{
	public class DLN
	{
		public Process Process { get { return STATIC.Process; } }
		public Option option { get { return STATIC.Rcp.Option; } }
		//public const int RED_LAMP1 = 1;
		//public const int GREEN_LAMP1 = 2;
		//public const int YELLOW_LAMP1 = 3;
		public const int ID_1 = 6;
		public const int ID_2 = 7;
		public const int START = 8;
		public const int EMG = 8;
		public const int POWERON = 9;
		public const int STOP = 9;
		public const int SOCKET_OUT = 20;
		public const int SOCKET_IN = 21;
		public const int SOCKET_OUT_M = 16;
		public const int SOCKET_IN_M = 17;
		public const int COVER_DOWN = 18;
		public const int COVER_UP = 19;
		public const int RESET1 = 20;
		public const int RESET2 = 21;
		public const int RED_LAMP2 = 28;
		public const int YELLOW_LAMP2 = 29;
		public const int GREEN_LAMP2 = 30;
		public const int BUZZER = 31;
		public const int LIGHT_CURETAIN = 5;

		public bool IsVirtual = false;
		public uint m_PortCount = 0;
		public List<Device> DLNdevice = new List<Device>();
		public Dln.I2cMaster.Port[] DLNi2c;
		public Module[] DLNgpio;

		public bool IsEMG = false;
		
		public bool IsLoad = false;
		public bool IsCoverUp = false;

		private bool IsSwitch = false;
		public event EventHandler SwitchOn = null;
		public event EventHandler StopOn = null;
		public event EventHandler ResetOn = null;


		public event EventHandler EMGOn = null;
		private bool IsStop = false;
		private bool IsReset = false;

		private bool isSafeOn = false;
		System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer tm_stop = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer tm_EMG = new System.Windows.Forms.Timer();
        Stopwatch st = new Stopwatch();
        Stopwatch st_stop = new Stopwatch();
		Stopwatch st_EMG = new Stopwatch();
        public bool IsSafeOn
		{
			get { return isSafeOn; }
			set { if (value != isSafeOn) { isSafeOn = value; } }
		}
		object i2cLock = new object();	

        public DLN()
		{
			if (!Init()) return;

			if (option.isPosture && !option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
			{
				if (DLNgpio[1].Pins[COVER_DOWN].OutputValue == 1)
					IsCoverUp = true;
				if (DLNgpio[1].Pins[SOCKET_IN_M].OutputValue == 1)
					IsLoad = true;
			}
			else
			{
				if (DLNgpio[0].Pins[SOCKET_IN].OutputValue == 1)
					IsLoad = true;
			}
            tm.Tick += Tm_Tick;
			tm.Interval = 1500;
            tm_stop.Tick += Tm_stop_Tick;
			tm_stop.Interval = 1500;
            tm_EMG.Tick += Tm_EMG_Tick;
			tm_EMG.Interval = 1500;
		}

        private void Tm_EMG_Tick(object sender, EventArgs e)
        {
			st_EMG.Stop();
			st_EMG.Reset();
			tm_EMG.Enabled = false;

        }

        private void Tm_stop_Tick(object sender, EventArgs e)
        {
			st_stop.Stop();
			st_stop.Reset();
			tm_stop.Enabled = false;
         
        }

        private void Tm_Tick(object sender, EventArgs e)
        {
			st.Stop();
			st.Reset();
			tm.Enabled = false;
        }

        public void PinSet(int ch, int id, int dir)
		{
			lock(i2cLock)
			{
                DLNdevice[ch].Gpio.Pins[id].Enabled = true;
                DLNdevice[ch].Gpio.Pins[id].PulldownEnabled = true;
                DLNdevice[ch].Gpio.Pins[id].Direction = dir;
            }
			
		}
		public void GpioOnoff(int ch, int id, bool isOn)
		{
			lock(i2cLock)
			{
				try
				{
					if (DLNgpio == null) return;
					if (DLNgpio[ch] == null) return;
					if (isOn) DLNgpio[ch].Pins[id].OutputValue = 1;
					else DLNgpio[ch].Pins[id].OutputValue = 0;
				}
				catch
				{ }
            }
           
		
        }
        public bool GpioRead(int ch, int id)
        {
			lock(i2cLock)
            {
                try
                {
                    if (DLNgpio == null) return false;
                    if (DLNgpio[ch] == null) return false;
                    if (DLNgpio[ch].Pins[id].OutputValue == 1) return true;
                    else return false;
                }
                catch
                {
                    return false;
                }

            }
           
       
        }
        public bool Init()
        {
            try
            {
                Library.Connect("localhost", Connection.DefaultPort);

                m_PortCount = Device.Count();

                if (m_PortCount == 0)
                {
                    MessageBox.Show("--- No DLN-series adapters ---.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            for (int i = 0; i < m_PortCount; i++)
            {
                try
                {
                    DLNdevice.Add(Device.Open(i));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Port " + i + " : " + ex.Message + "\n Re-Connect USB Cable!");    // disappeared
                    return false;
                }
            }

            DLNi2c = new Dln.I2cMaster.Port[m_PortCount];
            DLNgpio = new Module[m_PortCount];

            for (int i = 0; i < m_PortCount; i++)
            {
                try
                {
                    if (DLNdevice[i].I2cMaster.Ports[0].Restrictions.MaxReplyCount != Restriction.NotSupported)
                        DLNdevice[i].I2cMaster.Ports[0].MaxReplyCount = 10;

                    if (DLNdevice[i].I2cMaster.Ports[0].Restrictions.Frequency == Restriction.MustBeDisabled)
                        DLNdevice[i].I2cMaster.Ports[0].Enabled = false;

                    DLNdevice[i].I2cMaster.Ports[0].Frequency = 900000;
                    DLNdevice[i].I2cMaster.Ports[0].Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Port " + i + " : " + ex.Message);
                    return false;
                }
            }

            //Define Pin
            for (int i = 0; i < m_PortCount; i++)
            {
                PinSet(i, ID_1, 0);
                PinSet(i, ID_2, 0);
            }

            Thread.Sleep(100);
            //Ports Set
            for (int i = 0; i < m_PortCount; i++)
            {
                int portID = 0;
                int[] res = new int[2];
                res[0] = DLNdevice[i].Gpio.Pins[ID_1].Value;
                if (res[0] == 1)
                    portID++;
                res[1] = DLNdevice[i].Gpio.Pins[ID_2].Value;
                if (res[1] == 1)
                    portID += 2;

                int portCount = DLNdevice[i].I2cMaster.Ports.Count;
                if (portCount == 0)
                {
                    MessageBox.Show("Current DLN-series adapter doesn't support I2C Master interface.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //try
                //{
                //    DLNdevice[i].I2cMaster.Ports[0].ScanDevices();
                //}

                //catch 
                //{
                //    MessageBox.Show(string.Format("Port {0} Slave Set Failed..", portID));
                //    return false;
                //}

                DLNi2c[portID] = DLNdevice[i].I2cMaster.Ports[0];
                DLNgpio[portID] = DLNdevice[i].Gpio;
            }

            if (STATIC.Rcp.Option.isPosture)
            {
				if(option.is1CH_MC/*STATIC.Rcp.Model.ModelName == "SO1C81_M1"*/) //자세차 1Port 용
                {
					//Output
					for (int i = 0; i < 2; i++)
					{
						//Output
						//PinSet(i, SOCKET_OUT, 1);
						//PinSet(i, SOCKET_IN, 1);
						//PinSet(i, RESET1, 1);
						//PinSet(i, RESET2, 1);
						PinSet(i, RED_LAMP2, 1);
						PinSet(i, GREEN_LAMP2, 1);
						PinSet(i, YELLOW_LAMP2, 1);
                        //Input
                        PinSet(i, START, 0);
                        //        PinSet(i, EMG, 0);
                        PinSet(i, STOP, 0);
                    //    PinSet(i, POWERON, 0);
						PinSet(i, 0, 0);
                        PinSet(i, BUZZER, 1);
                        PinSet(i, LIGHT_CURETAIN, 0);
                    }


                    //Start
                    DLNgpio[0].Pins[START].ConditionMetThreadSafe += SWEventHandler;
                    DLNgpio[0].Pins[START].SetEventConfiguration(EventType.LevelHigh, 50);
                    //EMG
                    //DLNgpio[1].Pins[EMG].ConditionMetThreadSafe += EMGEventHandler; ;
                    //DLNgpio[1].Pins[EMG].SetEventConfiguration(EventType.LevelHigh, 50);

                    DLNgpio[0].Pins[STOP].ConditionMetThreadSafe += StopEventHandler;
                    DLNgpio[0].Pins[STOP].SetEventConfiguration(EventType.LevelHigh, 50);

                    //DLNgpio[0].Pins[STOP].ConditionMetThreadSafe += StopEventHandler;
                    //DLNgpio[0].Pins[STOP].SetEventConfiguration(EventType.LevelHigh, 50);

                    //Stop
                    //DLNgpio[1].Pins[RESET1].ConditionMetThreadSafe += StopEventHandler; ;
                    //DLNgpio[1].Pins[RESET1].SetEventConfiguration(EventType.LevelHigh, 50);
                    //Reset
                    DLNgpio[1].Pins[0].ConditionMetThreadSafe += ResetEventHandler; ;
                    DLNgpio[1].Pins[0].SetEventConfiguration(EventType.LevelHigh, 50);
                    //Safe
                    DLNgpio[1].Pins[LIGHT_CURETAIN].ConditionMetThreadSafe += SafeEventHandler;
                    DLNgpio[1].Pins[LIGHT_CURETAIN].SetEventConfiguration(EventType.LevelLow, 50);
                }
                else if (!option.is1CH_MC/*STATIC.Rcp.Model.ModelName == "SO1C81_M2"*/)
                {
					for(int i = 0; i < 2; i++)
                    {   
						//Output
                        PinSet(i, SOCKET_OUT_M, 1);
                        PinSet(i, SOCKET_IN_M, 1);
                        PinSet(i, COVER_UP, 1);
                        PinSet(i, COVER_DOWN, 1);
                        PinSet(i, RED_LAMP2, 1);
                        PinSet(i, YELLOW_LAMP2, 1);
                        PinSet(i, GREEN_LAMP2, 1);
                        PinSet(i, BUZZER, 1);
                        //Input
                        PinSet(i, LIGHT_CURETAIN, 0);
                        PinSet(i, START, 0);
                        PinSet(i, STOP, 0);
                    }

                    //Start
                    DLNgpio[0].Pins[START].ConditionMetThreadSafe += SWEventHandler;
                    DLNgpio[0].Pins[START].SetEventConfiguration(EventType.LevelHigh, 50);
                    //Stop
                    DLNgpio[0].Pins[STOP].ConditionMetThreadSafe += StopEventHandler;
                    DLNgpio[0].Pins[STOP].SetEventConfiguration(EventType.LevelHigh, 50);
                    //Safe
                    DLNgpio[1].Pins[LIGHT_CURETAIN].ConditionMetThreadSafe += SafeEventHandler;
                    DLNgpio[1].Pins[LIGHT_CURETAIN].SetEventConfiguration(EventType.LevelLow, 50);
                }
            }
			else
			{
				for (int i = 0; i < 2; i++)
				{
					PinSet(i, SOCKET_OUT, 1);
					PinSet(i, SOCKET_IN, 1);
					//Input
					PinSet(i, START, 0);
				}
                //Start
                DLNgpio[0].Pins[START].ConditionMetThreadSafe += SWEventHandler;
                DLNgpio[0].Pins[START].SetEventConfiguration(EventType.LevelHigh, 50);
            }

			DLNgpio[1].Pins[POWERON].Enabled = true;
			DLNgpio[1].Pins[POWERON].Direction = 0;
			DLNgpio[1].Pins[POWERON].OutputValue = 1;	
            return true;
        }

		private void ResetEventHandler(object sender, ConditionMetEventArgs e)
		{
			if (e.Value == 1 && !IsReset)
			{
				if (IsEMG) return;
				DLNgpio[1].Pins[0].SetEventConfiguration(EventType.LevelLow, 50);
				IsReset = true;
				ResetOn?.Invoke(null, EventArgs.Empty);
			}
			else if (e.Value == 0 && IsReset)
			{
				DLNgpio[1].Pins[0].SetEventConfiguration(EventType.LevelHigh, 50);
				IsReset = false;
			}
		}

     

        private void EMGEventHandler(object sender, ConditionMetEventArgs e)
        {
			tm_EMG.Enabled = true;
			if(e.Value == 1)
			{
				if (!st_EMG.IsRunning)
					st_EMG.Start();
				if(st_EMG.ElapsedMilliseconds> 200 && !IsEMG)
				{
					IsEMG = true;
                    DLNgpio[1].Pins[EMG].SetEventConfiguration(EventType.LevelLow, 50);
                    EMGOn?.Invoke(null, EventArgs.Empty);
					st_EMG.Stop();
					st_EMG.Reset();
                }
			}
			else if(e.Value == 0 && IsEMG)
			{
                DLNgpio[1].Pins[EMG].SetEventConfiguration(EventType.LevelHigh, 50);
                IsEMG = false;
				st_EMG.Stop();
				st_EMG.Reset();
            }
          
        }
        private void StopEventHandler(object sender, ConditionMetEventArgs e)
        {
            if (option.isPosture && option.is1CH_MC/*STATIC.Rcp.Model.ModelName == "SO1C81_M1"*/)
            {
                if (e.Value == 1 && !IsStop)
                {
                    DLNgpio[0].Pins[STOP].SetEventConfiguration(EventType.LevelLow, 50);
                    IsStop = true;
                    StopOn?.Invoke(null, EventArgs.Empty);
                }
                else if (e.Value == 0 && IsStop)
                {
                    DLNgpio[0].Pins[STOP].SetEventConfiguration(EventType.LevelHigh, 50);
                    IsStop = false;
                }
            }
            else if (option.isPosture && !option.is1CH_MC/*STATIC.Rcp.Model.ModelName == "SO1C81_M2"*/)
            {
				tm_stop.Enabled = true;
				if(e.Value == 1)
				{
					if (!st_stop.IsRunning)
						st_stop.Start();
					if (IsEMG) return;

					if(st_stop.ElapsedMilliseconds > 200 && !IsStop)
					{
                        IsStop = true;
                        DLNgpio[0].Pins[STOP].SetEventConfiguration(EventType.LevelLow, 50);
                        StopOn?.Invoke(null, EventArgs.Empty);
						st_stop.Stop();
						st_stop.Reset();
                    }
				}
				else if(e.Value == 0 && IsStop)
				{
                    DLNgpio[0].Pins[STOP].SetEventConfiguration(EventType.LevelHigh, 50);
                    IsStop = false;
                    st_stop.Stop();
                    st_stop.Reset();
                }
            }
        }
        private void SWEventHandler(object sender, ConditionMetEventArgs e)
        {
			tm.Enabled = true;
			
            if (e.Value == 1)
            {
				if(!st.IsRunning)
                    st.Start();
                if (IsEMG) return;
             
              
				if(st.ElapsedMilliseconds > 500 && !IsSwitch)
				{
					IsSwitch = true;
                    DLNgpio[0].Pins[START].SetEventConfiguration(EventType.LevelLow, 50);
                    SwitchOn?.Invoke(null, EventArgs.Empty);
					st.Stop();
					st.Reset();
				
                }
                

            }
            else if (e.Value == 0 && IsSwitch)
            {
                DLNgpio[0].Pins[START].SetEventConfiguration(EventType.LevelHigh, 50);
                IsSwitch = false;
                st.Stop();
                st.Reset();
               
               
            }
        }

        private void SafeEventHandler(object sender, ConditionMetEventArgs e)
        {
			if (e.Value == 1 && IsSafeOn)
			{
				DLNgpio[1].Pins[LIGHT_CURETAIN].SetEventConfiguration(EventType.LevelLow, 50);
				IsSafeOn = false;
			}
			else if (e.Value == 0 && !IsSafeOn)
			{

				if (option.SafeSensor)
				{
					DLNgpio[1].Pins[LIGHT_CURETAIN].SetEventConfiguration(EventType.LevelHigh, 50);
					IsSafeOn = true;
				}
				else
				{
                  //  DLNgpio[1].Pins[LIGHT_CURETAIN].SetEventConfiguration(EventType.LevelLow, 50);
                    IsSafeOn = false;
				}
            }
		}

        public double GetCurrent(int ch)
        {
            double res = 0;
            int RegAddr = 0x01;
            byte[] buffer2 = new byte[2];
            try
            {
				lock (i2cLock) DLNi2c[ch].Read(0x40, 1, RegAddr, buffer2);
                res = (buffer2[0] * 256 + buffer2[1]) / 10.0;
            }
            catch
            {
                res = 0;
            }
            return res;
        }

        public void LoadSocket(bool bUp)
        {
            if (Process.IsVirtual) return;
            if (DLNgpio == null) return;
            if (option.isPosture && !option.is1CH_MC/*STATIC.Rcp.Model.ModelName == "SO1C81_M2"*/)
            {
                if (!IsCoverUp) return;
                if (bUp)
                {
					lock(i2cLock)
					{
                        DLNgpio[1].Pins[SOCKET_OUT_M].OutputValue = 0;
                        DLNgpio[1].Pins[SOCKET_IN_M].OutputValue = 1;
                    }
                    Thread.Sleep(1000);
                    IsLoad = true;
                }
                else
                {
					lock(i2cLock)
					{
                        DLNgpio[1].Pins[SOCKET_OUT_M].OutputValue = 1;
                        DLNgpio[1].Pins[SOCKET_IN_M].OutputValue = 0;
                    }
                    Thread.Sleep(500);
                    IsLoad = false;
                }
            }
            else
            {
                if (bUp)
                {
					lock(i2cLock)
					{
                        DLNgpio[0].Pins[SOCKET_OUT].OutputValue = 0;
                        DLNgpio[0].Pins[SOCKET_IN].OutputValue = 1;
                    }
                    IsLoad = true;
                }
                else
                {
					lock(i2cLock)
					{
                        DLNgpio[0].Pins[SOCKET_OUT].OutputValue = 1;
                        DLNgpio[0].Pins[SOCKET_IN].OutputValue = 0;
                    }
                    IsLoad = false;
                }
            }
        }
        public void CoverUpDown(bool bUp)
        {
            if (Process.IsVirtual) return;
            if (DLNgpio == null) return;
            if (bUp)
            {
				lock(i2cLock)
				{
                    DLNgpio[1].Pins[COVER_DOWN].OutputValue = 1;
                    DLNgpio[1].Pins[COVER_UP].OutputValue = 0;
                }
                Thread.Sleep(500);
                IsCoverUp = true;
            }
            else
            {
				lock(i2cLock)
				{
                    DLNgpio[1].Pins[COVER_DOWN].OutputValue = 0;
                    DLNgpio[1].Pins[COVER_UP].OutputValue = 1;
                }
                Thread.Sleep(500);
                IsCoverUp = false;
            }
        }
        public bool WriteCurrent(int ch, int value)
        {
            byte[] bufferL = new byte[1];
            int lch = (ch / 2) * 2 + 1; //  0,1 -> 1    ;   2,3 -> 3
            int lDACaddr = 0x4C + (ch % 2);
            byte bufferH = (byte)(value / 256);
            bufferL[0] = (byte)(value % 256);
            try
            {
				lock (i2cLock) { if (DLNi2c[lch] != null) DLNi2c[lch].Write(lDACaddr, 1, bufferH, bufferL); }              
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SetLEDpower(int id, int value)
        {
			if (DLNi2c == null) return;

            byte bufferH = 0;
            byte[] bufferL = new byte[1];

            int lDACaddr = 0x4F;        // A0,A1상태에 따라 ID 변경, 지금은  A0,A1 pull up

            //  기존 single channel dac code
            //   | XXXX | XXXX |  
            //   | XXXX | XXXX | XXXX | 0000 |
            //   | Address | CtrlByte | Value(12bit) |
            bufferH = (byte)(value / 16);
            bufferL[0] = (byte)(value << 4);
            //  기존 single channel dac code
            //bufferH = (byte)(value / 256);
            //bufferL[0] = (byte)(value % 256);
            byte[] LeftTopLed = { 0x10 };      //1
            byte[] RightToptLed = { 0x12 };    //2
            byte[] LeftBottomLed = { 0x14 };     //3
            byte[] RightBottomLed = { 0x16 };   //4

            //int ch = 0;
            int ch = 1;
            if (DLNi2c[ch] == null) return;
            try
            {
                if (id == 0)
                {
                    byte[] datas = { LeftTopLed[0], bufferH, bufferL[0] };
                    lock(i2cLock) DLNi2c[ch].Write(lDACaddr, datas); // diolan(0,1기준) 1번에서  LED control
                }
                else if (id == 1)
                {
                    byte[] datas = { RightToptLed[0], bufferH, bufferL[0] };
                    lock (i2cLock) DLNi2c[ch].Write(lDACaddr, datas); // diolan(0,1기준) 1번에서  LED control
                }
                else if (id == 2)
                {
                    byte[] datas = { LeftBottomLed[0], bufferH, bufferL[0] };
                    lock (i2cLock) DLNi2c[ch].Write(lDACaddr, datas); // diolan(0,1기준) 1번에서  LED control
                }
                else if (id == 3)
                {
                    byte[] datas = { RightBottomLed[0], bufferH, bufferL[0] };
                    lock (i2cLock) DLNi2c[ch].Write(lDACaddr, datas); // diolan(0,1기준) 1번에서  LED control
                }

            }
            catch (Exception ex)
            {
    //            MessageBox.Show("Fail to Set LED Power :: " + ex.Message);
				//DLNi2c[ch] = null;
            }
        }

        public bool TopLED_OnOff(bool LEDOn = false)
        {
            try
            {
                if (LEDOn)
                {
                    //TOP LED ON
					lock(i2cLock)
					{
                        DLNgpio[1].Pins[START].Enabled = true;
                        DLNgpio[1].Pins[START].Direction = 1;
                        DLNgpio[1].Pins[START].OutputValue = 1;
                        DLNgpio[1].Pins[START].PulldownEnabled = false;
                        DLNgpio[1].Pins[START].PullupEnabled = true;
                    }
                    LEDOn = true;
                }
                else
                {
					lock(i2cLock)
					{
                        DLNgpio[1].Pins[START].Enabled = true;
                        DLNgpio[1].Pins[START].Direction = 0;
                        DLNgpio[1].Pins[START].OutputValue = 0;
                        DLNgpio[1].Pins[START].PulldownEnabled = false;
                        DLNgpio[1].Pins[START].PullupEnabled = true;
                    }
                    // TOP LED OFF
                    LEDOn = false;
                }

            }
            catch (Exception)
            {
                throw;
            }

            return LEDOn;
        }


        //Base I2c Func==========================================================================
        public bool WriteArray(int ch, int slaveAddr, int memAddr, int memCnt, byte[] data, bool OccupiedDisable = false)
        {          
            try
            {
                if (IsVirtual) return true;

				lock (i2cLock) { if (DLNi2c[ch] != null) DLNi2c[ch].Write(slaveAddr, memCnt, memAddr, data); }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Write_2byte(int ch, int slaveAddr, int memAddr, int memCnt, ushort data)
        {
			byte[] wdata = new byte[2];
			wdata[0] = (byte)((data >> 8) & 0x00FF);
            wdata[1] = (byte)((data) & 0x00FF);
           
            try
            {
                if (IsVirtual) return true;

				lock (i2cLock) { if (DLNi2c[ch] != null) DLNi2c[ch].Write(slaveAddr, memCnt, memAddr, wdata); }
             
                return true;
            }
            catch
            {
                return false;
            }
        }
		public bool ReadArray(int ch, int slaveAddr, int memAddr, int memCnt, byte[] data, bool OccupiedDisable = false)
        {
           
            try
            {
                if (IsVirtual) return true;
				lock (i2cLock) { if (DLNi2c[ch] != null) DLNi2c[ch].Read(slaveAddr, memCnt, memAddr, data); }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public double Read_2Byte(int ch, int slaveAddr, int memAddr, int memCnt)
        {
			byte[] rData = new byte[2];       
            try
            {
                if (IsVirtual) return 0;

				lock (i2cLock) { if (DLNi2c[ch] != null) DLNi2c[ch].Read(slaveAddr, memCnt, memAddr, rData); }
                return (double)((rData[0] << 8) + rData[1]);
            }
            catch
            {
                return -1;
            }
        }
        public ushort Read2Byte(int ch, int slaveAddr, int memAddr, int memCnt)
        {
            byte[] b = new byte[2];
            ReadArray(ch, slaveAddr, memAddr, memCnt, b);
            return (ushort)((b[0] << 8) | b[1]);
        }
        public short Read2Byte_Short(int ch, int slaveAddr, int memAddr, int memCnt)
        {
            byte[] b = new byte[2];
            ReadArray(ch, slaveAddr, memAddr, memCnt, b);
            return (short)((b[0] << 8) | b[1]);
       
        }

        public double Read_3Byte(int ch, int slaveAddr, int memAddr, int memCnt)
        {
            byte[] rData = new byte[3];
          
            try
            {
                if (IsVirtual) return 0;

				lock (i2cLock) { if (DLNi2c[ch] != null) DLNi2c[ch].Read(slaveAddr, memCnt, memAddr, rData); }
                return (double)((rData[0] << 16) + (rData[1] << 8) + rData[2]);
            }
            catch
            {
                return -1;
            }
        }

		public void powerOnOff(bool isOn)
		{
			if (DLNgpio == null) return;
			lock (i2cLock)
            {
                if (isOn) { DLNgpio[1].Pins[POWERON].Direction = 1; }
                else { DLNgpio[1].Pins[POWERON].Direction = 0; }
            }
		}


        //=======================================================================================
    }
    public class DriverIC
    {
        public const int OIS_X = 0;
        public const int OIS_Y = 1;
        public const int IRIS = 2;
        public const int AK = 3;
        public LogEvent Log { get { return STATIC.LogEvent; } }
        public Condition Condition { get { return STATIC.Rcp.Condition; } }
        public Spec Spec { get { return STATIC.Rcp.Spec; } }
        public Option Option { get { return STATIC.Rcp.Option; } }
        public Model Model { get { return STATIC.Rcp.Model; } }
        public DLN Dln { get; set; }
        public string Name { get; set; }
        public int[] AkSlave = new int[2];


        public int RohmSlave { get; set; }

        public struct sPoint
        {
            public double x;
            public double y;
        };
        public struct sLine
        {
            public double dSlope;
            public double dYintercept;
        };

        public sLine Line_fitting(sPoint[] data, int dataSize)
        {
            sLine rtnLine;
            double SUMx = 0; //sum of x values
            double SUMy = 0; //sum of y values
            double SUMxy = 0; //sum of x * y
            double SUMxx = 0; //sum of x^2 
            double slope = 0; //slope of regression line
            double y_intercept = 0; //y intercept of regression line 
            double AVGy = 0; //mean of y
            double AVGx = 0; //mean of x

            //calculate various sums 
            for (int i = 0; i < dataSize; i++)
            {
                SUMx = SUMx + data[i].x;
                SUMy = SUMy + data[i].y;
                SUMxy = SUMxy + data[i].x * data[i].y;
                SUMxx = SUMxx + data[i].x * data[i].x;
            }

            //calculate the means of x and y
            AVGy = SUMy / dataSize;
            AVGx = SUMx / dataSize;

            //slope or a1
            slope = (dataSize * SUMxy - SUMx * SUMy) / (dataSize * SUMxx - SUMx * SUMx);

            //y itercept or a0
            y_intercept = AVGy - slope * AVGx;

            rtnLine.dSlope = slope;
            rtnLine.dYintercept = y_intercept;

            return rtnLine;
        }


        public bool WaiteCheck(int ch, int sAddr, int memAddr, int memCnt, int target, int timeout = 1000)
        {
            byte[] rData = new byte[1];
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (!Dln.ReadArray(ch, sAddr, memAddr, memCnt, rData))
                {
                    return false;
                }
                if (rData[0] == target) return true;
                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.AddLog(ch, string.Format("Time Out Read Mem 0x{0:X2} at Target : 0x{1:X2}", memCnt, target));
                    sw.Stop();
                    return false;
                }
            }
        }
        public virtual bool Move(int ch, int mode, int val, bool b = false) { return true; }
		public virtual int ReadHall(int ch, int mode, bool b = false) { return 0; }
        public virtual bool IRIS_IC_Init(int ch) { return true; }
        public virtual bool OIS_IC_Init(int ch) { return true; }
        public virtual bool AF_IC_Init(int ch) { return true; }
        public virtual bool IRIS_Servo_On(int ch) { return true; }
        public virtual bool AF_Adjustment(int ch) { return true; }
        public virtual bool IRIS_Adjustment(int ch) { return true; }
        public virtual bool IRIS_Adjustment_byMode(int ch, int Mode) { return true; }
     
        public virtual bool OIS_Adjustment(int ch) { return true; }
        public virtual bool IRIS_Adjustment_DataCheck(int ch) { return true; }
        public virtual bool ReadPID(string path) { return true; }
        public virtual (double PM, double Freq) AK7316_PhaseMargin(int ch, ushort FinalFreq, ushort StartFreq, ushort StepFreq, ushort Amp, ushort GainThr) { return (1, 1); }


    }

    public class SO1C81 : DriverIC
    {
   
		byte[] IC_SETTING_AKM7316 = new byte[1];
        byte[] IC_SETTING_AKM7316_Reg = new byte[1];

        public byte Addr19Val = 0xFF;
        byte[] IC_DATA_AKM7316 = new byte[1];
		double[] Pm_2nd = new double[2];
		int RohmFlashAddr = 0x50;
        public SO1C81(DLN dln)
        {
            Dln = dln;
            Name = "SO1C81";
            RohmSlave = 0x3E;
            AkSlave[0] = 0x4C;
            AkSlave[1] = 0x4C;
        }
        public override bool Move(int ch, int mode, int val, bool b = false)
        {
            switch (mode)
            {
                case AK:
					
					if (!Dln.WriteArray(ch, AkSlave[ch], 0x00, 1, new byte[2] { (byte)(val >> 4), (byte)(val << 4) }, b)) return false;                
                    break;
                default: return false;
            }
            return true;
        }
        public override int ReadHall(int ch, int mode, bool b = false)
        {
            byte[] rData = new byte[4];
            switch (mode)
            {
                case AK:
                    if (!Dln.ReadArray(ch, AkSlave[ch], 0x84, 1, rData, b)) return 0;
                    return ((rData[1] >> 4) + (rData[0] << 4));
                default: return 0;
            }
        }
        public override bool IRIS_IC_Init(int ch)
        {
            //bool tmpbool = AK7316_Address_Change(ch);
            //if (!tmpbool)
            //    return false;
            bool tmpbool = AK7316_addr_check(ch);
			if (!tmpbool)
                return false;
            Dln.WriteArray(ch, AkSlave[ch], 0x02, 1, new byte[1] { 0x40 });
            Log.AddLog(ch, string.Format("IRIS Mode OFF."));
            AK7316_memory_upadate(ch, 5);
            Move(ch, DriverIC.AK, 2048);
            Dln.WriteArray(ch, AkSlave[ch], 0x02, 1, new byte[1] { 0x00 });
            Log.AddLog(ch, string.Format("IRIS Mode ON."));
            return true;
        }
        public override bool IRIS_Adjustment(int ch)
        {
            byte[] rbuf = new byte[1];


            byte index, cTemp, cBackup;
            Dln.WriteArray(ch, AkSlave[ch], 0x02, 1, new byte[1] { 0x40 });
            Log.AddLog(ch, string.Format("IRIS Mode OFF."));
            Dln.WriteArray(ch, AkSlave[ch], 0xAE, 1, new byte[1] { 0x3B });

            //Dln.WriteArray(ch, AkSlave[ch], 0x0C, 1, new byte[1] { Condition.CalVal1 });
            //Log.AddLog(ch, string.Format("0x0C = 0x{0}", Condition.CalVal1.ToString("X2")));
            Dln.WriteArray(ch, AkSlave[ch], IC_SETTING_AKM7316_Reg[1], 1, new byte[1] { IC_SETTING_AKM7316[1] });
            Dln.WriteArray(ch, AkSlave[ch], IC_SETTING_AKM7316_Reg[0], 1, new byte[1] { IC_SETTING_AKM7316[0] });
            //if (Option.ReverseDrv)
            //    Dln.WriteArray(ch, AkSlave[ch], 0x0E, 1, new byte[1] { 0x84 });
            //else
            //    Dln.WriteArray(ch, AkSlave[ch], 0x0E, 1, new byte[1] { 0x80 });
            Dln.WriteArray(ch, AkSlave[ch], IC_SETTING_AKM7316_Reg[2], 1, new byte[1] { IC_SETTING_AKM7316[2] });
            Dln.WriteArray(ch, AkSlave[ch], IC_SETTING_AKM7316_Reg[3], 1, new byte[1] { IC_SETTING_AKM7316[3] });
            Dln.WriteArray(ch, AkSlave[ch], IC_SETTING_AKM7316_Reg[4], 1, new byte[1] { IC_SETTING_AKM7316[4] });
            Dln.WriteArray(ch, AkSlave[ch], IC_SETTING_AKM7316_Reg[5], 1, new byte[1] { IC_SETTING_AKM7316[5] });
            //Dln.WriteArray(ch, AkSlave[ch], 0xC0, 1, new byte[2] { 0x00, 0x00 });
            //Dln.WriteArray(ch, AkSlave[ch], 0xC1, 1, new byte[2] { 0x00, 0x00 });
            //Dln.WriteArray(ch, AkSlave[ch], 0xC2, 1, new byte[2] { 0x00, 0x00 });
            //Dln.WriteArray(ch, AkSlave[ch], 0xC3, 1, new byte[2] { 0x00, 0x00 });

            for (int i = 0xC0; i <= 0xC3; i++)
                Dln.WriteArray(ch, AkSlave[ch], i, 1, new byte[] { 0x00 });
            Log.AddLog(ch, string.Format("Reset EPA data."));


            for (index = 0xC5; index <= 0xDF; index++)
                Dln.WriteArray(ch, AkSlave[ch], index, 1, new byte[1] { 0x00 });
            Log.AddLog(ch, string.Format("Reset Linearity comp coeff data."));

            for (int i = 0; i < IC_DATA_AKM7316.Length; i += 2)
                Dln.WriteArray(ch, AkSlave[ch], IC_DATA_AKM7316[i], 1, new byte[1] { IC_DATA_AKM7316[i + 1] });
            Log.AddLog(ch, string.Format("PID Parameter setting."));

            Thread.Sleep(10);


            //Dln.WriteArray(ch, AkSlave[ch], 0x5D, 1, new byte[1] { 0x00 });
            Dln.ReadArray(ch, AkSlave[ch], 0x70, 1, rbuf);
            cBackup = cTemp = rbuf[0];
            Log.AddLog(ch, string.Format("1 Reg 0x90 : 0x{0:X2}", cTemp));
            cBackup &= 0x80;
            Log.AddLog(ch, string.Format("2 Reg 0x90 : 0x{0:X2}", cBackup));
            cTemp = (byte)((cTemp << 1) & 0x7E);
            cTemp |= cBackup;
            Log.AddLog(ch, string.Format("3 Reg 0x90 : 0x{0:X2}", cTemp));
            Dln.ReadArray(ch, AkSlave[ch], 0x71, 1, rbuf);
            cBackup = rbuf[0];
            cBackup &= 0x80;
            Log.AddLog(ch, string.Format("4 Reg 0x90 : 0x{0:X2}", cBackup));
            cTemp |= (byte)(cBackup >> 7);
            Log.AddLog(ch, string.Format("5 Reg 0x5D : 0x{0:X2}", cTemp));
            //	cTemp = (byte)(rbuf[0] / 2);


            Dln.WriteArray(ch, AkSlave[ch], 0x5D, 1, new byte[1] { cTemp });
            Dln.WriteArray(ch, AkSlave[ch], 0x0C, 1, new byte[] { 0x62 });
            Dln.WriteArray(ch, AkSlave[ch], 0x02, 1, new byte[] { 0x18 });
            Thread.Sleep(150);
            STATIC.Read_Temp[ch] = cTemp;
            Log.AddLog(ch, string.Format("Setting2 data."));

            Dln.ReadArray(ch, AkSlave[ch], 0x19, 1, rbuf);
            rbuf[0] = (byte)((rbuf[0] - 0x80) * 2 + 0x80);
            if (0x80 <= rbuf[0] && rbuf[0] <= 0xB0)
            {
                Log.AddLog(ch, string.Format("Reg 0x19 : 0x{0:X2}", rbuf[0]));
                Dln.WriteArray(ch, AkSlave[ch], 0x19, 1, rbuf);
                //Dln.WriteArray(ch, DrvIC.AkSlave[ch], 0xF3, 1, new byte[] { 0x1C });
                //Thread.Sleep(30);
                Dln.WriteArray(ch, AkSlave[ch], 0x03, 1, new byte[] { 0x01 });
                Thread.Sleep(100);
                Dln.WriteArray(ch, AkSlave[ch], 0x03, 1, new byte[] { 0x02 });
                Thread.Sleep(180);
                Dln.WriteArray(ch, AkSlave[ch], 0x03, 1, new byte[] { 0x04 });
                Thread.Sleep(150);
                Dln.WriteArray(ch, AkSlave[ch], 0x03, 1, new byte[] { 0x08 });
                Thread.Sleep(160);

                Dln.ReadArray(ch, AkSlave[ch], 0x4B, 1, rbuf);
                if ((rbuf[0] & 0x02) != 0x00)
                {
                    return false;
                }
            }
            else { return false; }
            Dln.WriteArray(ch, AkSlave[ch], 0xAE, 1, new byte[1] { 0x00 });
            //Move(ch, DriverIC.AK, 4000);
            Dln.WriteArray(ch, AkSlave[ch], 0x02, 1, new byte[1] { 0x40 });
           // Log.AddLog(ch, string.Format("IRIS Mode ON."));

            return true;

        }

        public override bool IRIS_Adjustment_byMode(int ch, int Mode)
        {
   //         byte[] rbuf = new byte[1];
   //         byte[] rbuf2 = new byte[2];

   //         byte index, cTemp, cBackup;
   //         Dln.WriteArray(ch, AkSlave[ch], 0x02, 1, new byte[1] { 0x40 });
   //         Log.AddLog(ch, string.Format("IRIS Mode OFF."));
   //         Dln.WriteArray(ch, AkSlave[ch], 0xAE, 1, new byte[1] { 0x3B });

   //         Dln.WriteArray(ch, AkSlave[ch], 0x0C, 1, new byte[1] { Condition.CalVal1 });
   //         Log.AddLog(ch, string.Format("0x0C = 0x{0}", Condition.CalVal1.ToString("X2")));
   //         Dln.WriteArray(ch, AkSlave[ch], IC_SETTING_AKM7316_Reg[1], 1, new byte[1] { IC_SETTING_AKM7316[1] });
   //         Dln.WriteArray(ch, AkSlave[ch], IC_SETTING_AKM7316_Reg[0], 1, new byte[1] { IC_SETTING_AKM7316[0] });
   //         if (Option.ReverseDrv)
   //             Dln.WriteArray(ch, AkSlave[ch], 0x0E, 1, new byte[1] { 0x84 });
   //         else
   //             Dln.WriteArray(ch, AkSlave[ch], 0x0E, 1, new byte[1] { 0x80 });
   //         Dln.WriteArray(ch, AkSlave[ch], IC_SETTING_AKM7316_Reg[4], 1, new byte[1] { IC_SETTING_AKM7316[4] });
   //         Dln.WriteArray(ch, AkSlave[ch], IC_SETTING_AKM7316_Reg[5], 1, new byte[1] { IC_SETTING_AKM7316[5] });
   //         Dln.WriteArray(ch, AkSlave[ch], IC_SETTING_AKM7316_Reg[6], 1, new byte[1] { IC_SETTING_AKM7316[6] });
   //         Dln.WriteArray(ch, AkSlave[ch], 0xC0, 1, new byte[2] { 0x00, 0x00 });
   //         Dln.WriteArray(ch, AkSlave[ch], 0xC1, 1, new byte[2] { 0x00, 0x00 });
   //         Dln.WriteArray(ch, AkSlave[ch], 0xC2, 1, new byte[2] { 0x00, 0x00 });
   //         Dln.WriteArray(ch, AkSlave[ch], 0xC3, 1, new byte[2] { 0x00, 0x00 });



   //         for (index = 0xC5; index <= 0xDF; index++)
   //             Dln.WriteArray(ch, AkSlave[ch], index, 1, new byte[1] { 0x00 });
   //         Log.AddLog(ch, string.Format("Reset Linearity comp coeff data."));

   //         for (int i = 0; i < IC_DATA_AKM7316.Length; i += 2)
   //             Dln.WriteArray(ch, AkSlave[ch], IC_DATA_AKM7316[i], 1, new byte[1] { IC_DATA_AKM7316[i + 1] });
   //         Log.AddLog(ch, string.Format("PID Parameter setting."));

   //         Dln.WriteArray(ch, AkSlave[ch], 0x5D, 1, new byte[1] { 0x00 });
   //         Dln.ReadArray(ch, AkSlave[ch], 0x90, 1, rbuf);
   //         cBackup = cTemp = rbuf[0];
   //         Log.AddLog(ch, string.Format("1 Reg 0x90 : 0x{0:X2}", cTemp));
   //         cBackup &= 0x80;
   //         Log.AddLog(ch, string.Format("2 Reg 0x90 : 0x{0:X2}", cBackup));
   //         cTemp = (byte)((cTemp << 1) & 0x7E);
   //         cTemp |= cBackup;
   //         Log.AddLog(ch, string.Format("3 Reg 0x90 : 0x{0:X2}", cTemp));
   //         Dln.ReadArray(ch, AkSlave[ch], 0x91, 1, rbuf);
   //         cBackup = rbuf[0];
   //         cBackup &= 0x80;
   //         Log.AddLog(ch, string.Format("4 Reg 0x90 : 0x{0:X2}", cBackup));
   //         cTemp |= (byte)(cBackup >> 7);
   //         Log.AddLog(ch, string.Format("5 Reg 0x5D : 0x{0:X2}", cTemp));
   //         //	cTemp = (byte)(rbuf[0] / 2);


   //         Dln.WriteArray(ch, AkSlave[ch], 0x5D, 1, new byte[1] { cTemp });
   //         STATIC.Read_Temp = cTemp;
   //         Log.AddLog(ch, string.Format("Setting2 data."));


   //         index = 0x00;
   //         cTemp = 0x12;

   //         Log.AddLog(ch, string.Format("Hall Cal Seq 1"));
   //         Dln.WriteArray(ch, AkSlave[ch], 0x0C, 1, new byte[1] { 0xE2/*IC_SETTING_AKM7316[2]*/ });
   //         Dln.WriteArray(ch, AkSlave[ch], 0x02, 1, new byte[1] { 0x18 });
			//Thread.Sleep(500);
			//STATIC.beforePcal[ch] = (short)(Dln.Read2Byte_Short(ch, AkSlave[ch], 0x04, 1));
   //         STATIC.beforeNcal[ch] = (short)(Dln.Read2Byte_Short(ch, AkSlave[ch], 0x06, 1));



   //         if (Mode == 0)
   //         {
   //             Log.AddLog(ch, string.Format("Hall Cal Seq 2"));
   //             Dln.WriteArray(ch, AkSlave[ch], 0x02, 1, new byte[1] { 0x00 });
   //             Dln.WriteArray(ch, AkSlave[ch], 0xA6, 1, new byte[1] { 0x7B });
   //             Dln.WriteArray(ch, AkSlave[ch], 0x00, 1, new byte[2] { 0x00, 0x00 });
   //             Thread.Sleep(500);
   //             Dln.ReadArray(ch, AkSlave[ch], 0x80, 1, rbuf2);
   //             Dln.WriteArray(ch, AkSlave[ch], 0x06, 1, rbuf2);
   //             Log.AddLog(ch, string.Format("0x80 : 0x{0}, 0x{1}", rbuf2[0].ToString("X2"), rbuf2[1].ToString("X2")));

   //             //int a = 4095;
   //             //  Dln.WriteArray(ch, AkSlave[ch], 0x00, 1, new byte[2] { (byte)(a >> 4), (byte)(a << 4) }); //옵션//고정

   //             Dln.WriteArray(ch, AkSlave[ch], 0x00, 1, new byte[2] { 0xFF, 0xFF });
   //             Thread.Sleep(500);
   //             Dln.ReadArray(ch, AkSlave[ch], 0x80, 1, rbuf2);
   //             Dln.WriteArray(ch, AkSlave[ch], 0x04, 1, rbuf2);
   //             Log.AddLog(ch, string.Format("0x80 : 0x{0}, 0x{1}", rbuf2[0].ToString("X2"), rbuf2[1].ToString("X2")));
   //             Dln.WriteArray(ch, AkSlave[ch], 0xA6, 1, new byte[1] { 0x00 });
			//	Dln.WriteArray(ch, AkSlave[ch], 0x00, 1, new byte[2] { 0x00, 0x00 });
   //             Thread.Sleep(300);
   //             STATIC.afterPcal[ch] = (short)(Dln.Read2Byte_Short(ch, AkSlave[ch], 0x04, 1));
   //             STATIC.afterNcal[ch] = (short)(Dln.Read2Byte_Short(ch, AkSlave[ch], 0x06, 1));
   //         }
   //         else if (Mode == 1)
   //         {
   //             Log.AddLog(ch, string.Format("Hall Cal Seq 3"));
   //             Dln.WriteArray(ch, AkSlave[ch], 0x02, 1, new byte[1] { 0x00 });
   //             Dln.WriteArray(ch, AkSlave[ch], 0xA6, 1, new byte[1] { 0x7B });
           

   //             Dln.WriteArray(ch, AkSlave[ch], 0x00, 1, new byte[2] { 0xFF, 0xFF });
   //             Thread.Sleep(500);
   //             Dln.ReadArray(ch, AkSlave[ch], 0x80, 1, rbuf2);
   //             Dln.WriteArray(ch, AkSlave[ch], 0x04, 1, rbuf2);
   //             Log.AddLog(ch, string.Format("0x80 : 0x{0}, 0x{1}", rbuf2[0].ToString("X2"), rbuf2[1].ToString("X2")));
   //             Dln.WriteArray(ch, AkSlave[ch], 0xA6, 1, new byte[1] { 0x00 });
   //             Dln.WriteArray(ch, AkSlave[ch], 0x00, 1, new byte[2] { 0x00, 0x00 });               
   //             Thread.Sleep(300);
   //             STATIC.afterPcal[ch] = (short)(Dln.Read2Byte_Short(ch, AkSlave[ch], 0x04, 1));
   //             STATIC.afterNcal[ch] = (short)(Dln.Read2Byte_Short(ch, AkSlave[ch], 0x06, 1));
   //         }
   //         else if(Mode == 2)
   //         {
   //             Log.AddLog(ch, string.Format("Hall Cal Seq 4"));
   //             Dln.WriteArray(ch, AkSlave[ch], 0x02, 1, new byte[1] { 0x00 });
   //             Dln.WriteArray(ch, AkSlave[ch], 0xA6, 1, new byte[1] { 0x7B });


   //             Dln.WriteArray(ch, AkSlave[ch], 0x00, 1, new byte[2] { 0xFF, 0xFF });
   //             Thread.Sleep(500);
   //             Dln.ReadArray(ch, AkSlave[ch], 0x80, 1, rbuf2);
   //             int a = (rbuf2[0] << 8) | rbuf2[1];
   //             if (a > STATIC.beforePcal[ch])
   //                 Dln.Write_2byte(ch, AkSlave[ch], 0x04, 1, (ushort)((STATIC.beforePcal[ch] + a) / 2));

   //             Log.AddLog(ch, string.Format("Before Pcal = {0}, After Pcal = {1}", STATIC.beforePcal[ch], a));
   //             Dln.WriteArray(ch, AkSlave[ch], 0xA6, 1, new byte[1] { 0x00 });
   //             Dln.WriteArray(ch, AkSlave[ch], 0x00, 1, new byte[2] { 0x00, 0x00 });
   //             Thread.Sleep(300);
   //             STATIC.afterPcal[ch] = (short)(Dln.Read2Byte_Short(ch, AkSlave[ch], 0x04, 1));
   //             STATIC.afterNcal[ch] = (short)(Dln.Read2Byte_Short(ch, AkSlave[ch], 0x06, 1));
   //         }




   //         Dln.WriteArray(ch, AkSlave[ch], 0xC0, 1, new byte[1] { Condition.CalVal2 });
   //         Log.AddLog(ch, string.Format("0xC0 = 0x{0}", Condition.CalVal2.ToString("X2")));
   //         Dln.WriteArray(ch, AkSlave[ch], 0xC1, 1, new byte[1] { Condition.CalVal3 });
   //         Log.AddLog(ch, string.Format("0xC1 = 0x{0}", Condition.CalVal3.ToString("X2")));
   //         Dln.WriteArray(ch, AkSlave[ch], 0xC2, 1, new byte[1] { Condition.CalVal4 });
   //         Log.AddLog(ch, string.Format("0xC2 = 0x{0}", Condition.CalVal4.ToString("X2")));
   //         Dln.WriteArray(ch, AkSlave[ch], 0xC3, 1, new byte[1] { Condition.CalVal5 });
   //         Log.AddLog(ch, string.Format("0xC3 = 0x{0}", Condition.CalVal5.ToString("X2")));

   //         if (Option.UseAddr19)
   //         {
   //             Dln.WriteArray(ch, AkSlave[ch], 0x19, 1, new byte[1] { Addr19Val });
   //             Log.AddLog(ch, string.Format("0x19 = 0x{0}", Addr19Val.ToString("X2")));

   //         }


   //         AK7316_memory_upadate(ch, 1);
   //         AK7316_memory_upadate(ch, 2);
   //         AK7316_memory_upadate(ch, 3);
   //         AK7316_memory_upadate(ch, 4);
   //         AK7316_memory_upadate(ch, 5);
   //         Dln.WriteArray(ch, AkSlave[ch], 0xAE, 1, new byte[1] { 0x00 });
   //         Move(ch, DriverIC.AK, 4000);
   //         Dln.WriteArray(ch, AkSlave[ch], 0x02, 1, new byte[1] { 0x00 });
            //Log.AddLog(ch, string.Format("IRIS Mode ON."));

            return true;

        }

    


        public override bool IRIS_Adjustment_DataCheck(int ch)
		{
			byte[] rbuf = new byte[1];
			byte[] rbuf2 = new byte[2];

			Dln.ReadArray(ch, AkSlave[ch], 0x0A, 1, rbuf);
			if (rbuf[0] != IC_SETTING_AKM7316[0])
            {
                Log.AddLog(ch, string.Format("0x0A = 0x{0}.", rbuf[0].ToString("X2")));
                return false;
			}
            Dln.ReadArray(ch, AkSlave[ch], 0x0B, 1, rbuf);
            if (rbuf[0] != IC_SETTING_AKM7316[1])
            {
                Log.AddLog(ch, string.Format("0x0B = 0x{0}.", rbuf[0].ToString("X2")));
                return false;
            }
            //Dln.ReadArray(ch, AkSlave[ch], 0x0E, 1, rbuf);
            //if (rbuf[0] != IC_SETTING_AKM7316[3])
            //{
            //    Log.AddLog(ch, string.Format("0x0E = 0x{0}.", rbuf[0].ToString("X2")));
            //    return false;
            //}
            Dln.ReadArray(ch, AkSlave[ch], 0x0F, 1, rbuf);
            if (rbuf[0] != IC_SETTING_AKM7316[3])
            {
                Log.AddLog(ch, string.Format("0x0F = 0x{0}.", rbuf[0].ToString("X2")));
                return false;
            }
            Dln.ReadArray(ch, AkSlave[ch], 0x08, 1, rbuf);
            if (rbuf[0] != IC_SETTING_AKM7316[4])
            {
                Log.AddLog(ch, string.Format("0x08 = 0x{0}.", rbuf[0].ToString("X2")));
                return false;
            }
            Dln.ReadArray(ch, AkSlave[ch], 0x09, 1, rbuf);
            if (rbuf[0] != IC_SETTING_AKM7316[5])
            {
                Log.AddLog(ch, string.Format("0x09 = 0x{0}.", rbuf[0].ToString("X2")));
                return false;
            }
            for (int i = 0; i < IC_DATA_AKM7316.Length; i += 2)
            {
                if (IC_DATA_AKM7316[i] != 0x5D && IC_DATA_AKM7316[i] != 0x02)
                {
                    Dln.ReadArray(ch, AkSlave[ch], IC_DATA_AKM7316[i], 1, rbuf);

                    if (rbuf[0] != IC_DATA_AKM7316[i + 1])
                    {
                        Log.AddLog(ch, string.Format("0x{0} = 0x{1}.", IC_DATA_AKM7316[i].ToString("X2"), rbuf[0].ToString("X2")));
                        return false;
                    }
                }
                

            }
            Dln.ReadArray(ch, AkSlave[ch], 0x5D, 1, rbuf);
            if (rbuf[0] != STATIC.Read_Temp[ch])
            {
                Log.AddLog(ch, string.Format("0x5D = 0x{0}.", rbuf[0].ToString("X2")));
                return false;
            }
            //Dln.ReadArray(ch, AkSlave[ch], 0xF3, 1, rbuf);
            //if (rbuf[0] != 0x01)
            //{
            //    Log.AddLog(ch, string.Format("0xF3 = 0x{0}.", rbuf[0].ToString("X2")));
            //    return false;
            //}
            //Dln.ReadArray(ch, AkSlave[ch], 0x19, 1, rbuf);
            //if (rbuf[0] != STATIC.ReadData)
            //{
            //    Log.AddLog(ch, string.Format("0x19 = 0x{0}.", rbuf[0].ToString("X2")));
            //    return false;
            //}


            return true;
		}
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
                Dln.WriteArray(ch, AkSlave[ch], 0x03, 1, new byte[1] { val });
                Thread.Sleep(time);
                Dln.ReadArray(ch, AkSlave[ch], 0x4B, 1, rbuf);
                if ((rbuf[0] & 0x04) == 0x00)
                    break;
            }
            if (temp > 4) { Log.AddLog(ch, string.Format("Memory update NG.")); }
        }
        bool AK7316_addr_check(int ch)
        {
			//      byte[] icaddr = new byte[] { 0x18, 0x1E, 0xE8, 0x5A, 0xE4, 0xEC, 0x1A, 0x50, 0x98, 0x68, 0xDA, 0x64, 0x6C, 0x9A, 0xD0 };
			byte[] icaddr = new byte[] { 0x98 };
            byte temp;
            byte addr_temp;
            byte[] rbuf = new byte[1];
            Dln.WriteArray(ch, AkSlave[ch], 0x02, 1, new byte[1] { 0x40 });
            Log.AddLog(ch, string.Format("IRIS Mode OFF."));
            Dln.ReadArray(ch, AkSlave[ch], 0x03, 1, rbuf);
            if (rbuf[0] != 0x00)
            {
                Log.AddLog(ch, string.Format("IC addr/version : 0x{0:X2} / 0x{1:X2}", AkSlave[ch], rbuf[0]));
			
                return true;
            }
            int length = icaddr.Length / sizeof(byte);
            for (temp = 0; temp < length; temp++)
            {
                addr_temp = (byte)(icaddr[temp] >> 1); /// 나중에 확인
                Dln.WriteArray(ch, AkSlave[ch], 0x02, 1, new byte[1] { 0x40 });
                Log.AddLog(ch, string.Format("IRIS Mode OFF."));
                rbuf[0] = 0x00;
                Dln.ReadArray(ch, addr_temp, 0x03, 1, rbuf);

                if (rbuf[0] != 0x00)
                {
                    AkSlave[ch] = addr_temp;
                    Log.AddLog(ch, string.Format("IC addr/version : 0x{0:X2} / 0x{1:X2}", AkSlave[ch], rbuf[0]));
               
                    return true;
                }
                //else Log.AddLog(ch, string.Format("IC addr : 0x{0:X2} => Fail", addr_temp));
            }
			return false;

        }
        void AK7316_IC_data(int ch)
        {
            byte[] rbuf2 = new byte[2];
            short poscal, negcal, posvt, negvt;
            Dln.WriteArray(ch, AkSlave[ch], 0xAE, 1, new byte[1] { 0x3B });
            AK7316_memory_upadate(ch, 5);
            Dln.WriteArray(ch, AkSlave[ch], 0xAE, 1, new byte[1] { 0x00 });
            Dln.ReadArray(ch, AkSlave[ch], 0x04, 1, rbuf2);
            poscal = (short)((rbuf2[0] << 8 + rbuf2[1]) / 2);
            Dln.ReadArray(ch, AkSlave[ch], 0x06, 1, rbuf2);
            negcal = (short)((rbuf2[0] << 8 + rbuf2[1]) / 2);
            Dln.ReadArray(ch, AkSlave[ch], 0xC0, 1, rbuf2);
            posvt = (short)((rbuf2[0] << 8 + rbuf2[1]) / 16);
            Dln.ReadArray(ch, AkSlave[ch], 0xC2, 1, rbuf2);
            negvt = (short)((rbuf2[0] << 8 + rbuf2[1]) / 16);

            AK7316_Check_byte(ch, 0x00, 0x0F);
            AK7316_Check_byte(ch, 0x10, 0x1F);
            AK7316_Check_byte(ch, 0x20, 0x2F);
            AK7316_Check_byte(ch, 0x30, 0x3F);
            AK7316_Check_byte(ch, 0x50, 0x5F);
            AK7316_Check_byte(ch, 0xA0, 0xAF);
            AK7316_Check_byte(ch, 0xB0, 0xBF);
            AK7316_Check_byte(ch, 0xC0, 0xCF);
            AK7316_Check_byte(ch, 0xD0, 0xDF);
            AK7316_Check_byte(ch, 0xE0, 0xEF);
            AK7316_Check_byte(ch, 0xF0, 0xFF);
            Log.AddLog(ch, string.Format("pcal : {0:%5d}, ncal : {1:%5d}", poscal, negcal));
            Log.AddLog(ch, string.Format("pvt : {0:%5d}, nvt : {1:%5d}", posvt, negvt));
        }
        bool AK7316_Address_Change(int ch)
        {
			bool tmpbool = false;
            byte[] icaddr = { 0x18, 0x1E, 0xE8, 0x98 };
            byte icaddr_temp = 0xFF;
            byte[] temp = new byte[1];
            byte[] i2c_1st = new byte[2];
            byte[] i2c_2nd = new byte[2];

            Log.AddLog(ch, string.Format("Slave Addr =  0x{0:X2}", AkSlave[ch]));
            Dln.ReadArray(ch, AkSlave[ch], 0x03, 1, temp);
            if (temp[0] == 0x1C)
            {
                Log.AddLog(ch, string.Format("IC address check OK (change skip)"));
				
				return true;
            }
            for (int i = 0; i < 4; i++)
            {

                Dln.ReadArray(ch, icaddr[i] >> 1, 0x03, 1, temp);
                i2c_2nd[0] = temp[0];
                if (i2c_2nd[0] == 0x1C)
                {
                    icaddr_temp = (byte)(icaddr[i] >> 1);
                    break;
                }
                else
                {
                    Log.AddLog(ch, string.Format("i2c NG"));

                }
            }
            if (icaddr_temp != 0xFF)
            {
                Dln.WriteArray(ch, icaddr_temp, 0x02, 1, new byte[1] { 0x40 });
                Thread.Sleep(5);
                Dln.WriteArray(ch, icaddr_temp, 0xAE, 1, new byte[1] { 0x3B });
                Dln.WriteArray(ch, icaddr_temp, 0x0A, 1, new byte[1] { IC_SETTING_AKM7316[0] });
                AK7316_memory_upadate(ch, 1);
                AK7316_memory_upadate(ch, 5);
                Dln.WriteArray(ch, icaddr_temp, 0xAE, 1, new byte[1] { 0x00 });

                Dln.ReadArray(ch, AkSlave[ch], 0x03, 1, temp);
                if (temp[0] == 0x1C)
                {
                    Log.AddLog(ch, string.Format("I2C Slave Addr Change from 0x{0:X2} to 0x{1:X2}", icaddr_temp, AkSlave[ch]));
					tmpbool = true;
                }
                else
                {
                    Log.AddLog(ch, string.Format(" I2C address change NG(fpga error)"));
					tmpbool = false;
                }

            }
            else
            {
                Log.AddLog(ch, string.Format(" I2C address change NG(check error)"));
				tmpbool = false;
            }
			return tmpbool;
        }
        void AK7316_Check_byte(int ch, ushort Reg_s, ushort Reg_e)
        {
            byte[] rbuf = new byte[1];
            ushort index = 0;
            Log.AddLog(ch, string.Format("0x{0:X2} ~ 0x{1:X2}", Reg_s, Reg_e));
            for (ushort i = Reg_s; i <= Reg_e; i++)
            {
                Dln.ReadArray(ch, AkSlave[ch], (byte)i, 1, rbuf);
                if ((index & 0x0003) == 0x0000)
                {
                    Log.AddLog(ch, string.Format("0x{0:X2}", rbuf[0]));
                }

                index++;
            }
        }
		public override (double PM, double Freq) AK7316_PhaseMargin(int ch, ushort FinalFreq, ushort StartFreq, ushort StepFreq, ushort Amp, ushort GainThr)
		{
			try
			{
				int FPGAAddr = 0x14;
				double gain_temp = 0;

                int frequency_value, freq_temp, freq_PM, old_freq;
                double gain_value = 0, phasemargin_value, phase_temp, pre_pm = 0;
				int[] before_after_zero_freq = new int[2];
				double[] before_after_zero_gain = new double[2];
				byte[] i2cdata = new byte[2];
				byte flag_2nd = 0;
				

				i2cdata[0] = 0x00;
				i2cdata[1] = 0x01;
				Dln.DLNi2c[ch].Write(FPGAAddr, i2cdata);
                i2cdata[0] = 0x00;
                i2cdata[1] = 0x00;
                Dln.DLNi2c[ch].Write(FPGAAddr, i2cdata);
                i2cdata[0] = 0x6F;
                i2cdata[1] = 0x98;
                Dln.DLNi2c[ch].Write(FPGAAddr, i2cdata);
                Log.AddLog(ch, string.Format("Phase Margin test(High Freq Start)"));
				Dln.WriteArray(ch, AkSlave[ch], 0x02, 1, new byte[1] { 0x40 });
				Thread.Sleep(1);
                Dln.WriteArray(ch, AkSlave[ch], 0xAE, 1, new byte[1] { 0x3B });
				Dln.WriteArray(ch, FPGAAddr, 0x56, 1, new byte[1] { 0x80 });
                Dln.WriteArray(ch, FPGAAddr, 0xAC, 1, new byte[1] { 0x01 });
				Thread.Sleep(5);
                Dln.WriteArray(ch, FPGAAddr, 0x54, 1, new byte[1] { 0x0F });
                Dln.WriteArray(ch, FPGAAddr, 0x55, 1, new byte[1] { 0x00 });
                Thread.Sleep(5);
                Dln.WriteArray(ch, FPGAAddr, 0xA8, 1, new byte[1] { 0xC5 });
				Thread.Sleep(1000);
				Dln.Write_2byte(ch, FPGAAddr, 0x52, 1, (ushort)(Amp << 6));


				frequency_value = StartFreq;
				freq_temp = frequency_value * StepFreq / 100;
				frequency_value += freq_temp;
				Dln.Write_2byte(ch, AkSlave[ch], 0x50, 1, (ushort)(frequency_value << 1));
				Thread.Sleep(30000 / frequency_value + 10);
				Thread.Sleep(100);
				gain_temp = Dln.Read_3Byte(ch, AkSlave[ch], 0x94, 1);
				gain_value = Math.Log10(((double)gain_temp) / 65536) * 20;
				phase_temp = (double)Dln.Read_2Byte(ch, AkSlave[ch], 0x98, 1);
				phase_temp /= 128;
				if (phase_temp > 256)
					phase_temp -= 512;
				phasemargin_value = 180 + phase_temp;
				if (phasemargin_value > 180)
					phasemargin_value -= 360;
				if (phasemargin_value < -180)
					phasemargin_value += 360;
                Log.AddLog(ch, string.Format("skip high freq for aging"));
                Log.AddLog(ch, string.Format("Amp {0}, Freq {1}, Gain {2:0.000}, Phase {3:0.000}", Amp, frequency_value, gain_value, phasemargin_value));

                Log.AddLog(ch, string.Format("Amp		Freq		Gain		P/M"));
                Log.AddLog(ch, string.Format("--------------------------------------"));
				for (old_freq = frequency_value = StartFreq; frequency_value >= FinalFreq; frequency_value -= freq_temp)
				{
					Dln.Write_2byte(ch, FPGAAddr, 0x50, 1, (ushort)(frequency_value << 1));
					// Thread.Sleep(30000 / frequency_value + 10);
					Thread.Sleep(1000 / old_freq + 5000 / frequency_value + 10);
                    old_freq = frequency_value;
					gain_temp = Dln.Read_3Byte(ch, FPGAAddr, 0x94, 1);
					gain_value = Math.Log10(((double)(gain_temp)) / 65536) * 20;
					phase_temp = (double)Dln.Read_2Byte(ch, FPGAAddr, 0x98, 1);

					phase_temp /= 128;
					if (phase_temp > 256)
						phase_temp -= 512;
					phasemargin_value = 180 + phase_temp;
					if (phasemargin_value > 180)
						phasemargin_value = 360 - phasemargin_value;
					if (phasemargin_value < -180)
						phasemargin_value += 360;

                    Log.AddLog(ch, string.Format("{0}		{1}		{2:0.000}		{3:0.000}", Amp, frequency_value, gain_value, phasemargin_value));
					if(gain_value > 0)
					{
						before_after_zero_freq[1] = frequency_value;
						before_after_zero_gain[1] = gain_value;
					}
					else
					{
						before_after_zero_freq[0] = frequency_value;
						before_after_zero_gain[0] = gain_value;
					}
					if((Math.Abs(gain_value) < 3) &&(flag_2nd != 1))
					{
						Pm_2nd[ch] = phasemargin_value;
						flag_2nd = 1;
					}
					pre_pm = phasemargin_value;
					freq_temp = frequency_value * StepFreq / 100;
					if (freq_temp < 1)
						freq_temp = 1;
                }
        
                Log.AddLog(ch, string.Format("--------------------------------------"));
                Log.AddLog(ch, string.Format("zero Freq before = {0},{1:0.000}", before_after_zero_freq[0], before_after_zero_gain[0]));
                Log.AddLog(ch, string.Format("zero Freq after = {0},{1:0.000}", before_after_zero_freq[1], before_after_zero_gain[1]));

				if(frequency_value == StartFreq)
				{
                    Log.AddLog(ch, string.Format("Error type1 : Gain over zero at 1st cycle1"));
                    Dln.WriteArray(ch, FPGAAddr, 0xA8, 1, new byte[1] { 0x00 });
                    Dln.WriteArray(ch, FPGAAddr, 0xAF, 1, new byte[1] { 0xEE });
                    Thread.Sleep(1);
                    Dln.WriteArray(ch, FPGAAddr, 0xAC, 1, new byte[1] { 0x00 });
                    Thread.Sleep(15);

                    Dln.WriteArray(ch, AkSlave[ch], 0xAE, 1, new byte[1] { 0x00 });
                    IRIS_IC_Init(ch);
                    FPGA_I2C_Clear(ch);
                    return (1, 1);
                }
				if((frequency_value <= FinalFreq) && (gain_value <= 0))
				{
                    Log.AddLog(ch, string.Format("Error type4 : No cross over point during period"));
                    Dln.WriteArray(ch, FPGAAddr, 0xA8, 1, new byte[1] { 0x00 });
                    Dln.WriteArray(ch, FPGAAddr, 0xAF, 1, new byte[1] { 0xEE });
                    Thread.Sleep(1);
                    Dln.WriteArray(ch, FPGAAddr, 0xAC, 1, new byte[1] { 0x00 });
                    Thread.Sleep(15);

                    Dln.WriteArray(ch, AkSlave[ch], 0xAE, 1, new byte[1] { 0x00 });
                    IRIS_IC_Init(ch);
                    FPGA_I2C_Clear(ch);
                    return (4, 4);
                }
				freq_PM = (int)(((before_after_zero_gain[1] * before_after_zero_freq[0]) - (before_after_zero_gain[0] * before_after_zero_freq[1])) / (before_after_zero_gain[1] - before_after_zero_gain[0]));
				Dln.Write_2byte(ch, AkSlave[ch], 0x50, 1, (ushort)(freq_PM << 1));
				Thread.Sleep(1000 / old_freq + 5000 / frequency_value + 10);
				old_freq = frequency_value;

				gain_temp = Dln.Read_3Byte(ch, AkSlave[ch], 0x94, 1);
				gain_value = Math.Log10(((double)(gain_temp)) / 65536) * 20;
				phase_temp = (double)(Dln.Read_2Byte(ch, AkSlave[ch], 0x98, 1));

				if(Math.Abs(gain_value - before_after_zero_gain[1]) > GainThr)
				{
                    Log.AddLog(ch, string.Format("Error type 2: gain is changed drastically over {0}", GainThr));
					Dln.WriteArray(ch, FPGAAddr, 0xA8, 1, new byte[1] { 0x00 });
					Dln.WriteArray(ch, FPGAAddr, 0xAF, 1, new byte[1] { 0xEE });
					Thread.Sleep(1);
					Dln.WriteArray(ch, FPGAAddr, 0xAC, 1, new byte[1] { 0x00 });
					Thread.Sleep(15);

					Dln.WriteArray(ch, AkSlave[ch], 0xAE, 1, new byte[1] { 0x00 });
					IRIS_IC_Init(ch);
					FPGA_I2C_Clear(ch);
					return (2, 2);
                }

				phase_temp /= 128;
				if(phase_temp > 256)
                    phase_temp -= 512;
				phasemargin_value = 180 + phase_temp;
				if (phasemargin_value > 180)
					phasemargin_value -= 360;
				if (phasemargin_value < -180)
					phasemargin_value += 360;


                Log.AddLog(ch, string.Format("Use Linear Interpolation"));
                Log.AddLog(ch, string.Format("--------------------------------------"));
                Log.AddLog(ch, string.Format("Amp {0}, Freq {1}, Gain {2:0.000}, P/M {3:0.000}", Amp, freq_PM, gain_value, phasemargin_value));
                Log.AddLog(ch, string.Format("--------------------------------------"));

                Log.AddLog(ch, string.Format("Phase at -4dB {0:0.000}", Pm_2nd[ch]));

				Dln.WriteArray(ch, FPGAAddr, 0xA8, 1, new byte[1] { 0x00 });
                Dln.WriteArray(ch, FPGAAddr, 0xAF, 1, new byte[1] { 0xEE });
				Thread.Sleep(1);
                Dln.WriteArray(ch, FPGAAddr, 0xAF, 1, new byte[1] { 0x00 });
				Thread.Sleep(15);
				Dln.WriteArray(ch, AkSlave[ch], 0xAE, 1, new byte[1] { 0x00 });
				IRIS_IC_Init(ch);
				FPGA_I2C_Clear(ch);

				return (phasemargin_value, freq_PM) ;
			}
			catch
			{
				return (-1, -1);
			}
		}
		void FPGA_I2C_Clear(int ch)
		{

		}

        public override bool ReadPID(string path)
        {
            try
            {
                string textVal = System.IO.File.ReadAllText(path);
                string[] t = textVal.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                IC_DATA_AKM7316 = new byte[(t.Length - 1) * 2];
                for (int i = 0; i < t.Length; i++)
                {
                    if (i == 0)
                    {
                        string[] b = t[i].Split(new string[] { ",", " ", "\t", "//", "Reg" }, StringSplitOptions.RemoveEmptyEntries);
                        IC_SETTING_AKM7316 = new byte[b.Length / 2];
                        IC_SETTING_AKM7316_Reg = new byte[b.Length / 2];
                        for (int j = 0; j < b.Length; j++)
                        {
							if (j < b.Length / 2)
								IC_SETTING_AKM7316[j] = Convert.ToByte(b[j], 16);
							else
								IC_SETTING_AKM7316_Reg[j - b.Length / 2] = Convert.ToByte(b[j], 16);

                        }
                    }
                    else
                    {
                        string[] b = t[i].Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
                        bool setNext = false;
                        for (int j = 0; j < 2; j++)
                        {

                            IC_DATA_AKM7316[(i - 1) * 2 + j] = Convert.ToByte(b[j], 16);
                            //if (setNext)
                            //{
                            //    Addr19Val = IC_DATA_AKM7316[(i - 1) * 2 + j];
                            //    setNext = false;
                            //}
                            //if (IC_DATA_AKM7316[(i - 1) * 2 + j] == 0x19)
                            //    setNext = true;

                        }


                    }
                }
                return true;
            }
            catch
            {
                return false;
            }


        }

    }
}