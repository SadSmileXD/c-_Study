using System;
using UnityEngine;
[Serializable]
public abstract class BaseSkillSO : ScriptableObject
{
    public abstract void Use();
}