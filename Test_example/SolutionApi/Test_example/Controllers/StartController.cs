using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test_example.Domain;
using Test_example.Service.Services;

namespace Test_example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StartController : ControllerBase
    {
        private readonly IRabbitService _service;
        public StartController(IRabbitService service)
        {
            _service = service;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost("add")]
        public async Task<ActionResult<Guid>> PostAdd([FromBody] ApiRequest request)
        {
            try
            {
                //AutoMapper maybe use
                return await _service.SendRequest(new Request()
                {
                    ClientId = request.client_id,
                    DepartmentAddress = request.department_address,
                    Amount = request.amount,
                    Currency = request.currency,
                    DateTime = DateTime.Now,
                    ClientIp = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                    RequestStatus = "new"
                });
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost("id")]
        public async Task<ActionResult<Request>> PostId([FromBody] IdRequest request)
        {
            try
            {
                return await _service.SendRequestId(request.request_id);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost("params")]
        public async Task<ActionResult<List<Request>>> PostParams([FromBody] ParamsRequest request)
        {
            try
            {
                return await _service.SendParams(new Request()
                {
                    ClientId = request.client_id,
                    DepartmentAddress = request.department_address
                });
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
