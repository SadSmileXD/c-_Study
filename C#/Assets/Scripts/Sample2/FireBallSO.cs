using UnityEngine;

[CreateAssetMenu(menuName = "Skill/FireBall")]
public class FireBallSO : BaseSkillSO
{
    public int damage = 100;

    public override void Use()
    {
        Debug.Log($"FireBall : {damage}");
    }
}