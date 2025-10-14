namespace Paftax.Pafta.Drawing.Structs
{
    public struct ElementId(long value)
    {
        public long Value { get; set; } = value;
        public void Equals(ElementId other)
        {
            Value = other.Value;
        }
        public override readonly string ToString()
        {
            return Value.ToString();
        }
    }
}
