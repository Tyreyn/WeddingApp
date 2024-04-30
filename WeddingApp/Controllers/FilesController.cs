using Microsoft.AspNetCore.Components.Forms;
using WeddingApp.Entities;

namespace WeddingApp.Controllers
{
    public class FilesController(SqlServerDataController sqlServerDataController)
    {
        private List<IBrowserFile> loadedFiles = new();
        private decimal progressPercent;
        private long maxFileSize = 15728640;
        private int maxAllowedFiles = 3;
        public string statusMessage;
        private List<string> pathtopictures = new();

        private SqlServerDataController sqlServerDataController { get; set; } = sqlServerDataController;

        public event Action<decimal> OnStateChange;

        public event Action<List<PictureEntity>> OnPictureLoad;

        public async Task UploadFiles(InputFileChangeEventArgs e, int userID)
        {
            loadedFiles.Clear();
            progressPercent = 0;

            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                try
                {
                    string randomFileName = Path.GetRandomFileName();
                    string originalFileExtension = Path.GetExtension(file.Name);
                    string newFileName = Path.ChangeExtension(randomFileName, originalFileExtension);

                    string path = Path.Combine("wwwroot", "Uploads", newFileName);
                    statusMessage = path;
                    await using FileStream writeStream = new(path, FileMode.Create);
                    using var readStream = file.OpenReadStream(maxFileSize);
                    var bytesRead = 0;
                    var totalRead = 0;
                    var buffer = new byte[1024 * 10];

                    while ((bytesRead = await readStream.ReadAsync(buffer)) != 0)
                    {
                        totalRead += bytesRead;
                        await writeStream.WriteAsync(buffer, 0, bytesRead);
                        progressPercent = Decimal.Divide(totalRead, file.Size);
                        this.NotifyStateChanged(progressPercent);
                    }

                    await this.sqlServerDataController.AddPictureToDatabase(
                        userID: userID,
                        picturePath: path);

                    Console.WriteLine(
                        $"Unsafe Filename: {file.Name}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("File: {FileName} Error: {Error}",
                        file.Name, ex.Message);
                }
            }
        }

        public async Task<List<PictureEntity>> LoadFiles()
        {
            return this.sqlServerDataController.GetAllPictures().Result;
        }
        private void NotifyStateChanged(decimal progressPercent)
        {
            this.OnStateChange?.Invoke(progressPercent);
        }

        private void NotifyLoadedPictures(List<PictureEntity> pictures)
        {
            this.OnPictureLoad?.Invoke(pictures);
        }

    }
}
