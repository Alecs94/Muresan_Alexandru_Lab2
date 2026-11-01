using System.ComponentModel.DataAnnotations;

namespace Muresan_Alexandru_Lab2.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
        [ScaffoldColumn(false)]
        public ICollection<BookCategory>? BookCategories { get; set; }
    }
}
