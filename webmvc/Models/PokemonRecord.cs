using PokeApiNet;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace webmvc.Models
{
    [Table("Pokemon")]
    public class Pokemon
    {
        public int Id { get; set; }
        public string? Name { get; set; } = "";
        public int UserId { get; set; }
    }
}