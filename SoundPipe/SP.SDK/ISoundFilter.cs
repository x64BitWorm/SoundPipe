namespace SP.SDK
{
    public interface ISoundFilter: ISoundProvider
    {
        public void Initialize(object[] args);
        public void SetHotValue(string key, object value);
        public object GetHotValue(string key);
        public void Destroy();
    }
}
