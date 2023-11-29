using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Bulkyy.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[MaxLength(30)]
		[DisplayName("Category Name")]
		public string Name { get; set; }

		[DisplayName("Display Order")]
		[Range(1, 100, ErrorMessage = "It should be between 1-100")]
		public int DisplayOrder { get; set; }
	}
}
