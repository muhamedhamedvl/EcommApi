﻿namespace WebApiEcomm.Core.Entites.Basket
{
    public class BasketItems
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image {  get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }

    }
}