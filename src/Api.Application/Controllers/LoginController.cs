using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Api.Domain.Interfaces.Services.User;
using application.Models;
using application.Models.Responses;
using application.Models.Users;
using Domain.Interfaces.Services.User;
using Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace application.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginService _service;
        private ILogger<LoginController> _logger;

        private SigninConfiguration _signingConfigurations;
        private TokenConfiguration _tokenConfigurations;

        public LoginController(SigninConfiguration signinConfiguration,
                               TokenConfiguration tokenConfiguration,
                               ILoginService service, 
                               ILogger<LoginController> logger)
        {
            this._service = service;
            this._logger = logger;

            this._signingConfigurations = signinConfiguration;
            this._tokenConfigurations = tokenConfiguration;
        }


        // GET: api/<ValuesController>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Auth([FromBody] UserAuthModel model)
        {
            SingleResponseModel<Api.Domain.Entities.UserEntity> response = new SingleResponseModel<Api.Domain.Entities.UserEntity>();
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Pesquisa Inválida!");
                }

                Api.Domain.Entities.UserEntity user = await this._service.FindByEmail(model.Email);

                if (user == null)
                {
                    throw new Exception("Usuário não encontrado");
                }
                else
                {
                    var claimsIdentity = new ClaimsIdentity(new GenericIdentity(model.Email),
                                                            new[] {
                                                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                                                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString())
                                                            });

                    DateTime createdDate = DateTime.Now;
                    DateTime expirationDate = DateTime.Now.AddDays(1);


                    var handler = new JwtSecurityTokenHandler();
                    var token = this.CreateToken(claimsIdentity, createdDate, expirationDate, handler);

                    var objToResponse = new {
                        authenticated = true,
                        created = createdDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        accessToken = token,
                        username = user.Email,
                        message = $"Usuário {user.Name} Autenticado"
                    };

                    return Ok(objToResponse);
                }
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                response.Message = e.Message;
                response.Data = null;
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        private string CreateToken(ClaimsIdentity identity, DateTime createdAt, DateTime expirationDate, JwtSecurityTokenHandler handler)
        {
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.Credential,
                Subject = identity,
                NotBefore = createdAt,
                Expires = expirationDate,
            });

            var token = handler.WriteToken(securityToken);
            return token;
        }

    }
}
