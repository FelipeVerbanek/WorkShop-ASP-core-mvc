﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Services;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModel;
using SalesWebMVC.Services.Exceptions;
using System.Diagnostics;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {

		private readonly SellerService _sellerService;
		private readonly DepartmentService _departmentService;

		public SellersController(SellerService sellerService, DepartmentService departmentService)
		{
			_sellerService = sellerService;
			_departmentService = departmentService;
		}

        public IActionResult Index()
        {
			var list = _sellerService.FindAll();

            return View(list);

        }

		public IActionResult Create()
		{
			var departments = _departmentService.FieldAll();
			var viewmodel = new SellerFormViewModel { Departments = departments };
			return View(viewmodel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Seller seller)
		{
			_sellerService.Insert(seller);

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not provided"});

			}
			var obj = _sellerService.FindById(id.Value);

			if (obj == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id Not Found" });
			}

			return View(obj);

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(int id)
		{
			_sellerService.Remove(id);

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				 return RedirectToAction(nameof(Error), new { message = "Id Not provided" });
			}

			var obj = _sellerService.FindById(id.Value);

			if (obj == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id Not found" });
			}

			return View(obj);
		}

		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return  RedirectToAction(nameof(Error), new { message = "Id Not provided" });

			}
			var obj = _sellerService.FindById(id.Value);
			if (obj == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id Not found" });
			}
			List<Department> department = _departmentService.FieldAll();
			SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = department };
			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(int id, Seller seller)
		{
			if(id != seller.Id)
			{
				return RedirectToAction(nameof(Error), new { message = "Id missmatch " });
			}
			try
			{
				_sellerService.Update(seller);
				return RedirectToAction(nameof(Index));
			}
			catch (NotFoundException e)
			{
				return NotFound();
			}
			catch (DbConcurrencyException e)
			{
				return BadRequest();
			}
		}
		public IActionResult Error(string message)
		{
			var viewModel = new ErrorViewModel
			{
				Message = message,
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
			};

			return View(viewModel);
		}
	}
}