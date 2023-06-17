﻿
using AutoMapper;
using Application.Commons;
using Domain.Entities;
using Application.ViewModels;
using Application.ViewModels.Product;
using Application.ViewModels.Product.ProductImage;
using Application.ViewModels.SupplierDTO;
using Application.ViewModels.CategoryDTO;
using Application.ViewModels.TourGuideDTO;
using Application.ViewModels.GroupDTOs;
using Application.ViewModels.MenuDTOs;
using Application.ViewModels.ProductInMenuDTOs;
using Application.ViewModels.CartDTO;

namespace Infrastructures.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            #region UserMapping
            CreateMap<User, UserViewDTO>().ReverseMap();
            #endregion

            #region ProductMapping
            CreateMap<CreateProductDTO, Product>().ReverseMap();
            CreateMap<ViewProductDTO, Product>()
                .ForMember(v => v.Images, r => r.MapFrom(x => x.Images))
                .ForMember(v => v.Supplier, r => r.MapFrom(x => x.Supplier))
                .ForMember(v => v.Category, r => r.MapFrom(x => x.Category))
                .ReverseMap();
            #endregion

            #region ProductImageMapping
            CreateMap<ProductImageViewDTO, ProductImage>().ReverseMap();
            CreateMap<ProductImageMenuViewDTO, ProductImage>().ReverseMap();
            CreateMap<ImageViewDTO, ProductImage>().ReverseMap();
            #endregion

            #region SupplierMapping
            CreateMap<Supplier, SupplierViewDTO>().ReverseMap();
            CreateMap<SupplierCreateDTO, Supplier>().ReverseMap();
            CreateMap<SupplierUpdateDTO, Supplier>()
                .ForMember(des=>des.SupplierName,opt=>opt.MapFrom(src=>src.SupplierName))
                .ForMember(des => des.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(des => des.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ReverseMap();
            #endregion

            #region Categories
            CreateMap<Category, CategoryCreateDTO>().ReverseMap();
            CreateMap<CategoryViewDTO, Category>().ReverseMap();
            CreateMap<CategoryCreateDTO, Category>().ReverseMap();
            #endregion

            #region TourGuides 
            CreateMap<TourGuide, TourGuideViewDTO>().ReverseMap();
            CreateMap<TourGuide,TourGuideCreateDTO>().ReverseMap();
            CreateMap<TourGuide,TourGuideUpdateDTO>().ReverseMap();
            #endregion

            #region GroupMapping
            CreateMap<Group,GroupViewDTO>().ReverseMap();
            CreateMap<Group, GroupCreateDTO>().ReverseMap();
            CreateMap<Group, GroupUpdateDTO>().ReverseMap();
            #endregion

            #region MenuMapping
            CreateMap<Menu, MenuViewDTO>()
                .ForMember(des => des.Id, opt => opt.MapFrom(opt => opt.Id))
                .ForMember(des => des.TourGuideId, opt => opt.MapFrom(opt => opt.TourGuideId))
                .ForMember(des => des.Quantity, opt => opt.MapFrom(opt => opt.Quantity))
                .ForMember(des => des.Status, opt => opt.MapFrom(opt => opt.Status))
                .ReverseMap();
            CreateMap<Menu, MenuCreateDTO>().ReverseMap();
            #endregion

            #region ProductInMenu
            CreateMap<ProductInMenu,ProductMenuViewDTO>()
                .ForMember(v => v.MenuId, r => r.MapFrom(x => x.MenuId))
                .ForMember(v => v.ActualPrice, r => r.MapFrom(x => x.ActualPrice))
                .ForMember(v => v.SupplierPrice, r => r.MapFrom(x => x.SupplierPrice))
                .ForMember(v=>v.Description,r=>r.MapFrom(x => x.Description))
                .ReverseMap();

            CreateMap<ProductMenuViewDTO, Product>()
                .ForPath(v => v.ProductName, r => r.MapFrom(x => x.ProductName))
                .ForPath(v => v.Description, r => r.MapFrom(x => x.Description))
                .ForPath(v => v.Category.CategoryName, r => r.MapFrom(x => x.CategoryName))
                .ForPath(v => v.Supplier.SupplierName, r => r.MapFrom(x => x.SupplierName))
                .ForPath(v => v.Supplier.Address, r => r.MapFrom(x => x.Address))
                .ForPath(v => v.Supplier.Id, r => r.MapFrom(x => x.SupplierId))
                .ReverseMap();


            CreateMap<ProductInMenu,ProductMenuCreateDTO>()
                .ForMember(v => v.ProductId, r => r.MapFrom(x => x.ProductId))
                .ForMember(v => v.ResellPrice, r => r.MapFrom(x => x.ActualPrice))
                .ForMember(v => v.ResellPrice, r => r.MapFrom(x => x.SupplierPrice))
                .ReverseMap();
            #endregion

            #region CartItemMapping
            CreateMap<ItemViewDTO, Product>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ProductName))
                .ForMember(d=>d.Description,o=>o.MapFrom(s => s.Description))
                .ForPath(d => d.Supplier.SupplierName, o => o.MapFrom(s => s.SupplierName))
                .ForPath(d => d.Supplier.Id, o => o.MapFrom(s => s.Id))
                .ForPath(d => d.Category.CategoryName, o => o.MapFrom(s => s.CategoryName))
                .ForMember(d => d.ResellPrice, o => o.MapFrom(s => s.SuppleirPrice))
                .ReverseMap();

            #endregion
        }
    }
}
