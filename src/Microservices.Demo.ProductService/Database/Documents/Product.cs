using System;

namespace Microservices.Demo.ProductService.Database.Documents
{
    public class Product
    {
        public Product(string code, string name, string description)
        {
            this.Code = code;
            this.Name = name;
            this.Description = description;
        }

        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
