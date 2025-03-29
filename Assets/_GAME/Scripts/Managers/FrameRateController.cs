using System.Threading.Tasks;
using UnityEngine;

public class FrameRateController : MonoBehaviour
{
    [Header("Frame Settings")]            // Tắt giới hạn mặc định của Unity
    public float TargetFrameRate = 60.0f;        // Mục tiêu frame rate mong muốn
    private float currentFrameTime;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;          // Tắt vSync để không đồng bộ với GPU
        Application.targetFrameRate = 9999;   // Đặt giới hạn frame rate cực cao
        currentFrameTime = Time.realtimeSinceStartup;
        StartFrameControl(); // Bắt đầu hàm async
    }

    async void StartFrameControl()
    {
        await WaitForNextFrame(); // Gọi hàm async để điều chỉnh frame rate
    }

    // ✅ Chuyển sang async Task thay vì IEnumerator
    private async Task WaitForNextFrame()
    {
        while (true)
        {
            await Task.Yield(); // Đợi frame hiện tại hoàn tất
            currentFrameTime += 1.0f / TargetFrameRate;

            // Tính thời gian còn lại cho frame tiếp theo
            float t = Time.realtimeSinceStartup;
            float sleepTime = (currentFrameTime - t) * 1000.0f;

            // Nếu còn thời gian, dùng Task.Delay để tránh chiếm CPU
            if (sleepTime > 1.0f)  // Nếu sleepTime nhỏ hơn 1ms, bỏ qua Delay
            {
                await Task.Delay((int)sleepTime);
            }

            // Đảm bảo không vượt quá thời gian mong muốn của frame tiếp theo
            while (Time.realtimeSinceStartup < currentFrameTime)
            {
                await Task.Yield();  // Giữ cho CPU nhàn rỗi trong thời gian ngắn
            }
        }
    }
}
