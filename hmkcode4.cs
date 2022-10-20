using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hmkcode4 {

    public class Network {

        private int[] layers;
        private double[][] neurons;
        private double[][] biases;
        public double[][][] weights;
        private double[][][] oldWeights;

        public Network(params int[] layers) {

            this.layers = layers;

            neurons = new double[layers.Length][];
            for (var i = 0; i < layers.Length; i++) {
                neurons[i] = new double[layers[i]];
            }

            var r = new Random();
            weights = new double[layers.Length - 1][][];
            oldWeights = new double[layers.Length - 1][][];
            biases = new double[layers.Length - 1][];
            for (var i = 1; i < layers.Length; i++) {

                weights[i - 1] = new double[layers[i]][];
                oldWeights[i - 1] = new double[layers[i]][];
                biases[i - 1] = new double[layers[i]];
                for (var j = 0; j < layers[i]; j++) {

                    weights[i - 1][j] = new double[layers[i - 1]];
                    oldWeights[i - 1][j] = new double[layers[i - 1]];
                    biases[i - 1][j] = r.NextDouble() - 0.5;
                    for (var k = 0; k < layers[i - 1]; k++) {
                        weights[i - 1][j][k] = r.NextDouble() - 0.5;
                    }
                }

            }

        }

        public double[] think(params double[] inputs) {

            for (var i = 0; i < inputs.Length; i++) {
                neurons[0][i] = inputs[i];
            }

            for (var i = 1; i < layers.Length; i++) {
                for (var j = 0; j < layers[i]; j++) {
                    var value = 0.0; //biases[i - 1][j];
                    for (var k = 0; k < layers[i - 1]; k++) {
                        value += neurons[i - 1][k] * weights[i - 1][j][k];
                    }
                    neurons[i][j] = sigmoidActivation(value);
                }
            }

            return neurons[layers.Length - 1];

        }

        public void train(double learningRate, int count, double[][] inputs, double[][] expected) {
            if (inputs == null || expected == null) {
                throw new ArgumentException("Missing arguments");
            }
            if (inputs.Length != expected.Length) {
                throw new ArgumentException("Input and output size is not equal");
            }
            for (var j = 0; j < count; j++) {
                for (var i = 0; i < inputs.Length; i++) {
                    train(learningRate, inputs[i], expected[i]);
                }
            }
        }

        public void train(double learningRate, double[] inputs, double[] expected) {

            think(inputs);

            var errorBackup = default(double[]);
            for (var i = layers.Length - 1; i > 0; i--) {

                var errors = new double[layers[i]];
                for (var j = 0; j < errors.Length; j++) {

                    var error = 0.0;
                    if (i == layers.Length - 1) {
                        error = neurons[i][j] - expected[j];
                    }
                    else {
                        for (var k = 0; k < layers[i + 1]; k++) {
                            error += sigmoidDerivative(neurons[i][j]) * oldWeights[i][k][j] * errorBackup[k];
                        }
                    }

                    biases[i - 1][j] -= error * learningRate;
                    for (var k = 0; k < layers[i - 1]; k++) {
                        oldWeights[i - 1][j][k] = weights[i - 1][j][k];
                        weights[i - 1][j][k] -= neurons[i - 1][k] * error * learningRate;
                    }
                    errors[j] = error;

                }

                errorBackup = errors;

            }

        }

        private double sigmoidActivation(double x) {
            return x;
            //return 1.0 / (1.0 + Math.Exp(-x));
        }

        private double sigmoidDerivative(double x) {
            return x;
            //return x * (1.0 - x);
        }

    }

    public static class Test {

        public static void test() {

            var nn = new Network(1, 2, 3, 4);

            nn.weights = new double[][][] {
                new double[][]{
                    new double[] { .1 },
                    new double[] { .2 }
                },
                new double[][]{
                    new double[] { .3, .6 },
                    new double[] { .4, .7 },
                    new double[] { .5, .8 }
                },
                new double[][]{
                    new double[] { .9, .13, .17 },
                    new double[] { .10, .14, .18 },
                    new double[] { .11, .15, .19 },
                    new double[] { .12, .16, .20 },
                }
            };


            var r = nn.think(new double[] { 0.5 });
            Console.WriteLine($"0.5 => {r[0]:0.00} {r[1]:0.00} {r[2]:0.00} {r[3]:0.00}");

            nn.train(1, new double[] { 0.5 }, new double[] { 1, 2, 3, 4 });

            r = nn.think(new double[] { 0.5 });
            Console.WriteLine($"0.5 => {r[0]:0.00} {r[1]:0.00} {r[2]:0.00} {r[3]:0.00}");

        }

        public static void test2() {

            var nn = new Network(2, 2, 1);

            nn.weights = new double[][][] {
                new double[][]{
                    new double[] { .11, .21 },
                    new double[] { .12, .08 }
                },
                new double[][]{
                    new double[] { .14, .15 }
                }
            };

            Console.WriteLine("asd = " + nn.think(new double[] { 2, 3 })[0]);

            nn.train(1, new double[] { 2.0, 3.0 }, new double[] { 1.0 });

            Console.WriteLine("asd = " + nn.think(new double[] { 2, 3 })[0]);


        }

    }

}