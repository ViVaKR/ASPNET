using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace kimbumjun.Models;

public interface IProgram
{
    [Key]
    [JsonProperty(PropertyName = "id")]
    int Id { get; set; }

    [Required]
    [StringLength(250)]
    [JsonProperty(PropertyName = "title")]
    string Title { get; set; }

    [MaxLength]
    [DataType(DataType.Date)]
    [JsonProperty(PropertyName = "dateOfWriting")]
    string Contents { get; set; }

    [MaxLength]
    [JsonProperty(PropertyName = "note")]
    public string Note { get; set; }

    [JsonProperty(PropertyName = "Category")]
    public int Category { get; set; }
}