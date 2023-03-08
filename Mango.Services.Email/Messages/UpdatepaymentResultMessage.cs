namespace Mango.Services.Email.Messages
{
    public class UpdatepaymentResultMessage
    {
        public int OrderId { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }

    }
}
