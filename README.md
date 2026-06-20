# 스레드 (Thread) - "수동 제어"

과거에는 스레드를 직접 생성하여 백그라운드 작업을 했습니다. 하지만 스레드 생성 비용이 크고, 메인 스레드(UI)로 돌아오려면 복잡한 절차가 필요합니다.

```Csharp
using System.Threading;
using UnityEngine;

public class ThreadExample : MonoBehaviour
{
    void Start()
    {
        // 스레드 직접 생성 (오버헤드 발생)
        Thread thread = new Thread(() => {
            // 복잡한 연산
            Debug.Log("백그라운드에서 계산 중...");
            
            // 유니티 API는 멀티스레드에서 접근 불가 (에러 발생)
            // 반드시 메인 스레드로 돌아와야 함 (이게 매우 복잡함)
        });
        thread.Start();
    }
}
```

# 2단계: Task (TAP) - "표준 비동기 패턴"
C# 5.0부터 도입된 async/await입니다. 스레드 풀을 자동으로 관리하여 훨씬 효율적입니다.

```Csharp
using System.Threading.Tasks;
using UnityEngine;

public class TaskExample : MonoBehaviour
{
    async void Start()
    {
        // 스레드 풀에서 작업 실행
        string result = await Task.Run(() => {
            return "계산 완료";
        });
        
        // await 이후에는 호출했던 스레드(유니티 메인 스레드)로 자동 복귀
        Debug.Log(result);
    }
}
```
# 3단계: UniTask - "유니티 최적화의 끝판왕"
유니티 개발자가 만든 라이브러리로, 할당 비용(GC)을 제로에 가깝게 만들고 유니티 엔진에 완벽히 통합됩니다.

```Csharp
using Cysharp.Threading.Tasks; // 반드시 필요
using UnityEngine;

public class UniTaskExample : MonoBehaviour
{
    async void Start()
    {
        // 1. 메모리 할당 없는 대기
        await UniTask.Delay(1000); 

        // 2. 메인 스레드로 명시적 전환 (필요시)
        await UniTask.SwitchToMainThread();
        
        Debug.Log("UI 업데이트 가능");

        // 3. 오브젝트 파괴 시 자동 취소 (메모리 누수 방지)
        await UniTask.Yield(PlayerLoopTiming.Update).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
    }
}
```

### Q: 코루틴을 쓰지 않고 UniTask를 쓰는 결정적인 이유가 뭔가요?

- A: "첫째는 성능입니다. 코루틴은 매번 IEnumerator 객체를 생성하여 GC를 유발하지만, UniTask는 구조체 기반으로 설계되어 Zero Allocation을 지향합니다.

둘째는 안정성입니다. try-catch로 예외 처리가 가능하고, 반환값을 즉시 받을 수 있어 비동기 로직의 가독성과 유지보수성이 비약적으로 상승하기 때문입니다."

### 멀티스레딩 구현 시 가장 주의할 점은 무엇인가요?
A: "유니티의 대부분의 API(Transform, GameObject, UI 등)는 Thread-Safe하지 않습니다. 따라서 백그라운드 스레드에서 무거운 연산을 마친 뒤, 반드시 UniTask.SwitchToMainThread()와 같은 방식으로 메인 스레드로 복귀하여 엔진 API를 호출해야 한다는 점입니다."

### 요약: 언제 무엇을 쓸까?

- 스레드: 이제는 공부용으로만 보세요. (직접 쓰지 않음)

- Task: 일반적인 C# 비동기 로직이 필요할 때 사용합니다.

- UniTask: 유니티 게임 내의 모든 비동기 작업(UI 애니메이션, 데이터 로드, 대기)에 사용하세요.