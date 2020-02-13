using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using Shop.Data;
using Microsoft.EntityFrameworkCore;

namespace Shop.Controllers
{
    [Route("v1/categories")]
    public class CategoryController : Controller
    {

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        //Cachear por método
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
        //Se habilitado para todos no Startup, desabilitar método que não pode
        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<List<Category>>> Get(
            [FromServices]DataContext context)
        {
            var Categories = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(Categories);
        }


        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> Get(int id,
            [FromServices]DataContext context)
        {
            Category category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            return Ok(category);
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Category>>> Post(
            [FromBody]Category category,
            [FromServices]DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Categories.Add(category);
                await context.SaveChangesAsync();
                return Ok(category);
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível criar a categoria." });
            }

        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Category>>> Put(int id,
        [FromBody]Category category,
        [FromServices] DataContext context)
        {
            if (id != category.Id)
                return NotFound(new { message = "Categoria não ecncontrada." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Category>(category).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(category);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro já foi atualizado." });
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível atualizar a categoria." });
            }


        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Category>>> Delete(
                int id,
                [FromServices] DataContext context)
        {
            Category category = await context.Categories.FirstOrDefaultAsync(e => e.Id == id);
            if (category == null)
                return NotFound(new { message = "Categoria não encontrada." });
            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(new { message = "Categoria removida com sucesso." });
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível remover a categoria." });
            }
        }

        // [HttpGet]
        // [Route("")]
        // [AllowAnonymous]
        // [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
        // // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        // {
        //     var categories = await context.Categories.AsNoTracking().ToListAsync();
        //     return categories;
        // }

        // [HttpGet]
        // [Route("{id:int}")]
        // [AllowAnonymous]
        // public async Task<ActionResult<Category>> GetById([FromServices] DataContext context, int id)
        // {
        //     var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        //     return category;
        // }

        // [HttpPost]
        // [Route("")]
        // // [Authorize(Roles = "employee")]
        // [AllowAnonymous]
        // public async Task<ActionResult<Category>> Post(
        //     [FromServices] DataContext context,
        //     [FromBody]Category model)
        // {
        //     // Verifica se os dados são válidos
        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState);

        //     try
        //     {
        //         context.Categories.Add(model);
        //         await context.SaveChangesAsync();
        //         return model;
        //     }
        //     catch (Exception)
        //     {
        //         return BadRequest(new { message = "Não foi possível criar a categoria" });

        //     }
        // }

        // [HttpPut]
        // [Route("{id:int}")]
        // [Authorize(Roles = "employee")]
        // public async Task<ActionResult<Category>> Put(
        //     [FromServices] DataContext context,
        //     int id,
        //     [FromBody]Category model)
        // {
        //     // Verifica se o ID informado é o mesmo do modelo
        //     if (id != model.Id)
        //         return NotFound(new { message = "Categoria não encontrada" });

        //     // Verifica se os dados são válidos
        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState);

        //     try
        //     {
        //         context.Entry<Category>(model).State = EntityState.Modified;
        //         await context.SaveChangesAsync();
        //         return model;
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         return BadRequest(new { message = "Não foi possível atualizar a categoria" });

        //     }
        // }

        // [HttpDelete]
        // [Route("{id:int}")]
        // [Authorize(Roles = "employee")]
        // public async Task<ActionResult<Category>> Delete(
        //     [FromServices] DataContext context,
        //     int id)
        // {
        //     var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        //     if (category == null)
        //         return NotFound(new { message = "Categoria não encontrada" });

        //     try
        //     {
        //         context.Categories.Remove(category);
        //         await context.SaveChangesAsync();
        //         return category;
        //     }
        //     catch (Exception)
        //     {
        //         return BadRequest(new { message = "Não foi possível remover a categoria" });

        //     }
        // }
    }
}