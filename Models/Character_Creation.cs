namespace Character_Creation.Models
{
    public class Character_Creation
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

      
    }
}
