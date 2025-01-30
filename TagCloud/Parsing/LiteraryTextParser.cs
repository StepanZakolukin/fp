using System.Diagnostics;
using System.Text;
using ErrorHandling;

namespace TagCloud.Parsing;

public class LiteraryTextParser : IParser
{
    public string TypeOfParsing => "Литературный текст";
    private readonly Dictionary<string, string> _decryptionGrammems = new()
    {
        { "A", "прилагательное" },
        { "ADV", "наречие" },
        { "ADVPRO", "местоименное наречие" },
        { "ANUM", "числительное-прилагательное" },
        { "APRO", "местоимение-прилагательное" },
        { "COM", "часть композита - сложного слова" },
        { "CONJ", "союз" },
        { "INTJ", "междометие" },
        { "NUM", "числительное" },
        { "PART", "частица" },
        { "PR", "предлог" },
        { "S", "существительное" },
        { "SPRO", "местоимение-существительное" },
        { "V", "глагол" }
    };

    private IEnumerable<string> ParseText(Func<IEnumerable<string>> getTextLineByLine)
    {
        var tempFile = Path.ChangeExtension(Path.GetTempFileName(), ".txt");
        WriteLinesToFile(getTextLineByLine(), tempFile);

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardInput = false,
                RedirectStandardOutput = true,
                FileName = "Parsing/Mystem.exe",
                Arguments = $"-ling {tempFile}",
            }
        };
        process.Start();
        
        using (var reader = new StreamReader(process.StandardOutput.BaseStream, Encoding.UTF8))
        {
            while (reader.ReadLine() is { } line)
                yield return line;
        }
        process.WaitForExit();
        File.Delete(tempFile);
    }

    private void WriteLinesToFile(IEnumerable<string> lines, string fileName)
    {
        using var writer = new StreamWriter(fileName);
        foreach (var line in lines)
            writer.WriteLine(line);
    }
    
    public Result<WordInfo[]> Parse(Func<IEnumerable<string>> getTextLineByLine)
    {
        var result = new List<WordInfo>();
        var textInfo = ParseText(getTextLineByLine);
        var countingDictionary = new Dictionary<Tuple<string, string>, int>();

        foreach (var line in textInfo)
        {
            var info = line.Split('=');
            if (info.Length < 2 || info[0].Contains('?')) continue;
            var wordAndPartOfSpeech = Tuple.Create(info[0], info[1].Split(',')[0]);
            if (!countingDictionary.TryAdd(wordAndPartOfSpeech, 1))
                countingDictionary[wordAndPartOfSpeech]++;
        }

        foreach (var pair in countingDictionary)
        {
            var res = WordInfo.Create(
                pair.Key.Item1,
                _decryptionGrammems[pair.Key.Item2],
                pair.Value);
            if (!res.IsSuccess)
                return Result.Fail<WordInfo[]>($"Встретилась {res.Error}");
            result.Add(res.GetValueOrThrow());
        }

        return result.Count == 0 ? Result.Fail<WordInfo[]>("Файл оказался пустым") : Result.Ok(result.ToArray());
    }
}