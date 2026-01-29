using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

public static class MySqlBackupHelper
{
    private static string _lastBackupFile = "";

    /// <summary>
    /// Creates a backup of the MySQL database
    /// </summary>
    /// <param name="showSuccessMessage">Whether to show a success message dialog</param>
    /// <returns>True if backup succeeded, false otherwise</returns>
    public static bool CreateBackup(bool showSuccessMessage = false)
    {
        try
        {
            // Read from App.config
            string connStr = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            // Parse connection string
            var builder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(connStr);
            string dbUser = builder.UserID;
            string dbPassword = builder.Password;
            string dbName = builder.Database;
            uint dbPort = builder.Port;

            // Create backup folder if it doesn't exist
            string backupFolder = @"D:\FertilizerSOP\SQLBackups";
            if (!Directory.Exists(backupFolder))
                Directory.CreateDirectory(backupFolder);

            // Generate filename with timestamp
            string fileName = $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql";
            string filePath = Path.Combine(backupFolder, fileName);

            // Path to mysqldump executable
            string mysqldumpPath = @"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysqldump.exe";

            // Check if mysqldump exists
            if (!File.Exists(mysqldumpPath))
            {
                System.Windows.Forms.MessageBox.Show(
                    $"MySQL backup tool not found at:\n{mysqldumpPath}\n\n" +
                    "Please ensure MySQL is installed correctly.",
                    "Backup Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }

            // Build mysqldump arguments
            string arguments = $"-u {dbUser} -p{dbPassword} -P {dbPort} --routines --triggers --databases {dbName}";

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = mysqldumpPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                // Check for errors
                if (!string.IsNullOrWhiteSpace(error) && !error.Contains("Warning"))
                {
                    File.AppendAllText(
                        Path.Combine(backupFolder, "backup_error.log"),
                        $"[{DateTime.Now}] {error}\n"
                    );

                    System.Windows.Forms. MessageBox.Show(
                        $"Backup encountered errors:\n{error}\n\nCheck backup_error.log for details.",
                        "Backup Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return false;
                }

                // Save backup file
                if (!string.IsNullOrWhiteSpace(output))
                {
                    File.WriteAllText(filePath, output);
                    _lastBackupFile = filePath;

                    Debug.WriteLine($"✅ Backup created successfully: {filePath}");

                    if (showSuccessMessage)
                    {
                        // Calculate file size
                        FileInfo fi = new FileInfo(filePath);
                        double fileSizeMB = fi.Length / (1024.0 * 1024.0);

                        System.Windows.Forms.MessageBox.Show(
                            $"✅ Database backup created successfully!\n\n" +
                            $"📁 Location: {filePath}\n" +
                            $"📊 Size: {fileSizeMB:F2} MB\n" +
                            $"⏰ Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                            "Backup Successful",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }

                    return true;
                }
                else
                {
                    Debug.WriteLine("❌ No output from mysqldump. Backup failed.");

                    if (showSuccessMessage)
                    {
                        System.Windows.Forms.MessageBox.Show(
                            "Backup failed: No data received from database.\n\n" +
                            "Please check your database connection.",
                            "Backup Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }

                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Backup failed: {ex.Message}");

            System.Windows.Forms.MessageBox.Show(
                $"Backup failed with error:\n\n{ex.Message}",
                "Backup Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );

            return false;
        }
    }

    /// <summary>
    /// Shows a confirmation dialog and creates backup if user confirms
    /// </summary>
    /// <returns>DialogResult indicating user's choice (Yes/No/Cancel)</returns>
    public static DialogResult CreateBackupWithConfirmation()
    {
        var result = System.Windows.Forms.MessageBox.Show(
            "💾 Would you like to backup your database before closing?\n\n" +
            "This will save all your current data to a backup file.\n" +
            "Recommended: Yes\n\n" +
            "⚠️ Click Cancel to return to the application.",
            "Database Backup",
            System.Windows.Forms.MessageBoxButtons.YesNoCancel,
            System.Windows.Forms.MessageBoxIcon.Question,
            System.Windows.Forms.MessageBoxDefaultButton.Button1
        );

        if (result == DialogResult.Yes)
        {
            bool success = CreateBackup(true);
            return success ? DialogResult.Yes : DialogResult.No;
        }

        return result;
    }

    /// <summary>
    /// Gets information about the last backup that was created
    /// </summary>
    public static string GetLastBackupInfo()
    {
        if (string.IsNullOrEmpty(_lastBackupFile))
            return "No backup created in this session.";

        if (!File.Exists(_lastBackupFile))
            return $"Last backup file not found: {_lastBackupFile}";

        FileInfo fi = new FileInfo(_lastBackupFile);
        return $"Last backup: {fi.Name} ({fi.Length / (1024.0 * 1024.0):F2} MB) at {fi.CreationTime:yyyy-MM-dd HH:mm:ss}";
    }

    /// <summary>
    /// Cleans up old backup files, keeping only the specified number of recent backups
    /// </summary>
    /// <param name="keepCount">Number of recent backups to keep</param>
    public static void CleanupOldBackups(int keepCount = 10)
    {
        try
        {
            string backupFolder = @"D:\FertilizerSOP\SQLBackups";
            if (!Directory.Exists(backupFolder))
                return;

            var backupFiles = Directory.GetFiles(backupFolder, "backup_*.sql")
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.CreationTime)
                .ToList();

            // Keep only the specified number of backups
            var filesToDelete = backupFiles.Skip(keepCount).ToList();

            foreach (var file in filesToDelete)
            {
                try
                {
                    file.Delete();
                    Debug.WriteLine($"🗑️ Deleted old backup: {file.Name}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"⚠️ Failed to delete backup {file.Name}: {ex.Message}");
                }
            }

            if (filesToDelete.Count > 0)
            {
                Debug.WriteLine($"✅ Cleaned up {filesToDelete.Count} old backup file(s)");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"⚠️ Error cleaning up old backups: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets the total size of all backup files
    /// </summary>
    public static string GetTotalBackupSize()
    {
        try
        {
            string backupFolder = @"D:\FertilizerSOP\SQLBackups";
            if (!Directory.Exists(backupFolder))
                return "0 MB";

            var totalBytes = Directory.GetFiles(backupFolder, "backup_*.sql")
                .Sum(f => new FileInfo(f).Length);

            double totalMB = totalBytes / (1024.0 * 1024.0);

            if (totalMB > 1024)
                return $"{totalMB / 1024.0:F2} GB";
            else
                return $"{totalMB:F2} MB";
        }
        catch
        {
            return "Unknown";
        }
    }
}