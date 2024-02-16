using Microsoft.AspNetCore.Http;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}