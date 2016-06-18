
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gameBITS
{

    /**
     * Classe que define um par�metro a ser utilizado para caracterizar elemento em um artefato.
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
         * @param Nome do par�metro.
         */
        public gBParameter(string name) : base()
        {
            //Inicializa atributos
            this.name = name;
        }

        /**
         * M�todo de configura��o de nome do par�metro.
         * @param Configura nome de par�metro
         */
        public void SetName(String name)
        {
            this.name = name;
        }

        /**
         * M�todo de aquisi��o de nome do par�metro.
         * @return Retorna nome do par�metro.
         */
        public String GetName()
        {
            return this.name;
        }

        //******************************************************************
        // Atributos da classe *********************************************
        //******************************************************************

        /**
         * Nome do par�metro.
         */
        public String name;
    }
}