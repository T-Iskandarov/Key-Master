using System.IO;
using System.Media;

namespace QisqaTugma.Services;

public class AudioService : IDisposable
{
    private SoundPlayer? _player;
    private bool _disposed;
    private readonly string _soundPath;

    public AudioService()
    {
        var appDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "QisqaTugma");
        _soundPath = Path.Combine(appDir, "keyclick.wav");
        GenerateClickSound();
    }

    private void GenerateClickSound()
    {
        if (File.Exists(_soundPath)) return;

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_soundPath)!);

            // Generate a simple mechanical key click WAV
            int sampleRate = 44100;
            int durationMs = 30;
            int numSamples = sampleRate * durationMs / 1000;
            var samples = new short[numSamples];
            var random = new Random(42);

            for (int i = 0; i < numSamples; i++)
            {
                double t = (double)i / sampleRate;
                // Sharp attack with fast decay envelope
                double envelope = Math.Exp(-t * 200);
                // Mix of noise + low frequency click
                double noise = (random.NextDouble() * 2 - 1) * 0.3;
                double click = Math.Sin(2 * Math.PI * 800 * t) * 0.7;
                double sample = (noise + click) * envelope;
                samples[i] = (short)(sample * 20000);
            }

            using var fs = new FileStream(_soundPath, FileMode.Create);
            using var writer = new BinaryWriter(fs);

            // WAV header
            int dataSize = numSamples * 2;
            writer.Write("RIFF"u8);
            writer.Write(36 + dataSize);
            writer.Write("WAVE"u8);
            writer.Write("fmt "u8);
            writer.Write(16);           // chunk size
            writer.Write((short)1);     // PCM
            writer.Write((short)1);     // mono
            writer.Write(sampleRate);
            writer.Write(sampleRate * 2); // byte rate
            writer.Write((short)2);     // block align
            writer.Write((short)16);    // bits per sample
            writer.Write("data"u8);
            writer.Write(dataSize);

            foreach (var s in samples)
                writer.Write(s);

            _player = new SoundPlayer(_soundPath);
            _player.Load();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Audio init error: {ex.Message}");
        }
    }

    public void PlayKeySound()
    {
        try
        {
            _player?.Play();
        }
        catch
        {
            // Ignore playback errors
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _player?.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }

    ~AudioService() => Dispose();
}
