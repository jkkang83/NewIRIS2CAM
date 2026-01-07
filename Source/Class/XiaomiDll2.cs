using System.Runtime.InteropServices;


namespace DavinciIRISTester
{
    public class XiaomiDll2
    {
        unsafe public struct INPUTINFO
        {
            public byte* grayBuffer;
            public int height;
            public int width;
            public double pixelRatio;
            public double VAcropRatio;
            public double VAthresh;
            public double VAthresh2;
            public double coverDiameterPixel;
            public bool LogSave;
            public int testFlag;
            public bool F14Flag;
            public string LogPath;
        }
        unsafe public struct RESULT
        {
            public double VA_x_circle;
            public double VA_y_circle;
            public double VA_diameter_polygon;
            public double VA_area;
            public double VA_circleAccuracy;
            public double VA_shapeAccuracy;
            public double cover_x;
            public double cover_y;
            public double cover_diameter;
        }

        [DllImport("IRIS_Circle_Test.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int IRIS_Circle_Test(INPUTINFO inputInfo, out RESULT result);

    }
}
