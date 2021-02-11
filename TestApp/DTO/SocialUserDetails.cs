﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.DTO
{
    public class SocialUserDetails
    {

        [JsonProperty("first_name")]
            public string FirstName { get; set; }

            [JsonProperty("last_name")]
            public string LastName { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }
        }
 
    }

