namespace DemoToListBE.Dto.Responses
{
    public record RegistrationResponse: AuthResponse
    {
        public DateTime Expiry { get; set; }
    }
}
