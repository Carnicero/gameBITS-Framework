using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gameBITS
{

    /**
     * Classe base que define artefatos concretos gerados proceduralmente.
     */
    public abstract class gBConcrete : gBArtifact
    {

        /**
         * Construtor da classe.
         */
        public gBConcrete()
        {
        }

        /**
         * M�todo respons�vel pela atualiza��o do artefato.
         */
        public abstract void Update();

        /**
         * M�todo respons�vel pela aquisi��o do sistema de LOD do artefato.
         * @return Retorna o sistema de LOD do artefato.
         */
        public gBLODSystem GetLODSystem()
        {
            return this.lod_system;
        }

        /**
         * M�todo respons�vel pela adi��o de artefato abstrato.
         * @param Artefato abstrato a ser adicionado ao artefato.
         * @param Apelido do artefato abstrato, usado para busca.
         * @return Retorna status da opera��o. N�o pode haver dois artefatos com mesmo apelido.
         */
        public bool AddAbstractArtifact(string alias, gBAbstract abstract_artifact)
        {
            //Verifica se j� existe
            if (this.abstract_artifacts.ContainsKey(alias))
            {
                return false;
            }

            this.abstract_artifacts.Add(alias, abstract_artifact);

            return true;
        }

        /**
         * M�todo de remo��o de artefato abstrato.
         * @param Apelido do recurso, usado para busca.
         * @return Retorna status da opera��o.
         */
        public bool RemoveAbstractArtifact(string alias)
        {
            return this.abstract_artifacts.Remove(alias);
        }

        /**
         * M�todo de sele��o de artefato abstrato.
         * @param Apelido do artefato abstrato, usado para busca.
         * @return Retorna artefato abstrato selecionado, ou uma refer�ncia nula.
         */
        public gBAbstract SelectAbstractArtifact(string alias)
        {
            if (this.abstract_artifacts.ContainsKey(alias))
            {
                return this.abstract_artifacts[alias];
            }

            return null;
        }

        /**
         * M�todo de adi��o de nodos filhos ao artefato.
         * @param Nodo a ser adicionado como filho.
         * @return Retorna status da oper��o.
         */
        public bool AddChildNode(gBConcrete child_node)
        {
            if (this != child_node)
            {
                //Adiciona nodo na lista de nodos filhos
                this.child_nodes.Add(child_node);

                //Notifica gerenciador que estrutura de nodos fora alterada
                gBManager.Instance.NodeHierarchyChanged();

                child_node.parent_node = this;
                child_node.OnParentNodeSet(this);

                this.OnChildNodeAdd(child_node);

                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * M�todo de remo��o de nodo filho do artefato.
         * @param Nodo a ser removido da lista de nodos filhos.
         * @return Retorna o status da opera��o.
         */
        public bool RemoveChildNode(gBConcrete child_node)
        {
            if (this.child_nodes.Remove(child_node))
            {

                //Notifica gerenciador que estrutura de nodos fora alterada
                gBManager.Instance.NodeHierarchyChanged();

                child_node.parent_node = null;
                child_node.OnParentNodeSet(null);

                this.OnChildNodeRemove(child_node);


                return true;
            }

            return false;
        }

        /**
         * M�todo de aquisi��o de lista de nodos filhos do artefato.
         * @return Retorna lista de nodos filhos do artefato.
         */
        public List<gBConcrete> GetChildNodes()
        {
            return this.child_nodes;
        }

        /**
         * M�todo de configura��o do nodo pai do artefato.
         * @param Nodo a ser definido como pai do artefato.
         * @return Retorna status da opera��o. Um nodo n�o pode ser pai de si mesmo.
         */
        public bool SetParentNode(gBConcrete parent_node)
        {
            if (this != parent_node)
            {
                //Remove nodo de lista de pai atual, se este n�o for nulo
                if(this.parent_node != null)
                {
                    if(!this.parent_node.RemoveChildNode(this))
                    {
                        return false;
                    }
                }

                //Adiciona nodo em lista de nodo pai, se este n�o for nulo
                if (parent_node != null)
                {
                    if (!parent_node.AddChildNode(this))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        /**
         * M�todo de aquisi��o do nodo pai do artefato.
         * @return Retorna nodo pai do artefato.
         */
        public gBConcrete GetParentNode()
        {
            return this.parent_node;
        }

        /**
         * M�todo virtual chamado automaticamente no sucesso do ato de defini��o de um nodo pai.
         */
        public virtual void OnParentNodeSet(gBConcrete parent_node)
        {
        }

        /**
         * M�todo virtual chamado automaticamente no sucesso do ato de adi��o de um nodo filho.
         */
        public virtual void OnChildNodeAdd(gBConcrete child_node)
        {
        }

        /**
         * M�todo virtual chamado automaticamente no sucesso do ato de remo��o de um nodo filho.
         */
        public virtual void OnChildNodeRemove(gBConcrete child_node)
        {
        }

        /**
         * M�todo de aquisi��o do nodo filho correspondente ao ID informado.
         * @param id ID referente ao nodo filho.
         * @return Retorna nodo correspondente ao ID solicitado ou retorna null caso n�o encontre.
         */
        public gBConcrete FindChildNodeByID(Guid id)
        {
            //Busca por nodo que cont�m o ID
            foreach (gBConcrete node in this.child_nodes)
            {
                if (node.GetID() == id)
                {
                    return node;
                }
            }

            return null;
        }

        /**
         * M�todo de aquisi��o do nodo filho correspondente � tag informada.
         * @param tag Tag referente ao nodo filho.
         * @return Retorna nodo correspondente � tag solicitada ou retorna null caso n�o encontre.
         */
        public gBConcrete FindChildNodeByTag(string tag)
        {
            //Busca por nodo que cont�m a tag
            foreach (gBConcrete node in this.child_nodes)
            {
                if (node.tag == tag)
                {
                    return node;
                }
            }

            return null;
        }

        /**
         * M�todo de aquisi��o de lista de nodos filhos correspondente � tag informada.
         * @param tag Tag referente aos nodos filhos.
         * @return Retorna lista de nodos correspondentes � tag solicitada ou retorna null caso n�o encontre.
         */
        public List<gBConcrete> FindChildNodesByTag(string tag)
        {
            List<gBConcrete> node_list = new List<gBConcrete>();

            //Busca por nodo que cont�m a tag
            foreach (gBConcrete node in this.child_nodes)
            {
                if (node.tag == tag)
                {
                    node_list.Add(node);
                }
            }

            return node_list;
        }


        //******************************************************************
        // Atributos da classe *********************************************
        //******************************************************************

        /**
         * Conjunto de regras para controle de n�vel de detalhe.
         */
        protected gBLODSystem lod_system = null;

        /**
         * Mapa de artefatos abstratos que comp�em o artefato concreto.
         */
        protected Dictionary<string, gBAbstract> abstract_artifacts = new Dictionary<string, gBAbstract>();

        /**
         * Nodos filhos.
         */
        private List<gBConcrete> child_nodes = new List<gBConcrete>();

        /**
         * Nodo pai.
         */
        private gBConcrete parent_node = null;

    }

}