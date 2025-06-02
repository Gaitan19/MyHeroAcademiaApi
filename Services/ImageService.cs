namespace MyHeroAcademiaApi.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _config;
        private readonly ILogger<ImageService> _logger;

        public ImageService(IWebHostEnvironment environment, IConfiguration config, ILogger<ImageService> logger)
        {
            _environment = environment;
            _config = config;
            _logger = logger;
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile, string entityType)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                    throw new ArgumentException("No image file provided");

                // Validar extensión
                var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                var allowedExtensions = _config.GetSection("ImageSettings:AllowedExtensions").Get<string[]>();

                if (string.IsNullOrEmpty(extension)
                    || !allowedExtensions.Contains(extension))
                {
                    throw new InvalidOperationException(
                        $"Invalid file extension '{extension}'. Allowed: {string.Join(", ", allowedExtensions)}");
                }

                // Validar tamaño
                var maxSizeMB = _config.GetValue<int>("ImageSettings:MaxFileSizeMB");
                if (imageFile.Length > maxSizeMB * 1024 * 1024)
                {
                    throw new InvalidOperationException(
                        $"File size {imageFile.Length / 1024 / 1024}MB exceeds limit of {maxSizeMB}MB");
                }

                // Determinar ruta
                var pathConfig = entityType switch
                {
                    "Hero" => _config["ImageSettings:HeroImagePath"],
                    "Villain" => _config["ImageSettings:VillainImagePath"],
                    _ => "Images/General"
                };

                var uploadPath = Path.Combine(_environment.WebRootPath, pathConfig);
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                // Generar nombre único
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return $"/{pathConfig}/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving image");
                throw;
            }
        }
    }
}
