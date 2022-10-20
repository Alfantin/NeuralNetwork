using System;
using System.IO;
using System.Text;

public class NeuralNetwork {

    private int[] layers;
    private double[][] neurons;
    private double[][] biases;
    private double[][,] weights;

    public NeuralNetwork(params int[] layers) {

        this.layers = layers;

        neurons = new double[layers.Length][];
        for (var i = 0; i < layers.Length; i++) {
            neurons[i] = new double[layers[i]];
        }

        weights = new double[layers.Length - 1][,];
        biases = new double[layers.Length - 1][];
        for (var i = 1; i < layers.Length; i++) {
            weights[i - 1] = new double[layers[i], layers[i - 1]];
            biases[i - 1] = new double[layers[i]];
        }

        var random = new Random();
        for (var i = 1; i < layers.Length; i++) {
            for (var j = 0; j < layers[i]; j++) {
                for (var k = 0; k < layers[i - 1]; k++) {
                    weights[i - 1][j, k] = random.NextDouble() * 2.0 - 1.0;
                }
            }
        }

    }

    public double[] think(params double[] inputs) {

        if (inputs == null) {
            throw new ArgumentException("Missing arguments");
        }

        if (inputs.Length != layers[0]) {
            throw new ArgumentException("Input size is wrong");
        }

        for (var i = 0; i < inputs.Length; i++) {
            neurons[0][i] = inputs[i];
        }

        for (var i = 1; i < layers.Length; i++) {
            for (var j = 0; j < layers[i]; j++) {
                var value = biases[i - 1][j];
                for (var k = 0; k < layers[i - 1]; k++) {
                    value += neurons[i - 1][k] * weights[i - 1][j, k];
                }
                neurons[i][j] = sigmoidActivation(value);
            }
        }

        return neurons[layers.Length - 1];

    }

    public void train(double learningRate, double[] inputs, double[] expected) {
        train(learningRate, 1, new double[][] { inputs }, new double[][] { expected });
    }

    public void train(double learningRate, int epoch, double[][] inputs, double[][] expected) {

        if (inputs == null || expected == null) {
            throw new ArgumentException("Missing arguments");
        }

        if (inputs.Length != expected.Length) {
            throw new ArgumentException("Input and output size is not equal");
        }

        var errors = new double[layers.Length][];
        for (var i = 0; i < layers.Length; i++) {
            errors[i] = new double[layers[i]];
        }

        var oldWeights = new double[layers.Length - 1][,];
        for (var i = 1; i < layers.Length; i++) {
            oldWeights[i - 1] = new double[layers[i], layers[i - 1]];
        }

        while (epoch-- > 0) {
            for (var t = 0; t < inputs.Length; t++) {

                think(inputs[t]);

                for (var i = layers.Length - 1; i > 0; i--) {
                    for (var j = 0; j < layers[i]; j++) {

                        var error = 0.0;
                        if (i == layers.Length - 1) {
                            error = sigmoidDerivative(neurons[i][j]) * (neurons[i][j] - expected[t][j]);
                        }
                        else {
                            for (var k = 0; k < layers[i + 1]; k++) {
                                error += sigmoidDerivative(neurons[i][j]) * (oldWeights[i][k, j] * errors[i + 1][k]);
                            }
                        }

                        biases[i - 1][j] -= error * learningRate;
                        for (var k = 0; k < layers[i - 1]; k++) {
                            oldWeights[i - 1][j, k] = weights[i - 1][j, k];
                            weights[i - 1][j, k] -= neurons[i - 1][k] * error * learningRate;
                        }

                        errors[i][j] = error;

                    }
                }

            }
        }

    }

    public void load(string fileName) {
        if (File.Exists(fileName)) {
            using (var stream = File.Open(fileName, FileMode.Open)) {
                using (var reader = new BinaryReader(stream, Encoding.UTF8, false)) {
                    for (var i = 0; i < layers.Length - 1; i++) {
                        for (var j = 0; j < layers[i]; j++) {
                            biases[i][j] = reader.ReadDouble();
                            for (var k = 0; k < layers[i]; k++) {
                                weights[i][j, k] = reader.ReadDouble();
                            }
                        }

                    }
                }
            }
        }
    }

    public void save(string fileName) {
        using (var stream = File.Open(fileName, FileMode.Create)) {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false)) {
                for (var i = 0; i < layers.Length - 1; i++) {
                    for (var j = 0; j < layers[i]; j++) {
                        writer.Write(biases[i][j]);
                        for (var k = 0; k < layers[i]; k++) {
                            writer.Write(weights[i][j, k]);
                        }
                    }
                }
            }
        }
    }

    private double sigmoidActivation(double x) {
        return 1.0 / (1.0 + Math.Exp(-x));
    }

    private double sigmoidDerivative(double x) {
        return x * (1.0 - x);
    }

}