using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsPotions.Controllers;

[ApiController, Route("/potions")]
public class PotionController : ControllerBase
{
    private readonly IStudent _studentImplementation;
    private readonly IIngredient _ingredientImplementation;
    private readonly IRecipe _recipeImplementation;
    private readonly IPotion _potionImplementation;

    public PotionController(IStudent student, IIngredient ingredient, IRecipe recipe, IPotion potion)
    {
        _studentImplementation = student;
        _ingredientImplementation = ingredient;
        _recipeImplementation = recipe;
        _potionImplementation = potion;
    }

    [HttpGet]
    public async Task<ActionResult<List<Potion>>> GetAllPotions()
    {
        List<Potion> potions = await _potionImplementation.GetAllPotions();
        if (potions.Count == 0)
        {
            return NoContent();
        }
        return Ok(potions);
    }

    [HttpPost("{studentId}")]
    public async Task<ActionResult<Potion>> AddPotion(long studentId, [FromBody] Potion potion)
    {
        Student student = await _studentImplementation.GetStudent(studentId);

        if (student is null)
        {
            return NotFound($"Student #{studentId} doesn't exist!");
        }

        potion.Student = student;
        await _potionImplementation.AddPotion(potion);
        return Created("AddPotion", potion);
    }

    [HttpGet("{studentId}")]
    public async Task<ActionResult<List<Potion>>> GetAllPotionsByStudent(long studentId)
    {
        List<Potion> potions = await _potionImplementation.GetAllPotionsByStudent(studentId);
        if (potions.Count == 0)
        {
            return NoContent();
        }
        return Ok(potions);
    }

    [HttpPost("brew/{studentId}")]
    public async Task<ActionResult<Potion>> BrewPotion(long studentId)
    {
        Student student = await _studentImplementation.GetStudent(studentId);

        if (student is null)
        {
            return NotFound($"Student #{studentId} doesn't exist!");
        }

        Potion potion = await _potionImplementation.AddEmptyPotion(student);
        return Created("BrewPotion", potion);
    }

    [HttpPut("{potionId}/add")]
    public async Task<ActionResult<Potion>> AddIngredientToPotion(long potionId, [FromBody] Ingredient ingredient)
    {
        Potion potion = await _potionImplementation.GetPotion(potionId);

        if (potion is null)
        {
            return NotFound($"Potion #{potionId} doesn't exist!");
        }

        if (potion.Ingredients.Count >= 5)
        {
            return StatusCode(500, $"Potion #{potionId} has too many ingredient.");
        }

        List<Ingredient> ingredients = await _ingredientImplementation.GetAllIngredients();

        foreach (Ingredient ingrdnt in ingredients)
        {
            if (ingrdnt.Name == ingredient.Name)
            {
                await _potionImplementation.AddIngredientToPotion(potionId, ingrdnt);

                if (potion.Ingredients.Count >= 5)
                {
                    await _recipeImplementation.ChangePotionStatus(potion);
                }

                return Ok(potion);
            }
        }

        await _ingredientImplementation.AddIngredient(ingredient);
        await _potionImplementation.AddIngredientToPotion(potionId, ingredient);

        if (potion.Ingredients.Count >= 5)
        {
            await _recipeImplementation.ChangePotionStatus(potion);
        }

        return Ok(potion);
    }

    [HttpGet("{potionId}/help")]
    public async Task<ActionResult<List<Recipe>>> GetAllRecipesWithPotionIngredients(long potionId)
    {
        List<Recipe> recipies = await _recipeImplementation.GetAllRecipesWithPotionIngredients(potionId);

        if (recipies.Count == 0)
        {
            return NoContent();
        }

        return Ok(recipies);
    }
}