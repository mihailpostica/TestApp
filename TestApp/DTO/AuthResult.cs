using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.DTO.Responses
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public ICollection<string> Errors { get; set; }
        public AuthResult()
        {
            Errors = new List<String>();
        }
    }
    }

