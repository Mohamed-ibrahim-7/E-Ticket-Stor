using System.ComponentModel.DataAnnotations;

namespace E_Ticket_Stor.ViewModels
{
    public class LoginVM
    {
        public int Id { get; set; }
        [Required]
        public string EmailORUserName { get; set; } = string.Empty;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}
