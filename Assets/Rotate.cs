using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Rotate : MonoBehaviour
{
    public Text _debugText1;
    public Text _audioText;
    public InputField _inputField;
    private bool canRotate = true;

    private AudioSource _audioSource;

    // Use this for initialization
    void Awake()
    {
        _inputField.text = "sister";
        UnityMessageManager.Instance.OnRNMessage += onMessage;

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
    }

    void onDestroy()
    {
        UnityMessageManager.Instance.OnRNMessage -= onMessage;
    }

    void onMessage(MessageHandler message)
    {
        var data = message.getData<string>();
        Debug.Log("onMessage:" + data);
    }

    public void SendRecord()
    {
        AudioListener.pause = true;
        var clip = Microphone.Start(null, false, 3, 44100);
        StartCoroutine(WaitForCompleted());

        IEnumerator WaitForCompleted()
        {
            _debugText1.text = "Đang thu âm";
            yield return new WaitUntil(() => Microphone.IsRecording(null) == false);
            _debugText1.text = "Thu âm xong";
            AudioListener.pause = false;
            WavUtility.FromAudioClip(clip, out string path, true);
            string message = "{'action':'start', 'data':'" + path + "'}";
            Debug.Log(message);
            UnityMessageManager.Instance.SendMessageToRN(message);
        }
        
    }

    public void ResultAudio(string message)
    {
        //iPhoneSpeaker.ForceToSpeaker();
        AudioListener.pause = false;
        AudioListener.volume = 1f;
        _debugText1.text = message;
    }

    private void Update()
    {
        _audioText.text = string.Format("AudioListener: pause:{0} volume:{1}", AudioListener.pause, AudioListener.volume);
    }

}