using Newtonsoft.Json;

namespace TheScientistAPI.Model
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Result { get; set; }
        [JsonIgnore]
        public List<string> Errors { get; set; }
    }
}