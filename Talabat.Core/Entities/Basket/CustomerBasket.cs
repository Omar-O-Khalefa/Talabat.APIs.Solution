﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Basket
{
    public class CustomerBasket
    {
        public string Id { get; set; } = null!;
        public List<BasketItem> Items { get; set; }
        public string? PayemntIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public decimal  ShippingPrice { get; set; }
        public int? DeliveryMethodId { get; set; }
        public CustomerBasket(string id)
        {
            Id = id;
            Items = new List<BasketItem>();
        }

    }
}
