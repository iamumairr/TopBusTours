namespace TopBusToursLondon_Model.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public Ticket Ticket { get; set; }
    }
}
