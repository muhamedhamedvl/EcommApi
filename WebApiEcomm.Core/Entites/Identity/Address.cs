using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiEcomm.Core.Entites.Identity
{
    public class Address: BaseEntity
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string City { get; set; }    
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string AppUserId { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public virtual AppUser AppUser { get; set; }


    }
}