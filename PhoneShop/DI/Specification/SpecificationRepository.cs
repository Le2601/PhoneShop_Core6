using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using Stripe;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Linq;


namespace PhoneShop.DI.Specification
{
    public class SpecificationRepository : ISpecificationRepository
    {
        private readonly ShopPhoneDbContext _context;
        public SpecificationRepository(ShopPhoneDbContext context)
        {

            _context = context;

        }
        public List<SpecificationsViewModel> GetSpecificationByIdProduct(int id)
        {


            var item = _context.specifications.Where(x => x.ProductId == id).Select(x => new SpecificationsViewModel
            {

                ProductId = x.ProductId,
                Display = x.Display,
                Model = x.Model,
                OperatingSystem = x.OperatingSystem,
                Processor = x.Processor,
                InternalStorage = x.InternalStorage,
                Camera = x.Camera,
                RandomAccessMemory = x.RandomAccessMemory,
                Battery = x.Battery,
                WaterResistance = x.WaterResistance,
                DimensionsAndeight = x.DimensionsAndeight,
                Color = x.Color,
                Connectivity = x.Connectivity,
                Id = x.Id,

            }).ToList();


            return item;

        }
        public void CreateSpecifications(SpecificationsData model)
        {
            var item = new PhoneShop.Models.specifications
            {

                ProductId = model.ProductId,
                Display = model.Display,
                Model = model.Model,
                OperatingSystem = model.OperatingSystem,
                Processor = model.Processor,
                InternalStorage = model.InternalStorage,
                Camera = model.Camera,
                RandomAccessMemory = model.RandomAccessMemory,
                Battery = model.Battery,
                WaterResistance = model.WaterResistance,
                DimensionsAndeight = model.DimensionsAndeight,
                Color = model.Color,
                Connectivity = model.Connectivity,
                Id = model.Id,


            };
            _context.specifications.Add(item);
            _context.SaveChanges();
        }

        public void UpdateSpecificationByIdProduct(int IdProduct, SpecificationsData model)
        {
            var item = _context.specifications.Where(x => x.ProductId == IdProduct).FirstOrDefault();




            item.Display = model.Display;
            item.Model = model.Model;
            item.OperatingSystem = model.OperatingSystem;
            item.Processor = model.Processor;
            item.InternalStorage = model.InternalStorage;
            item.Camera = model.Camera;
            item.RandomAccessMemory = model.RandomAccessMemory;
            item.Battery = model.Battery;
            item.WaterResistance = model.WaterResistance;
            item.DimensionsAndeight = model.DimensionsAndeight;
            item.Color = model.Color;
            item.Connectivity = model.Connectivity;


            _context.SaveChanges();


        }
    }
}
