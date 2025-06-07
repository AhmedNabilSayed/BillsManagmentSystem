using AutoMapper;
using BillsBLL.IReposatories;
using BillsBLL.Specifications.BillSpecifications;
using BillsBLL.Specifications.VendorSpecifications;
using BillsEntity;
using BillsManagmentSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BillsManagmentSystem.Controllers
{
	[Authorize]
	public class VendorsController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public VendorsController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		// GET: VendorsController
		public async Task<ActionResult> Index(string? searchByName)
		{
			if (!string.IsNullOrWhiteSpace(searchByName))
			{
				var spec = new VendorWithSpec(searchByName);
                var vendors = await _unitOfWork.VendorRepository.GetAllWithSpecAsync(spec);
                var mappedVendors = _mapper.Map<IEnumerable<VendorViewModel>>(vendors);
                return View(mappedVendors);

            }
			else
			{
                var vendors = await _unitOfWork.VendorRepository.GetAllAsync();
                var mappedVendors = _mapper.Map<IEnumerable<VendorViewModel>>(vendors);
                return View(mappedVendors);
            }
			
		}



		[Authorize(Roles = "Admin")]
		public async Task<ActionResult> CreateVendor(VendorViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var spec = new VendorWithSpec(model.VndNam);
					var existVendor = await _unitOfWork.VendorRepository.GetAllWithSpecAsync(spec);	
					if(existVendor.Count() == 0)
					{
                        var mappedVendor = _mapper.Map<Vendor>(model);
                        _unitOfWork.VendorRepository.Add(mappedVendor);
                        return Json("not exist");
					}
					else
					{
						return Json("exist");
					}
					

				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View(model);
		}


		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<ActionResult> Edit( VendorViewModel model)
		{
			
			try
			{
				if (ModelState.IsValid)
				{
					var mappedVendor = _mapper.Map<Vendor>(model);
					await _unitOfWork.VendorRepository.UpdateAsync(mappedVendor);
					return RedirectToAction(nameof(Index));
				}

			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View(model);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<ActionResult> Delete(int VndCod)
		{
			try{
				if (ModelState.IsValid)
				{
					var billsVendor = await _unitOfWork.BillHeaderRepository.GetByVendor(VndCod);
					if (billsVendor.Count() <= 0)
					{
						var vendor = await _unitOfWork.VendorRepository.GetByIdAsync(VndCod);
                        _unitOfWork.VendorRepository.Delete(vendor);
                        return RedirectToAction(nameof(Index));
					}
					else
					{
						return Json("false");
					}
					
				}

			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View();
		}
	}
}
