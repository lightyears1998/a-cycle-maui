namespace ACycle.DataStructures
{
    public class BidirectionalMap<TForward, TReverse>
        where TForward : notnull
        where TReverse : notnull
    {
        private readonly Dictionary<TForward, TReverse> _forward = new();
        private readonly Dictionary<TReverse, TForward> _reverse = new();

        public TReverse this[TForward forwardKey]
        {
            get
            {
                return _forward[forwardKey];
            }
            set
            {
                _forward.Remove(forwardKey);
                _forward[forwardKey] = value;
            }
        }

        public TForward this[TReverse reverseKey]
        {
            get
            {
                return _reverse[reverseKey];
            }
            set
            {
                _reverse.Remove(reverseKey);
                _reverse[reverseKey] = value;
            }
        }

        public void Clear()
        {
            _forward.Clear();
            _reverse.Clear();
        }
    }
}
