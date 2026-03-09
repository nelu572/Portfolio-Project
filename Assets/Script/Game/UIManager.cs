using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image DashCooldown;
    void Update()
    {
        Debug.Log(PlayerState.Instance.GetDashRemainingNormalized());
        DashCooldown.fillAmount = PlayerState.Instance.GetDashRemainingNormalized();
    }
}
