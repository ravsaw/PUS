using PUS.Models;

namespace PUS.ViewModels
{
    public class ServiceCreateViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; } = null;
        public IFormFile Image { get; set; }
        public double CropScale { get; set; }
        public int CropX { get; set; }
        public int CropY { get; set; }
    }
}
