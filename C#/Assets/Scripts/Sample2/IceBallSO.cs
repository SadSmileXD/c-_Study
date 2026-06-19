using UnityEngine;

[CreateAssetMenu(menuName = "Skill/IceBall")]
public class IceBallSO : BaseSkillSO
{
    public int damage = 50;

    public override void Use()
    {
        Debug.Log($"IceBall : {damage}");
    }
}