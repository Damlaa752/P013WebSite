using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Xml.Linq;

namespace P013WebSite.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [Display(Name = "Adı :"), StringLength(50)]
        public string Name { get; set; }
        [Display(Name = "Kategori Açıklaması :")]
        public string? Description { get; set; }
        [Display(Name = "Eklenme Tarihi :")]
        public DateTime? CreateDate { get; set; }=
        DateTime.Now; // sonradan 1 class a bu şekilde property eklersek yeni bir  migration eklememiz gerekiyor! yoksa proje çalışırken hata alırız.
        public virtual List<Product>? Products { get; set; } // 1 Kategorinin 1 den çok ürünü olabilir. ( bire çok ilişkli) veritabanını etkileyen bir durum olmadığı için migrationa gerek yok 

    }
}
