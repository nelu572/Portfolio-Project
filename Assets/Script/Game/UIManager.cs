using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image DashCooldown;
    void Update()
    {
        DashCooldown.fillAmount = PlayerState.Instance.GetDashRemainingNormalized();
    }
}
