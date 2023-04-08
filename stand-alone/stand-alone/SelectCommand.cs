using Sharprompt;


namespace stand_alone
{
    class SelectCommand
    {
        EnvBuildCommand envBuildCommand = new EnvBuildCommand();
        public void rez_command()
        {
            var select_type = Prompt.Select<RezCommandType>("Select rez command:");

        }

        public void setup_project()
        {

        }

        public void local_build()
        {
            var select_type = Prompt.Select<LocalBuildSeting>("Select local Build command:");
            switch (select_type)
            {
                case LocalBuildSeting.rez_env_to_local:
                    envBuildCommand.build_rez_env_to_local();
                    string? save_path = Prompt.Input<string>("Input rez env save path");
                    string? app_name = Prompt.Input<string>("Input start app name");
                    if (save_path != null)
                    {
                        envBuildCommand.export_env_to_path(save_path, app_name);
                    }
                    else {
                        Console.WriteLine("Error: path not null");
                    }

                    break;

                case LocalBuildSeting.rez_env_to_local_all:
                    envBuildCommand.build_all_rez_env_to_local();
                    break;

                case LocalBuildSeting.cp_python_path_to_local:
                    envBuildCommand.cp_python_path_to_local();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }


        }

        public void project_info()
        {

        }


    }

    class EnvBuildCommand
    {
        private REZTool? rez_instance;
        private Dictionary<string, string>? get_current_env;
        private Dictionary<string, string>? get_window_env;
        private Dictionary<string, string>? get_linux_env;

        private string template_path = Path.Combine(Environment.CurrentDirectory, "template");

        public void build_rez_env_to_local()
        {
            Prompt.ColorSchema.Answer = ConsoleColor.DarkYellow;
            string? rez_env_args = Prompt.Input<string>("Input rez env");
            if (rez_env_args == null)
            {
                Console.WriteLine("You must enter a package name");
                return;
            }

            rez_instance = new REZTool(rez_env_args);
            get_window_env = rez_instance.get_windows_env();
            get_linux_env = rez_instance.get_linux_env();

        }

        public void build_all_rez_env_to_local() { 

            
        
        }

        public void export_env_to_path(string export_path, string app_name) {
            export_path = export_path.Replace("\\", "/");

            string windows_content = File.ReadAllText(Path.Combine(template_path, "dcc_env/run_cmd.bat"));
            string linux_content = File.ReadAllText(Path.Combine(template_path, "dcc_env/run_cmd.sh"));

            if (get_window_env != null)
            {
                _export_file($"{export_path}/{app_name}.bat", windows_content, app_name, get_window_env, "SET", ";", "%PATH%");
            }


            if (get_linux_env != null)
            {
                _export_file($"{export_path}/{app_name}.sh", linux_content, app_name, get_linux_env, "export", ";", "$PATH");
            }
        }

        private void _export_file(string save_path, string content, string app_name, Dictionary<string, string> env_key, string args_1, string args_2, string path_env)
        {
            string content_env = "";
            foreach (var item in env_key)
            {
                if (item.Key == "PATH")
                {
                    content_env += $"{args_1} {item.Key}={item.Value}\n";
                }
                else
                {
                    content_env += $"{args_2} PATH={item.Value};{path_env}\n";
                }

            }
            content = content.Replace("{{rez_env_body}}", content_env).Replace("{{run_app}}", app_name);
            File.WriteAllText(save_path, content);
        }

        public void cp_python_path_to_local()
        {

        }
    }
}
