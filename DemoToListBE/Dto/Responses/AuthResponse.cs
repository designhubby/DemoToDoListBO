namespace DemoToListBE.Dto.Responses
{
    public record AuthResponse
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
