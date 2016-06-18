#undef gB_DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace gameBITS
{

    /**
     * Classe de gerenciamento dos recursos. Centraliza todas as referências de dados gerados a partir da estrutura do framework.
     */
    public class gBManager
    {

        /**
         * Construtor da classe.
         */
        public gBManager()
        {
            #if (gB_DEBUG)
                UnityEngine.Debug.Log("<color=green>gameBITS Framework: Gerenciador inicializado!</color>");
            #endif
        }

        /**
         * Destrutor da classe.
         */
        ~gBManager()
        {
            #if (gB_DEBUG)
                UnityEngine.Debug.Log("<color=green>gameBITS Framework: Gerenciador finalizado!</color>");
            #endif
        }

        /**
         * Método de integração com ciclo de atualização da engine, também responsável por atualizar os artefatos concretos. 
         * @param Tempo entre o último frame e o atual, fornecido pela engine. 
         */
        public void Update(float delta_time)
        {
            gBManager manager = gBManager.Instance;

            //Atualiza threads--------------------------------------------------------------------------------------------
            uint threads_running = 0;

            //Trava acesso à lista
            lock (gBManager.requests_lock)
            {

                //Verifica status, atualiza objeto de requisição e remove da lista em caso de operação concluída
                for (int request = manager.requests.Count - 1; request >= 0; request--)
                {
                    gBRequest request_object = manager.requests[request];

                    //UnityEngine.Debug.Log("<color=green>" + "thread.IsAlive = " + request_object.thread.IsAlive + "</color>");
                    //UnityEngine.Debug.Log("thread.ThreadState = " + request_object.thread.ThreadState);

                    //Verifica se está rodando
                    if (request_object.thread.IsAlive && (request_object.thread.ThreadState & ThreadState.Running) == ThreadState.Running)
                    {
                        //UnityEngine.Debug.Log("<color=blue>" + "ESTÁ RODANDO" + "</color>");
                        request_object.is_running = true;
                        request_object.is_done = false;
                        threads_running++;
                    }
                    //Verifica se finalizou
                    else if ((request_object.thread.ThreadState & ThreadState.Stopped) == ThreadState.Stopped || (request_object.thread.ThreadState & ThreadState.Aborted) == ThreadState.Aborted)
                    {
                        //UnityEngine.Debug.Log("<color=blue>" + "FINALIZOU" + "</color>");
                        request_object.is_running = false;
                        request_object.is_done = true;

                        request_object.thread.Join();

                        //Remove da lista de requisições
                        manager.requests.RemoveAt(request);
                    }
                }

                //UnityEngine.Debug.Log("<color=green>" + "manager.max_threads = " + manager.max_threads + "</color>");
                //UnityEngine.Debug.Log("<color=green>" + "threads_running = " + threads_running + "</color>");
                //UnityEngine.Debug.Log("<color=green>" + "manager.requests.Count = " + manager.requests.Count + "</color>");

                //Verifica se novas threads podem ser inicializadas
                foreach (gBRequest request in manager.requests)
                {
                    if (threads_running < manager.max_threads)
                    {
                        //UnityEngine.Debug.Log("<color=blue>" + "PODE RODAR MAIS THREADS" + "</color>");
                        if ((request.thread.ThreadState & ThreadState.Unstarted) == ThreadState.Unstarted)
                        {
                            //UnityEngine.Debug.Log("<color=blue>" + "THREAD NÂO INICIALIZADA" + "</color>");
                            try
                            {
                                //UnityEngine.Debug.Log("<color=blue>" + "START()" + request.GetID() + "</color>");
                                request.thread.Start();
                            }
                            catch (Exception e)
                            {
                                UnityEngine.Debug.LogError(e);
                            }
                            threads_running++;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

            }

            //------------------------------------------------------------------------------------------------------------

            //Atualiza delta_time
            manager.delta_time = delta_time;

            //Atualiza artefatos à partir do nodo raiz registrado
            if(manager.root_node != null)
            {
                //Cria lista de nodos a serem ignorados
                HashSet<Guid> ignore = new HashSet<Guid>();

                //Cria lista de nodos temporária
                List<gBConcrete> nodes = manager.GenerateNodeUpdateList(ref ignore);

                //Reconfigura flag de controle de alteração de hierarquia de nodos
                manager.node_hierarchy_changed = false;

                //Enquanto não atualizar todos os nodos
                while(nodes.Count > 0)
                {
                    //Inicia processo de atualização de nodos
                    nodes.Reverse();
                    int nodes_count = nodes.Count;
                    for (int current_node = nodes_count - 1; current_node >= 0; current_node--)
                    {
                        gBConcrete node = nodes[current_node];

                        //Adiciona nodo à lista de nodos a serem ignorados no caso de uma nova geração da lista de nodos
                        ignore.Add(node.GetID());

                        //Atualiza sistema de LOD do artefato, caso o artefato possua algum sistema de LOD atrelado
                        gBLODSystem lod_system = node.GetLODSystem();
                        if (lod_system != null)
                        {
                            lod_system.Update();
                        }
                        //Atualiza artefato
                        node.Update();

                        //Verifica flag de controle de alteração de hierarquia de nodos, se fora alterada, refaz a lista de nodos a serem atualizados
                        if(manager.node_hierarchy_changed == true)
                        {
                            //Gera lista de nodos novamente
                            nodes = manager.GenerateNodeUpdateList(ref ignore);

                            //Limpa flag
                            manager.node_hierarchy_changed = false;

                            //Sai do laço para recomeçar
                            break;
                        }
                        //Se não houve alteração, remove nodo da lista
                        else
                        {
                            nodes.RemoveAt(current_node);
                        }

                    }
                }

            }
        }

        /**
         * Método de sinalização de alteração de hierarquia de nodos.
         */
        public void NodeHierarchyChanged()
        {
            gBManager.Instance.node_hierarchy_changed = true;
        }

        /**
         * Método de uso interno para geração de lista de nodos a serem atualizados.
         * @param ignore_ids Lista de ids a serem ignorados na geração da lista.
         * @return Retorna uma lista de nodos a serem atualizados.
         */
        private List<gBConcrete> GenerateNodeUpdateList(ref HashSet<Guid> ignore)
        {
            //Copia lista para manipulação
            HashSet<Guid> temp_ignore = new HashSet<Guid>();
            foreach(Guid id in ignore)
            {
                temp_ignore.Add(id);
            }

            //Cria nova lista de nodos a serem atualizados
            List<gBConcrete> nodes = new List<gBConcrete>();
            if(gBManager.Instance.root_node != null)
            {
                gBManager.Instance.FindNodesToUpdate(gBManager.Instance.root_node, ref nodes, ref temp_ignore);
            }

            return nodes;
        }

        /**
         * Método recursivo de busca e adição de nodos à lista fornecida.
         * @param nodes_list Lista de nodos.
         * @param ignore_ids ID's de nodos a serem ignorados.
         */
        private void FindNodesToUpdate(gBConcrete node, ref List<gBConcrete> nodes, ref HashSet<Guid> ignore)
        {
            //Vasculha os nodos filhos de artefato
            List<gBConcrete> child_nodes = node.GetChildNodes();
            foreach (gBConcrete child_node in child_nodes)
            {
                gBManager.Instance.FindNodesToUpdate(child_node, ref nodes, ref ignore);
            }

            //Busca nodo na lista de ids a serem ignorados
            foreach(Guid ignore_id in ignore)
            {
                if(ignore_id == node.GetID())
                {
                    ignore.Remove(ignore_id);
                    return;
                }
            }

            //Se não for ignorado, adiciona na lista
            nodes.Add(node);
        }

        /**
         * Define um artefato concreto como nodo raiz.
         * @param Artefato a ser definido como nodo raiz.
         */
        public void SetRootNode(gBConcrete node)
        {
            //Adiciona nodo na lista de nodos raiz
            gBManager.Instance.root_node = node;
            if (node != null)
            {
                node.SetParentNode(null);
            }
        }

        /**
         * Pega nodo raiz.
         * @return Retorna artefato nodo.
         */
        public gBConcrete GetRootNode()
        {
            return gBManager.Instance.root_node;
        }

        /**
         * Método de inicialização das configurações do gerenciador.
         */
        private void Setup()
        {
            gBManager manager = gBManager.Instance;

            //Inicializa atributos
            manager.delta_time = 0;
            manager.max_threads = 2;

            //Inicializa nodo raiz
            manager.root_node = null;

            //Inicializa mapa de armazenamento de ferramentas
            manager.tools = new Dictionary<String, gBTool>();

            //Inicializa lista de requisições assíncronas
            manager.requests = new List<gBRequest>();
        }

        /**
         * Método de atribuição de novo ID único.
         * @param Recipiente a receber o novo ID.
         */
        public void NewID(ref Guid id)
        {
            lock(gBManager.ids_lock)
            {
                id = Guid.NewGuid();
            }
        }

        /**
         * Método de registro de ferramenta pelo uso de "apelido" único.
         * @param tool Referência da ferrramenta.
         * @param alias Apelido a ser usado.
         */
        public bool AddTool(String alias, gBTool tool)
        {
            gBManager manager = gBManager.Instance;

            bool temp_return;

            lock(gBManager.tools_lock)
            {
                if (manager.tools.ContainsKey(alias))
                {
                    temp_return = false;
                }
                else
                {
                    manager.tools.Add(alias, tool);
                    temp_return = true;
                }
            }

            return temp_return;
        }

        /**
         * Método de remoção de registro de ferramenta pelo "apelido" único.
         * @param alias Apelido da ferramenta.
         * @return Retorna status da operação.
         */
        public bool RemoveTool(String alias)
        {
            gBManager manager = gBManager.Instance;

            bool temp_return;

            lock (gBManager.tools_lock)
            {
                temp_return = manager.tools.Remove(alias);
            }

            return temp_return;
        }

        /**
         * Método de remoção de registro de ferramentas.
         */
        public void RemoveAllTools()
        {
            gBManager manager = gBManager.Instance;

            lock (gBManager.tools_lock)
            {
                manager.tools.Clear();
            }
        }

        /**
         * Método de seleção de ferramenta pelo "apelido" único.
         * @param alias Apelido da ferramenta.
         */
        public gBTool SelectTool(String alias)
        {
            gBManager manager = gBManager.Instance;

            gBTool temp_return = null;

            lock (gBManager.tools_lock)
            {
                if (manager.tools.ContainsKey(alias))
                {
                    temp_return = manager.tools[alias];
                }
            }

            return temp_return;
        }

        /**
         * Método de aquisição de instância única.
         * @return Retorna instância única de si.
         */
        public static gBManager Instance
        {
            get
            {
                if (gBManager.instance == null)
                {
                    gBManager.instance = new gBManager();

                    //Realiza a chamada do método de configurações iniciais do gerenciador
                    gBManager.instance.Setup();
                }
                return gBManager.instance;
            }
        }

        /**
         * Método de destruição de instância única.
         */
        public bool Destroy()
        {
            if (gBManager.instance == null)
            {
                return false;
            }
            else
            {
                gBManager.instance.Terminate();
                gBManager.instance = null;
                return true;
            }
        }

        /**
         * Método de aquisição de delta time.
         * @return Retorna valor de último delta time registrado.
         */
        public float GetDeltaTime()
        {
            return gBManager.Instance.delta_time;
        }

        /**
         * Método chamado na finalização da engine.
         */
        private void Terminate()
        {
            //Limpar threads
            //Trava acesso à lista
            lock (gBManager.requests_lock)
            {
                foreach(gBRequest request in this.requests)
                {
                    request.thread.Abort();
                }
            }

            //Limpa nodos
            this.SetRootNode(null);

            //Limpa tools
            this.RemoveAllTools();
        }

        /**
         * Método chamado para configurar o número máximo de threads simultâneas.
         * @param Número máximo de threads a correr simultâneamente.
         */
        public void SetMaxThreads(uint max_threads)
        {
            gBManager manager = gBManager.Instance;

            manager.max_threads = max_threads;
        }

        /**
         * Método de aquisição do número máximo de threads simultâneas.
         * @return Número máximo de threads.
         */
        public uint GetMaxThreads()
        {
            gBManager manager = gBManager.Instance;

            return manager.max_threads;
        }

        /**
         * Método chamado para registrar uma nova requisição. Uma thread é gerada e registrada na requisição. A requisição é inserida na lista conforme prioridade.
         * @param Objeto de requisição.
         */
        public void RegisterRequest(gBRequest request)
        {
            gBManager manager = gBManager.Instance;

            //Trava acesso à lista
            lock (gBManager.requests_lock)
            {
                if (manager.requests.Count > 0)
                {
                    //Busca posição correta na lista de threads conforme prioridade
                    for (int position = 0; position < manager.requests.Count; position++)
                    {
                        //Se prioridade da requisição consultada for menor, encaixa na frente dela
                        if (manager.requests[position].priority < request.priority)
                        {
                            //Registra requisição e thread
                            manager.requests.Insert(position, request);
                            break;
                        }
                        //Se for última e não encontrou outra requisição de menos prioridade, adiciona ao fim da lista
                        else if (position == manager.requests.Count - 1)
                        {
                            //Registra requisição e thread
                            manager.requests.Add(request);
                            break; 
                        }
                    }
                }
                else
                {
                    //Registra requisição e thread
                    manager.requests.Add(request);
                }
            }
        }

        /**
         * Método de aquisição do número atual de threads em andamento.
         * @return Número atual de threads rodando.
         */
        public uint GetCurrentRunningThreads()
        {
            gBManager manager = gBManager.Instance;

            uint threads_running = 0;

            //Trava acesso à lista
            lock(gBManager.requests_lock)
            {
                foreach (gBRequest request in manager.requests)
                {
                    //Verifica se está rodando
                    if (request.is_running)
                    {
                        threads_running++;
                    }
                }
            }

            return threads_running;
        }

        /**
         * Método de aquisição do número total de threads na fila.
         * @return Número atual de threads na fila.
         */
        public uint GetCurrentThreads()
        {
            gBManager manager = gBManager.Instance;

            uint threads = 0;

            //Trava acesso à lista
            lock (gBManager.requests_lock)
            {
                threads = (uint)manager.requests.Count;
            }

            return threads;
        }

        //******************************************************************
        // Atributos da classe *********************************************
        //******************************************************************

        /**
         * Registro de ferramentas pelo uso de apelidos.
         */
        private Dictionary<String, gBTool> tools;

        /**
         * Artefato que representa nodo raiz.
         */
        private gBConcrete root_node;

        /**
         * Referência para instância única.
         */
        private static gBManager instance;

        /**
         * Tempo entre o último frame e o atual, informado pela engine no processo de atualização.
         */
        private float delta_time;

        /**
         * Número máximo de threads executadas simultaneamente.
         */
        private uint max_threads;

        /**
         * Threads em curso.
         */
        private List<gBRequest> requests;

        /**
         * Flag que sinaliza se estrutura de nodos foi alterada.
         */
        private bool node_hierarchy_changed = false;

        /**
         * Para controle de threads.
         */
        private static object requests_lock = new Object();
        private static object tools_lock = new Object();
        private static object ids_lock = new Object();
    }

}