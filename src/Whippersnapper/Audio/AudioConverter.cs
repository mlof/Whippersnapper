using Concentus.Oggfile;
using Concentus.Structs;
using NAudio.Wave;

namespace Whippersnapper.Audio;

public static class AudioConverter
{
    public static async Task ConvertToWav(string filePath, string wavFilePath, CancellationToken cancellationToken)
    {
        Console.WriteLine("Converting Ogg to WAV");
        await using var fileIn = new FileStream(filePath, FileMode.Open);
        await using var pcmStream = new MemoryStream();
        var tempFileName = wavFilePath + ".tmp";
        var decoder = OpusDecoder.Create(48000, 1);
        var oggIn = new OpusOggReadStream(decoder, fileIn);
        while (oggIn.HasNextPacket)
        {
            var packet = oggIn.DecodeNextPacket();
            if (packet == null)
            {
                continue;
            }

            foreach (var t in packet)
            {
                var bytes = BitConverter.GetBytes(t);
                await pcmStream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
            }
        }

        pcmStream.Position = 0;
        await using var wavStream = new RawSourceWaveStream(pcmStream, new WaveFormat(48000, 1));
        var sampleProvider = wavStream.ToSampleProvider();
        WaveFileWriter.CreateWaveFile16(tempFileName, sampleProvider);


        // convert to target sample rate

        await ConvertWavToSampleRate(tempFileName, wavFilePath);

        File.Delete(wavFilePath + ".tmp");
    }

    private static async Task ConvertWavToSampleRate(string tempFileName, string wavFilePath)
    {
        await using var wavFileReader = new WaveFileReader(tempFileName);
        // 16Khz
        var targetSampleRate = 16000;

        using var resampler = new MediaFoundationResampler(wavFileReader, new WaveFormat(targetSampleRate, 1));

        WaveFileWriter.CreateWaveFile16(wavFilePath, resampler.ToSampleProvider());
    }
}