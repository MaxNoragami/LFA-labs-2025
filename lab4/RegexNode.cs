namespace lab4
{
    class RegexNode
    {
        public RegexNodeType Type { get; set; }
        public string Value { get; set; } = "";
        public List<RegexNode> Children { get; set; } = new List<RegexNode>();
        public int MinRepeat { get; set; }
        public int MaxRepeat { get; set; }
        
        public RegexNode(RegexNodeType type)
        {
            Type = type;
        }
    }
}