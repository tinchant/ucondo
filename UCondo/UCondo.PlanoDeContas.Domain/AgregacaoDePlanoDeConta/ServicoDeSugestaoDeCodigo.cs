using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCondo.PlanoDeContas.Domain.AgregacaoDePlanoDeConta
{
    public class ServicoDeSugestaoDeCodigo
    {
        public string SugerirCodigo(PlanoDeConta plano, AgregacaoDeContaDbContext dbContext)
        {
            if (plano.Tipo == nameof(Receita))
            {
                var receita = (Receita)plano;
                dbContext.Entry(receita)
                    .Collection(r => r.Receitas)
                    .Load();
                try
                {
                    var proximo = ObterProximo(plano.Codigo, receita.Receitas.Select(r => r.Codigo), dbContext);
                    return proximo;
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw;
                }
                catch (SystemException ex)
                {
                    var novoPai = dbContext.PlanosDeConta.Find(ex.Message);
                    return SugerirCodigo(novoPai, dbContext);
                }
            }
            else
            {
                var despesa = (Despesa)plano;
                dbContext.Entry(despesa)
                    .Collection(r => r.Despesas)
                    .Load();
                try
                {
                    var proximo = ObterProximo(plano.Codigo, despesa.Despesas.Select(r => r.Codigo), dbContext);
                    return proximo;
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw;
                }
                catch (SystemException ex)
                {
                    var novoPai = dbContext.PlanosDeConta.Find(ex.Message);
                    return SugerirCodigo(novoPai, dbContext);
                }
                
            }
        }

        private string ObterProximo(string pai,IEnumerable<string> enumerable, AgregacaoDeContaDbContext dbContext)
        {
            if (!enumerable.Any())
            {
                return pai + ".1";
            }

            var maiorCodigo = enumerable.Max();
            var partes = maiorCodigo.Split(".");
            var ultimaParte = partes.Last();
            
            int parse = int.Parse(ultimaParte);
            if (parse < 999)
            {
                var contas = dbContext.PlanosDeConta.ToList();
                var lista = partes.ToList();
                bool invalido = true;
                string cod = string.Empty;
                while (invalido)
                {
                    lista.Remove(lista.Last());
                    parse++;
                    lista.Add(parse.ToString());
                    cod = string.Join(".", lista);
                    invalido = contas.Any(x => x.Codigo == cod);
                }
                return cod;
            }
            if (partes.Length ==2)
            {
                throw new ArgumentOutOfRangeException();
            }
            throw new SystemException(string.Join(".", partes.SkipLast(2)));
        }
    }
}
