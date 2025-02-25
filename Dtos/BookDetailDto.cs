﻿namespace BookAPI.Dtos
{
    public class BookDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PublicationYear { get; set; }
        public string AuthorName { get; set; }
        public int ViewsCount { get; set; }
        public double PopularityScore { get; set; }
        public int YearsSincePublished { get; set; }
    }
}
