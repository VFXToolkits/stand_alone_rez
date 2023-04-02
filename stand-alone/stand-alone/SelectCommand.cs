using Sharprompt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stand_alone
{
    class SelectCommand
    {
        public void rez_command()
        {
            var select_type = Prompt.Select<RezCommandType>("Select rez command:");

        }

        public void setup_project()
        {

        }
    }
}
