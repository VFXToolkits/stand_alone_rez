namespace rez
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new SystemException("error: argument COMMAND");
            }

            if (args[0] != "env") {
                throw new SystemException("error: Currently only [env] is supported");
            }

            var rez_instance = new REZRequest(string.Join(" ", args));
            rez_instance.run_exec();

        }
    }
}


