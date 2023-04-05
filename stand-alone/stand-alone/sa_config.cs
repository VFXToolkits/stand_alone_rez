
namespace stand_alone
{
    static class SAConfig
    {
        public static string sa_version = "0.0.1";
    }


    public enum InputCommandType
    {
        rez_command,
        setup_project,
        local_build,
        project_info,
        quit
    }

    public enum RezCommandType { 
    
        bind,
        build,
        env,
        config,
        context,
        complete,
        cp,
        depends,
        diff,
        forward,
        help,
        interpret,
        plugins,
        release,
        search,
        status,
        test,
        view,
        bundle

    }

    public enum LocalBuildSeting
    {
        rez_env_to_local,
        rez_env_to_local_all,
        cp_python_path_to_local
    }
}
