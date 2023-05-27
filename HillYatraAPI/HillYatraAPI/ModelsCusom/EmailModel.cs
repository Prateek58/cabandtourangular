using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HillYatraAPI.ModelsCusom
{
    public class EmailModel
    {
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string EmailToCC { get; set; }
        public string EmailToFirstName { get; set; }
        public string EmailToLastName { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
    }

   
}
