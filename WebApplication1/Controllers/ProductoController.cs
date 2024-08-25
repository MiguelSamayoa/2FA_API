using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProductoController : ControllerBase
    {
        private readonly IProductServices iproductoServices;
        private readonly IMapper mapper;

        public ProductoController( IProductServices IproductoServices, IMapper mapper )
        {
            iproductoServices = IproductoServices;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<Producto>>> GetProdcutos()
        {
            List<Producto> products = await iproductoServices.GetProductos();

            if (products == null) return BadRequest("No se encontraron productos");
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProdcutos(int id)
        {
            Producto products = await iproductoServices.GetProductos( id );

            if (products == null) return BadRequest("No se encontro el producto");

            return Ok(products);
        }

        [HttpPost("AddStock")]
        public async Task<ActionResult<Producto>> AddStock(ChangeStockDTO ChangeStock)
        {
            Producto producto = await iproductoServices.AddStock(ChangeStock.Id, ChangeStock.NewStock);

            if (producto == null) return BadRequest("No se encotro el producto");

            return Ok(producto);
        }

        [HttpPost("SubtractStock")]
        public async Task<ActionResult<Producto>> SubtractStock(ChangeStockDTO ChangeStock)
        {
            Producto producto = await iproductoServices.SubtractStock(ChangeStock.Id, ChangeStock.NewStock);

            if (producto == null) return BadRequest("No se encotro el producto");

            return Ok(producto);
        }

        [HttpPut]
        public async Task<ActionResult<Producto>> CreateProduct( [FromBody ]ProductoInDTO producto )
        {
            if (producto == null) return BadRequest("ingrese un producto");

            Producto p = await iproductoServices.CreateProducto( producto );
            if (p == null) return BadRequest(p);

            return Ok(p);
        }

        [HttpPut("List")]
        public async Task<ActionResult<Producto>> CreateProduct([FromBody] List<ProductoInDTO> producto)
        {
            if (producto == null) return BadRequest("ingrese un producto");


            foreach( ProductoInDTO p in producto)
            {
                await iproductoServices.CreateProducto(p);
                if (p == null) return BadRequest(p);
            }
                

            return Ok(producto);
        }
    }
}
