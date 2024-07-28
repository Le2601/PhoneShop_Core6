using PhoneShop.Services.MoMo.model.momo;
using PhoneShop.Services.MoMo.Model.Order;
namespace PhoneShop.Services.MoMo.Services;

    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }

