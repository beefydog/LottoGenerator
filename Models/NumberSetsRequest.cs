namespace LottoGeneratorService.Models
{
    public class NumberGroup
    {
        public int min { get; set; }
        public int max { get; set; }
        public int numbersPerGroup { get; set; }
        public decimal divergence { get; set; }
        public bool sumCheck { get; set; }
        public bool oeCheck { get; set; }
    }

    public class SetsRequest
    {
        public List<NumberGroup> numberSet { get; set; }
        public int sets { get; set; }
    }
}
