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

# UniTask
| 메서드 | 용도 | 특징 |
|---------|---------|---------|
| `UniTask.Delay()` | 일정 시간 대기 | 밀리초, 프레임 단위 모두 지원 |
| `UniTask.DelayFrame()` | 지정 프레임 수만큼 대기 | 프레임 기반 처리에 적합 |
| `UniTask.WaitUntil()` | 조건이 참이 될 때까지 대기 | `await UniTask.WaitUntil(() => isLoaded);` |
| `UniTask.WaitWhile()` | 조건이 거짓이 될 때까지 대기 | `WaitUntil`의 반대 |
| `UniTask.SwitchToMainThread()` | 메인 스레드로 복귀 | Unity API 접근 전 사용 |
| `UniTask.SwitchToThreadPool()` | 백그라운드 스레드로 이동 | 무거운 연산 처리 |
| `UniTask.Yield()` | 다음 프레임까지 대기 | `yield return null`과 동일 |
| `UniTask.NextFrame()` | 다음 프레임까지 대기 | `Yield`보다 의도가 명확 |
| `UniTask.WhenAll()` | 여러 작업 병렬 실행 | 모든 작업 완료까지 대기 |
| `UniTask.WhenAny()` | 여러 작업 중 하나 완료 시 반환 | 레이스 처리 가능 |
| `.Forget()` | Fire-and-Forget 실행 | 반환값 무시 |
| `.AttachExternalCancellation()` | 외부 CancellationToken 연결 | 객체 파괴 시 취소 가능 |
| `.SuppressCancellationThrow()` | 취소 예외 무시 | try-catch 감소 |                         |


---

# Task
| 메서드 | 용도 | 샘플 코드 |
|---------|---------|---------|
| `Task.Run()` | 백그라운드 스레드에서 작업 실행 | `await Task.Run(() => HeavyLogic());` |
| `Task.WhenAll()` | 여러 작업이 모두 끝날 때까지 대기 | `await Task.WhenAll(t1, t2, t3);` |
| `Task.WhenAny()` | 가장 빨리 끝나는 작업 하나 대기 | `var finished = await Task.WhenAny(t1, t2);` |
| `Task.Delay()` | 비동기 시간 지연 | `await Task.Delay(1000);` |
| `Task.FromResult()` | 이미 완료된 값을 가진 Task 반환 | `return Task.FromResult(42);` |
| `Task.CompletedTask` | 완료된 Task 반환 (`void` 대체) | `return Task.CompletedTask;` |
| `Task.FromException()` | 예외를 포함한 Task 생성 | `return Task.FromException<int>(ex);` |
| `Task.FromCanceled()` | 취소된 Task 생성 | `return Task.FromCanceled(token);` |
| `ContinueWith()` | 이전 작업 완료 후 후속 작업 실행 | `task.ContinueWith(t => Debug.Log("Done"));` |


------추가 ---  

### TaskCompletionSource 

TaskCompletionSource<TResult>는 .NET에서 비동기 작업이 아닌 일반적인 작업이나 이벤트를 Task 기반의 비동기 패턴(TAP)으로 변환할 때 사용하는 매우 강력한 도구

쉽게 말해, "언제 끝날지 모르는 어떤 작업을 Task 객체로 감싸서, 우리가 원하는 시점에 결과(성공, 실패, 취소)를 수동으로 알려줄 수 있게 만드는 래퍼(Wrapper)"라고 이해하시면 됩니다.

### 왜 사용하는가?
보통 Task.Run이나 async/await은 기존에 비동기 기능을 지원하는 API를 호출할 때 사용합니다. 하지만 외부 라이브러리, 레거시 API, 혹은 특정 이벤트 기반의 코드는 Task를 반환하지 않습니다. 이때 TaskCompletionSource를 사용하면 이를 Task처럼 다룰 수 있어 비동기 흐름(await)에 쉽게 통합할 수 있습니다.

### 핵심 동작 원리
TaskCompletionSource는 내부적으로 Task 객체를 가지고 있으며, 다음 메서드들을 통해 상태를 제어합니다.

- SetResult(TResult): 작업을 성공적으로 완료하고 결과를 반환합니다.

- SetException(Exception): 작업 도중 오류가 발생했음을 알립니다.

- SetCanceled(): 작업이 취소되었음을 알립니다.

```Csharp
코드 예제: 이벤트 기반 작업을 Task로 변환하기
다음은 특정 이벤트가 발생할 때까지 기다렸다가 결과를 받는 예제입니다.
public Task<string> WaitForEventAsync()
{
    var tcs = new TaskCompletionSource<string>();

    // 어떤 외부 이벤트 발생 시 처리
    myObject.OnDataReceived += (data) => 
    {
        // 이벤트가 발생하면 Task를 완료 처리함
        tcs.SetResult(data);
    };

    return tcs.Task; // 호출자에게 Task를 반환하여 await 할 수 있게 함
}

// 사용 예시
string result = await WaitForEventAsync();
Console.WriteLine(result);
```

```Csharp
TaskCompletionSource를 활용한 차징 로직 예시
차징을 시작하고, 특정 시간이 지나거나 혹은 사용자가 버튼을 뗐을 때 결과를 반환하는 방식입니다.
using System.Threading.Tasks;
using UnityEngine;

public class ChargeSkill : MonoBehaviour
{
    private TaskCompletionSource<float> _tcs;

    public async Task<float> StartChargingAsync()
    {
        _tcs = new TaskCompletionSource<float>();
        
        float chargeTime = 0f;
        bool isCharging = true;

        // 차징 루프
        while (isCharging)
        {
            if (Input.GetMouseButtonUp(0)) // 버튼을 떼면 종료
            {
                isCharging = false;
            }
            else
            {
                chargeTime += Time.deltaTime;
                // 최대 차징 시간 제한 예시
                if (chargeTime >= 3.0f) isCharging = false; 
            }
            await Task.Yield(); // 다음 프레임까지 대기
        }

        _tcs.SetResult(chargeTime); // 최종 차징 시간 반환
        return await _tcs.Task;
    }
}
```
