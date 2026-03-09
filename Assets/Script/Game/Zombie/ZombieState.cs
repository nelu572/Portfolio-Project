using UnityEngine;

public class ZombieState : MonoBehaviour
{
    private Animator anima;

    [SerializeField] protected float FULL_HP = 30;
    protected float hp;

    void Start()
    {
        hp = FULL_HP;
        anima = GetComponent<Animator>();
    }

    public void Hit(float Damage)
    {
        anima.SetTrigger("Hit");
        hp -= Damage;
        if (CheckDie())
        {
            Die();
        }
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
