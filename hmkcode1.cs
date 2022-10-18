using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hmkcode1 {

    public class Net {

        private int[] layers;
        private double[][] neurons;
        private double[][] biases;
        public double[][][] weights;
        private double[][][] oldWeights;
        private double[] errors;
        private double[] expected;

        public Net(params int[] layers) {

            this.layers = layers;

            errors = new double[layers[layers.Length-1]];
            expected = new double[layers.Length];

            //initialize neurons
            neurons = new double[layers.Length][];
            for (var i = 0; i < layers.Length; i++) {
                neurons[i] = new double[layers[i]];
            }

            //initialize weights and biases
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
                    biases[i - 1][j] = (r.NextDouble() * 2.0 - 1.0);
                    for (var k = 0; k < layers[i - 1]; k++) {
                        weights[i - 1][j][k] = (r.NextDouble() * 2.0 - 1.0);
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
                    var value = 0.0;// biases[i - 1][j];
                    for (var k = 0; k < layers[i - 1]; k++) {
                        var a = neurons[i - 1][k];
                        var b = weights[i - 1][j][k];
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

            var output = think(inputs);

            //update output layer
            var outputErrors = new double[layers[layers.Length - 1]];

            for (var j = 0; j < layers[layers.Length - 1]; j++) {

                var error = sigmoidDerivative(output[j]) * (expected[j] - output[j]);

                biases[layers.Length - 2][j] += error * learningRate;
                for (var k = 0; k < layers[layers.Length - 2]; k++) {
                    weights[layers.Length - 2][j][k] += neurons[layers.Length - 2][k] * error * learningRate;
                }
                outputErrors[j] = error;
            }

            //update input & hidden (weight [layer sayısı]  [a nöron sayısı] [b nöron sayısı])
            for (var i = layers.Length - 2; i > 0; i--) {
                var errors = new double[layers[i]];
                for (var j = 0; j < layers[i]; j++) {

                    var totalError = 0.0;
                    for (var k = 0; k < outputErrors.Length; k++) {
                        totalError += outputErrors[k] * weights[i][k][j];
                    }
                    totalError *= sigmoidDerivative(neurons[i][j]);

                    errors[j] = totalError;

                    biases[i - 1][j] += totalError * learningRate;
                    for (var k = 0; k < layers[i - 1]; k++) {
                        weights[i - 1][j][k] += neurons[i - 1][k] * totalError * learningRate;
                    }

                }
                outputErrors = errors;
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

        //int[] layers;
        //double[][] neurons;
        //double[][][] weights;

        //int NEURON_INDEX = 0;
        //int LAYER_INDEX = 0;

        double LEARNING_RATE = 0.05;

        //weight row her bir sol nöron
        //weight col her bir sağ nöron

        public void train2(double learningRate, double[] inputs, double[] expected) {

            for (var i = 0; i < inputs.Length; i++) {
                neurons[0][i] = inputs[i];
            }

            this.expected = expected;
            LEARNING_RATE = learningRate;

            calculateError();

            for (var i = layers.Length - 2; i >= 0; i--) {
                for (var r = 0; r < layers[i]; r++) { //weight row her bir sol nöron
                    for (var c = 0; c < layers[i + 1]; c++) { //weight col her bir sağ nöron
                        //Console.WriteLine(weights[i][c][r]);
                        oldWeights[i][c][r] = weights[i][c][r];
                        weights[i][c][r] = oldWeights[i][c][r] - (LEARNING_RATE * sigmoidActivation(neurons[i][r]) * partial(i, r, c));
                        Console.WriteLine(weights[i][c][r]);
                    }
                }
            }

        }

        double partial(int wl, int r, int c) {
            var gradient = 0.0;
            if (wl + 1 < weights.Length) {
                for (var i = 0; i < layers[wl + 2]; i++) {
                    //Console.WriteLine(c + "-" + i);
                    //Console.WriteLine(oldWeights[wl + 1][i][c]);
                    gradient += sigmoidDerivative(neurons[wl + 1][c]) * oldWeights[wl + 1][i][c] * partial(wl + 1, c, i);
                    //Console.WriteLine(gradient);
                }
            }
            else {
                return errors[c];
            }
            return gradient;
        }

        void calculateError() {
            for (var i = 0; i < errors.Length; i++) {
                errors[i] = neurons[layers.Length - 1][i] - expected[i];
            }
        }

    }


    public static class Test {

        public static void test() {

            var nn = new Net(2, 2, 1);

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

            nn.train2(0.05, new double[] { 2.0, 3.0 }, new double[] { 1.0 });

            Console.WriteLine("asd = " + nn.think(new double[] { 2, 3 })[0]);


        }

    }

}