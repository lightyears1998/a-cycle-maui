namespace ACycle.Helpers
{
    public static class FileHelper
    {
        public static async Task CopyAsync(string sourceFilePath, string destinationFilePath, bool overWrite = false)
        {
            const int bufferSize = 4096;
            FileMode writeMode = overWrite ? FileMode.Create : FileMode.CreateNew;

            using var sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan);
            using var destinationStream = new FileStream(destinationFilePath, writeMode, FileAccess.Write, FileShare.None, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan);
            await sourceStream.CopyToAsync(destinationStream);
        }
    }
}
