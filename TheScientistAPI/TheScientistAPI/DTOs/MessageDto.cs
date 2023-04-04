using TheScientistAPI.Model;

namespace TheScientistAPI.DTOs
{
    public class MessageDto
    {
        public int PaperId { get; set; }
        public string Text { get; set; }
    }
    public class MessageResponseDto
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public string User { get; set; }
    }

    public static partial class MessageExtension
    {
        public static MessageResponseDto AsDto(this Message message)
        {
            return new MessageResponseDto
            {
                Id = message.Id,
                Text = message.Text,
                User = message.User.UserName
            };
        }
    }
}
