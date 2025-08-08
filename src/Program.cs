using System.IO;
using System;
namespace sitebuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            var contentDir = Path.Combine("pages");
            var templatePath = Path.Combine("templates", "template.html");
            var outputDir = Path.Combine("output");

            Console.WriteLine("Building static site...");

            if (!File.Exists(templatePath))
            {
                Console.WriteLine(templatePath);
                Console.WriteLine($"base directory: {AppDomain.CurrentDomain.BaseDirectory}");
                Console.WriteLine($"current working directory: {Directory.GetCurrentDirectory()}");
                Console.WriteLine("Missing template file.");
                return;
            }
            var template = File.ReadAllText(templatePath);

            // Clean output folder
            if (Directory.Exists(outputDir))
                Directory.Delete(outputDir, recursive: true);
            Directory.CreateDirectory(outputDir);

            // Copy assets
            var assetSource = "assets";
            var assetTarget = Path.Combine(outputDir, "assets");
            if (Directory.Exists(assetSource))
                CopyDirectory(assetSource, assetTarget);

            // Build each HTML file
            foreach (var file in Directory.GetFiles(contentDir, "*.html"))
            {
                var filename = Path.GetFileName(file);
                var content = File.ReadAllText(file);
                var fullPage = template.Replace("{{content}}", content);
                File.WriteAllText(Path.Combine(outputDir, filename), fullPage);
                Console.WriteLine($"Built: {filename}");
            }

            Console.WriteLine("Build complete.");
            return;
        }
        static void CopyDirectory(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var targetFile = Path.Combine(targetDir, Path.GetFileName(file));
                File.Copy(file, targetFile);
            }
            foreach (var directtory in Directory.GetDirectories(sourceDir))
            {
                CopyDirectory(directtory, targetDir);
            }
        }
    }
}
