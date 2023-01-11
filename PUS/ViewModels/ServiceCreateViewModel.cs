using PUS.Models;

namespace PUS.ViewModels
{
    public class ServiceCreateViewModel
    {
        public string Title { get; set; } = string.Empty;
        // będziemy widzieli, które samochody przynależą do tego komentarza
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; } = null;
        public IFormFile Image { get; set; }
        public float CropScale { get; set; }
        public int CropX { get; set; }
        public int CropY { get; set; }
    }
}
