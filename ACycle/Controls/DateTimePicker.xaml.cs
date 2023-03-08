namespace ACycle.Controls
{
    public partial class DateTimePicker : ContentView
    {
        public static readonly BindableProperty DateTimeProperty = BindableProperty.Create(
                nameof(DateTime),
                typeof(DateTime),
                typeof(DateTimePicker),
                defaultValueCreator: (_) => DateTime.Now,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    DateTimePicker self = (DateTimePicker)bindable;
                    DateTime newDateTime = (DateTime)newValue;
                    self._datePicker.Date = newDateTime;
                    self._timePicker.Time = newDateTime.TimeOfDay;
                });

        public DateTime DateTime
        {
            get => (DateTime)GetValue(DateTimeProperty);
            set => SetValue(DateTimeProperty, value);
        }

        public DateTimePicker()
        {
            InitializeComponent();
        }

        private void OnDatePickerOrTimePickerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var name = e.PropertyName;

            if (name == nameof(_datePicker.Date))
            {
                DateTime = _datePicker.Date + DateTime.TimeOfDay;
            }
            else if (name == nameof(_timePicker.Time))
            {
                DateTime = DateTime.Date + _timePicker.Time;
            }
        }
    }
}
