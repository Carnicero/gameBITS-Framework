
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gameBITS
{

    /**
     * Classe base que define um objeto dentro do contexto do sistema de geração.
     */
    public abstract class gBObject
    {

        /**
         * Construtor da classe.
         */
        public gBObject()
        {
            //Solicita novo ID único
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
         * Método responsável pela inicialização do objeto.
         */
        public abstract void Setup();

        /**
         * Método responsável pela finalização do objeto.
         */
        public abstract void Terminate();

        /**
         * Método de aquisição de ID único do objeto.
         * @return ID único do objeto.
         */
        public Guid GetID()
        {
            return this.id;
        }

        //******************************************************************
        // Atributos da classe *********************************************
        //******************************************************************

        /**
         * Tag de identificação do objeto.
         */
        public String tag;

        /**
         * ID global único.
         */
        private Guid id;

    }
}