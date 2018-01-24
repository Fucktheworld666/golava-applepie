namespace GoLava.ApplePie.Components
{
    public struct ProcessOutput
    {
        public ProcessOutput(string data, bool isError)
        {
            this.Data = data;
            this.IsError = isError;
        }

        public string Data { get; set; }

        public bool IsError { get; set; }
    }
}