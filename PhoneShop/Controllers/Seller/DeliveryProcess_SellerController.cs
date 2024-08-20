using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.DI.DeliveryProcess;
using PhoneShop.DI.DI_User.Evaluate_Product_User;
using PhoneShop.DI.DI_User.Product_User;
using PhoneShop.DI.Order;
using PhoneShop.Models;

namespace PhoneShop.Controllers.Seller
{
    [Authorize(Roles = "Seller")]
    public class DeliveryProcess_SellerController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly IDeliveryProcessRepository _deliveryProcessRepository;
        private readonly IEvaluate_ProductRepository _evaluate_ProductRepository;
        private readonly IProduct_UserRepository _productRepository;
        public DeliveryProcess_SellerController(ShopPhoneDbContext shopPhoneDbContext, IOrderRepository orderRepository,
            IDeliveryProcessRepository deliveryProcessRepository, IEvaluate_ProductRepository evaluate_ProductRepository
            , IProduct_UserRepository productRepository)
        {
            _context = shopPhoneDbContext;

            _orderRepository = orderRepository;
            _deliveryProcessRepository = deliveryProcessRepository;
            _evaluate_ProductRepository = evaluate_ProductRepository;
            _productRepository = productRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Info_Order_Address() {

            
            
            
            
            
            
            return View(); }
        //Update_DeliveryProcess_Order_COD

       
        public IActionResult Update_DeliveryProcess_Order_COD(int Id)
        {
            var Check_DeliveryProcesses = _context.DeliveryProcesses.Where(x=> x.Order_Detail_Id == Id).FirstOrDefault();
            var Check_Order = _context.Order_Details.Where(x=> x.Id == Id).FirstOrDefault()!;
            ViewBag.Address_Order = Check_Order.Address;

            //neu da ton tai qua trinh giao hang roi thi hien thi ra

            if (Check_DeliveryProcesses != null)
            {

                ViewBag.item_DeliveryProcesses = Check_DeliveryProcesses;

                ViewBag.DeliveryStatus = Check_DeliveryProcesses.DeliveryStatus;



                ViewBag.Comfirm_item = 1;


            }
            else
            {
                ViewBag.item_DeliveryProcesses = null;
               
                ViewBag.Comfirm_item = 0;

            }
            




            return View(Check_Order);
        }

        [HttpPost]
        public async Task<IActionResult> Create_DeliveryProcess(IFormCollection form)
        {
            string DeliveryStatus = form["DeliveryStatus"];
            string Order_Detail_Id = form["Order_Detail_Id"];
            
            string Order_Id = form["Order_Id"];
            string DeliveryDate = form["DeliveryDate"];
            string DeliveryAddress = form["DeliveryAddress"];
            //check DeliveryProcess co ton tai hay ko    
            var Check_DeliveryProcess_Order_Detail = _context.DeliveryProcesses.Where(x => x.Order_Detail_Id == int.Parse(Order_Detail_Id)).FirstOrDefault();

            if (Check_DeliveryProcess_Order_Detail != null)
            {
                //neu xac nhan giao hang thanh cong
                if (int.Parse(DeliveryStatus) == 4)
                {
                    //add Evaluate or update
                    var GetOrdInDeli = _context.DeliveryProcesses.Where(x => x.Order_Detail_Id == int.Parse(Order_Detail_Id))
                        .Include(x => x.Order_Details).ThenInclude(x => x.Order).FirstOrDefault()!;
                    _evaluate_ProductRepository.Check_Evaluate_Insert_Db(GetOrdInDeli.Order_Details.ProductId, GetOrdInDeli.Order_Details.Quantity, GetOrdInDeli.Order_Details.Order.AccountId);

                    var Update_DeliveryProcess = new DeliveryProcessData
                    {
                        DeliveryDate = DateTime.Parse(DeliveryDate),
                        DeliveryStatus = int.Parse(DeliveryStatus),
                        DeliveryAddress = DeliveryAddress,
                    };


                    


                    _deliveryProcessRepository.Update(Update_DeliveryProcess, int.Parse(Order_Detail_Id));

                    //giam san pham trong kho
                    _productRepository.Reduced_In_Stock(GetOrdInDeli.Order_Details.ProductId, GetOrdInDeli.Order_Details.Quantity);

                }
                else
                {
                    var Update_DeliveryProcess = new DeliveryProcessData
                    {
                        DeliveryDate = DateTime.Parse(DeliveryDate),
                        DeliveryStatus = int.Parse(DeliveryStatus),
                        DeliveryAddress = DeliveryAddress,
                    };


                    _deliveryProcessRepository.Update(Update_DeliveryProcess, int.Parse(Order_Detail_Id));
                }
            }
            else
            {
                var Create_Item = new DeliveryProcessData
                {
                    DeliveryStatus = int.Parse(DeliveryStatus),
                    Order_Id = Order_Id,
                    Order_Detail_Id = int.Parse(Order_Detail_Id),
                    DeliveryDate = DateTime.Parse(DeliveryDate),
                    DeliveryAddress = DeliveryAddress,


                };
                _deliveryProcessRepository.Create(Create_Item);

            }




            return RedirectToAction("Info_Order_Address", "Order_Seller",new {id = int.Parse(Order_Detail_Id) });

        }
    }
}
