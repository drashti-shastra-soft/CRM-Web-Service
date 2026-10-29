using CRMWebService.Infrastructure.CustomException;
using CRMWebService.Infrastructure.IService;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System;

namespace CRMWebService.Controllers
{
    [Authorize, RoutePrefix("api/UserType")]
    public class UserTypeController : ApiController
    {
        private readonly IUserTypeService _service;

        public UserTypeController(IUserTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.OK, _service.GetAll());
            }
            catch (APIRequestFailedException ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

    }
}
