using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Zombie;
    [SerializeField] private Transform player;
    [SerializeField] private Vector3[] spawnPoints;
    
    void Start()
    {
        SpawnZombie();
    }
    public void SpawnZombie()
    {
        foreach (Vector3 spawnPoint in spawnPoints)
        {
            GameObject zombie = Instantiate(Zombie, spawnPoint, Quaternion.identity);

            zombie.GetComponent<ZombieAI>().Init(player);
        }
    }
}