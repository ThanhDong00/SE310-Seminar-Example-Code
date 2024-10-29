//namespace DesignPatterns.Adapter.Example
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.WriteLine("Hello, World!");
//        }
//    }
//}
using System;

namespace DesignPatterns.Adapter.Example
{
    // ITarget: Giao diện chuẩn mà hệ thống sử dụng
    public interface IPaymentProcessor
    {
        void Pay(decimal amount);
    }

    // Adaptee: Lớp của bên thứ ba (PayPal) với giao diện không tương thích
    public class PayPal
    {
        public void MakePayment(decimal amount)
        {
            Console.WriteLine($"Thanh toán {amount} bằng PayPal thành công.");
        }
    }

    // Adapter: Lớp chuyển đổi từ PayPal sang IPaymentProcessor
    public class PayPalAdapter : IPaymentProcessor
    {
        private readonly PayPal _payPal;

        public PayPalAdapter(PayPal payPal)
        {
            _payPal = payPal;
        }

        // Implement phương thức của IPaymentProcessor và chuyển đổi để sử dụng PayPal
        public void Pay(decimal amount)
        {
            // Sử dụng phương thức của PayPal
            _payPal.MakePayment(amount);
        }
    }

    // Client: Sử dụng hệ thống thanh toán chuẩn với giao diện IPaymentProcessor
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // Khởi tạo đối tượng PayPal của bên thứ ba
            PayPal payPal = new PayPal();

            // Tạo adapter để chuyển đổi PayPal thành IPaymentProcessor
            IPaymentProcessor paymentProcessor = new PayPalAdapter(payPal);

            // Client chỉ làm việc với giao diện IPaymentProcessor mà không quan tâm đến lớp PayPal bên dưới
            paymentProcessor.Pay(100.0m);
        }
    }
}