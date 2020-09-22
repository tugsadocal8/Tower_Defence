using PathCreation.Examples;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject enemy;
    public Vector3 unitPosBarrel;
    public Vector3 target;
    public Vector3 shoot;
    public float unitDamage;

    void Start()
    {
        this.gameObject.transform.position = new Vector3(unitPosBarrel.x, unitPosBarrel.y, unitPosBarrel.z);
        target = enemy.transform.position;
        shoot = (target - unitPosBarrel).normalized;
    }
    void Update()
    {
        
        if (enemy != null)
        {
            this.gameObject.transform.position += shoot * 10.0f * Time.deltaTime;
            float bulletEnemyDistance = Vector3.Distance(this.gameObject.transform.position, target);
            if (bulletEnemyDistance < 1.5f)
            {

                Destroy(gameObject);
                GiveDamage();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GiveDamage()
    {
        enemy.GetComponent<PathFollower>().TakeDamage(unitDamage);
    }
}
