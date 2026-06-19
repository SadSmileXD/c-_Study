using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
public class ReflectionTest001 : MonoBehaviour
{
    Dictionary<Type, BaseMonster> monsters = new();
    [SerializeReference] public List<BaseMonster> Monsters = new();
    private void Start()
    {
        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (typeof(BaseMonster).IsAssignableFrom(type)&& !type.IsAbstract)
            {
                BaseMonster monster =
                    Activator.CreateInstance(type) as BaseMonster;

                monsters.Add(type, monster);
                Monsters.Add(monster);
            }
        }

        foreach (BaseMonster monster in monsters.Values)
        {
            Debug.Log(monster.GetType().Name);
            monster.Attack();
        }
    }
}
