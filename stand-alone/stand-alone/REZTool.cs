﻿using rez;

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

        public Dictionary<string, string> get_rez_run_env()
        {
            Dictionary<string, string> env = new Dictionary<string, string>();

            ApiResponse temp_env = rez_request.GetRezEnv();
            Console.WriteLine($"INFO:{temp_env.msg}");
            if ((!temp_env.status))
            {
                return env;
            }
            env = temp_env.data;
            return env;

        }
    }
}
