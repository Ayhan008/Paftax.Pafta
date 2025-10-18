namespace Paftax.Pafta.Drawing.Entities.Abstracts
{
    public abstract class Annotation : Entity
    {
        public double Scale { get; set; }
        public double Rotation { get; set; }
        public string SheetNumber { get; set; } = "A101";
        public string DetailNumber { get; set; } = "1";
    }
}
