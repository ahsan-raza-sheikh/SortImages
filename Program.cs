using System.Drawing;
using System.Drawing.Imaging;
using ImageMagick;

Console.Write("Enter the source directory path: ");
var sourceDirectory = Console.ReadLine();
Console.Write("Enter the target directory path: ");
var targetDirectory = Console.ReadLine();

if (!Directory.Exists(sourceDirectory))
{
    Console.WriteLine("Source directory does not exist.");
    return;
}

if (!Directory.Exists(targetDirectory))
{
    if (targetDirectory != null) _ = Directory.CreateDirectory(targetDirectory);
}

string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".heic" };

var imageFiles = Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories)
    .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower()))
    .ToArray();

foreach (var filePath in imageFiles)
{
    try
    {
        DateTime dateTaken;
        var fileExtension = Path.GetExtension(filePath).ToLower();

        if (fileExtension == ".heic")
        {
            dateTaken = GetDateTakenFromHeic(filePath) ?? File.GetLastWriteTime(filePath);
        }
        else
        {
            dateTaken = GetDateTakenFromImage(filePath) ?? File.GetLastWriteTime(filePath);
        }

        var year = dateTaken.Year.ToString();
        var month = dateTaken.ToString("MMMM");

        var yearMonthDirectory = Path.Combine(targetDirectory, year, month);

        if (!Directory.Exists(yearMonthDirectory))
        {
            Directory.CreateDirectory(yearMonthDirectory);
        }

        var fileName = Path.GetFileName(filePath);
        var destinationPath = Path.Combine(yearMonthDirectory, fileName);

        File.Copy(filePath, destinationPath, true);
        Console.WriteLine($"Copied {fileName} to {yearMonthDirectory}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error processing {filePath}: {ex.Message}");
    }
}

DateTime? GetDateTakenFromImage(string path)
{
    try
    {
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var myImage = Image.FromStream(fs, false, false);
        if (myImage.PropertyIdList.Contains(36867))
        {
            PropertyItem? propItem = myImage.GetPropertyItem(36867);
            var dateTaken = System.Text.Encoding.UTF8.GetString(propItem.Value);
            dateTaken = dateTaken.Trim('\0');
            return DateTime.ParseExact(dateTaken, "yyyy:MM:dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error extracting date from {path}: {ex.Message}");
    }
    return null;
}

DateTime? GetDateTakenFromHeic(string path)
{
    try
    {
        using var image = new MagickImage(path);
        var exifProfile = image.GetExifProfile();
        var dateTimeOriginal = exifProfile?.GetValue(ExifTag.DateTimeOriginal);
        if (dateTimeOriginal != null)
        {
            return DateTime.ParseExact(dateTimeOriginal.Value, "yyyy:MM:dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error extracting date from {path}: {ex.Message}");
    }
    return null;
}
