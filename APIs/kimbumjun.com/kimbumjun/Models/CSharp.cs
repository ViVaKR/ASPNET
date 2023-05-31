using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace kimbumjun.Models;

public class CSharp : IProgram
{
    [Key]
    [JsonProperty(PropertyName = "id")]
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; }

    [MaxLength]
    [JsonProperty(PropertyName = "contents")]
    public string Contents { get; set; }
    
    [MaxLength]
    [JsonProperty(PropertyName = "note")]
    public string Note { get; set; }
    
   
    [JsonProperty(PropertyName = "Category")]
    public int Category { get; set; }
}