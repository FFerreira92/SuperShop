using System;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters lenght.")]
        public string Name { get; set; }



        // coloca formatação de preço com duas casas decimais (currency 2) mas só coloca a formatação após o input do utilizador,
        // permitindo que o mesmo possa colocar mais do que duas casas decimais sem que ocorra erro (applyFormatInEditMode = false)
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]         
        public decimal Price { get; set; }



        [Display(Name = "Image")] // Define o nome do campo que vai surgir na página Web para esta respetiva Propriedade (só visual)
        public string ImageUrl { get; set; }



        [Display(Name = "Last Purchase")]
        public DateTime? LastPurchase { get; set; }



        [Display(Name = "Last Sale")]
        public DateTime? LastSale { get; set; }

        

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }



        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }
    }
}
