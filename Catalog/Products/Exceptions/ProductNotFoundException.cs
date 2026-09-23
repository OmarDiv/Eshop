using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Products.Exceptions
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(string message) : base(message)
        {
        }
        public ProductNotFoundException(Guid key) : base("Product", key)
        {
        }
    }
}
