namespace be_movie_booking.Infrastructure.Interfaces.Service
{
    public interface IVnpayService
    {
        string CreatePaymentUrl(double money, int description, string ipAddress);
    }
}
