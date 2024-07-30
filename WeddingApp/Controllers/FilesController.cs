using Microsoft.AspNetCore.Components.Forms;
using WeddingAppBL.Repository;
using WeddingAppDTO.DataTransferObject;

namespace WeddingApp.Controllers
{
    public class FilesController(PictureRepository pictureOperations)
    {
        private decimal progressPercent;
        private long maxFileSize = 15728640;
        private int maxAllowedFiles = 3;
        public string statusMessage;
        private List<string> pathtopictures = new();

        public event Action<decimal> OnStateChange;

        public event Action<List<Picture>> OnPictureLoad;

        private PictureRepository PictureOperations { get; set; } = pictureOperations;

        public async Task UploadFiles(IReadOnlyList<IBrowserFile> e, int userID)
        {
            progressPercent = 0;

            foreach (var file in e)
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

                    await this.PictureOperations.AddPictureToDatabase(
                        userID: userID,
                        pathToPicture: path);

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

        public async Task DeletePicture(string pathToPicture)
        {
            await this.PictureOperations.DeletePicture(pathToPicture);
        }

        public async Task<List<Picture>> LoadFiles()
        {
            return this.PictureOperations.GetAllPictures().Result;
        }

        private void NotifyStateChanged(decimal progressPercent)
        {
            this.OnStateChange?.Invoke(progressPercent);
        }

    }
}
