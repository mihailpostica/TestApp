using System.Collections.Generic;

namespace Test
{
    public class ValidationError
    {
        public string Field { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}