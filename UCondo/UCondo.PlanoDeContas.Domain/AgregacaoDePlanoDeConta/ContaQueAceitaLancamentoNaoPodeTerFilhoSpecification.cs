using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCondo.PlanoDeContas.Domain.AgregacaoDePlanoDeConta
{
    public static class ContaQueAceitaLancamentoNaoPodeTerFilhoSpecification
    {
        public static bool IsNotSatisfiedBy(PlanoDeConta planoDeConta)
        {
            return planoDeConta.AceitaLancamento;
        }
    }
}
