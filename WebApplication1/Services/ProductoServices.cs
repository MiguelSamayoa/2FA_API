using WebApplication1.DTOs;
using WebApplication1.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Services
{
    public interface IProductServices 
    {
        public Task<List<Producto>> GetProductos();
        public Task<Producto> GetProductos( int id );
        public Task<Producto> SubtractStock( int id, int Subtract);
        public Task<Producto> AddStock(int id, int Add);
        public Task<Producto> DeleteProducto(int Id);
        public Task<Producto> CreateProducto(ProductoInDTO producto);
    }

    public class ProductoServices : IProductServices
    {
        private readonly JwtContext context;
        private readonly IMapper mapper;

        //Contructor
        public ProductoServices(JwtContext Context, IMapper mapper)
        {
            context = Context;
            this.mapper = mapper;
        }

        
        public async Task<Producto> DeleteProducto(int Id)
        {

            //Verificar que el producto exista
            Producto producto = await context.Productos.Where( x => x.Id == Id ).FirstOrDefaultAsync();

            if (producto == null) throw new Exception("No se encontraron productos con ese ID");

            context.Remove(producto);
            context.SaveChanges();

            return producto;
        }


        public async Task<List<Producto>> GetProductos()
        {
            List<Producto> products = await context.Productos.ToListAsync();

            if (products.Count() == 0 || products == null) throw new Exception("No se encontro ningun producto");

            return products;
        }

        public async Task<Producto> GetProductos(int id)
        {
            Producto products = await context.Productos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (products == null) throw new Exception("No se encontraron productos con ese ID");

            return products;
        }

        public async Task<Producto> SubtractStock(int id, int Subtract)
        {
            Producto product = await context.Productos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (product == null) throw new Exception("No se encontraron productos con ese ID");

            if (product.Stock < Subtract) throw new Exception("La canttidad supera el Stock actual");

            context.SaveChanges();


            return product;
        }
        
        public async Task<Producto> AddStock(int id, int Add)
        {
            Producto product = await context.Productos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (product == null) throw new Exception("No se encontraron productos con ese ID");

            product.Stock += Add;

            context.SaveChanges();

            product = await context.Productos.Where(x => x.Id == id).FirstOrDefaultAsync();

            return product;
        }

        public async Task<Producto> CreateProducto(ProductoInDTO producto)
        {
            Producto p = mapper.Map<Producto>(producto);

            await context.Productos.AddAsync(p);
            await context.SaveChangesAsync();

            return p;

        }

        public Task<Producto> DeleteProducto(int Id, int asd)
        {
            throw new NotImplementedException();
        }
    }
}
