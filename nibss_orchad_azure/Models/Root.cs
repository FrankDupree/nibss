using System;

namespace nibss_orchad_azure.Models
{
    
    public class Description
    {
        public string Html { get; set; }
    }

    public class Experience
    {
        public string Text { get; set; }
    }

    public class Location
    {
        public string Text { get; set; }
    }

    public class MinQ
    {
        public string Text { get; set; }
    }

    public class Role
    {
        public string Text { get; set; }
    }

    public class DeadLine
    {
        public DateTime Value { get; set; }
    }

    public class JobOpenings
    {
        public Description Description { get; set; }
        public Experience Experience { get; set; }
        public Location Location { get; set; }
        public MinQ MinQ { get; set; }
        public Role Role { get; set; }
        public DeadLine DeadLine { get; set; }
    }

    public class TitlePart
    {
        public string Title { get; set; }
    }

    public class Root
    {
        public string ContentItemId { get; set; }
        public string ContentItemVersionId { get; set; }
        public string ContentType { get; set; }
        public string DisplayText { get; set; }
        public bool Latest { get; set; }
        public bool Published { get; set; }
        public DateTime ModifiedUtc { get; set; }
        //public DateTime PublishedUtc { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string Owner { get; set; }
        public string Author { get; set; }
        public JobOpenings JobOpenings { get; set; }
        public TitlePart TitlePart { get; set; }
    }

}
