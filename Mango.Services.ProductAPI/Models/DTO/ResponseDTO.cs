namespace Mango.Services.ProductAPI.Models.DTO
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; }
        public string DesplayMessage { get; set; } = string.Empty;
        public List<string> ErrorMessage { get; set; }
    }
}
