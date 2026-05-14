using System.ComponentModel;

namespace FluentValidator.Extensions.Br.Tests.Api.Dtos
{
    public enum DocumentType
    {
        [Description("Undefined")]
        UNDEFINED,
        [Description("CPF")]
        CPF,
        [Description("CNPJ")]
        CNPJ
    }
    public class GetValidatorRequest
    {
        public string? Document { get; set; }
        public DocumentType? Type { get; set; }
    }
}
