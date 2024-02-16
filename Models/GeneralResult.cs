

namespace backend.Models
{
    public abstract class GeneralResult
    {
        private static readonly IReadOnlyCollection<string> EMPTY_LIST = new List<string>().AsReadOnly<string>();
        public IReadOnlyCollection<string> Errors { get; set; }

        protected GeneralResult(List<string> errors)
        {
            Errors = errors.AsReadOnly();
        }
        protected GeneralResult()
        {
            Errors = EMPTY_LIST;
        }
    }
}