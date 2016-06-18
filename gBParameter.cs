
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gameBITS
{

    /**
     * Classe que define um parâmetro a ser utilizado para caracterizar elemento em um artefato.
     */
    public abstract class gBParameter : gBResource
    {

        /**
         * Construtor da classe.
         */
        public gBParameter()
        {
        }

        /**
         * Construtor da classe.
         * @param Nome do parâmetro.
         */
        public gBParameter(string name) : base()
        {
            //Inicializa atributos
            this.name = name;
        }

        /**
         * Método de configuração de nome do parâmetro.
         * @param Configura nome de parâmetro
         */
        public void SetName(String name)
        {
            this.name = name;
        }

        /**
         * Método de aquisição de nome do parâmetro.
         * @return Retorna nome do parâmetro.
         */
        public String GetName()
        {
            return this.name;
        }

        //******************************************************************
        // Atributos da classe *********************************************
        //******************************************************************

        /**
         * Nome do parâmetro.
         */
        public String name;
    }
}