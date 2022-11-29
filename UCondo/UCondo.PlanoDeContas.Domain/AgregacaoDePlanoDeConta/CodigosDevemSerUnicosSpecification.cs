namespace UCondo.PlanoDeContas.Domain.AgregacaoDePlanoDeConta
{
    public static class CodigosDevemSerUnicosSpecification
    {
        public static bool IsNotSatisfiedBy(Receita pai, Receita filho)
        {
            return pai.Receitas.Any(r=> r.Codigo==filho.Codigo);
        }
        public static bool IsNotSatisfiedBy(Despesa pai, Despesa filho)
        {
            return pai.Despesas.Any(r => r.Codigo == filho.Codigo);
        }
    }
}
