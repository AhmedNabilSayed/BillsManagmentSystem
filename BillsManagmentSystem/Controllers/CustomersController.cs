using AutoMapper;
using BillsBLL.IReposatories;
using BillsBLL.Specifications.CustomerSpecifications;
using BillsEntity;
using BillsManagmentSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BillsManagmentSystem.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: CustomerController
        public async Task<ActionResult> Index(string searchByName)
        {
            if (!string.IsNullOrEmpty(searchByName))
            {
                var spec = new CustomerWithSpec(searchByName);
                var customers = await _unitOfWork.CustomerRepository.GetAllWithSpecAsync(spec);
                var mappedCustomers = _mapper.Map<IEnumerable<CustomerViewModel>>(customers);
                return View(mappedCustomers);
            }
            else
            {
                var customers = await _unitOfWork.CustomerRepository.GetAllAsync();
                var mappedCustomers = _mapper.Map<IEnumerable<CustomerViewModel>>(customers);
                return View(mappedCustomers);
            }


        }

        // GET: CustomerController/Details/5
        //     public async Task<ActionResult> Details(int id)
        //     {
        //         try
        //         {
        //             if (id == 0)
        //                 return NotFound();
        //	var item = await _unitOfWork.CustomersRepository.GetByIdAsync(id);
        //             if(item == null)
        //                 return NotFound();
        //	var mappedCustomer = _mapper.Map<CustomerViewModel>(item);

        //	return View(mappedCustomer);
        //}
        //catch (Exception ex)
        //         {
        //             return RedirectToAction("Error", "Home");
        //         }
        //     }

        // GET: CustomerController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: CustomerController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(CustomerViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedCustomer = _mapper.Map<Customer>(model);
                    _unitOfWork.CustomerRepository.Add(mappedCustomer);
                    return RedirectToAction(nameof(Index));

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }

        // GET: CustomerController/Edit/5
        //      public async Task<ActionResult> Edit(int id)
        //      {
        //	try
        //	{
        //		if (id == 0)
        //			return NotFound();
        //		var item = await _unitOfWork.CustomersRepository.GetByIdAsync(id);
        //		if (item == null)
        //			return NotFound();
        //		var mappedCustomer = _mapper.Map<CustomerViewModel>(item);

        //		return View(mappedCustomer);
        //	}
        //	catch (Exception ex)
        //	{
        //		return RedirectToAction("Error", "Home");
        //	}
        //}

        // POST: CustomerController/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(int id, CustomerViewModel model)
        {
            if (id != model.CustomerId)
                return null;
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedCustomer = _mapper.Map<Customer>(model);
                    await _unitOfWork.CustomerRepository.UpdateAsync(mappedCustomer);
                    return Json(mappedCustomer);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return null;
        }

        // GET: CustomerController/Delete/5
        //      public async Task<ActionResult> Delete(int id)
        //      {
        //	try
        //	{
        //		if (id == 0)
        //			return NotFound();
        //		var item = await _unitOfWork.CustomersRepository.GetByIdAsync(id);
        //		if (item == null)
        //			return NotFound();
        //		var mappedCustomer = _mapper.Map<CustomerViewModel>(item);

        //		return View(mappedCustomer);
        //	}
        //	catch (Exception ex)
        //	{
        //		return RedirectToAction("Error", "Home");
        //	}
        //}

        // POST: CustomerController/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int CustomerId)
        {
            //if (id != model.ItmCod)
            //	return NotFound();
            try
            {
                if (ModelState.IsValid)
                {
                    var Customer = await _unitOfWork.CustomerRepository.GetByIdAsync(CustomerId);
                    _unitOfWork.CustomerRepository.Delete(Customer);
                    return RedirectToAction(nameof(Index));
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
