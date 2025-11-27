namespace SHOPDIENTU.Models
{
    public class CheckoutInfo
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

  
        public string ShippingMethod { get; set; }

        public string Province { get; set; }
        public string District { get; set; }
        public string StoreAddress { get; set; }

        public string DeliveryAddress { get; set; }
        public string Address { get; set; }
        public string PaymentMethod { get; set; }

    }
}
