namespace iChat.Models {
    public class DirectMessage : Message {
        public int ReceiverId { get; set; }
        public User Receiver { get; set; }
    }
}