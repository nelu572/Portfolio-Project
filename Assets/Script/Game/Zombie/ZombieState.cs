using UnityEngine;

public class ZombieState : MonoBehaviour
{
    private Animator anima;

    [SerializeField] protected float FULL_HP = 30;
    protected float hp;

    protected bool isInvincible;

    void Start()
    {
        hp = FULL_HP;
        anima = GetComponent<Animator>();
    }

    public void Hit(float Damage)
    {
        if (isInvincible)
        {
            if (CheckDie())
            {
                Die();
            }
            return;
        }
        isInvincible = true;
        anima.SetTrigger("Hit");
        hp -= Damage;
        if (CheckDie())
        {
            Die();
        }
    }

    public void EndInvincibleTime()
    {
        isInvincible = false;
    }

    private bool CheckDie()
    {
        Debug.Log(hp);
        return hp <= 0;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
