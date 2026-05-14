using FluentValidation;
using FluentValidator.Extensions.Br.Tests.Api.Dtos;
using FluentValidator.Extensions.Br.Tests.Api.Validators;
using Microsoft.AspNetCore.Mvc;

namespace FluentValidator.Extensions.Br.Tests.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Validators : ControllerBase
    {                

        [HttpPost]
        public ObjectResult Post([FromBody] GetValidatorRequest request)
        {
            IValidator<string> validator = request.Type switch
            {
                DocumentType.CPF => new CpfValidator(),
                DocumentType.CNPJ => new CnpjValidator(),
                _ => new CpfCnpjValidator()
            };

            var validation = validator.Validate(request.Document!);
            if (validation.IsValid)
            {
                return new OkObjectResult(true);
            }

            return new BadRequestObjectResult(string.Concat(validation.Errors.Select(x => x.ErrorMessage)));
        }
    }
}
