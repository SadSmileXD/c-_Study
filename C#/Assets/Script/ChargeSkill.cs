using UnityEngine;
using System.Threading.Tasks;
public class ChargeSkill : MonoBehaviour
{
    private TaskCompletionSource<float> _tcs;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 클릭 시작 시
        {
            // 비동기 함수 호출
            ExecuteSkill();
        }
    }
    private async void ExecuteSkill()
    {
        Debug.Log("차징 시작!");

        // await를 사용하여 StartChargingAsync가 끝날 때까지 기다립니다.
        float finalCharge = await  StartChargingAsync();

        Debug.Log($"차징 완료! 최종 시간: {finalCharge}초");

        // 여기에 차징 시간에 따른 스킬 발사 로직 추가
    }

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