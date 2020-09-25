using PathCreation;
using PathCreation.Examples;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[Serializable]
public class EnemyPrefabData
{
    public EnemyEnum type;
    public GameObject prefab;
}
[Serializable]
public class UnitPrefabData
{
    public int unitIndex;
    public float unitRange;
    public UnitEnum type;
    public float unitValue;
    public GameObject prefab;
}

[Serializable]
public class DwarfData
{
    public DwarfEnum type;
    public WalkingOrc dwarf;
    public float range;
    public float damage;
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<GameObject> EnemyList;
    public List<EnemyPrefabData> EnemyPrefabDataList;
    public List<UnitPrefabData> UnitPrefabDataList;
    public List<DwarfData> DwarfDataList;
    public UnitManager activeUnitManager;
    public UnitCanvas unitCanvas;
    public LayerMask layerMask;

    public LevelManager levelManager;
    public GameObject bullet;
    public PathCreator creator;

    public GameObject tower;
    public GameObject bestTarget;

    public int activeLevel;
    public int activeWave;
    public float money = 20;
    public Text moneyText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
            levelManager = new LevelManager();
            LoadData();
            CreateGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SpawnEnemy(EnemyEnum enemyEnum, float enemyOffTime, float speed, float health, float monetaryValue)
    {
        

        for (int i = 0; i < EnemyPrefabDataList.Count; i++)
        {
            if (EnemyPrefabDataList[i].type == enemyEnum)
            {
                GameObject clone = Instantiate(EnemyPrefabDataList[i].prefab);
                PathFollower pathFollower = clone.GetComponent<PathFollower>();
                pathFollower.pathCreator = creator;
                pathFollower.tower = tower;
                pathFollower.timer = enemyOffTime;
                pathFollower.speed = speed;
                pathFollower.health = health;
                pathFollower.monetaryValue = monetaryValue;
                AddEnemyList(clone);
            }
        }
    }
    public void SpawnUnit(UnitEnum unitEnum, Vector3 unitPos)
    {
        for (int i = 0; i < UnitPrefabDataList.Count; i++)
        {
            if (UnitPrefabDataList[i].type == unitEnum)
            {
                // if (money >= UnitPrefabDataList[i].unitValue)
                // {
                GameObject clone = Instantiate(UnitPrefabDataList[i].prefab);
                clone.transform.position = unitPos;
                UnitManager unitManager = clone.GetComponent<UnitManager>();
                unitManager.unitPos = unitPos;
                unitManager.unitPrice = UnitPrefabDataList[i].unitValue;
                SpendMoney(UnitPrefabDataList[i].unitValue);
                // }
            }
        }
    }
    public void SpawnBullet(float unitDamage, GameObject enemy, Vector3 unitPosBarrel)
    {
        GameObject clone = Instantiate(bullet);
        Shooting shooting = clone.GetComponent<Shooting>();
        shooting.unitPosBarrel = unitPosBarrel;
        shooting.unitDamage = unitDamage;
        shooting.enemy = enemy;
    }

    public void AddEnemyList(GameObject enemy)
    {
        EnemyList.Add(enemy);
    }
    public void RemoveEnemyList(GameObject enemy)
    {
        EnemyList.Remove(enemy);
        if (EnemyList.Count == 0)
        {
            activeWave++;
            CreateGame();
        }
    }
    public void LoadData()
    {
        levelManager.LoadLevelData();
    }
    public void CreateGame()
    {
        activeLevel = 1;
        levelManager.SetLevel(activeLevel);
        levelManager.SetWave(activeWave);
        levelManager.CreateWave();
    }
    public void CreateUnit(UnitEnum unitType, Vector3 unitPosition)
    {

        unitPosition = new Vector3(unitPosition.x, 0.5f, unitPosition.z);
        SpawnUnit(unitType, unitPosition);
    }
    public void OnUnitClick(UnitManager unitManager)
    {
        if (activeUnitManager == unitManager)
        {
            activeUnitManager = null;
        }
        else
        {
            activeUnitManager = unitManager;
        }
        unitCanvas.OpenCanvas();
    }

    public void EarnMoney(float monetaryValue)
    {
        money += monetaryValue;
        moneyText.text = money.ToString();
    }
    public void SpendMoney(float unitPrice)
    {
        money -= unitPrice;
        moneyText.text = money.ToString();
    }
    public void SaleUnit(float unitPrice)
    {
        money += unitPrice - (unitPrice * 0.25f);
        moneyText.text = money.ToString();
    }
    public void UpgradePrice(float unitPrice)
    {
        money -= unitPrice;
        moneyText.text = money.ToString();
    }

    public void DwarfSetPosition(DwarfEnum dwarfType, Vector3 dwarfPos)
    {
        foreach (var item in DwarfDataList)
        {
            if (item.type == dwarfType)
            {
                GameObject hand = GameObject.Find(item.dwarf.gameObject.name);
                if (hand!=null)
                {
                    Vector3 newDwarfPos = dwarfPos;
                    item.dwarf.GetComponent<WalkingOrc>().SetPosition(newDwarfPos);
                }
                item.dwarf.gameObject.SetActive(true);
                item.dwarf.GetComponent<WalkingOrc>().SetPosition(dwarfPos);
                item.dwarf.SetPosition(dwarfPos);
            }
            else
            {
                item.dwarf.gameObject.SetActive(false);
            }
        }
    }

    public void GetDwarfDamage(DwarfEnum dwarfType, GameObject target)
    {
        for (int i = 0; i < DwarfDataList.Count; i++)
        {
            if (DwarfDataList[i].type == dwarfType)
            {
                target.GetComponent<PathFollower>().TakeDamage(DwarfDataList[i].damage);
                DwarfDataList[i].dwarf.isDwarfPlaying=true;

            }
        }

    }
}


