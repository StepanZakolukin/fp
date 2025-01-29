using System.Drawing;

namespace TagCloud.ImageGeneration;

public class VisualizationSettings(VisualizationSettingsDto visualizationSettings)
    : ISettingsProvider<VisualizationSettingsDto>
{
    public VisualizationSettingsDto Settings { get; } = visualizationSettings;
}