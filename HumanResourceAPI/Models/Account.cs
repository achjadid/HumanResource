using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanResourceAPI.Models
{
    public class Account
    {
        [Key]
        public string NIK { get; set; }
        public string Password { get; set; }
        [ForeignKey("NIK")]
        [JsonIgnore]
        public Employee? Employee { get; set; }
    }
}
