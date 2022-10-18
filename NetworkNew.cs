using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

//https://hmkcode.com/ai/backpropagation-step-by-step
public class deneme {

    public static void test() {

        var Inputs = new double[] { 2.0, 3.0 };

        var Expecteds = new double[] { 1.0 };

        var Layers = new int[] { 2, 2, 1 };

        var Values = new double[][] {
            new double[] { 2.0, 3.0 },      
            new double[] { 0.0, 0.0 },      
            new double[] { 0.0 },           
        };

        var Weights = new double[][][] {
            new double[][]{                 
                new double[] { 0.11, 0.21 },
                new double[] { 0.12, 0.08 } 
            },
            new double[][]{                 
                new double[] { 0.14, 0.15 },
            },
            new[]{
                new double[] { 1 }
            }
        };

        var LAYER_COUNT = Layers.Length;
        var OUTPUT_LAYER = LAYER_COUNT - 1;

        //signal
        for (var n = 0; n < Inputs.Length; n++) {
            Values[0][n] = Inputs[n];
        }

        //FEED FORWARD
        for (var i = 1; i < LAYER_COUNT; i++) {

            //layer nöronlarının değerlerini hesapla
            for (var j = 0; j < Layers[i]; j++) {

                var sum = 0.0;

                //bir önceki layer nöronlarından ağırlık ile değer al
                for (var k = 0; k < Layers[i - 1]; k++) {
                    sum += Values[i - 1][k] * Weights[i - 1][j][k];
                }

                Values[i][j] = sum;

            }
        }

        //BACK 

        var learningRate = 0.05;
        var delta = Values[2][0] - Expecteds[0];

        //LAYER 1 NEURON 0
        var w5d = Weights[1][0][0] - Values[1][0] * delta * learningRate;                    //0.14 - 0.85 * (-0.809 * 0.05)        = 0.17
        var w6d = Weights[1][0][1] - Values[1][1] * delta * learningRate;                    //0.15 - 0.48 * (-0.809 * 0.05)        = 0.17

        //LAYER 2 NEURON 0
        var w1d = Weights[0][0][0] - Values[0][0] * delta * learningRate * Weights[1][0][0]; //0.11 - 2.00 * (-0.809 * 0.05) * 0.14 = 0.12
        var w2d = Weights[0][0][1] - Values[0][1] * delta * learningRate * Weights[1][0][0]; //0.21 - 3.00 * (-0.809 * 0.05) * 0.14 = 0.23

        //LAYER 2 NEURON 1
        var w3d = Weights[0][1][0] - Values[0][0] * delta * learningRate * Weights[1][0][1]; //0.12 - 2.00 * (-0.809 * 0.05) * 0.15 = 0.13
        var w4d = Weights[0][1][1] - Values[0][1] * delta * learningRate * Weights[1][0][1]; //0.08 - 3.00 * (-0.809 * 0.05) * 0.15 = 0.1

        for (var i = Layers.Length - 2; i >= 0; i--) {
            Console.WriteLine("LAYER " + i);


            for (var j = 0; j < Layers[i + 1]; j++) {
                Console.WriteLine(" NEURON " + j);

                if (i == Layers.Length - 2) {
                    delta = Values[i + 1][j] - Expecteds[j];
                }

                var previousWeight = 0.0;
                for (var k = 0; k < Layers[i + 1]; k++) {
                    previousWeight = Weights[i][j][k];
                    var asdas = 0;
                }
                var asdasd = 0.0;
                Console.WriteLine(previousWeight);

                //var asd1 = new double[Weights[i][j].Length];
                //var asd2 = new double[Weights[i][j].Length];

                //var sumError = 0.0;
                //for (var k = 0; k < Layers[i]; k++) {
                //    sumError = Weights[i][0][k];
                //    var sdfsfds = 0;
                //}

                for (var k = 0; k < Layers[i]; k++) {

                    var w = Weights[i][j][k] - Values[i][k] * delta * learningRate * Weights[i + 1][0][j];

                    Console.WriteLine($"   Weights[{i}][{j}][{k}] - Values[{i}][{k}] * delta * rate * Weights[{i + 1}][0][{j}] = {w}");

                }


            }


        }


    }



    public static void testb() {

        var Inputs = new double[] { 2.0, 3.0 };

        var Expecteds = new double[] { 1.0 };

        var Layers = new int[] { 2, 2, 1 };

        var Values = new double[][] {
            new double[] { 2.0, 3.0 },       // L0
            new double[] { 0.0, 0.0 },       // L1
            new double[] { 0.0 },            // L2
        };

        var Weights = new double[][][] {
            new double[][]{                  // L0
                new double[] { 0.11, 0.21 }, // W1, W2
                new double[] { 0.12, 0.08 }  // W3, W4
            },
            new double[][]{                  // L1
                new double[] { 0.14, 0.15 }, // W5, W6
            },
            new[]{
                new double[] { 1 }
            }
        };

        var LAYER_COUNT = Layers.Length;
        var OUTPUT_LAYER = LAYER_COUNT - 1;

        //signal
        for (var n = 0; n < Inputs.Length; n++) {
            Values[0][n] = Inputs[n];
        }

        //FEED FORWARD
        for (var i = 1; i < LAYER_COUNT; i++) {

            //layer nöronlarının değerlerini hesapla
            for (var j = 0; j < Layers[i]; j++) {

                var sum = 0.0;

                //bir önceki layer nöronlarından ağırlık ile değer al
                for (var k = 0; k < Layers[i - 1]; k++) {
                    sum += Values[i - 1][k] * Weights[i - 1][j][k];
                }

                Values[i][j] = sum;

            }
        }

        //BACK 

        var learningRate = 0.05;

        var delta = Values[2][0] - Expecteds[0];

        //i = 1
        //WEIGHTS HIDDEN X OUTPUT
        var w5d = Weights[1][0][0] - Values[1][0] * delta * learningRate;                    //0.14 - 0.85 * -0.809 * 0.05        = 0.17
        var w6d = Weights[1][0][1] - Values[1][1] * delta * learningRate;                    //0.15 - 0.48 * -0.809 * 0.05        = 0.17

        //i = 0
        //WEIGHTS INPUTS X HIDDEN

        var w1d = Weights[0][0][0] - Values[0][0] * delta * learningRate * Weights[1][0][0]; //0.11 - 2.00 * -0.809 * 0.05 * 0.14 = 0.12
        var w2d = Weights[0][0][1] - Values[0][1] * delta * learningRate * Weights[1][0][0]; //0.21 - 3.00 * -0.809 * 0.05 * 0.14 = 0.23

        var w3d = Weights[0][1][0] - Values[0][0] * delta * learningRate * Weights[1][0][1]; //0.12 - 2.00 * -0.809 * 0.05 * 0.15 = 0.13
        var w4d = Weights[0][1][1] - Values[0][1] * delta * learningRate * Weights[1][0][1]; //0.08 - 3.00 * -0.809 * 0.05 * 0.15 = 0.1


        for (var i = Layers.Length-2; i >= 0; i--) {

            Console.WriteLine("LAYER " + i);
            for (var j = 0; j < Layers[i + 1]; j++) {

                Console.WriteLine(" NEURON " + j);
                for (var k = 0; k < Layers[i]; k++) {

                    Console.WriteLine($"   Weights[{i}][{j}][{k}] - Values[{i}][{k}] * delta * learningRate * Weights[{i+1}][0][{j}]");
                    Console.WriteLine(Weights[i][j][k] - Values[i][k] * delta * learningRate * Weights[i+1][0][j]);

                }

            }


        }


    }

    public static void test4() {

        var Inputs = new double[] { 2.0, 3.0 };

        var Expecteds = new double[] { 1.0 };

        var Layers = new int[] { 2, 2, 1 };

        var Values = new double[][] {
            new double[] { 2.0, 3.0 },       // L0
            new double[] { 0.0, 0.0 },       // L1
            new double[] { 0.0 },            // L2
        };

        var Weights = new double[][][] {
            new double[][]{                  // L0
                new double[] { 0.11, 0.21 }, // W1, W2
                new double[] { 0.12, 0.08 }  // W3, W4
            },
            new double[][]{                  // L1
                new double[] { 0.14, 0.15 }, // W5, W6
            },
            new[]{
                new double[] { 1 }
            }
        };

        var LAYER_COUNT = Layers.Length;
        var OUTPUT_LAYER = LAYER_COUNT - 1;

        //signal
        for (var n = 0; n < Inputs.Length; n++) {
            Values[0][n] = Inputs[n];
        }

        //FEED FORWARD
        for (var i = 1; i < LAYER_COUNT; i++) {

            //layer nöronlarının değerlerini hesapla
            for (var j = 0; j < Layers[i]; j++) {

                var sum = 0.0;

                //bir önceki layer nöronlarından ağırlık ile değer al
                for (var k = 0; k < Layers[i - 1]; k++) {
                    sum += Values[i - 1][k] * Weights[i - 1][j][k];
                }

                Values[i][j] = sum;

            }
        }

        //back propagation
        var learningRate = 0.05;

        for (var i = OUTPUT_LAYER - 1; i > 0; i--) {

            for (var j = 0; j < Layers[i - 1]; j++) {

                for (var k = 0; k < Layers[i]; k++) {

                    var v = Values[i][k];
                    var w = Weights[i][j][k];

                    var asfddsf = Weights[i][j][k] - learningRate * -0.809 * v;//        = 0.17438250000000002

                    var sdfgdfg = 1;

                }



                //OUTPUT LAYER
                //0.14 - 0.05 * -0.809 * 0.85        = 0.17438250000000002
                //0.15 - 0.05 * -0.809 * 0.48        = 0.169416

                //HIDDEN LAYER
                //0.11 - 0.05 * -0.809 * 2.00 * 0.14 = 0.121326
                //0.21 - 0.05 * -0.809 * 3.00 * 0.15 = 0.2282025

                //0.12 - 0.05 * -0.809 * 2.00 * 0.14 = 0.131326
                //0.08 - 0.05 * -0.809 * 3.00 * 0.15 = 0.0982025

            }

        }


        //sondan başa doğru tüm layerleri dolaş
        for (var i = OUTPUT_LAYER; i > 0; i--) {

            var layerNeurons = Layers[i - 1];

            //her bir layer nöronlarını dolaş
            for (var j = 0; j < Layers[i - 1]; j++) {



                //önceki layere her bir snaps
                var leftNeurons = Layers[i];
                for (var k = 0; k < Layers[i]; k++) {

                    var outputValue = Values[i][k];
                    var expectedValue = Expecteds[0];
                    var deltaValue = outputValue - expectedValue;

                    //OUTPUT LAYER
                    //0.14 - 0.05 * -0.809 * 0.85        = 0.17438250000000002
                    //0.15 - 0.05 * -0.809 * 0.48        = 0.169416

                    //HIDDEN LAYER
                    //0.11 - 0.05 * -0.809 * 2.00 * 0.14 = 0.121326
                    //0.21 - 0.05 * -0.809 * 3.00 * 0.15 = 0.2282025

                    //0.12 - 0.05 * -0.809 * 2.00 * 0.14 = 0.131326
                    //0.08 - 0.05 * -0.809 * 3.00 * 0.15 = 0.0982025

                    var weight2 = Weights[i][j][k];
                    var weight = Weights[i - 1][j][k];
                    var value = Values[i - 1][k];
                    var learned = learningRate * deltaValue;
                    var newWeight = 0.0;

                    //OUTPUT ERRORS
                    if (i == OUTPUT_LAYER) {
                        newWeight = weight - learned * value;
                    }

                    else {

                        //var sum = 0.0;
                        //for (var k2 = 0; k2 < Layers[i]; k2++) {
                        //    var w = Weights[i][j][k2];
                        //    sum += Weights[i][j][k2];
                        //}

                        //newWeight = weight - learningRate * -0.809 * value * Weights[i][j][k];
                        //var asd =    0.11 - 0.05         * -0.809 * 2.00 * 0.14;

                    }

                    Console.WriteLine($"L{i} W{j}:{weight} N{k}:{value} NW{j}:{newWeight} ");

                }

                Console.WriteLine();


                //i1
                //i2
                //w1
                //w2 
                //w3 
                //w4  
                //h1 
                //h2  
                //w5 
                //w6  
                //o1 
                //ex  

            }


        }


    }

    public static void test3() {

        var Inputs = new double[] { 2.0, 3.0 };

        var Expecteds = new double[] { 1.0 };

        var Layers = new int[] { 2, 2, 1 };

        var Values = new double[][] {
            new double[] { 2.0, 3.0 }, //LAYER 0
            new double[] { 0.0, 0.0 }, //LAYER 1
            new double[] { 0.0 },      //LAYER 2
        };

        var Weights = new double[][][] {
            new double[][]{ //LAYER 0
                new double[] { 0.11, 0.21 }, // W1, W2
                new double[] { 0.12, 0.08 }  // W3, W4
            },
            new double[][]{ //LAYER 1
                new double[] { 0.14, 0.15 }, // W5, W6
            },
            new[]{
                new double[] { 1 }
            }
        };

        //signal
        for (var n = 0; n < Inputs.Length; n++) {
            Values[0][n] = Inputs[n];
        }

        //feed forward
        for (var l = 1; l < Layers.Length; l++) {
            for (var n = 0; n < Layers[l]; n++) {
                var sum = 0.0;
                for (var s = 0; s < Layers[l - 1]; s++) {
                    sum += Values[l - 1][s] * Weights[l - 1][n][s];
                }
                Values[l][n] = sum;
            }
        }

        //back propagation
        var learningRate = 0.05;

        //output layer

        //var outputErrors = new double[Layers[Layers.Length - 1]];
        //for (var j = 0; j < Layers[Layers.Length - 1]; j++) {
        //    var error = Values[Layers.Length - 1][j] - Expecteds[j];
        //    //for (var k = 0; k < Layers[Layers.Length - 2]; k++) {
        //    //    Weights[Layers.Length - 2][j][k] -= Values[Layers.Length - 2][k] * (error * learningRate);
        //    //}
        //    outputErrors[j] = error;
        //}

        //HIDDEN LAYER
        var delta = Values[2][0] - 1.0;

        var w5d = Weights[1][0][0] - Values[1][0] * delta * learningRate;                    //0.14 - 0.85 * -0.809 * 0.05        = 0.17
        var w6d = Weights[1][0][1] - Values[1][1] * delta * learningRate;                    //0.15 - 0.48 * -0.809 * 0.05        = 0.17

        var w1d = Weights[0][0][0] - Values[0][0] * delta * learningRate * Weights[1][0][0]; //0.11 - 2.00 * -0.809 * 0.05 * 0.14 = 0.12
        var w2d = Weights[0][0][1] - Values[0][1] * delta * learningRate * Weights[1][0][0]; //0.21 - 3.00 * -0.809 * 0.05 * 0.14 = 0.23

        var w3d = Weights[0][1][0] - Values[0][0] * delta * learningRate * Weights[1][0][1]; //0.12 - 2.00 * -0.809 * 0.05 * 0.15 = 0.13
        var w4d = Weights[0][1][1] - Values[0][1] * delta * learningRate * Weights[1][0][1]; //0.08 - 3.00 * -0.809 * 0.05 * 0.15 = 0.1

        //input & hidden lalyers
        var wg = new double[Layers[Layers.Length - 1]];
        for (int k = 0; k < Layers[Layers.Length - 1]; k++) {
            wg[k] = 1.0;
        }

        //asdasddsad
        //var outputErrors = new double[Layers[Layers.Length - 1]];
        //for (var j = 0; j < Layers[Layers.Length - 1]; j++) {
        //    var error = Expecteds[j] - Values[Layers.Length - 1][j];

        //    for (var k = 0; k < Layers[Layers.Length - 2]; k++) {
        //        error += Values[Layers.Length - 2][k] * (Values[Layers.Length - 2][k] - Expecteds[j]) * learningRate;
        //    }
        //    //outputErrors[j] = error;
        //}

        //layer
        for (var i = Layers.Length - 1; i > 0; i--) {

            //layer nöronları
            for (var j = 0; j < Layers[i]; j++) {

                //her bir weight
                for (var k = 0; k < Layers[i - 1]; k++) {

                    //var asdsadas = Weights[i - 1][j][k] - Values[i - 1][k] * delta * learningRate;
                    //for (int k2 = 0; k2 < Layers[i]; k2++) {
                    //    asdsadas *= Weights[i][j][0];
                    //}

                    //var Weights = new double[][][] {
                    //    new double[][]{ //LAYER 0
                    //        new double[] { 0.11, 0.21 }, // W1, W2
                    //        new double[] { 0.12, 0.08 }  // W3, W4
                    //    },
                    //    new double[][]{ //LAYER 1
                    //        new double[] { 0.14, 0.15 }, // W5, W6
                    //    },
                    //    new[]{
                    //        new double[] { 1 }
                    //    }
                    //};

                    //0.11 - 2.00 * -0.809 * 0.05 * 0.14 = 0.12

                    for (var k2 = 0; k2 < 4; k2++) {
                        var asdsadas = Weights[i - 1][j][k] - Values[i - 1][k] * delta * learningRate * Weights[i][k2][j];
                    }
                    //var dfgdfg = 0;
                    //Console.WriteLine(asdsadas);
                }

                wg = Weights[i - 1][j];

            }
        }

    }

    public static void test2() {

        var Inputs = new double[] { 2.0, 3.0 };

        var Values = new double[][] {
            new double[] { 2.0, 3.0 }, // I1, I2
            new double[] { 0.0, 0.0 }, // H1, H2
            new double[] { 0.0 },      // OUT
        };

        var Weights = new double[][][] {
            new double[][]{
                new double[] { 0.11, 0.21 }, // W1, W2
                new double[] { 0.12, 0.08 }  // W3, W4
            },                                  
            new double[][]{                     
                new double[] { 0.14, 0.15 }, // W5, W6
            },
        };

        //feed forward

        var asd0 = Values[0][0] = Inputs[0];
        var asd1 = Values[0][1] = Inputs[1];

        var asd2 = Values[1][0] = (Values[0][0] * Weights[0][0][0]) + (Values[0][1] * Weights[0][0][1]);
        var asd3 = Values[1][1] = (Values[0][0] * Weights[0][1][0]) + (Values[0][1] * Weights[0][1][1]);
        var asd4 = Values[2][0] = (Values[1][0] * Weights[1][0][0]) + (Values[1][1] * Weights[1][0][1]);

        //back propagation
        var learningRate = 0.05;
        var delta = Values[2][0] - 1.0;

        var w5d = Weights[1][0][0] - ((delta * learningRate) * Values[1][0]);                    //0.14 - ((0.05 * -0.809) * 0.85)        = 0.17
        var w6d = Weights[1][0][1] - ((delta * learningRate) * Values[1][1]);                    //0.15 - ((0.05 * -0.809) * 0.48)        = 0.17
        var w1d = Weights[0][0][0] - ((delta * learningRate) * Values[0][0]) * Weights[1][0][0]; //0.11 - ((0.05 * -0.809) * 2   ) * 0.14 = 0.12
        var w2d = Weights[0][0][1] - ((delta * learningRate) * Values[0][1]) * Weights[1][0][0]; //0.21 - ((0.05 * -0.809) * 3   ) * 0.14 = 0.23
        var w3d = Weights[0][1][0] - ((delta * learningRate) * Values[0][0]) * Weights[1][0][1]; //0.12 - ((0.05 * -0.809) * 2   ) * 0.15 = 0.13
        var w4d = Weights[0][1][1] - ((delta * learningRate) * Values[0][1]) * Weights[1][0][1]; //0.08 - ((0.05 * -0.809) * 3   ) * 0.15 = 0.1

    }

}

//public class NetworkNew {

//    private int[] layers;
//    private double[][] neurons;
//    private double[][] biases;
//    private double[][][] weights;

//    public NetworkNew(params int[] layers) {

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