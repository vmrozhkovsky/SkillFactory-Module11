namespace VoiceTexterBot.Configurations;

public interface IAppSettings
{
    string BotToken { get; set; }
    string DownloadsFolder { get; set; }
    string AudioFileName { get; set; }
    string InputAudioFormat { get; set; }
}