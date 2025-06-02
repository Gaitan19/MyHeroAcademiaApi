namespace MyHeroAcademiaApi.Services
{
    // Services/ImageService.cs
    public class ImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _config;

        public ImageService(IWebHostEnvironment environment, IConfiguration config)
        {
            _environment = environment;
            _config = config;
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile, string entityType)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("No image file provided");

            // Validar extensión
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            var allowedExtensions = _config.GetSection("ImageSettings:AllowedExtensions").Get<string[]>();
            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException($"Invalid file extension. Allowed: {string.Join(", ", allowedExtensions)}");

            // Validar tamaño
            var maxSizeMB = _config.GetValue<int>("ImageSettings:MaxFileSizeMB");
            if (imageFile.Length > maxSizeMB * 1024 * 1024)
                throw new InvalidOperationException($"File size exceeds {maxSizeMB}MB limit");

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

            // Guardar imagen
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/{pathConfig}/{fileName}";
        }
    }
}
