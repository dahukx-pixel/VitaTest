namespace VitaTest.AppCore
{
    public class AppSettings
    {
        public string DbAddress { get; set; } = string.Empty;
        public string DbName { get; set; } = string.Empty;
        public string DbLogin { get; set; } = string.Empty;
        public string DbPassword { get; set; } = string.Empty;
        public bool LocalDb { get; set; }

        public AppSettings()
        {

        }
    }
}
