﻿namespace UtilityBot.Configurations;

public class AppSettings
{
    public string BotToken { get; set; }
    /// <summary>
    /// Папка загрузки аудио файлов
    /// </summary>
    public string DownloadsFolder { get; set; }
    /// <summary>
    /// Имя файла при загрузке
    /// </summary>
    public string AudioFileName { get; set; }
    /// <summary>
    /// Формат аудио при загрузке
    /// </summary>
    public string InputAudioFormat { get; set; }
    public string OutputAudioFormat { get; set; }
    public string FFMpegFolder { get; set; }
}