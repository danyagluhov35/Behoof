namespace Behoof.Models.Product
{
    public class FoldProduct
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? ImageLink { get; set; }
        public string? CategoryName { get; set; }

        public int? BallDesign { get; set; }
        public int? BallBatery { get; set; }
        public int? BallCamera { get; set; }
        public int? BallAnswer { get; set; }
        public int? BallPortatable { get; set; }

        public int? SummBall { get; set; }
    }
}
