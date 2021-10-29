using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ZonyLrcTools.Ctrls.Downloader.Lyric;
using ZonyLrcTools.Ctrls.Downloader.Lyric.Interfaces;
using ZonyLrcTools.Ctrls.MusicTag;
using ZonyLrcTools.Models;
using ZonyLrcTools.Models.MusicTag;

namespace ZonyLrcTools
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (CommandLineOptions.Instance.Help)
            {
                return;
            }

            FileStream fileStream = null;

            LyricDownloaderContainer downloaders = new LyricDownloaderContainer();
            ILyricDownloader downloader = downloaders.GetDownloader();

            if (!CommandLineOptions.Instance.IsEmpty)
            {
                if (string.IsNullOrEmpty(CommandLineOptions.Instance.Title))
                {
                    Console.WriteLine($"Empty song title.");
                    return;
                }
                if (string.IsNullOrEmpty(CommandLineOptions.Instance.Artist))
                {
                    CommandLineOptions.Instance.Artist = CommandLineOptions.Instance.Title;
                }

                try
                {
                    MusicInfo musicInfo = new MusicInfo(CommandLineOptions.Instance.Title, CommandLineOptions.Instance.Artist);
                    Task<LyricItemCollection> result = downloader.DownloadAsync(musicInfo);
                    byte[] lrc = Encoding.UTF8.GetBytes(result.Result.ToString());

                    if (lrc.Length > 0)
                    {
                        fileStream = File.OpenWrite(CommandLineOptions.Instance.Output ?? $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}{CommandLineOptions.Instance.Artist} - {CommandLineOptions.Instance.Title}.lrc");
                        if (fileStream != null)
                        {
                            fileStream.Write(lrc, default, lrc.Length);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Lyric empty.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    fileStream?.Dispose();
                }
            }
            else
            {
                MusicInfoLoaderByTagLib musicInfoLoader = new MusicInfoLoaderByTagLib();

                for (int i = default; i < args.Length; i++)
                {
                    FileInfo fileInfo = new FileInfo(args[i]);

                    try
                    {
                        if (fileInfo.Exists)
                        {
                            MusicInfo musicInfo = musicInfoLoader.Load(args[i]);

                            Console.WriteLine($"[{i}] File '{args[i]}' tag is title={musicInfo?.Name}, artist={musicInfo?.Artist}.");
                            Task<LyricItemCollection> result = downloader.DownloadAsync(musicInfo);
                            byte[] lrc = Encoding.UTF8.GetBytes(result.Result.ToString());

                            if (lrc.Length > 0)
                            {
                                fileStream = File.OpenWrite($"{fileInfo.DirectoryName}{Path.DirectorySeparatorChar}{fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length)}.lrc");
                                if (fileStream != null)
                                {
                                    fileStream.Write(lrc, default, lrc.Length);
                                }
                            }
                            else
                            {
                                Console.WriteLine($"[{i}] Lyric empty filename of '{args[i]}'.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"[{i}] Unknown filename of '{args[i]}'.");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    finally
                    {
                        fileStream?.Dispose();
                    }
                }
            }
        }
    }
}
