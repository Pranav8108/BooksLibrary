using Bulkyy.DataAccess.Data;
using Bulkyy.DataAccess.Repository.IRepository;
using Bulkyy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulkyy.DataAccess.Repository
{
	public class ProductRepository :Repository<Product>, IProductRepository
	{
		private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
				_db = db;
        }

      

		public void Update(Product obj)
		{
			var objFromDb = _db.Products.FirstOrDefault(s => s.Id == obj.Id);
			if (objFromDb != null)
			{
				objFromDb.Title = obj.Title;
				objFromDb.Description = obj.Description;
				objFromDb.Price = obj.Price;
				objFromDb.CategoryId = obj.CategoryId;
				objFromDb.ISBN = obj.ISBN;
				objFromDb.Author = obj.Author;
				objFromDb.ListPrice = obj.ListPrice;
				objFromDb.Price100 = obj.Price100;
				objFromDb.Price50 = obj.Price50;
				if (obj.ImageUrl != null)
				{
					objFromDb.ImageUrl = obj.ImageUrl;
				}
				
			}
		}
	}
}
