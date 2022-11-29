using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace UCondo.PlanoDeContas.Domain.AgregacaoDePlanoDeConta
{
    [KnownType(typeof(Receita)), KnownType(typeof(Despesa))]
    public abstract class PlanoDeConta
    {
        [Key, RegularExpression(@"[12](.\d{1,3})*", ErrorMessage ="Códigos devem ser compostos por numerais até 999 e precedidos de ponto final")]
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public bool AceitaLancamento { get; set; }
        public string Tipo { get { return GetType().Name; } }


    }
    
    public class Receita : PlanoDeConta
    {
        public virtual ICollection<Receita> Receitas { get; set; } = new List<Receita>();

    }
    public class Despesa : PlanoDeConta
    {
        public virtual ICollection<Despesa> Despesas { get; set; } = new List<Despesa>();

    }
}
