
namespace backend.Models
{
    public class ResultValue<T> 
    : GeneralResult
    where T : struct
    {
        public T? Value { get; set; }

        public ResultValue(List<string> errors, T? value) : base(errors)
        {
            Value = value;
        }

        public ResultValue(T? value) : base()
        {
            Value = value;
        }
    }
}