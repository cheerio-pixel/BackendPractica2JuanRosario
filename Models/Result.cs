

namespace backend.Models
{
    public class Result<T>
    : GeneralResult
    where T: class
    {
        public T? Value { get; set; }

        public Result(List<string> errors, T? value) : base(errors)
        {
            Value = value;
        }

        public Result(T? value) : base()
        {
            Value = value;
        }
    }
}