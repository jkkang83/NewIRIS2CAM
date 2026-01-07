
using OpenCvSharp;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp.Extensions;
using System.Diagnostics;


namespace M1Wide
{
    public partial class F_Vision : Form
    {
        public Camera Cam { get { return STATIC.Camera; } }
        public InspectionApi[] InspApi { get { return STATIC.InspectionApi; } }
        public Process Process { get { return STATIC.Process; } }
        public DriverIC DrvIC { get { return STATIC.DrvIC; } }
        public DLN Dln { get { return STATIC.Dln; } }
        public VisionSettingFile VisionFile { get { return STATIC.Rcp.VisionFile; } }
        public DecenterScale Dscale { get { return STATIC.Rcp.Dscale; } }

        public bool[] CStatus = new bool[2] { false, false };

        public F_Vision()
        {
            InitializeComponent();
        }

        private void BackBtn_Click(object sender, System.EventArgs e)
        {
            for (int i = 0; i < Cam.CamList.Count; i++)
            {
                Cam.CamList[i].Live(false);
            }
            Process.LEDs_All_On(false);

            STATIC.State = (int)STATIC.STATE.Manage;
        }

        private void F_Vision_Load(object sender, System.EventArgs e)
        {
          

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(100);
                    if (LoadUnloadCover.InvokeRequired)
                    {
                        LoadUnloadCover.BeginInvoke((MethodInvoker)delegate
                        {
                            if (Dln.IsCoverUp) LoadUnloadCover.ForeColor = Color.White;
                            else LoadUnloadCover.ForeColor = Color.Red;
                        });
                    }
                    else
                    {
                        if (Dln.IsCoverUp) LoadUnloadCover.ForeColor = Color.White;
                        else LoadUnloadCover.ForeColor = Color.Red;
                    }

                    if (LoadUnloadSocket.InvokeRequired)
                    {
                        LoadUnloadSocket.BeginInvoke((MethodInvoker)delegate
                        {
                            if (Dln.IsLoad) LoadUnloadSocket.ForeColor = Color.Red;
                            else LoadUnloadSocket.ForeColor = Color.White;
                        });
                    }
                    else
                    {
                        if (Dln.IsLoad) LoadUnloadSocket.ForeColor = Color.Red;
                        else LoadUnloadSocket.ForeColor = Color.White;
                    }


                    if (LoadUnloadAll.InvokeRequired)
                    {
                        LoadUnloadAll.BeginInvoke((MethodInvoker)delegate
                        {
                            if (Dln.IsLoad && !Dln.IsCoverUp) LoadUnloadAll.ForeColor = Color.Red;
                            else LoadUnloadAll.ForeColor = Color.White;
                        });
                    }
                    else
                    {
                        if (Dln.IsLoad && !Dln.IsCoverUp) LoadUnloadAll.ForeColor = Color.Red;
                        else LoadUnloadAll.ForeColor = Color.White;
                    }
                    if (CStatus[0]) CStatus0.BackColor = Color.Blue;
                    else CStatus0.BackColor = Color.LightGray;
                    if (CStatus[1]) CStatus1.BackColor = Color.Blue;
                    else CStatus1.BackColor = Color.LightGray;
                }
            });
            for (int i = 0; i < Cam.CamList.Count; i++)
            {
                if (Cam.CamList[i].isOpen)
                    Cam.CamList[i].OnNewFrame += F_Vision_OnNewFrame;
            }
        }

        private void F_Vision_OnNewFrame(int camIndex, Mat newFrame)
        {
            if (camIndex == 0)
            {
                STATIC.InspMat[0] = newFrame.Clone();
                // Cv2.CvtColor(STATIC.InspMat[0], STATIC.InspMat[0], ColorConversionCodes.BGR2GRAY);

                if (STATIC.State == (int)STATIC.STATE.Vision)
                {
                    pictureBox1.Image = newFrame.ToBitmap();
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }


            }
            else
            {
                STATIC.InspMat[1] = newFrame.Clone();
                //    Cv2.CvtColor(STATIC.InspMat[1], STATIC.InspMat[1], ColorConversionCodes.BGR2GRAY);
                if (STATIC.State == (int)STATIC.STATE.Vision)
                {
                    pictureBox2.Image = newFrame.ToBitmap();
                    pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                }

            }
        }

        private void LoadBMP_Click(object sender, System.EventArgs e)
        {

            pictureBox1.Image = null;
            string sFilePath = "\\RawData\\";
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = "bmp";
            openFile.InitialDirectory = sFilePath;
            openFile.Multiselect = true;
            openFile.Filter = "BMP(*.bmp)|*.bmp";
            if (openFile.ShowDialog() != DialogResult.OK)
                return;
            STATIC.InspMat[0] = new Mat(openFile.FileName, ImreadModes.Grayscale);
            pictureBox1.Image = STATIC.InspMat[0].Clone().ToBitmap();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void btnLive2_Click(object sender, System.EventArgs e)
        {
            Process.LEDs_All_On(true);
            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Live(true);
               
            });
      
           
        }

        private void btnGrab2_Click(object sender, System.EventArgs e)
        {
            Process.LEDs_All_On(true);
            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Live(false);

            });
            Thread.Sleep(200);

            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Acquire();

            });

           // Process.LEDs_All_On(false);
        }

        private void btnHalt2_Click(object sender, System.EventArgs e)
        {
            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Live(false);

            });
            Thread.Sleep(100);
            pictureBox1.Image = null;
            pictureBox2.Image = null;
         //   Process.LEDs_All_On(false);
        }

        private void btnClear1_Click(object sender, System.EventArgs e)
        {

            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Live(false);

            });
            Thread.Sleep(100);
            pictureBox1.Image = null;
            pictureBox2.Image = null;
          //  Process.LEDs_All_On(false);

            tbScanLog.Text = "";
        }

        private void btnFOV_Click(object sender, System.EventArgs e)
        {
           

            Button b = (Button)sender;
            int ch = 0;
            if (rdFOVL.Checked)
                ch = 0;
            else ch = 1;
            Cam.CamList[ch].Live(false);
            Process.LEDs_All_On(true);
            switch (b.Name)
            {
                case "btnFOVup":
                    if(STATIC.DefaultHeight + Cam.CamList[ch].CamInfo.OffsetY + 4 <= STATIC.CamDefaultHeight)
                    {
                        Cam.CamList[ch].CamInfo.OffsetY += 4;
                        Cam.CamList[ch].OffsetY = Cam.CamList[ch].CamInfo.OffsetY;
                    }
                  
                    break;
                case "btnFOVdown":
                    if (Cam.CamList[ch].CamInfo.OffsetY - 4 >= 0)
                    {
                        Cam.CamList[ch].CamInfo.OffsetY -= 4;
                        Cam.CamList[ch].OffsetY = Cam.CamList[ch].CamInfo.OffsetY;
                    }
                   
                    break;
                case "btnFOVLeft":
                    if (STATIC.DefaultWidth + Cam.CamList[ch].CamInfo.OffsetX + 4 <= STATIC.CamDefaultWidth)
                    {
                        Cam.CamList[ch].CamInfo.OffsetX += 4;
                        Cam.CamList[ch].OffsetX = Cam.CamList[ch].CamInfo.OffsetX;
                    }
                   
                    break;
                case "btnFOVRight":
                    if (Cam.CamList[ch].CamInfo.OffsetX - 4 >= 0)
                    {
                        Cam.CamList[ch].CamInfo.OffsetX -= 4;
                        Cam.CamList[ch].OffsetX = Cam.CamList[ch].CamInfo.OffsetX;
                    }
                  
                    break;
            }
            Cam.SaveXml();
            
            Cam.CamList[ch].Acquire();

        }

       

    

        private void AllLEDOff_Click(object sender, System.EventArgs e)
        {
            Process.LEDs_All_On(false);
        }

        private void btnLEDUp_Click(object sender, System.EventArgs e)
        {
            if (rbLiTop.Checked)
            {
                if (VisionFile.TopInnerLED_L < 4)
                    VisionFile.TopInnerLED_L += 0.01;
                tbLEDvoltage.Text = VisionFile.TopInnerLED_L.ToString("F2");
                Process.Drive_LEDs(0, VisionFile.TopInnerLED_L);
            }
            else if(rbLOTop.Checked)
            {
                if (VisionFile.TopOuterLED_L < 4)
                    VisionFile.TopOuterLED_L += 0.01;
                tbLEDvoltage.Text = VisionFile.TopOuterLED_L.ToString("F2");
                if (STATIC.Rcp.Option.isPosture && STATIC.Rcp.Option.is1CH_MC) Process.Drive_LEDs(1, VisionFile.TopOuterLED_L);

            }
            else if (rbLBtm.Checked)
            {
                if (VisionFile.BtmLED_L < 4)
                    VisionFile.BtmLED_L += 0.01;
                tbLEDvoltage.Text = VisionFile.BtmLED_L.ToString("F2");
                if (STATIC.Rcp.Option.isPosture && STATIC.Rcp.Option.is1CH_MC)
                    Process.Drive_LEDs_BTM_Posture(0, VisionFile.BtmLED_L);
                else
                    Process.Drive_LEDs(1, VisionFile.BtmLED_L);
            }
            else if (rbRTop.Checked)
            {
                if (VisionFile.TopInnerLED_R < 4)
                    VisionFile.TopInnerLED_R += 0.01;
                tbLEDvoltage.Text = VisionFile.TopInnerLED_R.ToString("F2");
                Process.Drive_LEDs(2, VisionFile.TopInnerLED_R);
            }
            else
            {
                if (VisionFile.BtmLED_R < 4)
                    VisionFile.BtmLED_R += 0.01;
                tbLEDvoltage.Text = VisionFile.BtmLED_R.ToString("F2");
                Process.Drive_LEDs(3, VisionFile.BtmLED_R);

            }

 
            VisionFile.Save();
        }

        private void btnLEDDown_Click(object sender, System.EventArgs e)
        {
            if (rbLiTop.Checked)
            {
                if (VisionFile.TopInnerLED_L > 0)
                    VisionFile.TopInnerLED_L -= 0.01;
                tbLEDvoltage.Text = VisionFile.TopInnerLED_L.ToString("F2");
                Process.Drive_LEDs(0, VisionFile.TopInnerLED_L);
            }
            else if(rbLOTop.Checked)
            {
                if (VisionFile.TopOuterLED_L > 0)
                    VisionFile.TopOuterLED_L -= 0.01;
                tbLEDvoltage.Text = VisionFile.TopOuterLED_L.ToString("F2");
                if (STATIC.Rcp.Option.isPosture && STATIC.Rcp.Option.is1CH_MC) Process.Drive_LEDs(1, VisionFile.TopOuterLED_L);
            }
            else if (rbLBtm.Checked)
            {
                if (VisionFile.BtmLED_L > 0)
                    VisionFile.BtmLED_L -= 0.01;
                tbLEDvoltage.Text = VisionFile.BtmLED_L.ToString("F2");
                if (STATIC.Rcp.Option.isPosture && STATIC.Rcp.Option.is1CH_MC)
                    Process.Drive_LEDs_BTM_Posture(0, VisionFile.BtmLED_L);
                else
                    Process.Drive_LEDs(1, VisionFile.BtmLED_L);
            }
            else if (rbRTop.Checked)
            {
                if (VisionFile.TopInnerLED_R > 0)
                    VisionFile.TopInnerLED_R -= 0.01;
                tbLEDvoltage.Text = VisionFile.TopInnerLED_R.ToString("F2");
                Process.Drive_LEDs(2, VisionFile.TopInnerLED_R);
            }
            else
            {
                if (VisionFile.BtmLED_R > 0)
                    VisionFile.BtmLED_R -= 0.01;
                tbLEDvoltage.Text = VisionFile.BtmLED_R.ToString("F2");
                Process.Drive_LEDs(3, VisionFile.BtmLED_R);
            }

            VisionFile.Save();
        }
        bool TopLEDState = false;
        private void btnTopLED_OnOff_Click(object sender, System.EventArgs e)
        {
            TopLEDState = !TopLEDState;
            Process.TOPLED_OnOff(TopLEDState);

        }

      

        private void btnLongExposure_Click(object sender, System.EventArgs e)
        {

            int[] exposure = new int[2] { Convert.ToInt32(tbExposure.Text), Convert.ToInt32(tbExposureR.Text) };
            for (int i = 0; i < Cam.CamList.Count; i++)
            {
                Cam.CamList[i].CamInfo.ExpTime = exposure[i];
                Cam.CamList[i].ExposureTime = exposure[i];
                
            }
            Cam.SaveXml();

        }

        private void LoadSocket_Click(object sender, System.EventArgs e)
        {
            if (!Dln.IsLoad)
                Dln.LoadSocket(true);
            else
                Dln.LoadSocket(false);
        }

    

        private void btnFindDavinciSmall_Click(object sender, System.EventArgs e)
        {

            DateTime dtNow = DateTime.Now;
            string sFilePath = STATIC.BaseDir + "\\SaveImg\\" + dtNow.ToString("_hhmmsss") + ".bmp";
            if(rdFOVL.Checked)
                STATIC.InspMat[0].SaveImage(sFilePath);
            else
                STATIC.InspMat[1].SaveImage(sFilePath);
        }

   
        private void CheckArea_Click(object sender, EventArgs e)
        {
            try
            {
                double[] Area = new double[2];
                Process.LEDs_All_On(true);
                Parallel.For(0, Cam.CamList.Count, i =>
                {
                    Cam.CamList[i].Live(false);

                });
                Thread.Sleep(500);
                Parallel.For(0, Cam.CamList.Count, i =>
                {
                    Cam.CamList[i].Acquire();
                    Area[i] = InspApi[i].JustCheckArea(i, STATIC.InspMat[i].Clone(), true);

                });
                tbScanLog.Text += string.Format("AL\t{0:0.000}\tAR\t{1:0.000}\r\n", Area[0], Area[1]);
                pictureBox1.Image = STATIC.ResMat[0].Clone().ToBitmap();
                if (Cam.CamList.Count > 1)
                    pictureBox2.Image = STATIC.ResMat[1].Clone().ToBitmap();

            }
            catch
            { 
                MessageBox.Show("Error"); 
            }
          //  Process.LEDs_All_On(false);


        }

        private void FindCover_Click(object sender, EventArgs e)
        {
            InspResult[] res = new InspResult[2] { new InspResult(), new InspResult() };
            int camCnt = Cam.CamList.Count;
            if (IsLoadImage.Checked) camCnt = 1;
            Process.LEDs_All_On(true);
            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Live(false);

            });       
            Thread.Sleep(200);

            Parallel.For(0, camCnt, i =>
            {
                if (!IsLoadImage.Checked) Cam.CamList[i].Acquire();
                InspApi[i].NewFineCOG(i, 0, STATIC.InspMat[i].Clone(), InspectionType.FindCover, res[i], true, false, true);

            });
            tbScanLog.Text += string.Format("Center X \r\nLeft > {0:0.000}\tRight > {1:0.000}\r\n", res[0].Cover_cx, res[1].Cover_cx);
            tbScanLog.Text += string.Format("Center Y \r\nLeft > {0:0.000}\tRight > {1:0.000}\r\n", res[0].Cover_cy, res[1].Cover_cy);
            pictureBox1.Image = STATIC.ResMat[0].Clone().ToBitmap();
            if (camCnt > 1)
                pictureBox2.Image = STATIC.ResMat[1].Clone().ToBitmap();
         //   Process.LEDs_All_On(false);
         
        }

        private void FindOpenIris_Click(object sender, EventArgs e)
        {
            InspResult[] res = new InspResult[2] { new InspResult(), new InspResult() };
            int camCnt = Cam.CamList.Count;
            if (IsLoadImage.Checked) camCnt = 1;
            Process.LEDs_All_On(true);
            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Live(false);

            });
            Thread.Sleep(200);
            Parallel.For(0, camCnt, i =>
            {
                if (!IsLoadImage.Checked) Cam.CamList[i].Acquire();
                if (STATIC.Rcp.Option.AreaDecenterUse)
                    InspApi[i].NewFineCOG(i, 0,STATIC.InspMat[i].Clone(), InspectionType.Area_Center, res[i], true, false, true);
                else InspApi[i].NewFineCOG(i, 0, STATIC.InspMat[i].Clone(), InspectionType.InCircle_Center, res[i], true, false, true);

            });
            tbScanLog.Text += string.Format("Center X \r\nLeft > {0:0.000}\tRight > {1:0.000}\r\n", res[0].cx, res[1].cx);
            tbScanLog.Text += string.Format("Center Y \r\nLeft > {0:0.000}\tRight > {1:0.000}\r\n", res[0].cy, res[1].cy);
            pictureBox1.Image = STATIC.ResMat[0].Clone().ToBitmap();
            if (camCnt > 1)
                pictureBox2.Image = STATIC.ResMat[1].Clone().ToBitmap();
         //   Process.LEDs_All_On(false);


        }

        private void FindSmallIris_Click(object sender, EventArgs e)
        {
            //Process.LEDs_All_On(true);
            //Thread.Sleep(200);

            //for (int i = 0; i < Vision.CamList.Count; i++)
            //{
            //    Vision.MilList[i].GrabA();

            //    ScanResult result = new ScanResult();
            //    Vision.MilList[i].FineCOG(0, "SmallIris", result, false, true);
            //    tbScanLog.Text += string.Format("Close intruDist\r\nLeft > {0:0.000}\tRight > {1:0.000}\r\n", result.dist[0], result.dist[1]);
            //}
            //Process.LEDs_All_On(false);
        }

        private void OpenDecenter_Click(object sender, EventArgs e)
        {

            InspResult[] res = new InspResult[2] { new InspResult(), new InspResult() };
            int camCnt = Cam.CamList.Count;
            if (IsLoadImage.Checked) camCnt = 1;
            Process.LEDs_All_On(true);
            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Live(false);

            });
            Thread.Sleep(200);
            Parallel.For(0, camCnt, i =>
            {
                if (!IsLoadImage.Checked) Cam.CamList[i].Acquire();
                if (STATIC.Rcp.Option.AreaDecenterUse)
                    InspApi[i].NewFineCOG(i, 0, STATIC.InspMat[i].Clone(), InspectionType.Area_Decenter, res[i], true, false, true);
                else InspApi[i].NewFineCOG(i, 0, STATIC.InspMat[i].Clone(), InspectionType.InCircle_Decenter, res[i], true, false, true);

            });

            double LX = STATIC.ScaleResolution[0] * 1000 * (res[0].Cover_cx - res[0].cx) * Dscale.DecenterScale_L;    //  Left Decenter_X
            double LY = STATIC.ScaleResolution[0] * 1000 * (res[0].Cover_cy - res[0].cy) * Dscale.DecenterScale_L;    //  Left Decenter_Y
            double RX = STATIC.ScaleResolution[1] * 1000 * (res[1].Cover_cx - res[1].cx) * Dscale.DecenterScale_R;    //  Right Decenter_X
            double RY = STATIC.ScaleResolution[1] * 1000 * (res[1].Cover_cy - res[1].cy) * Dscale.DecenterScale_R;    //  Right Decenter_X

            tbScanLog.Text += string.Format("Left > Decenter_X : \t{0:0.000}  \tDecenter_Y : \t{1:0.000}\r\n", LX, LY);
            tbScanLog.Text += string.Format("nRight > Decenter_X : \t{0:0.000}  \tDecenter_Y : \t{1:0.000}\r\n", RX, RY);
            pictureBox1.Image = STATIC.ResMat[0].Clone().ToBitmap();
            if (camCnt > 1)
                pictureBox2.Image = STATIC.ResMat[1].Clone().ToBitmap();
          //  Process.LEDs_All_On(false);

        }

        private void CloseDecenter_Click(object sender, EventArgs e)
        {

            InspResult[] res = new InspResult[2] { new InspResult(), new InspResult() };
            int camCnt = Cam.CamList.Count;
            if (IsLoadImage.Checked) camCnt = 1;
            Process.LEDs_All_On(true);
            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Live(false);

            });
            Thread.Sleep(200);
            Parallel.For(0, camCnt, i =>
            {
                if (!IsLoadImage.Checked) Cam.CamList[i].Acquire();
                InspApi[i].TestCDll2(i, STATIC.InspMat[i].Clone(), false, 3, res[i], false);

            });
            double LX = 1000 * (res[0].Cover_cx - res[0].cx);    //  Left Decenter_X
            double LY = 1000 * (res[0].Cover_cy - res[0].cy);    //  Left Decenter_Y
            double RX = 1000 * (res[1].Cover_cx - res[1].cx);    //  Right Decenter_X
            double RY = 1000 * (res[1].Cover_cy - res[1].cy);    //  Right Decenter_X
            double CircleAccL = res[0].CCircleAcuraccy * 1000;
            double CircleAccR = res[1].CCircleAcuraccy * 1000;
            double ShapeAccL = res[0].CShapeAccuracy * 1000;
            double ShapeAccR = res[1].CShapeAccuracy * 1000;


            tbScanLog.Text += string.Format("AL\t{0:0.000}\tAR\t{1:0.000}\r\n", res[0].CArea, res[1].CArea);
            tbScanLog.Text += string.Format("Left > Decenter_X : \t{0:0.000}  \tDecenter_Y : \t{1:0.000}\r\n", LX, LY);
            tbScanLog.Text += string.Format("nRight > Decenter_X : \t{0:0.000}  \tDecenter_Y : \t{1:0.000}\r\n", RX, RY);
            tbScanLog.Text += string.Format("Left > Circle Accuracy : \t{0:0.000}  \tShape Accuracy : \t{1:0.000}\r\n", CircleAccL, ShapeAccL);
            tbScanLog.Text += string.Format("nRight > Circle Accuracy : \t{0:0.000}  \tShape Accuracy : \t{1:0.000}\r\n", CircleAccR, ShapeAccR);
         //   Process.LEDs_All_On(false);

        }

        private void IRIS_Init_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Process.ChannelCnt; i++)
                DrvIC.IRIS_IC_Init(i);

            tbScanLog.Text += "Init complete\r\n";
        } 

        private void IRIS_Cal_Click(object sender, EventArgs e)
        {

            DrvIC.ReadPID(STATIC.Rcp.Current.PIDName);
            for (int i = 0; i < Process.ChannelCnt; i++)
                DrvIC.IRIS_Adjustment_byMode(i, 0);
            tbScanLog.Text += "Cal complete\r\n";
        }

        private void IRISOpen_Click(object sender, EventArgs e)
        {
            
            int[] chHall = new int[2] { -999, -999 };

            for (int i = 0; i < Process.ChannelCnt; i++)
            {
                if (STATIC.Rcp.Option.ReverseDrv)
                    DrvIC.Move(i, DriverIC.AK, 0);
                else
                    DrvIC.Move(i, DriverIC.AK, 4095);
                Thread.Sleep(100);
                chHall[i] = DrvIC.ReadHall(i, DriverIC.AK);
            }
            tbScanLog.Text += "Read Hall : CH1 = " + chHall[0] + "\t CH2 = " + chHall[1] + "\r\n";

        }

        private void IRISClose_Click(object sender, EventArgs e)
        {

            int[] chHall = new int[2] { -999, -999 };

            for (int i = 0; i < Process.ChannelCnt; i++)
            {
                if (STATIC.Rcp.Option.ReverseDrv)
                    DrvIC.Move(i, DriverIC.AK, 4095);
                else
                    DrvIC.Move(i, DriverIC.AK, 0);
                Thread.Sleep(100);
                chHall[i] = DrvIC.ReadHall(i, DriverIC.AK);
            }
            tbScanLog.Text += "Read Hall : CH1 = " + chHall[0] + "\t CH2 = " + chHall[1] + "\r\n";
        }

        private void IRISCodeOpen_Click(object sender, EventArgs e)
        {
            int a = Convert.ToInt32(tbIrisCode.Text);
            int[] chHall = new int[2] { -999, -999 };

            for (int i = 0; i < Process.ChannelCnt; i++)
            {
                DrvIC.Move(i, DriverIC.AK, a);
                Thread.Sleep(100);
                chHall[i] = DrvIC.ReadHall(i, DriverIC.AK);
            }
            tbScanLog.Text += "Read Hall : CH1 = " + chHall[0] + "\t CH2 = " + chHall[1] + "\r\n";

        }

        private void AF_Cal_Click(object sender, EventArgs e)
        {
         
            DrvIC.Dln.WriteArray(0, 0x4C, 0x02, 1, new byte[1] { 0x40 });
            DrvIC.Dln.WriteArray(0, 0x4C, 0xAE, 1, new byte[1] { 0x3B });
          

            // Process.AddHeadResult("s");
        }

        private void OIS_Cal_Click(object sender, EventArgs e)
        {
        }

        private void ReadHall_Click(object sender, EventArgs e)
        {
        }
     

        private void button4_Click(object sender, EventArgs e)
        {
            double[] Area = new double[2];

            Process.LEDs_All_On(true);
            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Live(false);

            });
           
            tbIrisCode.Text = ((Convert.ToInt32(tbIrisCode.Text) + Convert.ToInt32(textBox1.Text))).ToString();
            if ((Convert.ToInt32(tbIrisCode.Text)) > 4095)
                tbIrisCode.Text = "4095";
            int a = Convert.ToInt32(tbIrisCode.Text);
            DrvIC.Move(0, DriverIC.AK, a);
            DrvIC.Move(1, DriverIC.AK, a);
            Thread.Sleep(300);
            int ReadHallCh1 = DrvIC.ReadHall(0, DriverIC.AK);
            int ReadHallCh2 = DrvIC.ReadHall(1, DriverIC.AK);
            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Acquire();
                Area[i] = InspApi[i].JustCheckArea(i, STATIC.InspMat[i].Clone(), true);

            });
            pictureBox1.Image = STATIC.ResMat[0].Clone().ToBitmap();
            if (Cam.CamList.Count > 1)
                pictureBox2.Image = STATIC.ResMat[1].Clone().ToBitmap();
            tbScanLog.Text += string.Format("AL  Hall {0}  Area {1:0.000}  AR  Hall {2}  Area {3:0.000}\r\n", ReadHallCh1, Area[0], ReadHallCh2, Area[1]);
          //  Process.LEDs_All_On(false);
        }

        private void button5_Click(object sender, EventArgs e)
        {

            double[] Area = new double[2];

            Process.LEDs_All_On(true);
            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Live(false);

            });

            tbIrisCode.Text = ((Convert.ToInt32(tbIrisCode.Text) - Convert.ToInt32(textBox1.Text))).ToString();
            if ((Convert.ToInt32(tbIrisCode.Text)) <= 0)
                tbIrisCode.Text = "0";
            int a = Convert.ToInt32(tbIrisCode.Text);
            DrvIC.Move(0, DriverIC.AK, a);
            DrvIC.Move(1, DriverIC.AK, a);
            Thread.Sleep(300);
            int ReadHallCh1 = DrvIC.ReadHall(0, DriverIC.AK);
            int ReadHallCh2 = DrvIC.ReadHall(1, DriverIC.AK);
            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Acquire();
                Area[i] = InspApi[i].JustCheckArea(i, STATIC.InspMat[i].Clone(), true);

            });
            pictureBox1.Image = STATIC.ResMat[0].Clone().ToBitmap();
            if (Cam.CamList.Count > 1)
                pictureBox2.Image = STATIC.ResMat[1].Clone().ToBitmap();
            tbScanLog.Text += string.Format("AL  Hall {0}  Area {1:0.000}  AR  Hall {2}  Area {3:0.000}\r\n", ReadHallCh1, Area[0], ReadHallCh2, Area[1]);
         //   Process.LEDs_All_On(false);

        }

        private void btrnAllLedOn_Click(object sender, EventArgs e)
        {
            Process.LEDs_All_On(true);
        }

        private void btnFindOpenHexagon_Click(object sender, EventArgs e)
        {
            Process.AK7316_F14OpenLoop(0, false, false);
            //InspectionApi tmp = new InspectionApi();
            //tmp.TestCDll_test(0, STATIC.InspMat[0].Clone());
           // Process.LEDs_All_On(true);
           // Thread.Sleep(200);
           // for (int i = 0; i < Vision.CamList.Count; i++)
           // {
           //     if (!IsLoadImage.Checked) Vision.MilList[i].GrabA();

           //     ScanResult result = new ScanResult();
           //     ScanResult result2 = new ScanResult();
           //     Vision.MilList[i].FineCOG(0, "FindCover", result2, false, true);
           //     Vision.MilList[i].FineCOG(0, "OpenHexagon", result, false, true, 0.0, true);
           //     tbScanLog.Text += string.Format("IntruDist\tLeft > {0:0.000}um\tRight > {1:0.000}um\r\n", result.dist[0], result.dist[1]);
           ////     tbScanLog.Text += string.Format("Area : \tLeft > {0:0.000}mm" + "\u00B2" + "\tRight > {1:0.000}mm" + "\u00B2" + "\r\n", result.area[0][0], result.area[0][1]);
           //     double LX = STATIC.ScaleResolution[0] * 1000 * (result2.cx[0][0] - result.cx[0][0]) * Dscale.DecenterScale_L;    //  Left Decenter_X
           //     double LY = STATIC.ScaleResolution[0] * 1000 * (result2.cy[0][0] - result.cy[0][0]) * Dscale.DecenterScale_L;    //  Left Decenter_Y
           //     double RX = STATIC.ScaleResolution[1] * 1000 * (result2.cx[0][1] - result.cx[0][1]) * Dscale.DecenterScale_R;    //  Right Decenter_X
           //     double RY = STATIC.ScaleResolution[1] * 1000 * (result2.cy[0][1] - result.cy[0][1]) * Dscale.DecenterScale_R;    //  Right Decenter_X

           //     tbScanLog.Text += string.Format("Left > Decenter_X : {0:0.000}  Decenter_Y : {1:0.000}\r\n", LX, LY);
           //     tbScanLog.Text += string.Format("nRight > Decenter_X : {0:0.000}  Decenter_Y : {1:0.000}\r\n", RX, RY);
           // }
           // Process.LEDs_All_On(false);
        }

      

       

        private void CArea_Click(object sender, EventArgs e)
        {

            InspResult[] res = new InspResult[2] { new InspResult(), new InspResult() };
            int camCnt = Cam.CamList.Count;
            if (IsLoadImage.Checked) camCnt = 1;
            Process.LEDs_All_On(true);
            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Live(false);

            });
            Thread.Sleep(500);
            Parallel.For(0, camCnt, i =>
            {
                if (!IsLoadImage.Checked) Cam.CamList[i].Acquire();
                InspApi[i].TestCDll2(i, STATIC.InspMat[i].Clone(), false, 1, res[i], false);

            });
            tbScanLog.Text += string.Format("AL\t{0:0.000}\tAR\t{1:0.000}\r\n", res[0].CArea, res[1].CArea);
          //  Process.LEDs_All_On(false);

        }

     
        private void btnFindVertex_Click(object sender, EventArgs e)
        {
            InspResult[] res = new InspResult[2] { new InspResult(), new InspResult() };
            int camCnt = Cam.CamList.Count;
            if (IsLoadImage.Checked) camCnt = 1;
            Process.LEDs_All_On(true);
            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Live(false);

            });
            Thread.Sleep(200);
            Parallel.For(0, camCnt, i =>
            {
                if (!IsLoadImage.Checked) Cam.CamList[i].Acquire();
                InspApi[i].NewFineCOG(i, 0, STATIC.InspMat[i].Clone(), InspectionType.JustFind_Vertex, res[i], true, false, true);

            });
            pictureBox1.Image = STATIC.ResMat[0].Clone().ToBitmap();
            if (camCnt > 1)
                pictureBox2.Image = STATIC.ResMat[1].Clone().ToBitmap();
           // Process.LEDs_All_On(false);

        }

        private void btnFindCircleAcc_Click(object sender, EventArgs e)
        {

            InspResult[] res = new InspResult[2] { new InspResult(), new InspResult() };
            int camCnt = Cam.CamList.Count;
            if (IsLoadImage.Checked) camCnt = 1;
            Process.LEDs_All_On(true);
            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Live(false);

            });
            Thread.Sleep(200);
            Parallel.For(0, camCnt, i =>
            {
                if (!IsLoadImage.Checked) Cam.CamList[i].Acquire();
                if (STATIC.Rcp.Option.AreaDecenterUse)
                    InspApi[i].NewFineCOG(i, 0, STATIC.InspMat[i].Clone(), InspectionType.Area_CircleAccuracy, res[i], true, false, true);
                else InspApi[i].NewFineCOG(i, 0, STATIC.InspMat[i].Clone(), InspectionType.InCircle_CircleAccuracy, res[i], true, false, true);

            });
            tbScanLog.Text += string.Format("Circle Accuracy \r\nLeft > {0:0.000}\tRight > {1:0.000}\r\n", res[0].CircleAcuraccy, res[1].CircleAcuraccy);
            pictureBox1.Image = STATIC.ResMat[0].Clone().ToBitmap();
            if (camCnt > 1)
                pictureBox2.Image = STATIC.ResMat[1].Clone().ToBitmap();
         //   Process.LEDs_All_On(false);

            if(IsLoadImage.Checked)
            {
                string dateDir = STATIC.CreateDateDir();
                string path = string.Empty;
                string path1 = string.Empty;
                dateDir += "ActroImg\\";
                if (!Directory.Exists(dateDir))
                    Directory.CreateDirectory(dateDir);
                path = string.Format("{0}{1}Position_{2}.bmp", dateDir, Convert.ToInt32(tbInspIndex.Text), 0);
                path1 = string.Format("{0}{1}Position_{2}_res.txt", dateDir, Convert.ToInt32(tbInspIndex.Text), 0);
                Bitmap b = STATIC.ResMat[0].ToBitmap();

                b.Save(path);
              
                StreamWriter sw = new StreamWriter(path1);
                sw.WriteLine(tbScanLog.Text);
                sw.Close();
                   
            }

        }

        private void button14_Click(object sender, EventArgs e)
        {
            string sFilePath = STATIC.BaseDir;
            OpenFileDialog opfd = new OpenFileDialog();
            opfd.DefaultExt = "txt";
            opfd.InitialDirectory = sFilePath;
            opfd.Filter = "Txt(*.txt)|*.txt";
            opfd.Title = "PID Update Path";
            if (opfd.ShowDialog() == DialogResult.OK)
            {
                STATIC.ManualDrivePath = opfd.FileName;// + "\\" + opfd.SafeFileName;

                tbManualDrvPath.Text = STATIC.ManualDrivePath;
            }
        }
        
        void ManualTest()
        {
            try
            {
                Parallel.For(0, Cam.CamList.Count, i =>
                {
                    Cam.CamList[i].Live(false);

                });
              
                string Path = STATIC.DataDir + "ManualDrvImage\\";
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);

            

                string textval = System.IO.File.ReadAllText(STATIC.ManualDrivePath);
                string[] t = textval.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                if (tbScanLog.InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate
                    {
                        tbScanLog.Text = string.Empty;
                    });
                }
                else
                {
                    tbScanLog.Text = string.Empty;
                }
                
                for (int i = 0; i < t.Length; i++)
                {
                    string[] b = t[i].Split(new string[] { ",", " ", "\t", "//", "Reg" }, StringSplitOptions.RemoveEmptyEntries);
                    switch (b[0])
                    {
                        case "Code":
                        case "code":
                            DrvIC.Move(0, DriverIC.AK, Convert.ToInt32(b[1]));

                            if (tbScanLog.InvokeRequired)
                            {
                                BeginInvoke((MethodInvoker)delegate
                                {
                                    tbScanLog.Text += "\r\n";
                                    tbScanLog.Text += "CH1 >> Move " + b[1] + "\r\n";
                                });
                            }
                            else
                            {
                                tbScanLog.Text += "\r\n";
                                tbScanLog.Text += "CH1 >> Move " + b[1] + "\r\n";
                            }

                            break;
                        case "Delay":
                        case "delay":
                            Thread.Sleep(Convert.ToInt32(b[1]));
                            if (tbScanLog.InvokeRequired)
                            {
                                BeginInvoke((MethodInvoker)delegate
                                {
                                    tbScanLog.Text += "\r\n";
                                    tbScanLog.Text += "Delay " + b[1] + "\r\n";
                                });
                            }
                            else
                            {
                                tbScanLog.Text += "\r\n";
                                tbScanLog.Text += "Delay " + b[1] + "\r\n";
                            }

                         
                            break;
                        case "Result":
                        case "result":

                            InspResult CoverRes = new InspResult();
                            InspResult Res = new InspResult();
                            InspResult CRes = new InspResult();
                         
                            Process.LEDs_All_On(true);
                            Thread.Sleep(200);
                            if (!IsLoadImage.Checked) Cam.CamList[0].Acquire();
                            string fileName = DateTime.Now.ToString("yyMMddhhmmss.fff") + ".bmp";
                            if (tbScanLog.InvokeRequired)
                            {
                                BeginInvoke((MethodInvoker)delegate
                                {
                                    tbScanLog.Text += "\r\n";
                                    tbScanLog.Text += "Image >>" +  fileName;
                                });
                            }
                            else
                            {
                                tbScanLog.Text += "\r\n";
                                tbScanLog.Text += "Image >>" + fileName;
                            }
                            STATIC.InspMat[0].SaveImage(fileName);

                            InspApi[i].NewFineCOG(0, 0,STATIC.InspMat[0].Clone(), InspectionType.FindCover, CoverRes, true, false, true);

                            if (STATIC.Rcp.Option.AreaDecenterUse)
                                InspApi[i].NewFineCOG(0, 0, STATIC.InspMat[0].Clone(), InspectionType.Area_InspAll, Res, true, false, true);
                            else
                                InspApi[i].NewFineCOG(0, 0, STATIC.InspMat[0].Clone(), InspectionType.InCircle_InspAll, Res, true, false, true);


                            InspApi[i].TestCDll2(0, STATIC.InspMat[0].Clone(), false, 3, CRes, false);

                            int ReadHallCh1 = 0;
                            ReadHallCh1 = DrvIC.ReadHall(0, DriverIC.AK);

                            double LX = STATIC.ScaleResolution[0] * 1000 * (CoverRes.Cover_cx - Res.cx);    //  Left Decenter_X
                            double LY = STATIC.ScaleResolution[0] * 1000 * (CoverRes.Cover_cy - Res.cy);    //  Left Decenter_Y

                            double CLX = /*STATIC.ScaleResolution[0] **/ 1000 * (CRes.Cover_cx - CRes.cx);    //  Left Decenter_X
                            double CLY = /*STATIC.ScaleResolution[0] **/ 1000 * (CRes.Cover_cy - CRes.cy);    //  Left Decenter_Y

                            if (tbScanLog.InvokeRequired)
                            {
                                BeginInvoke((MethodInvoker)delegate
                                {
                                    tbScanLog.Text += "\r\n";
                                    tbScanLog.Text += string.Format("CH1 >> Hall = {0}\r\n", ReadHallCh1.ToString("D4"));
                                    tbScanLog.Text += string.Format("CH1 >> Area = {0: 00.000}\r\n", Res.Area);
                                    tbScanLog.Text += string.Format("CH1 >> Decenter X = {0: 000.000}, Decenter Y = {1: 000.000}\r\n", LX, LY);
                                    tbScanLog.Text += string.Format("CH1 >> Circle Accuracy = {0: 000.000}\r\n", Res.CircleAcuraccy);
                                    tbScanLog.Text += string.Format("CH1 >> Shape Accuracy = {0: 000.000}\r\n", Res.ShapeAccuracy);

                                    tbScanLog.Text += "\r\n";

                                    tbScanLog.Text += string.Format("CH1 >> C_Area = {0: 00.000}\r\n", CRes.CArea);
                                    tbScanLog.Text += string.Format("CH1 >> C_Decenter X = {0: 000.000}, C_Decenter Y = {1: 000.000}\r\n", CLX, CLY);
                                    tbScanLog.Text += string.Format("CH1 >> CCircle Accuracy = {0: 000.000}\r\n", CRes.CircleAcuraccy * 1000);
                                    tbScanLog.Text += string.Format("CH1 >> CShape Accuracy = {0: 000.000}\r\n", CRes.ShapeAccuracy * 1000);
                                });
                            }
                            else
                            {
                                tbScanLog.Text += "\r\n";
                                tbScanLog.Text += string.Format("CH1 >> Hall = {0}\r\n", ReadHallCh1.ToString("D4"));
                                tbScanLog.Text += string.Format("CH1 >> Area = {0: 00.000}\r\n", Res.Area);
                                tbScanLog.Text += string.Format("CH1 >> Decenter X = {0: 000.000}, Decenter Y = {1: 000.000}\r\n", LX, LY);
                                tbScanLog.Text += string.Format("CH1 >> Circle Accuracy = {0: 000.000}\r\n", Res.CircleAcuraccy);
                                tbScanLog.Text += string.Format("CH1 >> Shape Accuracy = {0: 000.000}\r\n", Res.ShapeAccuracy);

                                tbScanLog.Text += "\r\n";

                                tbScanLog.Text += string.Format("CH1 >> C_Area = {0: 00.000}\r\n", CRes.CArea);
                                tbScanLog.Text += string.Format("CH1 >> C_Decenter X = {0: 000.000}, C_Decenter Y = {1: 000.000}\r\n", CLX, CLY);
                                tbScanLog.Text += string.Format("CH1 >> CCircle Accuracy = {0: 000.000}\r\n", CRes.CircleAcuraccy * 1000);
                                tbScanLog.Text += string.Format("CH1 >> CShape Accuracy = {0: 000.000}\r\n", CRes.ShapeAccuracy * 1000);
                            }
                         //   Process.LEDs_All_On(false);

                            break;
                    }
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
        }
        private void btnDrvManual_Click(object sender, EventArgs e)
        {
            Task t = new Task(() => ManualTest());
            t.Start();
        
            
        }

        private void btnFindShapeAcc_Click(object sender, EventArgs e)
        {

            InspResult[] res = new InspResult[2] { new InspResult(), new InspResult() };
            int camCnt = Cam.CamList.Count;
            if (IsLoadImage.Checked) camCnt = 1;
            Process.LEDs_All_On(true);
            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Live(false);

            });
            Thread.Sleep(200);
            Parallel.For(0, camCnt, i =>
            {
                if (!IsLoadImage.Checked) Cam.CamList[i].Acquire();
                if (STATIC.Rcp.Option.AreaDecenterUse)
                    InspApi[i].NewFineCOG(i, 0, STATIC.InspMat[i].Clone(), InspectionType.Area_ShapeAccuracy, res[i], true, false, true);
                else InspApi[i].NewFineCOG(i, 0, STATIC.InspMat[i].Clone(), InspectionType.InCircle_ShapeAccuracy, res[i], true, false, true);

            });

            tbScanLog.Text += string.Format("Shape Accuracy \r\nLeft > {0:0.000}\tRight > {1:0.000}\r\n", res[0].ShapeAccuracy, res[1].ShapeAccuracy);
            pictureBox1.Image = STATIC.ResMat[0].Clone().ToBitmap();
            if (camCnt > 1)
                pictureBox2.Image = STATIC.ResMat[1].Clone().ToBitmap();
           // Process.LEDs_All_On(false);


        }

        private void btnFind14ActDecenter_Click(object sender, EventArgs e)
        {
            //double LX = 0;
            //double LY = 0;


            //Process.LEDs_All_On(true);
            //Thread.Sleep(200);
            //for (int i = 0; i < Vision.CamList.Count; i++)
            //{
            //    if (!IsLoadImage.Checked) Vision.MilList[i].GrabA();

            //    ScanResult result = new ScanResult();
            //    ScanResult result2 = new ScanResult();
            //    Vision.MilList[i].FineCOG(0, "OpenHexagon", result2, false, true);
            //    Vision.MilList[i].NewFineCOG(false, 0, InspectionType.InCircle_Decenter, result, false, true, Convert.ToInt32(tbInspIndex.Text), false);
            //    // Vision.MilList[i].DrawPloygon(result2.pt[i], Brushes.Orange);

            //    Point2f ctpt = new Point2f();
            //    float rad = 0;

            //    OpenCvSharp.Point[] pts = new OpenCvSharp.Point[result2.pt[0].Length];
            //    for (int j = 0; j < pts.Length; j++)
            //    {
            //        pts[j] = new OpenCvSharp.Point(result2.pt[0][j].X, result2.pt[0][j].Y);
            //    }


            //    Cv2.MinEnclosingCircle(pts, out ctpt, out rad);
            //    System.Drawing.PointF center = new PointF(ctpt.X, ctpt.Y);
            //    Vision.MilList[i].DrawDCCircle(center, rad, Brushes.Orange, 2);
            //    Vision.MilList[i].DrawDCCross(Brushes.Orange, (int)center.X, (int)center.Y, 3, 2);



            //    LX = STATIC.ScaleResolution[0] * 1000 * (center.X - result.cx[0][0]) * Dscale.DecenterScale_L;    //  Left Decenter_X
            //    LY = STATIC.ScaleResolution[0] * 1000 * (center.Y - result.cy[0][0]) * Dscale.DecenterScale_L;    //  Left Decenter_Y

            //    tbScanLog.Text += string.Format("Left > Decenter_X : \t{0:0.000}  \tDecenter_Y : \t{1:0.000}\r\n", LX, LY);

            //}
            //Process.LEDs_All_On(false);
        }

        private void LoadUnloadCover_Click(object sender, EventArgs e)
        {
            if (!Dln.IsCoverUp) Dln.CoverUpDown(true);
            else Dln.CoverUpDown(false);
        }

        private void LoadUnloadAll_Click(object sender, EventArgs e)
        {
            if (!Dln.IsLoad && Dln.IsCoverUp)
            {
                Dln.LoadSocket(true);

                while (true) if (Dln.IsLoad) break;
                
                Dln.CoverUpDown(false);

                for(int i = 0; i < 2; i++)
                {
                    try 
                    {
                        int [] addr = Dln.DLNi2c[i].ScanDevices();
                        string log = "";
                        foreach (int ad in addr) {
                            log += string.Format(" {0}", ad);
                            if (ad == DrvIC.AkSlave[i]) CStatus[i] = true;
                                }
                    }
                    catch
                    {
                        return;
                    }

                }
            }
            else if (Dln.IsLoad && !Dln.IsCoverUp)
            {
                for (int i = 0; i < 2; i++) CStatus[i] = false;

                Dln.CoverUpDown(true);

                while (true) if (Dln.IsCoverUp) break;

                Dln.LoadSocket(false);

                while (true) if (!Dln.IsLoad) break;
            }
        }

        private void LED_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton bt = (RadioButton)sender;

            if (bt.Checked)
            {
                btnLEDUp.Text = string.Format("{0} brighter", bt.Text);
                btnLEDDown.Text = string.Format("{0} darker", bt.Text);
                if (bt.Text.Contains("L Outer"))
                    tbLEDvoltage.Text = VisionFile.TopOuterLED_L.ToString("f2");
                else if (bt.Text.Contains("L Inner"))
                    tbLEDvoltage.Text = VisionFile.TopInnerLED_L.ToString("f2");
                else if (bt.Text.Contains("L Btm"))
                    tbLEDvoltage.Text = VisionFile.BtmLED_L.ToString("f2");
                else if (bt.Text.Contains("R Top"))
                    tbLEDvoltage.Text = VisionFile.TopInnerLED_R.ToString("f2");
                else if (bt.Text.Contains("R Btm"))
                    tbLEDvoltage.Text = VisionFile.BtmLED_R.ToString("f2");

            }
        }

        private void F_Vision_VisibleChanged(object sender, EventArgs e)
        {
            if(Visible)
            {
                tbExposure.Text = Cam.CamList[0].CamInfo.ExpTime.ToString();
                if (Cam.CamList.Count > 1)
                    tbExposureR.Text = Cam.CamList[1].CamInfo.ExpTime.ToString();
                tbLeftScale.Text = VisionFile.Scale_L.ToString();
                tbRightScale.Text = VisionFile.Scale_R.ToString();
                tbLDecScale.Text = Dscale.DecenterScale_L.ToString();
                tbRDecscale.Text = Dscale.DecenterScale_R.ToString();
            }
        }

        private void btnApplyScale_Click(object sender, EventArgs e)
        {
            VisionFile.Scale_L = Convert.ToDouble(tbLeftScale.Text);
            VisionFile.Scale_R = Convert.ToDouble(tbRightScale.Text);
            Dscale.DecenterScale_L = Convert.ToDouble(tbLDecScale.Text);
            Dscale.DecenterScale_R = Convert.ToDouble(tbRDecscale.Text);
            STATIC.ScaleResolution[0] = STATIC.DefaultResolution * VisionFile.Scale_L;
            STATIC.ScaleResolution[1] = STATIC.DefaultResolution * VisionFile.Scale_R;
            VisionFile.Save();
            Dscale.Save();
        }


     
     

        private void button3_Click(object sender, EventArgs e)
        {
            InspResult[] res = new InspResult[2] { new InspResult(), new InspResult() };
            int camCnt = Cam.CamList.Count;
            if (IsLoadImage.Checked) camCnt = 1;
            Process.LEDs_All_On(true);
            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Live(false);

            });
            Thread.Sleep(200);
            Parallel.For(0, camCnt, i =>
            {
                if (!IsLoadImage.Checked) Cam.CamList[i].Acquire();
                if (STATIC.Rcp.Option.AreaDecenterUse)
                    InspApi[i].NewFineCOG(i, 0,STATIC.InspMat[i].Clone(), InspectionType.Area_CircleAccuracy, res[i], true, true, true);
                else InspApi[i].NewFineCOG(i, 0,STATIC.InspMat[i].Clone(), InspectionType.InCircle_CircleAccuracy, res[i], true, true, true);

            });
            tbScanLog.Text += string.Format("Circle Accuracy \r\nLeft > {0:0.000}\tRight > {1:0.000}\r\n", res[0].CircleAcuraccy, res[1].CircleAcuraccy);
            pictureBox1.Image = STATIC.ResMat[0].Clone().ToBitmap();
            if (camCnt > 1)
                pictureBox2.Image = STATIC.ResMat[1].Clone().ToBitmap();
         //   Process.LEDs_All_On(false);

            if (IsLoadImage.Checked)
            {
                string dateDir = STATIC.CreateDateDir();
                string path = string.Empty;
                string path1 = string.Empty;
                dateDir += "ActroImg\\";
                if (!Directory.Exists(dateDir))
                    Directory.CreateDirectory(dateDir);
                path = string.Format("{0}{1}Position_{2}.bmp", dateDir, Convert.ToInt32(tbInspIndex.Text), 0);
                path1 = string.Format("{0}{1}Position_{2}_res.txt", dateDir, Convert.ToInt32(tbInspIndex.Text), 0);
                Bitmap b = STATIC.ResMat[0].ToBitmap();

                b.Save(path);

                StreamWriter sw = new StreamWriter(path1);
                sw.WriteLine(tbScanLog.Text);
                sw.Close();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InspResult[] res = new InspResult[2] { new InspResult(), new InspResult() };
            int camCnt = Cam.CamList.Count;
            if (IsLoadImage.Checked) camCnt = 1;
            Process.LEDs_All_On(true);
            Parallel.For(0, Cam.CamList.Count, i =>
            {
                Cam.CamList[i].Live(false);

            });
            Thread.Sleep(200);
            Parallel.For(0, camCnt, i =>
            {
                if (!IsLoadImage.Checked) Cam.CamList[i].Acquire();
                InspApi[i].TestCDll2(i, STATIC.InspMat[i].Clone(), true, 3, res[i], false);

            });
            double LX = 1000 * (res[0].Cover_cx - res[0].cx);    //  Left Decenter_X
            double LY = 1000 * (res[0].Cover_cy - res[0].cy);    //  Left Decenter_Y
            double RX = 1000 * (res[1].Cover_cx - res[1].cx);    //  Right Decenter_X
            double RY = 1000 * (res[1].Cover_cy - res[1].cy);    //  Right Decenter_X
            double CircleAccL = res[0].CCircleAcuraccy * 1000;
            double CircleAccR = res[1].CCircleAcuraccy * 1000;
            double ShapeAccL = res[0].CShapeAccuracy * 1000;
            double ShapeAccR = res[1].CShapeAccuracy * 1000;


            tbScanLog.Text += string.Format("AL\t{0:0.000}\tAR\t{1:0.000}\r\n", res[0].CArea, res[1].CArea);
            tbScanLog.Text += string.Format("Left > Decenter_X : \t{0:0.000}  \tDecenter_Y : \t{1:0.000}\r\n", LX, LY);
            tbScanLog.Text += string.Format("nRight > Decenter_X : \t{0:0.000}  \tDecenter_Y : \t{1:0.000}\r\n", RX, RY);
            tbScanLog.Text += string.Format("Left > Circle Accuracy : \t{0:0.000}  \tShape Accuracy : \t{1:0.000}\r\n", CircleAccL, ShapeAccL);
            tbScanLog.Text += string.Format("nRight > Circle Accuracy : \t{0:0.000}  \tShape Accuracy : \t{1:0.000}\r\n", CircleAccR, ShapeAccR);
          //  Process.LEDs_All_On(false);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            STATIC.Rcp.F16Scale.F16Scale = Convert.ToDouble(tbF16AreaScale.Text);
            STATIC.Rcp.F16Scale.Save();

            //VisionFile.Scale_L = Convert.ToDouble(tbLeftScale.Text);
            //VisionFile.Scale_R = Convert.ToDouble(tbRightScale.Text);
            //Dscale.DecenterScale_L = Convert.ToDouble(tbLDecScale.Text);
            //Dscale.DecenterScale_R = Convert.ToDouble(tbRDecscale.Text);
            //STATIC.ScaleResolution[0] = STATIC.DefaultResolution * VisionFile.Scale_L;
            //STATIC.ScaleResolution[1] = STATIC.DefaultResolution * VisionFile.Scale_R;
            //VisionFile.Save();
            //Dscale.Save();
        }

        private void btnBottomLedOn_Click(object sender, EventArgs e)
        {
            Process.BtmLed(true);
        }

        private void btnTO_On_Click(object sender, EventArgs e)
        {
            Process.DualOuterLed(true);
        }

        private void btnTO_Off_Click(object sender, EventArgs e)
        {
            Process.DualOuterLed(false);
        }

        private void btnTI_On_Click(object sender, EventArgs e)
        {
            Process.DualInnerLed(true);
        }

        private void btnTI_Off_Click(object sender, EventArgs e)
        {
            Process.DualInnerLed(false);
        }

        private void btnBtmLedOff_Click(object sender, EventArgs e)
        {
            Process.BtmLed(false);
        }

        private void btnPowerOn_Click(object sender, EventArgs e)
        {
            Dln.powerOnOff(true);
        }

        private void btnPowerOff_Click(object sender, EventArgs e)
        {
            Dln.powerOnOff(false);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < Process.ChannelCnt; i++)
                Dln.WriteArray(i, DrvIC.AkSlave[i], 0x02, 1, new byte[] { 0x40 });
        }
    }
}

