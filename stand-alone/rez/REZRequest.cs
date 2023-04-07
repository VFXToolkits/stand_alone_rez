using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;


namespace rez
{
    public class REZRequest
    {
        public string rez_command_str;
        public string? rez_exec;

        private string json_path = Path.Combine(Environment.CurrentDirectory, "rez_connect_config.json");
        private string? get_current_url;
        private string join_env_str = ";";

        protected readonly RestClient? http_client;

        public REZRequest(string rez_command) {
            if (rez_command.Contains(" -- "))
            {
                var rez_args_list = rez_command.Split(" -- ");
                rez_command_str = rez_args_list[0];
                rez_exec = rez_args_list[1];
            }
            else {
                rez_command_str = rez_command;
            }

            load_url_config();

            if (get_current_url == null)
            {
                Console.WriteLine("Failed to read configuration file");
            }
            else
            {
                http_client = new RestClient(get_current_url);
            }

        }

        public bool run_exec() {
            if (rez_exec == null)
            {
                return direct_run();
            }

            return run_sb_env();
        }

        private bool run_sb_env() {
            ApiResponse rez_env_req = GetRezEnv();
            Console.WriteLine($"INFO:{rez_env_req.msg}");
            if (!rez_env_req.status)
            {
                return false;
            }
            SetCurrentEnv(rez_env_req.data);

            while (true) {
                string? user_input = Console.ReadLine();
                if (user_input == null) {
                    Console.WriteLine("Need to enter the program to start");
                    continue;
                }
                if(user_input == "exit")
                {
                    Console.WriteLine("Bye");
                    break;
                }

                Process.Start(user_input);
            }

            return true;
        }

        private void SetCurrentEnv(Dictionary<string, string> new_env) {
            foreach (var item in new_env)
            {
                if (item.Value == "PATH")
                {
                    Environment.SetEnvironmentVariable("PATH", item.Value + join_env_str + Environment.GetEnvironmentVariable("PATH"));
                }
                else
                {
                    Environment.SetEnvironmentVariable(item.Key, item.Value);
                }
            }
            
        }

        private bool direct_run() {
            ApiResponse rez_env_req = GetRezEnv();
            Console.WriteLine($"INFO:{rez_env_req.msg}");
            if (rez_env_req.status)
            {
                ProcessStartInfo startinfo = new ProcessStartInfo();

                foreach (var item in rez_env_req.data) {
                    if (item.Value == "PATH") {
                        startinfo.EnvironmentVariables["PATH"] = item.Value + join_env_str + Environment.GetEnvironmentVariable("PATH");
                    }
                    else
                    {
                        startinfo.EnvironmentVariables[item.Key] = item.Value;
                    }
                }
                Process process = new Process();
                process.StartInfo = startinfo;
                if (rez_exec.Contains(" "))
                {
                    var args_list = rez_exec.Split(' ');
                    process.StartInfo.FileName = args_list[0];
                    process.StartInfo.Arguments = args_list[1];
                }
                else
                {
                    process.StartInfo.FileName = rez_exec;
                }
                process.Start();
               
                return true;
            }
            
            return false;
        }

        private void load_url_config() {
            if (File.Exists(json_path))
            {
                var config_field = new ConfigField();
                config_field = JsonConvert.DeserializeObject<ConfigField>(File.ReadAllText(json_path));
                if (config_field != null)
                {
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    {
                        get_current_url = config_field.windows_http_url;
                        join_env_str = ";";
                    }
                    else {
                        get_current_url = config_field.linux_http_url;
                        join_env_str = ":";
                    }
                }
            }

        }

        public ApiResponse GetRezEnv()
        {
            var request = new RestRequest("env");
            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(rez_command_str), ParameterType.RequestBody);
            RestResponse response = http_client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
            }
            else{
                return new ApiResponse()
                {
                    data = { },
                    status = false,
                    msg = response.ErrorMessage
                };
            }

        }
    
    }

    public class ConfigField
    {
        public string windows_http_url { get; set; }
        public string linux_http_url { get; set; }

        public string mac_http_url { get; set; }

    }

    public class ApiResponse { 
        public Dictionary<string, string> data { get; set; }
        public bool status { get; set; }
        public string? msg { get; set; }
    }
}
