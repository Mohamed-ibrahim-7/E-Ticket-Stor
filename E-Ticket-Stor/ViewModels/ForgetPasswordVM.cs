using System.ComponentModel.DataAnnotations;

namespace E_Ticket_Stor.ViewModels
{
    public class ForgetPasswordVM
    {
        public int Id { get; set; }
        [Required]
        public string EmailORUserName { get; set; } = string.Empty;
    }
}
