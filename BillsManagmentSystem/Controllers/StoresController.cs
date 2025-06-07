using AutoMapper;
using BillsBLL.IReposatories;
using BillsBLL.Reposatories;
using BillsBLL.Specifications.StockSpecifications;
using BillsEntity;
using BillsManagmentSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BillsManagmentSystem.Controllers
{
    [Authorize]
    public class StoresController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StoresController(IUnitOfWork unitOfWork
                                , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: StoresController
        public async Task<ActionResult> Index()
        {
            var stores = await _unitOfWork.StoresRepository.GetAllAsync();
            var mappedStores = _mapper.Map<IEnumerable<StoreViewModel>>(stores);
            return View(mappedStores);
        }



		// POST: StoreController/Create
		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Create(StoreViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedStore = _mapper.Map<Stores>(model);
                    _unitOfWork.StoresRepository.Add(mappedStore);
                    return RedirectToAction(nameof(Index));

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }


		// POST: StoreController/Edit/5
		[Authorize(Roles = "Admin")]
		[HttpPost]
        public async Task<ActionResult> Edit(int id, StoreViewModel model)
        {
            if (id != model.Id)
                return NotFound();
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedStore = _mapper.Map<Stores>(model);
                    await _unitOfWork.StoresRepository.UpdateAsync(mappedStore);
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }


		// POST: StoreController/Delete/5
		[Authorize(Roles = "Admin")]
		[HttpPost]
        public async Task<ActionResult> Delete(int Id)
        {
            
            try
            {

                if (ModelState.IsValid)
                {
                    var spec = new StockWithSpec(storeId: Id);
                    var billsStore = await _unitOfWork.StockRepository.GetAllWithSpecAsync(spec);
                    var c = billsStore.Sum(s => s.ItemQuantity);
                    if (billsStore.Sum(s => s.ItemQuantity) <= 0)
                    {
                        var store = await _unitOfWork.StoresRepository.GetByIdAsync(Id);
                        _unitOfWork.StoresRepository.Delete(store);
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
