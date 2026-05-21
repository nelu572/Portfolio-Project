using UnityEngine;

namespace PortfolioFilling.Visual
{
    public sealed class FlickerLight : MonoBehaviour
    {
        [SerializeField] private float minIntensity = 0.8f;
        [SerializeField] private float maxIntensity = 1.7f;
        [SerializeField] private float speed = 7f;

        private Light _light;
        private float _seed;

        private void Awake()
        {
            _light = GetComponent<Light>();
            _seed = Random.Range(0f, 100f);
        }

        private void Update()
        {
            if (_light == null)
            {
                return;
            }

            var noise = Mathf.PerlinNoise(_seed, Time.time * speed);
            _light.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        }
    }
}
