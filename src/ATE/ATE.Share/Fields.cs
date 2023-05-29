namespace ATE.Share
{
	public static class Fields
    {
		private static string AppName => "ProcessAutmation";

		private static string DataFolderName => "Data";

		private static string MainDataFileName => "data.db";

		private static string LogFolderName => "Log";

        public static string AppPath
		{
			get  {
				string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);
				return path;
            }
		}

        public static string LogFilePath
        {
            get
            {
                string path = Path.Combine(AppPath, LogFolderName);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
				string filepath = Path.Combine(path, $"{DateTime.Today.ToShortDateString().Replace("/", "-")}.log");
                return filepath;
            }
        }

        public static string LocalDbFilePath
		{
			get {
				string dataFolderPath = Path.Combine(AppPath, DataFolderName);
				if (!Directory.Exists (dataFolderPath))
					Directory.CreateDirectory(dataFolderPath);
				return Path.Combine(dataFolderPath, MainDataFileName);
			}
		}


	}
}
