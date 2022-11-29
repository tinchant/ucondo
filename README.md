# ucondo

como eu não tinha aqui um sql server que pudesse usar acabei usando o entity framework no modo in memory pra ser mais simples.

para execução basta baixar e executar a restauração 

eu criei uma separação entre api e dominio mas claramente devido a simplicidade não era necessario nenhuma outra camada

no dominio quis exemplificar a specification pattern e o uso da unitOfWorkPattern ao invés da repository pattern para melhor se adequar ao EF

eu optei também por usar da herança para garantir alguns requisitos quanto a tipagem de objetos simplificando o código
