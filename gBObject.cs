
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gameBITS
{

    /**
     * Classe base que define um objeto dentro do contexto do sistema de gera��o.
     */
    public abstract class gBObject
    {

        /**
         * Construtor da classe.
         */
        public gBObject()
        {
            //Solicita novo ID �nico
            gBManager.Instance.NewID(ref this.id);

            this.Setup();
        }

        /**
         * Destrutor da classe.
         */
        ~gBObject()
        {
            this.Terminate();
        }

        /**
         * M�todo respons�vel pela inicializa��o do objeto.
         */
        public abstract void Setup();

        /**
         * M�todo respons�vel pela finaliza��o do objeto.
         */
        public abstract void Terminate();

        /**
         * M�todo de aquisi��o de ID �nico do objeto.
         * @return ID �nico do objeto.
         */
        public Guid GetID()
        {
            return this.id;
        }

        //******************************************************************
        // Atributos da classe *********************************************
        //******************************************************************

        /**
         * Tag de identifica��o do objeto.
         */
        public String tag;

        /**
         * ID global �nico.
         */
        private Guid id;

    }
}