using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;

namespace API.Controllers;

[Route("api/tarefa")]
[ApiController]
public class TarefaController : ControllerBase
{

    
    private readonly AppDataContext _context;

    public TarefaController(AppDataContext context) =>
        _context = context;

    // GET: api/tarefa/listar
    [HttpGet]
    [Route("listar")]
    public IActionResult Listar()
    {
        try
        {
            List<Tarefa> tarefas = _context.Tarefas.Include(x => x.Categoria).ToList();
            return Ok(tarefas);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // POST: api/tarefa/cadastrar
    [HttpPost]
    [Route("cadastrar")]
    public IActionResult Cadastrar([FromBody] Tarefa tarefa)
    {
        try
        {
            Categoria? categoria = _context.Categorias.Find(tarefa.CategoriaId);
            if (categoria == null)
            {
                return NotFound();
            }
            tarefa.Categoria = categoria;
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
            return Created("", tarefa);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
        [HttpPatch]
        [Route("alterar/{id}")]
        public IActionResult Alterar(int id, [FromBody] Tarefa tarefa)
        {
            try
            {
                if (tarefa == null)
                {
                    return BadRequest("O objeto Tarefa não pode ser nulo.");
                }

                // Verifica se existe uma tarefa com o mesmo id antes de fazer a alteração
                var existingTarefa = _context.Tarefas.FirstOrDefault(f => f.TarefaId == id);

                if (existingTarefa == null)
                {
                    return NotFound("Tarefa não encontrada para alteração.");
                }

                // Realiza a alteração nos campos necessários
                existingTarefa.Titulo = tarefa.Titulo;
                existingTarefa.Descricao = tarefa.Descricao;
                existingTarefa.CategoriaId = tarefa.CategoriaId; // Adicionado para atualizar o ID da categoria

                _context.Tarefas.Update(existingTarefa);
                _context.SaveChanges();

                return Ok(existingTarefa);
            }
            catch (Exception e)
            {
                return BadRequest($"Ocorreu um erro ao tentar alterar a tarefa: {e.Message}");
            }
        
        }
        
    }

