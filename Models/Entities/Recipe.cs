using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HogwartsPotions.Models.Entities;

public class Recipe
{
    public Recipe()
    {
        Ingredients = new();
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }
    public string Name { get; set; }

    public Student Student { get; set; }
    public HashSet<Ingredient> Ingredients { get; set; }
}