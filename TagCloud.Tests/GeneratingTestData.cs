using System.Collections.Immutable;

namespace TagCloud.Tests;

public class GeneratingTestData
{
    public static readonly ImmutableDictionary<string, int> FrequencyDictionary = ImmutableDictionary
        .CreateRange(new KeyValuePair<string, int>[] 
            {
                new("привет", 5),
                new("морозный", 7),
                new("быстрый", 3),
                new("я", 20),
                new("человек", 2),
                new("отчаянно", 8)
            });

    public TItems[] Shuffle<TItems>(TItems[] source)
    {
        var lines = source.ToArray();
        new Random().Shuffle(lines);
        
        return lines;
    }
    
    public string[] CreateArrayOfWords(IDictionary<string, int> frequencyDictionary)
    {
        var list = new List<string>();
        foreach (var pair in frequencyDictionary)
            for (var i = 0; i < pair.Value; i++)
                list.Add(pair.Key);
        
        return list.ToArray();
    }
}