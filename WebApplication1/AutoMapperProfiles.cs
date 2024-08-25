using WebApplication1.DTOs;
using WebApplication1.Models;
using AutoMapper;

namespace WebApplication1
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<Producto, ProductoInDTO>();
            CreateMap<ProductoInDTO, Producto>();
        }
    }
}
