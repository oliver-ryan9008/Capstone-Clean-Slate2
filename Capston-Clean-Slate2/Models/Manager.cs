using System.ComponentModel.DataAnnotations;

namespace Capston_Clean_Slate2.Models
{
    public class Manager
    {
        [Key]
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ApprovalStatus { get; set; }
    }
}