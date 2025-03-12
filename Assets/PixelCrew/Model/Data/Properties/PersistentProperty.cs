namespace PixelCrew.Model.Data.Properties
{
    //[Serializable]
    public abstract class PersistentProperty<TPropertyType> : ObservableProperty<TPropertyType>
    {
        protected TPropertyType _stored; //что что будет записано непосредственно на самом диске
        
        private TPropertyType _defaultValue;
        
        //public event OnPropertyChanged OnChanged;
        
        public PersistentProperty(TPropertyType defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public override TPropertyType Value
        {
            get => _stored;
            set
            {
                //мы проверяем (сравниваем значения), если они одинаковые, то мы выходим, а если нет, то записываем и вызываем ивент
                //Делаем мы это в целях производительности, чтобы постоянно не дергать метод Write
                var isEquals = _stored.Equals(value);
                if(isEquals) return;

                var oldValue = _stored;
                Write(value);
                _stored = _value = value;
                
                //OnChanged?.Invoke(value, oldValue);
                InvokeOnChangedEvent(value, oldValue);
            }
        }

        //в этом методе мы просто прочитаем значение из нашего хранилища(стораджа)
        public void Init()
        {
            _stored = _value = Read(_defaultValue);
        }
        
        protected abstract void Write(TPropertyType value);
        protected abstract TPropertyType Read(TPropertyType defaultValue);
        
        public void Validate()
        {
            //если то что у нас на диске не равно, что у нас в инспекторе, мы перезапишем и на диск в переменную сторед( диск + инспектор)
            if (!_stored.Equals(_value))
                Value = _value;
        }
    }
}