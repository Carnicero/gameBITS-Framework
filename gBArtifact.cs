using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gameBITS
{

    /**
     * Classe base que define artefatos gerados proceduralmente.
     */
    public abstract class gBArtifact : gBResource
    {

        /**
         * Construtor da classe.
         */
        public gBArtifact()
        {
        }

        /**
         * Método responsável pela adição de parâmetro.
         * @param Paraâmetro a ser adicionado ao artefato.
         * @param Apelido do parâmetro, usado para busca.
         * @return Retorna status da operação. Não é permitido dois parâmetros de nome igual.
         */
        public bool AddParameter(string alias, gBParameter parameter)
        {
            //Verifica se já existe
            if (this.parameters.ContainsKey(alias))
            {
                return false;
            }

            this.parameters.Add(alias, parameter);

            return true;
        }

        /**
         * Método de remoção de parâmetro.
         * @param Apelido do parâmetro, usado para busca.
         * @return Retorna status da operação.
         */
        public bool RemoveParameter(string alias)
        {
            return this.parameters.Remove(alias);
        }

        /**
         * Método de seleção de parâmetro.
         * @param Apelido do parâmetro, usado para busca.
         * @return Retorna parâmetro selecionado, ou uma referência nula.
         */
        public gBParameter SelectParameter(string alias)
        {
            if (this.parameters.ContainsKey(alias))
            {
                return this.parameters[alias];
            }
             
            #if (gB_DEBUG)
                UnityEngine.Debug.Log("<color=red>gameBITS Framework: Parâmetro '"+ alias + "' não encontrado!</color>");
            #endif

            return null;
        }

        //******************************************************************
        // Atributos da classe *********************************************
        //******************************************************************

        /**
         * Lista de parâmetros que descrevem o artefato.
         */
        protected Dictionary<string, gBParameter> parameters = new Dictionary<string, gBParameter>();

    }
}
