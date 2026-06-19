using System.Reflection;
using UnityEngine;

public class ReflectionReflection_Test : MonoBehaviour
{
    public PlayerData player;
    public int LV;
    public string m_name;
    [ContextMenu("Reflection")]
    public void Reflection()
    {
        player = new();
        player.LV = LV;
        player.Player_Name = m_name;
       // 1. 객체의 타입 정보(Type)를 가져옵니다.
       var data = player.GetType();

        // 2. 클래스 내부의 모든 필드(멤버 변수) 정보를 가져옵니다.
        // BindingFlags를 사용해 public/private 등 범위를 지정할 수 있습니다.
        FieldInfo[] fields = data.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var field in fields)
        {
            // 3. 필드 이름과 현재 가지고 있는 값을 출력합니다.
            Debug.Log($"필드 이름: {field.Name}, 값: {field.GetValue(player)}");
        }
    }
    
}
