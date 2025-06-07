using AutoMapper;
using BillsBLL.IReposatories;
using BillsBLL.Reposatories;
using BillsBLL.Specifications.StockSpecifications;
using BillsManagmentSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillsManagmentSystem.Controllers
{
	[Authorize]
	public class StockController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public StockController(IUnitOfWork unitOfWork
								, IMapper mapper)
        {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
        public async Task<IActionResult> Index()
		{
			var spec = new StockWithSpec();
			var stocks = await _unitOfWork.StockRepository.GetAllWithSpecAsync(spec);
			var mappedStock = _mapper.Map<IEnumerable<StockViewModel>>(stocks);
			return View(mappedStock);
		}
	}
}
