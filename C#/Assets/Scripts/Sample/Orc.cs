using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class Orc : BaseMonster
{
    public override void Attack()
    {
        Debug.Log("오크 공격!");
    }
}