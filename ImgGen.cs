using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public static class ImgGen {

    public static double[] getKernel(Bitmap bmp, int x, int y) {
        var index = 0;
        var kernel = new double[9];
        for (var nx = -1; nx <= +1; nx++) {
            for (var ny = -1; ny <= +1; ny++) {
                var wx = x + nx;
                var wy = y + ny;
                var inputColor = bmp.GetPixel(wx, wy);
                var inputGray = Util.ColorToGray(inputColor);
                kernel[index++] = (double)inputGray / 255.0;
            }
        }
        return kernel;
    }

    public static void test() {

        var image1 = new Bitmap("../../data/test2.png");
        var image2 = new Bitmap("../../data/test1.png");

        //train
        var inputs = new List<double[]>();
        var ouputs = new List<double[]>();
        for (var x = 1; x < image1.Width - 1; x++) {
            for (var y = 1; y < image1.Height - 1; y++) {

                var outputColor = image1.GetPixel(x, y);
                var outputGray = Util.ColorToGray(outputColor);
                var outputArray = Util.CreateOutputArray(256, outputGray);
                ouputs.Add(outputArray);

                var kernel = getKernel(image1, x, y);
                inputs.Add(kernel);

            }
        }

        var nn = new Network(9, 8, 256);
        nn.train(0.2, 100, inputs.ToArray(), ouputs.ToArray());

        //test
        var image3 = new Bitmap(image1.Width, image2.Height);
        for (var x = 1; x < image2.Width - 1; x++) {
            for (var y = 1; y < image2.Height - 1; y++) {

                //random color
                var kernel = getKernel(image2, x, y);
                var result = nn.think(kernel);
                var resultIndex = Util.RandomWeightedChoiceSorted(result);
                var color1 = Color.FromArgb(255, resultIndex, resultIndex, resultIndex);

                //back color
                var color2 = image2.GetPixel(x, y);

                //me
                //image3.SetPixel(x, y, color1);
                image3.SetPixel(x, y, Blend(color1, color2));

            }

        }

        image3.Save("../../data/test3.png");

        Console.WriteLine("bitti");

    }

    public static Color Blend(Color c1, Color c2) {
        var r = ((int)c1.R + (int)c2.R) / 2;
        var g = ((int)c1.G + (int)c2.G) / 2;
        var b = ((int)c1.B + (int)c2.B) / 2;
        return Color.FromArgb(255, r, g, b);
    }

}