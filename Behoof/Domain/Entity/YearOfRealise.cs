﻿namespace Behoof.Domain.Entity
{
    public class YearOfRealise
    {
        public string Id { get; set; }
        public DateTime? DateTime { get; set; }
        public List<Product>? Product { get; set; }
    }
}

