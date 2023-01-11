using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUS.Models
{
    [Table("Services")] 
    public class Service
    {
        [Key] 
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

		public Profile Owner { get; set; }
        public ICollection<Chat> Chats { get; set; }
    }
}