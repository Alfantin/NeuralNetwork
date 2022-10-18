using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public static class TextGen {

    private static Random rnd = new Random();

    public static void test0() {

        var sampleText = File.ReadAllText("../../data/data.txt");
        sampleText = Regex.Replace(sampleText, @"\s+", " ");
        sampleText = sampleText.ToLower();

        var usedCharacters = "";
        foreach (var c in sampleText) {
            if (!usedCharacters.Contains(c)) usedCharacters += c;
        }

        var inputs = new List<double[]>();
        var expecteds = new List<double[]>();
        for (var i = 0; i < sampleText.Length - 1; i++) {

            var inp = new double[usedCharacters.Length];
            inp[usedCharacters.IndexOf(sampleText[i])] = 1f;
            inputs.Add(inp);

            var opt = new double[usedCharacters.Length];
            opt[usedCharacters.IndexOf(sampleText[i + 1])] = 1f;
            expecteds.Add(opt);

        };

        var nn = new Network(usedCharacters.Length, usedCharacters.Length / 4, usedCharacters.Length);
        nn.train(1, 100, inputs.ToArray(), expecteds.ToArray());

        var w = "f";
        for (var i = 0; i < 1000; i++) {

            var testInput = new double[usedCharacters.Length];
            testInput[usedCharacters.IndexOf(sampleText[i])] = 1f;

            var result = nn.think(testInput);
            var sorted = result.OrderByDescending(k => k).ToArray();

            var index = rnd.Next(0, 3);
            if (index < 0) index = 0;
            else if (index >= sorted.Length) index = sorted.Length - 1;
            var randomChoise = sorted[index];

            var resultIndex = Array.IndexOf(result, randomChoise);
            var resultChar = usedCharacters[resultIndex];

            w += resultChar;

            //w += usedCharacters[weightedRandomChooice(nn.think(testInput))];

        }

        Console.Write(w);

    }

    public static void test1() {

        var sampleText = @"
                Ey Türk gençliği! Birinci vazifen; Türk istiklalini, Türk cumhuriyetini, ilelebet muhafaza ve müdafaa etmektir.
                Mevcudiyetinin ve istikbalinin yegâne temeli budur. Bu temel, senin en kıymetli hazinendir. İstikbalde dahi seni bu hazineden mahrum etmek isteyecek dâhilî ve haricî bedhahların olacaktır. Bir gün, istiklal ve cumhuriyeti müdafaa mecburiyetine düşersen, vazifeye atılmak için içinde bulunacağın vaziyetin imkân ve şeraitini düşünmeyeceksin. Bu imkân ve şerait, çok namüsait bir mahiyette tezahür edebilir. İstiklal ve cumhuriyetine kastedecek düşmanlar, bütün dünyada emsali görülmemiş bir galibiyetin mümessili olabilirler. Cebren ve hile ile aziz vatanın bütün kaleleri zapt edilmiş, bütün tersanelerine girilmiş, bütün orduları dağıtılmış ve memleketin her köşesi bilfiil işgal edilmiş olabilir. Bütün bu şeraitten daha elim ve daha vahim olmak üzere, memleketin dâhilinde iktidara sahip olanlar, gaflet ve dalalet ve hatta hıyanet içinde bulunabilirler. Hatta bu iktidar sahipleri, şahsi menfaatlerini müstevlilerin siyasi emelleriyle tevhit edebilirler. Millet, fakruzaruret içinde harap ve bitap düşmüş olabilir.
                Ey Türk istikbalinin evladı! İşte, bu ahval ve şerait içinde dahi vazifen, Türk istiklal ve cumhuriyetini kurtarmaktır. Muhtaç olduğun kudret, damarlarındaki asil kanda mevcuttur.
            ";

        //var sampleText = File.ReadAllText("data.txt");
        sampleText = Regex.Replace(sampleText, @"\s+", " ");
        sampleText = sampleText.ToLower();

        var usedCharacters = "";
        foreach (var c in sampleText) {
            if (!usedCharacters.Contains(c)) usedCharacters += c;
        }

        var inputs = new List<double[]>();
        var expecteds = new List<double[]>();
        for (var i = 0; i < sampleText.Length - 5; i++) {

            inputs.Add(new double[] {
                (double)usedCharacters.IndexOf(sampleText[i]) / usedCharacters.Length,
                (double)usedCharacters.IndexOf(sampleText[i + 1]) / usedCharacters.Length,
            });

            var opt = new double[usedCharacters.Length];
            opt[usedCharacters.IndexOf(sampleText[i + 2])] = 1f;
            expecteds.Add(opt);

        };

        var nn = new Network(2, 8, usedCharacters.Length);
        nn.train(1f, 500, inputs.ToArray(), expecteds.ToArray());

        var w = "asd";
        for (var i = 0; i < 1000; i++) {

            var result = nn.think(
                normalizeChar(usedCharacters, w[w.Length - 2]),
                normalizeChar(usedCharacters, w[w.Length - 1])
            );

            var sorted = result.OrderByDescending(k => k).ToArray();

            var index = rnd.Next(0, 2);
            if (index < 0) index = 0;
            else if (index >= sorted.Length) index = sorted.Length - 1;
            var randomChoise = sorted[index];

            var resultIndex = Array.IndexOf(result, randomChoise);
            var resultChar = usedCharacters[resultIndex];

            //var resultIndex =  Array.IndexOf(result, result.Max());
            //var resultChar = usedCharacters[resultIndex];

            //var resultValue = weightedRandomChooice(result);
            //var resultChar = usedCharacters[resultValue];

            w += resultChar;

        }

        Console.Write(w);

    }

    public static void test2() {

        //var sampleText = @"
        //    Ey Türk gençliği! Birinci vazifen; Türk istiklalini, Türk cumhuriyetini, ilelebet muhafaza ve müdafaa etmektir.
        //    Mevcudiyetinin ve istikbalinin yegâne temeli budur. Bu temel, senin en kıymetli hazinendir. İstikbalde dahi seni bu hazineden mahrum etmek isteyecek dâhilî ve haricî bedhahların olacaktır. Bir gün, istiklal ve cumhuriyeti müdafaa mecburiyetine düşersen, vazifeye atılmak için içinde bulunacağın vaziyetin imkân ve şeraitini düşünmeyeceksin. Bu imkân ve şerait, çok namüsait bir mahiyette tezahür edebilir. İstiklal ve cumhuriyetine kastedecek düşmanlar, bütün dünyada emsali görülmemiş bir galibiyetin mümessili olabilirler. Cebren ve hile ile aziz vatanın bütün kaleleri zapt edilmiş, bütün tersanelerine girilmiş, bütün orduları dağıtılmış ve memleketin her köşesi bilfiil işgal edilmiş olabilir. Bütün bu şeraitten daha elim ve daha vahim olmak üzere, memleketin dâhilinde iktidara sahip olanlar, gaflet ve dalalet ve hatta hıyanet içinde bulunabilirler. Hatta bu iktidar sahipleri, şahsi menfaatlerini müstevlilerin siyasi emelleriyle tevhit edebilirler. Millet, fakruzaruret içinde harap ve bitap düşmüş olabilir.
        //    Ey Türk istikbalinin evladı! İşte, bu ahval ve şerait içinde dahi vazifen, Türk istiklal ve cumhuriyetini kurtarmaktır. Muhtaç olduğun kudret, damarlarındaki asil kanda mevcuttur.
        //";
        var sampleText = File.ReadAllText("data.txt");

        sampleText = Regex.Replace(sampleText, @"\s+", " ");
        sampleText = sampleText.ToLower();

        var usedCharacters = "";
        foreach (var c in sampleText) {
            if (!usedCharacters.Contains(c)) usedCharacters += c;
        }

        var sampleTextBytes = System.Text.Encoding.ASCII.GetBytes(sampleText);
        var input = new List<double[]>();
        var expected = new List<double[]>();

        for (var i = 0; i < sampleTextBytes.Length - 3; i++) {

            var c1 = normalizeChar(usedCharacters, sampleText[i]);
            var c2 = normalizeChar(usedCharacters, sampleText[i + 1]);
            var c3 = normalizeChar(usedCharacters, sampleText[i + 2]);
            input.Add(new double[] { c1, c2, c3 });

            var exp = new double[usedCharacters.Length];
            var ci = usedCharacters.IndexOf(sampleText[i + 3]);
            exp[ci] = 1f;
            expected.Add(exp);

        };

        var nn = new Network(3, usedCharacters.Length / 4, usedCharacters.Length);

        nn.train(
            1f,
            100,
            input.ToArray(),
            expected.ToArray()
        );

        var w = "türk";
        for (var i = 0; i < 1000; i++) {

            var c1 = normalizeChar(usedCharacters, w[w.Length - 3]);
            var c2 = normalizeChar(usedCharacters, w[w.Length - 2]);
            var c3 = normalizeChar(usedCharacters, w[w.Length - 1]);

            var result = nn.think(c1, c2, c3);
            var resultIndex = Array.IndexOf(result, result.Max());

            w += usedCharacters[resultIndex];

        }

        Console.Write(w);

    }

    public static void test() {


        //var text = @"
        //    Ey Türk gençliği! Birinci vazifen; Türk istiklalini, Türk cumhuriyetini, ilelebet muhafaza ve müdafaa etmektir.
        //    Mevcudiyetinin ve istikbalinin yegâne temeli budur. Bu temel, senin en kıymetli hazinendir. İstikbalde dahi seni bu hazineden mahrum etmek isteyecek dâhilî ve haricî bedhahların olacaktır. Bir gün, istiklal ve cumhuriyeti müdafaa mecburiyetine düşersen, vazifeye atılmak için içinde bulunacağın vaziyetin imkân ve şeraitini düşünmeyeceksin. Bu imkân ve şerait, çok namüsait bir mahiyette tezahür edebilir. İstiklal ve cumhuriyetine kastedecek düşmanlar, bütün dünyada emsali görülmemiş bir galibiyetin mümessili olabilirler. Cebren ve hile ile aziz vatanın bütün kaleleri zapt edilmiş, bütün tersanelerine girilmiş, bütün orduları dağıtılmış ve memleketin her köşesi bilfiil işgal edilmiş olabilir. Bütün bu şeraitten daha elim ve daha vahim olmak üzere, memleketin dâhilinde iktidara sahip olanlar, gaflet ve dalalet ve hatta hıyanet içinde bulunabilirler. Hatta bu iktidar sahipleri, şahsi menfaatlerini müstevlilerin siyasi emelleriyle tevhit edebilirler. Millet, fakruzaruret içinde harap ve bitap düşmüş olabilir.
        //    Ey Türk istikbalinin evladı! İşte, bu ahval ve şerait içinde dahi vazifen, Türk istiklal ve cumhuriyetini kurtarmaktır. Muhtaç olduğun kudret, damarlarındaki asil kanda mevcuttur.
        //";

        //var text = File.ReadAllText("../../data/bakara.txt");
        //var text = File.ReadAllText("../../data/SecmeAyet.txt");
        //var text = File.ReadAllText("../../data/Ayetelkürsi.txt");
        //var text = File.ReadAllText("../../data/NasFelak.txt");
        //var text = File.ReadAllText("../../data/Donat.txt");

        generate(
            "AYETELKÜRSİ",
            "Allah, O Allah’tır. O, yegâne hak mâbuddur ki O’ndan başka İlâh yok, yalnız O; daima yaşayan, duran, tutan, her an bütün hilkat üzerinde hâkim, Hayy ü Kayyum ancak O’dur. Ne gaflet basar O’nu, ne uyku. Göklerde, yerde ne varsa hepsi O’nundur. Kimin haddine ki izni olmaksızın O’nun yanında şefaat edebilsin? Allah, yarattıklarının işlediklerini, işleyenlerini, geçmişlerini, geleceklerini bilir. Onlar ise O’nun bildiklerinden yalnız dilediği kadarını kavrayabilir; başka bir şey bilemezler. O’nun kürsüsü, ilmi bütün gökleri ve yeri kucaklamıştır ve bunların koruyuculuğu, bunları görüp gözetmek kendisine bir ağırlık da vermez. O, öyle ulu, öyle büyük ve yücedir."
        );

        generate(
            "FELAK",
            "Yaratılmışların şerrinden, karanlık çöktüğü zaman gecenin şerrinden, düğümlere üfleyenlerin şerrinden ve haset edenin, içindeki hasedini dışarıya vurduğu vakit, şerrinden; şafak aydınlığının Rabbine Allah’a sığınırım."
        );

        generate(
            "DONAT",
            "tamamen allah'ın hükmüyle, allah'ın kelamıyla, allah'ın rahmetiylen birlikte donat donat donat; etrafa, sağdan sola, üstten alta, önden arkaya doğru donat ve bin dört yüzüylen birlikte, hümana ve hümanaylan birlikte sağdan sola donat ve seyir halini gerçekleştir. kulyas'a karşı, zuzula'ya karşı, demon'a karşı, afarit'e karşı etraflarında olanlara, göğsü sıkışanlara, içinde seyir halini hissedenlere ve yaşayanlara ve yaşatanlara doğru, görevli olanlara doğru ayna görevi ol ve kendilerine doğru gönder ve göster."
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
        
        var sampleText = sample
            .ToLower()
            .Replace(".", " ")
            .Replace(",", " ")
            .Replace("?", " ")
            .Replace("!", " ")
            .Replace(";", " ")
            .Replace("'", " ")
            .Split(new char[] { ' ', '\'', '\r', '\n' },
            StringSplitOptions.RemoveEmptyEntries
        );

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

        var nn = new Network(size, size * 2, usedWords.Count);
        nn.train(0.02, 5000, input.ToArray(), expected.ToArray() );

        var outputs = new List<string>();
        for (var i = 0; i < size; i++) {
            outputs.Add(usedWords[Util.Random(usedWords.Count)]);
        }

        for (var i = 0; i < 30; i++) {

            var inp = new double[size];
            for (var j = 0; j < size; j++) {
                inp[j] = Util.NormalizeInput(usedWords, outputs[outputs.Count - (size - j)]);
            }

            var result = nn.think(inp);
            var resultIndex = Util.RandomWeightedChoiceSorted(result);
            var resultValue = usedWords[resultIndex];
            outputs.Add(resultValue);

        }

        Console.WriteLine(string.Join(" ", outputs.Skip(size).ToArray()));
        Console.WriteLine("-------------------------------------------------");
    }

    private static int GetRandomWeightedIndex(double[] weights) {
        // Get the total sum of all the weights.
        var weightSum = 0.0;
        for (int i = 0; i < weights.Length; ++i) {
            weightSum += weights[i];
        }

        // Step through all the possibilities, one by one, checking to see if each one is selected.
        int index = 0;
        int lastIndex = weights.Length - 1;
        while (index < lastIndex) {
            // Do a probability check with a likelihood of weights[index] / weightSum.
            if (rnd.NextDouble() * weightSum < weights[index]) {
                return index;
            }

            // Remove the last item from the sum of total untested weights and try again.
            weightSum -= weights[index++];
        }

        // No other item was selected, so return very last index.
        return index;
    }

    private static int weightedRandomChooice(double[] p) {
        var rand = rnd.NextDouble();
        var sum = 0.0;
        for (var i = 0; i < p.Length; i++) {
            var nsum = sum + p[i];
            if (rand >= sum && rand <= nsum) {
                return i;
            }
            sum = nsum;
        }
        return p.Length - 1;
    }

    private static double normalizeWord(List<string> words, string c) {
        return (double)words.IndexOf(c) / words.Count;
    }

    private static double normalizeChar(string usedCharacters, char c) {
        return (double)usedCharacters.IndexOf(c) / usedCharacters.Length;
    }

    private static char denormalizeChar(string usedCharacters, double v) {
        return usedCharacters[(int)Math.Floor(v * usedCharacters.Length)];
    }

}