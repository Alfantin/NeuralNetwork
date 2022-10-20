using System;

public class NetworkNew {

    private int[] layers;
    private double[][] neurons;
    private double[][] biases;
    private double[][][] weights;
    private double[][][] oldWeights;

    public NetworkNew(params int[] layers) {

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
                biases[i - 1][j] = 0; // r.NextDouble() - 0.5; // * 2.0 - 1.0;
                for (var k = 0; k < layers[i - 1]; k++) {
                    weights[i - 1][j][k] = r.NextDouble() - 0.5;// * 2.0 - 1.0;
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
                    error = sigmoidDerivative(neurons[i][j]) * (neurons[i][j] - expected[j]);
                }
                else {
                    for (var k = 0; k < layers[i + 1]; k++) {
                        error += sigmoidDerivative(neurons[i][j]) * (oldWeights[i][k][j] * errorBackup[k]);
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
        return 1.0 / (1.0 + Math.Exp(-x));
    }

    private double sigmoidDerivative(double x) {
        return x * (1.0 - x);
    }

}