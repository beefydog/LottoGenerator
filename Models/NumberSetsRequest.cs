namespace LottoGeneratorService.Models
{
    public sealed class NumberGroup
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public int NumbersPerGroup { get; set; }
        public decimal Divergence { get; set; }
        public bool SumCheck { get; set; }
        public bool OeCheck { get; set; }
    }

    public sealed class SetsRequest
    {
        public SetsRequest(List<NumberGroup> numberSet, int sets)
        {
            this.NumberSet = numberSet ?? throw new ArgumentNullException(nameof(numberSet));
            this.Sets = sets;
        }

        public List<NumberGroup> NumberSet { get; set; }
        public int Sets { get; set; }
    }
}
