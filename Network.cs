using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

public class Network {

    private int[] layers;
    private double[][] neurons;
    private double[][] biases;
    private double[][][] weights;

    public Network(params int[] layers) {

        this.layers = layers;

        //initialize neurons
        neurons = new double[layers.Length][];
        for (var i = 0; i < layers.Length; i++) {
            neurons[i] = new double[layers[i]];
        }

        //initialize weights and biases
        var r = new Random();
        weights = new double[layers.Length - 1][][];
        biases = new double[layers.Length - 1][];
        for (var i = 1; i < layers.Length; i++) {

            weights[i - 1] = new double[layers[i]][];
            biases[i - 1] = new double[layers[i]];
            for (var j = 0; j < layers[i]; j++) {

                weights[i - 1][j] = new double[layers[i - 1]];
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
                var value = biases[i - 1][j];
                for (var k = 0; k < layers[i - 1]; k++) {
                    value += neurons[i - 1][k] * weights[i - 1][j][k];
                }
                neurons[i][j] = sigmoidActivation(value);
            }
        }

        return neurons[layers.Length - 1];

    }

    public void mutation() {
        for (var i = 1; i < layers.Length; i++) {
            for (var j = 0; j < layers[i]; j++) {
                biases[i - 1][j] = Util.Random() * 0.01 - 0.005;
                for (var k = 0; k < layers[i - 1]; k++) {
                    weights[i - 1][j][k] = Util.Random() * 0.01 - 0.005;
                }
            }
        }
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

    //private double sigmoidActivation(double x) {
    //    return 1.0 / (1.0 + Math.Exp(-x));
    //}

    //private double sigmoidDerivative(double x) {
    //    return x * (1.0 - x);
    //}
    private double sigmoidActivation(double x) {
        return 1.0 / (1.0 + Math.Exp(-x));
    }

    private double sigmoidDerivative(double x) {
        return x * (1.0 - x);
    }
}


//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.IO;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Threading;

//public class Network {

//    private int[] layers;
//    private double[][] neurons;
//    private double[][] biases;
//    private double[][][] weights;

//    public Network(params int[] layers) {

//        this.layers = layers;

//        //initialize neurons
//        neurons = new double[layers.Length][];
//        for (var i = 0; i < layers.Length; i++) {
//            neurons[i] = new double[layers[i]];
//        }

//        //initialize weights and biases
//        var r = new Random();
//        weights = new double[layers.Length - 1][][];
//        biases = new double[layers.Length - 1][];
//        for (var i = 1; i < layers.Length; i++) {

//            weights[i - 1] = new double[layers[i]][];
//            biases[i - 1] = new double[layers[i]];
//            for (var j = 0; j < layers[i]; j++) {

//                weights[i - 1][j] = new double[layers[i - 1]];
//                biases[i - 1][j] = (r.NextDouble() * 2.0 - 1.0);
//                for (var k = 0; k < layers[i - 1]; k++) {
//                    weights[i - 1][j][k] = (r.NextDouble() * 2.0 - 1.0);
//                }
//            }

//        }

//    }

//    public double[] think(params double[] inputs) {

//        for (var i = 0; i < inputs.Length; i++) {
//            neurons[0][i] = inputs[i];
//        }

//        for (var i = 1; i < layers.Length; i++) {
//            for (var j = 0; j < layers[i]; j++) {
//                var value = biases[i - 1][j];
//                for (var k = 0; k < layers[i - 1]; k++) {
//                    value += neurons[i - 1][k] * weights[i - 1][j][k];
//                }
//                neurons[i][j] = sigmoidActivation(value);
//            }
//        }

//        return neurons[layers.Length - 1];

//    }

//    public void mutation() {
//        for (var i = 1; i < layers.Length; i++) {
//            for (var j = 0; j < layers[i]; j++) {
//                biases[i - 1][j] = Util.Random() * 0.01 - 0.005;
//                for (var k = 0; k < layers[i - 1]; k++) {
//                    weights[i - 1][j][k] = Util.Random() * 0.01 - 0.005;
//                }
//            }
//        }
//    }

//    public void train(double learningRate, int count, double[][] inputs, double[][] expected) {
//        if (inputs == null || expected == null) {
//            throw new ArgumentException("Missing arguments");
//        }
//        if (inputs.Length != expected.Length) {
//            throw new ArgumentException("Input and output size is not equal");
//        }
//        for (var j = 0; j < count; j++) {
//            for (var i = 0; i < inputs.Length; i++) {
//                train(learningRate, inputs[i], expected[i]);
//            }
//        }
//    }

//    public void train(double learningRate, double[] inputs, double[] expected) {

//        var output = think(inputs);

//        //update output layer
//        var outputErrors = new double[layers[layers.Length - 1]];

//        for (var j = 0; j < layers[layers.Length - 1]; j++) {
//            var error = sigmoidDerivative(output[j]) * (expected[j] - output[j]);
//            biases[layers.Length - 2][j] += error * learningRate;
//            for (var k = 0; k < layers[layers.Length - 2]; k++) {
//                weights[layers.Length - 2][j][k] += neurons[layers.Length - 2][k] * error * learningRate;
//            }
//            outputErrors[j] = error;
//        }

//        //update input & hidden (weight [layer sayısı]  [a nöron sayısı] [b nöron sayısı])
//        for (var i = layers.Length - 2; i > 0; i--) {
//            var errors = new double[layers[i]];
//            for (var j = 0; j < layers[i]; j++) {

//                var totalError = 0.0;
//                for (var k = 0; k < outputErrors.Length; k++) {
//                    totalError += outputErrors[k] * weights[i][k][j];
//                }
//                totalError *= sigmoidDerivative(neurons[i][j]);
//                errors[j] = totalError;

//                biases[i - 1][j] += totalError * learningRate;
//                for (var k = 0; k < layers[i - 1]; k++) {
//                    weights[i - 1][j][k] += neurons[i - 1][k] * totalError * learningRate;
//                }

//            }
//            outputErrors = errors;
//        }

//    }

//    private double sigmoidActivation(double x) {
//        return 1.0 / (1.0 + Math.Exp(-x));
//    }

//    private double sigmoidDerivative(double x) {
//        return x * (1.0 - x);
//    }

//}