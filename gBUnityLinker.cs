#undef gB_DEBUG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace gameBITS
{

    /**
     * Classe responsável por prover a ligação entre a Unity e o Framework.
     */
    public class gBUnityLinker : MonoBehaviour
    {

        void Awake()
        {
            #if (gB_DEBUG)
                UnityEngine.Debug.Log("<color=green>gameBITS Framework: Inicializando gerenciador!</color>");
            #endif

            //Inicializa de gerenciador
            this.gB_manager = gameBITS.gBManager.Instance;
        }

        void Update()
        {
            //Interliga o motor ao framework
            if (this.is_update_enabled)
            {
                this.gB_manager.Update(Time.deltaTime);
            }

            //Mostra ou esconde Debug de estrutura de nodos
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                this.par_show_node_structure = !this.par_show_node_structure;
            }
        }

        void OnDestroy()
        {
            //Avisa framework da finalização
            gameBITS.gBManager.Instance.Destroy();


        }

        void OnGUI()
        {
            #if (gB_DEBUG)
            {
                if (this.par_show_node_structure)
                {
                    GUILayout.BeginArea(new Rect(10, 10, 700, Screen.height - 20), GUI.skin.box);
                    GUILayout.Label("* Nodos Registrados:");
                    this.par_scroll_position = GUILayout.BeginScrollView(this.par_scroll_position);
                    {
                        //Pega lista de nodos raiz do gerenciador
                        gBConcrete root_node = this.gB_manager.GetRootNode();

                        if (root_node != null)
                        {
                            DebugChildNodes(root_node, 0);
                        }
                    }
                    GUILayout.EndScrollView();
                    GUILayout.EndArea();
                }
            }
            #endif
        }

        /**
         * Imprime na tela a sub-estrutura de nodos do nodo informado.
         */
        void DebugChildNodes(gBConcrete node, uint level)
        {
            if ( GUILayout.Button((new string(' ', (int)level * 4)) + "Type: " + node.GetType().ToString() + " / ID: " + node.GetID().ToString(), GUILayout.ExpandWidth(false)) )
            {
                Debug.Log("<color=blue>Parent ID: " + (node.GetParentNode()!=null? node.GetParentNode().GetID().ToString():"null") + "</color>");
            }

            //Imprime nodos filhos
            List<gBConcrete> nodes = node.GetChildNodes();
            foreach (gBConcrete child_node in nodes)
            {
                DebugChildNodes(child_node, level+1);
            }
        }

        /**
         * Habilita ciclo de atualização do gerenciador.
         */
        public void Enable()
        {
            this.is_update_enabled = true;
        }

        /**
         * Desabilita ciclo de atualização do gerenciador.
         */
        public void Disable()
        {
            this.is_update_enabled = false;
        }

        //******************************************************************
        // Atributos da classe *********************************************
        //******************************************************************

        /**
         * Referência local do gerenciador do Framework.
         */
        private gameBITS.gBManager gB_manager;

        /**
         * Referência local do gerenciador do scrollView.
         */
        private Vector2 par_scroll_position = new Vector2();

        /**
         * Mostra ou esconde scrollView de Debug
         */
        private bool par_show_node_structure = true;

        /**
         * Define se deve executar ciclo de atualização do gerenciador.
         */
        private bool is_update_enabled = true;
    }
}