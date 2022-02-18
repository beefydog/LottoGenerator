namespace LottoGeneratorService.Models
{
    public class NumberParam
    {
        public int min { get; set; }
        public int max { get; set; }
        public int numbers { get; set; }
        public int spreadpercent { get; set; }
    }

    public class SetsRequest
    {
        public List<NumberParam> numberParams { get; set; }
        public int sets { get; set; }
    }
}
