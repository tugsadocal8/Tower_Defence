using System.Collections.Generic;

public enum EnemyEnum
{
    Archer = 0,
    Knight = 1,
    Horseman = 2,
    Crossbowman = 3,
    Skirmish = 4
}

public enum UnitEnum
{
    XSmall = 0,
    Small = 1,
    Medium = 2,
    Large = 3,
    XLarge = 4,

}

public enum DwarfEnum
{
    Warrior = 0,
    Engineer = 1,
    Archer = 2,
}
#region JsonClassesEnemy

public class Enemy
{
    public int EnemyId { get; set; }
    public float Offtime { get; set; }
    public float Speed { get; set; }
    public int Health { get; set; }
    public float MonetaryValue { get; set; }
}
public class Wave
{
    public int Index;
    public List<Enemy> Enemies { get; set; }
}
public class Level
{
    public int Index { get; set; }
    public List<Wave> Waves { get; set; }
}
public class LevelData
{
    public List<Level> Levels { get; set; }
}
#endregion
