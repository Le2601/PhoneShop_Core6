using PhoneShop.Data;
using PhoneShop.Models;
using Twilio.Http;

namespace PhoneShop.DI.DI_User.PaymentResponses
{
    public class PaymentResponses_Repository : IPaymentResponse_Repository
    {
        private readonly ShopPhoneDbContext _context;

        public PaymentResponses_Repository(ShopPhoneDbContext context)
        {
            _context = context;
        }
        public void Create(PaymentResponeData response)
        {
            if(response.Success == true)
            {
                var item = new PaymentResponse
                {
                    OrderDescription = response.OrderDescription,
                    TransactionId = response.TransactionId,
                    OrderId = response.OrderId,
                    PaymentMethod = response.PaymentMethod,
                    PaymentId = response.PaymentId,
                    Success = response.Success,
                    Token = response.Token,
                    VnPayResponseCode = response.VnPayResponseCode
                };

                _context.paymentResponses.Add(item);
                _context.SaveChanges();
            }


            

        }
    }
}
