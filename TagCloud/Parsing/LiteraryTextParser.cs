using System.Diagnostics;
using System.Text;
using Castle.Core.Internal;
using ErrorHandling;

namespace TagCloud.Parsing;

public class LiteraryTextParser : IParser
{
    public string TypeOfParsing => "Литературный текст";
    private readonly ProcessStartInfo _startInfo = new()
    {
        CreateNoWindow = true,
        UseShellExecute = false,
        RedirectStandardError = true,
        RedirectStandardInput = false,
        RedirectStandardOutput = true,
        FileName = "Parsing/Mystem.exe",
    };
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
    private readonly Dictionary<Tuple<string, string>, int> _countingDictionary = new();

    private ActionStatus ParseText(Func<IEnumerable<string>> getTextLineByLine)
    {
        var tempFile = Path.ChangeExtension(Path.GetTempFileName(), ".txt");
        WriteLinesToFile(getTextLineByLine(), tempFile);
        _startInfo.Arguments = $"-ling {tempFile}";
        
        using var process = Process.Start(_startInfo);
        using var reader = new StreamReader(process.StandardOutput.BaseStream, Encoding.UTF8);
        var stopwatch = Stopwatch.StartNew();
        while (reader.ReadLine() is { } line && stopwatch.ElapsedMilliseconds < 5000)
        {
            if (TryExtractWordAndPartOfSpeech(line, out var wordAndPartOfSpeech))
                AddInfoAboutWord(wordAndPartOfSpeech);
        }
        stopwatch.Stop();
        var errors = process.StandardError.ReadToEnd();

        process.Kill();
        process.WaitForExit();
        File.Delete(tempFile);
        
        if (stopwatch.ElapsedMilliseconds >= 5000) 
            return ActionStatus.Fail("обработка текста длится слишком долго");
        return errors.IsNullOrEmpty() ? ActionStatus.Ok() : ActionStatus.Fail(errors);
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
        var parsingStatus = ParseText(getTextLineByLine);
        if (!parsingStatus.IsSuccess)
            return Result.Fail<WordInfo[]>("Литературная обработка текста не доступна. " +
                                           $"Ошибка работы стороннего приложения Mystem: {parsingStatus.Error}");

        foreach (var pair in _countingDictionary)
        {
            var res = WordInfo.Create(pair.Key.Item1,_decryptionGrammems[pair.Key.Item2], pair.Value);
            if (!res.IsSuccess) 
                return Result.Fail<WordInfo[]>($"Встретилась {res.Error}");
            result.Add(res.GetValueOrThrow());
        }

        return result.Count == 0 ? Result.Fail<WordInfo[]>("Файл оказался пустым") : Result.Ok(result.ToArray());
    }

    private bool TryExtractWordAndPartOfSpeech(string wordAnalysis, out Tuple<string, string>? result)
    {
        result = null;
        var info = wordAnalysis.Split('=');
        
        if (info.Length < 2 || info[0].Contains('?')) return false;
        result = Tuple.Create(info[0], info[1].Split(',')[0]);
        return true;
    }

    private void AddInfoAboutWord(Tuple<string, string> wordAndPartOfSpeech)
    {
        if (!_countingDictionary.TryAdd(wordAndPartOfSpeech, 1))
            _countingDictionary[wordAndPartOfSpeech]++;
    }
}