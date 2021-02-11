using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test
{
    public class RegisterRequestPayload
    {
        public string email { get; set; }
        public string password { get; set; }
        public string password2 { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

    }
}
