using UnityEngine;
using UnityEngine.SceneManagement;

namespace PortfolioFilling.Core
{
    public static class HarnessSceneBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            var scene = SceneManager.GetActiveScene();
            if (!scene.IsValid() || scene.name != "HarnessTestScene")
            {
                return;
            }

            if (Object.FindFirstObjectByType<HarnessSceneInstaller>() != null)
            {
                return;
            }

            var root = new GameObject("HarnessSceneRoot");
            root.AddComponent<HarnessSceneInstaller>();
        }
    }
}
