namespace SHOPDIENTU.Models
{
    public class UserOrder
    {
        public string CustomerName { get; set; }  
        public string Phone { get; set; }       
        public string Address { get; set; }        
        public string PaymentMethod { get; set; }

        public decimal Total { get; set; }
    }
}

