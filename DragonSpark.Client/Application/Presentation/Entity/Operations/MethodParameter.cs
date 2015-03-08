using DragonSpark.Application.Presentation.ComponentModel;

namespace DragonSpark.Application.Presentation.Entity.Operations
{
    public class MethodParameter : ViewAwareObject
    {
        public string ParameterName
        {
            get { return parameterName; }
            set { SetProperty( ref parameterName, value, () => ParameterName ); }
        }	string parameterName;

        public object Value
        {
            get { return valueField; }
            set { SetProperty( ref valueField, value, () => Value ); }
        }	object valueField;
    }
}