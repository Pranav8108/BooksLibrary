﻿using System.Linq.Expressions;

namespace Bulkyy.DataAccess.Repository.IRepository
{
	 public interface IRepository<T> where T: class 
	{
		//T-category
		T Get(Expression<Func<T,bool>> filter, string? includeProperties = null);
		IEnumerable<T> GetAll(string? includeProperties = null);
		
		void Add(T entity);
		
		void Remove(T entity);
		void RemoveRange(IEnumerable<T> entity);
	}
}
