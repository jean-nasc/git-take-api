using System;

namespace Api.ViewModels
{
    public class GitTakeViewModel
    {
        public string avatar_url { get; set; }
        public string full_name { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; }
    }
}