
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gameBITS
{

    /**
     * Classe base para definição de regra para controle de LOD.
     */
    public abstract class gBLODRule
    {

        /**
         * Construtor da classe.
         */
        public gBLODRule()
        {
            this.Setup();
        }

        /**
         * Método responsável pela inicialização do objeto.
         */
        public abstract void Setup();

        /**
         * Método de validação de regra de LOD.
         * @param artifact Artefato alvo da validação da regra.
         * @return Retorna status de validação da regra.
         */
        public abstract bool Validate(gBConcrete concrete_artifact);

        /**
         * Método de ativação manual de regra de LOD.
         * @param concrete_artifact Artefato alvo da validação da regra.
         */
        public abstract void Execute(gBConcrete concrete_artifact);
    }

}