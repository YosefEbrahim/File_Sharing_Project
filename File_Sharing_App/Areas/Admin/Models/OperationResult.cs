namespace File_Sharing_App.Areas.Admin.Models
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public static OperationResult NotFound(string msg="Item Not Found")
        {
            return new OperationResult
            {
                Success = false,
                Message = msg
            };
        }
        public static OperationResult Succeded(string msg = "Operation Completed successfully")
        {
            return new OperationResult
            {
                Success = true,
                Message = msg
            };
        }
    }
}
