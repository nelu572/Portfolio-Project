using System;
using System.IO;
using UnityEngine;

namespace PortfolioFilling.Core
{
    [Serializable]
    public sealed class HarnessSaveData
    {
        public int highestWaveReached;
        public int scrap;
    }

    public sealed class SaveManager : MonoBehaviour
    {
        private string SavePath => Path.Combine(Application.persistentDataPath, "harness-save.json");

        public void Save(HarnessSaveData data)
        {
            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
        }

        public HarnessSaveData LoadOrCreate()
        {
            if (!File.Exists(SavePath))
            {
                return new HarnessSaveData();
            }

            var json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<HarnessSaveData>(json) ?? new HarnessSaveData();
        }
    }
}
