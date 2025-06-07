using AutoMapper;
using BillsBLL.IReposatories;
using BillsBLL.Specifications.BillHeader_Specifications;
using BillsBLL.Specifications.BillSpecifications;
using BillsBLL.Specifications.StockSpecifications;
using BillsDAL.Reposatories;
using BillsEntity;
using BillsManagmentSystem.Reports;
using BillsManagmentSystem.ViewModels;
using Demo.PL.Helper;
using DevExpress.XtraPrinting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BillsManagmentSystem.Controllers
{
	[Authorize(Roles = "Admin")]
	public class BillsController : Controller
    {
        private readonly IMapper mapper;
		private readonly IUnitOfWork unitOfWork;

		public BillsController(IMapper mapper , 
                                IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
			this.unitOfWork = unitOfWork;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var spec = new BillHeaderWithSpec();
			var invoices = await unitOfWork.BillHeaderRepository.GetAllWithSpecAsync(spec);
            ViewBag.Vendor = await unitOfWork.VendorRepository.GetAllAsync();
            ViewBag.Items = await unitOfWork.ItemsRepository.GetAllAsync();
            return View(invoices);
		}


		[HttpGet]
        public async Task<IActionResult> Create()
        {

            ViewBag.Vendor = await unitOfWork.VendorRepository.GetAllAsync() ;
            ViewBag.Items = await unitOfWork.ItemsRepository.GetAllAsync();
			ViewBag.Stores = await unitOfWork.StoresRepository.GetAllAsync();

			var lastBILCOD = unitOfWork.BillHeaderRepository.GetLastId();
			ViewBag.CurrentBilCod = lastBILCOD + 1;

            DateTime date = DateTime.Now;
            ViewBag.CurrentDate = date.ToString("yyyy-MM-dd HH:mm:ss");

            return View();
        }

		
		public async Task<BillHeaderViewModel?> CreateOrder( BillHeaderViewModel billHeaderViewModel , string listproducts)
		{
			List<BillDetailsViewModel> listitems = JsonConvert.DeserializeObject<List<BillDetailsViewModel>>(listproducts);

			var mappedBillHeader = mapper.Map<BillHeader>(billHeaderViewModel);
            if (billHeaderViewModel.Image is not null)	
				mappedBillHeader.BILIMG = DocumentSettings.UploadFile(billHeaderViewModel.Image, "Images");

			var existingBill = await unitOfWork.BillHeaderRepository.GetByIdAsync(mappedBillHeader.BILCOD);
			if(existingBill == null && listitems.Count > 0)
			{


				unitOfWork.BillHeaderRepository.Add(mappedBillHeader);
				foreach (var item in listitems)
				{
					var mappedBillDetails = mapper.Map<BillDetails>(item);
					mappedBillDetails.BILCOD = mappedBillHeader.BILCOD;
					unitOfWork.BillDetailsRepository.Add(mappedBillDetails);

					var stockSpec = new StockWithSpec(mappedBillDetails.ITMCOD, mappedBillHeader.StoreId);
					var existingStock = await unitOfWork.StockRepository.GetByIdWithSpecAsync(stockSpec);	

					if(existingStock != null)
					{
						existingStock.ItemQuantity += mappedBillDetails.ITMQTY;
						await unitOfWork.StockRepository.UpdateAsync(existingStock);
					}
					else
					{
						var stock = new Stock() {
							ItemId = mappedBillDetails.ITMCOD,
							StoreId = mappedBillHeader.StoreId,
							ItemQuantity = mappedBillDetails.ITMQTY,
						};
						unitOfWork.StockRepository.Add(stock);
					}


                }
				return billHeaderViewModel;

			}
			return null;

		}

		public async Task<JsonResult> GetPrice(int itemId)
		{
			var storeTable = await unitOfWork.ItemsRepository.GetByIdAsync(itemId) ;
			var price = storeTable.ItmPrc;
			return Json(price);
		}

		public async Task<JsonResult> GetTotalPrice(int invoiceNo)
		{
			//var invoiceNum = int.Parse(invoiceNo);
			var products = await unitOfWork.BillDetailsRepository.GetByInvoiceNoAsync(invoiceNo);
			decimal totalPrice = 0;

			foreach (var item in products)
			{
				var total = item.ITMQTY * item.ITMPRC;
				totalPrice += total;
			}

			var spec = new BillHeaderWithSpec(invoiceNo);
			var billHeader = await unitOfWork.BillHeaderRepository.GetByIdWithSpecAsync(spec);
			billHeader.BILPRC = totalPrice;

			await unitOfWork.BillHeaderRepository.UpdateAsync(billHeader);

			return Json(totalPrice);
		}
		public async Task<ActionResult> Details(string id)
		{
			if (id == null)
				return RedirectToAction(nameof(Create));
			int Id = int.Parse(id);
			var specHeader = new BillHeaderWithSpec(Id);
			var specDetails = new BillDetailsWithSpec(Id);
			var bh = await unitOfWork.BillHeaderRepository.GetByIdWithSpecAsync(specHeader);
			var list = await unitOfWork.BillDetailsRepository.GetByInvoiceNoAsync(Id);
			ViewBag.pro = list;
			return View(bh);
		}



        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
                return NotFound();

			ViewBag.Vendor = await unitOfWork.VendorRepository.GetAllAsync();
			ViewBag.Items = await unitOfWork.ItemsRepository.GetAllAsync();
			ViewBag.Stores = await unitOfWork.StoresRepository.GetAllAsync();

			var billheader = await unitOfWork.BillHeaderRepository.GetByInvoiceNoAsync(id);

			var billdetails = await unitOfWork.BillDetailsRepository.GetByInvoiceNoAsync(id);

			var mapview = new BillViewModel();


            if (billheader is not null &&  billdetails.Count > 0)
			{
				var mappedBillDetails = mapper.Map<List<BillDetails>, List<BillDetailsViewModel>>(billdetails);
				var mappedBillHeader = mapper.Map<BillHeaderViewModel>(billheader);

				ViewBag.BillDetails = mappedBillDetails;
                return View(mappedBillHeader);
			}
			else
			{
				return RedirectToAction("Error", "Home");
			}

            
            
        }

        [HttpPost]
        public async Task<JsonResult> Update(BillHeaderViewModel model , string billDetails)
        {
            try
            {
                if(ModelState.IsValid)
                {
					List<BillDetailsViewModel> listItems = JsonConvert.DeserializeObject<List<BillDetailsViewModel>>(billDetails);

					if(model.Image is not null)
					{
						model.BILIMG = DocumentSettings.UploadFile(model.Image, "Images");
					}

					var mappedbillHeader = mapper.Map<BillHeader>(model);

					if(mappedbillHeader is not null && listItems is not null)
					{
						foreach (var item in listItems)
						{
							if (item.DTLCOD != 0)
							{
								var mappedBillDetails = mapper.Map<BillDetails>(item);

								var stockSpec = new StockWithSpec(item.ITMCOD, model.StoreId);
								var existingStock = await unitOfWork.StockRepository.GetByIdWithSpecAsync(stockSpec);

								if (item.IsDeleted == true)
								{
									//Update In Stock
									existingStock.ItemQuantity -= item.ITMQTY;
                                    await unitOfWork.StockRepository.UpdateAsync(existingStock);

									//Update In BillDetails
									await unitOfWork.BillDetailsRepository.UpdateAsync(mappedBillDetails);
								}
								else
								{
									// This BillDetails Before it Gets Any Update 
									var existingItem = await unitOfWork.BillDetailsRepository.GetByIdAsync(item.DTLCOD);

									int quantityDiff = item.ITMQTY - existingItem.ITMQTY;
									//Update In Stock
									existingStock.ItemQuantity += quantityDiff;
									await unitOfWork.StockRepository.UpdateAsync(existingStock);

									//Update In BillDetails
									existingItem.ITMCOD = item.ITMCOD;
									existingItem.ITMQTY = item.ITMQTY;
									existingItem.ITMPRC = item.ITMPRC;
									await unitOfWork.BillDetailsRepository.UpdateAsync(existingItem);
								}
							}
							else if (item.DTLCOD == 0 && !item.IsDeleted)
							{
								item.BILCOD = model.BILCOD;
								var mappedBillDetails = mapper.Map<BillDetails>(item);

								// Add Item IN Stock
								var stockSpec = new StockWithSpec(item.ITMCOD, model.StoreId);
								var existingStock = await unitOfWork.StockRepository.GetByIdWithSpecAsync(stockSpec);

								if (existingStock != null)
								{
									existingStock.ItemQuantity += item.ITMQTY;
									await unitOfWork.StockRepository.UpdateAsync(existingStock);
								}
								else
								{
									var stock = new Stock()
									{
										ItemId = item.ITMCOD,
										StoreId = model.StoreId,
										ItemQuantity = item.ITMQTY,
									};
									unitOfWork.StockRepository.Add(stock);
								}

								unitOfWork.BillDetailsRepository.Add(mappedBillDetails);
							}
						}

						await unitOfWork.BillHeaderRepository.UpdateAsync(mappedbillHeader);

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
				var spec = new BillDetailsWithSpec(model.DTLCOD);
				var item = await unitOfWork.BillDetailsRepository.GetByIdWithSpecAsync(spec) ;
				quantityDiff = model.ITMQTY - item.ITMQTY;
				item.ITMPRC = model.ITMPRC;
				item.ITMQTY = model.ITMQTY;
				await unitOfWork.BillDetailsRepository.UpdateAsync(item);

                //Update In Stock
                var stockSpec = new StockWithSpec(item.ITMCOD, item.BillHeader.StoreId);
                var existingStock = await unitOfWork.StockRepository.GetByIdWithSpecAsync(stockSpec);
                existingStock.ItemQuantity += quantityDiff;
                await unitOfWork.StockRepository.UpdateAsync(existingStock);

                var total =  await GetTotalPrice(item.BILCOD);
				return Json(item) ;
			}
				
			return null;
		}


        public async Task<JsonResult> GetItems(int? BILCOD)
		{
            if (BILCOD == null || BILCOD == 0)
            {
                // Handle the case when BILCOD is null or 0, for example, returning an empty array
                return Json(new List<BillDetails>());
            }

            // Retrieve items associated with the provided BILCOD
            List<BillDetails> items = await unitOfWork.BillDetailsRepository.GetByInvoiceNoAsync(BILCOD);

            // Return the JSON result
            return Json(items);
        }

		public async Task<IActionResult> Delete(int id)
		{
			if (id == 0)
				return BadRequest();


            var billHeader = await unitOfWork.BillHeaderRepository.GetByInvoiceNoAsync(id);

            var billDetails = await unitOfWork.BillDetailsRepository.GetByInvoiceNoAsync(id);

			if (billHeader is null && billDetails is null)
				return BadRequest();

			foreach(var item in billDetails)
			{
				//Soft Delete on BillDeatails
				item.IsDeleted = true;
				await unitOfWork.BillDetailsRepository.UpdateAsync(item);

				//Update on Stock
                var stockSpec = new StockWithSpec(item.ITMCOD, billHeader.StoreId);
                var existingStock = await unitOfWork.StockRepository.GetByIdWithSpecAsync(stockSpec);
				existingStock.ItemQuantity -= item.ITMQTY;
                await unitOfWork.StockRepository.UpdateAsync(existingStock);

            }

			//Soft Delete on BillHeader
			billHeader.IsDeleted = true;
		     await unitOfWork.BillHeaderRepository.UpdateAsync(billHeader);

            return RedirectToAction(nameof(Index));
		}

		public async Task<JsonResult> DeleteItem(int DTLCOD)
		{
            if (DTLCOD != 0)
            {
				//Update In BillDetails
				var spec = new BillDetailsWithSpec(DTLCOD);
                var item = await unitOfWork.BillDetailsRepository.GetByIdWithSpecAsync(spec);
				item.IsDeleted = true;
                await unitOfWork.BillDetailsRepository.UpdateAsync(item);


				//Update In Stock
                var stockSpec = new StockWithSpec(item.ITMCOD, item.BillHeader.StoreId);
                var existingStock = await unitOfWork.StockRepository.GetByIdWithSpecAsync(stockSpec);
                existingStock.ItemQuantity -= item.ITMQTY;
                await unitOfWork.StockRepository.UpdateAsync(existingStock);

				//Update BILPRC in BillHeader
                await GetTotalPrice(item.BILCOD);
                return Json(item);
            }

            return null;
        }

        #region Section Details
        public FileResult PagePrinter(int id)
        {
            PrintPurchaseBillReport report = new PrintPurchaseBillReport();
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
            PrintPurchaseBillReport report = new PrintPurchaseBillReport();
            report.Parameters["FilterByBilCod"].Value = id;

            // Return PDF content without forcing download
            return View(report);
        }
        #endregion

    }
}
