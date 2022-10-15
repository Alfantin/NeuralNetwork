using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Util {

    private static Random rnd = new Random();

    public static int FloorToInt(double value) {
        return (int)Math.Floor(value);
    }

    public static int ColorToGray(this Color c) {
        return (c.R + c.G + c.B) / 3;
    }

    public static List<T> CreateIndexTable<T>(T[] samples) {
        var ret = new List<T>();
        foreach (var c in samples) {
            if (!ret.Contains(c)) ret.Add(c);
        }
        return ret;
    }

    public static double[] CreateOutputArray(int size, int choise) {
        var ret = new double[size];
        ret[choise] = 1.0;
        return ret;
    }

    public static double[] ExpectedOutput<T>(List<T> all, T expected) {
        var ret = new double[all.Count];
        ret[all.IndexOf(expected)] = 1.0;
        return ret;
    }

    public static int RandomWeightedChoiceSorted(double[] weightes) {

        var sorted = weightes.OrderByDescending(i => i).ToArray();
        var sortedIndex = RandomWeightedChoice(sorted);
        var sortedValue = sorted[sortedIndex];

        var normalIndex = Array.IndexOf(weightes, sortedValue);

        return normalIndex;
    }

    public static int RandomWeightedChoice(double[] weightes) {
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

    public static double[] createOutputArray<T>(T[] values, T expectedValue) {
        var ret = new double[values.Length];
        ret[Array.IndexOf(values, expectedValue)] = 1.0;
        return ret;
    }

    public static int GetIndexOfMax(double[] values) {
        var maxValue = values.Max();
        return Array.IndexOf(values, maxValue);
    }

    public static double NormalizeInput<T>(List<T> items, T item) {
        return (double)items.IndexOf(item) / items.Count;
    }

    public static double Random() {
        return rnd.NextDouble();
    }

    public static int Random(int max) {
        return rnd.Next(max);
    }

    public static int Random(int min, int max) {
        return rnd.Next(min, max);
    }

    //public static float ColorToNormalizedGray(Color value) {
    //    return Util.Floor((c.R + c.G + c.B) / 3);
    //}

}