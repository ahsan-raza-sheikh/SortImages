# SortImages

## Overview

SortImages is a C# console application that organizes image files from a source directory into a target directory, categorizing them by the year and month they were taken. This tool supports various image formats and extracts date information from the image's EXIF data.

## Features

- Supports multiple image formats: `.jpg`, `.jpeg`, `.png`, `.bmp`, `.gif`, `.tiff`, and `.heic`.
- Organizes images into directories based on the year and month they were taken.
- Automatically creates necessary directories if they do not exist.
- Handles exceptions and errors gracefully.

## Requirements

- .NET Framework
- [ImageMagick](https://imagemagick.org/index.php) library for handling HEIC files.

## Installation

1. Clone the repository:
   ```shell
   git clone https://github.com/ahsan-raza-sheikh/SortImages.git
   ```

2. Navigate to the project directory:
   ```shell
   cd SortImages
   ```

3. Build the project:
   ```shell
   dotnet build
   ```

## Usage

1. Run the application:
   ```shell
   dotnet run
   ```

2. Enter the source directory path when prompted.

3. Enter the target directory path when prompted.

The application will then process all images in the source directory and subdirectories, copying them to the target directory organized by year and month.

## Code Explanation

The main functionality is implemented in `Program.cs`:

- Prompts the user for source and target directory paths.
- Verifies the existence of the source directory and creates the target directory if necessary.
- Retrieves a list of image files from the source directory.
- For each image, it extracts the date taken from EXIF data or the file's last write time.
- Organizes images into directories named by year and month.
- Copies the images to the appropriate directories in the target location.

Helper functions:
- `GetDateTakenFromImage`: Retrieves the date taken from the EXIF data of an image.
- `GetDateTakenFromHeic`: Retrieves the date taken from the EXIF data of a HEIC file.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgments

- ImageMagick for handling HEIC files.
- [EXIF Tags](https://exiftool.org/TagNames/EXIF.html) for EXIF data reference.
