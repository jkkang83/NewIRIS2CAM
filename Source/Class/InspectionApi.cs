using DavinciIRISTester;
using MathNet.Numerics.LinearAlgebra;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace M1Wide
{
    public class GrabImgInfo : IDisposable
    {
        public int InspIndex;
        public Mat Img;

        public GrabImgInfo Copy()
        {
            return new GrabImgInfo()
            {
                InspIndex = InspIndex,
                Img = Img.Clone(),
            };
        }
        public void Dispose()
        {
            if (Img != null) Img.Dispose();

            GC.Collect();
        }
    }

    public class Point2D
    {
        public double X;
        public double Y;
        public Point2D(double lx = 0, double ly = 0)
        {
            X = lx; Y = ly;
        }
    }
    public class CircleInfo
    {
        public double X;
        public double Y;
        public double R;
    }
    public class ringBufLinComp
    {
        public float[] dataX = new float[65];
        public float[] dataY = new float[65];
        public int startIndex;
        public int endIndex;
        public int size;

    }
    public class InspResult
    {
        public int code;
        public double current;
        public int IrisHall;

        public double Area;
        public double cx;
        public double cy;
        public double Cover_cx;
        public double Cover_cy;
       
        public double DecenterR;
        public double DecenterX;
        public double DecenterY;
        public double CircleAcuraccy;
        public double ShapeAccuracy;

        public double CArea;
        public double CDecenterR;
        public double CDecenterX;
        public double CDecenterY;
        public double CCircleAcuraccy;
        public double CShapeAccuracy;
        public double CCover_dia;
        public Bitmap img;    
        public List<PointF[]> pt = new List<PointF[]>() { new PointF[1], new PointF[1] };
        public bool isFinish = false;

    }
    public class SettlingData
    {
        public int ReadHall;
        public double Time;
        public double InitialTime;
        public double SettlingTime;
        public int InitCode;
        public int FinalCode;

    }
    public class CAResult
    {
        public List<double> VertexLength = new List<double>();
        public int MaxVertexIndex = 0;
        public int MinVertexIndex = 0;
        public List<PointF> OtherVertex = new List<PointF>();
        public double CA = 0;
        public PointF MaxPoint = new PointF();
        public PointF MinPoint = new PointF();

    }
    public class Camera
    {
        public List<Grabber> CamList;
        public string CamSysFile { get; set; }
         
        public Camera()
        {
            if (!Directory.Exists(STATIC.RootDir)) Directory.CreateDirectory(STATIC.RootDir);
            CamSysFile = STATIC.RootDir + "CameraInfo.xml";
            CamList = new List<Grabber>();

        }
        public Camera(string path)
        {
            CamSysFile = path;
            CamList = new List<Grabber>();

        }
        public void SaveXml()
        {
            XmlSerializer xml_serializer = new XmlSerializer(this.GetType());
            using (StreamWriter stream_writer = new StreamWriter(CamSysFile))
            {
                xml_serializer.Serialize(stream_writer, this);
                stream_writer.Close();
            }
        }

        public void LoadXml()
        {
            if (File.Exists(CamSysFile))
            {
                XmlSerializer xml_serializer = new XmlSerializer(this.GetType());

                using (StreamReader file_stream = new StreamReader(CamSysFile, true))
                {
                    LoadFileSet((Camera)xml_serializer.Deserialize(file_stream));
                }
            }
            else
            {
                Initialize();
            }
        }

        public void LoadFileSet(Camera param)
        {
            CamList = new List<Grabber>();
            CamList = param.CamList.ToList();
        }

        public void Initialize()
        {
            CamList = new List<Grabber>();

        }
        public void Dispose()
        {


            if (CamList != null)
            {
                while (CamList.Count > 0)
                {
                    CamList[0].DestoryCamera();
                    CamList.RemoveAt(0);
                }
                CamList.Clear();
            }
            CamList = null;
            GC.SuppressFinalize(this);
        }


        public void LoadCameraList()
        {
            STATIC.DefaultWidth = 1500;
            STATIC.DefaultHeight = 1500;
            //if (STATIC.Rcp.Model.ModelName == "SO1C81")
            //{
               
            //}
            if (File.Exists(CamSysFile))
            {
                LoadXml();
                CameraInfo info;
                for (int i = 0; i < CamList.Count; i++)
                {
                    info = CamList[i].CamInfo.Copy();
                    CamList[i].OpenCamera(CamList[i].CamInfo.SerialNumber, false);
                    CamList[i].CamInfo.CamIndex = info.CamIndex;
                    CamList[i].CamInfo.IPAddr = info.IPAddr;
                    CamList[i].CamInfo.SerialNumber = info.SerialNumber;
                    CamList[i].CamInfo.ModelName = info.ModelName;
                    CamList[i].CamInfo.TrigMode = info.TrigMode;
                    CamList[i].CamInfo.TrigSource = info.TrigSource;
                    CamList[i].CamInfo.ExpTime = info.ExpTime;
                    CamList[i].CamInfo.GainOffset = info.GainOffset;
                    CamList[i].CamInfo.Width = info.Width;
                    CamList[i].CamInfo.Height = info.Height;
                    CamList[i].CamInfo.OffsetX = info.OffsetX;
                    CamList[i].CamInfo.OffsetY = info.OffsetY;

                    CamList[i].ExposureTime = info.ExpTime;
                    CamList[i].GainOffset = info.GainOffset;
                    if (STATIC.DefaultWidth + info.OffsetX > STATIC.CamDefaultWidth)
                        info.OffsetX = STATIC.CamDefaultWidth - STATIC.DefaultWidth;
                    CamList[i].OffsetX = info.OffsetX;
                    if (STATIC.DefaultHeight + info.OffsetY > STATIC.CamDefaultHeight)
                        info.OffsetY = STATIC.CamDefaultHeight - STATIC.DefaultHeight;
                    CamList[i].OffsetY = info.OffsetY;
                    CamList[i].TriggerMode = info.TrigMode;
                    CamList[i].TriggerSource = info.TrigSource;

                }
                if (CamList[0].CamInfo.CamIndex != 0)
                {
                    CamList.Reverse();
                }

                SaveXml();
            }
            else
            {
                string[] camName = CameraList.GetCameraModelNames;
                for (int i = 0; i < camName.Length; i++)
                {
                    CamList.Add(new Grabber());
                    CamList[CamList.Count - 1].OpenCamera(i, true);
                }
                SaveXml();
            }
        }
    }  
    public class InspectionApi
    {
        #region LinComp


        float[] sarrX13 = new float[STATIC.MAX_NUM_DATA];
        float[] sarrIDEAL = new float[STATIC.MAX_NUM_DATA];
        float[] sarrINL0 = new float[STATIC.MAX_NUM_DATA];
        float[] sarrINL2 = new float[STATIC.MAX_NUM_DATA];

        float[] snorX = new float[STATIC.MAX_NUM_DATA];
        float[] sarrY13 = new float[STATIC.MAX_NUM_DATA];
        float[] snorY = new float[STATIC.MAX_NUM_DATA];
        float[] sarrIdeal = new float[STATIC.MAX_NUM_DATA];
        float[] sarrInl0 = new float[STATIC.MAX_NUM_DATA];
        float[] sarrInl2 = new float[STATIC.MAX_NUM_DATA];
        float[] sarrLin2 = new float[STATIC.MAX_NUM_DATA];

        int[] sarrRegCoef2 = new int[STATIC.NUM_COEF2_1 + STATIC.NUM_COEF2_2];

        float[] sSlope = new float[STATIC.MAX_NUM_DATA];
        float[] sIntercept = new float[STATIC.MAX_NUM_DATA];

        float[] snorXLNC = new float[STATIC.MAX_NUM_DATA];
        float[] snorYLNC = new float[STATIC.MAX_NUM_DATA];
        float[] sarrX13LNC = new float[STATIC.MAX_NUM_DATA];
        float[] sarrY13LNC = new float[STATIC.MAX_NUM_DATA];

        float[] sarrNorCoef2 = new float[STATIC.NUM_COEF2_1 + STATIC.NUM_COEF2_2];
        float[] sarrCoef2 = new float[STATIC.NUM_COEF2_1 + STATIC.NUM_COEF2_2];
        public int LinCompMain(float[] targPosi, float[] Area, int numData, int pVt, int nVt, int ignInf, int ignMac, int[] linCoef, ref float resError, bool isReverse)
        {
            for (int i = 0; i < 27; i++)
                linCoef[i] = 0;
            resError = 5000;
            ringBufLinComp lineData = new ringBufLinComp();
            for (int i = 0; i < numData; i++)
            {
                lineData.dataX[i + 1] = targPosi[i];
                if (isReverse)
                    lineData.dataY[i + 1] = -Area[i];
                else
                    lineData.dataY[i + 1] = Area[i];
            }
            lineData.startIndex = 1;
            lineData.endIndex = numData;
            lineData.size = numData;

            float spPos = 0.0f;
            float snPos = 0.0f;
            if (PreData(ref lineData, pVt, nVt, ignInf, ignMac, ref spPos, ref snPos) != 0) { return 1; }
            float smaxX = 0;
            float sminX = 0;
            float smaxY;
            float sminY;
            smaxY = lineData.dataY[numData];
            sminY = lineData.dataY[1];
            int posibit = 10;
            if (ConvData(lineData, ref posibit, spPos, snPos, sarrX13, sarrY13, snorX, snorY, sarrIDEAL, sarrIdeal, ref smaxX, ref sminX, ref smaxY, ref sminY) != 0) { return 2; }
            CalINL0(sarrX13, sarrY13, snorX, snorY, sarrIDEAL, sarrIdeal, sarrINL0, sarrInl0);

            // Calculation of line compensation coefficients2
            for (int i = 0; i < 73; i++) { sarrRegCoef2[i] = 0; }
            if (CalLinCompCoef2(snorX, sarrInl0, snorY, sarrX13, sarrY13, spPos, snPos, smaxX, sminX, smaxY, sminY, sarrIDEAL, sarrIdeal, sarrRegCoef2, sarrINL2, sarrLin2, sarrInl2) != 0) { return STATIC.ERROR_CALCOEF2; }

            // INL calculation after LNC2
            CalINL2(sarrX13, sarrY13, snorX, snorY, sarrIDEAL, sarrIdeal, sarrINL2, sarrLin2, sarrInl2, sarrRegCoef2, spPos, snPos, smaxX, sminX, smaxY, sminY, 1);

            // Residual INL2 Error
            resError = 0;
            for (int i = 0; i < STATIC.MAX_NUM_DATA; i++)
            {
                if (AKM_Abs(sarrINL2[i]) > resError) { resError = AKM_Abs(sarrINL2[i]); }
            }

            // Register Address
            if (ConvReg(sarrRegCoef2, linCoef) != 0) { return STATIC.ERROR_CONVREG; }

            /*
            //debug----------
            if (posibit == 14){
                for(int i = 0; i < MAX_NUM_DATA; i++)			{sarrX13[i] = sarrX13[i] * 2.0f;}
            }else{
                for(int i = 0; i < MAX_NUM_DATA; i++)			{sarrX13[i] = sarrX13[i] / AKM_Pow(2.0f, 13 - posibit);}
            }

            printf("\npVt, nVt, ignInf, ignMac, posibit, spPos, snPos, &smaxX, &sminX, &smaxY, &sminY\n");
            printf("%d, %d, %d, %d, %d, %f, %f, %f, %f, %f, %f\n ", pVt, nVt, ignInf, ignMac, posibit, spPos, snPos, smaxX, sminX, smaxY, sminY);

            printf("\ntargPosi, lensPosi\n");
            for (int i = 0; i < numData; i++)					{printf("%f, %f\n ", lineData.dataX[i+1],lineData.dataY[i+1]);}

            printf("\nsarrX13[i], sarrINL0[i], sarrINL2[i], snorY[i], sarrInl0[i], sarrInl2[i], sarrLin2[i]\n");
            for(int i = 0; i < MAX_NUM_DATA; i++)				{printf("%f, %f, %f, %f, %f, %f, %f\n", sarrX13[i], sarrINL0[i], sarrINL2[i], snorY[i], sarrInl0[i], sarrInl2[i], sarrLin2[i]);}

            printf("\nsarrY13[i], snorX[i], sarrIDEAL[i], sarrIdeal[i]\n");
            for(int i = 0; i < MAX_NUM_DATA; i++)				{printf("%f, %f, %f, %f\n", sarrY13[i], snorX[i], sarrIDEAL[i], sarrIdeal[i]);}

            printf("\nCOEF2\n");
            printf("0, ");
            for (int i = 0; i < NUM_COEF2_1; i++)	{printf("%d,", sarrRegCoef2[i]);}
            printf("127\n ");
            for (int i = 0; i < NUM_COEF2_2; i++)	{printf("%d,", sarrRegCoef2[i+NUM_COEF2_1]);}
            //-----------------
            */

            return 0;
        }
        int ConvReg(int[] arrRegCoef2, int[] linCoef)
        {
            int[] arrRegCoefX = new int[4];
            int[] arrRegCoefY = new int[STATIC.NUM_COEF2_2];
            int[] XCoef = new int[4];

            //arrRegX
            int k = 0;
            for (int i = 0; i < STATIC.NUM_COEF2_1; i++)
            {
                if ((arrRegCoef2[i] & 2) == 2)
                {
                    arrRegCoefX[k] = arrRegCoef2[i];
                    XCoef[k] = i + 1;
                    if (k == 3) { break; }
                    k += 1;
                }
            }
            if (k != 3) { return 1; }

            //arrRegY
            for (int i = 0; i < STATIC.NUM_COEF2_2 - 5; i++)
            {
                if (i < XCoef[0])
                {
                    arrRegCoefY[i] = arrRegCoef2[STATIC.NUM_COEF2_1 + i];
                }
                else if (i < XCoef[1] - 1)
                {
                    arrRegCoefY[i] = arrRegCoef2[STATIC.NUM_COEF2_1 + i + 1];
                }
                else if (i < XCoef[2] - 2)
                {
                    arrRegCoefY[i] = arrRegCoef2[STATIC.NUM_COEF2_1 + i + 2];
                }
                else if (i < XCoef[3] - 3)
                {
                    arrRegCoefY[i] = arrRegCoef2[STATIC.NUM_COEF2_1 + i + 3];
                }
                else
                {
                    arrRegCoefY[i] = arrRegCoef2[STATIC.NUM_COEF2_1 + i + 4];
                }
            }
            for (int i = 0; i < 4; i++)
            {
                arrRegCoefY[STATIC.NUM_COEF2_2 - 5 + i] = arrRegCoef2[STATIC.NUM_COEF2_1 + XCoef[i]];
            }

            arrRegCoefY[37] = arrRegCoef2[STATIC.NUM_COEF2_1 + STATIC.NUM_COEF2_2 - 1];

            //Register 
            for (int i = 0; i < 5; i++)
            {
                linCoef[i * 5 + 0] = (arrRegCoefY[i * 8 + 1] & 7) * 32 + (arrRegCoefY[i * 8 + 0]);
                linCoef[i * 5 + 1] = (arrRegCoefY[i * 8 + 3] & 1) * 128 + (arrRegCoefY[i * 8 + 2]) * 4 + (arrRegCoefY[i * 8 + 1] >> 3);
                linCoef[i * 5 + 2] = (arrRegCoefY[i * 8 + 4] & 15) * 16 + (arrRegCoefY[i * 8 + 3] >> 1);
                if (i < 4)
                {
                    linCoef[i * 5 + 3] = (arrRegCoefY[i * 8 + 6] & 3) * 64 + (arrRegCoefY[i * 8 + 5]) * 2 + (arrRegCoefY[i * 8 + 4] >> 4);
                    linCoef[i * 5 + 4] = (arrRegCoefY[i * 8 + 7]) * 8 + (arrRegCoefY[i * 8 + 6] >> 2);
                }
            }
            linCoef[23] = (arrRegCoefX[0]) * 2 + (arrRegCoefY[STATIC.NUM_COEF2_2 - 2] >> 4);
            linCoef[24] = (arrRegCoefY[37] & 1) * 128 + arrRegCoefX[1];
            linCoef[25] = (arrRegCoefY[37] >> 1) * 128 + arrRegCoefX[2];
            linCoef[26] = arrRegCoefX[3];

            //Error check
            for (int i = 0; i < 27; i++)
            {
                if (linCoef[i] < 0) { return 1; }
                if (linCoef[i] > 255) { return 1; }
            }

            return 0;
        }
        int PreData(ref ringBufLinComp lineData, int pVt, int nVt, int ignInf, int ignMac, ref float pPos, ref float nPos)
        {
            if (lineData.size < ignInf + ignMac + 6) { return 1; }
            for (int i = lineData.startIndex; i <= lineData.endIndex - 1; i++)
            {
                if (lineData.dataX[i] >= lineData.dataX[i + 1]) { return 1; }
            }

            lineData.startIndex += ignInf;
            lineData.size -= (ignInf + ignMac);
            lineData.endIndex = lineData.startIndex + lineData.size - 1;

            if (pVt > 511) { pPos = ((pVt - 1024) / 1024.0f + 0.5f); }
            else { pPos = (pVt / 1024.0f + 0.5f); }

            if (nVt > 511) { nPos = ((nVt - 1024) / 1024.0f - 0.5f); }
            else { nPos = (nVt / 1024.0f - 0.5f); }

            if (pPos == 0 && nPos == 0) { return 1; }

            return 0;
        }
        int ConvData(ringBufLinComp lineData, ref int posibit, float pPos, float nPos, float[] arrX13, float[] arrY13, float[] norX, float[] norY, float[] arrIDEAL, float[] arrIdeal, ref float maxX, ref float minX, ref float maxY, ref float minY)
        {
            float bitconv;

            // Resolution of Position
            if (lineData.dataX[lineData.endIndex] < AKM_Pow(2.0f, 10)) { posibit = 10; bitconv = AKM_Pow(2.0f, 13 - posibit); }
            else if (lineData.dataX[lineData.endIndex] < AKM_Pow(2.0f, 11)) { posibit = 11; bitconv = AKM_Pow(2.0f, 13 - posibit); }
            else if (lineData.dataX[lineData.endIndex] < AKM_Pow(2.0f, 12)) { posibit = 12; bitconv = AKM_Pow(2.0f, 13 - posibit); }
            else if (lineData.dataX[lineData.endIndex] < AKM_Pow(2.0f, 13)) { posibit = 13; bitconv = AKM_Pow(2.0f, 13 - posibit); }
            else if (lineData.dataX[lineData.endIndex] < AKM_Pow(2.0f, 14)) { posibit = 14; bitconv = 0.5f; }
            else { return 1; }

            float Lensmax = maxY;
            float Lensmin = minY;
            float[] dataX = new float[65];
            float[] dataY = new float[65];
            for (int i = lineData.startIndex; i < lineData.endIndex + 1; i++)
            {
                lineData.dataY[i] -= Lensmin;
            }
          
            maxX = lineData.dataX[lineData.endIndex] * bitconv;
            minX = lineData.dataX[lineData.startIndex] * bitconv;
            maxY = lineData.dataY[lineData.startIndex];
            minY = lineData.dataY[lineData.startIndex];
            for (int i = lineData.startIndex; i < lineData.endIndex + 1; i++)
            {
                dataX[i - lineData.startIndex] = lineData.dataX[i] * bitconv;
                dataY[i - lineData.startIndex] = lineData.dataY[i];
                if (maxY < lineData.dataY[i]) { maxY = lineData.dataY[i]; }
                if (minY > lineData.dataY[i]) { minY = lineData.dataY[i]; }
            }

            if (maxX == 0 && minX == 0) { return 1; }
            if (maxY == 0 && minY == 0) { return 1; }

            for (int i = 0; i < 65; i++)
            {
                arrY13[i] = (maxY - minY) / (65 - 1) * i + minY;
            }
            CalApprox(dataY, dataX, arrY13, arrX13, lineData.size, 65);

            maxY = Lensmax;
            minY = Lensmin;

            for (int i = 0; i < 65; i++)
            {
                norX[i] = nPos + arrX13[i] * (pPos - nPos) / AKM_Pow(2.0f, 13);
                norY[i] = nPos + arrY13[i] * (pPos - nPos) / (maxY - minY);
            }


            //--------------------------------------------------------------------------------------
            //	Ideal Data
            //--------------------------------------------------------------------------------------

            float SlopeIDEAL, InterceptIDEAL;
            float SlopeIdeal, InterceptIdeal;


            SlopeIDEAL = (lineData.dataY[lineData.endIndex] - lineData.dataY[lineData.startIndex]) / (maxX - minX);
            InterceptIDEAL = lineData.dataY[lineData.startIndex] - SlopeIDEAL * minX;

            SlopeIdeal = (norX[65 - 1] - norX[0]) / (norY[65 - 1] - norY[0]);
            InterceptIdeal = norX[0] - SlopeIdeal * norY[0];

            for (int i = 0; i < 65; i++)
            {
                arrIDEAL[i] = SlopeIDEAL * arrX13[i] + InterceptIDEAL;
                arrIdeal[i] = SlopeIdeal * norY[i] + InterceptIdeal;
            }

            return 0;


        }
        float AKM_Pow(float dbData, int num)
        {
            float pow = 1.0f;

            for (int i = 0; i < num; i++)
            {
                pow *= dbData;
            }

            return pow;
        }
        float AKM_Abs(float dbData)
        {
            float abs = dbData;

            if (dbData < 0) { abs = dbData * -1.0f; }

            return abs;
        }
        int CalApprox(float[] inputX, float[] inputY, float[] outputX, float[] outputY, int numData1, int numData2)
        {
            for (int i = 0; i < numData2; i++)
            {
                if (outputX[i] < inputX[0])
                {
                    int k = 0;
                    float slope = (inputY[k + 1] - inputY[k]) / (inputX[k + 1] - inputX[k]);
                    float offset = inputY[k] - slope * inputX[k];

                    outputY[i] = slope * outputX[i] + offset;
                }
                else
                {
                    for (int k = 0; k < numData1 - 1; k++)
                    {
                        if (inputX[k + 1] >= outputX[i] && inputX[k] <= outputX[i])
                        {

                            // Output error if there is dead zone
                            if (inputX[k + 1] == inputX[k]) { return 1; }

                            float slope = (inputY[k + 1] - inputY[k]) / (inputX[k + 1] - inputX[k]);
                            float offset = inputY[k] - slope * inputX[k];

                            outputY[i] = slope * outputX[i] + offset;
                            break;
                        }
                        else if (outputX[i] > inputX[numData1 - 1])
                        {
                            k = numData1 - 2;
                            float slope = (inputY[k + 1] - inputY[k]) / (inputX[k + 1] - inputX[k]);
                            float offset = inputY[k] - slope * inputX[k];

                            outputY[i] = slope * outputX[i] + offset;
                        }
                    }
                }
            }

            return 0;
        }

        int CalINL0(float[] arrX13, float[] arrY13, float[] norX, float[] norY, float[] arrIDEAL, float[] arrIdeal, float[] arrINL0, float[] arrInl0)
        {
            // -------------------------------------
            // Normalized Data 
            // -------------------------------------

            // Calculation of the difference value between the ideal value and the mesuared value when the target position to achieve a certain lens position
            for (int i = 0; i < 65; i++)
            {
                arrInl0[i] = norX[i] - arrIdeal[i];
            }

            // -------------------------------------
            // Target, Lens Position Data 
            // -------------------------------------

            // Calculation of the difference value between the ideal value and the mesuared value when the target position to achieve a certain lens position
            for (int i = 0; i < 65; i++)
            {
                arrINL0[i] = arrIDEAL[i] - arrY13[i];
            }

            return 0;
        }
        int CalINL2(float[] arrX13, float[] arrY13, float[] norX, float[] norY, float[] arrIDEAL, float[] arrIdeal, float[] arrINL2, float[] arrLin2, float[] arrInl2, int[] arrRegCoef2, float pPos, float nPos, float maxX, float minX, float maxY, float minY, int INL)
        {
            float[] arrRegAbsCoef2 = new float[38];

            // Normalized arrCoef2_1
            for (int i = 0; i < 35; i++)
            {
                sarrNorCoef2[i] = arrRegCoef2[i] / AKM_Pow(2.0f, 7) * (pPos - nPos) + nPos;
            }

            // Signed arrCoef2_2 : YB1
            for (int i = 0; i < 38 - 1; i++)
            {
                if (arrRegCoef2[35 + i] > AKM_Pow(2.0f, 4) - 1) { arrRegAbsCoef2[i] = (arrRegCoef2[35 + i] - 32) / AKM_Pow(2.0f, 4); }
                else { arrRegAbsCoef2[i] = arrRegCoef2[35 + i] / AKM_Pow(2.0f, 4); }
            }

            // Incremental to Absolute arrCoef2_2: YSUM
            sarrNorCoef2[35] = arrRegAbsCoef2[0];
            for (int i = 1; i < 38 - 1; i++)
            {
                sarrNorCoef2[35 + i] = sarrNorCoef2[35 + i - 1] + arrRegAbsCoef2[i];
            }

            sarrNorCoef2[35 + 38 - 1] = AKM_Pow(2.0f, arrRegCoef2[35 + 38 - 1]) * 0.5f;

            // Calclation of the compensation amount2 


            CalLinGain2(norY, arrRegAbsCoef2, sarrNorCoef2, pPos, nPos, arrLin2);

            for (int i = 0; i < 65; i++)
            {
                snorXLNC[i] = norX[i] - arrLin2[i];
                sarrX13LNC[i] = (snorXLNC[i] - nPos) * AKM_Pow(2.0f, 13) / (pPos - nPos);
            }

            // -------------------------------------
            // Normalized Data 
            // -------------------------------------

            // Calculation of the INL after LNC2 at Normalized Target data
            for (int i = 0; i < 65; i++)
            {
                arrInl2[i] = snorXLNC[i] - arrIdeal[i];
            }

            // -------------------------------------
            // Target, Lens Position Data 
            // -------------------------------------

            // Caliculation of Lens data after LNC2
            if (INL == 1)
            {
                CalApprox(sarrX13LNC, arrY13, arrX13, sarrY13LNC, 65, 65);
                for (int i = 0; i < 65; i++)
                {
                    arrINL2[i] = arrIDEAL[i] - sarrY13LNC[i];
                }
            }

            return 0;
        }
        int CalLinGain2(float[] inputX, float[] YB1, float[] arrCoef2, float pPos, float nPos, float[] arrLin2)
        {
            int k = 0;
            for (int i = 0; i < 65; i++)
            {

                switch (k)
                {
                    case 0:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35] + YB1[1] / (arrCoef2[0] - nPos) * (inputX[i] - nPos);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 1:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 2:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 3:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 4:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 5:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 6:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 7:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 8:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 9:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 10:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 11:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 12:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 13:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 14:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 15:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 16:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 17:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 18:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 19:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 20:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 21:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 22:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 23:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 24:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 25:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 26:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 27:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 28:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 29:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 30:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 31:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 32:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 2])
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 3;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        break;

                    case 33:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else if (inputX[i] <= arrCoef2[k + 1])
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 2;
                            arrLin2[i] = arrCoef2[35 + 38 - 3] + YB1[36] / (pPos - arrCoef2[35 - 1]) * (inputX[i] - arrCoef2[35 - 1]);
                        }
                        break;

                    case 34:
                        if (inputX[i] <= arrCoef2[k])
                        {
                            arrLin2[i] = arrCoef2[35 + k] + YB1[k + 1] / (arrCoef2[k] - arrCoef2[k - 1]) * (inputX[i] - arrCoef2[k - 1]);
                        }
                        else
                        {
                            k += 1;
                            arrLin2[i] = arrCoef2[35 + 38 - 3] + YB1[36] / (pPos - arrCoef2[35 - 1]) * (inputX[i] - arrCoef2[35 - 1]);
                        }
                        break;

                    default:
                        arrLin2[i] = arrCoef2[35 + 38 - 3] + YB1[36] / (pPos - arrCoef2[35 - 1]) * (inputX[i] - arrCoef2[35 - 1]);
                        break;
                }

                arrLin2[i] *= arrCoef2[35 + 38 - 1] / AKM_Pow(2.0f, 6);

            }

            return 0;
        }
        int CalLinCompCoef2(float[] norX, float[] arrINL0, float[] norY, float[] arrX13, float[] arrY13, float pPos, float nPos, float maxX, float minX, float maxY, float minY, float[] arrIDEAL, float[] arrIdeal, int[] arrRegCoef2, float[] arrINL2, float[] arrLin2, float[] arrInl2)
        {
            // Candidate
            int NUM_INT = (65 - 1) / (STATIC.OptXFix + 1);
            int NUM_INT2 = 256 / NUM_INT;
            float[] arrExtremX = new float[STATIC.NUM_COEF2_1];
            int[] ExtremXRank = new int[STATIC.NUM_COEF2_1];
            int OptXNum = STATIC.NUM_COEF2_1 - STATIC.OptXFix;

            // Slope
            float[] SlopeINL = new float[STATIC.MAX_NUM_DATA];
            float[] SlopeVal = new float[STATIC.MAX_NUM_DATA];
            float[] SlopeValCoef = new float[STATIC.OptXCan];
            int[] XCoef = new int[STATIC.OptXCan];
            int[] SlopeValRank = new int[STATIC.OptXCan];

            float[] arrNorCoef2X = new float[STATIC.NUM_COEF2_2];
            float[] arrNorCoef2Y = new float[STATIC.NUM_COEF2_2];

            //Initialization
            for (int i = 0; i < STATIC.NUM_COEF2_1; i++)
            {
                arrExtremX[i] = 0;
            }
            for (int i = 0; i < STATIC.OptXCan; i++)
            {
                SlopeValRank[i] = 100;
            }

            // modulate X param: Fix point
            for (int i = 0; i < STATIC.OptXFix; i++)
            {
                arrExtremX[i] = (float)(NUM_INT * (i + 1));
            }
            //------------------

            if (STATIC.OptParamX == 1)
            {
                //Slope judge ------------------------
                // Calculation of Slope
                for (int i = 0; i < STATIC.MAX_NUM_DATA - 1; i++)
                {
                    SlopeINL[i] = (arrINL0[i + 1] - arrINL0[i]) / (arrX13[i + 1] - arrX13[i]);
                }

                // Calculation of Slope Valiation
                for (int i = 0; i < STATIC.MAX_NUM_DATA - 2; i++)
                {
                    SlopeVal[i] = AKM_Abs(SlopeINL[i + 1] - SlopeINL[i]);
                }

                // Slope Vaulation Ranking
                for (int i = 0; i < STATIC.OptXCan; i++)
                {
                    XCoef[i] = NUM_INT * i + 2;
                    SlopeValCoef[i] = SlopeVal[XCoef[i]];
                }

                // Avoiding equal ranking
                for (int i = 0; i < STATIC.OptXCan; i++)
                {
                    SlopeValCoef[i] += i * 0.00000001f;
                }

                Rank(STATIC.OptXCan, SlopeValCoef, 1, SlopeValRank);

                // Sort SlopeValRank
                for (int rank = 0; rank < OptXNum; rank++)
                {
                    for (int i = 0; i < STATIC.OptXCan; i++)
                    {
                        if (SlopeValRank[i] == rank)
                        {
                            arrExtremX[rank + STATIC.NUM_COEF2_1 - OptXNum] = (float)XCoef[i];
                        }
                    }
                }
                //---------------------------------

            }
            else
            {

                //Error variation between Xfix
                int[] sint = new int[STATIC.OptXFix + 2];
                sint[0] = 0;
                sint[STATIC.OptXFix + 1] = STATIC.MAX_NUM_DATA - 1;

                for (int i = 0; i < STATIC.OptXFix; i++)
                {
                    sint[i + 1] = (int)arrExtremX[i];
                }

                for (int i = 0; i < STATIC.OptXFix + 1; i++)
                {
                    SlopeVal[i] = AKM_Abs((arrINL0[sint[i + 1]] - arrINL0[sint[i]]));
                }

                // Avoiding equal ranking
                for (int i = 0; i < STATIC.OptXCan; i++)
                {
                    SlopeVal[i] += i * 0.00000001f;
                }

                Rank(STATIC.OptXFix + 1, SlopeVal, 1, SlopeValRank);

                // Sort SlopeValRank
                for (int rank = 0; rank < OptXNum; rank++)
                {
                    for (int i = 0; i < STATIC.OptXFix + 1; i++)
                    {
                        if (SlopeValRank[i] == rank)
                        {
                            arrExtremX[rank + STATIC.NUM_COEF2_1 - OptXNum] = sint[i] + NUM_INT * 0.5f;
                        }
                    }
                }
            }

            // Sort Extreme X Value
            int s = 0;
            Rank(STATIC.NUM_COEF2_1, arrExtremX, 0, ExtremXRank);
            for (int rank = 0; rank < STATIC.NUM_COEF2_1; rank++)
            {
                for (int i = 0; i < STATIC.NUM_COEF2_1; i++)
                {
                    if (ExtremXRank[i] == rank)
                    {
                        s = (int)arrExtremX[i];
                        sarrCoef2[rank] = (float)(s * NUM_INT2);
                    }
                }
            }

            //Xparam bit -> nor
            arrNorCoef2X[0] = nPos;
            arrNorCoef2X[STATIC.NUM_COEF2_1 + 1] = pPos;

            for (int i = 0; i < STATIC.NUM_COEF2_1; i++)
            {
                arrNorCoef2X[i + 1] = nPos + sarrCoef2[i] / AKM_Pow(2.0f, 13) * (pPos - nPos);
            }

            if (STATIC.VT == 0) { CalApprox(norX, arrINL0, arrNorCoef2X, arrNorCoef2Y, STATIC.MAX_NUM_DATA, STATIC.NUM_COEF2_1 + 2); }
            if (STATIC.VT == 1) { CalApprox(norY, arrINL0, arrNorCoef2X, arrNorCoef2Y, STATIC.MAX_NUM_DATA, STATIC.NUM_COEF2_1 + 2); }

            //Yparam nor -> 10bit code 
            for (int i = 0; i < STATIC.NUM_COEF2_2; i++)
            {
                sarrCoef2[i + STATIC.NUM_COEF2_1] = arrNorCoef2Y[i] / (pPos - nPos) * AKM_Pow(2.0f, 10);
            }

            int[] arrRegCoef2Sub = new int[STATIC.NUM_COEF2_2];
            if (CheckParam2(sarrCoef2, arrRegCoef2, arrRegCoef2Sub) != 0) { return 1; }

            // Optimize Y parameter
            if (STATIC.OptALL == 1)
            {
                float[] ResError = new float[STATIC.OptALLTimes + 10];
                float ResError0 = 0;
                int[] min = new int[STATIC.OptALLNum + 1];
                int[] arrRegCoef2Opt = new int[STATIC.NUM_COEF2_2];
                int Gain = 0;
                int Sametime = 0;
                ResError[0] = 0.0f;

                CalINL2(arrX13, arrY13, norX, norY, arrIDEAL, arrIdeal, arrINL2, arrLin2, arrInl2, arrRegCoef2, pPos, nPos, 0, 0, 0, 0, 0);

                for (int i = 1; i < STATIC.OptALLTimes + 1; i++)
                {
                    OptParamSeg(arrX13, arrY13, norX, norY, arrIDEAL, arrIdeal, arrINL2, arrLin2, arrInl2, min, arrRegCoef2, pPos, nPos);
                    CalINL2(arrX13, arrY13, norX, norY, arrIDEAL, arrIdeal, arrINL2, arrLin2, arrInl2, arrRegCoef2, pPos, nPos, 0, 0, 0, 0, 0);

                    // Check residual error after OptParamAll
                    ResError[i] = 0.0f;
                    for (int m = 0; m < STATIC.MAX_NUM_DATA; m++)
                    {
                        if (AKM_Abs(arrInl2[m]) > ResError[i]) { ResError[i] = AKM_Abs(arrInl2[m]); }
                    }
                    if (ResError[i] < STATIC.ERROR_TH / (maxY - minY) * (pPos - nPos))
                    {
                        break;
                    }

                    if (ResError[i] == ResError[i - 1])
                    {
                        Sametime += 1;
                    }
                    if (Sametime == 10)
                    {
                        int maxReg = 0;
                        int minReg = 0;
                        for (int j = 0; j < STATIC.OptALLNum + 1; j++)
                        {
                            if (min[j] > maxReg) { maxReg = min[j]; }
                            if (min[j] < minReg) { minReg = min[j]; }
                        }

                        //Gain Adjustment
                        if (Gain == 0 && (maxReg > 11 || minReg < -12) && arrRegCoef2[STATIC.NUM_COEF2_1 + STATIC.NUM_COEF2_2 - 1] < 3)
                        {
                            ResError0 = ResError[i];
                            Gain = 1;
                            Sametime = 0;
                            for (int j = 0; j < STATIC.NUM_COEF2_2; j++)
                            {
                                arrRegCoef2Opt[j] = arrRegCoef2[STATIC.NUM_COEF2_1 + j];
                                arrRegCoef2[STATIC.NUM_COEF2_1 + j] = arrRegCoef2Sub[j];
                            }
                        }
                        else
                        {
                            if (Gain == 1)
                            {
                                if (ResError[i] > ResError0)
                                {
                                    for (int j = 0; j < STATIC.NUM_COEF2_2; j++)
                                    {
                                        arrRegCoef2[STATIC.NUM_COEF2_1 + j] = arrRegCoef2Opt[j];
                                    }
                                }
                            }
                            break;
                        }
                    }

                    if (i == STATIC.OptALLTimes && Gain == 1)
                    {
                        if (ResError[i] > ResError0)
                        {
                            for (int j = 0; j < STATIC.NUM_COEF2_2; j++)
                            {
                                arrRegCoef2[STATIC.NUM_COEF2_1 + j] = arrRegCoef2Opt[j];
                            }
                        }
                    }
                }

            }

            return 0;
        }

        void Rank(int num, float[] dbData, int direction, int[] dbDataRank)
        {
            for (int i = 0; i < num; i++)
            {
                dbDataRank[i] = 0;
            }

            if (direction == 0)
            {
                for (int i = 1; i < num; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (dbData[j] > dbData[i]) { dbDataRank[j]++; }
                        if (dbData[j] < dbData[i]) { dbDataRank[i]++; }
                    }
                }
            }
            if (direction == 1)
            {
                for (int i = 1; i < num; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (dbData[j] < dbData[i]) { dbDataRank[j]++; }
                        if (dbData[j] > dbData[i]) { dbDataRank[i]++; }
                    }
                }
            }

        }
        int CheckParam2(float[] arrCoef2, int[] arrRegCoef2, int[] arrRegCoef2Sub)
        {
            int maxReg;
            int minReg;
            int[] arrRegAbsCoef2 = new int[STATIC.NUM_COEF2_2];

            // Adjustment approximation coefficients including LinGainP
            for (int i = 0; i < AKM_Pow(2.0f, STATIC.BIT_COEF2G); i++)
            {
                arrCoef2[STATIC.NUM_COEF2_1 + STATIC.NUM_COEF2_2 - 1] = 0.5f * AKM_Pow(2.0f, i);
                arrRegCoef2[STATIC.NUM_COEF2_1 + STATIC.NUM_COEF2_2 - 1] = i;
                maxReg = 0;
                minReg = 0;

                for (int j = 0; j < STATIC.NUM_COEF2_2 - 1; j++)
                {
                    arrRegAbsCoef2[j] = (int)(arrCoef2[STATIC.NUM_COEF2_1 + j] / arrCoef2[STATIC.NUM_COEF2_1 + STATIC.NUM_COEF2_2 - 1]);
                }

                //Incremental
                arrRegCoef2[STATIC.NUM_COEF2_1] = arrRegAbsCoef2[0];
                for (int j = 1; j < STATIC.NUM_COEF2_2 - 1; j++)
                {
                    arrRegCoef2[STATIC.NUM_COEF2_1 + j] = arrRegAbsCoef2[j] - arrRegAbsCoef2[j - 1];
                    if (arrRegCoef2[STATIC.NUM_COEF2_1 + j] > maxReg) { maxReg = arrRegCoef2[STATIC.NUM_COEF2_1 + j]; }
                    if (arrRegCoef2[STATIC.NUM_COEF2_1 + j] < minReg) { minReg = arrRegCoef2[STATIC.NUM_COEF2_1 + j]; }
                }
                if (minReg > -17 && maxReg < 16)
                {
                    if (i < AKM_Pow(2.0f, STATIC.BIT_COEF2G) - 1)
                    {
                        arrCoef2[STATIC.NUM_COEF2_1 + STATIC.NUM_COEF2_2 - 1] = 0.5f * AKM_Pow(2.0f, i + 1);
                        arrRegCoef2Sub[STATIC.NUM_COEF2_2 - 1] = i + 1;

                        for (int j = 0; j < STATIC.NUM_COEF2_2 - 1; j++)
                        {
                            arrRegAbsCoef2[j] = (int)(arrCoef2[STATIC.NUM_COEF2_1 + j] / arrCoef2[STATIC.NUM_COEF2_1 + STATIC.NUM_COEF2_2 - 1]);
                        }
                        //Incremental
                        arrRegCoef2Sub[0] = arrRegAbsCoef2[0];
                        for (int j = 1; j < STATIC.NUM_COEF2_2 - 1; j++)
                        {
                            arrRegCoef2Sub[j] = arrRegAbsCoef2[j] - arrRegAbsCoef2[j - 1];
                        }
                    }
                    break;
                }
                else if (i == AKM_Pow(2.0f, STATIC.BIT_COEF2G) - 1)
                {
                    int adjustVal = 0;
                    for (int j = 1; j < STATIC.NUM_COEF2_2 - 1; j++)
                    {
                        if (arrRegCoef2[STATIC.NUM_COEF2_1 + j] > 15)
                        {
                            adjustVal = arrRegCoef2[STATIC.NUM_COEF2_1 + j] - 15;
                            arrRegCoef2[STATIC.NUM_COEF2_1 + j] = 15;
                            if (j < STATIC.NUM_COEF2_2 - 2)
                            {
                                arrRegCoef2[STATIC.NUM_COEF2_1 + j + 1] += adjustVal;
                            }
                        }
                        else if (arrRegCoef2[STATIC.NUM_COEF2_1 + j] < -16)
                        {
                            adjustVal = arrRegCoef2[STATIC.NUM_COEF2_1 + j] + 16;
                            arrRegCoef2[STATIC.NUM_COEF2_1 + j] = -16;
                            if (j < STATIC.NUM_COEF2_2 - 2)
                            {
                                arrRegCoef2[STATIC.NUM_COEF2_1 + j + 1] += adjustVal;
                            }
                        }
                    }
                }
            }

            // Calculation of Register value
            for (int i = 0; i < STATIC.NUM_COEF2_1; i++)
            {
                arrRegCoef2[i] = AKM_Round(arrCoef2[i] / AKM_Pow(2.0f, 13) * AKM_Pow(2.0f, 7));
            }

            for (int i = 0; i < STATIC.NUM_COEF2_2 - 1; i++)
            {
                if (arrRegCoef2[STATIC.NUM_COEF2_1 + i] < 0) { arrRegCoef2[STATIC.NUM_COEF2_1 + i] += 32; }
                if (arrRegCoef2Sub[i] < 0) { arrRegCoef2Sub[i] += 32; }
            }

            return 0;
        }
       

        int AKM_Round(float dbData)
        {
            float round = 0;

            if (dbData > 0) { round = dbData + 0.5f; }
            else if (dbData < 0) { round = dbData - 0.5f; }

            return (int)round;
        }
        int OptParamSeg(float[] arrX13, float[] arrY13, float[] norX, float[] norY, float[] arrIDEAL, float[] arrIdeal, float[] arrINL2, float[] arrLin2, float[] arrInl2, int[] min, int[] arrRegCoef2, float pPos, float nPos)
        {
            float resError = STATIC.PRE_ERROR;
            float resIntError = STATIC.PRE_ERROR;
            float ResErrorIni = 0.0f;
            float ResError = 0.0f;
            float ResIntError = 0.0f;
            int ResErrorSeg = 0;
            float[] ResINLSegabs = new float[STATIC.NUM_COEF2_1 + 1];
            float[] arrNorCoef2X = new float[STATIC.NUM_COEF2_2];
            int[] arrRegIniCoef2 = new int[STATIC.NUM_COEF2_2];
            int[] arrRegSigCoef2 = new int[STATIC.NUM_COEF2_2];
            int[] OptX = new int[STATIC.OptALLNum];
            int[] yIni = new int[STATIC.OptALLNum + 1];
            int[] Int = new int[STATIC.OptALLNum];

            // resualINL each Segment
            for (int i = 0; i < STATIC.NUM_COEF2_1 + 1; i++) { ResINLSegabs[i] = 0.0f; }

            for (int i = 0; i < STATIC.NUM_COEF2_1; i++)
            {
                arrNorCoef2X[i] = nPos + arrRegCoef2[i] / AKM_Pow(2.0f, 7) * (pPos - nPos);
            }

            if (STATIC.VT == 0)
            {
                for (int i = 0; i < STATIC.MAX_NUM_DATA; i++)
                {
                    if (norX[i] < arrNorCoef2X[0])
                    {
                        int k = 0;
                        if (AKM_Abs(arrInl2[i]) > ResINLSegabs[k]) { ResINLSegabs[k] = AKM_Abs(arrInl2[i]); }
                    }
                    else
                    {
                        for (int k = 1; k < STATIC.NUM_COEF2_1; k++)
                        {
                            if (norX[i] == arrNorCoef2X[k - 1] || (norX[i] > arrNorCoef2X[k - 1] && norX[i] < arrNorCoef2X[k]))
                            {
                                if (AKM_Abs(arrInl2[i]) > ResINLSegabs[k]) { ResINLSegabs[k] = AKM_Abs(arrInl2[i]); }
                                break;
                            }
                            else if (norX[i] >= arrNorCoef2X[STATIC.NUM_COEF2_1 - 1])
                            {
                                k = STATIC.NUM_COEF2_1;
                                if (AKM_Abs(arrInl2[i]) > ResINLSegabs[k]) { ResINLSegabs[k] = AKM_Abs(arrInl2[i]); }
                            }
                        }
                    }
                }
            }

            if (STATIC.VT == 1)
            {
                for (int i = 0; i < STATIC.MAX_NUM_DATA; i++)
                {
                    if (norY[i] < arrNorCoef2X[0])
                    {
                        int k = 0;
                        if (AKM_Abs(arrInl2[i]) > ResINLSegabs[k]) { ResINLSegabs[k] = AKM_Abs(arrInl2[i]); }
                    }
                    else
                    {
                        for (int k = 1; k < STATIC.NUM_COEF2_1; k++)
                        {
                            if (norY[i] == arrNorCoef2X[k - 1] || (norY[i] > arrNorCoef2X[k - 1] && norY[i] < arrNorCoef2X[k]))
                            {
                                if (AKM_Abs(arrInl2[i]) > ResINLSegabs[k]) { ResINLSegabs[k] = AKM_Abs(arrInl2[i]); }
                                break;
                            }
                            else if (norY[i] >= arrNorCoef2X[STATIC.NUM_COEF2_1 - 1])
                            {
                                k = STATIC.NUM_COEF2_1;
                                if (AKM_Abs(arrInl2[i]) > ResINLSegabs[k]) { ResINLSegabs[k] = AKM_Abs(arrInl2[i]); }
                            }
                        }
                    }
                }
            }

            //Max, Min INL Segment
            for (int i = 0; i < STATIC.NUM_COEF2_1 + 1; i++)
            {
                if (ResINLSegabs[i] > ResErrorIni)
                {
                    ResErrorIni = ResINLSegabs[i];
                    ResErrorSeg = i;
                }
            }

            //Xpoint of adjust Yparam 
            OptX[0] = ResErrorSeg;
            OptX[1] = ResErrorSeg + 1;

            // Signed arrCoef2_2
            for (int i = 0; i < STATIC.NUM_COEF2_2 - 1; i++)
            {
                arrRegIniCoef2[i] = arrRegCoef2[STATIC.NUM_COEF2_1 + i];
                if (arrRegCoef2[STATIC.NUM_COEF2_1 + i] > AKM_Pow(2.0f, 4) - 1) { arrRegSigCoef2[i] = (arrRegCoef2[STATIC.NUM_COEF2_1 + i] - 32); }
                else { arrRegSigCoef2[i] = arrRegCoef2[STATIC.NUM_COEF2_1 + i]; }
            }

            yIni[0] = (int)(arrRegSigCoef2[OptX[0]] - STATIC.RANGE * 0.5f);
            if (yIni[0] < -16) { yIni[0] = -16; }
            yIni[1] = arrRegSigCoef2[OptX[1]];

            if (ResErrorSeg < STATIC.NUM_COEF2_1)
            {
                yIni[STATIC.OptALLNum] = arrRegSigCoef2[OptX[1] + 1] + STATIC.RANGE / 2;
                if (yIni[STATIC.OptALLNum] > 15) { yIni[STATIC.OptALLNum] = 15; }
            }

            //Optimized Parameter Search
            for (Int[0] = 0; Int[0] < STATIC.RANGE; Int[0]++)
            {
                arrRegSigCoef2[OptX[0]] = yIni[0] + Int[0];
                if (arrRegSigCoef2[OptX[0]] > 15) { arrRegSigCoef2[OptX[0]] = 15; }

                for (Int[1] = 0; Int[1] < STATIC.RANGE; Int[1]++)
                {
                    arrRegSigCoef2[OptX[1]] = yIni[1] + Int[1] - Int[0];
                    if (arrRegSigCoef2[OptX[1]] > 15) { arrRegSigCoef2[OptX[1]] = 15; }
                    if (arrRegSigCoef2[OptX[1]] < -16) { arrRegSigCoef2[OptX[1]] = -16; }

                    if (ResErrorSeg < STATIC.NUM_COEF2_1 - 1)
                    {
                        arrRegSigCoef2[OptX[1] + 1] = yIni[STATIC.OptALLNum] - Int[1];
                        if (arrRegSigCoef2[OptX[1] + 1] < -16) { arrRegSigCoef2[OptX[1] + 1] = -16; }
                    }

                    //Y param Unsiged
                    for (int i = 0; i < STATIC.NUM_COEF2_2 - 1; i++)
                    {
                        if (arrRegSigCoef2[i] < 0) { arrRegCoef2[STATIC.NUM_COEF2_1 + i] = arrRegSigCoef2[i] + 32; }
                        else { arrRegCoef2[STATIC.NUM_COEF2_1 + i] = arrRegSigCoef2[i]; }
                    }

                    CalINL2(arrX13, arrY13, norX, norY, arrIDEAL, arrIdeal, arrINL2, arrLin2, arrInl2, arrRegCoef2, pPos, nPos, 0, 0, 0, 0, 0);

                    ResError = 0.0f;
                    ResIntError = 0.0f;

                    //Error check for All Segmet
                    for (int i = 0; i < STATIC.MAX_NUM_DATA; i++)
                    {
                        ResIntError += AKM_Abs(arrInl2[i]);
                        if (AKM_Abs(arrInl2[i]) > ResError) { ResError = AKM_Abs(arrInl2[i]); }
                    }

                    if (ResError < resError)
                    {
                        resError = ResError;
                        resIntError = ResIntError;

                        for (int i = 0; i < STATIC.OptALLNum; i++)
                        {
                            min[i] = arrRegSigCoef2[OptX[i]];

                            if (ResErrorSeg < STATIC.NUM_COEF2_1 - 1)
                            {
                                min[STATIC.OptALLNum] = arrRegSigCoef2[OptX[1] + 1];
                            }
                        }
                    }

                    if (ResError == resError)
                    {
                        if (ResIntError < resIntError)
                        {
                            resIntError = ResIntError;

                            for (int i = 0; i < STATIC.OptALLNum; i++)
                            {
                                min[i] = arrRegSigCoef2[OptX[i]];

                                if (ResErrorSeg < STATIC.NUM_COEF2_1 - 1)
                                {
                                    min[STATIC.OptALLNum] = arrRegSigCoef2[OptX[1] + 1];
                                }
                            }
                        }
                    }
                }
            }

            if (resError > ResErrorIni)
            {
                for (int i = 0; i < STATIC.NUM_COEF2_2 - 1; i++)
                {
                    arrRegCoef2[STATIC.NUM_COEF2_1 + i] = arrRegIniCoef2[i];
                }
            }
            else
            {
                arrRegSigCoef2[OptX[0]] = min[0];
                arrRegSigCoef2[OptX[1]] = min[1];
                if (arrRegSigCoef2[OptX[0]] > 15) { arrRegSigCoef2[OptX[0]] = 15; }
                if (arrRegSigCoef2[OptX[1]] > 15) { arrRegSigCoef2[OptX[1]] = 15; }
                if (arrRegSigCoef2[OptX[1]] < -16) { arrRegSigCoef2[OptX[1]] = -16; }

                if (ResErrorSeg < STATIC.NUM_COEF2_1 - 1)
                {
                    arrRegSigCoef2[OptX[1] + 1] = min[STATIC.OptALLNum];
                    if (arrRegSigCoef2[OptX[1] + 1] < -16) { arrRegSigCoef2[OptX[1] + 1] = -16; }
                }

                //Y param Unsiged
                for (int i = 0; i < STATIC.NUM_COEF2_2 - 1; i++)
                {
                    if (arrRegSigCoef2[i] < 0) { arrRegCoef2[STATIC.NUM_COEF2_1 + i] = arrRegSigCoef2[i] + 32; }
                    else { arrRegCoef2[STATIC.NUM_COEF2_1 + i] = arrRegSigCoef2[i]; }
                }

            }

            return 0;
        }
        #endregion
        #region FindCover
        public double BinInterpolation(ref byte[] array, double px, double py)
        {
            try
            {
                int width = STATIC.DefaultWidth;
                double res = 0;

                //  주변 4점을 이용한 Lagrange Interpolated Bin 값
                int ix = (int)px;
                int iy = (int)py;
                double u = px - ix;
                double v = py - iy;

                int ix_iy_m_FOV_Width = ix + iy * width;

                res = (array[ix + iy * width] + array[ix - 1 + iy * width] + array[ix - 1 + (iy - 1) * width] + array[ix + (iy - 1) * width]) * (1 - u) * (1 - v)
                    + (array[ix + (iy + 1) * width] + array[ix - 1 + (iy + 1) * width] + array[ix + (iy + 2) * width] + array[ix + 1 + (iy + 2) * width]) * (1 - u) * v
                    + (array[(ix + 1) + iy * width] + array[(ix + 2) + iy * width] + array[(ix + 2) + (iy - 1) * width] + array[(ix + 1) + (iy - 1) * width]) * u * (1 - v)
                    + (array[(ix + 1) + (iy + 1) * width] + array[(ix + 2) + (iy + 1) * width] + array[(ix + 2) + (iy + 2) * width] + array[(ix + 1) + (iy + 2) * width]) * u * v;
                //res = array[ix + iy * m_FOV_Width]              * (1 - u) * (1 - v)
                //    + array[ix + (iy + 1) * m_FOV_Width]        * (1 - u) * v
                //    + array[(ix + 1) + iy * m_FOV_Width]        * u * (1 - v)
                //    + array[(ix + 1) + (iy + 1) * m_FOV_Width]  * u * v;

                return res / 4;

            }
            catch
            {
                return -1;
            }

        }

        public int EdgeOfDecenterCheckArea(ref byte[] array, ref double[] ex, ref double[] ey, double start, double end)
        {
            //  ex[9998], ey[9998] :  center of left open
            //  ex[9999], ey[9999] :  center of right open
            int i = 0;
            int k = 0;

            double px1 = 0;
            double py1 = 0;
            double px2 = 0;
            double py2 = 0;
            double theta = 0;
            double sin = 0;
            double cos = 0;
            double[] diff = new double[2];
            double maxSlope = 0;
            double slope = 0;
            double r_prev = 0;
         
       

            double cx = ex[9998];
            double cy = ey[9998];
            if (cx > 0 && cy > 0)
            {
                for (i = 0; i < 960; i++)
                {
                    //  중심에서 반경방향으로 0.05deg 단위로 돌아가면서 기울기 최대값지점의 좌표를 linear interpolation 해서 찾아 저장한다.
                    theta = i / 480.0 * Math.PI;
                    sin = Math.Sin(theta);
                    cos = Math.Cos(theta);
                    maxSlope = 0;
                    for (double r = start; r < end; r += 0.5)
                    {
                        px1 = cx + r * cos;
                        py1 = cy + r * sin;
                        if (py1 < 3 || py1 > 338)
                            continue;

                        diff[0] = BinInterpolation(ref array, px1, py1);
                        px2 = px1 + 1.5 * cos;
                        py2 = py1 + 1.5 * sin;
                        diff[1] = BinInterpolation(ref array, px2, py2);


                        if (diff[0] < 80 && diff[1] < 80) //  이미 외측으로 나간 경우
                            break;
                    
                        slope = diff[0] - diff[1];
                        if (slope > maxSlope)
                        {
                            maxSlope = slope;
                            ex[k] = (px1 + px2) / 2;
                            ey[k] = (py1 + py2) / 2;
                            r_prev = r;
                        }
                    }
                  
                    if (px1 < 155 && (py1 < 175 && py1 > 165))
                        k++;
                    else
                        k++;
                }
            }
 
            return k;
        }
        public int EdgeOfCover(ref byte[] array, ref double[] ex, ref double[] ey, bool IsManual = true)
        {
            //  ex[9998], ey[9998] :  center of left open
            //  ex[9999], ey[9999] :  center of right open
            int i = 0;
            int k = 0;

            double px1 = 0;
            double py1 = 0;
            double px2 = 0;
            double py2 = 0;
            double theta = 0;
            double sin = 0;
            double cos = 0;
            double[] diff = new double[2];
            double maxSlope = 0;
            double slope = 0;
            int r_start = 0;
            int r_end = 0;
            r_start = 600;  //  제품 대조리개 반경은 118~ 119
            r_end = 500;    //  제품 커버원 반경은 138~140
            //switch (STATIC.Rcp.Model.ModelName)
            //{
            //    case "SO1G73":
            //        r_start = 750;  //  제품 대조리개 반경은 118~ 119
            //        r_end = 830;    //  제품 커버원 반경은 138~140
            //        break;
            //    case "SO1C81":

            //        r_start = 600;  //  제품 대조리개 반경은 118~ 119
            //        r_end = 500;    //  제품 커버원 반경은 138~140
            //        break;
            //}
            double r = 0;
            double cx = ex[9999];
            double cy = ey[9999];
            cx = 750; cy = 750;
            if (cx > 0 && cy > 0)
            {
                try
                {
                    for (i = 0; i < 360; i++)
                    {
                        //  중심에서 반경방향으로 0.05deg 단위로 돌아가면서 기울기 최대값지점의 좌표를 linear interpolation 해서 찾아 저장한다.
                        theta = i / 180.0 * Math.PI;
                        sin = Math.Sin(theta);
                        cos = Math.Cos(theta);
                        maxSlope = 0;
                        for (r = r_start; r > r_end; r -= 0.5)
                        {
                            px1 = cx + r * cos;
                            py1 = cy + r * sin;
                            if (py1 >= STATIC.DefaultHeight - 1 || py1 < 3)
                                continue;

                            diff[0] = BinInterpolation(ref array, px1, py1);    //  외측
                                                                                //px2 = cx + (r - 2) * cos;
                                                                                //py2 = cy + (r - 2) * sin;
                            px2 = px1 - 2 * cos;
                            py2 = py1 - 2 * sin;
                            diff[1] = BinInterpolation(ref array, px2, py2);    //  내측

                            if (diff[1] > diff[0] - 3) // 외측이 밝고 내측이 어두운 경우
                            {
                                r--;
                                continue;
                            }
                            slope = diff[0] - diff[1]; // 외측이 밝고 내측이 어두운 경우
                       //     break;
                            //switch (STATIC.Rcp.Model.ModelName)
                            //{
                            //    //case "SO1G73":
                            //    //case "SO1C81":

                            //    //    if (diff[0] > diff[1] - 3)  //  내측이 밝고 외측이 어두워야 한다. 그러므로 외측이 밝은 경우 통과.
                            //    //    {
                            //    //        r--;
                            //    //        continue;
                            //    //    }
                            //    //    slope = diff[1] - diff[0];
                            //    //    break;
                            //    default:
                            //        if (diff[1] > diff[0] - 3) // 외측이 밝고 내측이 어두운 경우
                            //        {
                            //            r--;
                            //            continue;
                            //        }
                            //        slope = diff[0] - diff[1]; // 외측이 밝고 내측이 어두운 경우
                            //        break;
                            //}
                            if (slope > maxSlope)
                            {
                                maxSlope = slope;
                                ex[k] = (px1 + px2) / 2;
                                ey[k] = (py1 + py2) / 2;
                            }
                        }
                        k++;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error in EdgeOfCover() : " + e.ToString());
                }
            }


            //StreamWriter wr = new StreamWriter("CoverEdge.txt");
            //for (i = 0; i < k; i++)
            //    wr.WriteLine(i.ToString() + "]=\t" + ex[i].ToString("F1") + "\t" + ey[i].ToString("F1"));

            //wr.Close();
            return k;
        }
        public int EdgeOfCover(ref byte[] array, ref double[] ex, ref double[] ey, double Min, double Max,  bool IsManual = true)
        {
            //  ex[9998], ey[9998] :  center of left open
            //  ex[9999], ey[9999] :  center of right open
            int i = 0;
            int k = 0;

            double px1 = 0;
            double py1 = 0;
            double px2 = 0;
            double py2 = 0;
            double theta = 0;
            double sin = 0;
            double cos = 0;
            double[] diff = new double[2];
            double maxSlope = 0;
            double slope = 0;
            int r_start = 0;
            int r_end = 0;
            r_start = (int)Min - 1;  //  제품 대조리개 반경은 118~ 119
            r_end = (int)Max + 1;    //  제품 커버원 반경은 138~140
            //switch (STATIC.Rcp.Model.ModelName)
            //{
            //    case "SO1G73":
            //        r_start = 750;  //  제품 대조리개 반경은 118~ 119
            //        r_end = 830;    //  제품 커버원 반경은 138~140
            //        break;
            //    case "SO1C81":

            //        r_start = (int)Min - 1;  //  제품 대조리개 반경은 118~ 119
            //        r_end = (int)Max + 1;    //  제품 커버원 반경은 138~140
            //        break;
            //}
            double r = 0;
            double cx = ex[9999];
            double cy = ey[9999];
            if (cx > 0 && cy > 0)
            {
                try
                {
                    for (i = 0; i < 360; i++)
                    {
                        //  중심에서 반경방향으로 0.05deg 단위로 돌아가면서 기울기 최대값지점의 좌표를 linear interpolation 해서 찾아 저장한다.
                        theta = i / 180.0 * Math.PI;
                        sin = Math.Sin(theta);
                        cos = Math.Cos(theta);
                        maxSlope = 0;
                        for (r = r_start; r < r_end; r += 0.5)
                        {
                            px1 = cx + r * cos;
                            py1 = cy + r * sin;
                            if (py1 >= STATIC.DefaultHeight - 1 || py1 < 3)
                                continue;

                            diff[0] = BinInterpolation(ref array, px1, py1);    //  외측
                                                                                //px2 = cx + (r - 2) * cos;
                                                                                //py2 = cy + (r - 2) * sin;
                            px2 = px1 - 2 * cos;
                            py2 = py1 - 2 * sin;
                            diff[1] = BinInterpolation(ref array, px2, py2);    //  내측

                            if (diff[1] > diff[0] - 3)  //  내측이 밝고 외측이 어두워야 한다. 그러므로 외측이 밝은 경우 통과.
                            {
                                r++;
                                continue;
                            }
                            slope = diff[0] - diff[1];
                            //break;


                            //switch (STATIC.Rcp.Model.ModelName)
                            //{
                            //    case "SO1G73":
                            //    case "SO1C81":

                            //        if (diff[1] > diff[0] - 3)  //  내측이 밝고 외측이 어두워야 한다. 그러므로 외측이 밝은 경우 통과.
                            //        {
                            //            r++;
                            //            continue;
                            //        }
                            //        slope = diff[0] - diff[1];
                            //        break;
                            //    default:
                            //        if (diff[0] - 3 < diff[1]) // 외측이 밝고 내측이 어두운 경우
                            //        {
                            //            r++;
                            //            continue;
                            //        }
                            //        slope = diff[0] - diff[1]; // 외측이 밝고 내측이 어두운 경우
                            //        break;
                            //}
                            if (slope > maxSlope)
                            {
                                maxSlope = slope;
                                ex[k] = (px1 + px2) / 2;
                                ey[k] = (py1 + py2) / 2;
                            }
                        }
                        k++;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error in EdgeOfCover() : " + e.ToString());
                }
            }


            //StreamWriter wr = new StreamWriter("CoverEdge.txt");
            //for (i = 0; i < k; i++)
            //    wr.WriteLine(i.ToString() + "]=\t" + ex[i].ToString("F1") + "\t" + ey[i].ToString("F1"));

            //wr.Close();
            return k;
        }
        public void InverseU(ref double[,] invM, int dim)
        {
            //  Calculate Inverse Matrix of matrixU and save result to invM
            #region OLD VERSION
            //double[,] copySrc = new double[dim, dim];

            //for (int i = 0; i < dim; ++i) // copy the values
            //    for (int j = 0; j < dim; ++j)
            //        copySrc[i, j] = invM[i, j];

            //double[,] result = MatrixInverse(copySrc, dim);
            //for (int i = 0; i < dim; ++i) // copy the values
            //    for (int j = 0; j < dim; ++j)
            //        invM[i, j] = result[i, j];
            #endregion

            #region ALGLIB VERSION
            //int info;
            //alglib.matinvreport rep;
            //alglib.rmatrixinverse(ref invM, out info, out rep);
            #endregion

            //-------------------- Math.NET VERSION ----------------------------
            try
            {
                var A = Matrix<double>.Build.DenseOfArray(invM);
                invM = A.Inverse().ToArray();

            }
            catch (Exception)
            {
                MessageBox.Show("error Matrix");
            }


        }
        // Updated 2022.10.31
        public void MatrixCross(ref double[,] dimbydim, ref double[] dimby1, ref double[] res, int dim)
        {
            #region OLD VERSION
            //  Calculate [ dim x dim ] x [ dim ]
            //for (int i = 0; i < dim; i++)
            //{
            //    res[i] = 0;
            //    for (int j = 0; j < dim; j++)
            //    {
            //        res[i] += dimbydim[i, j] * dimby1[j];
            //    }
            //}
            #endregion

            //-------------------- Math.NET VERSION ----------------------------
            var A = Matrix<double>.Build.DenseOfArray(dimbydim);
            var B = Vector<double>.Build.DenseOfArray(dimby1);
            res = A.Multiply(B).ToArray();

        }
        public double GetDist(OpenCvSharp.Point2d a, OpenCvSharp.Point2d b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
        public int FindRansacCircle(OpenCvSharp.Point2d[] pArray, double tolerance, ref CircleInfo resInfo)
        {
            int len = pArray.Length;
            double radious = resInfo.R;
            OpenCvSharp.Point2d c0 = new OpenCvSharp.Point2d(resInfo.X, resInfo.Y);
            List<OpenCvSharp.Point2d> effPeri = new List<OpenCvSharp.Point2d>();
            int resCount = 0;
            for (int i = 0; i < len; i++)
            {
                double ddist = GetDist(pArray[i], c0);
                if (Math.Abs(ddist - radious) > tolerance)
                    continue;
                effPeri.Add(pArray[i]);
            }
            mcLMSCircle(effPeri.ToArray(), effPeri.Count, ref resInfo);

            if (tolerance < 1.7)
                return effPeri.Count;
            else
                //resCount = FindRansacCircle(effPeri.ToArray(), tolerance * 0.9, ref resInfo);
                resCount = FindRansacCircle(pArray, tolerance * 0.9, ref resInfo);

            return resCount;
        }
        public void mcLMSCircle(OpenCvSharp.Point2d[] wp, int length, ref CircleInfo ci)
        {
            double[,] XXTinv = new double[3, 3];
            double[] XTA = new double[3];
            double[] A = new double[3];
            double xx = 0;
            double xy = 0;
            double yy = 0;
            for (int i = 0; i < length; i++)
            {
                if (wp[i].X == 0 || wp[i].Y == 0)
                    continue;

                //XXTinv[0, 0] += wp[i].X * wp[i].X;
                //XXTinv[0, 1] += wp[i].X * wp[i].Y;
                //XXTinv[0, 2] += wp[i].X;

                //XXTinv[1, 0] += wp[i].Y * wp[i].X;
                //XXTinv[1, 1] += wp[i].Y * wp[i].Y;
                //XXTinv[1, 2] += wp[i].Y;

                //XXTinv[2, 0] += wp[i].X;
                //XXTinv[2, 1] += wp[i].Y;
                //XXTinv[2, 2] += 1;

                //XTA[0] -= wp[i].X * (wp[i].X * wp[i].X + wp[i].Y * wp[i].Y);
                //XTA[1] -= wp[i].Y * (wp[i].X * wp[i].X + wp[i].Y * wp[i].Y);
                //XTA[2] -= (wp[i].X * wp[i].X + wp[i].Y * wp[i].Y);

                xx = wp[i].X * wp[i].X;
                xy = wp[i].X * wp[i].Y;
                yy = wp[i].Y * wp[i].Y;

                XXTinv[0, 0] += xx;
                XXTinv[0, 1] += xy;
                XXTinv[0, 2] += wp[i].X;

                XXTinv[1, 0] += xy;
                XXTinv[1, 1] += yy;
                XXTinv[1, 2] += wp[i].Y;

                XXTinv[2, 0] += wp[i].X;
                XXTinv[2, 1] += wp[i].Y;
                XXTinv[2, 2] += 1;

                XTA[0] -= wp[i].X * (xx + yy);
                XTA[1] -= wp[i].Y * (xx + yy);
                XTA[2] -= (xx + yy);
            }
            InverseU(ref XXTinv, 3);
            MatrixCross(ref XXTinv, ref XTA, ref A, 3);
            ci.X = -A[0] / 2;
            ci.Y = -A[1] / 2;
            ci.R = Math.Sqrt(-A[2] + ci.X * ci.X + ci.Y * ci.Y);
        }
        public int FindBestCirleAlongEdge(OpenCvSharp.Point2d[] pArray, ref Point2D bestC, ref double bestR, bool IsCover = false)
        {
            //int EffCount = 0;
            List<OpenCvSharp.Point2d> pCircle = pArray.ToList();
            int fullLen = pCircle.Count;
            int arcLen = fullLen / 6;

            if (IsCover)
                arcLen = fullLen;

            pCircle.AddRange(pCircle.GetRange(0, arcLen));  // Circular 참조를 위해.

            List<OpenCvSharp.Point2d> pArc = new List<OpenCvSharp.Point2d>();
            CircleInfo cInfo = new CircleInfo();
            CircleInfo cInfo_max = new CircleInfo();
            int[] arcPtCnt = new int[360];
            int i_max = 0;
            int arcPtCnt_max = 0;
            int i_step = fullLen / 180;
            int arcIndex = 0;

            if (IsCover)
                i_step = fullLen;

            for (int i = 0; i < fullLen; i += i_step)
            {
                if (i + arcLen > fullLen)
                    arcLen = fullLen - i;

                pArc = pCircle.GetRange(i, arcLen);
                mcLMSCircle(pArc.ToArray(), pArc.Count, ref cInfo);
                arcPtCnt[arcIndex] = FindRansacCircle(pArray, 3, ref cInfo);
                if (arcPtCnt_max < arcPtCnt[arcIndex])
                {
                    arcPtCnt_max = arcPtCnt[arcIndex];
                    i_max = arcIndex;
                    bestC.X = cInfo.X;
                    bestC.Y = cInfo.Y;
                    bestR = cInfo.R;
                }
                if (arcPtCnt[arcIndex] > fullLen / 2)
                    break;
                arcIndex++;
            }
            return arcPtCnt[arcIndex];
        }

        #endregion
        public double PointToLine(OpenCvSharp.Point pt, OpenCvSharp.Point p1, OpenCvSharp.Point p2)
        {
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            if ((dx == 0) && (dy == 0))
            {

                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            // Calculate the t that minimizes the distance.
            float t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) / (dx * dx + dy * dy);

            if (t < 0)
            {

                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
            }
            else if (t > 1)
            {

                dx = pt.X - p2.X;
                dy = pt.Y - p2.Y;
            }
            else
            {

                PointF closest = new PointF(p1.X + t * dx, p1.Y + t * dy);
                dx = pt.X - closest.X;
                dy = pt.Y - closest.Y;
            }

            return Math.Sqrt(dx * dx + dy * dy);
        }

        unsafe public void TestCDll_test(int ch, Mat img)
        {
            try
            {

                //Mat mask = Mat.Zeros(img.Rows, img.Cols, MatType.CV_8UC1);
                //Mat SendImg = new Mat();
                //int radius = mask.Cols / 2;
                //Cv2.Circle(mask, new OpenCvSharp.Point(mask.Cols / 2, mask.Rows / 2), radius, Scalar.White, -1);
                //img.CopyTo(SendImg, mask);

                byte[] arr = new byte[img.Height * img.Width];
                img.GetArray(out arr);
                string Dir = STATIC.CreateDateDir();
                if (ch == 0)
                    Dir += "CLogicImg_L\\";
                else Dir += "CLogicImg_R\\";
                if (!Directory.Exists(Dir))
                    Directory.CreateDirectory(Dir);

                XiaomiDll2.INPUTINFO info = new XiaomiDll2.INPUTINFO();
                XiaomiDll2.RESULT res = new XiaomiDll2.RESULT();

                fixed (byte* p = arr)
                {
                    IntPtr ptr = (IntPtr)p;
                    info.grayBuffer = p;
                }

                info.height = 1500;
                info.width = 1500;
                info.pixelRatio = 0.0048;
                info.VAcropRatio = 0.2;
                info.VAthresh = 70;
                info.VAthresh2 = 120;
                info.coverDiameterPixel = 1300;
                info.LogSave = true;


                info.testFlag = 3; // 1 : va 2 : cover 3 : Va & cover ;
                info.F14Flag = false;
                info.LogPath = Dir;
                XiaomiDll2.IRIS_Circle_Test(info, out res);
                //Res.cx = res.VA_x_circle;
                //Res.cy = res.VA_y_circle;
                //Res.CArea = res.VA_area;
                //Res.CCircleAcuraccy = res.VA_circleAccuracy;
                //Res.CShapeAccuracy = res.VA_shapeAccuracy;
                //Res.Cover_cx = res.cover_x;
                //Res.Cover_cy = res.cover_y;

            }
            catch { }



        }
        double GetDistance(PointF ct, OpenCvSharp.Point pt)
        {
            return Math.Sqrt(Math.Pow(ct.X - pt.X, 2) + Math.Pow(ct.Y - pt.Y, 2));
        }
        unsafe public void TestCDll2(int ch, Mat img, bool isF14, int TestFlag, InspResult Res, bool isSearch)
        {
            try
            {

                Mat mask = Mat.Zeros(img.Rows, img.Cols, MatType.CV_8UC1);
                Mat SendImg = new Mat();
                int radius = mask.Cols / 2;
                Cv2.Circle(mask, new OpenCvSharp.Point(mask.Cols / 2, mask.Rows / 2), radius, Scalar.White, -1);
                img.CopyTo(SendImg, mask);

                byte[] arr = new byte[img.Height * img.Width];
                SendImg.GetArray(out arr);
                string Dir = STATIC.CreateDateDir();
                if (ch == 0)
                    Dir += "CLogicImg_L\\";
                else Dir += "CLogicImg_R\\";
                if (!Directory.Exists(Dir))
                    Directory.CreateDirectory(Dir);

                XiaomiDll2.INPUTINFO info = new XiaomiDll2.INPUTINFO();
                XiaomiDll2.RESULT res = new XiaomiDll2.RESULT();

                fixed (byte* p = arr)
                {
                    IntPtr ptr = (IntPtr)p;
                    info.grayBuffer = p;
                }

                info.height = STATIC.DefaultHeight;
                info.width = STATIC.DefaultWidth;
                info.pixelRatio = STATIC.ScaleResolution[ch];
                info.VAcropRatio = 0.2;
                info.VAthresh = 70;
                info.VAthresh2 = 120;

                if (STATIC.Rcp.Option.isActMode)
                    info.coverDiameterPixel = 1400;
                else
                    info.coverDiameterPixel = 1300;
                //if (STATIC.Rcp.Model.ModelName == "SO1C81")
                //{
                //    if (STATIC.Rcp.Option.isActMode)
                //        info.coverDiameterPixel = 1400;
                //    else
                //        info.coverDiameterPixel = 1300;
                //}
                //else info.coverDiameterPixel = 1660;
                if (isSearch)
                    info.LogSave = false;
                else { } //info.LogSave = STATIC.Rcp.Option.DllLogSave;


                info.testFlag = TestFlag; // 1 : va 2 : cover 3 : Va & cover ;
                info.F14Flag = isF14;
                info.LogPath = Dir;            
                XiaomiDll2.IRIS_Circle_Test(info, out res);
                Res.cx = res.VA_x_circle;
                Res.cy = res.VA_y_circle;

                if(isSearch)
                    Res.Area = res.VA_area;
                else
                    Res.CArea = res.VA_area;

                Res.CCircleAcuraccy = res.VA_circleAccuracy;
                Res.CShapeAccuracy = res.VA_shapeAccuracy;
                Res.Cover_cx = res.cover_x;
                Res.Cover_cy = res.cover_y;
                Res.CCover_dia = res.cover_diameter;
            }
            catch { }
        
         

        }
        public double JustCheckArea(int ch, Mat img, bool isSetupView)
        {
            try
            {
                using (Mat tmp = img.Clone())
                {

                    List<OpenCvSharp.Point[]> C = new List<OpenCvSharp.Point[]>();
                    List<PointF> center = new List<PointF>();
                    List<double> Area = new List<double>();
                    OpenCvSharp.Point[][] contour;
                    HierarchyIndex[] hierarchy;
                    Mat grayImage = new Mat();
                    Cv2.InRange(tmp, 180, 255, grayImage);
                    Cv2.FindContours(grayImage, out contour, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
                    RotatedRect roRect = new RotatedRect();
                    for (int i = 0; i < contour.Length; i++)
                    {
                        double dArea = Cv2.ContourArea(contour[i]) * STATIC.ScaleResolution[ch] * STATIC.ScaleResolution[ch];
                        roRect = Cv2.MinAreaRect(contour[i]);
                        if (dArea > 1)
                        {
                            center.Add(new PointF(roRect.Center.X, roRect.Center.Y));
                            Area.Add(dArea);
                            C.Add(contour[i]);
                        }
                    }
                    double minLength = double.MaxValue;
                    int centerIndex = 0;
                    for (int i = 0; i < center.Count; i++)
                    {
                        double a = Math.Sqrt(Math.Pow(grayImage.Width / 2f - center[i].X, 2) + Math.Pow(grayImage.Height / 2f - center[i].Y, 2));
                        if (minLength > a)
                        {
                            minLength = a;
                            centerIndex = i;

                        }
                    }

                    if (isSetupView)
                    {

                        using (Pen shapeLinePen = new Pen(Color.Green, 10f))
                        using (Bitmap resBmp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(img))
                        using (Bitmap res2bmp = new Bitmap(resBmp.Width, resBmp.Height))
                        using (Graphics gr = Graphics.FromImage(res2bmp))
                        {
                            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            gr.DrawImage(resBmp, 0, 0);
                            PointF[] pt = new PointF[C[centerIndex].Length];
                            for (int i = 0; i < C[centerIndex].Length; i++)
                                pt[i] = new PointF(C[centerIndex][i].X, C[centerIndex][i].Y);


                            gr.DrawPolygon(shapeLinePen, pt);

                            STATIC.ResMat[ch] = res2bmp.ToMat();
                        }
                    }
                    return Area[centerIndex];
                }
            }
            catch { return 0; }

        }
        //public double JustCheckArea(int ch, Mat img, bool isSetupView)
        //{
        //    try
        //    {

        //        Mat inspImg = img.Clone();
        //        List<List<PointF>> ctList = new List<List<PointF>>() { new List<PointF>(), new List<PointF>(), new List<PointF>() };
        //        List<List<double>> AreaList = new List<List<double>>() { new List<double>(), new List<double>(), new List<double>() };
        //        List<List<OpenCvSharp.Point[]>> ContourList = new List<List<OpenCvSharp.Point[]>>() { new List<OpenCvSharp.Point[]>(), new List<OpenCvSharp.Point[]>(), new List<OpenCvSharp.Point[]>() };
        //        int[] CoverIndex = new int[3] { 0, 0, 0 };
        //        double[] CoverVal = new double[3] { double.MinValue, double.MinValue, double.MinValue };
        //        int OpenIndex = 0;
        //        double OpenVal = double.MinValue;
        //        PointF OpenCenter = new PointF();
        //        Parallel.For(0, 3, i =>
        //        {
        //            Mat binImg = new Mat();
        //            //  Cv2.GaussianBlur(inspImg, inspImg, new OpenCvSharp.Size(3, 3), 1, 1, BorderTypes.Default);

        //            if (i == 0) Cv2.InRange(inspImg, 140, 255, binImg);
        //            else if (i == 1) Cv2.InRange(inspImg, 160, 255, binImg);
        //            else Cv2.InRange(inspImg, 180, 255, binImg);
        //            OpenCvSharp.Point[][] contour;
        //            HierarchyIndex[] h;
        //            Cv2.FindContours(binImg, out contour, out h, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
        //            RotatedRect roRect = new RotatedRect();

        //            int tmpIndex = 0;
        //            for (int j = 0; j < contour.Length; j++)
        //            {
        //                double dArea = Cv2.ContourArea(contour[j]) * STATIC.ScaleResolution[ch] * STATIC.ScaleResolution[ch];

        //                if (dArea > 1)
        //                {
        //                    roRect = Cv2.MinAreaRect(contour[j]);
        //                    ctList[i].Add(new PointF(roRect.Center.X, roRect.Center.Y));
        //                    AreaList[i].Add(dArea);
        //                    ContourList[i].Add(contour[j].ToArray());
        //                    if (CoverVal[i] < dArea)
        //                    {
        //                        CoverIndex[i] = tmpIndex;
        //                        CoverVal[i] = dArea;

        //                    }
        //                    tmpIndex++;
        //                }
        //            }

        //        });
        //        int index = 0;
        //        if (ContourList[2].Count > 1 && CoverVal[2] > 25) index = 2;
        //        else if (ContourList[1].Count > 1 && CoverVal[1] > 25) index = 1;



        //        for (int i = 0; i < ContourList[index].Count; i++)
        //        {
        //            if (i != CoverIndex[index])
        //            {
        //                if (Cv2.PointPolygonTest(ContourList[index][i], new Point2f(ctList[index][CoverIndex[index]].X, ctList[index][CoverIndex[index]].Y), false) != -1)
        //                {
        //                    if (OpenVal < AreaList[index][i])
        //                    {
        //                        OpenIndex = i;
        //                        OpenVal = AreaList[index][i];
        //                        OpenCenter = ctList[index][i];
        //                    }
        //                }

        //            }
        //        }

        //        var dist = ContourList[index][OpenIndex].Select(pt => GetDistance(OpenCenter, pt)).ToList();


        //        double[] lex = new double[10000];
        //        double[] ley = new double[10000];

        //        lex[9999] = OpenCenter.X;
        //        ley[9999] = OpenCenter.Y;
        //        inspImg.GetArray(out byte[] b);
        //        int cnt = EdgeOfCover(ref b, ref lex, ref ley, dist.Min(), dist.Max());
        //        OpenCvSharp.Point[] pArray = new OpenCvSharp.Point[cnt];

        //        for (int i = 0; i < cnt; i++) pArray[i] = new OpenCvSharp.Point((int)lex[i], (int)ley[i]);

        //        if (isSetupView)
        //        {

        //            using (Pen shapeLinePen = new Pen(Color.Green, 10f))
        //            using (Bitmap resBmp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(img))
        //            using (Bitmap res2bmp = new Bitmap(resBmp.Width, resBmp.Height))
        //            using (Graphics gr = Graphics.FromImage(res2bmp))
        //            {
        //                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        //                gr.DrawImage(resBmp, 0, 0);
        //                PointF[] pt = new PointF[pArray.Length];
        //                for (int i = 0; i < pArray.Length; i++)
        //                    pt[i] = new PointF(pArray[i].X, pArray[i].Y);
        //                gr.DrawPolygon(shapeLinePen, pt);

        //                STATIC.ResMat[ch] = res2bmp.ToMat();
        //            }
        //        }
        //        return Cv2.ContourArea(pArray) * STATIC.ScaleResolution[ch] * STATIC.ScaleResolution[ch];
        //    }
        //    catch
        //    {
        //        return 0;
        //    }

        //}

        public (PointF ct, double d, OpenCvSharp.Point[] c) CheckArea(int ch, Mat img)
        {
            try
            {
                using (Mat tmp = img.Clone())
                {

                    List<OpenCvSharp.Point[]> C = new List<OpenCvSharp.Point[]>();
                    List<PointF> center = new List<PointF>();
                    List<double> Area = new List<double>();
                    OpenCvSharp.Point[][] contour;
                    HierarchyIndex[] hierarchy;
                    Mat grayImage = new Mat();
                    Cv2.InRange(tmp, 180, 255, grayImage);
                    Cv2.FindContours(grayImage, out contour, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
                    RotatedRect roRect = new RotatedRect();
                    for (int i = 0; i < contour.Length; i++)
                    {
                        double dArea = Cv2.ContourArea(contour[i]) * STATIC.ScaleResolution[ch] * STATIC.ScaleResolution[ch];
                        roRect = Cv2.MinAreaRect(contour[i]);
                        if (dArea > 1)
                        {
                            center.Add(new PointF(roRect.Center.X, roRect.Center.Y));
                            Area.Add(dArea);
                            C.Add(contour[i]);
                        }
                    }
                    double minLength = double.MaxValue;
                    int centerIndex = 0;
                    for (int i = 0; i < center.Count; i++)
                    {
                        double a = Math.Sqrt(Math.Pow(grayImage.Width / 2f - center[i].X, 2) + Math.Pow(grayImage.Height / 2f - center[i].Y, 2));
                        if (minLength > a)
                        {
                            minLength = a;
                            centerIndex = i;

                        }
                    }
                    return (center[centerIndex], Area[centerIndex], C[centerIndex]);
                }
            }
            catch { return (new PointF(), 0, new OpenCvSharp.Point[1]); }


        }



        //public (double d, OpenCvSharp.Point[] c) CheckArea(int ch, Mat img)
        //{
        //    try
        //    {
        //        Mat inspImg = img.Clone();
        //        List<List<PointF>> ctList = new List<List<PointF>>() { new List<PointF>(), new List<PointF>(), new List<PointF>() };
        //        List<List<double>> AreaList = new List<List<double>>() { new List<double>(), new List<double>(), new List<double>() };
        //        List<List<OpenCvSharp.Point[]>> ContourList = new List<List<OpenCvSharp.Point[]>>() { new List<OpenCvSharp.Point[]>(), new List<OpenCvSharp.Point[]>(), new List<OpenCvSharp.Point[]>() };
        //        int[] CoverIndex = new int[3] { 0, 0, 0 };
        //        double[] CoverVal = new double[3] { double.MinValue, double.MinValue, double.MinValue };
        //        int OpenIndex = 0;
        //        double OpenVal = double.MinValue;
        //        PointF OpenCenter = new PointF();
        //        Parallel.For(0, 3, i =>
        //        {
        //            Mat binImg = new Mat();
        //            //  Cv2.GaussianBlur(inspImg, inspImg, new OpenCvSharp.Size(3, 3), 1, 1, BorderTypes.Default);

        //            if (i == 0) Cv2.InRange(inspImg, 140, 255, binImg);
        //            else if (i == 1) Cv2.InRange(inspImg, 160, 255, binImg);
        //            else Cv2.InRange(inspImg, 180, 255, binImg);
        //            OpenCvSharp.Point[][] contour;
        //            HierarchyIndex[] h;
        //            Cv2.FindContours(binImg, out contour, out h, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
        //            RotatedRect roRect = new RotatedRect();

        //            int tmpIndex = 0;
        //            for (int j = 0; j < contour.Length; j++)
        //            {
        //                double dArea = Cv2.ContourArea(contour[j]) * STATIC.ScaleResolution[ch] * STATIC.ScaleResolution[ch];

        //                if (dArea > 1)
        //                {
        //                    roRect = Cv2.MinAreaRect(contour[j]);
        //                    ctList[i].Add(new PointF(roRect.Center.X, roRect.Center.Y));
        //                    AreaList[i].Add(dArea);
        //                    ContourList[i].Add(contour[j].ToArray());
        //                    if (CoverVal[i] < dArea)
        //                    {
        //                        CoverIndex[i] = tmpIndex;
        //                        CoverVal[i] = dArea;

        //                    }
        //                    tmpIndex++;
        //                }
        //            }

        //        });
        //        int index = 0;
        //        if (ContourList[2].Count > 1 && CoverVal[2] > 25) index = 2;
        //        else if (ContourList[1].Count > 1 && CoverVal[1] > 25) index = 1;



        //        for (int i = 0; i < ContourList[index].Count; i++)
        //        {
        //            if (i != CoverIndex[index])
        //            {
        //                if (Cv2.PointPolygonTest(ContourList[index][i], new Point2f(ctList[index][CoverIndex[index]].X, ctList[index][CoverIndex[index]].Y), false) != -1)
        //                {
        //                    if (OpenVal < AreaList[index][i])
        //                    {
        //                        OpenIndex = i;
        //                        OpenVal = AreaList[index][i];
        //                        OpenCenter = ctList[index][i];
        //                    }
        //                }

        //            }
        //        }

        //        var dist = ContourList[index][OpenIndex].Select(pt => GetDistance(OpenCenter, pt)).ToList();


        //        double[] lex = new double[10000];
        //        double[] ley = new double[10000];

        //        lex[9999] = OpenCenter.X;
        //        ley[9999] = OpenCenter.Y;
        //        inspImg.GetArray(out byte[] b);
        //        int cnt = EdgeOfCover(ref b, ref lex, ref ley, dist.Min(), dist.Max());
        //        OpenCvSharp.Point[] pArray = new OpenCvSharp.Point[cnt];

        //        for (int i = 0; i < cnt; i++) pArray[i] = new OpenCvSharp.Point((int)lex[i], (int)ley[i]);
        //        return (Cv2.ContourArea(pArray) * STATIC.ScaleResolution[ch] * STATIC.ScaleResolution[ch], pArray);
        //    }
        //    catch { return (0, new OpenCvSharp.Point[1]); }



        //}


        (Point2D ct, double DesignC) FindCover(Mat img, PointF ct)
        {
            try
            {
                Point2D resCenter = new Point2D();
                double Design = 0;
                double[] lex = new double[10000];
                double[] ley = new double[10000];

                lex[9999] = ct.X;
                ley[9999] = ct.Y;
                img.GetArray(out byte[] b);
                int cnt = EdgeOfCover(ref b, ref lex, ref ley);
                List<Point2d> pArray = new List<Point2d>();
                for (int i = 0; i < cnt; i++)
                {
                    pArray.Add(new Point2d(lex[i], ley[i]));
                }
                FindBestCirleAlongEdge(pArray.ToArray(), ref resCenter, ref Design, true);
                return (resCenter, Design);
            }
            catch
            {
                return (new Point2D(), 0);
            }
        }
        //public (Point2D ct, double DesignC) FindCover(int ch, Mat img)
        //{
        //    try
        //    {
        //        Mat inspImg = img.Clone();
        //        Mat binImg = new Mat();
        //        //    Cv2.GaussianBlur(inspImg, inspImg, new OpenCvSharp.Size(3, 3), 1, 1, BorderTypes.Default);
        //        Cv2.InRange(inspImg, 130, 255, binImg);
        //        OpenCvSharp.Point[][] contour;
        //        HierarchyIndex[] h;
        //        Cv2.FindContours(binImg, out contour, out h, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
        //        RotatedRect roRect = new RotatedRect();
        //        List<PointF> ctList = new List<PointF>();
        //        List<double> AreaList = new List<double>();
        //        List<OpenCvSharp.Point[]> ContourList = new List<OpenCvSharp.Point[]>();

        //        int CoverIndex = 0;
        //        double CoverVal = double.MinValue;
        //        int tmpIndex = 0;
        //        for (int i = 0; i < contour.Length; i++)
        //        {
        //            double dArea = Cv2.ContourArea(contour[i]) * STATIC.ScaleResolution[ch] * STATIC.ScaleResolution[ch];

        //            if (dArea > 1)
        //            {
        //                roRect = Cv2.MinAreaRect(contour[i]);
        //                ctList.Add(new PointF(roRect.Center.X, roRect.Center.Y));
        //                AreaList.Add(dArea);
        //                ContourList.Add(contour[i].ToArray());
        //                if (CoverVal < dArea)
        //                {
        //                    CoverIndex = tmpIndex;
        //                    CoverVal = dArea;

        //                }
        //                tmpIndex++;
        //            }
        //        }
        //        Point2d[] Cover = new Point2d[ContourList[CoverIndex].Length];
        //        Point2D ct = new Point2D();
        //        double Design = 0;
        //        for (int i = 0; i < ContourList[CoverIndex].Length; i++)
        //            Cover[i] = new Point2d(ContourList[CoverIndex][i].X, ContourList[CoverIndex][i].Y);


        //        FindBestCirleAlongEdge(Cover, ref ct, ref Design);

        //        return (ct, Design);
        //    }
        //    catch { return (new Point2D(), 0); }




        //}


        (PointF pt, double d) FindShape(OpenCvSharp.Point[] pt, OpenCvSharp.Point ctpt)
        {
            try
            {
            
                OpenCvSharp.Point[] hull = Cv2.ConvexHull(pt);

                Dictionary<OpenCvSharp.Point, double> MaxPoints = new Dictionary<OpenCvSharp.Point, double>();

                for (int i = 0; i < hull.Length; i++)
                {
                    List<double> pointsVal = new List<double>();
                    List<OpenCvSharp.Point> points = new List<OpenCvSharp.Point>();
                    OpenCvSharp.Point[] input = new OpenCvSharp.Point[3];
                    if (i == hull.Length - 1)
                    {
                        input = new OpenCvSharp.Point[3]
                        {
                            new OpenCvSharp.Point(hull[i].X, hull[i].Y),
                            new OpenCvSharp.Point(hull[0].X, hull[0].Y),
                            new OpenCvSharp.Point(ctpt.X, ctpt.Y),
                        };
                    }
                    else
                    {
                        input = new OpenCvSharp.Point[3]
                     {
                        new OpenCvSharp.Point(hull[i].X, hull[i].Y),
                         new OpenCvSharp.Point(hull[i + 1].X, hull[i + 1].Y),
                          new OpenCvSharp.Point(ctpt.X, ctpt.Y),
                     };
                    }

                    for (int j = 0; j < pt.Length; j++)
                    {
                        double a = Cv2.PointPolygonTest(input, pt[j], false);
                        if (a != -1)
                        {
                            if (i == hull.Length - 1)
                                pointsVal.Add(PointToLine(pt[j], hull[i], hull[0]));
                            else
                                pointsVal.Add(PointToLine(pt[j], hull[i], hull[i + 1]));
                            points.Add(pt[j]);
                        }
                    }
                    if (pointsVal.Count > 0)
                    {
                        int maxIndex = pointsVal.ToList().IndexOf(pointsVal.Max());
                        if (!MaxPoints.ContainsKey(points[maxIndex]))
                            MaxPoints.Add(points[maxIndex], pointsVal[maxIndex]);
                    }
                }

               
                MaxPoints = MaxPoints.OrderByDescending(p => p.Value).ToDictionary(x => x.Key, x => x.Value);
                return (new PointF(MaxPoints.Keys.ElementAt(0).X, MaxPoints.Keys.ElementAt(0).Y), MaxPoints.Values.ElementAt(0));

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return (new PointF(0, 0), 0);
            }



        }

        PointF[] FindVertex(OpenCvSharp.Point[] contour, bool isoverF20)
        {
            try
            {

                List<OpenCvSharp.Point> pt = new List<OpenCvSharp.Point>();
                for (int i = 0; i < contour.Length; i++)
                    pt.Add(new OpenCvSharp.Point(contour[i].X, contour[i].Y));
             
                OpenCvSharp.Point[] hull = Cv2.ConvexHull(pt);
                List<OpenCvSharp.Point> Hull2 = new List<OpenCvSharp.Point>();
                OpenCvSharp.Point MaxHull = new OpenCvSharp.Point();

                double RefDeg = 0;
                double[] HullDeg = new double[hull.Length];

            
                MaxHull = new OpenCvSharp.Point(hull[0].X, hull[0].Y);

                Moments m = Cv2.Moments(pt.ToArray());
                Point2f CtPt = new Point2f((float)(m.M10 / m.M00), (float)(m.M01 / m.M00));
                Dictionary<int, double> hullLength = new System.Collections.Generic.Dictionary<int, double>();

                for (int i = 0; i < hull.Length; i++)
                {
                    double a = Math.Sqrt((MaxHull.X - CtPt.X) * (MaxHull.X - CtPt.X) + (MaxHull.Y - CtPt.Y) * (MaxHull.Y - CtPt.Y));
                    double b = Math.Sqrt((hull[i].X - CtPt.X) * (hull[i].X - CtPt.X) + (hull[i].Y - CtPt.Y) * (hull[i].Y - CtPt.Y));
                    if (b > a)
                        MaxHull = hull[i];
                }
                RefDeg = Math.Atan2(MaxHull.Y - CtPt.Y, MaxHull.X - CtPt.X) * 180 / Math.PI - 18;

                for (int i = 0; i < hull.Length; i++)
                {
                    HullDeg[i] = Math.Atan2(hull[i].Y - CtPt.Y, hull[i].X - CtPt.X) * 180 / Math.PI - RefDeg;
                    if (HullDeg[i] < 0)
                        HullDeg[i] = HullDeg[i] + 360;
                }

                int MaxHullKey = 0;
                int BeforeHullKey = 0;
                for (int i = 0; i < 10; i++)
                {
                    MaxHull = new OpenCvSharp.Point(0, 0);
                    MaxHullKey = 0;

                    for (int j = 0; j < hull.Length; j++)
                    {

                        if (i * 36 <= HullDeg[j] && HullDeg[j] < ((i + 1) * 36) /*&& Math.Abs(HullDeg[BeforeHullKey] - HullDeg[j]) > 15*/)
                        {


                            if (MaxHull.X == 0 && MaxHull.Y == 0)
                            {
                                MaxHull = new OpenCvSharp.Point(hull[j].X, hull[j].Y);
                                MaxHullKey = j;
                            }
                            else
                            {
                                double a = Math.Sqrt((MaxHull.X - CtPt.X) * (MaxHull.X - CtPt.X) + (MaxHull.Y - CtPt.Y) * (MaxHull.Y - CtPt.Y));
                                double b = Math.Sqrt((hull[j].X - CtPt.X) * (hull[j].X - CtPt.X) + (hull[j].Y - CtPt.Y) * (hull[j].Y - CtPt.Y));
                                if (b > a)
                                {
                                    MaxHull = new OpenCvSharp.Point(hull[j].X, hull[j].Y);
                                    MaxHullKey = j;
                                }

                            }
                        }
                    }
                    BeforeHullKey = MaxHullKey;

                    if (!hullLength.ContainsKey(MaxHullKey))
                        hullLength.Add(MaxHullKey, Math.Sqrt((MaxHull.X - CtPt.X) * (MaxHull.X - CtPt.X) + (MaxHull.Y - CtPt.Y) * (MaxHull.Y - CtPt.Y)));

                }

                hullLength = hullLength.OrderByDescending(p => p.Value).ToDictionary(x => x.Key, x => x.Value);

                int Count = hullLength.Count;
                PointF[] Vertex = new PointF[Count];

                for (int k = 0; k < Vertex.Length; k++)
                {
                    try
                    {

                        if (Count - 1 >= k)
                        {
                            int key = hullLength.Keys.ElementAt(k);
                            Vertex[k] = new PointF((float)hull[key].X, (float)hull[key].Y);
                        }
                    }
                    catch
                    {

                    }

                }

                return Vertex;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                PointF[] Vertex = new PointF[12];
             
                return Vertex;
            }
        }

        (PointF ct, double DesignC) InCircleCt(OpenCvSharp.Point[] contour)
        {
            try
            {
                Rect rect = new Rect();
                List<OpenCvSharp.Point> pt = new List<OpenCvSharp.Point>();
                List<List<OpenCvSharp.Point>> pList = new List<List<OpenCvSharp.Point>>();


                rect = Cv2.BoundingRect(contour);
                Mat tmp = Mat.Zeros(new OpenCvSharp.Size(rect.Width, rect.Height), MatType.CV_8U);
                for (int i = 0; i < contour.Length; i++)
                {
                    pt.Add(new OpenCvSharp.Point(contour[i].X - rect.X, contour[i].Y - rect.Y));
                }
                pList.Add(pt);
                Cv2.FillPoly(tmp, pList, Scalar.White);
                double MaxVal = double.MaxValue;
                double MinVal = double.MinValue;
                OpenCvSharp.Point MaxLoc = new OpenCvSharp.Point();
                OpenCvSharp.Point MinLoc = new OpenCvSharp.Point();
                Cv2.DistanceTransform(tmp, tmp, DistanceTypes.L2, DistanceTransformMasks.Precise);
                Cv2.MinMaxLoc(tmp, out MinVal, out MaxVal, out MinLoc, out MaxLoc);
                PointF CtPt = new PointF((float)(MaxLoc.X + rect.X), (float)(MaxLoc.Y + rect.Y));
                return (CtPt, MaxVal);
            }
            catch { return (new PointF(), 0); }
        }
        PointF AreaCt(OpenCvSharp.Point[] contour)
        {
            try
            {
               
                List<OpenCvSharp.Point> pt = new List<OpenCvSharp.Point>();
                for (int i = 0; i < contour.Length; i++)
                    pt.Add(new OpenCvSharp.Point(contour[i].X, contour[i].Y));
                Moments m = Cv2.Moments(pt.ToArray());
                PointF CtPt = new PointF((float)(m.M10 / m.M00), (float)(m.M01 / m.M00));
                return CtPt;
            }
            catch { return (new PointF()); }


        }
        void CirAccuracy(PointF ctpt, PointF[] vertex, CAResult res)
        {
           
            for (int i = 0; i < vertex.Length; i++)
            {
                if (vertex[i].X != 0 && vertex[i].Y != 0)
                    res.VertexLength.Add(Math.Sqrt((vertex[i].X - ctpt.X) * (vertex[i].X - ctpt.X) + (vertex[i].Y - ctpt.Y) * (vertex[i].Y - ctpt.Y)));
            }
            res.CA = (res.VertexLength.Max() - res.VertexLength.Min());
            res.MaxVertexIndex = res.VertexLength.ToList().IndexOf(res.VertexLength.Max());
            res.MinVertexIndex = res.VertexLength.ToList().IndexOf(res.VertexLength.Min());
            for (int i = 0; i < vertex.Length; i++)
            {
                if (i != res.MaxVertexIndex && i != res.MinVertexIndex)
                {
                    res.OtherVertex.Add(new PointF(vertex[i].X, vertex[i].Y));
                }
            }
            res.MaxPoint = new PointF(vertex[res.MaxVertexIndex].X, vertex[res.MaxVertexIndex].Y);
            res.MinPoint = new PointF(vertex[res.MinVertexIndex].X, vertex[res.MinVertexIndex].Y);
        }
        public bool NewFineCOG(int ch, int index ,Mat img, InspectionType inspType, InspResult Res, bool ResultShow, bool OverF20, bool isSetup, int CurrentPos = 0, int ItrCnt = 0)
        {
            try
            {
                if ((!STATIC.Process.m_ChannelOn[ch] || STATIC.isNonSpecError[ch]) && !isSetup )
                    return true;
                Point2D resCenter = new Point2D();
                CAResult CARes = new CAResult();
                PointF ctPt = new PointF();
                double DesignC = 0;
                double DesignR = 0;
                OpenCvSharp.Point[] Contour = new OpenCvSharp.Point[1];
                PointF[] Vertex = new PointF[1]; 
                double Area = 0;
                PointF ShapePointF = new PointF();
                double ShapeLength = 0;
                (ctPt, Area, Contour) = CheckArea(ch, img.Clone());
                switch (inspType)
                {
                    case InspectionType.FindCover:
                        (resCenter, DesignC) = FindCover(img.Clone(), ctPt);
                        Res.Cover_cx = resCenter.X;
                        Res.Cover_cy = resCenter.Y;
                     //   Res.CCover_dia = DesignC;
                        break;
                    case InspectionType.InCircle_Center:
                        (ctPt, DesignR) = InCircleCt(Contour);
                        Res.cx = ctPt.X;
                        Res.cy = ctPt.Y;
                        break;
                    case InspectionType.Area_Center:
                        ctPt = AreaCt(Contour);
                        Res.cx = ctPt.X;
                        Res.cy = ctPt.Y;
                        break;
                    case InspectionType.InCircle_Decenter:
                        (resCenter, DesignC) = FindCover(img.Clone(), ctPt);
                        //   (resCenter, DesignC) = FindCover(ch, img.Clone());
                        Res.Cover_cx = resCenter.X;
                        Res.Cover_cy = resCenter.Y;
                        (ctPt, DesignR) = InCircleCt(Contour);
                        Res.cx = ctPt.X;
                        Res.cy = ctPt.Y;
                        break;
                    case InspectionType.Area_Decenter:
                        (resCenter, DesignC) = FindCover(img.Clone(), ctPt);
                        //  (resCenter, DesignC) = FindCover(ch, img.Clone());
                        Res.Cover_cx = resCenter.X;
                        Res.Cover_cy = resCenter.Y;
                        ctPt = AreaCt(Contour);
                        Res.cx = ctPt.X;
                        Res.cy = ctPt.Y;
                        break;
                    case InspectionType.JustFind_Vertex:
                        Vertex = FindVertex(Contour, OverF20);
                        break;
                    case InspectionType.Area_CircleAccuracy:
                        ctPt = AreaCt(Contour);
                        Vertex = FindVertex(Contour, OverF20);
                        CirAccuracy(ctPt, Vertex, CARes);
                        Res.CircleAcuraccy = CARes.CA * STATIC.ScaleResolution[ch] * 1000;
                        break;
                    case InspectionType.InCircle_CircleAccuracy:                        
                        (ctPt, DesignR) = InCircleCt(Contour);
                        Vertex = FindVertex(Contour, OverF20);
                        CirAccuracy(ctPt, Vertex, CARes);
                        Res.CircleAcuraccy = CARes.CA * STATIC.ScaleResolution[ch] * 1000;
                        break;
                    case InspectionType.Area_ShapeAccuracy:
                        ctPt = AreaCt(Contour);
                        (ShapePointF, ShapeLength) = FindShape(Contour, new OpenCvSharp.Point(ctPt.X, ctPt.Y));
                        Res.ShapeAccuracy = ShapeLength * STATIC.ScaleResolution[ch] * 1000;
                        break;
                    case InspectionType.InCircle_ShapeAccuracy:                      
                        (ctPt, DesignR) = InCircleCt(Contour);
                        (ShapePointF, ShapeLength) = FindShape(Contour, new OpenCvSharp.Point(ctPt.X, ctPt.Y));
                        Res.ShapeAccuracy = ShapeLength * STATIC.ScaleResolution[ch] * 1000;
                        break;
                    case InspectionType.Area_InspAll:
                        (resCenter, DesignC) = FindCover(img.Clone(), ctPt);
                        Res.Cover_cx = resCenter.X;
                        Res.Cover_cy = resCenter.Y;
                        ctPt = AreaCt(Contour);
                        Res.cx = ctPt.X;
                        Res.cy = ctPt.Y;
                        Vertex = FindVertex(Contour, OverF20);
                        CirAccuracy(ctPt, Vertex, CARes);
                        Res.CircleAcuraccy = CARes.CA * STATIC.ScaleResolution[ch] * 1000;
                        (ShapePointF, ShapeLength) = FindShape(Contour, new OpenCvSharp.Point(ctPt.X, ctPt.Y));
                        Res.ShapeAccuracy = ShapeLength * STATIC.ScaleResolution[ch] * 1000;
                        Res.Area = Area;
                        break;
                    case InspectionType.InCircle_InspAll:
                        (resCenter, DesignC) = FindCover(img.Clone(), ctPt);
                        Res.Cover_cx = resCenter.X;
                        Res.Cover_cy = resCenter.Y;
                        (ctPt, DesignR) = InCircleCt(Contour);
                        Res.cx = ctPt.X;
                        Res.cy = ctPt.Y;
                        Vertex = FindVertex(Contour, OverF20);
                        CirAccuracy(ctPt, Vertex, CARes);
                        Res.CircleAcuraccy = CARes.CA * STATIC.ScaleResolution[ch] * 1000;
                        (ShapePointF, ShapeLength) = FindShape(Contour, new OpenCvSharp.Point(ctPt.X, ctPt.Y));
                        Res.ShapeAccuracy = ShapeLength * STATIC.ScaleResolution[ch] * 1000;
                        Res.Area = Area;

                        break;    
                }
                if(ResultShow)
                {

                    if(isSetup)
                    {
                      
                        using (Pen shapeLinePen = new Pen(Color.Green, 10f))
                        using (Pen shapeLinePen4 = new Pen(Color.Magenta, 10f))
                        using (Pen shapeLinePen5 = new Pen(Color.Cyan, 10f))
                        using (Pen shapeLinePen2 = new Pen(Color.Red, 10f))
                        using (Pen shapeLinePen3 = new Pen(Color.Yellow, 10f))
                        using (Pen abnomal = new Pen(Color.Blue, 10f))
                        using (Bitmap resBmp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(img))
                        using (Bitmap res2bmp = new Bitmap(resBmp.Width, resBmp.Height))
                        using (Graphics gr = Graphics.FromImage(res2bmp))
                        {
                            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            gr.DrawImage(resBmp, 0, 0);

                            switch (inspType)
                            {
                                case InspectionType.FindCover:
                                    gr.DrawEllipse(shapeLinePen4, (float)(resCenter.X - DesignC), (float)(resCenter.Y - DesignC), (float)(2 * DesignC), (float)(2 * DesignC));
                                    gr.DrawLine(shapeLinePen4, (float)resCenter.X - 3, (float)resCenter.Y, (float)resCenter.X + 3, (float)resCenter.Y);
                                    gr.DrawLine(shapeLinePen4, (float)resCenter.X, (float)resCenter.Y - 3, (float)resCenter.X, (float)resCenter.Y + 3);
                                    break;
                                case InspectionType.InCircle_Center:
                                    gr.DrawEllipse(shapeLinePen, (float)(ctPt.X - DesignR), (float)(ctPt.Y - DesignR), (float)(2 * DesignR), (float)(2 * DesignR));
                                    gr.DrawLine(shapeLinePen, (float)ctPt.X - 3, (float)ctPt.Y, (float)ctPt.X + 3, (float)ctPt.Y);
                                    gr.DrawLine(shapeLinePen, (float)ctPt.X, (float)ctPt.Y - 3, (float)ctPt.X, (float)ctPt.Y + 3);
                                    break;
                                case InspectionType.Area_Center:
                                    gr.DrawLine(shapeLinePen, (float)ctPt.X - 3, (float)ctPt.Y, (float)ctPt.X + 3, (float)ctPt.Y);
                                    gr.DrawLine(shapeLinePen, (float)ctPt.X, (float)ctPt.Y - 3, (float)ctPt.X, (float)ctPt.Y + 3);
                                    break;
                                case InspectionType.InCircle_Decenter:
                                    gr.DrawEllipse(shapeLinePen4, (float)(resCenter.X - DesignC), (float)(resCenter.Y - DesignC), (float)(2 * DesignC), (float)(2 * DesignC));
                                    gr.DrawLine(shapeLinePen4, (float)resCenter.X - 3, (float)resCenter.Y, (float)resCenter.X + 3, (float)resCenter.Y);
                                    gr.DrawLine(shapeLinePen4, (float)resCenter.X, (float)resCenter.Y - 3, (float)resCenter.X, (float)resCenter.Y + 3);
                                    gr.DrawEllipse(shapeLinePen, (float)(ctPt.X - DesignR), (float)(ctPt.Y - DesignR), (float)(2 * DesignR), (float)(2 * DesignR));
                                    gr.DrawLine(shapeLinePen, (float)ctPt.X - 3, (float)ctPt.Y, (float)ctPt.X + 3, (float)ctPt.Y);
                                    gr.DrawLine(shapeLinePen, (float)ctPt.X, (float)ctPt.Y - 3, (float)ctPt.X, (float)ctPt.Y + 3);
                                    break;
                                case InspectionType.Area_Decenter:
                                    gr.DrawEllipse(shapeLinePen4, (float)(resCenter.X - DesignC), (float)(resCenter.Y - DesignC), (float)(2 * DesignC), (float)(2 * DesignC));
                                    gr.DrawLine(shapeLinePen4, (float)resCenter.X - 3, (float)resCenter.Y, (float)resCenter.X + 3, (float)resCenter.Y);
                                    gr.DrawLine(shapeLinePen4, (float)resCenter.X, (float)resCenter.Y - 3, (float)resCenter.X, (float)resCenter.Y + 3);
                                    gr.DrawLine(shapeLinePen, (float)ctPt.X - 3, (float)ctPt.Y, (float)ctPt.X + 3, (float)ctPt.Y);
                                    gr.DrawLine(shapeLinePen, (float)ctPt.X, (float)ctPt.Y - 3, (float)ctPt.X, (float)ctPt.Y + 3);
                                    break;
                                case InspectionType.JustFind_Vertex:
                                    for (int i = 0; i < Vertex.Length; i++)
                                    {
                                        gr.DrawLine(abnomal, (float)Vertex[i].X - 3, (float)Vertex[i].Y, (float)Vertex[i].X + 3, (float)Vertex[i].Y);
                                        gr.DrawLine(abnomal, (float)Vertex[i].X, (float)Vertex[i].Y - 3, (float)Vertex[i].X, (float)Vertex[i].Y + 3);
                                    }
                                    break;
                                case InspectionType.Area_CircleAccuracy:
                                case InspectionType.InCircle_CircleAccuracy:
                                    gr.DrawLine(shapeLinePen, (float)ctPt.X, (float)ctPt.Y, (float)CARes.MaxPoint.X, (float)CARes.MaxPoint.Y);
                                    gr.DrawLine(shapeLinePen2, (float)ctPt.X, (float)ctPt.Y, (float)CARes.MinPoint.X, (float)CARes.MinPoint.Y);
                                    for (int j = 0; j < CARes.OtherVertex.Count; j++)
                                    {
                                        if (CARes.OtherVertex[j].X != 0 && CARes.OtherVertex[j].Y != 0)
                                            gr.DrawLine(shapeLinePen3, (float)ctPt.X, (float)ctPt.Y, (float)CARes.OtherVertex[j].X, (float)CARes.OtherVertex[j].Y);
                                    }
                                    break;
                                case InspectionType.Area_ShapeAccuracy:
                                case InspectionType.InCircle_ShapeAccuracy:

                                    if (Res.ShapeAccuracy > 0)
                                    {
                                        gr.DrawLine(abnomal, (float)ShapePointF.X - 3, (float)ShapePointF.Y, (float)ShapePointF.X + 3, (float)ShapePointF.Y);
                                        gr.DrawLine(abnomal, (float)ShapePointF.X, (float)ShapePointF.Y - 3, (float)ShapePointF.X, (float)ShapePointF.Y + 3);

                                    }
                                    break;
                                case InspectionType.Area_InspAll:
                                    gr.DrawEllipse(shapeLinePen5, (float)(resCenter.X - DesignC), (float)(resCenter.Y - DesignC), (float)(2 * DesignC), (float)(2 * DesignC));
                                    gr.DrawLine(shapeLinePen, (float)ctPt.X, (float)ctPt.Y, (float)CARes.MaxPoint.X, (float)CARes.MaxPoint.Y);
                                    gr.DrawLine(shapeLinePen2, (float)ctPt.X, (float)ctPt.Y, (float)CARes.MinPoint.X, (float)CARes.MinPoint.Y);
                                    for (int j = 0; j < CARes.OtherVertex.Count; j++)
                                    {
                                        if (CARes.OtherVertex[j].X != 0 && CARes.OtherVertex[j].Y != 0)
                                            gr.DrawLine(shapeLinePen3, (float)ctPt.X, (float)ctPt.Y, (float)CARes.OtherVertex[j].X, (float)CARes.OtherVertex[j].Y);
                                    }
                                    if (Res.ShapeAccuracy > 0)
                                    {
                                        gr.DrawLine(abnomal, (float)ShapePointF.X - 3, (float)ShapePointF.Y, (float)ShapePointF.X + 3, (float)ShapePointF.Y);
                                        gr.DrawLine(abnomal, (float)ShapePointF.X, (float)ShapePointF.Y - 3, (float)ShapePointF.X, (float)ShapePointF.Y + 3);

                                    }
                                    break;
                                case InspectionType.InCircle_InspAll:
                                    gr.DrawEllipse(shapeLinePen5, (float)(resCenter.X - DesignC), (float)(resCenter.Y - DesignC), (float)(2 * DesignC), (float)(2 * DesignC));
                                    gr.DrawEllipse(shapeLinePen4, (float)(ctPt.X - DesignR), (float)(ctPt.Y - DesignR), (float)(2 * DesignR), (float)(2 * DesignR));
                                    gr.DrawLine(shapeLinePen, (float)ctPt.X, (float)ctPt.Y, (float)CARes.MaxPoint.X, (float)CARes.MaxPoint.Y);
                                    gr.DrawLine(shapeLinePen2, (float)ctPt.X, (float)ctPt.Y, (float)CARes.MinPoint.X, (float)CARes.MinPoint.Y);
                                    for (int j = 0; j < CARes.OtherVertex.Count; j++)
                                    {
                                        if (CARes.OtherVertex[j].X != 0 && CARes.OtherVertex[j].Y != 0)
                                            gr.DrawLine(shapeLinePen3, (float)ctPt.X, (float)ctPt.Y, (float)CARes.OtherVertex[j].X, (float)CARes.OtherVertex[j].Y);
                                    }
                                    if (Res.ShapeAccuracy > 0)
                                    {
                                        gr.DrawLine(abnomal, (float)ShapePointF.X - 3, (float)ShapePointF.Y, (float)ShapePointF.X + 3, (float)ShapePointF.Y);
                                        gr.DrawLine(abnomal, (float)ShapePointF.X, (float)ShapePointF.Y - 3, (float)ShapePointF.X, (float)ShapePointF.Y + 3);

                                    }
                                    break;



                            }

                            STATIC.ResMat[ch] = res2bmp.ToMat();
                        }
                    }
                    else
                    {

                        //Task t = new Task(() => ProcessImageThr(ch, index, img.Clone(), inspType, ctPt, CARes, Res, DesignR, ShapePointF, CurrentPos, ItrCnt));
                        //t.Start();
                        ProcessImageThr(ch, index, img.Clone(), inspType, ctPt, CARes, Res, DesignR, ShapePointF, CurrentPos, DesignC, resCenter, ItrCnt);
                    }


                }


                return true;
            }
            catch
            {
                return false;
            }

        }
        object lockobj = new object();
        void ProcessImageThr(int ch, int index, Mat img, InspectionType type, PointF ctPt, CAResult CARes, InspResult Res, double DesignR, PointF ShapePointF, int CurPos, double DesignC, Point2D Coverctpt, int itrCnt)
        {
            lock(lockobj)
            {
                using (Pen shapeLinePen = new Pen(Color.Green, 10f))
                using (Pen shapeLinePen4 = new Pen(Color.Magenta, 10f))
                using (Pen shapeLinePen2 = new Pen(Color.Red, 10f))
                using (Pen shapeLinePen3 = new Pen(Color.Yellow, 10f))
                using (Pen shapeLinePen5 = new Pen(Color.Cyan, 10f))
                using (Pen abnomal = new Pen(Color.Blue, 10f))
                using (Bitmap resBmp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(img))
                using (Bitmap res2bmp = new Bitmap(resBmp.Width, resBmp.Height))
                using (Graphics gr = Graphics.FromImage(res2bmp))
                {
                    gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    gr.DrawImage(resBmp, 0, 0);

                    switch (type)
                    {

                        case InspectionType.Area_InspAll:
                            gr.DrawEllipse(shapeLinePen5, (float)(Coverctpt.X - DesignC), (float)(Coverctpt.Y - DesignC), (float)(2 * DesignC), (float)(2 * DesignC));
                            gr.DrawLine(shapeLinePen, (float)ctPt.X, (float)ctPt.Y, (float)CARes.MaxPoint.X, (float)CARes.MaxPoint.Y);
                            gr.DrawLine(shapeLinePen2, (float)ctPt.X, (float)ctPt.Y, (float)CARes.MinPoint.X, (float)CARes.MinPoint.Y);
                            for (int j = 0; j < CARes.OtherVertex.Count; j++)
                            {
                                if (CARes.OtherVertex[j].X != 0 && CARes.OtherVertex[j].Y != 0)
                                    gr.DrawLine(shapeLinePen3, (float)ctPt.X, (float)ctPt.Y, (float)CARes.OtherVertex[j].X, (float)CARes.OtherVertex[j].Y);
                            }
                            if (Res.ShapeAccuracy > 0)
                            {
                                gr.DrawLine(abnomal, (float)ShapePointF.X - 3, (float)ShapePointF.Y, (float)ShapePointF.X + 3, (float)ShapePointF.Y);
                                gr.DrawLine(abnomal, (float)ShapePointF.X, (float)ShapePointF.Y - 3, (float)ShapePointF.X, (float)ShapePointF.Y + 3);

                            }
                            break;
                        case InspectionType.InCircle_InspAll:
                            gr.DrawEllipse(shapeLinePen5, (float)(Coverctpt.X - DesignC), (float)(Coverctpt.Y - DesignC), (float)(2 * DesignC), (float)(2 * DesignC));
                            gr.DrawEllipse(shapeLinePen4, (float)(ctPt.X - DesignR), (float)(ctPt.Y - DesignR), (float)(2 * DesignR), (float)(2 * DesignR));
                            gr.DrawLine(shapeLinePen, (float)ctPt.X, (float)ctPt.Y, (float)CARes.MaxPoint.X, (float)CARes.MaxPoint.Y);
                            gr.DrawLine(shapeLinePen2, (float)ctPt.X, (float)ctPt.Y, (float)CARes.MinPoint.X, (float)CARes.MinPoint.Y);
                            for (int j = 0; j < CARes.OtherVertex.Count; j++)
                            {
                                if (CARes.OtherVertex[j].X != 0 && CARes.OtherVertex[j].Y != 0)
                                    gr.DrawLine(shapeLinePen3, (float)ctPt.X, (float)ctPt.Y, (float)CARes.OtherVertex[j].X, (float)CARes.OtherVertex[j].Y);
                            }
                            if (Res.ShapeAccuracy > 0)
                            {
                                gr.DrawLine(abnomal, (float)ShapePointF.X - 3, (float)ShapePointF.Y, (float)ShapePointF.X + 3, (float)ShapePointF.Y);
                                gr.DrawLine(abnomal, (float)ShapePointF.X, (float)ShapePointF.Y - 3, (float)ShapePointF.X, (float)ShapePointF.Y + 3);

                            }
                            break;
                    }

                    STATIC.ResMatOnProcess[ch][index] = res2bmp.ToMat();


                    if (STATIC.Rcp.Option.SaveImage)
                    {
                        string dateDir = STATIC.CreateDateDir();
                        dateDir += "Position_DrvImg\\";
                        if (!Directory.Exists(dateDir))
                            Directory.CreateDirectory(dateDir);
                        string path_res = string.Empty;
                        string path_resTxt = string.Empty;
                        path_res = string.Format("{0}{1}Position_{2}_{3}_{4}_Res.bmp", dateDir, STATIC.Process.m_StrIndex[ch], index, CurPos, itrCnt);
                        path_resTxt = string.Format("{0}{1}Position_{2}_{3}_{4}_ResTxt.txt", dateDir, STATIC.Process.m_StrIndex[ch], index, CurPos, itrCnt);


                        Bitmap b = STATIC.ResMatOnProcess[ch][index].ToBitmap();
                        b.Save(path_res);
                        string s = string.Format("CircleAccu L : {0 : 0.000} , CircleAccu R : {1 : 0.000}", Res.CircleAcuraccy, Res.CircleAcuraccy);
                        StreamWriter sw1 = new StreamWriter(path_resTxt);
                        sw1.WriteLine(s);
                        sw1.Close();
                    }
                }
            }


        }
    }
   
}
