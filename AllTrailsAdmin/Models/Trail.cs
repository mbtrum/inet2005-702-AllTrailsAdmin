using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace AllTrailsAdmin.Models
{
    public class Trail
    {
        // primary key
        public int Id { get; set; }

        // foreign key
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        // navigation property
        [ValidateNever]
        [ScaffoldColumn(false)]
        public Category Category { get; set; } = null!;

        // navigation property
        [ValidateNever]
        [ScaffoldColumn(false)]
        public List<HikerReview> HikerReviews { get; set; } = new List<HikerReview>();


        [StringLength(100)]
        [Required]
        public string Title { get; set; } = string.Empty;

        [StringLength(2500)]
        [Required]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Length (km)")]
        [Range(0.1, 28000)] // trans canada trail
        [Required]
        public double Length { get; set; } // 7.9km

        [Display(Name="Elevation Gain (m)")]
        [Range(-441, 30000)] // dead sea to mt. everest
        [Required]
        public int ElevationGain { get; set; } // 200m

        [Required]
        public string Location { get; set; } = string.Empty; // address, city, province... display Google map later

        // Url to image stored in Azure Storage
        public string ImageUrl { get; set; } = string.Empty;

        //// A trail belongs to one category (REMOVED)
        //public string Category { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:MMMM d, yyyy}")]
        [Display(Name = "Created")]
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now; // default to current timestamp
    }
}
