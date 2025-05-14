using UnityEngine;
using UnityEngine.Audio;

public class RecordSoundCenter : MonoBehaviour
{
    //사운드 관련 로직 구현하는 곳
    //Dinoinfo UI Update에서 동작한다.

    public AudioSource audios;

    public AudioClip recordedClip; // 녹음된 파일
    string micName = null; // 기본 마이크 사용 //메타퀘스트껄로 바꾸면 될듯함


    private void Start()
    {
        audios = GetComponent<AudioSource>();
    }


    public void StartRecording()
    {
        //10초까지 녹음, 44100Hz
        recordedClip = Microphone.Start(micName, false, 10, 44100);
        audios.clip = recordedClip;
        Debug.Log("녹음 시작");
    }

    public byte[] StopRecording()
    {
        Microphone.End(micName);
        Debug.Log("녹음 종료");

        // 필요하다면 바로 재생
        //audios.Play();

        byte[] recordedWavBytes = WavUtility.FromAudioClip(recordedClip);
        return recordedWavBytes;
    }
}

public static class WavUtility
{
    public static byte[] FromAudioClip(AudioClip clip)
    {
        if (clip == null) return null;
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        byte[] wav = ConvertToWav(samples, clip.channels, clip.frequency);
        return wav;
    }

    private static byte[] ConvertToWav(float[] samples, int channels, int sampleRate)
    {
        // 16비트 PCM 변환
        short[] intData = new short[samples.Length];
        byte[] bytesData = new byte[samples.Length * 2];

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * short.MaxValue);
            byte[] byteArr = System.BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        // WAV 헤더 생성
        byte[] header = GetWavHeader(bytesData.Length, channels, sampleRate);

        byte[] wav = new byte[header.Length + bytesData.Length];
        header.CopyTo(wav, 0);
        bytesData.CopyTo(wav, header.Length);

        return wav;
    }

    private static byte[] GetWavHeader(int dataLength, int channels, int sampleRate)
    {
        int byteRate = sampleRate * channels * 2;
        int fileSize = 36 + dataLength;

        byte[] header = new byte[44];
        System.Text.Encoding.ASCII.GetBytes("RIFF").CopyTo(header, 0);
        System.BitConverter.GetBytes(fileSize).CopyTo(header, 4);
        System.Text.Encoding.ASCII.GetBytes("WAVE").CopyTo(header, 8);
        System.Text.Encoding.ASCII.GetBytes("fmt ").CopyTo(header, 12);
        System.BitConverter.GetBytes(16).CopyTo(header, 16); // Subchunk1Size
        System.BitConverter.GetBytes((short)1).CopyTo(header, 20); // AudioFormat
        System.BitConverter.GetBytes((short)channels).CopyTo(header, 22);
        System.BitConverter.GetBytes(sampleRate).CopyTo(header, 24);
        System.BitConverter.GetBytes(byteRate).CopyTo(header, 28);
        System.BitConverter.GetBytes((short)(channels * 2)).CopyTo(header, 32); // BlockAlign
        System.BitConverter.GetBytes((short)16).CopyTo(header, 34); // BitsPerSample
        System.Text.Encoding.ASCII.GetBytes("data").CopyTo(header, 36);
        System.BitConverter.GetBytes(dataLength).CopyTo(header, 40);
        return header;
    }
}