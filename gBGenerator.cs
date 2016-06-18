
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace gameBITS
{

    /**
     * Classe base que define um gerador procedural de recursos.
     */
    public abstract class gBGenerator : gBTool
    {

        /**
         * Construtor da classe.
         */
        public gBGenerator()
        {
        }

        /**
         * M�todo de gera��o de recurso com configura��o personalizada com base no modelo recebido.
         * @param Recurso a gerado conforme configura��es descritas por ele.
         */
        public abstract void Generate(gBResource resource);

        //******************************************************************
        // Atributos da classe *********************************************
        //******************************************************************

    }

}