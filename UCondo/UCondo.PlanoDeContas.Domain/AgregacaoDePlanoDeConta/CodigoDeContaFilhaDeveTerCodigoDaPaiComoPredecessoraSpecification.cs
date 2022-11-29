namespace UCondo.PlanoDeContas.Domain.AgregacaoDePlanoDeConta
{
    public static class CodigoDeContaFilhaDeveTerCodigoDaPaiComoPredecessoraSpecification
    {
        public static bool IsNotSatisfiedBy(string codigoPai, PlanoDeConta planoDeConta)
        {
            return !planoDeConta.Codigo.StartsWith(codigoPai);
        }
    }
}
