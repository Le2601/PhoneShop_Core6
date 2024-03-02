using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PhoneShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneShop.Extension
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }

        public static List<VoucherItemModel> GetListSessionCartVoucher(string KeySession, HttpContext httpContext)
        {

            if (KeySession == null)
            {
                throw new ArgumentNullException(nameof(KeySession));
            }

            List<VoucherItemModel> CartVoucher = httpContext.Session.Get<List<VoucherItemModel>>(KeySession) ?? new List<VoucherItemModel>();

            return CartVoucher;


        }
        public static List<CartItemModel> GetListSessionCartItem(string KeySession, HttpContext httpContext)
        {

            if (KeySession == null)
            {
                throw new ArgumentNullException(nameof(KeySession));
            }

            List<CartItemModel> CartItems = httpContext.Session.Get<List<CartItemModel>>(KeySession) ?? new List<CartItemModel>();

            return CartItems;


        }


    }
}
