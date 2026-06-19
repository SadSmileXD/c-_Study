using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOReflection2 : MonoBehaviour
{
    public List<BaseSkillSO> m_List=new();
    void Start()
    {
        BaseSkillSO[] skills =
         Resources.LoadAll<BaseSkillSO>("Skills");
        
        foreach (BaseSkillSO skill in skills)
        {
            Debug.Log(skill.name);
            m_List.Add(skill);
            skill.Use();
        }
    }
 
}
