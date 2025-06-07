using AutoMapper;
using BillsBLL.IReposatories;
using BillsBLL.Specifications.BillSpecifications;
using BillsBLL.Specifications.SalesBillSpecifications;
using BillsBLL.Specifications.StockSpecifications;
using BillsEntity;
using BillsManagmentSystem.Reports;
using BillsManagmentSystem.ViewModels;
using Demo.PL.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.QueryBuilder.Native.Services;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer.Native.Services;
using DevExpress.XtraPrinting;
using Microsoft.AspNetCore.Authorization;

namespace BillsManagmentSystem.Controllers
{
	[Authorize]
    public class SalesController : Controller
	{
		private readonly IMapper mapper;
		private readonly IUnitOfWork unitOfWork;

		public SalesController(IMapper mapper,
								IUnitOfWork unitOfWork)
		{
			this.mapper = mapper;
			this.unitOfWork = unitOfWork;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var spec = new SalesBillHeaderWithSpec();
			var invoices = await unitOfWork.SalesBillHeaderRepository.GetAllWithSpecAsync(spec);
			//ViewBag.Customer = await unitOfWork.CustomerRepository.GetAllAsync();
			//ViewBag.Items = await unitOfWork.ItemsRepository.GetAllAsync();
			return View(invoices);
		}


		[HttpGet]
		public async Task<IActionResult> Create( int StoreId)
		{

			//var spec = new StockWithSpec();
			//ViewBag.Items = unitOfWork.StockRepository.GetAllWithSpecAsync(spec).Result.Where(c => c.StoreId == StoreId && c.ItemQuantity > 0).Select(s => s.Item);
			ViewBag.Customer = await unitOfWork.CustomerRepository.GetAllAsync();
			ViewBag.Stores = await unitOfWork.StoresRepository.GetAllAsync();

			var lastBILCOD = unitOfWork.SalesBillHeaderRepository.GetLastId();
			ViewBag.CurrentBilCod = lastBILCOD + 1;

			DateTime date = DateTime.Now;
			ViewBag.CurrentDate = date.ToString("yyyy-MM-dd HH:mm:ss");

			return View();
		}


		public async Task<SalesBillHeaderViewModel?> CreateOrder(SalesBillHeaderViewModel billHeaderViewModel, string listproducts)
		{
			List<SalesBillDetailsViewModel> listitems = JsonConvert.DeserializeObject<List<SalesBillDetailsViewModel>>(listproducts);

			var mappedSalesBillHeader = mapper.Map<SalesBillHeader>(billHeaderViewModel);
			if (billHeaderViewModel.Image is not null)
				mappedSalesBillHeader.BILIMG = DocumentSettings.UploadFile(billHeaderViewModel.Image, "Images");

			var existingBill = await unitOfWork.SalesBillHeaderRepository.GetByIdAsync(mappedSalesBillHeader.BILCOD);
			if (existingBill == null && listitems.Count > 0)
			{


				unitOfWork.SalesBillHeaderRepository.Add(mappedSalesBillHeader);
				foreach (var item in listitems)
				{
					var mappedSalesBillDetails = mapper.Map<SalesBillDetails>(item);
					mappedSalesBillDetails.BILCOD = mappedSalesBillHeader.BILCOD;
					unitOfWork.SalesBillDetailsRepository.Add(mappedSalesBillDetails);

					var stockSpec = new StockWithSpec(mappedSalesBillDetails.ITMCOD, mappedSalesBillHeader.StoreId);
					var existingStock = await unitOfWork.StockRepository.GetByIdWithSpecAsync(stockSpec);

					existingStock.ItemQuantity -= mappedSalesBillDetails.ITMQTY;
					await unitOfWork.StockRepository.UpdateAsync(existingStock);

				}
				return billHeaderViewModel;

			}
			return null;

		}


		#region Section Gets
		public async Task<JsonResult> GetPrice(int itemId)
		{
			var storeTable = await unitOfWork.ItemsRepository.GetByIdAsync(itemId);
			var price = storeTable.ItmPrc;
			return Json(price);
		}

		public async Task<JsonResult> GetTotalPrice(int invoiceNo)
		{
			//var invoiceNum = int.Parse(invoiceNo);
			var products = await unitOfWork.SalesBillDetailsRepository.GetByInvoiceNoAsync(invoiceNo);
			decimal totalPrice = 0;

			foreach (var item in products)
			{
				var total = item.ITMQTY * item.ITMPRC;
				totalPrice += total;
			}

			var spec = new SalesBillHeaderWithSpec(invoiceNo);
			var billHeader = await unitOfWork.SalesBillHeaderRepository.GetByIdWithSpecAsync(spec);
			billHeader.BILPRC = totalPrice;

			await unitOfWork.SalesBillHeaderRepository.UpdateAsync(billHeader);

			return Json(totalPrice);
		}

		public async Task<JsonResult> GetItems(int? BILCOD)
		{
			if (BILCOD == null || BILCOD == 0)
			{
				// Handle the case when BILCOD is null or 0, for example, returning an empty array
				return Json(new List<SalesBillDetails>());
			}

			// Retrieve items associated with the provided BILCOD
			List<SalesBillDetails> items = await unitOfWork.SalesBillDetailsRepository.GetByInvoiceNoAsync(BILCOD);

			// Return the JSON result
			return Json(items);
		}

		public JsonResult GetItemsByStore(int? storeId)
		{
			if(storeId != null)
			{
				var spec = new StockWithSpec();
				var Items = unitOfWork.StockRepository.GetAllWithSpecAsync(spec).Result.Where(c => c.StoreId == storeId && c.ItemQuantity > 0);
				return Json(Items);	
			}

			return Json(null);
		}
		#endregion

		#region Section Details
		public FileResult PagePrinter(int id)
		{
            PrintSalesBillReport report = new PrintSalesBillReport();
			report.Parameters["FilterByBilCod"].Value = id;
            byte[] pdfContent;

            var pdfOptions = new PdfExportOptions
            {
                ShowPrintDialogOnOpen = true,
                Compressed = true
            };

            using (var memoryStream = new MemoryStream())
            {
                report.ExportToPdf(memoryStream, pdfOptions);
                pdfContent = memoryStream.ToArray();
            }

            // Return PDF content without forcing download
            return File(pdfContent, "application/pdf");
        }
		
		public IActionResult TestReport(int id)
		{
            PrintSalesBillReport report = new PrintSalesBillReport();
			report.Parameters["FilterByBilCod"].Value = id;

            // Return PDF content without forcing download
            return View(report);
        }
		#endregion



		#region Section Update
		[HttpGet("Update")]
		public async Task<IActionResult> Update(int? id)
		{
			if (id is null)
				return NotFound();

			
			ViewBag.Customer = await unitOfWork.CustomerRepository.GetAllAsync();
			ViewBag.Stores = await unitOfWork.StoresRepository.GetAllAsync();
			ViewBag.Items = await unitOfWork.ItemsRepository.GetAllAsync();

			var billheader = await unitOfWork.SalesBillHeaderRepository.GetByInvoiceNoAsync(id);

			var billdetails = await unitOfWork.SalesBillDetailsRepository.GetByInvoiceNoAsync(id);

			//var spec = new StockWithSpec();
			//ViewBag.Items = unitOfWork.StockRepository.GetAllWithSpecAsync(spec).Result.Where(c => c.StoreId == billheader.StoreId && c.ItemQuantity > 0);

			var mapview = new BillViewModel();


			if (billheader is not null && billdetails.Count > 0)
			{
				var mappedSalesBillDetails = mapper.Map<List<SalesBillDetails>, List<SalesBillDetailsViewModel>>(billdetails);
				var mappedSalesBillHeader = mapper.Map<SalesBillHeaderViewModel>(billheader);

				ViewBag.SalesBillDetails = mappedSalesBillDetails;
				return View(mappedSalesBillHeader);
			}
			else
			{
				return RedirectToAction("Error", "Home");
			}



		}

		[HttpPost]
		public async Task<JsonResult> Update(SalesBillHeaderViewModel model, string billDetails)
		{
			try
			{
				if (ModelState.IsValid)
				{
					List<SalesBillDetailsViewModel> listItems = JsonConvert.DeserializeObject<List<SalesBillDetailsViewModel>>(billDetails);

					if (model.Image is not null)
					{
						model.BILIMG = DocumentSettings.UploadFile(model.Image, "Images");
					}

					var mappedbillHeader = mapper.Map<SalesBillHeader>(model);

					if (mappedbillHeader is not null && listItems is not null)
					{
						foreach (var item in listItems)
						{
							if (item.DTLCOD != 0)
							{
								var mappedSalesBillDetails = mapper.Map<SalesBillDetails>(item);

								var stockSpec = new StockWithSpec(item.ITMCOD, model.StoreId);
								var existingStock = await unitOfWork.StockRepository.GetByIdWithSpecAsync(stockSpec);

								if (item.IsDeleted == true)
								{
									//Update In Stock
									existingStock.ItemQuantity += item.ITMQTY;
									await unitOfWork.StockRepository.UpdateAsync(existingStock);

									//Update In SalesBillDetails
									await unitOfWork.SalesBillDetailsRepository.UpdateAsync(mappedSalesBillDetails);
								}
								else
								{
									// This SalesBillDetails Before it Gets Any Update 
									var existingItem = await unitOfWork.SalesBillDetailsRepository.GetByIdAsync(item.DTLCOD);

									int quantityDiff = existingItem.ITMQTY - item.ITMQTY;
									//Update In Stock
									existingStock.ItemQuantity += quantityDiff;
									await unitOfWork.StockRepository.UpdateAsync(existingStock);

									//Update In SalesBillDetails
									existingItem.ITMCOD = item.ITMCOD;
									existingItem.ITMQTY = item.ITMQTY;
									existingItem.ITMPRC = item.ITMPRC;
									await unitOfWork.SalesBillDetailsRepository.UpdateAsync(existingItem);
								}
							}
							else if (item.DTLCOD == 0 && !item.IsDeleted)
							{
								item.BILCOD = model.BILCOD;
								var mappedSalesBillDetails = mapper.Map<SalesBillDetails>(item);

								// Add Item IN Stock
								var stockSpec = new StockWithSpec(item.ITMCOD, model.StoreId);
								var existingStock = await unitOfWork.StockRepository.GetByIdWithSpecAsync(stockSpec);

								existingStock.ItemQuantity -= item.ITMQTY;
								await unitOfWork.StockRepository.UpdateAsync(existingStock);


								unitOfWork.SalesBillDetailsRepository.Add(mappedSalesBillDetails);
							}
						}

						await unitOfWork.SalesBillHeaderRepository.UpdateAsync(mappedbillHeader);

						return Json(mappedbillHeader);
					}
					else
					{
						return null;
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}


			return null;

		}


		public async Task<JsonResult> UpdateItem(ItemDetailsViewModel model)
		{
			if (ModelState.IsValid)
			{
				// Quantity Difference that should be happened in Stock
				int quantityDiff = 0;

				// Get item that will do update on it
				var spec = new SalesBillDetailsWithSpec(model.DTLCOD);
				var item = await unitOfWork.SalesBillDetailsRepository.GetByIdWithSpecAsync(spec);
				quantityDiff = item.ITMQTY - model.ITMQTY;
				item.ITMPRC = model.ITMPRC;
				item.ITMQTY = model.ITMQTY;
				await unitOfWork.SalesBillDetailsRepository.UpdateAsync(item);

				//Update In Stock
				var stockSpec = new StockWithSpec(item.ITMCOD, item.BillHeader.StoreId);
				var existingStock = await unitOfWork.StockRepository.GetByIdWithSpecAsync(stockSpec);
				existingStock.ItemQuantity += quantityDiff;
				await unitOfWork.StockRepository.UpdateAsync(existingStock);

				var total = await GetTotalPrice(item.BILCOD);
				return Json(item);
			}

			return null;
		}

		#endregion


		#region Section Delete
		public async Task<IActionResult> Delete(int id)
		{
			if (id == 0)
				return BadRequest();


			var billHeader = await unitOfWork.SalesBillHeaderRepository.GetByInvoiceNoAsync(id);

			var billDetails = await unitOfWork.SalesBillDetailsRepository.GetByInvoiceNoAsync(id);

			if (billHeader is null && billDetails is null)
				return BadRequest();

			foreach (var item in billDetails)
			{
				//Soft Delete on BillDeatails
				item.IsDeleted = true;
				await unitOfWork.SalesBillDetailsRepository.UpdateAsync(item);

				//Update on Stock
				var stockSpec = new StockWithSpec(item.ITMCOD, billHeader.StoreId);
				var existingStock = await unitOfWork.StockRepository.GetByIdWithSpecAsync(stockSpec);
				existingStock.ItemQuantity += item.ITMQTY;
				await unitOfWork.StockRepository.UpdateAsync(existingStock);

			}

			//Soft Delete on SalesBillHeader
			billHeader.IsDeleted = true;
			await unitOfWork.SalesBillHeaderRepository.UpdateAsync(billHeader);

			return RedirectToAction(nameof(Index));
		}

		public async Task<JsonResult> DeleteItem(int DTLCOD)
		{
			if (DTLCOD != 0)
			{
				//Update In SalesBillDetails
				var spec = new SalesBillDetailsWithSpec(DTLCOD);
				var item = await unitOfWork.SalesBillDetailsRepository.GetByIdWithSpecAsync(spec);
				item.IsDeleted = true;
				await unitOfWork.SalesBillDetailsRepository.UpdateAsync(item);


				//Update In Stock
				var stockSpec = new StockWithSpec(item.ITMCOD, item.BillHeader.StoreId);
				var existingStock = await unitOfWork.StockRepository.GetByIdWithSpecAsync(stockSpec);
				existingStock.ItemQuantity += item.ITMQTY;
				await unitOfWork.StockRepository.UpdateAsync(existingStock);

				//Update BILPRC in SalesBillHeader
				await GetTotalPrice(item.BILCOD);
				return Json(item);
			}

			return null;
		}
		#endregion


	}
}
