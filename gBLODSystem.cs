using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gameBITS
{

    /**
     * Classe base para sistemas de LOD de um artefato.
     */
    public abstract class gBLODSystem
    {

        /**
         * Construtor da classe.
         * @param Artefato concreto ao qual pertence o sistema de LOD.
         */
        public gBLODSystem(gBConcrete concrete_artifact)
        {
            //Guarda referência de artefato concreto dono do sistema de LOD 
            this.concrete_artifact = concrete_artifact;
        }

        /**
         * Método responsável por disparar a atualização do sistema de LOD.
         */
        public abstract void Update();

        //******************************************************************
        // Atributos da classe *********************************************
        //******************************************************************

        /**
         * Referência do artefato concreto ao qual se aplicam as regras.
         */
        public gBConcrete concrete_artifact;

    }

}