using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

// 1. 기능 인터페이스
public interface IItemEffect { void Execute(); }

// 2. 실제 기능 클래스들
public class JumpEffect : IItemEffect { public void Execute() => Debug.Log("점프 완료!"); }
public class AttackEffect : IItemEffect { public void Execute() => Debug.Log("공격 완료!"); }

public class Reflectionfirst : MonoBehaviour
{
    // 성능을 위한 델리게이트 캐싱 딕셔너리
    private static Dictionary<string, Action> _cachedEffects = new Dictionary<string, Action>();

    void Start()
    {
        // 테스트: 문자열로 기능 실행
        ExecuteEffect("JumpEffect");
        ExecuteEffect("AttackEffect");
    }

    public void ExecuteEffect(string className)
    {
        // 1. 캐시 확인
        if (!_cachedEffects.ContainsKey(className))
        {
            Debug.Log($"[{className}] 처음 호출, 리플렉션으로 생성 중...");
            
            // 2. 리플렉션으로 타입 찾기
            Type type = Type.GetType(className);
            if (type == null) return;

            // 3. 인스턴스 생성 및 델리게이트 변환 (여기가 핵심!)
            // IItemEffect 객체를 만들고 그 안의 Execute 메서드를 델리게이트로 연결
            IItemEffect effect = (IItemEffect)Activator.CreateInstance(type);
            _cachedEffects[className] = effect.Execute;
        }

        // 4. 캐시된 델리게이트 실행 (일반 메서드 호출만큼 빠름)
        _cachedEffects[className]?.Invoke();
    }
}