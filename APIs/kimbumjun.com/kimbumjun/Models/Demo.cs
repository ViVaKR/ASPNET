using System.ComponentModel.DataAnnotations;

namespace kimbumjun.Models
{
    public class Demo
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public string MyName { get; set; }

    }
}
