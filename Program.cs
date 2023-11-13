public static class Explorer
{
    private static int selectedItemIndex;
    private static FileSystemInfo[] items;

    public static void ExploreDrives()
    {
        int selectedDriveIndex = 0;
        DriveInfo[] drives = DriveInfo.GetDrives();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Выберите диск:");

            for (int i = 0; i < drives.Length; i++)
            {
                Console.WriteLine($"{(i == selectedDriveIndex ? " ->" : "   ")} {GetDriveName(drives[i])}");
            }

            ConsoleKeyInfo driveKey = Console.ReadKey();

            if (driveKey.Key == ConsoleKey.Escape)
            {
                break;
            }

            if (driveKey.Key == ConsoleKey.UpArrow && selectedDriveIndex > 0)
            {
                selectedDriveIndex--;
            }
            else if (driveKey.Key == ConsoleKey.DownArrow && selectedDriveIndex < drives.Length - 1)
            {
                selectedDriveIndex++;
            }
            else if (driveKey.Key == ConsoleKey.Enter)
            {
                ExploreDrive(drives[selectedDriveIndex].RootDirectory);
            }
        }
    }

    private static void ExploreDrive(DirectoryInfo directory)
    {
        selectedItemIndex = 0;
        items = directory.GetFileSystemInfos();

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Папка: {directory.FullName}");

            Console.WriteLine("-----------------------------------------------------------------------------");
            Console.WriteLine("| Имя файла                | Дата создания        | Тип файла               |");
            Console.WriteLine("-----------------------------------------------------------------------------");

            for (int i = 0; i < items.Length; i++)
            {
                Console.WriteLine($"|{(i == selectedItemIndex ? " ->" : "   ")} {GetFileName(items[i]),-25} | {items[i].CreationTime,-20} | {GetFileType(items[i]),-25} |");
            }

            Console.WriteLine("-----------------------------------------------------------------------------");

            ConsoleKeyInfo exploreKey = Console.ReadKey();

            if (exploreKey.Key == ConsoleKey.Escape)
            {
                return;
            }

            int totalItems = items.Length;

            if (exploreKey.Key == ConsoleKey.UpArrow && selectedItemIndex > 0)
            {
                selectedItemIndex--;
            }
            else if (exploreKey.Key == ConsoleKey.DownArrow && selectedItemIndex < totalItems - 1)
            {
                selectedItemIndex++;
            }
            else if (exploreKey.Key == ConsoleKey.Enter)
            {
                FileSystemInfo selectedItem = items[selectedItemIndex];

                if (selectedItem is DirectoryInfo selectedDirectory)
                {
                    ExploreDrive(selectedDirectory);
                }
                else if (selectedItem is FileInfo selectedFile)
                {
                    OpenFile(selectedFile);
                }
            }
        }
    }

    private static void OpenFile(FileInfo file)
    {
        Console.Clear();
        Console.WriteLine($"Открываем файл: {file.FullName}");

        try
        {
            System.Diagnostics.Process.Start(file.FullName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при открытии файла: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
        }

        Console.WriteLine("Для продолжения нажмите любую клавишу...");
        Console.ReadKey();
    }

    private static string GetFileName(FileSystemInfo info)
    {
        return info.Name;
    }

    private static string GetFileType(FileSystemInfo info)
    {
        return info is DirectoryInfo ? "[Папка]" : "[Файл]";
    }

    private static string GetDriveName(DriveInfo drive)
    {
        return $"{drive.Name} - {FormatBytes(drive.AvailableFreeSpace)} свободно из {FormatBytes(drive.TotalSize)}";
    }

    private static string FormatBytes(long bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
        int suffixIndex = 0;
        double size = bytes;

        while (size >= 1024 && suffixIndex < suffixes.Length - 1)
        {
            size /= 1024;
            suffixIndex++;
        }

        return $"{size:N1} {suffixes[suffixIndex]}";
    }
}


