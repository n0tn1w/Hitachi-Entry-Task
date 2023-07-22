namespace HitachiBE.Models.Request
{
    public class EmailDataRequest
    {
        public string sender { get; set; }   

        public string password { get; set; }

        public string receiver { get; set; }

        public IFormFile file { get; set; }
    }
}
