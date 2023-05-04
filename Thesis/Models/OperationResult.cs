using Microsoft.AspNetCore.Identity;

namespace Thesis.Models
{
    public class OperationResult
    {
        public bool success { get; set; }
        public string text { get; set; }
        public List<string> data { get; set; }

        public OperationResult()
        {

        }

        public OperationResult(IdentityResult result)
        {
            success = result.Succeeded;
            if (result.Errors.Count() > 0)
            {
                text = result.Errors.FirstOrDefault().Description;
            }
        }
    }
}
