using UnityEngine;
using System.Collections;
using PathCreation.Examples;
public class triggerProjectile : MonoBehaviour
{

    public GameObject projectile;
    public Transform shootPoint;

    private GameObject magicMissile;
    //public float attackLenght;
    //public float attackRange;
    public GameObject hitEffect;

    public GameObject hideObject;
    public DwarfEnum dwarfType;

    public void shoot()
    {
        if (GameManager.Instance.bestTarget != null)
        {
            magicMissile = Instantiate(projectile, shootPoint.position, transform.rotation) as GameObject;
            StartCoroutine(lerpyLoop(magicMissile));
        }
    }

    // shoot loop
    public IEnumerator lerpyLoop(GameObject projectileInstance)
    {

        if (hideObject)
            hideObject.SetActive(false);

        if (GameManager.Instance.bestTarget == null)
        {
            Destroy(projectileInstance);
            yield return null;
        }
        GameObject target = GameManager.Instance.bestTarget;
        var victim = target.GetComponent<PathFollower>().GetTargetLocation(1.0f);

        float progress = 0;
        float timeScale = 1.0f;
        //attackLenght;

        Vector3 origin = projectileInstance.transform.position;

        // lerp ze missiles!
        while (progress < 1)
        {
            if (projectileInstance)
            {
                progress += timeScale * Time.deltaTime;
                float ypos = (progress - Mathf.Pow(progress, 2)) * 12;
                float ypos_b = ((progress + 0.1f) - Mathf.Pow((progress + 0.1f), 2)) * 12;
                projectileInstance.transform.position = Vector3.Lerp(origin, victim, progress) + new Vector3(0, ypos, 0);
                if (progress < 0.9f)
                {
                    projectileInstance.transform.LookAt(Vector3.Lerp(origin, victim, progress + 0.1f) + new Vector3(0, ypos_b, 0));
                }
                yield return null;
            }
        }
        if ((int)dwarfType == 2)
        {
            if (target != null)
            {
                GameManager.Instance.GetDwarfDamage(dwarfType, target);
            }

        }

        if ((int)dwarfType == 1)
        {
            for (int i = 0; i < GameManager.Instance.EnemyList.Count; i++)
            {
                if (Vector3.Distance(projectileInstance.transform.position, GameManager.Instance.EnemyList[i].transform.position) < 5.0f)
                {
                    GameManager.Instance.GetDwarfDamage(dwarfType, GameManager.Instance.EnemyList[i]);
                }
            }
        }

        Destroy(projectileInstance);

        if (hitEffect)
            Instantiate(hitEffect, victim, transform.rotation);

        if (hideObject)
            hideObject.SetActive(true);
        yield return null;
    }

    public void clearProjectiles()
    {
        if (magicMissile)
            Destroy(magicMissile);
    }

}
