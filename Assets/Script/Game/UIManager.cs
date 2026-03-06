using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image DashCooldown;
    void Update()
    {
        Debug.Log(PlayerStatus.Instance.GetDashRemainingNormalized());
        DashCooldown.fillAmount = PlayerStatus.Instance.GetDashRemainingNormalized();
    }
}
