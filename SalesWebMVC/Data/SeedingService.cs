using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMVC.Models;
using SalesWebMVC.Models.Enums;

namespace SalesWebMVC.Data
{
	public class SeedingService
	{
		private SalesWebMVCContext _context;

		public SeedingService(SalesWebMVCContext context)
		{
			_context = context;
		}

		public void Seed()
		{
			if (_context.Department.Any() || _context.Seller.Any() || _context.salesRecord.Any())
			{
				return; //DB já está copulado
			}

			Department d1 = new Department(1, "Computers");
			Department d2 = new Department(2, "Electronics");
			Department d3 = new Department(3, "Fashion");
			Department d4 = new Department(4, "Books");

			Seller s1 = new Seller(1, "Bob Brown", "bob@gmail.com", new DateTime(1998, 4, 21), 10000, d1);
			Seller s2 = new Seller(2, "Bob 2", "teste@gmail.com", new DateTime(1998, 4, 21), 10000, d2);
			Seller s3 = new Seller(3, "Bob Teste", "teste@gmail.com", new DateTime(1998, 4, 21), 2000, d3);

			SalesRecord r1 = new SalesRecord(1, new DateTime(1998, 3, 5), 11000, SalerStatus.Billed, s1);
			SalesRecord r2 = new SalesRecord(2, new DateTime(1996, 3, 5), 12000, SalerStatus.Billed, s2);
			SalesRecord r3 = new SalesRecord(3, new DateTime(1994, 3, 5), 11000, SalerStatus.Billed, s3);

			_context.Department.AddRange(d1,d2,d3,d4);
			_context.Seller.AddRange(s1, s2, s3);
			_context.salesRecord.AddRange(r1, r2, r3);

			_context.SaveChanges(); //Commit
		}
	}
}
