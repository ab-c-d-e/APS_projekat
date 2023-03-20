using TheScientistAPI.Model;

namespace TheScientistAPI.DTOs
{
    public class ScientificPaperEditMessage
    {
        public string EditorName { get; set; }
        public ScientificPaperMemento PreviousState { get; set; }
        public ScientificPaperMemento NewState { get; set; }
    }

    public class ScientificPaperMemento
    {
        public string Title { get; set; }
        public string Abstract { get; set; }
        public List<Section> Sections { get; set; }
        public List<Keyword> Keywords { get; set; }
        public ScientificPaperMemento(ScientificPaper scientificPaper)
        {
            Title = scientificPaper.Title;
            Abstract = scientificPaper.Abstract;
            Keywords = scientificPaper.Keywords;
            Sections = scientificPaper.Sections;
        }
    }
}
