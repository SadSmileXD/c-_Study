using System.Diagnostics;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TaskBenchmark : MonoBehaviour
{
    private async void Start()
    {
        await TestTask();
        await TestUniTask();
    }

    async Task TestTask()
    {
        Stopwatch sw = Stopwatch.StartNew();

        for (int i = 0; i < 10; i++)
        {
            await DummyTask();
        }

        sw.Stop();

        Debug.Log($"Task : {sw.ElapsedMilliseconds} ms");
    }

    async UniTask TestUniTask()
    {
        Stopwatch sw = Stopwatch.StartNew();

        for (int i = 0; i < 10; i++)
        {
            await DummyUniTask();
        }

        sw.Stop();

        Debug.Log($"UniTask : {sw.ElapsedMilliseconds} ms");
    }

    async Task DummyTask()
    {
        await Task.Yield();
    }

    async UniTask DummyUniTask()
    {
        await UniTask.Yield();
    }
}