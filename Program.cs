using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class Program {

    private static Random rnd = new Random();

    public static void Main(string[] args) {

        Console.WriteLine("XOR");
        xorTest();

        Console.WriteLine("TEXT GEN");
        generateText("Al bu takatukaları takatukacıya götür. Takatukacı bu taka tukaları takatukalatmazsa al bu takatukaları takatukacıdan takatukalatmadan geri getir.");

        Console.WriteLine("WORD GEN");
        generateWord("Al bu takatukaları takatukacıya götür. Takatukacı bu taka tukaları takatukalatmazsa al bu takatukaları takatukacıdan takatukalatmadan geri getir." );

        Console.WriteLine("IMAGE RECOGNIZE");
        imageRecognize();

        Console.ReadLine();

    }

    private static void xorTest() {

        var nn = new Network(2, 2, 1);

        nn.train(
            1,
            5000,
            new double[][] {
                new double[] { 0, 0 },
                new double[] { 1, 0 },
                new double[] { 0, 1 },
                new double[] { 1, 1 }
            },
            new double[][] {
                new double[] { 0 },
                new double[] { 1 },
                new double[] { 1 },
                new double[] { 0 }
            }
        );

        Console.WriteLine("0 XOR 0 = " + nn.think(new double[] { 0, 0 })[0]);
        Console.WriteLine("1 XOR 0 = " + nn.think(new double[] { 1, 0 })[0]);
        Console.WriteLine("0 XOR 1 = " + nn.think(new double[] { 0, 1 })[0]);
        Console.WriteLine("1 XOR 1 = " + nn.think(new double[] { 1, 1 })[0]);
        Console.WriteLine("-------------------------------------------------");

    }

    private static void generateText(string sample) {

        var sampleText = sample
            .ToLower()
            .Replace(".", " ")
            .Replace(",", " ")
            .Replace("?", " ")
            .Replace("!", " ")
            .Replace(";", " ")
            .Replace("'", " ")
            .Split(new char[] { ' ', '\'', '\r', '\n' },
            StringSplitOptions.RemoveEmptyEntries
        );

        var usedWords = CreateIndexTable(sampleText.ToArray());

        var input = new List<double[]>();
        var expected = new List<double[]>();

        var size = 3;
        for (var i = 0; i < sampleText.Length - size; i++) {

            var inp = new double[size];
            for (var j = 0; j < size; j++) {
                inp[j] = NormalizeInput(usedWords, sampleText[i + j]);
            }
            input.Add(inp);

            var expectedOutput = ExpectedOutput(usedWords, sampleText[i + size]);
            expected.Add(expectedOutput);

        };

        var nn = new Network(size, 5, usedWords.Count);
        nn.train(0.25, 5000, input.ToArray(), expected.ToArray());

        var outputs = new List<string>();
        for (var i = 0; i < size; i++) {
            outputs.Add(usedWords[Random(usedWords.Count)]);
        }

        for (var i = 0; i < 30; i++) {

            var inp = new double[size];
            for (var j = 0; j < size; j++) {
                inp[j] = NormalizeInput(usedWords, outputs[outputs.Count - (size - j)]);
            }

            var result = nn.think(inp);
            var resultIndex = RandomWeightedChoice(result);
            var resultValue = usedWords[resultIndex];
            outputs.Add(resultValue);

        }

        Console.WriteLine(string.Join(" ", outputs.Skip(size).ToArray()));
        Console.WriteLine("-------------------------------------------------");

    }

    private static void generateWord(string sample) {

        var sampleText = sample;
        sampleText = Regex.Replace(sampleText, @"\s+", " ");
        sampleText = sampleText.ToLower();
        var usedWords = CreateIndexTable(sampleText.ToArray());

        var input = new List<double[]>();
        var expected = new List<double[]>();
        var size = 3;
        for (var i = 0; i < sampleText.Length - size; i++) {

            var inp = new double[size];
            for (var j = 0; j < size; j++) {
                inp[j] = NormalizeInput(usedWords, sampleText[i + j]);
            }
            input.Add(inp);

            var expectedOutput = ExpectedOutput(usedWords, sampleText[i + size]);
            expected.Add(expectedOutput);

        };

        var nn = new Network(size, 8, usedWords.Count);
        nn.train(0.25, 5000, input.ToArray(), expected.ToArray());

        var outputs = "";
        for (var i = 0; i < size; i++) {
            outputs += usedWords[Random(usedWords.Count)];
        }

        for (var i = 0; i < 200; i++) {

            var inp = new double[size];
            for (var j = 0; j < size; j++) {
                inp[j] = NormalizeInput(usedWords, outputs[outputs.Length - (size - j)]);
            }

            var result = nn.think(inp);
            var resultIndex = RandomWeightedChoice(result);
            outputs += usedWords[resultIndex];

        }

        Console.WriteLine(outputs.Substring(size));
        Console.WriteLine("-------------------------------------------------");

    }

    private static void imageRecognize() {

        //etiketleri bul
        var labels = new List<string>();
        foreach (var i in Directory.GetDirectories("../../data/train")) {
            labels.Add(Path.GetFileName(i));
        }

        //train data oluştur
        var inputs = new List<double[]>();
        var expecteds = new List<double[]>();
        var sampleLabels = new List<string>();
        for (var i = 0; i < labels.Count; i++) {
            var label = labels[i];
            var exp = CreateOutputArray(labels.Count, i);
            foreach (var filePath in Directory.GetFiles("../../data/train/" + label, "*.png")) {
                inputs.Add(NormalizeImageBlackWhite(filePath));
                expecteds.Add(exp);
                sampleLabels.Add(label);
            }
        }

        //train
        var nn = new Network(32 * 32, 16, labels.Count);
        nn.train(1.0, 25, inputs.ToArray(), expecteds.ToArray());

        var randomIndex = Random(inputs.Count);
        var result = nn.think(inputs[randomIndex]);
        var percent = result.Max();
        var resultIndex = Array.IndexOf(result, percent);

        Console.WriteLine("Test: " + sampleLabels[randomIndex]);
        Console.WriteLine("Guess:" + labels[resultIndex]);
        Console.WriteLine("Percent: %" + ((double)percent * 100.0));
        Console.WriteLine("-------------------------------------------------");

    }

    private static int ColorToGray(Color c) {
        return (c.R + c.G + c.B) / 3;
    }

    private static List<T> CreateIndexTable<T>(T[] samples) {
        var ret = new List<T>();
        foreach (var c in samples) {
            if (!ret.Contains(c)) ret.Add(c);
        }
        return ret;
    }

    private static double[] CreateOutputArray(int size, int choise) {
        var ret = new double[size];
        ret[choise] = 1.0;
        return ret;
    }

    private static double[] ExpectedOutput<T>(List<T> all, T expected) {
        var ret = new double[all.Count];
        ret[all.IndexOf(expected)] = 1.0;
        return ret;
    }

    private static int RandomWeightedChoice(double[] weightes) {
        var rand = rnd.NextDouble();
        var sum = 0.0;
        for (var i = 0; i < weightes.Length; i++) {
            var nsum = sum + weightes[i];
            if (rand >= sum && rand <= nsum) {
                return i;
            }
            sum = nsum;
        }
        return weightes.Length - 1;
    }

    private static int GetIndexOfMax(double[] values) {
        return Array.IndexOf(values, values.Max());
    }

    private static double NormalizeInput<T>(List<T> items, T item) {
        return (double)items.IndexOf(item) / items.Count;
    }

    private static double Random() {
        return rnd.NextDouble();
    }

    private static int Random(int max) {
        return rnd.Next(max);
    }

    private static int Random(int min, int max) {
        return rnd.Next(min, max);
    }

    private static double[] NormalizeImageBlackWhite(string path) {
        var bmp = new Bitmap(path);
        var ret = new double[bmp.Width * bmp.Height];
        var i = 0;
        for (var x = 0; x < bmp.Width; x++) {
            for (var y = 0; y < bmp.Height; y++) {
                var color = bmp.GetPixel(x, y);
                ret[i++] = (double)ColorToGray(color) / 256.0;
            }
        }
        return ret;
    }

}