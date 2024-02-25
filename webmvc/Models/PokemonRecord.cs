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
        /// <summary>
        /// The identifier for this resource.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The name for this resource.
        /// 
        /// </summary>
        public string? Name { get; set; } = "";
    }
}