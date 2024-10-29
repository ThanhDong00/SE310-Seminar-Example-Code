using System;
using System.Threading;
using Polly;
using Polly.CircuitBreaker;

namespace DesignPatterns.CircuitBreaker.Example
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreaker(3, TimeSpan.FromSeconds(3),
                onBreak: (exception, duration) =>
                {
                    Console.WriteLine($"Circuit opened! Chờ {duration.TotalSeconds} giây...");
                },
                onReset: () => Console.WriteLine("Circuit closed!"),
                onHalfOpen: () => Console.WriteLine("\nCircuit is half-open, thử lại yêu cầu..."));

            for (int i = 0; i < 20; i++)
            {
                try
                {
                    // Sử dụng circuit breaker để gọi hàm giả lập dịch vụ
                    circuitBreakerPolicy.Execute(() => SimulateServiceCall());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Yêu cầu thất bại: {ex.Message}");
                }

                Thread.Sleep(1000); // Chờ 1 giây giữa các yêu cầu
            }
        }

        static void SimulateServiceCall()
        {
            // Giả lập lỗi ngẫu nhiên để mô phỏng tình trạng dịch vụ không ổn định
            Random random = new Random();
            if (random.Next(1, 4) != 1)  // 75% khả năng lỗi
            {
                throw new Exception("Lỗi dịch vụ!");
            }

            Console.WriteLine("Yêu cầu thành công.");
        }
    }
}
