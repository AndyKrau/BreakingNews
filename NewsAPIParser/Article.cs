﻿namespace NewsAPIParser
{
    public class Article
    {
        public string ArticleId { get; set; } = null!;
        public string? Source { get; set; }
        public string? Author { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? UrlToImage { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string? Content { get; set; }
    }
}
