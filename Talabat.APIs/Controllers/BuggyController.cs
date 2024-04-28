using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
	public class BuggyController : BaseApiController
	{
		private readonly StoreContext _dbContext;

		public BuggyController(StoreContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet("notFound")] //GEt : api/byggy/notFound
		public ActionResult GetNotFoundRequest()
		{
			var product = _dbContext.Products.Find(100);
			if (product is null) return NotFound(new APIResponse(404));
			return Ok(product);
		}

		[HttpGet("serverError")]
		public ActionResult GetServerError()
		{
			var product = _dbContext.Products.Find(100);
			var productReturn = product.ToString();  // Will throw Exception [Null Reference Exception]
			return Ok(productReturn);
		}

		[HttpGet("badrequesterror")]//GEt : api/byggy/badrequest
		public ActionResult GetBadRequest()
		{
			return BadRequest();
		}

		[HttpGet("badrequest/{id}")]
		public ActionResult GetBadRequest(int id)
		{
			return Ok();
		}

	}
}
