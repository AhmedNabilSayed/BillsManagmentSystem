using AutoMapper;
using BillsEntity;
using BillsManagmentSystem.Helper;
using BillsManagmentSystem.ViewModels;
using System.Globalization;

namespace BillsManagmentSystem.Mapped
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<BillViewModel, BillHeader>()
                .ForMember(d => d.VNDCOD, o => o.MapFrom(s => s.VndCod))
                .ForMember(d => d.BILCOD, o => o.MapFrom(s => s.InvoiceNo))
                .ForMember(d => d.BILDAT, o => o.MapFrom(s => s.BILDAT));




            CreateMap<BillDetails, BillDetailsViewModel>().ReverseMap();
            CreateMap<SalesBillDetails, SalesBillDetailsViewModel>().ReverseMap();


			CreateMap<BillHeader, BillHeaderViewModel>()
			.ForMember(dest => dest.BILDAT, opt => opt.MapFrom(src => src.BILDAT.ToString("yyyy-MM-dd HH:mm:ss")));

			CreateMap<BillHeaderViewModel, BillHeader>()
	        .ForMember(dest => dest.BILDAT,opt => opt.MapFrom(src => DateTime.ParseExact(src.BILDAT, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)));

			CreateMap<SalesBillHeader, SalesBillHeaderViewModel>()
			.ForMember(dest => dest.BILDAT, opt => opt.MapFrom(src => src.BILDAT.ToString("yyyy-MM-dd HH:mm:ss")));

			CreateMap<SalesBillHeaderViewModel, SalesBillHeader>()
			.ForMember(dest => dest.BILDAT, opt => opt.MapFrom(src => DateTime.ParseExact(src.BILDAT, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)));
			CreateMap<Items, ItemViewModel>().ReverseMap();
            CreateMap<Vendor, VendorViewModel>().ReverseMap();
            CreateMap<Stores, StoreViewModel>().ReverseMap();
            CreateMap<Customer, CustomerViewModel>().ReverseMap();
            CreateMap<Stock, StockViewModel>()
                .ForMember(d => d.ItemName , o => o.MapFrom(m => m.Item.ItmNam)) 
                .ForMember(d => d.StoreName , o => o.MapFrom(m => m.Store.Name)) ;


		}
	}
}
