using System.IO;
using UnityEngine;
using UnityNeuroSpeech.Shared;
using UnityNeuroSpeech.Utils;

namespace UnityNeuroSpeech.Runtime.JsonData
{
    internal class ControllerJsonDataModule
    {
        private string _jsonDialogHistoryFileName, _encryptionHistoryKey;

        public ControllerJsonDataModule(string jsonDialogHistoryFileName, string encryptionHistoryKey)
        {
            _jsonDialogHistoryFileName = jsonDialogHistoryFileName;
            _encryptionHistoryKey = encryptionHistoryKey;
        }

        public FrameworkSettings? LoadSharedSettingsModular()
        {
            var settingsFile = Resources.Load<TextAsset>(StaticData.FRAMEWORK_SETTINGS_FILE_PATH_IN_RESOURCES);

            if (!settingsFile)
            {
                LogUtils.LogError("You need to create settings in \"UnityNeuroSpeech/Create Settings\"!");
                return null;
            }
            
            return JsonUtility.FromJson<FrameworkSettings>(settingsFile.text);
        }
        
        /// <summary>
        /// For Ollama
        /// </summary>
        public void UpdateJsonDialogHistoryModular(DialogData lastDialog)
        {
            // If user doesn't want to save history
            if (string.IsNullOrEmpty(_jsonDialogHistoryFileName)) return;

            var jsonLoaded = File.ReadAllText(Path.Combine(StaticData.AGENT_HISTORY_PATH, $"{_jsonDialogHistoryFileName}.json"));

            DialogHistoryData runtimeData;
            if (!string.IsNullOrEmpty(_encryptionHistoryKey))
            {
                try
                {
                    var encryptedData = EncryptionUtils.Decrypt(jsonLoaded, _encryptionHistoryKey);
                    runtimeData = JsonUtility.FromJson<DialogHistoryData>(encryptedData);
                }
                catch
                {
                    LogUtils.LogError($"Error decoding encrypted data with this key! If you don't use any encryption, delete key parameter in your AgentBehaviour script.");
                    return;
                }
            }
            else runtimeData = JsonUtility.FromJson<DialogHistoryData>(jsonLoaded);

            runtimeData.dialogHistory.Add(lastDialog);

            var jsonToSave = JsonUtility.ToJson(runtimeData);

            if (!string.IsNullOrEmpty(_encryptionHistoryKey))
            {
                File.WriteAllText(Path.Combine(StaticData.AGENT_HISTORY_PATH, $"{_jsonDialogHistoryFileName}.json"), EncryptionUtils.Encrypt(jsonToSave, _encryptionHistoryKey));
            }
            else File.WriteAllText(Path.Combine(StaticData.AGENT_HISTORY_PATH, $"{_jsonDialogHistoryFileName}.json"), jsonToSave);

            LogUtils.LogMessage("Dialog history updated");
        }
    }
}