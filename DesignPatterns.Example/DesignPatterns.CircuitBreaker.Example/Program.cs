// Su dung thu vien
#region Polly package
//using System;
//using System.Threading;
//using Polly;
//using Polly.CircuitBreaker;

//namespace DesignPatterns.CircuitBreaker.Example
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {

//            Console.OutputEncoding = System.Text.Encoding.UTF8;

//            var circuitBreakerPolicy = Policy
//            .Handle<Exception>()
//            .CircuitBreaker(3, TimeSpan.FromSeconds(3),
//                onBreak: (exception, duration) =>
//                {
//                    Console.WriteLine($"Circuit opened! Chờ {duration.TotalSeconds} giây...");
//                },
//                onReset: () => Console.WriteLine("Circuit closed!"),
//                onHalfOpen: () => Console.WriteLine("Circuit is half-open, thử lại yêu cầu..."));

//            for (int i = 0; i < 20; i++)
//            {
//                try
//                {
//                    // Sử dụng circuit breaker để gọi hàm giả lập dịch vụ
//                    circuitBreakerPolicy.Execute(() => SimulateServiceCall());
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Yêu cầu thất bại: {ex.Message}");
//                }

//                Thread.Sleep(1000); // Chờ 1 giây giữa các yêu cầu
//            }
//        }

//        static void SimulateServiceCall()
//        {
//            // Giả lập lỗi ngẫu nhiên để mô phỏng tình trạng dịch vụ không ổn định
//            Random random = new Random();
//            if (random.Next(1, 4) != 1)  // 75% khả năng lỗi
//            {
//                throw new Exception("Lỗi dịch vụ!");
//            }

//            Console.WriteLine("Yêu cầu thành công.");
//        }
//    }
//} 
#endregion

// Cai dat Thu cong
#region Thu Cong
using System;
using System.Threading;

namespace DesignPatterns.CircuitBreaker.ManualExample
{
    public enum CircuitBreakerState
    {
        Closed,
        Open,
        HalfOpen
    }

    public class CircuitBreaker
    {
        private CircuitBreakerState state = CircuitBreakerState.Closed;
        private int failureCount = 0;
        private readonly int failureThreshold = 3;
        private readonly TimeSpan openTimeout = TimeSpan.FromSeconds(3);
        private DateTime lastFailureTime;

        public void Execute(Action action)
        {
            if (state == CircuitBreakerState.Open)
            {
                if (DateTime.Now - lastFailureTime > openTimeout)
                {
                    state = CircuitBreakerState.HalfOpen;
                    Console.WriteLine("Circuit chuyển sang trạng thái Half-Open.");
                }
                else
                {
                    Console.WriteLine("Circuit đang ở trạng thái Open. Yêu cầu bị từ chối.");
                    return;
                }
            }

            try
            {
                action();

                if (state == CircuitBreakerState.HalfOpen)
                {zs
                    state = CircuitBreakerState.Closed;
                    Console.WriteLine("Circuit chuyển sang trạng thái Closed.");
                }

                failureCount = 0; // Reset số lần thất bại nếu thành công
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Yêu cầu thất bại: {ex.Message}");
                failureCount++;

                if (failureCount >= failureThreshold)
                {
                    state = CircuitBreakerState.Open;
                    lastFailureTime = DateTime.Now;
                    Console.WriteLine($"Circuit chuyển sang trạng thái Open. Chờ {openTimeout.TotalSeconds} giây.");
                }
            }
        }
    }

    public class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var circuitBreaker = new CircuitBreaker();

            for (int i = 0; i < 20; i++)
            {
                circuitBreaker.Execute(() => SimulateServiceCall());
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

#endregion
