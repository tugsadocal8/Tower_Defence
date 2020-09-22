using UnityEngine;
using System;
using System.Collections.Generic;


[Serializable]
public class UnitSpecs
{
    public float unitType;
    public float unitFrequency;
    public float maxRangeUnit;
    public float unitDamage;
    public float minExperienceForUpgrade;
    public float timer;
}
public class UnitManager : MonoBehaviour
{
    public Vector3 unitPos;
    public List<UnitSpecs> specsArray;
    public float timer;
    public int unitTypeId;
    public float turretLagSpeed = 10f;
    public Vector3 finalTurretLookDir;
    public List<GameObject> turretBarrel;
    public List<GameObject> turret;
    public int unitCanvasId;
    public const int floorLayer = 8;
    public int activeUnitLevel;
    public float unitPrice;

    void Start()
    {
       
        activeUnitLevel = 0;
        timer = specsArray[activeUnitLevel].unitFrequency;
        SetActiveTurret();
    }
    void Update()
    {
        timer -= Time.deltaTime;

        for (int i = 0; i < GameManager.Instance.EnemyList.Count; i++)
        {
            GameObject enemy = GameManager.Instance.EnemyList[i];
            float distance = Vector3.Distance(turret[activeUnitLevel].transform.position, enemy.transform.position);
            if (distance < specsArray[activeUnitLevel].maxRangeUnit)
            {
                Vector3 turretLookDir = GameManager.Instance.EnemyList[i].transform.position - turret[activeUnitLevel].transform.position;
                turretLookDir.y = 0f;

                finalTurretLookDir = Vector3.Lerp(finalTurretLookDir, turretLookDir, Time.deltaTime * turretLagSpeed);
                turret[activeUnitLevel].transform.rotation = Quaternion.LookRotation(finalTurretLookDir);

                if (timer < 0)
                {
                    GameManager.Instance.SpawnBullet(specsArray[activeUnitLevel].unitDamage, enemy, turretBarrel[activeUnitLevel].transform.position);
                    timer = specsArray[activeUnitLevel].unitFrequency;
                    specsArray[activeUnitLevel].minExperienceForUpgrade--;
                    if (specsArray[activeUnitLevel].minExperienceForUpgrade<=0)
                    {
                        UpdateCanvas();
                    }                 
                }
                break;
            }
       }
    }
    private void SetActiveTurret()
    {
        foreach (var item in turret)
        {
            item.SetActive(false); 
        }
        turret[activeUnitLevel].SetActive(true);
    }
    public void UnitUp()
    {
        activeUnitLevel++;
        activeUnitLevel= Mathf.Min(activeUnitLevel, 2);
        GameManager.Instance.UpgradePrice(unitPrice*0.25f+unitPrice);
        SetActiveTurret();
        UpdateCanvas();

    }
    public void UpdateCanvas()
    {
        if (GameManager.Instance.activeUnitManager == this)
        {
            GameManager.Instance.unitCanvas.UpdateCanvas();
        }
    }   
    public bool isUpgradable()
    {
        return specsArray[activeUnitLevel].minExperienceForUpgrade <= 0;
    }
    public void UnitSelling()
    {
        GameManager.Instance.SaleUnit(unitPrice);
        Destroy(this.gameObject);
        
    }
}
