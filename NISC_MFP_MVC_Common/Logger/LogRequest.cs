namespace NISC_MFP_MVC_Common.Logger
{
    public class LogRequest
    {
        public LogRequest()
        {

        }
        public LogRequest(string newId, string newContent)
        {
            NewId = newId;
            NewContent = newContent;
        }
        public LogRequest(string oldId, string oldContent, string newId, string newContent)
        {
            OldId = oldId;
            OldContent = oldContent;
            NewId = newId;
            NewContent = newContent;
        }

        public string OldId { get; set; }
        public string OldContent { get; set; }
        public string NewId { get; set; }
        public string NewContent { get; set; }
    }
}
