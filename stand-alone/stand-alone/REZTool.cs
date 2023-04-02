using rez;

namespace stand_alone
{
    class REZTool
    {
        public REZRequest rez_request;
        public REZTool(string args) {
            rez_request = new REZRequest(args);
        }

        public void run_env()
        {
            rez_request.run_exec();
        }
    }
}
