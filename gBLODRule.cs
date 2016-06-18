
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gameBITS
{

    /**
     * Classe base para defini��o de regra para controle de LOD.
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
         * M�todo respons�vel pela inicializa��o do objeto.
         */
        public abstract void Setup();

        /**
         * M�todo de valida��o de regra de LOD.
         * @param artifact Artefato alvo da valida��o da regra.
         * @return Retorna status de valida��o da regra.
         */
        public abstract bool Validate(gBConcrete concrete_artifact);

        /**
         * M�todo de ativa��o manual de regra de LOD.
         * @param concrete_artifact Artefato alvo da valida��o da regra.
         */
        public abstract void Execute(gBConcrete concrete_artifact);
    }

}