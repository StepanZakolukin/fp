using TagCloudGUI.Controls;

namespace TagCloud.TextProcessing;

public interface IWordsProvider
{
    public IEnumerable<WordInfo> PerformPreprocessing(Func<IEnumerable<string>> getTextLineByLine,
        ContentStructure structureOfContent);
}