using System;
using PowerArgs;

namespace ResFinder
{

    // A class that describes the command line arguments for this program
    public class AppArgs
    {
        [ArgShortcut("-w"), ArgDescription("Width original resolution")]
        [ArgRequired(PromptIfMissing = true)]
        public int Width { get; set; }

        [ArgShortcut("-h"), ArgDescription("Height original resolution")]
        [ArgRequired(PromptIfMissing = true)]
        public int Height { get; set; }

    }

    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                var parsed = Args.Parse<AppArgs>(args);
                Console.WriteLine($@"Finding valid resolutions for {parsed.Width}x{parsed.Height}");

                FindRes(parsed.Width, parsed.Height, 0.5, 2.0);

            }
            catch (ArgException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ArgUsage.GenerateUsageFromTemplate<AppArgs>());
            }


            

        }

        static void FindRes(int originalWRes, int originalHRes, double minScale, double maxScale)
        {
            int MaxPixelWidth = originalWRes;
            int MaxPixelHeight = originalHRes;

            int paddingSize = Math.Max(MaxPixelWidth, MaxPixelHeight).ToString().Length;

            double scale = minScale;

            while (scale <= maxScale)
            {

                double scaledWPixel = Math.Round(MaxPixelWidth / scale, 2);
                bool isValidW = (scaledWPixel - Math.Truncate(scaledWPixel)) == 0d;


                double scaledHPixel = Math.Round(MaxPixelHeight / scale, 2);
                bool isValidH = (scaledHPixel - Math.Truncate(scaledHPixel)) == 0d;

                if (isValidW && isValidH)
                {
                    string resW = $@"{scaledWPixel}".PadLeft(paddingSize, ' ');
                    string resH = $@"{scaledHPixel}".PadLeft(paddingSize, ' ');
                    string resInfo = $@"[{resW} x {resH}]";
                    Console.WriteLine($@"{resInfo} scale {scale}");
                }

                scale = Math.Round(scale + 0.01, 2);

            }

            Console.WriteLine($@"Finish");
        }
    }

}
