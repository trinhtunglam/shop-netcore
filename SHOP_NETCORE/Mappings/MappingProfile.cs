using BUSINESS_OBJECTS;
using SHOP_NETCORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.Mappings
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductViewModel, Product>().ReverseMap();
            CreateMap<SupplierViewModel, Supplier>().ReverseMap();
            CreateMap<ProductCategoryViewModel, ProductCategory>().ReverseMap();
            CreateMap<ProducerViewModel, Producer>().ReverseMap();
            CreateMap<ReceiptNoteViewModel, ReceiptNote>().ReverseMap();
            CreateMap<ReceiptNoteDetailViewModel, ReceiptNoteDetail>().ReverseMap();
            CreateMap<OrderViewModel, Order>().ReverseMap();
            CreateMap<OrderDetailViewModel, OrderDetail>().ReverseMap();
            CreateMap<MenuViewModel, Menu>().ReverseMap();
            CreateMap<InfomationViewModel, Infomation>().ReverseMap();
            CreateMap<CustomerViewModel, Customer>().ReverseMap();
            CreateMap<SlideViewModel, Slide>().ReverseMap();
        }
    }
}
