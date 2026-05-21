using UnityEngine;
using UnityEngine.SceneManagement;

namespace PortfolioFilling.Core
{
    public sealed class SceneLoader : MonoBehaviour
    {
        public void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
