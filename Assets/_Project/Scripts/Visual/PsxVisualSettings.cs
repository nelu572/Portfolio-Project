using UnityEngine;

namespace PortfolioFilling.Visual
{
    public sealed class PsxVisualSettings : MonoBehaviour
    {
        [SerializeField] private Color fogColor = new(0.07f, 0.07f, 0.08f, 1f);
        [SerializeField] private float fogDensity = 0.025f;

        private void Start()
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.ambientIntensity = 0.65f;

            if (Camera.main == null)
            {
                return;
            }

            Camera.main.allowHDR = false;
            Camera.main.allowMSAA = false;
            Camera.main.backgroundColor = new Color(0.05f, 0.05f, 0.06f);
        }
    }
}
