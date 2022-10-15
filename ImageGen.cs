using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public static class ImageGen {

    private static Random rnd = new Random();

    public static void test() {

        var original = new Bitmap("../../data/view 240.png");

        var downScaled = new Bitmap(original, new Size(original.Width / 2, original.Height / 2));
        downScaled = new Bitmap(downScaled, new Size(original.Width, original.Height));
        downScaled.Save("../../data/deneme1.png");



        var nn = new Network(9, 18, 256);
        
        //train
        var trainOutput = new double[256];
        var trainInput = new double[9];
        for (var i = 0; i < 1000000; i++) {

            var sum = 0.0;
            for (var j = 0; j < 9; j++) {
                var r = rnd.NextDouble();
                sum += r;
                trainInput[j] = r;
            }
            sum /= 9;

            var choiseIndex = (int)(sum * 255);
            trainOutput[choiseIndex] = 1.0;
            nn.train(0.01, trainInput, trainOutput);
            trainOutput[choiseIndex] = 0.0;

        }






        //kernel
        double[] getKernel(Bitmap bmp, int x, int y) {
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

        ////train
        //var inputs = new List<double[]>();
        //var ouputs = new List<double[]>();
        //for (var x = 1; x < original.Width - 1; x++) {
        //    for (var y = 1; y < original.Height - 1; y++) {

        //        var outputColor = original.GetPixel(x, y);
        //        var outputGray = Util.ColorToGray(outputColor);
        //        var outputArray = Util.CreateOutputArray(256, outputGray);
        //        ouputs.Add(outputArray);

        //        var kernel = getKernel(original, x, y);
        //        inputs.Add(kernel);

        //    }
        //}

        //test
        var upScaled = new Bitmap(downScaled, new Size(downScaled.Width, downScaled.Height));
        for (var x = 1; x < downScaled.Width - 1; x++) {
            for (var y = 1; y < downScaled.Height - 1; y++) {

                var inputData = getKernel(downScaled, x, y);
                var result = nn.think(inputData);
                var outputIndex = Util.GetIndexOfMax(result);
                var outputColor = downScaled.GetPixel(x, y);

                var r = (outputIndex);
                var g = (outputIndex);
                var b = (outputIndex);
                var outputColor2 = Color.FromArgb(255, r, g, b);

                upScaled.SetPixel(x, y, outputColor2);

            }

        }
        upScaled.Save("../../data/deneme2.png");

        Console.WriteLine("finishe");

    }

    public static void testd() {

        var original = new Bitmap("../../data/view 240.png");

        var downScaled = new Bitmap(original, new Size(original.Width / 2, original.Height / 2));
        downScaled = new Bitmap(downScaled, new Size(original.Width, original.Height));
        downScaled.Save("../../data/deneme1.png");

        //kernel
        double[] getKernel(Bitmap bmp, int x, int y) {
            var index = 0;
            var kernel = new double[9];
            for (var nx = -1; nx <= +1; nx++) {
                for (var ny = -1; ny <= +1; ny++) {
                    var wx = x + nx;
                    var wy = y + ny;
                    var inputColor = bmp.GetPixel(wx, wy);
                    var inputGray =  Util.ColorToGray(inputColor);
                    kernel[index++] = (double)inputGray / 255.0;
                }
            }
            return kernel;
        }

        //train
        var inputs = new List<double[]>();
        var ouputs = new List<double[]>();
        for (var x = 1; x < original.Width - 1; x++) {
            for (var y = 1; y < original.Height - 1; y++) {

                var outputColor = original.GetPixel(x, y);
                var outputGray = Util.ColorToGray(outputColor);
                var outputArray = Util.CreateOutputArray(256, outputGray);
                ouputs.Add(outputArray);

                var kernel = getKernel(original, x, y);
                inputs.Add(kernel);

            }
        }

        var nn = new Network(9, 32, 256);
        nn.train(1, 1, inputs.ToArray(), ouputs.ToArray());

        //test
        var upScaled = new Bitmap(downScaled, new Size(downScaled.Width, downScaled.Height));
        for (var x = 1; x < downScaled.Width - 1; x++) {
            for (var y = 1; y < downScaled.Height - 1; y++) {

                var inputData = getKernel(downScaled, x, y);
                var result = nn.think(inputData);
                var outputIndex = Util.GetIndexOfMax(result);
                var outputColor = downScaled.GetPixel(x, y);

                var r = (outputIndex);
                var g = (outputIndex);
                var b = (outputIndex);
                var outputColor2 = Color.FromArgb(255, r, g, b);

                upScaled.SetPixel(x, y, outputColor2);

            }

        }
        upScaled.Save("../../data/deneme2.png");

        Console.WriteLine("finishe");

    }

    //public static void test() {

    //    var big = new Bitmap("../../data/view 240.png");

    //    var small = new Bitmap(big, new Size(big.Width / 2, big.Height / 2));
    //    var small2 = new Bitmap(small, new Size(big.Width, big.Height));
    //    small2.Save("../../data/deneme1.png");

    //    var small3 = new Bitmap(small2, new Size(big.Width, big.Height));

    //    var inputs = new List<double[]>();
    //    var ouputs = new List<double[]>();

    //    for (var x = 2; x < big.Width - 2; x++) {
    //        for (var y = 2; y < big.Height - 2; y++) {

    //            var inputData = new double[3 * 3 * 3];
    //            var outputData = new double[3 * 3 * 3];
    //            var i1 = 0;
    //            var i2 = 0;

    //            for (var nx = -1; nx <= +1; nx++) {
    //                var wx = x + nx;

    //                for (var ny = -1; ny <= +1; ny++) {
    //                    var wy = y + ny;

    //                    var c = small2.GetPixel(wx, wy);
    //                    inputData[i1++] = (double)c.R / 256.0;
    //                    inputData[i1++] = (double)c.R / 256.0;
    //                    inputData[i1++] = (double)c.G / 256.0;

    //                    c = big.GetPixel(wx, wy);
    //                    inputData[i2++] = (double)c.R / 256.0;
    //                    inputData[i2++] = (double)c.G / 256.0;
    //                    inputData[i2++] = (double)c.B / 256.0;

    //                }
    //            }

    //            inputs.Add(inputData);
    //            ouputs.Add(outputData);

    //        }
    //    }

    //    var ls = 3 * 3 * 3;
    //    var nn = new Network(ls, ls, ls);
    //    nn.train(0.1, 1, inputs.ToArray(), ouputs.ToArray());

    //    //test
    //    var testInput = new List<double[]>();
    //    for (var x = 1; x < big.Width - 1; x++) {

    //        for (var y = 1; y < big.Height - 1; y++) {
    //            var inputData = new double[3 * 3 * 3];
    //            var i = 0;

    //            for (var nx = -1; nx <= +1; nx++) {
    //                var wx = x + nx;

    //                for (var ny = -1; ny <= +1; ny++) {
    //                    var wy = y + ny;

    //                    var c = small2.GetPixel(wx, wy);
    //                    inputData[i++] = (double)c.R / 256.0;
    //                    inputData[i++] = (double)c.R / 256.0;
    //                    inputData[i++] = (double)c.G / 256.0;

    //                }
    //            }

    //            var result = nn.think(inputData);

    //            i = 0;
    //            for (var nx = -1; nx <= +1; nx++) {
    //                var wx = x + nx;

    //                for (var ny = -1; ny <= +1; ny++) {
    //                    var wy = y + ny;

    //                    var c = small2.GetPixel(wx, wy);
    //                    var r = ((int)(Math.Floor(result[i++] * 256.0) + c.R)) / 2;
    //                    var g = ((int)(Math.Floor(result[i++] * 256.0) + c.G)) / 2;
    //                    var b = ((int)(Math.Floor(result[i++] * 256.0) + c.B)) / 2;

    //                    c = Color.FromArgb(255, r, g, b);
    //                    small3.SetPixel(wx, wy, c);

    //                }
    //            }



    //        }

    //    }

    //    small3.Save("../../data/deneme2.png");


    //}

    private static int weightedRandomChooice(double[] p) {
        var rand = rnd.NextDouble();
        var sum = 0.0;
        for (var i = 0; i < p.Length; i++) {
            var nsum = sum + p[i];
            if (rand >= sum && rand <= nsum) {
                return i;
            }
            sum = nsum;
        }
        return p.Length - 1;
    }

    private static double normalizeChar(string usedCharacters, char c) {
        return (double)usedCharacters.IndexOf(c) / usedCharacters.Length;
    }

}