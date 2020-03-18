using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Game
{
    public class ReactNative : MonoBehaviour
    {
        // Tất cả các action name gửi cho React Native:
        /*
         "ready" : Unity đã load lên hoàn toàn.
         "error" : Thông báo lỗi cho React Native. - Kèm data: string
         "info"  : Thông báo đơn thuần cho React Native. - Kèm data: string
         "exit"  : Player muốn thoát game.
             */

        public static ReactNative Instance { get; private set; }
        private const string GAMEOBJECT_NAME = "ReactNative";
        private Action<bool> _onResultAudio;

        static ReactNative()
        {
            if (Instance != null)
                return;

            GameObject go = new GameObject(GAMEOBJECT_NAME);
            Instance = go.AddComponent<ReactNative>();
            DontDestroyOnLoad(go);
        }

        public static void Hello()
        {
            Debug.Log("Hello");
        }

        public void Goodbye()
        {
            Debug.Log("Goodbye");
        }

        private void OnApplicationPause(bool pause)
        {
            Time.timeScale = pause ? 0 : 1;
            AudioListener.pause = pause;
        }

        #region Gửi Message cho React

        /// <summary>
        /// Gửi điểm cho React Native
        /// </summary>
        public static void SendScore(SendScoreData scoreData)
        {
            string json = JsonConvert.SerializeObject(scoreData);
            UnityMessageManager.Instance.SendMessageToRN(json);
        }

        /// <summary>
        /// Gửi điểm cho React Native
        /// </summary>
        public static void SendScore(SendScoreData[] scoreData)
        {
            string json = JsonConvert.SerializeObject(scoreData);
            UnityMessageManager.Instance.SendMessageToRN(json);
        }

        /// <summary>
        /// Gửi điểm cho React Native
        /// </summary>
        public static void SendScore(List<SendScoreData> scoreData)
        {
            string json = JsonConvert.SerializeObject(scoreData);
            UnityMessageManager.Instance.SendMessageToRN(json);
        }

        private const string RECORD_AUDIO_NULL_ACTION = "Phải truyền vào hàm xử lý kết quả trả về của ghi âm";
        private const string RECORD_AUDIO_EMPTY_KEYWORD = "keyWord không được rỗng";
        private const string RECORD_AUDIO_SECOND_IS_ZERO = "second phải lớn hơn 0";
        public static void RecordAudio(string keyWord, int second, Action<bool> onResult)
        {
            Assert.IsNotNull(onResult, RECORD_AUDIO_NULL_ACTION);
            Assert.IsTrue(!string.IsNullOrEmpty(keyWord), RECORD_AUDIO_EMPTY_KEYWORD);
            Assert.IsTrue(second > 0, RECORD_AUDIO_SECOND_IS_ZERO);

            Instance._onResultAudio = onResult;
            Instance.StartCoroutine(StartRecording());

            IEnumerator StartRecording()
            {
                //AudioListener.pause = true;
                var clip = Microphone.Start(null, false, second, 44100);

                yield return new WaitUntil(() => Microphone.IsRecording(null) == false);
                //AudioListener.pause = false;
                WavUtility.FromAudioClip(clip, out string path, true);

                RecordAudioData recordData = new RecordAudioData(keyWord, path);
                Debug.Log(recordData.ToString());
                UnityMessageManager.Instance.SendMessageToRN(recordData.ToString());
            }
        }

        private const string RESULT_AUDIO_CORRECT = "true";
        public void ResultAudio(string result)
        {
            if (result == RESULT_AUDIO_CORRECT)
                _onResultAudio?.Invoke(true);
            else
                _onResultAudio?.Invoke(false);
        }

        private const string CMD_EXIT = "exit";
        public static void SendExitCommandToRN()
        {
            CommonData data = new CommonData
            {
                Action = CMD_EXIT,
                Data = null
            };

            string json = JsonConvert.SerializeObject(data);
            UnityMessageManager.Instance.SendMessageToRN(json);
        }

        #endregion

    }// class
}
