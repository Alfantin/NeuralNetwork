using System;

//https://github.com/snives/SimpleNeuralNetwork/
namespace NeuralNetwork2 {

    public class NeuralNetwork {

        private double[][] neurons;
        private double[][] biases;
        private double[][] errors;
        public double[][,] weights;
        private int lastLayer;
        
        public NeuralNetwork(params int[] layers) {

            var rnd = new Random(2);

            lastLayer = layers.Length - 1;

            neurons = new double[layers.Length][];
            biases = new double[layers.Length][];
            errors = new double[layers.Length][];
            for (var l = 0; l < layers.Length; l++) {
                neurons[l] = new double[layers[l]];
                biases[l] = new double[layers[l]];
                errors[l] = new double[layers[l]];
            }

            weights = new double[lastLayer][,];
            for (var l = 0; l < lastLayer; l++) {
                weights[l] = new double[layers[l], layers[l + 1]];
                for (var n = 0; n < layers[l]; n++) {
                    for (var j = 0; j < layers[l + 1]; j++) {
                        weights[l][n, j] = rnd.NextDouble() - 0.5;
                    }
                }
            }

        }

        public double[] think(double[] input) {

            for (var i = 0; i < neurons[0].Length; i++) {
                neurons[0][i] = input[i];
            }

            for (var i = 1; i < neurons.Length; i++) {
                for (var j = 0; j < neurons[i].Length; j++) {
                    var sum = 0.0;//biases[i][j];
                    for (var k = 0; k < neurons[i - 1].Length; k++) {
                        sum += neurons[i - 1][k] * weights[i - 1][k, j];
                    }
                    neurons[i][j] = sigmoidActivation(sum);
                }
            }

            var output = new double[neurons[lastLayer].Length];
            for (var i = 0; i < output.Length; i++) {
                output[i] = neurons[lastLayer][i];
            }
            return output;

        }

        public void train(double learningRate, int iteration, double[][] inputs, double[][] expected) {

            if (inputs == null || expected == null) {
                throw new ArgumentException("Missing arguments");
            }

            if (inputs.Length != expected.Length) {
                throw new ArgumentException("Input and output size is not equal");
            }

            while (iteration-- > 0) {
                for (var s = 0; s < inputs.Length; s++) {

                    think(inputs[s]);

                    for (var n = 0; n < neurons[lastLayer].Length; n++) {
                        errors[lastLayer][n] = neurons[lastLayer][n] - expected[s][n];
                    }

                    for (var i = lastLayer - 1; i > 0; i--) {
                        for (var j = 0; j < neurons[i].Length; j++) {
                            for (var k = 0; k < neurons[i + 1].Length; k++) {
                                errors[i][j] += sigmoidDerivative(neurons[i][j]) * weights[i][j, k] * errors[i + 1][k];
                            }
                        }
                    }

                    for (var i = 0; i < lastLayer; i++) {
                        for (var j = 0; j < neurons[i + 1].Length; j++) {
                            biases[i + 1][j] -= errors[i + 1][j] * learningRate;
                            for (var k = 0; k < neurons[i].Length; k++) {
                                weights[i][k, j] -= sigmoidDerivative(neurons[i][k]) * errors[i + 1][j] * learningRate;
                            }
                        }
                    }

                }
            }

        }

        private static double sigmoidActivation(double x) {
            return x;
            //return 1.0 / (1.0 + Math.Exp(-x));
        }

        private static double sigmoidDerivative(double x) {
            return x;
            //return x * (1.0 - x);
        }

    }

    public static class Test {

        public static void test() {

            var nn = new NeuralNetwork(1, 2, 3, 4);

            nn.weights = new double[][,] {
                new double[,]{
                    { .1, .2 }
                },
                new double[,]{
                    { .3, .4, .5 },
                    { .6, .7, .8 }
                },
                new double[,]{
                    { .9, .10, .11, .12 },
                    { .13, .14, .15, .16 },
                    { .17, .18, .19, .20 }
                }
            };

            var r = nn.think(new double[] { 0.5 });
            Console.WriteLine($"0.5 => {r[0]:0.00} {r[1]:0.00} {r[2]:0.00} {r[3]:0.00}");

            nn.train(1, 1, new[] { new double[] { 2.0, 3.0 } }, new[] { new double[] { 1, 2, 3, 4 } });

            r = nn.think(new double[] { 0.5 });
            Console.WriteLine($"0.5 => {r[0]:0.00} {r[1]:0.00} {r[2]:0.00} {r[3]:0.00}");

        }


    }

}