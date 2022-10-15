using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

public static class ImageRecognize {

    public static void test() {

        //etiketleri bul
        var labels = new List<string>();
        foreach (var i in Directory.GetDirectories("../../train")) {
            labels.Add(Path.GetFileName(i));
        }

        //train data oluştur
        var inputs = new List<double[]>();
        var expecteds = new List<double[]>();
        var sampleLabels = new List<string>();
        for (var i = 0; i < labels.Count; i++) {
            var label = labels[i];
            var exp = new double[labels.Count];
            exp[i] = 1.0;
            foreach (var filePath in Directory.GetFiles("../../train/" + label, "*.png")) {
                inputs.Add(loadImage(filePath));
                expecteds.Add(exp);
                sampleLabels.Add(label);
            }
        }

        //train
        var nn = new Network(32 * 32, 16, labels.Count);
        nn.train(1.0, 25, inputs.ToArray(), expecteds.ToArray());

        //test
        var random = new Random();
        while (true) {

            var randomIndex = random.Next(inputs.Count);
            var result = nn.think(inputs[randomIndex]);

            var percent = result.Max();
            var resultIndex = Array.IndexOf(result, percent);

            Console.WriteLine("Test: " + sampleLabels[randomIndex]);
            Console.WriteLine("Guess:" + labels[resultIndex]);
            Console.WriteLine("Percent: %" + ((double)percent * 100.0));
            Console.WriteLine();

            Thread.Sleep(1000);

        }

    }

    private static double[] loadImage(string path) {
        var bmp = new Bitmap(path);
        var ret = new double[bmp.Width * bmp.Height];
        var i = 0;
        for (var x = 0; x < bmp.Width; x++) {
            for (var y = 0; y < bmp.Height; y++) {
                var color = bmp.GetPixel(x, y);
                ret[i++] = ((double)(color.R + color.G + color.B) / 3.0) / 256.0;
            }
        }
        return ret;
    }

}