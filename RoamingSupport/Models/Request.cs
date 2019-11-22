using System;
using System.ComponentModel.DataAnnotations;

namespace RoamingSupport.Models
{
    public class Request
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Причина")]
        public string Reason { get; set; }
        
        [Required]
        [Display(Name = "Направление")]
        public Destinations Destination { get; set; }
        
        [Required]
        [Display(Name = "Номер абонента")]
        [Phone]
        public string PhoneNumber { get; set; }
        
        [Required]
        [Display(Name = "Страна")]
        public string Country { get; set; }
        
        [Required]
        [Display(Name = "Регион")]
        public string Region { get; set; }
        
        [Required]
        [Display(Name = "Населённый пункт")]
        public string Locality { get; set; }

        [Display(Name = "Время заявки")]
        public DateTime RequestDateTime { get; set; }

        public enum Destinations
        {
            [Display(Name = "Офис обслуживания")]
            ServiceOffice = 0,
            [Display(Name = "Контакт-Центр")]
            ContactCenter = 1
        }
    }
}