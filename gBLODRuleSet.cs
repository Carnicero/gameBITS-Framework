
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gameBITS
{

    /**
     * Classe que concentra o set de regras especificadas para um artefato.
     */
    public class gBLODRuleSet : gBLODSystem
    {

        /**
         * Construtor da classe.
         * @param Artefato concreto ao qual pertence o sistema de LOD.
         * @param Define se deve validar todas as regras ou se deve parar a validação após primeiro resultado positivo.
         */
        public gBLODRuleSet(gBConcrete concrete_artifact, bool validate_all_rules = true) : base(concrete_artifact)
        {
            //Inicializa lista de regras
            this.lod_rules = new Dictionary<string, gBLODRule>();
        }

        /**
         * Método responsável por disparar a validação de regras do conjunto de regras registradas.
         */
        public override void Update()
        {
            //Valida as regras de LOD registradas
            foreach (gBLODRule lod_rule in this.lod_rules.Values)
            {
                if (lod_rule.Validate(this.concrete_artifact) && this.validate_all_rules)
                {
                    break;
                }
            }
        }

        /**
         * Método de registro de regra relacionada ao controle de LOD do artefato.
         * @param alias Apelido da regra de lod.
         * @param lod_rule Regra de lod.
         * @return Retorna status da operação. Não é permitido duas regras de nome igual.
         */
        public bool AddRule(string alias, gBLODRule lod_rule)
        {
            //Verifica se já existe
            if (this.lod_rules.ContainsKey(alias))
            {
                return false;
            }

            this.lod_rules.Add(alias, lod_rule);

            return true;
        }

        /**
         * Método de remoção de regra.
         * @param alias Apelido da regra de lod.
         * @return Retorna status da operação.
         */
        public bool RemoveRule(string alias)
        {
            return this.lod_rules.Remove(alias);
        }

        /**
         * Método de seleção de regra.
         * @param alias Apelido da regra de lod.
         * @return Retorna regra selecionada, ou uma referência nula.
         */
        public gBLODRule SelectRule(string alias)
        {
            if (this.lod_rules.ContainsKey(alias))
            {
                return this.lod_rules[alias];
            }

            #if (gB_DEBUG)
                UnityEngine.Debug.Log("<color=red>gameBITS Framework: Regra '" + alias + "' não encontrada!</color>");
            #endif

            return null;
        }

        /**
         * Método de configuração de sistema de validação completo ou parcial.
         * @param Validar tudo, ou parar na primeira validação positiva.
         */
        public void ValidateAllRules(bool validate_all_rules)
        {
            this.validate_all_rules = validate_all_rules;
        }

        //******************************************************************
        // Atributos da classe *********************************************
        //******************************************************************

        /**
         * Lista de regras registradas.
         */
        private Dictionary<string, gBLODRule> lod_rules;

        /**
         * Flag que define se todas as regras devem ser validadas, ou se a validação deve ser interrompida na primeira validação positiva.
         */
        private bool validate_all_rules;

    }

}