using System.IO.MemoryMappedFiles;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

List<string> list = ReadFile();

Console.WriteLine("10 Наиболее часто встречающихся слов : \n");
MostCommonWords(list).ToList().ForEach(Console.WriteLine);

static List<string> ReadFile()
{
    List<string> list = new List<string>();
    using (var mappedFile1 = MemoryMappedFile.CreateFromFile("Text1.txt"))
    {
        using (Stream mmStream = mappedFile1.CreateViewStream())
        {
            using (StreamReader sr = new StreamReader(mmStream, Encoding.UTF8))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();

                    var noPunctuationText = new string(line.Where(c => !char.IsPunctuation(c)).ToArray());
                    var lineWords = noPunctuationText.Split(' ').ToList<string>();
                    lineWords.ForEach(list.Add);
                }
            }
        }
    }
    return list;
}

static IEnumerable<string> MostCommonWords(List<string> list)
{
    Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
    foreach (string word in list)
    {
        if (!keyValuePairs.ContainsKey(word))
        {
            keyValuePairs.Add(word, 1);
        }
        else
        {
            keyValuePairs[word]++;
        }
    }
    keyValuePairs = keyValuePairs.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
    var keyValuePairsReverse = keyValuePairs.Reverse();
    var words = keyValuePairsReverse.Take(11).Select(x => x.Key);
    return words;
}