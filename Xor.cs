using System;

public static class Xor {

    public static void test() {

        var nn = new Network(2, 2, 1);

        nn.train(
            1,
            10000,
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

    }

}