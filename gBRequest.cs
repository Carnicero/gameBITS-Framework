using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace gameBITS
{
    public class gBRequest
    {

        /**
         * Construtor da classe.
         * @param source Fonte da requisição.
         * @param target Destino da requisição.
         * @param resource Objeto da requisição.
         * @param is_async Define se operação deve ser realizada de forma assíncrona.
         * @param priority Prioridade da thread na lista em relação às outras. A prioridade não pode ser alterada posteriormente.
         */
        public gBRequest(gBResource source, gBGenerator target, gBResource resource, bool is_async, int priority = 0)
        {
            this.source = source;
            this.target = target;
            this.resource = resource;
            this.is_async = is_async;
            this.priority = priority;

            this.is_done = false;
            this.is_running = false;

            //Verifica se requisição deve ser feita sincronamente ou assíncronamente
            //Assíncrona
            if (is_async)
            {
                //Cria thread onde rodará a requisição
                this.thread = new Thread(() => target.Generate(resource) );
                this.thread.IsBackground = true;
                //this.thread.Priority = ThreadPriority.Lowest;

                //Registra requisição no gerenciador
                gBManager.Instance.RegisterRequest(this);
            }
            //Síncrona
            else
            {
                this.is_running = true;
                target.Generate(resource);
                this.is_running = false;
                this.is_done = true;
            }
        }

        /**
         * Método que verifica se operação está em curso. 
         * @return Retorna se operação está rodando. 
         */
        public bool IsRunning()
        {
            return this.is_running;
        }

        /**
         * Método que verifica se operação foi concluída. 
         * @return Retorna se operação foi concluída. 
         */
        public bool IsDone()
        {
            return this.is_done;
        }

        //******************************************************************
        // Atributos da classe *********************************************
        //******************************************************************

        /**
         * Recurso a ser gerado.
         */
        public gBResource resource;

        /**
         * Descreve se operação foi concluída.
         */
        public bool is_done;

        /**
         * Descreve se operação está em andamento.
         */
        public bool is_running;

        /**
         * Flag que sinaliza se requisição deve ser feita de forma assíncrona.
         */
        public bool is_async;

        /**
         * Thread para requisição assíncrona.
         */
        public Thread thread;

        /**
         * Origem da requisição.
         */
        public gBResource source;

        /**
         * Destino da requisição.
         */
        public gBGenerator target;

        /**
         * Prioridade da requisição.
         */
        public int priority;
    }
}
