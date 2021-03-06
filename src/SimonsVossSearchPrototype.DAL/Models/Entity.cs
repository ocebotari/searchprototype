﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimonsVossSearchPrototype.DAL.Models
{
    public abstract class Entity<TKey>
    {
        [JsonProperty(Order = 0)]
        public TKey Id { get; set; }        
    }
}
