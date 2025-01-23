using System.ComponentModel.DataAnnotations;

namespace FilmManagementSystem.Schemas;

public class FilmSchema
{
    
    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    [StringLength(50)]
    public string Genre { get; set; }

    [Required]
    [StringLength(100)]
    public string Director { get; set; }

    [Required]
    [Range(1900, 2100)]  
    public int ReleaseYear { get; set; }

    [Required]
    [Range(1, 10)] 
    public double Rating { get; set; }

    [StringLength(500)]  
    public string Description { get; set; }
}