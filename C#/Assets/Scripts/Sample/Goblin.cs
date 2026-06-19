using UnityEngine;
[System.Serializable]
public class Goblin : BaseMonster
{
    public override void Attack()
    {
        Debug.Log("고블린 공격!");
    }
}