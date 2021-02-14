using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Api.Domain.Interfaces.Services.User;
using application.Models.Responses;
using application.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace application.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _service;
        private ILogger<UserController> _logger;

        public UserController(IUserService service, ILogger<UserController> logger)
        {
            this._service = service;
            this._logger = logger;
        }


        // GET: api/<ValuesController>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Get(int? total = 25, int? page = 1, int? order = 0)
        {
            CollectionResponseModel<Api.Domain.Entities.UserEntity> response = new CollectionResponseModel<Api.Domain.Entities.UserEntity>();
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Pesquisa Inválida!");
                }

                if(page.HasValue)
                {
                    if(page.Value < 0)
                    {
                        page = 0;
                    }
                    else
                    {
                        page -= 1;
                    }
                }
                else
                {
                    page = 0;
                }

                if (total.HasValue)
                {
                    if (total.Value <= 0)
                    {
                        total = 25;
                    }
                }
                else
                {
                    total = 25;
                }

                if (order.HasValue)
                {
                    if (order.Value != 0 || order.Value != 1)
                    {
                        order = 1;
                    }
                }
                else
                {
                    order = 1;
                }


                KeyValuePair<int, IEnumerable<Api.Domain.Entities.UserEntity>> data = await  this._service.GetAll(perPage: total.Value, currentPage: page.Value, ordernation: order.Value);

                response.Message = data.Value.Any() ? "Registros localizados" : "Nenhuma registro foi localizado";
                response.Data = data.Value;
                response.CurrentPage = page.Value + 1;
                response.PerPage = total.Value;
                response.Total = data.Key;
                response.TotalPage = 1 + (int)(data.Key / total.Value);

                return Ok(response);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                response.Message = e.Message;
                response.Data = Array.Empty<Api.Domain.Entities.UserEntity>();
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        // GET api/<ValuesController>/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(Guid id)
        {
            SingleResponseModel<Api.Domain.Entities.UserEntity> response = new SingleResponseModel<Api.Domain.Entities.UserEntity>();
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Pesquisa Inválida!");
                }

                Api.Domain.Entities.UserEntity entity = await this._service.Get(id);

                response.Message = entity == null ? "Registro Não Localizado" : "Registro Localizado";
                response.Data = entity;

                return Ok(response);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);

                response.Message = e.Message;
                response.Data = null;
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        // POST api/<ValuesController>
        [Authorize("Bearer")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserCreateModel model)
        {
            SingleResponseModel<Api.Domain.Entities.UserEntity> response = new SingleResponseModel<Api.Domain.Entities.UserEntity>();
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Dados inválidos");
                }

                var entity = await this._service.Register(model.Name, model.Email);

                response.Message = entity == null ? "Usuário não foi registrado" : "Usuário registrado";
                response.Data = entity;

                return Ok(response);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);

                response.Message = e.Message;
                response.Data = null;
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        // PUT api/<ValuesController>/5
        [Authorize("Bearer")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] UserCreateModel model)
        {
            SingleResponseModel<Api.Domain.Entities.UserEntity> response = new SingleResponseModel<Api.Domain.Entities.UserEntity>();
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Dados inválidos");
                }

                var entity = await this._service.Update(id, model.Name, model.Email);

                response.Message = entity == null ? "Usuário não foi atualizado" : "Usuário atualizado";
                response.Data = entity;

                return Ok(response);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);

                response.Message = e.Message;
                response.Data = null;
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }


        // DELETE api/<ValuesController>/5
        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            SingleResponseModel<Api.Domain.Entities.UserEntity> response = new SingleResponseModel<Api.Domain.Entities.UserEntity>();
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Dados inválidos");
                }

                await this._service.Remove(id);

                response.Message = "Usuário Removido!";
                response.Data = null;

                return Ok(response);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);

                response.Message = e.Message;
                response.Data = null;
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }
    }
}
