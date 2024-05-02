using CBT3_Domain.Errors;

namespace CBT_Infrastructure.Services
{
    public class FileService
    {
        public async Task<Result<bool>> IsFileExists(string filePath)
        {
            try
            {
                string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string fullPath = Path.Combine(wwwRootPath, filePath.TrimStart('/'));
                bool result = File.Exists(fullPath);

            
                return Result<bool>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure<bool>(DomainErrors.SystemError.FileExistsError);
            }
        }
    }

}
