using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gameBITS
{

    /**
     * Classe base que define recursos gerados proceduralmente.
     */
    public abstract class gBResource : gBObject
    {

        /**
         * Construtor da classe.
         */
        public gBResource()
        {
        }

        /**
         * Método de configuração da semente inicial utilizada pelo gerador.
         * @param Semente a ser gravada.
         */
        public virtual void SetSeed(ulong seed)
        {
            this.seed = seed;
        }

        /**
         * Método de aquisição da semente inicial utilizada pelo gerador.
         * @return Retorna semente.
         */
        public virtual ulong GetSeed()
        {
            return this.seed;
        }

        //******************************************************************
        // Atributos da classe *********************************************
        //******************************************************************

        /**
         * Semente usada na geração procedural de recursos.
         */
        protected ulong seed;

    }
}
