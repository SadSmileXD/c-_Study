using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
public class SOReflection : MonoBehaviour
{
    Dictionary<Type, BaseSkillSO> skills = new();
    [SerializeReference] List<BaseSkillSO> skillLists=new();
    private void Start()
    {
        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (typeof(BaseSkillSO).IsAssignableFrom(type)
                && !type.IsAbstract)
            {
                BaseSkillSO skill =
                    ScriptableObject.CreateInstance(type) as BaseSkillSO;

                skills.Add(type, skill);
                skillLists.Add(skill);
            }
        }

        foreach (BaseSkillSO skill in skills.Values)
        {
            Debug.Log(skill.GetType().Name);
            skill.Use();
        }
    }
}
