using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UCondo.PlanoDeContas.Domain.AgregacaoDePlanoDeConta;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UCondo.PlanoDeContas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanoDeContaController : ControllerBase
    {
        // GET: api/<PlanoDeCOntaController>
        [HttpGet]
        public async Task<IEnumerable<PlanoDeConta>> GetByFilter([FromServices]AgregacaoDeContaDbContext dbContext, [FromQuery]string filtro = "")
        {
            return await dbContext.PlanosDeConta
                .Where(planoDeConta => planoDeConta.Nome.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        // GET api/<PlanoDeCOntaController>/5
        [HttpGet("{codigo}")]
        public async Task<PlanoDeConta> Get([FromServices] AgregacaoDeContaDbContext dbContext, [FromRoute]string codigo)
        {
            return (PlanoDeConta)(await dbContext.Despesas.Include(d=> d.Despesas).SingleOrDefaultAsync(planoDeConta => planoDeConta.Codigo == codigo))??(await dbContext.Receitas.Include(r=> r.Receitas).SingleOrDefaultAsync(planoDeConta => planoDeConta.Codigo == codigo));
        }

        [HttpGet("{codigo}/proximo")]
        public async Task<IActionResult> Proximo([FromServices] AgregacaoDeContaDbContext dbContext, [FromRoute] string codigo, [FromServices] ServicoDeSugestaoDeCodigo servicoDeSugestaoDeCodigo)
        {
            var plano = await dbContext.PlanosDeConta.FindAsync(codigo);
            if (ContaQueAceitaLancamentoNaoPodeTerFilhoSpecification.IsNotSatisfiedBy(plano))
                return BadRequest("Funcionalidade exclusiva para codigos de contas que não acentam lançamentos");
            try
            {
                return Ok(servicoDeSugestaoDeCodigo.SugerirCodigo(plano, dbContext));
            }
            catch (ArgumentOutOfRangeException)
            {

                return BadRequest("O limite de itens foi excedido");
            }
            
        }


        // PUT api/<PlanoDeCOntaController>/5
        [HttpPut("{codigo}/despesas")]
        public async Task<IActionResult> Put(string codigo, [FromBody] Despesa despesa, [FromServices] AgregacaoDeContaDbContext dbContext)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var despesaExistente = await dbContext.Despesas.Include(d=> d.Despesas).SingleAsync(p => p.Codigo == codigo);

            if (CodigosDevemSerUnicosSpecification.IsNotSatisfiedBy(despesaExistente, despesa))
                return BadRequest("Códigos devems ser únicos");
            if(ContaQueAceitaLancamentoNaoPodeTerFilhoSpecification.IsNotSatisfiedBy(despesaExistente))
                return BadRequest("Conta que aceita lançamento não pode ter filho");
            if (CodigoDeContaFilhaDeveTerCodigoDaPaiComoPredecessoraSpecification.IsNotSatisfiedBy(codigo, despesa))
                return BadRequest("Código da conta não pertence a sequência");


            despesaExistente.Despesas.Add(despesa);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{codigo}/receitas")]
        public async Task<IActionResult> Put(string codigo, [FromBody] Receita receita, [FromServices] AgregacaoDeContaDbContext dbContext)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var receitaExistente = await dbContext.Receitas.Include(r=> r.Receitas).SingleAsync(p => p.Codigo == codigo);

            if (CodigosDevemSerUnicosSpecification.IsNotSatisfiedBy(receitaExistente, receita))
                return BadRequest("Códigos devems ser únicos");
            if (ContaQueAceitaLancamentoNaoPodeTerFilhoSpecification.IsNotSatisfiedBy(receitaExistente))
                return BadRequest("Conta que aceita lançamento não pode ter filho");
            if(CodigoDeContaFilhaDeveTerCodigoDaPaiComoPredecessoraSpecification.IsNotSatisfiedBy(codigo, receita))
                return BadRequest("Código da conta não pertence a sequência");

            receitaExistente.Receitas.Add(receita);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        // DELETE api/<PlanoDeCOntaController>/5
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Delete(string codigo, [FromServices] AgregacaoDeContaDbContext dbContext)
        {
            var plano = await dbContext.PlanosDeConta.FindAsync(codigo);
            dbContext.PlanosDeConta.Remove(plano);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
