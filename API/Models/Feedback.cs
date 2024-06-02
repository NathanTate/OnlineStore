using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Feedback {

  [Key]
  public int Id { get; set; }

  [Required]
  [MaxLength(100)]
  public string Name { get; set; }

  [Required]
  [MaxLength(100)]
  public string Email { get; set; }
  
  [Required]
  [MaxLength(15)]
  public string Phone { get; set; }

  [Required]
  [MaxLength(500)]
  public string Message { get; set; }
}