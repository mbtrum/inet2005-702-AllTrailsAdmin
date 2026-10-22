using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AllTrailsAdmin.Models
{
    public class Category
    {
        // primary key
        public int Id { get; set; }

        // navigation property
        [ValidateNever]
        public List<Trail> Trails { get; set; } = new List<Trail>();



        public string Title { get; set; } = string.Empty;

        

    }
}
