using OrderMgmt.DataAccess;
using OrderMgmt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMgmt.Business
{
    public class ProductService
    {
        private readonly ProductRepository _repo;

        public ProductService(ProductRepository repo)
        {
            _repo = repo;
        }


        // Tüm ürünleri getir
        public List<Product> GetAllProducts()
        {
            return _repo.GetAll();
        }

        // Yeni ürün ekle
        public void AddProduct(Product product)
        {
            // StockCode boş olamaz
            if (string.IsNullOrWhiteSpace(product.StockCode))
            {
                throw new ArgumentException("StockCode boş olamaz!");
            }

            // Fiyat sıfırdan büyük olmalı
            if (product.Price <= 0)
            {
                throw new ArgumentException("Fiyat sıfırdan büyük olmalı!");
            }

            _repo.Add(product);
        }

        // Ürün güncelle
        public void UpdateProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.StockCode))
            {
                throw new ArgumentException("StockCode boş olamaz!");
            }

            if (product.Price <= 0)
            {
                throw new ArgumentException("Fiyat sıfırdan büyük olmalı!");
            }

            _repo.Update(product);
        }

        // Ürün sil
        public void DeleteProduct(int id)
        {
            _repo.Delete(id);
        }

        // Tek ürün getir
        public Product GetProductById(int id)
        {
            return _repo.GetById(id);
        }
    }
}

