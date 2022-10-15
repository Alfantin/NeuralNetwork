using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public static class WordGen {

    public static void test() {

        generate(
            "KARA BÜYÜCÜ SABRİ",
            "zebure kakasa nugaste azdana mananga kinamu kazanda diganna limanasta yirana karka namana lanto naroni santo taybo konto kinazo enter amano"
        );

        generate(
            "VELHELEBE",
            "vulkemul alul hasene vele kut ahaaa vele zugamba ya huveleee uc gase ve beal velekem heleve ya el heleve velekem bahalele ve elle vehelele circir velekaaaeeee velhelebe haleke velleheh mahalele ve makalele"
        );

        generate(
            "BERBER",
            "bir berber bir berbere bre berber gel beraber bir berber dükkanı açalım demiş"
        );

    }

    public static void generate(string title, string sample) {

        Console.WriteLine(title);

        var sampleText = sample;
        sampleText = Regex.Replace(sampleText, @"\s+", " ");
        sampleText = sampleText.ToLower();
        var usedWords = Util.CreateIndexTable(sampleText.ToArray());

        var input = new List<double[]>();
        var expected = new List<double[]>();
        var size = 3;
        for (var i = 0; i < sampleText.Length - size; i++) {

            var inp = new double[size];
            for (var j = 0; j < size; j++) {
                inp[j] = Util.NormalizeInput(usedWords, sampleText[i + j]);
            }
            input.Add(inp);

            var expectedOutput = Util.ExpectedOutput(usedWords, sampleText[i + size]);
            expected.Add(expectedOutput);

        };
        
        var nn = new Network(size, 9, usedWords.Count);
        nn.train(0.1, 5000, input.ToArray(), expected.ToArray());

        var outputs = "";
        for (var i = 0; i < size; i++) {
            outputs += usedWords[Util.Random(usedWords.Count)];
        }

        for (var i = 0; i < 200; i++) {

            var inp = new double[size];
            for (var j = 0; j < size; j++) {
                inp[j] = Util.NormalizeInput(usedWords, outputs[outputs.Length - (size - j)]);
            }

            var result = nn.think(inp);
            var resultIndex = Util.RandomWeightedChoice(result);
            outputs += usedWords[resultIndex];

        }

        Console.WriteLine(outputs.Substring(size));
        Console.WriteLine("-------------------------------------------------");


    }


}