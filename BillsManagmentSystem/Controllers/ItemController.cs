using AutoMapper;
using BillsBLL.IReposatories;
using BillsBLL.Specifications.BillSpecifications;
using BillsBLL.Specifications.ItemSpecifications;
using BillsBLL.Specifications.StockSpecifications;
using BillsDAL;
using BillsEntity;
using BillsManagmentSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BillsManagmentSystem.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

        public ItemController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
        }
        // GET: ItemController
        public async Task<ActionResult> Index(string searchByName)
        {
            if (!string.IsNullOrEmpty(searchByName))
            {
                var spec = new ItemWithSpec(searchByName);
                var items = await _unitOfWork.ItemsRepository.GetAllWithSpecAsync(spec);
                var mappedItems = _mapper.Map<IEnumerable<ItemViewModel>>(items);
                return View(mappedItems);
            }
            else
            {
                var items = await _unitOfWork.ItemsRepository.GetAllAsync();
                var mappedItems = _mapper.Map<IEnumerable<ItemViewModel>>(items);
                return View(mappedItems);
            }

            
        }

		[Authorize(Roles = "Admin")]
		public async Task<ActionResult> CreateItem(ItemViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var spec = new ItemWithSpec(model.ItmNam);
                    var existItem = await _unitOfWork.ItemsRepository.GetAllWithSpecAsync(spec);
                    if (existItem.Count() <= 0)
                    {
                        var mappedItem = _mapper.Map<Items>(model);
                        _unitOfWork.ItemsRepository.Add(mappedItem);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                        return Json("exist");
                   

				}
			}
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }

		[Authorize(Roles = "Admin")]
		public async Task<JsonResult> EditItem(int id, ItemViewModel model)
        {
            if (id != model.ItmCod)
                return null;
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedItem = _mapper.Map<Items>(model);
                    await _unitOfWork.ItemsRepository.UpdateAsync(mappedItem);
                    return Json(mappedItem); 
                }

            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return null;
        }

		[Authorize(Roles = "Admin")]
		public async Task<ActionResult> DeleteItem(int ItmCod)
        {
			
			try
			{
                
                if (ModelState.IsValid)
				{
                    var spec = new StockWithSpec(itemId: ItmCod);
                    var itemInStock = await _unitOfWork.StockRepository.GetAllWithSpecAsync(spec);
                    var c = itemInStock.Sum(i => i.ItemQuantity);
                    if (itemInStock.Sum(i => i.ItemQuantity) <= 0)
                    {
                        var item = await _unitOfWork.ItemsRepository.GetByIdAsync(ItmCod);
                        _unitOfWork.ItemsRepository.Delete(item);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                        return Json("false");
                    
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
