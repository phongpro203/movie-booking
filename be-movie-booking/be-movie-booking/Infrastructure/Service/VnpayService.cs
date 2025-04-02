using be_movie_booking.Infrastructure.Interfaces.Service;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using VNPAY.NET;
using VNPAY.NET.Utilities;

namespace be_movie_booking.Infrastructure.Service
{
    public class VnpayService : IVnpayService
    {
        private readonly IVnpay _vnpay;
        private readonly IConfiguration _configuration;

        public VnpayService(IVnpay vnpay, IConfiguration configuration)
        {
            _vnpay = vnpay;
            _configuration = configuration;
            _vnpay.Initialize(_configuration["Vnpay:TmnCode"], _configuration["Vnpay:HashSecret"], _configuration["Vnpay:BaseUrl"], _configuration["Vnpay:CallbackUrl"]);
        }

        public string CreatePaymentUrl(double money, int description, string ipAddress)
        {
            var request = new PaymentRequest
            {
                PaymentId = DateTime.Now.Ticks, // Sử dụng timestamp để tránh trùng lặp
                Money = money,
                Description = description.ToString(),
                IpAddress = ipAddress,
                BankCode = BankCode.ANY,
                CreatedDate = DateTime.Now,
                Currency = Currency.VND,
                Language = DisplayLanguage.Vietnamese
            };

            return _vnpay.GetPaymentUrl(request);
        }
    }
}
